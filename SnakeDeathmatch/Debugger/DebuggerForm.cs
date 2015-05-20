using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Vazba;
using SnakeDeathmatch.Players.Vazba.Debug;
using SnakeDeathmatch.Game;
using System.Threading;

namespace SnakeDeathmatch.Debugger
{
    public partial class DebuggerForm : Form
    {
        private object _rootObj;
        private DebugNode _rootDebugNode;

        private List<string> _affectedPaths = new List<string>();
        private Dictionary<string, DebugNode> _debugNodes = new Dictionary<string, DebugNode>();
        private string _nextBreakpointName = GameEngineBreakpointNames.MoveBegin;
        private bool _shouldContinue = true;

        public DebuggerForm(object rootObjToDebug)
        {
            InitializeComponent();
            _treeView.TreeViewNodeSorter = new TreeNodeSorter();

            _rootObj = rootObjToDebug;
        }

        private void DebuggerForm_Load(object sender, EventArgs e)
        {
            UpdateBreakpoints();
            if (_rootObj is IDebuggable)
                (_rootObj as IDebuggable).Breakpoint += DebuggerForm_Breakpoint;
        }

        public void UpdateUI()
        {
            var oldRootDebugNode = _rootDebugNode;
            var newRootDebugNode = new DebugNode(null, _rootObj.GetType().Name, _rootObj, _rootObj.GetType(), null);
            _rootDebugNode = newRootDebugNode;

            UpdateHandlers(oldRootDebugNode, newRootDebugNode);
            UpdateTreeView(oldRootDebugNode, newRootDebugNode);
            UpdateVisualizers(oldRootDebugNode, newRootDebugNode);
        }

        private void UpdateHandlers(DebugNode oldRootDebugNode, DebugNode newRootDebugNode)
        {
            if (oldRootDebugNode != null)
            {
                foreach (IDebuggable obj in oldRootDebugNode.GetAllNodesRecursively().Select(x => x.Obj).OfType<IDebuggable>())
                {
                    obj.Breakpoint -= DebuggerForm_Breakpoint;
                }
            }

            foreach (IDebuggable obj in newRootDebugNode.GetAllNodesRecursively().Select(x => x.Obj).OfType<IDebuggable>())
            {
                obj.Breakpoint += DebuggerForm_Breakpoint;
            }
        }

        private void UpdateTreeView(DebugNode oldRootDebugNode, DebugNode newRootDebugNode)
        {
            _treeView.BeginUpdate();
            try
            {
                IEnumerable<string> oldPaths = (oldRootDebugNode != null) ? oldRootDebugNode.GetAllNodesRecursively().Select(x => x.Path) : Enumerable.Empty<string>();
                IEnumerable<string> newPaths = newRootDebugNode.GetAllNodesRecursively().Select(x => x.Path);

                IEnumerable<string> pathsToDelete = oldPaths.Except(newPaths).OrderByDescending(x => x);
                foreach (string path in pathsToDelete)
                {
                    TreeNode treeNode = _treeView.Nodes.GetNodeByTagObjectRecursively(path);
                    DebugNode debugNode = oldRootDebugNode.GetAllNodesRecursively().First(x => x.Path == path);
                    treeNode.Remove();
                }

                IEnumerable<string> pathsToAdd = newPaths.Except(oldPaths);
                foreach (string path in pathsToAdd)
                {
                    DebugNode debugNode = newRootDebugNode.GetAllNodesRecursively().First(x => x.Path == path);

                    if (debugNode.Parent != null)
                    {
                        var treeNode = _treeView.Nodes.GetNodeByTagObjectRecursively(debugNode.Parent.Path);
                        treeNode.Nodes.Add(new TreeNode(GetTreeNodeText(debugNode)) { Tag = debugNode.Path });
                        treeNode.Expand();
                    }
                    else
                        _treeView.Nodes.Add(new TreeNode(GetTreeNodeText(debugNode)) { Tag = debugNode.Path });
                }

                IEnumerable<string> pathsToUpdate = oldPaths.Intersect(newPaths);
                foreach (string path in pathsToUpdate)
                {
                    TreeNode treeNode = _treeView.Nodes.GetNodeByTagObjectRecursively(path);
                    DebugNode debugNode = oldRootDebugNode.GetAllNodesRecursively().First(x => x.Path == path);
                    treeNode.Text = GetTreeNodeText(debugNode);
                }
            }
            finally
            {
                _treeView.EndUpdate();
            }
        }

        private string GetTreeNodeText(DebugNode debugNode)
        {
            return (debugNode.Obj == null)
                ? string.Format("{0}: null", debugNode.Name)
                : (debugNode.Obj.ToString() != debugNode.Obj.GetType().ToString())
                    ? string.Format("{0}: {1}", debugNode.Name, debugNode.Obj)
                    : debugNode.Name;
        }

        private void UpdateVisualizers(DebugNode oldRootDebugNode, DebugNode newRootDebugNode)
        {
            IEnumerable<string> oldPaths = (oldRootDebugNode != null) ? oldRootDebugNode.GetAllNodesRecursively().Select(x => x.Path) : Enumerable.Empty<string>();
            IEnumerable<string> newPaths = newRootDebugNode.GetAllNodesRecursively().Select(x => x.Path);

            IEnumerable<string> pathsToDelete = oldPaths.Except(newPaths);
            foreach (string path in pathsToDelete)
            {
                Control control = _panelControls.Controls.Cast<Control>().FirstOrDefault(x => x.Tag.Equals(path));
                if (control != null)
                    _panelControls.Controls.Remove(control);
            }

            IEnumerable<string> pathsToAdd = newPaths.Except(oldPaths);
            foreach (string path in pathsToAdd)
            {
                DebugNode debugNode = newRootDebugNode.GetAllNodesRecursively().First(x => x.Path == path);

                IVisualizer visualizer = null;
                if (debugNode.VisualizerType != null && !typeof(IEnumerable).IsAssignableFrom(debugNode.ObjType))
                    visualizer = (IVisualizer)Activator.CreateInstance(debugNode.VisualizerType);

                if (visualizer != null)
                {
                    visualizer.Update(debugNode.Obj);

                    Point nextLocation = new Point(0, 0);

                    Control lastControl = _panelControls.Controls.Cast<Control>().LastOrDefault();
                    if (lastControl != null)
                    {
                        nextLocation = new Point(lastControl.Location.X + lastControl.Size.Width, lastControl.Location.Y);
                        if (nextLocation.X >= 700)
                            nextLocation = new Point(0, lastControl.Location.Y + lastControl.Size.Height);
                    }

                    var control = visualizer as Control;
                    control.Location = new Point(2, 2);

                    var wrapperControl = new Panel();
                    wrapperControl.Tag = path;
                    wrapperControl.Location = nextLocation;
                    wrapperControl.Size = new Size(control.Size.Width + 4, control.Size.Height + 4);
                    wrapperControl.Controls.Add(control);

                    _panelControls.Controls.Add(wrapperControl);
                }
            }

            IEnumerable<string> pathsToUpdate = oldPaths.Intersect(newPaths);
            foreach (string path in pathsToUpdate)
            {
                Control wrapperControl = _panelControls.Controls.Cast<Control>().FirstOrDefault(x => x.Tag != null && x.Tag.Equals(path));
                if (wrapperControl != null)
                {
                    IVisualizer visualizer = (wrapperControl.Controls[0] as IVisualizer);
                    if (visualizer != null)
                    {
                        DebugNode debugNode = newRootDebugNode.GetAllNodesRecursively().First(x => x.Path == path);
                        visualizer.Update(debugNode.Obj);
                    }
                }
            }
        }

        private void UpdateBreakpoints()
        {
            string currentBreakpoint = _comboBoxBreakpoint.Text;
            _comboBoxBreakpoint.BeginUpdate();
            try
            {
                _comboBoxBreakpoint.Items.Clear();

                IEnumerable<Type> breakpointNamesTypes = _rootObj.GetType().Assembly.GetTypes().Where(x => typeof(IBreakpointNames).IsAssignableFrom(x) && x.IsClass);
                foreach (Type breakpointNamesType in breakpointNamesTypes)
                {
                    IBreakpointNames instance = (IBreakpointNames)Activator.CreateInstance(breakpointNamesType);
                    _comboBoxBreakpoint.Items.AddRange(instance.GetNames().ToArray());
                }
                _comboBoxBreakpoint.SelectedItem = GameEngineBreakpointNames.MoveBegin;
            }
            finally
            {
                _comboBoxBreakpoint.EndUpdate();
            }
        }

        private delegate void UpdateUIDelegate();

        private void DebuggerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_rootObj is IDebuggable)
                (_rootObj as IDebuggable).Breakpoint -= DebuggerForm_Breakpoint;

            foreach (IDebuggable obj in _rootDebugNode.GetAllNodesRecursively().Select(x => x.Obj).OfType<IDebuggable>())
            {
                obj.Breakpoint -= DebuggerForm_Breakpoint;
            }
        }

        private void DebuggerForm_Breakpoint(object sender, BreakpointEventArgs e)
        {
            string nextBreakpointName;
            lock (this)
            {
                nextBreakpointName = _nextBreakpointName;
            }

            if (e.BreakpointName == nextBreakpointName && nextBreakpointName == GameEngineBreakpointNames.MoveBegin_Running)
            {
                Invoke(new UpdateUIDelegate(UpdateUI), null);
                Thread.Sleep(300);
            }
            else if (e.BreakpointName == nextBreakpointName)
            {
                Invoke(new UpdateUIDelegate(UpdateUI), null);

                lock (this)
                {
                    _shouldContinue = false;
                }
                bool shouldContinue = false;

                while (!shouldContinue)
                {
                    lock (this)
                    {
                        shouldContinue = _shouldContinue;
                    }
                    Thread.Sleep(1);
                }
            }
        }

        private void _comboBoxBreakpoint_SelectedValueChanged(object sender, EventArgs e)
        {
            lock(this)
            {
                _nextBreakpointName = _comboBoxBreakpoint.Text;
            }
        }

        private void _buttonGoToTheEnd_Click(object sender, EventArgs e)
        {
            lock (this)
            {
                _shouldContinue = true;
            }
        }

        private void _treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string path = (string)e.Node.Tag;

            foreach(Control control in _panelControls.Controls.Cast<Control>())
            {
                control.BackColor = (control.Tag.Equals(path)) ? Color.Red : _panelControls.BackColor;
            }
        }
    }

    public static class TreeNodeCollectionExt
    {
        public static TreeNode GetNodeByTagObjectRecursively(this TreeNodeCollection treeNodeCollection, object tagObject)
        {
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                if (treeNode.Tag.Equals(tagObject))
                    return treeNode;

                var foundTreeNode = GetNodeByTagObjectRecursively(treeNode.Nodes, tagObject);
                if (foundTreeNode != null)
                    return foundTreeNode;
            }
            return null;
        }
    }

    public class TreeNodeSorter : IComparer
    {
        public int Compare(object treeNode1, object treeNode2)
        {
            return string.Compare((treeNode1 as TreeNode).Text, (treeNode2 as TreeNode).Text);
        }
    }
}
