using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater
{
    public class GameGround
    {
        #region props and fields

        public int[,] Ground { get; protected set; }

        public int SizeOfTable { get; protected set; }

        public IList<PlayerInfo> OtherPlayers { get; protected set; }

        public PlayerInfo OurHeroicPlayer { get; protected set; }

        private IDictionary<int, VersionRecord> _versionRecordDictionary = new Dictionary<int, VersionRecord>();
        
        public int CurrentVersion = 0;

        #endregion

        #region constr

        public GameGround(int[,] ground, int ourIdentificator, Direction ourInitialDirection)
        {
            Ground = ground;
            SizeOfTable = Ground.GetUpperBound(0) + 1;
            OtherPlayers = new List<PlayerInfo>();
            InitPositionsOfPlayers(ourIdentificator);
            OurHeroicPlayer.Direction = ourInitialDirection;
        }

        public GameGround(int[,] ground, int sizeOfTable, IList<PlayerInfo> otherPlayers, PlayerInfo ourHeroicPlayer)
        {
            Ground = ground;
            SizeOfTable = sizeOfTable;
            OtherPlayers = otherPlayers;
            OurHeroicPlayer = ourHeroicPlayer;
        }

        #endregion

        public void Update(int[,] newGround)
        {
            UpdatePlayersInfos(newGround);

            Ground = newGround;
        }

        public int GetPointValue(Point point)
        {
            return this[point.X,point.Y];
        }

        public int this[int x, int y]
        {
            get
            {
                return Ground[x, y];
            }
        }

        public GameGround MakeACopy()
        {
            IList<PlayerInfo> players = new List<PlayerInfo>();

            foreach (var player in OtherPlayers)
            {
                players.Add(player.MakeACopy());
            }

            return new GameGround((int[,])Ground.Clone(), SizeOfTable, players, OurHeroicPlayer.MakeACopy());
        }

        public void SimulateStateAfterOtherPlayersMove()
        {
            foreach (var player in OtherPlayers.Where(x => x.IsDown == false))
            {
                if (player.Direction == null)
                    continue;

                Point nextPoint = DirectionHelper.GetNextPoint(player.CurrentPosition, player.Direction.Value);
                if (nextPoint.X >= SizeOfTable || nextPoint.Y >= SizeOfTable || nextPoint.X < 0 || nextPoint.Y < 0)
                    continue;

                Ground[nextPoint.X, nextPoint.Y] = 666;
                UpdatePlayerPositionAndDirection(player, nextPoint);               
            }
        }

        public void VersionUp(Point nextPointForOurPlayer)
        {
            CurrentVersion++;
            _versionRecordDictionary.Add(new KeyValuePair<int, VersionRecord>(
                CurrentVersion,
                new VersionRecord(new List<Point> { nextPointForOurPlayer }, OurHeroicPlayer.CurrentPosition, OurHeroicPlayer.Direction)));
            
            Ground[nextPointForOurPlayer.X, nextPointForOurPlayer.Y] = OurHeroicPlayer.Identificator;
            UpdatePlayerPositionAndDirection(OurHeroicPlayer, nextPointForOurPlayer);
        }

        //public void SimulateOurMove(Point nextPointForOurPlayer)
        //{
        //    Ground[nextPointForOurPlayer.X, nextPointForOurPlayer.Y] = OurHeroicPlayer.Identificator;
        //    UpdatePlayerPositionAndDirection(OurHeroicPlayer, nextPointForOurPlayer);
        //}

        public void VersionDownTo(int version)
        {
            for (int i = CurrentVersion; i > version; i--)
            {
                var versionRecord = _versionRecordDictionary[i];
                _versionRecordDictionary.Remove(i);
                
                foreach (var point in versionRecord.ChangedPoints)
                {
                    Ground[point.X, point.Y] = 0;
                }

                OurHeroicPlayer.CurrentPosition = versionRecord.PreviousPoint;
                OurHeroicPlayer.Direction = versionRecord.PreviousDirection;

                CurrentVersion--;
            }
        }

        #region private

        private void InitPositionsOfPlayers(int ourIdentificator)
        {
            for (int x = 0; x < SizeOfTable; x++)
            {
                for (int y = 0; y < SizeOfTable; y++)
                {
                    if (Ground[x, y] == ourIdentificator)
                    {
                        OurHeroicPlayer = new PlayerInfo(x, y, ourIdentificator);

                        continue;
                    }

                    if (Ground[x, y] != 0)
                    {
                        AddOtherPlayerInfo(x, y, Ground[x, y]);
                    }
                }
            }
        }

        private void AddOtherPlayerInfo(int x, int y, int identificator)
        {
            if (OtherPlayers.Any(pl => pl.Identificator == identificator))
            {
                return;
            }

            OtherPlayers.Add(new PlayerInfo(x, y, identificator));
        }

        private void UpdatePlayersInfos(int[,] ground)
        {
            foreach (PlayerInfo player in OtherPlayers)
            {
                player.IsDown = true;
            }

            for (int x = 0; x < SizeOfTable; x++)
            {
                for (int y = 0; y < SizeOfTable; y++)
                {
                    int oldValue = Ground[x, y];
                    int newValue = ground[x, y];

                    if (oldValue != newValue)
                    {
                        PlayerInfo movedPlayer;

                        if (newValue == OurHeroicPlayer.Identificator)
                        {
                            movedPlayer = OurHeroicPlayer;
                        }
                        else
                        {
                           movedPlayer = OtherPlayers.FirstOrDefault(player => player.Identificator == newValue); 
                        }

                        if (movedPlayer == null)
                            continue;

                        movedPlayer.IsDown = false;

                        UpdatePlayerPositionAndDirection(movedPlayer, new Point(x, y));
                    }
                }
            }
        }

        private void UpdatePlayerPositionAndDirection(PlayerInfo movedPlayer, Point nextPoint)
        {
            Point oldPoint = movedPlayer.CurrentPosition;
            Point newPoint = nextPoint;

            Direction newDirection = DirectionHelper.GetDirection(oldPoint, newPoint);

            movedPlayer.Direction = newDirection;
            movedPlayer.CurrentPosition = newPoint;
        }

        #endregion

        private class VersionRecord
        {
            public VersionRecord(IList<Point> points, Point previousPoint, Direction? previousDirection)
            {
                ChangedPoints = points;
                PreviousPoint = previousPoint;
                PreviousDirection = previousDirection;
            }

            public IList<Point> ChangedPoints { get; protected set; }
            public Point PreviousPoint { get; protected set; }
            public Direction? PreviousDirection { get; protected set; }
        }
    }
}
