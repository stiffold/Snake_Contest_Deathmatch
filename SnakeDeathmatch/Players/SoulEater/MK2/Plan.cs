using System.Collections.Generic;

namespace SnakeDeathmatch.Players.SoulEater.MK2
{
    public class Plan
    {
        private Queue<Step> _steps;

        public int PlanIdentificator;
    }

    public class Step
    {
        public PathClass PathClass { get; set; }
    }
}
