using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using Deathmatch;
using Snake2.enums;
using Snake2.factories;
using Snake2.players;
using Direction = Snake2.enums.Direction;

namespace Snake2.game
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

            Player player1 = playerFactory.Create<TestPlayerUI>(Colors.Red);
            //TestPlayerUI test = new TestPlayerUI();
            //Player player1 = new Player(new Position(0, 0), Direction.Bottom, Colors.Red, test, 1);
            //_gameSurround[player1.Position.X, player1.Position.Y] = player1.Identificator;        
            _players.Add(player1);

            //TestPlayer test2 = new TestPlayer();
            //Player player2 = new Player(new Position(49, 0), Direction.BottomLeft, Colors.Blue, test2, 2);
            //_gameSurround[player2.Position.X, player2.Position.Y] = player2.Identificator;
            //_players.Add(player2);

            Player player3 = playerFactory.Create<Vazba>(Colors.Blue);
            //Vazba test3 = new Vazba();
            //Player player3 = new Player(new Position(25, 25), Direction.TopLeft, Colors.Blue, test3, 3);
            //_gameSurround[player3.Position.X, player3.Position.Y] = player3.Identificator;
            _players.Add(player3);

            //TestPlayerUI test4 = new TestPlayerUI();
            //Player player4 = new Player(new Position(0, 49), Direction.TopRight, Colors.Yellow, test4, 4);
            //_gameSurround[player4.Position.X, player4.Position.Y] = player4.Identificator;
            //_players.Add(player4);

            Player player5 = playerFactory.Create<TestPlayer>(Colors.White);
            //TestPlayer test5 = new TestPlayer();
            //Player player5 = new Player(new Position(50, 50), Direction.TopRight, Colors.White, test5, 5);
            //_gameSurround[player5.Position.X, player5.Position.Y] = player5.Identificator;
            _players.Add(player5);
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
                if (CrossColision(player))
                {
                    player.State = PlayerState.CrossCollission;
                    break;
                }

                //vlastní kolize v tahu
                if (_players.Any(p => player.Position.X == p.Position.X && player.Position.Y == p.Position.Y && p.Identificator != player.Identificator))
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

            if (!_players.Any(p => p.State == PlayerState.Playing))
            {
                _gameOver = true;
            }

            return _gameSurround;
        }

        private bool CrossColision(Player player)
        {
            if (player.Position.X == 0 || player.Position.Y == 0 || player.Position.X == _max - 1 || player.Position.Y == _max - 1)
            {
                return false;
            }

            switch (player.MyDirection)
            {
                case Direction.TopRight: return !((_gameSurround[player.Position.X, player.Position.Y + 1] == 0) && (_gameSurround[player.Position.X - 1, player.Position.Y] == 0));

                case Direction.BottomRight: return !((_gameSurround[player.Position.X, player.Position.Y - 1] == 0) && (_gameSurround[player.Position.X - 1, player.Position.Y] == 0));

                case Direction.BottomLeft: return !((_gameSurround[player.Position.X, player.Position.Y - 1] == 0) && (_gameSurround[player.Position.X + 1, player.Position.Y] == 0));

                case Direction.TopLeft: return !((_gameSurround[player.Position.X, player.Position.Y + 1] == 0) && (_gameSurround[player.Position.X + 1, player.Position.Y] == 0));

            }
            return false;
        }

        public Color GetColorForIdentificator(int i)
        {
            var player = _players.Where(p => p.Identificator == i).FirstOrDefault();
            if (player != null)
            {
                return player.Color;
            }
            return Colors.Aqua;
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
