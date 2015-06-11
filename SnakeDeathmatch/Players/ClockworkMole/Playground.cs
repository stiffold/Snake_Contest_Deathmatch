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

        public const int HeadTestDistance = 4;

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

            foreach (var playerHeadItem in PlayerHeads)
            {
                var playerHead = playerHeadItem.Value;

                TestCollissionArray.SelectItemsAndEnumerate(playerHead, HeadTestDistance)
                    .Where(x => x.Value == 0)
                    .Apply(x => x.Value = 100 + (HeadTestDistance - x.DistanceTo(playerHead.X, playerHead.Y) + 1))
                    .ToList();
            }

        }

        public void DecreaseTestCollistionArrayByOne()
        {

            foreach (var playerHeadItem in PlayerHeads)
            {
                var playerHead = playerHeadItem.Value;

                TestCollissionArray.SelectItemsAndEnumerate(playerHead, HeadTestDistance)
                    .Where(x => x.Value > 100 && x.Value < 200)
                    .Apply(x => x.Value = x.Value == 101 ? 0 : x.Value - 1)
                    .ToList();
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

            CurrentArray.SelectItemsAndEnumerate()
                    .Where(x => x.Value != 0 && x.Value != _playerId)
                    .Apply(x => PlayerHeads.Add(x.Value, new Position(x.X, x.Y, 0)))
                    .ToList();

        }

        private void ActualizeOtherPlayers()
        {

            List<int> foundHeads = new List<int>();

            foreach (var playerHeadItem in PlayerHeads)
            {

                CurrentArray.SelectItemsAndEnumerate(playerHeadItem.Value, 1)
                    .Where(arrayItem => arrayItem.Value == playerHeadItem.Key && arrayItem.Value != PreviousArray[arrayItem.X, arrayItem.Y])
                    .Apply(arrayItem =>
                    {
                        playerHeadItem.Value.Update(arrayItem.X, arrayItem.Y);
                        foundHeads.Add(playerHeadItem.Key);
                    })
                    .ToList();

            }

            //remove dead players
            foreach (var key in PlayerHeads.Keys.ToArray())
            {
                if (foundHeads.Contains(key) == false)
                {
                    PlayerHeads.Remove(key);
                }
            }

        }

        public string CurrentArrayString
        {
            get
            {
                if (CurrentArray != null)
                {
                    var maxX = CurrentArray.GetUpperBound(0);
                    var maxY = CurrentArray.GetUpperBound(1);

                    var sb = new StringBuilder();
                    for (int y = 0; y <= maxY; y++)
                    {
                        for (int x = 0; x <= maxX; x++)
                        {
                            sb.Append(CurrentArray[x, y]);
                        }
                        sb.AppendLine();
                    }

                    return sb.ToString();

                }

                return base.ToString();
            }
        }


        public string TestCollisionArrayString
        {
            get
            {
                if (CurrentArray != null)
                {
                    var maxX = TestCollissionArray.GetUpperBound(0);
                    var maxY = TestCollissionArray.GetUpperBound(1);

                    var sb = new StringBuilder();
                    for (int y = 0; y <= maxY; y++)
                    {
                        for (int x = 0; x <= maxX; x++)
                        {
                            sb.Append(TestCollissionArray[x, y]);
                        }
                        sb.AppendLine();
                    }

                    return sb.ToString();

                }

                return base.ToString();
            }
        }
    }
}
