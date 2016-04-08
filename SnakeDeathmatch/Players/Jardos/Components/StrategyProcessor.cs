using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Jardos.Components.Strategies;

namespace SnakeDeathmatch.Players.Jardos.Components
{
    /// <summary>
    /// měla by se starat o výběr správné strategiea měnění strategie
    /// </summary>
    public class StrategyProcessor : IDebuggable
    {
        private List<IStrategy> _strategies;
        private readonly Fact _fact;
        private IStrategy _choosenStrategy;
        private int _stepsToChange;

        public StrategyProcessor()
        {
            _strategies = new List<IStrategy>();
            _fact = new Fact();
            RecurseStrategy recurseStrategy = new RecurseStrategy();
            _strategies.Add(recurseStrategy);
            _stepsToChange = 0;
        }

        public Move GetMove(int[,] newBattleground, Point lastPoint, Direction lastDirection)
        {
            _fact.Update(newBattleground, lastPoint, lastDirection);
            _strategies.ForEach(s=>s.Update(_fact));

            if (_stepsToChange == 0 || _choosenStrategy == null || !_choosenStrategy.CanContinue())
            {
                _choosenStrategy = _strategies.OrderByDescending(s => s.EvaluateScore()).First();
                _stepsToChange = _choosenStrategy.Alocation;
            }

            _stepsToChange--;
            return _choosenStrategy.GetMove();
        }

        [ToDebug]
        public Fact Fact {
            get { return _fact; }
        }

        public event BreakpointEventHandler Breakpoint;
    }
}
