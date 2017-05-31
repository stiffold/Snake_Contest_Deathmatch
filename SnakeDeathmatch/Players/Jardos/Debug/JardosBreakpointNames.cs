using SnakeDeathmatch.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardos.Debug
{
    class JardosBreakpointNames :  IBreakpointNames
    {
        public const string Jardos_FactUpgradeBattleGround = "Jardos_FactUpgradeBattleGround";
        public const string Jardos_SurviveStrategyChangedScope = "Jardos_SurviveStrategyChangedScope";
        public const string Jardos_StrategyChanged = "Jardos_StrategyChanged";

        public IEnumerable<string> GetNames()
        {
            yield return Jardos_FactUpgradeBattleGround;
            yield return Jardos_SurviveStrategyChangedScope;
            yield return Jardos_StrategyChanged;
        }
    }
}
