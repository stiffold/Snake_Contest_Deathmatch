using System;

namespace SnakeDeathmatch.Debugger
{
    public class ToDebugAttribute : Attribute
    {
        public Type VisualizerType { get; private set; }

        public ToDebugAttribute(Type visualizerType = null)
        {
            VisualizerType = visualizerType;
        }
    }
}
