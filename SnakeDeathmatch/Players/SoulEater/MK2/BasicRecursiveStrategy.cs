using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater.MK2
{
    public class BasicRecursiveStrategy
    {
        private IList<Move> _moves = new List<Move> { Move.Left, Move.Straight, Move.Right };
        public int RecurseDepth { get; set; }

        public BasicRecursiveStrategy(int depth, bool useEstimation)
        {
            RecurseDepth = depth;
            UseEstimation = useEstimation;
        }

        public bool UseEstimation { get; set; }

        public Move GetNextMoveAndUpdateMyNextPositionAndDirection(GameGroundMK2 gameGround, ref PointClass myPositionPointClass, ref Direction myDirection)
        {
            IList<RecurseResult> recurseResults = new List<RecurseResult>();

            var paths = new List<PathClass>();
            foreach (var move in _moves)
            {
                var absoluteDirection = DirectionHelper.GetAbsoluteDirection(myDirection, move);
                PathClass path = myPositionPointClass.GetPath(absoluteDirection);
                paths.Add(path);
            }

            foreach (var path in paths)
            {
                if (path.PathState == PathState.Ok && path.PointTo.Danger != DangerType.Danger3)
                {
                    var points= TryPathRecursive(path, 0, new List<PathClass>());
                    var result = new RecurseResult(points, path);
                    recurseResults.Add(result);

                    if (result.Danger == DangerType.None && points == RecurseDepth) 
                        break;
                }
            }

            var orderedResults = recurseResults.OrderByDescending(x => x.Depth);

            RecurseResult bestResult = orderedResults.FirstOrDefault();

            if (bestResult != null)
            {
                var move = DirectionHelper.GetMove(myDirection, bestResult.Path.Direction);

                myPositionPointClass = bestResult.Path.PointTo;
                myDirection = bestResult.Path.Direction;
                return move;
            }

            return Move.Straight;
        }

        private int TryPathRecursive(PathClass path, int actualDepth, List<PathClass> pathClassList)
        {
            if (actualDepth == RecurseDepth)
                return actualDepth;

            PointClass point = path.PointTo;
            Direction[] possibleDirections = new Direction[3];

            int i = 0;
            foreach (var move in _moves)
            {
                possibleDirections[i++] = DirectionHelper.GetAbsoluteDirection(path.Direction, move);
            }

            actualDepth++;

            pathClassList.Add(path);

            var bestResult = actualDepth;
            foreach (Direction move in possibleDirections)
            {
                int result = 0;
                PathClass nextPath = point.GetPath(move);

                if (nextPath.PathState != PathState.Ok)
                {
                    continue;
                }
                if (nextPath.PointTo.Danger == DangerType.Danger3)
                {
                    continue;
                }
                bool conflictInUsedPath = false;
                foreach (var usedPath in pathClassList)
                {
                    if (nextPath.CrossPaths.Contains(nextPath))
                    {
                        conflictInUsedPath = true;
                        break;
                    }
                    if (nextPath.PointTo == usedPath.PointFrom)
                    {
                        conflictInUsedPath = true;
                        break;
                    }
                    
                }
                {
                    var crossPath = nextPath.CrossPaths.FirstOrDefault();
                    if (crossPath != null && ((crossPath.PointTo.IsUsed || pathClassList.Select(x => x.PointFrom).Contains(crossPath.PointTo)) && (crossPath.PointFrom.IsUsed || pathClassList.Select(x => x.PointFrom).Contains(crossPath.PointFrom))))
                    {
                        conflictInUsedPath = true;
                    }
                }
                if (conflictInUsedPath)
                {
                    continue;
                }
                if (nextPath.BestResult != null)
                {
                    result =  nextPath.BestResult.Value;
                }          
                else
                {
                    result = TryPathRecursive(nextPath, actualDepth, pathClassList);
                }
                
                pathClassList.Remove(nextPath);
                if (result > bestResult)
                {
                    bestResult = result;
                    if (bestResult == RecurseDepth)
                    {
                        break;
                    }
                }
            }

            if (bestResult != RecurseDepth && UseEstimation)
            {
                path.BestResult = bestResult;
            }
            return bestResult;
        }

        private class RecurseResult
        {
            public int Depth;

            public PathClass Path;

            public DangerType Danger {
                get { return Path.PointTo.Danger; }
            }

            public RecurseResult(int depth, PathClass path)
            {
                Depth = depth;
                Path = path;
            }

        }
    }
}
