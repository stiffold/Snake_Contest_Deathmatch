using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Jardos.Debug;

namespace SnakeDeathmatch.Players.Jardos.Components
{

    /// <summary>
    /// fakta o stavu hry
    /// </summary>
    public class Fact : IDebuggable
    {
        private Masker _masker;

        public Fact()
        {
            Round = 0;
            MaskBattleGround = new int[ComputeHelper.MaxSize, ComputeHelper.MaxSize];
            _masker = new Masker();
        }

        public int Round { get; private set; }
        public Point LastPoint { get; private set; }
        public Direction LastDirection { get; private set; }
        public Move LastMove { get; private set; }
        public int[,] LastUnmaskedBattleGround { get; set; }
        public int[,] ActualBattleGround { get; set; }
        public int[,] MaskBattleGround { get; set; }
        [ToDebug]
        public List<Snake> Snakes { get; set; }

        [ToDebug(typeof(DebugVizualizer))]
        public DebugablePlayground MaskBattleGroundDebug { get; set; }

        public void Update(int[,] newBattleground, Point lastPoint, Direction lastDirection, Move lastMove)
        {
            Round++;
            LastPoint = lastPoint;
            LastDirection = lastDirection;
            LastMove = LastMove;
            UpdateSnakes(newBattleground);
            ActualBattleGround = (int[,])newBattleground.Clone();
            LastUnmaskedBattleGround = (int[,])newBattleground.Clone();
            UpgradeBattleGround();
        }

        /// <summary>
        /// zaktualizuje stavy hadů
        /// </summary>
        /// <param name="newBattleground"></param>
        private void UpdateSnakes(int[,] newBattleground)
        {
            if (Round == 1)
            {
                var snakes = new List<Snake>();
                for (int x = 0; x < ComputeHelper.MaxSize; x++)
                {
                    for (int y = 0; y < ComputeHelper.MaxSize; y++)
                    {
                        if (newBattleground[x, y] != 0 && newBattleground[x, y] != ComputeHelper.MyId)
                        {
                            snakes.Add(new Snake { Id = newBattleground[x, y], HeadPoint = new Point(x, y), Live = true, StartPoint = new Point(x,y), WayPoints = new List<SnakePoint>()});
                        }
                    }
                }
                Snakes = snakes;
            }
            else
            {
                foreach (var s in Snakes.Where(s => s.Live))
                {
                    bool live = false;
                    foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                    {
                        var xp = s.HeadPoint;
                        xp.Update(dir);
                        if (xp.IsValid() && newBattleground[xp.X, xp.Y] != 0 && LastUnmaskedBattleGround[xp.X, xp.Y] == 0)
                        {
                            s.Evaluate(Round, xp, dir, ActualBattleGround);
                            live = true;
                            break;
                        }
                    }
                    s.Live = live;
                }
            }
    }

        /// <summary>
        /// namaskuje hrací plochu
        /// </summary>
        private void UpgradeBattleGround()
        {
            foreach (var s in Snakes.Where(s => s.Live))
            {
                //circle kolem hlav
                var distanceFromMe = s.HeadPoint.Distance(LastPoint);
                if (distanceFromMe > 7)
                {
                    Masker.Circle(MaskBattleGround, s.HeadPoint, 5, 1000);
                }
                else if (distanceFromMe > 5)
                {
                    Masker.Circle(MaskBattleGround, s.HeadPoint, 3, 1000);
                }
                else if (distanceFromMe > 3)
                {
                    Masker.Circle(MaskBattleGround, s.HeadPoint, 2, 1000);
                }

                foreach (var p in s.HeadPoint.GetNeighbours())
                {
                    if (_masker.Mask(p,ActualBattleGround))
                    {
                        MaskBattleGround[p.X, p.Y] = 1000;
                    }
                }
                //if (s.Id != ComputeHelper.MyId)
                //{
                //    foreach (var p in s.WayPoints.Where(x => x.IsFuturePoint == true))
                //    {
                //        if (p.Point.IsValid())
                //        {
                //            //musí zmizet v dalším tahu, proto do actual
                //            ActualBattleGround[p.Point.X, p.Point.Y] = 1001;
                //        }
                //    }
                //}                
            }

            for (int x = 0; x < ComputeHelper.MaxSize; x++)
            {
                for (int y = 0; y < ComputeHelper.MaxSize; y++)
                {
                    ActualBattleGround[x, y] += MaskBattleGround[x, y];
                }
            }

            MaskBattleGroundDebug = new DebugablePlayground((int[,])ActualBattleGround.Clone(), "Mask");

            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(JardosBreakpointNames.Jardos_FactUpgradeBattleGround));
        }

        public event BreakpointEventHandler Breakpoint;
    }
}
