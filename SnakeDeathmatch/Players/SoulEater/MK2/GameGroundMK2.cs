using System;
using System.Collections.Generic;
using System.Linq;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Jardik.Debug;

namespace SnakeDeathmatch.Players.SoulEater.MK2
{
    public class GameGroundMK2 : IIntArray
    {
        public PointClass[,] Points;
        public int GroundSize;

        public IList<PlayerInfoMk2> Players = new List<PlayerInfoMk2>();

        private int _myId;

        public GameGroundMK2(int groundSize, int myId)
        {
            GroundSize = groundSize;
            this._myId = myId;

            Points = new PointClass[GroundSize, GroundSize];
        }

        public void Init()
        {
            var fakePoint = CreateFakePoint();

            for (int y = 0; y < GroundSize; y++)
            {
                for (int x = 0; x < GroundSize; x++)
                {
                    var newPoint = new PointClass(x, y);
                    Points[x,y] = newPoint;

                    InitPointLinks(newPoint, x, y, fakePoint);
                }
            }

            InitCrossPaths();
        }

        private void InitCrossPaths()
        {
            for (int y = 0; y < GroundSize; y++)
            {
                for (int x = 0; x < GroundSize; x++)
                {
                    PointClass point = this[x, y];

                    var bottomLeftDirectionPath = point.GetPath(Direction.BottomLeft);
                    bottomLeftDirectionPath.CrossPaths = new List<PathClass>
                    {
                        point.GetLinkedPoint(Direction.Left).GetPath(Direction.BottomRight),
                        point.GetLinkedPoint(Direction.Left).GetPath(Direction.BottomRight).OpositePath
                    };

                    var bottomRightDirectionPath = point.GetPath(Direction.BottomRight);
                    bottomRightDirectionPath.CrossPaths = new List<PathClass>
                    {
                        point.GetLinkedPoint(Direction.Right).GetPath(Direction.BottomLeft),
                        point.GetLinkedPoint(Direction.Right).GetPath(Direction.BottomLeft).OpositePath
                    };

                    var topLeftDirectionPath = point.GetPath(Direction.TopLeft);
                    topLeftDirectionPath.CrossPaths = new List<PathClass>
                    {
                        point.GetLinkedPoint(Direction.Left).GetPath(Direction.TopRight),
                        point.GetLinkedPoint(Direction.Left).GetPath(Direction.TopRight).OpositePath
                    };

                    var topRightDirectionPath = point.GetPath(Direction.TopRight);
                    topRightDirectionPath.CrossPaths = new List<PathClass>
                    {
                        point.GetLinkedPoint(Direction.Right).GetPath(Direction.TopLeft),
                        point.GetLinkedPoint(Direction.Right).GetPath(Direction.TopLeft).OpositePath
                    };
                }
            }
        }

        private PointClass CreateFakePoint()
        {
            PointClass fakePoint = new PointClass(-1, -1);

            var bottomWay = new PathClass(fakePoint, fakePoint, Direction.Bottom, PathState.DeathInThisRound);
            bottomWay.OpositePath = bottomWay;
            fakePoint.PathsFromPoint.Add(bottomWay);

            var bottomLeftWay = new PathClass(fakePoint, fakePoint, Direction.BottomLeft, PathState.DeathInThisRound);
            bottomLeftWay.OpositePath = bottomLeftWay;
            fakePoint.PathsFromPoint.Add(bottomLeftWay);

            var bottomRightWay = new PathClass(fakePoint, fakePoint, Direction.BottomRight, PathState.DeathInThisRound);
            bottomRightWay.OpositePath = bottomRightWay;
            fakePoint.PathsFromPoint.Add(bottomRightWay);

            var leftWay = new PathClass(fakePoint, fakePoint, Direction.Left, PathState.DeathInThisRound);
            leftWay.OpositePath = leftWay;
            fakePoint.PathsFromPoint.Add(leftWay);

            var rightWay = new PathClass(fakePoint, fakePoint, Direction.Right, PathState.DeathInThisRound);
            rightWay.OpositePath = rightWay;
            fakePoint.PathsFromPoint.Add(rightWay);

            var topWay = new PathClass(fakePoint, fakePoint, Direction.Top, PathState.DeathInThisRound);
            topWay.OpositePath = topWay;
            fakePoint.PathsFromPoint.Add(topWay);

            var topLeftWay = new PathClass(fakePoint, fakePoint, Direction.TopLeft, PathState.DeathInThisRound);
            topLeftWay.OpositePath = topLeftWay;
            fakePoint.PathsFromPoint.Add(topLeftWay);

            var topRightWay = new PathClass(fakePoint, fakePoint, Direction.TopRight, PathState.DeathInThisRound);
            topRightWay.OpositePath = topRightWay;
            fakePoint.PathsFromPoint.Add(topRightWay);

            fakePoint.SetIsUsedAndUpdateItselfPaths();

            return fakePoint;
        }

        private BasicRecursiveStrategy otherPlayersStrategy = new BasicRecursiveStrategy(4, false);

        public void Update(int[,] newGround)
        {
            foreach (var player in Players)
            {
                player.IsDown = true;
            }

            for (int y = 0; y < GroundSize; y++)
            {
                for (int x = 0; x < GroundSize; x++)
                {
                    PointClass point = Points[x, y];
                    point.Danger = DangerType.None;

                    foreach (var path in point.PathsFromPoint)
                    {
                        path.BestResult = null;
                    }
                }
            }

            for (int y = 0; y < GroundSize; y++)
            {
                for (int x = 0; x < GroundSize; x++)
                {
                    var value = newGround[x, y];

                    if (value != 0)
                    {
                        PointClass point = Points[x, y];

                        if (point.IsUsed == false)
                        {
                            UpdateGroundPointIsUsed(point);
                            if (value != _myId)
                            {
                                UpdatePlayerLocationAndUpdateDangerZone(value, x, y);
                            }
                        }
                        
                    }
                }
            }          
        }

        private void UpdatePlayerLocationAndUpdateDangerZone(int playerIdentificator, int x, int y)
        {
            PointClass point = this[x, y];
            PlayerInfoMk2 player = Players.SingleOrDefault(p => p.Identificator == playerIdentificator);

            if (player == null)
            {
                player = new PlayerInfoMk2(point, playerIdentificator);
                Players.Add(player);

                return;
            }

            player.UpdatePosition(point);

            if (player.Direction == null)
                return;

            CreateDangerZoneForPlayer(player);
        }

        private void CreateDangerZoneForPlayer(PlayerInfoMk2 player)
        {
            //foreach (var previousPoint in player.PreviousPoints)
            //{
            //    var points = previousPoint.PathsFromPoint.Select(x => x.PointTo);
            //    foreach (var point in points)
            //    {
            //        point.Danger = DangerType.Danger1;
            //    }
            //}

            //CreateDangerZone(6, player.Point, player.Direction.Value);
        }

        private void CreateDangerZone(int numberOfPoints, PointClass point, Direction direction)
        {
            point = point.GetLinkedPoint(direction);

            if (numberOfPoints == 0)
                return;
            
            if (point.IsUsed)
                return;

            point.Danger = DangerType.Danger3;

            //var paths = point.PathsFromPoint.Where(p => p.PointTo.IsUsed == false);

            //foreach (var neibPoint in paths.Select(p => p.PointTo))
            //{
            //    neibPoint.Danger = DangerType.Danger1;
            //}

            //otherPlayersStrategy.GetNextMoveAndUpdateMyNextPositionAndDirection(this, ref point, ref direction);

            CreateDangerZone(--numberOfPoints, point, direction);
        }

        public PointClass this[int x, int y]
        {
            get
            {
                return Points[x, y];
            }
        }

        private void UpdateGroundPointIsUsed(PointClass point)
        {
            point.SetIsUsedAndUpdateItselfPaths();
            
            // crossColision
            bool topLeftPointIsUsed = point.GetLinkedPoint(Direction.TopLeft).IsUsed;
            if (topLeftPointIsUsed)
            {
                PathClass path = point.GetLinkedPoint(Direction.Top).GetPath(Direction.BottomLeft);
                path.PathState = PathState.DeathInThisRound;
                path.OpositePath.PathState = PathState.DeathInThisRound;
            }

            bool topRightPointIsUsed = point.GetLinkedPoint(Direction.TopRight).IsUsed;
            if (topRightPointIsUsed)
            {
                PathClass path = point.GetLinkedPoint(Direction.Top).GetPath(Direction.BottomRight);
                path.PathState = PathState.DeathInThisRound;
                path.OpositePath.PathState = PathState.DeathInThisRound;
            }

            bool bottomLeftPointIsUsed = point.GetLinkedPoint(Direction.BottomLeft).IsUsed;
            if (bottomLeftPointIsUsed)
            {
                PathClass path = point.GetLinkedPoint(Direction.Bottom).GetPath(Direction.TopLeft);
                path.PathState = PathState.DeathInThisRound;
                path.OpositePath.PathState = PathState.DeathInThisRound;
            }

            bool toprightPointIsUsed = point.GetLinkedPoint(Direction.BottomRight).IsUsed;
            if (toprightPointIsUsed)
            {
                PathClass path = point.GetLinkedPoint(Direction.Bottom).GetPath(Direction.TopRight);
                path.PathState = PathState.DeathInThisRound;
                path.OpositePath.PathState = PathState.DeathInThisRound;
            }
        }

        private void InitPointLinks(PointClass point, int x, int y, PointClass fakePoint)
        {
            // link s levym
            {
                PointClass pointToLink;
                PathState pathState;

                if (x != 0)
                {
                    pointToLink = Points[x - 1, y];
                    pathState = PathState.Ok;
                }
                else
                {
                    pointToLink = fakePoint;
                    pathState = PathState.DeathInThisRound;
                }

                PathClass pathFromPoint = new PathClass(point, pointToLink, Direction.Left, pathState);
                point.AddPath(pathFromPoint);

                PathClass pathToPoint = new PathClass(pointToLink, point, Direction.Left.GetOpositeDirection(), pathState);
                pointToLink.AddPath(pathToPoint);

                pathFromPoint.OpositePath = pathToPoint;
                pathToPoint.OpositePath = pathFromPoint;
            }

            // link s hornim levym
            {
                PointClass pointToLink;
                PathState pathState;

                if (x != 0 && y != 0)
                {
                    pointToLink = Points[x - 1, y - 1];
                    pathState = PathState.Ok;
                }
                else
                {
                    pointToLink = fakePoint;
                    pathState = PathState.DeathInThisRound;
                }

                PathClass pathFromPoint = new PathClass(point, pointToLink, Direction.TopLeft, pathState);
                point.AddPath(pathFromPoint);

                PathClass pathToPoint = new PathClass(pointToLink, point, Direction.TopLeft.GetOpositeDirection(), pathState);
                pointToLink.AddPath(pathToPoint);

                pathFromPoint.OpositePath = pathToPoint;
                pathToPoint.OpositePath = pathFromPoint;
            }

            // link s hornim
            {
                PointClass pointToLink;
                PathState pathState;

                if (y != 0)
                {
                    pointToLink = Points[x, y - 1];
                    pathState = PathState.Ok;
                }
                else
                {
                    pointToLink = fakePoint;
                    pathState = PathState.DeathInThisRound;
                }

                PathClass pathFromPoint = new PathClass(point, pointToLink, Direction.Top, pathState);
                point.AddPath(pathFromPoint);

                PathClass pathToPoint = new PathClass(pointToLink, point, Direction.Top.GetOpositeDirection(), pathState);
                pointToLink.AddPath(pathToPoint);

                pathFromPoint.OpositePath = pathToPoint;
                pathToPoint.OpositePath = pathFromPoint;
            }

            // link s hornim pravym
            {
                PointClass pointToLink;
                PathState pathState;

                if (x != GroundSize -1 && y != 0)
                {
                    pointToLink = Points[x + 1, y - 1];
                    pathState = PathState.Ok;
                }
                else
                {
                    pointToLink = fakePoint;
                    pathState = PathState.DeathInThisRound;
                }

                PathClass pathFromPoint = new PathClass(point, pointToLink, Direction.TopRight, pathState);
                point.AddPath(pathFromPoint);

                PathClass pathToPoint = new PathClass(pointToLink, point, Direction.TopRight.GetOpositeDirection(), pathState);
                pointToLink.AddPath(pathToPoint);

                pathFromPoint.OpositePath = pathToPoint;
                pathToPoint.OpositePath = pathFromPoint;
            }

            // link s pravym
            if (x == GroundSize - 1)
            {
                PathClass newPath = new PathClass(point, fakePoint, Direction.Right, PathState.DeathInThisRound);
                var opositeWay = fakePoint.GetPath(newPath.Direction.GetOpositeDirection());
                newPath.OpositePath = opositeWay;
                point.AddPath(newPath);
            }

            // link s pravym dolnim
            if (x == GroundSize - 1 || y == GroundSize - 1)
            {
                PathClass newPath = new PathClass(point, fakePoint, Direction.BottomRight, PathState.DeathInThisRound);
                var opositeWay = fakePoint.GetPath(newPath.Direction.GetOpositeDirection());
                newPath.OpositePath = opositeWay;
                point.AddPath(newPath);
            }

            // link s dolnim
            if (y == GroundSize - 1)
            {
                PathClass newPath = new PathClass(point, fakePoint, Direction.Bottom, PathState.DeathInThisRound);
                var opositeWay = fakePoint.GetPath(newPath.Direction.GetOpositeDirection());
                newPath.OpositePath = opositeWay;
                point.AddPath(newPath);
            }

            // link s dolnim levym
            if (x == 0 || y == GroundSize - 1)
            {
                PathClass newPath = new PathClass(point, fakePoint, Direction.BottomLeft, PathState.DeathInThisRound);
                var opositeWay = fakePoint.GetPath(newPath.Direction.GetOpositeDirection());
                newPath.OpositePath = opositeWay;
                point.AddPath(newPath);
            }
        }

        public int[,] InnerArray
        {
            get { return new int[100, 100]; }
        }
    }


    public class PointClass
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsUsed { get; set; }
        public DangerType Danger { get; set; }

        public IList<PathClass> PathsFromPoint { get; set; }

        public PointClass(int x, int y)
        {
            X = x;
            Y = y;

            PathsFromPoint = new List<PathClass>();
            IsUsed = false;
            Danger = DangerType.None;
        }

        public void AddPath(PathClass path)
        {
            if (path.PointFrom != this)
            {
                throw new InvalidOperationException("");
            }

            PathsFromPoint.Add(path);
        }

        public bool OtherPlayerDanger;

        public PathClass GetPath(Direction direction)
        {
            return PathsFromPoint.First(x => x.Direction == direction);
        }

        public void SetIsUsedAndUpdateItselfPaths()
        {
            if (IsUsed)
                return;

            foreach (var link in PathsFromPoint)
            {
                link.OpositePath.PathState = PathState.DeathInThisRound;
            }

            IsUsed = true;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}]", X, Y);
        }

        public PointClass GetLinkedPoint(Direction direction)
        {
            PathClass linkToPoint = PathsFromPoint.First(x => x.Direction == direction);

            return linkToPoint.PointTo;
        }
    }

    public class PathClass
    {
        public PointClass PointFrom { get; protected set; }
        public PointClass PointTo { get; protected set; }
        public Direction Direction { get; protected set; }

        public PathState PathState { get; set; }

        public PathClass OpositePath { get; set; }

        public List<PathClass> CrossPaths { get; set; }

        public override string ToString()
        {
            return string.Format("Z {0} do {1} směr {2}", PointFrom, PointTo, Direction);
        }

        public int? BestResult;

        public PathClass(PointClass pointFrom, PointClass pointTo, Direction direction, PathState pathState)
        {
            PointFrom = pointFrom;
            PointTo = pointTo;
            Direction = direction;
            PathState = pathState;
            CrossPaths =new List<PathClass>();
        }
    }

    public enum PathState
    {
        Ok,
        DeathInNextRound,
        DeathInThisRound
    }

    public enum DangerType
    {
        None,
        Danger1,
        Danger2,
        Danger3
    }
}
