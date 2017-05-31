using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Jardos.Components.Strategies;
using SnakeDeathmatch.Players.Jardos.Debug;

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

            //_strategies.Add(new SurviveStrategy());
            _strategies.Add(new RecurseStrategy());
            _strategies.Add(new RoterrorStrategy());
            //_strategies.Add(new BehindMeStrategy());
            _strategies.Add(new BullingStrategy());
            _stepsToChange = 0;
        }

        public Move GetMove(int[,] newBattleground, Point lastPoint, Direction lastDirection, Move lastMove)
        {
            _fact.Update(newBattleground, lastPoint, lastDirection, lastMove);
            _strategies.ForEach(s=>s.Update(_fact));

            if (_stepsToChange == 0 || _choosenStrategy == null || !_choosenStrategy.CanContinue())
            {
                _choosenStrategy = _strategies.OrderByDescending(s => s.EvaluateScore()).First();
                _stepsToChange = _choosenStrategy.Alocation;
                if (Breakpoint != null)
                    Breakpoint(this, new BreakpointEventArgs(JardosBreakpointNames.Jardos_StrategyChanged));
            }

            _stepsToChange--;
            return _choosenStrategy.GetMove();
        }

        [ToDebug]
        public Fact Fact {
            get { return _fact; }
        }

        [ToDebug]
        public IStrategy Choosen
        {
            get { return _choosenStrategy; }
        }

        [ToDebug]
        public string ChoosenName
        {
            get { return _choosenStrategy?.GetType().Name; }
        }

        public event BreakpointEventHandler Breakpoint;
    }
}
