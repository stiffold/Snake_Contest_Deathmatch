using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players;
using SnakeDeathmatch.Players.Vazba;
using SnakeDeathmatch.Players.Randomer;
using SnakeDeathmatch.Players.SoulEater;
using Direction = SnakeDeathmatch.Interface.Direction;

namespace SnakeDeathmatch.Game
{
    public class GameEngine
    {
        private int _max;
        private List<Player> _players = new List<Player>();
        private int[,] _gameSurround;
        private bool _gameOver;


        public GameEngine(int max)
        {
            //TODO Reflection

            _max = max;
            _gameSurround = new int[max, max];

            PlayerFactory playerFactory = new PlayerFactory(_gameSurround, _max);

            _players.Add(playerFactory.Create<Jardik>(Colors.Red));
            _players.Add(playerFactory.Create<VazbaPlayer>(Colors.Blue));
            _players.Add(playerFactory.Create<Setal>(Colors.Aqua));
            _players.Add(playerFactory.Create<SoulEaterBehavior>(Colors.White));
        }

        public int[,] Move()
        {
            foreach (Player player in _players.Where(p => p.State == PlayerState.Playing))
            {
                player.NextMove((int[,])_gameSurround.Clone());

                //kolize s velikostí pole
                if (player.Position.IsInCollission(_max))
                {
                    player.State = PlayerState.CollisionWithTerrain;
                    break;
                }

                //kolize s hráčem
                if (_gameSurround[player.Position.X, player.Position.Y] != 0)
                {
                    player.State = PlayerState.CollisionWithPlayer;
                    break;
                }

                //cross kolize
                if (CrossCollision(player))
                {
                    player.State = PlayerState.CrossCollission;
                    break;
                }

                //vlastní kolize v tahu
                if (_players.Where(p => p.State == PlayerState.Playing).Any(p => player.Position.X == p.Position.X && player.Position.Y == p.Position.Y && p.Identificator != player.Identificator))
                {
                    player.State = PlayerState.OneMoveCollission;
                    break;
                }

                //commit tahu
                if (player.State == PlayerState.Playing)
                {
                    _gameSurround[player.Position.X, player.Position.Y] = player.Identificator;
                }
            }

            FillUnreachableDots();


            if (!_players.Any(p => p.State == PlayerState.Playing))
            {
                _gameOver = true;
            }

            return _gameSurround;
        }

        private bool CrossCollision(Player player)
        {
            switch (player.MyDirection)
            {
                case Direction.TopRight: return (_gameSurround[player.Position.X, player.Position.Y + 1] != 0) && (_gameSurround[player.Position.X - 1, player.Position.Y] != 0);
                case Direction.BottomRight: return (_gameSurround[player.Position.X, player.Position.Y - 1] != 0) && (_gameSurround[player.Position.X - 1, player.Position.Y] != 0);
                case Direction.BottomLeft: return (_gameSurround[player.Position.X, player.Position.Y - 1] != 0) && (_gameSurround[player.Position.X + 1, player.Position.Y] != 0);
                case Direction.TopLeft: return (_gameSurround[player.Position.X, player.Position.Y + 1] != 0) && (_gameSurround[player.Position.X + 1, player.Position.Y] != 0);
            }
            return false;
        }

        private bool IsEmpty(int x, int y)
        {
            return (x >= 0 && x < _max && y >= 0 && y < _max) && (_gameSurround[x, y] == 0);
        }

        private void FillUnreachableDots()
        {
            for (int y = 0; y < _max; y++)
            {
                for (int x = 0; x < _max; x++)
                {
                    if (IsEmpty(x, y) && !IsEmpty(x, y-1) && !IsEmpty(x-1, y) && !IsEmpty(x, y+1) && !IsEmpty(x+1, y))
                    {
                        _gameSurround[x, y] = -1;
                    }
                }
            }
        }

        public Color GetColorForIdentificator(int i)
        {
            if (i == -1)
                return Color.FromRgb(64, 64, 64);

            var player = _players.Where(p => p.Identificator == i).FirstOrDefault();
            if (player != null)
            {
                return player.Color;
            }
            return Colors.Magenta;
        }

        public string ScoreMessage()
        {
            var stringBuilder = new StringBuilder();
            foreach (Player player in _players.OrderByDescending(p => p.Score))
            {
                stringBuilder.AppendLine(String.Format("Jméno {0}, Skóre {1}, Smrt {2}, Barva {3}", player.Name, player.Score, player.State, player.Color));
            }
            return stringBuilder.ToString();
        }

        public bool GameOver { get { return _gameOver; } }
    }
}
