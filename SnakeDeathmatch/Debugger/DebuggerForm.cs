using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Vazba;
using SnakeDeathmatch.Players.Vazba.Debug;
using SnakeDeathmatch.Game;
using System.Threading;
using System.Drawing;

namespace SnakeDeathmatch.Debugger
{
    public partial class DebuggerForm : Form
    {
        private object _rootObj;
        private DebugNode _rootDebugNode;

        private List<string> _affectedPaths = new List<string>();
        private Dictionary<string, DebugNode> _debugNodes = new Dictionary<string, DebugNode>();
        private string _nextBreakpointName;
        private bool _shouldContinue = true;
        private int _updateUISuspended;

        public DebuggerForm(object rootObjToDebug, string defaultBreakpointName = "")
        {
            InitializeComponent();
            _treeView.TreeViewNodeSorter = new TreeNodeSorter();

            _rootObj = rootObjToDebug;
            _nextBreakpointName = defaultBreakpointName;
            UpdateUI();
        }

        private void DebuggerForm_Load(object sender, EventArgs e)
        {
            UpdateBreakpoints();
            if (_rootObj is IDebuggable)
                (_rootObj as IDebuggable).Breakpoint += DebuggerForm_Breakpoint;
        }

        public void UpdateUI()
        {
            if (_updateUISuspended > 0)
                return;

            var oldRootDebugNode = _rootDebugNode;
            var newRootDebugNode = new DebugNode(null, _rootObj.GetType().Name, _rootObj, _rootObj.GetType(), null);
            _rootDebugNode = newRootDebugNode;

            UpdateHandlers(oldRootDebugNode, newRootDebugNode);
            UpdateTreeView(oldRootDebugNode, newRootDebugNode);
            UpdateVisualizerVisibility(newRootDebugNode);
            UpdateVisualizers(oldRootDebugNode, newRootDebugNode);
        }

        public void UpdateUI(object rootObject)
        {
            _rootObj = rootObject;
            UpdateUI();
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

        private void UpdateVisualizerVisibility(DebugNode rootDebugNode)
        {
            foreach (DebugNode debugNode in rootDebugNode.GetAllNodesRecursively())
            {
                TreeNode treeNode = _treeView.Nodes.GetNodeByTagObjectRecursively(debugNode.Path);
                debugNode.IsVisualizerVisible = (treeNode != null && treeNode.Checked);
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

                    TreeNode newTreeNode;
                    if (debugNode.Parent != null)
                    {
                        var treeNode = _treeView.Nodes.GetNodeByTagObjectRecursively(debugNode.Parent.Path);
                        newTreeNode = CreateNewTreeNode(debugNode);
                        treeNode.Nodes.Add(newTreeNode);
                        treeNode.Expand();
                    }
                    else
                    {
                        newTreeNode = CreateNewTreeNode(debugNode);
                        _treeView.Nodes.Add(newTreeNode);
                    }
                    //if (!debugNode.CanHaveVisualizer)
                    //    newTreeNode.HideCheckBox();
                }

                IEnumerable<string> pathsToUpdate = oldPaths.Intersect(newPaths);
                foreach (string path in pathsToUpdate)
                {
                    TreeNode treeNode = _treeView.Nodes.GetNodeByTagObjectRecursively(path);
                    DebugNode debugNode = newRootDebugNode.GetAllNodesRecursively().First(x => x.Path == path);
                    treeNode.Text = GetTreeNodeText(debugNode);
                }
            }
            finally
            {
                _treeView.EndUpdate();
            }
        }

        private TreeNode CreateNewTreeNode(DebugNode debugNode)
        {
            var treeNode = new TreeNode(GetTreeNodeText(debugNode))
            {
                Tag = debugNode.Path,
                ImageIndex = debugNode.CanHaveVisualizer ? 1 : -1,
            };

            return treeNode;
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
                RemoveVisualizer(path);
            }

            IEnumerable<string> pathsToAdd = newPaths.Except(oldPaths);
            foreach (string path in pathsToAdd)
            {
                DebugNode debugNode = newRootDebugNode.GetNodeByPath(path);

                if (debugNode.CanHaveVisualizer && debugNode.IsVisualizerVisible)
                {
                    var visualizer = (IVisualizer)Activator.CreateInstance(debugNode.VisualizerType);
                    visualizer.Update(debugNode.Obj);
                    AddVisualizer(visualizer, debugNode.Path);
                }
            }

            IEnumerable<string> pathsToUpdate = oldPaths.Intersect(newPaths);
            foreach (string path in pathsToUpdate)
            {
                DebugNode debugNode = newRootDebugNode.GetNodeByPath(path);

                Control wrapperControl = _panelControls.Controls.Cast<Control>().FirstOrDefault(x => x.Tag != null && x.Tag.Equals(path));
                if (wrapperControl == null && debugNode.CanHaveVisualizer && debugNode.IsVisualizerVisible)
                {
                    IVisualizer visualizer = (IVisualizer)Activator.CreateInstance(debugNode.VisualizerType);
                    visualizer.Update(debugNode.Obj);
                    AddVisualizer(visualizer, debugNode.Path);
                }
                else if (wrapperControl != null)
                {
                    if (debugNode.CanHaveVisualizer && debugNode.IsVisualizerVisible)
                    {
                        IVisualizer visualizer = (wrapperControl.Controls[0] as IVisualizer);
                        if (visualizer != null)
                            visualizer.Update(debugNode.Obj);
                    }
                    else
                        RemoveVisualizer(path);
                }
            }
        }

        private void AddVisualizer(IVisualizer visualizer, string path)
        {
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

        private void RemoveVisualizer(string path)
        {
            Control control = _panelControls.Controls.Cast<Control>().FirstOrDefault(x => x.Tag.Equals(path));
            if (control != null)
                _panelControls.Controls.Remove(control);

            // TODO: Rearrange visualizers
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
                _comboBoxBreakpoint.SelectedItem = _nextBreakpointName;
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

            if (e.BreakpointName == nextBreakpointName)
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

        private void _treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            _updateUISuspended++;
            foreach (TreeNode treeNode in e.Node.Nodes)
            {
                treeNode.Checked = e.Node.Checked;
            }
            _updateUISuspended--;

            UpdateUI();
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
