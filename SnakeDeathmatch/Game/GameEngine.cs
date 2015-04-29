using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using SnakeDeathmatch.Interface;
using Direction = SnakeDeathmatch.Interface.Direction;

namespace SnakeDeathmatch.Game
{
    public class GameEngine
    {
        public const int HeadToHeadCrashId = -1;

        private List<Player> _players = new List<Player>();
        private int[,] _gameSurround;
        private bool _gameOver;

        public int Size { get; private set; }

        public GameEngine(int size, IEnumerable<Player> players)
        {
            Size = size;
            _gameSurround = new int[size, size];
            _players = players.ToList();

            foreach (Player player in players)
            {
                _gameSurround[player.Position.X, player.Position.Y] = player.Identifier;
            }
        }

        public int[,] Move()
        {
            List<Player> livePlayers;

            // necháme všechny přeživší odehrát svůj tah
            livePlayers = _players.Where(p => p.State == PlayerState.Playing).ToList();
            foreach (Player player in livePlayers)
            {
                player.NextMove((int[,])_gameSurround.Clone());
            }

            // detekce kolize s okrajem pole
            livePlayers = _players.Where(p => p.State == PlayerState.Playing).ToList();
            foreach (Player player in livePlayers)
            {
                if (player.Position.IsInCollission(Size))
                {
                    player.State = PlayerState.BoundaryCollision;
                }
            }

            // detekce kolize do těla jiného hada
            livePlayers = _players.Where(p => p.State == PlayerState.Playing).ToList();
            foreach (Player player in livePlayers)
            {
                if (_gameSurround[player.Position.X, player.Position.Y] != 0)
                {
                    player.State = PlayerState.HeadToTailCollision;
                }
            }

            // detekce snahy projet diagonálně skrz tělo jiného hada
            livePlayers = _players.Where(p => p.State == PlayerState.Playing).ToList();
            foreach (Player player in livePlayers)
            {
                if ((player.Direction == Direction.TopRight && (_gameSurround[player.Position.X, player.Position.Y + 1] != 0) && (_gameSurround[player.Position.X - 1, player.Position.Y] != 0)) ||
                    (player.Direction == Direction.BottomRight && (_gameSurround[player.Position.X, player.Position.Y - 1] != 0) && (_gameSurround[player.Position.X - 1, player.Position.Y] != 0)) ||
                    (player.Direction == Direction.BottomLeft && (_gameSurround[player.Position.X, player.Position.Y - 1] != 0) && (_gameSurround[player.Position.X + 1, player.Position.Y] != 0)) ||
                    (player.Direction == Direction.TopLeft && (_gameSurround[player.Position.X, player.Position.Y + 1] != 0) && (_gameSurround[player.Position.X + 1, player.Position.Y] != 0)))
                {
                    player.State = PlayerState.HeadToTailCrossCollision;
                }
            }

            // detekce srážky hlav dvou (a více) hadů
            livePlayers = _players.Where(p => p.State == PlayerState.Playing).ToList();
            var headToHeadCrashes = new List<Position>();
            foreach (Player player in livePlayers)
            {
                if (livePlayers.Any(p => player.Position.X == p.Position.X && player.Position.Y == p.Position.Y && player.Identifier != p.Identifier))
                {
                    player.State = PlayerState.HeadToHeadCollision;
                    headToHeadCrashes.Add(player.Position);
                }
            }
            
            // detekce diagonálního překřížení hlav dvou hadů
            foreach (Player player in livePlayers)  // záměrně neupdatuju livePlayers
            {
                if ((player.Direction == Direction.TopRight && livePlayers.Any(p => p.Direction == Direction.TopLeft && player.Position.X == p.Position.X + 1 && player.Position.Y == p.Position.Y)) ||
                    (player.Direction == Direction.TopLeft && livePlayers.Any(p => p.Direction == Direction.TopRight && player.Position.X == p.Position.X - 1 && player.Position.Y == p.Position.Y)) ||
                    (player.Direction == Direction.BottomRight && livePlayers.Any(p => p.Direction == Direction.BottomLeft && player.Position.X == p.Position.X + 1 && player.Position.Y == p.Position.Y)) ||
                    (player.Direction == Direction.BottomLeft && livePlayers.Any(p => p.Direction == Direction.BottomRight && player.Position.X == p.Position.X - 1 && player.Position.Y == p.Position.Y)) ||

                    (player.Direction == Direction.TopRight && livePlayers.Any(p => p.Direction == Direction.BottomRight && player.Position.X == p.Position.X && player.Position.Y == p.Position.Y - 1)) ||
                    (player.Direction == Direction.BottomRight && livePlayers.Any(p => p.Direction == Direction.TopRight && player.Position.X == p.Position.X && player.Position.Y == p.Position.Y + 1)) ||
                    (player.Direction == Direction.TopLeft && livePlayers.Any(p => p.Direction == Direction.BottomLeft && player.Position.X == p.Position.X && player.Position.Y == p.Position.Y - 1)) ||
                    (player.Direction == Direction.BottomLeft && livePlayers.Any(p => p.Direction == Direction.TopLeft && player.Position.X == p.Position.X && player.Position.Y == p.Position.Y + 1)))
                {
                    if (player.State == PlayerState.Playing)
                        player.State = PlayerState.HeadToHeadCrossCollision;
                }
            }

            // zapsání tahu přeživších hadů do hracího pole
            livePlayers = _players.Where(p => p.State == PlayerState.Playing).ToList();
            foreach (Player player in livePlayers)
            {
                _gameSurround[player.Position.X, player.Position.Y] = player.Identifier;
            }

            // zapsání tahu společného pole kolize hlav
            foreach (Position position in headToHeadCrashes)
            {
                _gameSurround[position.X, position.Y] = HeadToHeadCrashId;
            }

            if (!_players.Any(p => p.State == PlayerState.Playing))
            {
                _gameOver = true;
            }

            return _gameSurround;
        }

        private bool IsEmpty(int x, int y)
        {
            return (x >= 0 && x < Size && y >= 0 && y < Size) && (_gameSurround[x, y] == 0);
        }

        public Color GetColorForIdentificator(int id)
        {
            if (id == HeadToHeadCrashId)
                return Colors.Magenta;

            var player = _players.Where(p => p.Identifier == id).FirstOrDefault();
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
                stringBuilder.AppendLine(String.Format("Jméno: {0}, Skóre: {1}, Smrt: {2}, Barva: {3}", player.Name, player.Score, player.State, player.Color));
            }
            return stringBuilder.ToString();
        }

        public bool GameOver { get { return _gameOver; } }
    }
}
