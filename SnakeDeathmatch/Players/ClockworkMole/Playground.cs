using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml.Linq;
using SnakeDeathmatch.Game;

namespace SnakeDeathmatch.Players.ClockworkMole
{
    public class Playground
    {

        public int HeadTestDistance = 4;

        public int _playgroundsize;

        public int _playerId;

        public Playground(int playgroundsize, int playerId)
        {
            _playgroundsize = playgroundsize;
            _playerId = playerId;
            PlayerHeads = new Dictionary<int, Position>();
        }

        public int[,] CurrentArray { get; set; }
        public int[,] PreviousArray { get; private set; }

        public int[,] TestCollissionArray { get; set; }

        private Dictionary<int, Position> PlayerHeads { get; set; }

        public void CreateTestCollisionArray()
        {
            TestCollissionArray = (int[,])CurrentArray.Clone();

            int xSize = TestCollissionArray.GetUpperBound(0);
            int ySize = TestCollissionArray.GetUpperBound(1);

            foreach (var playerHead in PlayerHeads)
            {
                int x1 = playerHead.Value.X - HeadTestDistance;
                x1 = x1 >= 0 ? x1 : 0;

                int x2 = playerHead.Value.X + HeadTestDistance;
                x2 = x2 <= xSize ? x2 : xSize;

                int y1 = playerHead.Value.Y - HeadTestDistance;
                y1 = y1 >= 0 ? y1 : 0;

                int y2 = playerHead.Value.Y + HeadTestDistance;
                y2 = y2 <= ySize ? y2 : ySize;

                for (int x = x1; x <= x2; x++)
                {
                    for (int y = y1; y <= y2; y++)
                    {
                        int value = Math.Max(Math.Abs(playerHead.Value.X - x), Math.Abs(playerHead.Value.Y - y));
                        value = HeadTestDistance - value + 1;
                        if (TestCollissionArray[x, y] == 0)
                            TestCollissionArray[x, y] = value + 100;
                    }
                }

            }

        }

        public void DecreaseTestCollistionArrayByOne()
        {
            int xSize = TestCollissionArray.GetUpperBound(0);
            int ySize = TestCollissionArray.GetUpperBound(1);

            foreach (var playerHead in PlayerHeads)
            {
                int x1 = playerHead.Value.X - HeadTestDistance;
                x1 = x1 >= 0 ? x1 : 0;

                int x2 = playerHead.Value.X + HeadTestDistance;
                x2 = x2 <= xSize ? x2 : xSize;

                int y1 = playerHead.Value.Y - HeadTestDistance;
                y1 = y1 >= 0 ? y1 : 0;

                int y2 = playerHead.Value.Y + HeadTestDistance;
                y2 = y2 <= ySize ? y2 : ySize;

                for (int x = x1; x <= x2; x++)
                {
                    for (int y = y1; y <= y2; y++)
                    {
                        if (TestCollissionArray[x, y] > 100 && TestCollissionArray[x, y] < 200)
                        {
                            TestCollissionArray[x, y]--;
                            if (TestCollissionArray[x, y] == 100)
                            {
                                TestCollissionArray[x, y] = 0;
                            }
                        }


                    }
                }

            }
        }

        public void ApplyNextMove(int[,] playgroundArray)
        {
            PreviousArray = CurrentArray;
            CurrentArray = playgroundArray;
            DetectOtherPlayers();

        }

        private void DetectOtherPlayers()
        {
            if (PreviousArray == null)
            {
                InitializeOtherPlayers();
            }
            else
            {
                ActualizeOtherPlayers();
            }
        }

        private void InitializeOtherPlayers()
        {
            for (int x = 0; x < _playgroundsize; x++)
            {
                for (int y = 0; y < _playgroundsize; y++)
                {
                    int arrayValue = CurrentArray[x, y];

                    if (arrayValue != 0 && arrayValue != _playerId)
                    {
                        PlayerHeads.Add(arrayValue, new Position(x, y, 0));
                    }
                }
            }
        }

        private void ActualizeOtherPlayers()
        {
            var deadPlayers = new List<int>();
            int xSize = CurrentArray.GetUpperBound(0);
            int ySize = CurrentArray.GetUpperBound(1);

            foreach (var playerHead in PlayerHeads)
            {
                int x1 = playerHead.Value.X > 0 ? playerHead.Value.X - 1 : 0;
                int x2 = playerHead.Value.X < xSize ? playerHead.Value.X + 1 : xSize;

                int y1 = playerHead.Value.Y > 0 ? playerHead.Value.Y - 1 : 0;
                int y2 = playerHead.Value.Y < ySize ? playerHead.Value.Y + 1 : ySize;

                bool headFound = false;
                for (int x = x1; x <= x2; x++)
                {
                    for (int y = y1; y <= y2; y++)
                    {
                        if (CurrentArray[x, y] == playerHead.Key)
                        {
                            if (CurrentArray[x, y] != PreviousArray[x, y])
                            {
                                //bingo, tady je hlava
                                playerHead.Value.Update(x, y);
                                headFound = true;
                                break;
                            }
                        }
                    }

                    if (headFound)
                        break;
                }

                if (headFound == false)
                    deadPlayers.Add(playerHead.Key);

            }

            foreach (var deadPlayer in deadPlayers)
            {
                PlayerHeads.Remove(deadPlayer);
            }

        }

    }
}
