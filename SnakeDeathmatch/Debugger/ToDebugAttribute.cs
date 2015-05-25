using System;

namespace SnakeDeathmatch.Debugger
{
    public class ToDebugAttribute : Attribute
    {
        public Type VisualizerType { get; private set; }

        public ToDebugAttribute() : this(null)
        {
        }

        public ToDebugAttribute(Type visualizerType)
        {
            VisualizerType = visualizerType;
        }
    }
}
