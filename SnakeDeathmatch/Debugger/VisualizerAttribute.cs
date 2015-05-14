using System;

namespace SnakeDeathmatch.Debugger
{
    public class VisualizerAttribute : Attribute
    {
        public Type VisualizerType { get; private set; }

        public VisualizerAttribute(Type visualizerType)
        {
            VisualizerType = visualizerType;
        }
    }
}
