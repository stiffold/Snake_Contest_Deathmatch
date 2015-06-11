using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.ClockworkMole
{
    public class FloodStrategy
    {
        private const int NearPlaceDistance = 4;
        private const int MaxRecursion = 15;
        private const int VataValue = 1000;


        public Move? GetNextMove(Playground playground, Position position)
        {

            playground.CreateTestCollisionArray();


            try
            {
                return AnalyseSurroundings(playground, position);
            }
            catch (Exception)
            {
                return Move.Left;
            }

        }


        private Move? AnalyseSurroundings(Playground playground, Position position)
        {


            var boundaryItems = playground.TestCollissionArray.SelectItemsAndEnumerate(position, NearPlaceDistance)
                            .Where(arrayItem => arrayItem.DistanceTo(position) == NearPlaceDistance)
                            .Where(arrayItem => arrayItem.Value == 0)
                            .ToArray();


            var reachableGateItems = boundaryItems
                .Select(arrayItem => new GateItem()
                                        {
                                            ArrayItem = arrayItem,
                                            PossibleMoves = GetMovesIfReachable(playground, position, arrayItem)
                                        })
                .Where(x => x.PossibleMoves.Count > 0)
                .ToList();


            if (reachableGateItems.Any() == false)
                return null;     //jsem uvezněn, žádná cesta ven!!!



            // ■ funguje, ale pomale
            var sw = new Stopwatch();
            sw.Start();

            foreach (var gateItem in reachableGateItems)
            {
                bool found = false;
                foreach (var compareItem in reachableGateItems)
                {
                    if (gateItem.ArrayItem.DistanceTo(new Position(compareItem.ArrayItem.X, compareItem.ArrayItem.Y, Direction.Top)) == 1 && compareItem.Area > 0)
                    {
                        gateItem.Area = compareItem.Area;
                        found = true;
                    }
                }

                if (found == false)
                    gateItem.Area = CalculateGateFreeArea(playground, position, gateItem.ArrayItem);

            }

            sw.Stop();
            var bestGateItem = reachableGateItems.Where(x => x.Area > 0)
                .OrderByDescending(x => x.Area)
                .FirstOrDefault();

            if (bestGateItem != null)
            {
                return bestGateItem.PossibleMoves.FirstOrDefault();
            }

            return null;

            // ■ varianta 2 - odladit.
            //var sw = new Stopwatch();
            //sw.Start();

            //var calculatedReachableItems = new List<GateItem>();
            //while (reachableGateItems.Any())
            //{
            //    var firstItem = reachableGateItems.First();
            //    reachableGateItems.Remove(firstItem);
            //    firstItem.Area = CalculateGateFreeArea(playground, position, firstItem.ArrayItem);
            //    calculatedReachableItems.Add(firstItem);

            //    var nextGroupItems = FindNextGroupItems(reachableGateItems, firstItem);
            //    foreach (var neighboringItem in nextGroupItems)
            //    {
            //        neighboringItem.Area = firstItem.Area;
            //        calculatedReachableItems.Add(neighboringItem);
            //        reachableGateItems.Remove(neighboringItem);
            //    }

            //}

            //var bestGateItem = calculatedReachableItems.Where(x => x.Area > 0)
            //    .OrderByDescending(x => x.Area)
            //    .FirstOrDefault();

            //sw.Stop();

            //if (bestGateItem != null)
            //{
            //    return bestGateItem.PossibleMoves.FirstOrDefault();
            //}

            //return null;



            // ■
            // nejake pokusy, zatim si nechavam 

            //var sortedItems = new List<GateItem>();

            //int minY = boundaryItems.Min(arrayItem => arrayItem.Y);
            //var topBorderItems = reachableGateItem.Where(arg => arg.ArrayItem.Y == minY).OrderBy(arg => arg.ArrayItem.X).ToArray();
            //sortedItems.AddRange(topBorderItems);

            //int maxX = boundaryItems.Max(arrayItem => arrayItem.X);
            //var rightBorderItems = reachableGateItem.Where(arg => arg.ArrayItem.X == maxX).OrderBy(arg => arg.ArrayItem.Y);
            //sortedItems.AddRange(rightBorderItems);

            //int maxY = boundaryItems.Max(arrayItem => arrayItem.Y);
            //var bottomBorderItems = reachableGateItem.Where(arg => arg.ArrayItem.Y == maxY).OrderByDescending(arg => arg.ArrayItem.X);
            //sortedItems.AddRange(bottomBorderItems);

            //int minX = boundaryItems.Min(arrayItem => arrayItem.X);
            //var leftBottomBorderItems = reachableGateItem.Where(arg => arg.ArrayItem.X == minX).OrderByDescending(arg => arg.ArrayItem.Y);
            //sortedItems.AddRange(leftBottomBorderItems);


            //GateItem firstItem = null;
            //GateItem lastProcessedItem = null;
            //int gateNumber = 1;
            //foreach (var gateItem in sortedItems)
            //{
            //    if (firstItem == null)
            //    {
            //        firstItem = gateItem;
            //        firstItem.GateNumber = gateNumber;
            //    }

            //    if (lastProcessedItem == null)
            //        lastProcessedItem = gateItem;

            //    if (gateItem.ArrayItem.DistanceTo(lastProcessedItem.ArrayItem.X, lastProcessedItem.ArrayItem.Y) <= 1)
            //    {
            //        if (gateItem != firstItem)
            //            gateItem.GateNumber = lastProcessedItem.GateNumber;

            //    }
            //    else
            //    {
            //        gateNumber += 1;
            //        gateItem.GateNumber = gateNumber;
            //    }
            //    lastProcessedItem = gateItem;
            //}

            //if (first)

            //var reachableItemsFilteredQuery = from reachableItem in reachableBoundaryItemsWithMoves
            //                                  join helpItem in reachableBoundaryItemsWithMoves.ToArray() on 1 equals 1
            //                                  select new { reachableItem, helpItem };

            //var filteredResult = reachableItemsFilteredQuery
            //                    .Where(x => x.reachableItem.arrayItem.DistanceTo(x.helpItem.arrayItem.X, x.helpItem.arrayItem.Y) == 1)
            //                    .Select(x => x.reachableItem).ToArray();

        }

        private static List<GateItem> FindNextGroupItems(IEnumerable<GateItem> reachableGateItems, GateItem item)
        {
            var foundItems = new List<GateItem>();

            foundItems.AddRange(reachableGateItems.Where(x => (Math.Abs(x.ArrayItem.X - item.ArrayItem.X) == 1 || Math.Abs(x.ArrayItem.Y - item.ArrayItem.Y) == 1)
                                                        && (x.ArrayItem.X == item.ArrayItem.X || x.ArrayItem.Y == item.ArrayItem.Y)));

            foreach (var foundItem in foundItems.ToList())
            {
                foundItems.AddRange(FindNextGroupItems(reachableGateItems.Except(foundItems), foundItem));
            }

            return foundItems;
        }



        private List<Move> GetMovesIfReachable(Playground playground, Position startPosition, ArrayItem<int> destinationArrayItem)
        {

            var moves = new List<Move>();

            if (IsMovePossible(playground, startPosition, Move.Left, startPosition, new Position(destinationArrayItem.X, destinationArrayItem.Y, Direction.Left), 0, moves))
            {
                moves.Add(Move.Left);
                moves.Reverse();
                return moves;
            }

            if (IsMovePossible(playground, startPosition, Move.Straight, startPosition, new Position(destinationArrayItem.X, destinationArrayItem.Y, Direction.Top), 0, moves))
            {
                moves.Add(Move.Straight);
                moves.Reverse();
                return moves;
            }

            if (IsMovePossible(playground, startPosition, Move.Right, startPosition, new Position(destinationArrayItem.X, destinationArrayItem.Y, Direction.Right), 0, moves))
            {
                moves.Add(Move.Right);
                moves.Reverse();
                return moves;
            }

            return moves;

        }

        private bool IsMovePossible(Playground playground, Position position, Move move, Position startPosition, Position destinationPosition, int counter, List<Move> moves)
        {
            if (counter > MaxRecursion)
                return false;

            if (CollisionHelper.IsCollision(playground, position, move))
                return false;

            var newPosition = position.Move(move);

            if (Math.Max(Math.Abs(startPosition.X - newPosition.X), Math.Abs(startPosition.Y - newPosition.Y)) > NearPlaceDistance)
            {
                return false;
            }

            if (newPosition.X == destinationPosition.X && newPosition.Y == destinationPosition.Y)
            {
                return true;
            }

            playground.TestCollissionArray[newPosition.X, newPosition.Y] = VataValue; // naplnit necim, aby se jednou projite pole znovu netestovalo
            counter++;

            bool leftMoves = IsMovePossible(playground, newPosition, Move.Left, startPosition, destinationPosition, counter, moves);
            if (leftMoves == true)
            {
                playground.TestCollissionArray[newPosition.X, newPosition.Y] = 0; // a tady vratit zpet 
                moves.Add(Move.Left);
                return true;
            }

            bool straightMoves = IsMovePossible(playground, newPosition, Move.Straight, startPosition, destinationPosition, counter, moves);
            if (straightMoves == true)
            {
                playground.TestCollissionArray[newPosition.X, newPosition.Y] = 0; // a tady vratit zpet 
                moves.Add(Move.Straight);
                return true;
            }

            bool rightMoves = IsMovePossible(playground, newPosition, Move.Right, startPosition, destinationPosition, counter, moves);
            if (rightMoves == true)
            {
                playground.TestCollissionArray[newPosition.X, newPosition.Y] = 0; // a tady vratit zpet 
                moves.Add(Move.Right);
                return true;
            }

            playground.TestCollissionArray[newPosition.X, newPosition.Y] = 0; // a tady vratit zpet 

            return false;

        }

        private int CalculateGateFreeArea(Playground playground, Position headPosition, ArrayItem<int> position)
        {

            //vyplnit vatou vše ve vnitřním okruhu
            playground.TestCollissionArray.SelectItemsAndEnumerate(headPosition, NearPlaceDistance)
                .Where(x => x.Value == 0)
                .Apply(x => x.Value = VataValue)
                .Execute();

            int count = CountFreeItems(playground, position);

            //odstranit vatu
            playground.TestCollissionArray.SelectItemsAndEnumerate()
                .Where(x => x.Value == VataValue)
                .Apply(x => x.Value = 0)
                .Execute();

            return count;
        }

        private int CountFreeItems(Playground playground, ArrayItem<int> position)
        {
            List<ArrayItem<int>> newItems = playground.TestCollissionArray.SelectItemsAndEnumerate(position.X, position.Y, 1)
                .Where(x => x.Value == 0)
                .Apply(x => x.Value = VataValue)
                .ToList();

            int itemCount = newItems.Count;

            while (newItems.Any())
            {
                List<ArrayItem<int>> tmpItems = new List<ArrayItem<int>>();

                foreach (var arrayItem in newItems)
                {
                    tmpItems.AddRange(playground.TestCollissionArray.SelectItemsAndEnumerate(arrayItem.X, arrayItem.Y, 1)
                        .Where(x => x.Value == 0 && CollisionHelper.IsCollision(playground, new Position(arrayItem.X, arrayItem.Y, Direction.Top), new Position(x.X, x.Y, Direction.Top)) == false)
                        .Apply(x => x.Value = VataValue)
                        .ToList());
                }

                newItems = tmpItems;
                itemCount += tmpItems.Count;

            }

            return itemCount;
        }
    }

    public class GateItem
    {
        public ArrayItem<int> ArrayItem { get; set; }
        public List<Move> PossibleMoves { get; set; }

        public int GateNumber { get; set; }

        public int Area { get; set; }
    }

}
