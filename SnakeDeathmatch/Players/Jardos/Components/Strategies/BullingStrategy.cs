using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Jardos.Debug;

namespace SnakeDeathmatch.Players.Jardos.Components.Strategies
{
    public class BullingStrategy : IStrategy
    {
        private Fact _fact;
        private BuillingVariant _variant;
        private RecurseStrategy _rec = new RecurseStrategy(true,false);
        private int[,] _wrappedBattleGround;

        public event BreakpointEventHandler Breakpoint;
        public int EvaluateScore()
        {
            _wrappedBattleGround = (int[,])_fact.ActualBattleGround.Clone();
            var variants = new List<BuillingVariant>();
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                BuillingVariant variant = new BuillingVariant();
                Point p = _fact.LastPoint;
                p.Update(dir, 23);
                variant.Direction = dir;
                variant.Points = this.CirclePoints(p, 25,_wrappedBattleGround).ToList();
                variants.Add(variant);
            }

            _variant = variants.OrderByDescending(x => x.Points.Count).First();
            for (int x = 0; x < ComputeHelper.MaxSize; x++)
            {
                for (int y = 0; y < ComputeHelper.MaxSize; y++)
                {
                    if (_wrappedBattleGround[x, y] == 0)
                    {
                        _wrappedBattleGround[x, y] = 1000;
                    }
                }
            }

            foreach (var p in _variant.Points)
            {
                _wrappedBattleGround[p.X, p.Y] = 0;
            }
            //foreach (var s in _fact.Snakes)
            //{
            //    Masker.Circle(_wrappedBattleGround, s.HeadPoint, 25, 1000);
            //}

            var arrroundPoints = _fact.LastPoint.GetNeighbours();
            if (arrroundPoints.All(x=>x.IsValid(_wrappedBattleGround)))
            {
                return 1001;
            }
            return 0;
        }

        

        public int Alocation {
            get { return _variant.Points.Count; }
        }

        public void Update(Fact fact)
        {
            _fact = fact;
            _rec.Update(fact);
            if (_wrappedBattleGround == null)
            {
                _wrappedBattleGround = (int[,])_fact.ActualBattleGround.Clone();
            }
            ComputeHelper.MergeArrays(_fact.LastUnmaskedBattleGround, _wrappedBattleGround);
            _rec.Update(_wrappedBattleGround);
        }

        public bool CanContinue()
        {
            foreach (var s in _fact.Snakes.Where(x=>x.Live))
            {
                if (_variant.Points.Any(x=>x.Equals(s.HeadPoint)))
                {
                    return false;
                }
            }
            return ArroundMyHeadIsOnlyZerosAndMask();
        }

        private bool ArroundMyHeadIsOnlyZerosAndMask()
        {
            foreach (var p in _fact.LastPoint.GetNeighbours())
            {
                int value = _wrappedBattleGround[p.X, p.Y];
                if (value != 0 && value != 1000 && value != ComputeHelper.MyId)
                {
                    return false;
                }
            }
            return true;
        }

        public Move GetMove()
        {
             return _rec.GetMove();
        }

        private IEnumerable<Point> CirclePoints(Point p, int radius, int[,] b)
        {
            for (int row = 0; row < ComputeHelper.MaxSize; row++)
            {
                for (int col = 0; col < ComputeHelper.MaxSize; col++)
                {
                    double d = Math.Sqrt((p.X - row) * (p.X - row) + (p.Y - col) * (p.Y - col));
                    if (d <= radius && b[row, col] == 0)
                    {
                        yield return new Point(row, col);
                    }
                }
            }
        }

        [ToDebug(typeof(DebugVizualizer))]
        public DebugablePlayground Choosen { get { return new DebugablePlayground(_wrappedBattleGround, "battleGround"); } set { } }

        class BuillingVariant
        {
            public Direction Direction { get; set; }
            public List<Point> Points { get; set; }
        }
    }
}
