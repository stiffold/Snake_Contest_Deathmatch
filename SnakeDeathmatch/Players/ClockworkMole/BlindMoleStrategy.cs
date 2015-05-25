using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.ClockworkMole
{
    public class BlindMoleStrategy
    {

        private const int MaxRecursion = 20;
        private int recursionTotalCounter;

        public Move GetNextMove(Playground playground, Position position)
        {
            recursionTotalCounter = 0;
            playground.CreateTestCollisionArray();

            var result = CaluclateMove(playground, position);
            var bestresult = result.OrderByDescending(x => x.Value).First();

            for (int i = 0; i < playground.HeadTestDistance; i++)
            {
                playground.DecreaseTestCollistionArrayByOne();

                result = CaluclateMove(playground, position);

                if (result.OrderByDescending(x => x.Value).First().Value > bestresult.Value)
                    bestresult = result.OrderByDescending(x => x.Value).First();

            }

            return bestresult.Key;
        }

        private List<KeyValuePair<Move, int>> CaluclateMove(Playground playground, Position position)
        {
            var list = new List<KeyValuePair<Move, int>>();

            list.Add(new KeyValuePair<Move, int>(Move.Left, PossibleMoveCount(playground, position, Move.Left, 0)));
            list.Add(new KeyValuePair<Move, int>(Move.Straight, PossibleMoveCount(playground, position, Move.Straight, 0)));
            list.Add(new KeyValuePair<Move, int>(Move.Right, PossibleMoveCount(playground, position, Move.Right, 0)));

            return list;
        }

        private int PossibleMoveCount(Playground playground, Position position, Move move, int counter)
        {
            recursionTotalCounter++;

            if (counter > MaxRecursion)
                return counter;

            if (CollisionHelper.IsCollision(playground, position, move))
                return counter;

            var newPosition = position.Move(move);
            playground.TestCollissionArray[newPosition.X, newPosition.Y] = 1000; // naplnit necim, aby se jednou projite pole znovu netestovalo
            counter++;

            int leftMoves = PossibleMoveCount(playground, newPosition, Move.Left, counter);
            if (leftMoves > MaxRecursion) return leftMoves;

            int straightMoves = PossibleMoveCount(playground, newPosition, Move.Straight, counter);
            if (straightMoves > MaxRecursion) return straightMoves;

            int rightMoves = PossibleMoveCount(playground, newPosition, Move.Right, counter);
            if (rightMoves > MaxRecursion) return rightMoves;


            playground.TestCollissionArray[newPosition.X, newPosition.Y] = 0; // a tady vratit zpet 

            return Math.Max(Math.Max(leftMoves, straightMoves), rightMoves);

        }
    }
}
