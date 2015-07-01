using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater.MK2
{
    public class TurnAroundBeforeClash : IStrategyTrigger
    {
        public IList<IStrategy> GetStrategy(GameGroundMK2 gameGround, Snake ourSnake)
        {
            foreach (Snake otherSnake in gameGround.OtherSnakes)
            {
                // protismer
                if (otherSnake.Direction != ourSnake.Direction.GetOpositeDirection())
                    continue;

                // jedou proti sobe
                int? distance = null;
                switch (otherSnake.Direction)
                {
                    case Direction.Top:
                        if (otherSnake.X == ourSnake.X)
                            distance = ourSnake.Y - otherSnake.Y;
                        break;
                    case Direction.TopRight:
                        if (otherSnake.X - ourSnake.X == otherSnake.Y - ourSnake.Y)
                            distance = ourSnake.Y - otherSnake.Y;
                        break;
                    case Direction.Right:
                        if (otherSnake.Y == ourSnake.Y)
                            distance = ourSnake.X - otherSnake.X;
                        break;
                    case Direction.BottomRight:
                        if (otherSnake.X - ourSnake.X == otherSnake.Y - ourSnake.Y)
                            distance = ourSnake.X - otherSnake.X;
                        break;

                    case Direction.Bottom:
                        if (otherSnake.X == ourSnake.X)
                            distance = ourSnake.Y - otherSnake.Y;
                        break;
                    case Direction.BottomLeft:
                        if (otherSnake.X - ourSnake.X == otherSnake.Y - ourSnake.Y)
                            distance = otherSnake.Y - ourSnake.Y;
                        break;
                    case Direction.Left:
                        if (otherSnake.Y == ourSnake.Y)
                            distance = otherSnake.X - ourSnake.X;
                        break;
                    case Direction.TopLeft:
                        if (otherSnake.X - ourSnake.X == otherSnake.Y - ourSnake.Y)
                            distance = otherSnake.X - ourSnake.X;
                        break;
                }
                if (distance == null || distance < 0)
                    continue;

                if (distance > 6)
                    continue;

                // neni mezi nimi prekazka
                PointClass point = otherSnake.Point;
                bool foundWall = false;
                for (int i = 0; i < distance; i++)
                {
                    point = point.GetLinkedPoint(otherSnake.Direction);
                    if (point != ourSnake.Point && point.IsUsed)
                        foundWall = true;
                }

                if (foundWall == true)
                    continue;

                return new List<IStrategy>{new TurnAroundStrategy(true), new TurnAroundStrategy(false)};
            }
            return new List<IStrategy>();
        }
    }

    public interface IStrategyTrigger
    {
        IList<IStrategy> GetStrategy(GameGroundMK2 gameGround, Snake ourSnake);
    }

    public interface IStrategy
    {
        IList<Move> GetPlanList();
    }

    public class TurnAroundStrategy : IStrategy
    {
        private readonly bool _isTurnRight;

        public TurnAroundStrategy(bool isTurnRight)
        {
            _isTurnRight = isTurnRight;
        }

        public IList<Move> GetPlanList()
        {
            if (_isTurnRight)
                return new[] {Move.Right, Move.Right, Move.Right, Move.Right};
            else
                return new[] {Move.Left, Move.Left, Move.Left, Move.Left};
        }
    }
}
