using SnakeDeathmatch.Players.Vazba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Debugger
{
    public class DebugNode
    {
        public DebugNode Parent { get; private set; }
        public string Name { get; private set; }
        public object Obj { get; set; }
        public Type ObjType { get; set; }
        public List<DebugNode> Children { get; private set; }
        public Type VisualizerType { get; private set; }
        public bool IsVisualizerVisible { get; set; }
        public string Path
        {
            get { return string.Format("{0}/{1}", (Parent != null) ? Parent.Path : string.Empty, Name); }
        }

        public DebugNode(DebugNode parent, string name, object obj, Type objType, Type visualizerType)
        {
            Parent = parent;
            Name = name;
            Obj = obj;
            ObjType = objType;
            VisualizerType = visualizerType;
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

        public DebugNode GetNodeByPath(string path)
        {
            return GetAllNodesRecursively().Where(x => x.Path == path).FirstOrDefault();
        }

        public bool CanHaveVisualizer
        {
            get
            {
                return (VisualizerType != null && !typeof(IEnumerable).IsAssignableFrom(ObjType));
            }
        }

        private void CreateChildren()
        {
            if (Obj == null)
                return;

            if (typeof(IEnumerable).IsAssignableFrom(Obj.GetType()) && Obj.GetType() != typeof(string))
            {
                IEnumerator enumerator = (Obj as IEnumerable).GetEnumerator();
                int i = 0;
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    var name = string.Format("[{0}]", i);
                    var debugNode = new DebugNode(this, name, obj, obj.GetType(), VisualizerType);
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
                    ToDebugAttribute attribute = property.GetCustomAttributes(true).OfType<ToDebugAttribute>().FirstOrDefault();
                    Type visualizerType = (attribute != null) ? attribute.VisualizerType : null;

                    var debugNode = new DebugNode(this, property.Name, obj, property.PropertyType, visualizerType);
                    Children.Add(debugNode);
                }
            }
        }
    }
}
