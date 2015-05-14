using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Vazba;
using SnakeDeathmatch.Players.Vazba.Debug;

namespace SnakeDeathmatch.Debugger
{
    public partial class DebuggerForm : Form
    {
        private object _rootObj;
        private DebugNode _rootDebugNode;

        private List<string> _affectedPaths = new List<string>();
        private Dictionary<string, DebugNode> _debugNodes = new Dictionary<string, DebugNode>();
        private string _nextDebugId = BreakpointNames.MoveEnd;

        public DebuggerForm(object rootObjToDebug)
        {
            InitializeComponent();
            _treeView.TreeViewNodeSorter = new TreeNodeSorter();

            _rootObj = rootObjToDebug;
            DoUpdate();
        }

        public void DoUpdate()
        {
            var oldRootDebugNode = _rootDebugNode;
            var newRootDebugNode = new DebugNode(null, _rootObj.ToString(), _rootObj, _rootObj.GetType());
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
                    obj.Breakpoint -= DebuggerForm_Debug;
                }
            }

            foreach (IDebuggable obj in newRootDebugNode.GetAllNodesRecursively().Select(x => x.Obj).OfType<IDebuggable>())
            {
                obj.Breakpoint += DebuggerForm_Debug;
            }
        }

        private void UpdateTreeView(DebugNode oldRootDebugNode, DebugNode newRootDebugNode)
        {
            _treeView.BeginUpdate();
            try
            {
                IEnumerable<string> oldPaths = (oldRootDebugNode != null) ? oldRootDebugNode.GetAllNodesRecursively().Select(x => x.Path) : Enumerable.Empty<string>();
                IEnumerable<string> newPaths = newRootDebugNode.GetAllNodesRecursively().Select(x => x.Path);

                IEnumerable<string> pathsToDelete = oldPaths.Except(newPaths);
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
                ? string.Format("{0} = null", debugNode.Name)
                : (debugNode.Obj.ToString() != debugNode.Obj.GetType().ToString())
                    ? string.Format("{0} = {1}", debugNode.Name, debugNode.Obj)
                    : debugNode.Name;
        }

        private void UpdateVisualizers(DebugNode oldRootDebugNode, DebugNode newRootDebugNode)
        {
            IEnumerable<string> oldPaths = (oldRootDebugNode != null) ? oldRootDebugNode.GetAllNodesRecursively().Select(x => x.Path) : Enumerable.Empty<string>();
            IEnumerable<string> newPaths = newRootDebugNode.GetAllNodesRecursively().Select(x => x.Path);

            IEnumerable<string> pathsToDelete = oldPaths.Except(newPaths);
            foreach (string path in pathsToDelete)
            {
                Control control = _panelBody.Controls.Cast<Control>().FirstOrDefault(x => x.Tag.Equals(path));
                if (control != null)
                    _panelBody.Controls.Remove(control);
            }

            IEnumerable<string> pathsToAdd = newPaths.Except(oldPaths);
            foreach (string path in pathsToAdd)
            {
                DebugNode debugNode = newRootDebugNode.GetAllNodesRecursively().First(x => x.Path == path);

                IVisualizer visualizer = CreateVisualizerForType(debugNode.ObjType);
                if (visualizer != null)
                {
                    visualizer.Update(debugNode.Obj);

                    Point nextLocation = new Point(0, 0);

                    Control lastControl = _panelBody.Controls.Cast<Control>().LastOrDefault();
                    if (lastControl != null)
                    {
                        nextLocation = new Point(lastControl.Location.X + lastControl.Size.Width + 4, lastControl.Location.Y);
                        if (nextLocation.X >= 700)
                            nextLocation = new Point(0, lastControl.Location.Y + lastControl.Size.Height + 4);
                    }

                    var control = visualizer as Control;
                    control.Tag = path;
                    control.Location = nextLocation;
                    _panelBody.Controls.Add(control);
                }
            }

            IEnumerable<string> pathsToUpdate = oldPaths.Intersect(newPaths);
            foreach (string path in pathsToUpdate)
            {
                Control control = _panelBody.Controls.Cast<Control>().FirstOrDefault(x => x.Tag != null && x.Tag.Equals(path));
                if (control != null)
                {
                    IVisualizer visualizer = (control as IVisualizer);
                    if (visualizer != null)
                    {
                        DebugNode debugNode = newRootDebugNode.GetAllNodesRecursively().First(x => x.Path == path);
                        visualizer.Update(debugNode.Obj);
                    }
                }
            }
        }

        private IVisualizer CreateVisualizerForType(Type objType)
        {
            IVisualizer objectVisualizer = null;
            if (objType == typeof(IntPlayground))
            {
                objectVisualizer = new IntPlaygroundVisualizer();  // TODO: Vytvoř typ uvedený v atributu [ObjectVisualizerAttribute].
            }
            else if (objType == typeof(DecimalPlayground))
            {
                objectVisualizer = new DecimalPlaygroundVisualizer();  // TODO: Vytvoř typ uvedený v atributu [ObjectVisualizerAttribute].
            }
            return objectVisualizer;
        }

        private void DebuggerForm_Debug(object sender, BreakpointEventArgs e)
        {
            if (e.BreakpointName == _nextDebugId)
            {
                DoUpdate();
                //MessageBox.Show(string.Format("Pause ({0})", e.DebugId), "Debugger");
            }
        }

        private void DebuggerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (IDebuggable obj in _rootDebugNode.GetAllNodesRecursively().Select(x => x.Obj).OfType<IDebuggable>())
            {
                obj.Breakpoint -= DebuggerForm_Debug;
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

    public class DebugNode
    {
        public DebugNode Parent { get; private set; }
        public string Name { get; private set; }
        public object Obj { get; set; }
        public Type ObjType { get; set; }
        public List<DebugNode> Children { get; private set; }
        public bool CanCreateVisualizer
        {
            get
            {
                if (Obj is IntPlayground)
                    return true;

                return false;
            }
        }
        public string Path
        {
            get { return string.Format("{0}/{1}", (Parent != null) ? Parent.Path : string.Empty, Name); }
        }

        public DebugNode(DebugNode parent, string name, object obj, Type objType)
        {
            Parent = parent;
            Name = name;
            Obj = obj;
            ObjType = objType;
            Children = new List<DebugNode>();
            CreateChildren();
        }

        public IEnumerable<DebugNode> GetAllNodesRecursively()
        {
            yield return this;
            foreach (var childNode in Children)
            {
                foreach (var childNodeFromRecursion in childNode.GetAllNodesRecursively())
                {
                    yield return childNodeFromRecursion;
                }
            }
        }

        private void CreateChildren()
        {
            if (Obj == null)
                return;

            if (typeof(IEnumerable).IsAssignableFrom(Obj.GetType()))
            {
                IEnumerator enumerator = (Obj as IEnumerable).GetEnumerator();
                int i = 0;
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    var name = string.Format("[{0}]", i);
                    var debugNode = new DebugNode(this, name, obj, obj.GetType());
                    Children.Add(debugNode);
                    i++;
                }
            }
            else
            {
                var properties = Obj.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(ToDebugAttribute), true).Any());
                foreach (var property in properties)
                {
                    object obj = property.GetValue(Obj, null);
                    var debugNode = new DebugNode(this, property.Name, obj, property.PropertyType);
                    Children.Add(debugNode);
                }
            }
        }
    }
}
