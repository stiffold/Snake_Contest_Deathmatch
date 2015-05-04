using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater
{
    public class GameGround
    {
        #region props

        public int[,] Ground { get; set; }

        public int SizeOfTable { get; set; }

        public IList<PlayerInfo> OtherPlayers { get; set; }

        public PlayerInfo OurHeroicPlayer { get; set; }

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

                        Point oldPoint = movedPlayer.CurrentPosition;
                        Point newPoint = new Point(x, y);

                        Direction newDirection = DirectionHelper.GetDirection(oldPoint, newPoint);

                        movedPlayer.Direction = newDirection;
                        movedPlayer.CurrentPosition = newPoint;
                    }
                }
            }
        }

        #endregion
    }
}
