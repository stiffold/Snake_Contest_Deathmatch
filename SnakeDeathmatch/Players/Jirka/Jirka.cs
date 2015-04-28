using System.Collections.Generic;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Jirka
{
    public class Jirka : IPlayerBehavior
    {
        private int identificator;
        private int boardSize;
        private Direction direction;
        private GamePoint actualPosition;
        private bool firstRun;

        private int[,] localBoard;

        public Jirka()
        {
            firstRun = true;
        }

        public void Init(int direction, int identificator)
        {
            this.direction = (Direction)direction;
            this.identificator = identificator;
        }

        public int NextMove(int[,] gameSurrond)
        {
            if (firstRun)
            {
                boardSize = gameSurrond.GetUpperBound(0) + 1;
                actualPosition = FindMySelf(gameSurrond);
                firstRun = false;
            }

            return 2;
        }

        public string MyName()
        {
            return "Jirka";
        }


        private GamePoint FindMySelf(int[,] gameSurrond)
        {
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                {
                    if (gameSurrond[i, j] == identificator)
                    {
                        return new GamePoint(i, j);
                    }
                }
            return new GamePoint(0, 0);
        }
    }


    internal class GamePoint
    {
        public GamePoint()
        {
        }

        public GamePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public bool IsMatch(int x, int y)
        {
            return ((X == x) && (Y == y));
        }

    }

}