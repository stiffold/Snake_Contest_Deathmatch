using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;


namespace SnakeDeathmatch.Players.Jardik
{
    public class Jardik : IPlayerBehavior
    {
        private Direction _myFirstDirection;

        Direction _myDirection;
        Position _myPosition = new Position(1, 1);
        Planner _planner;

        private int _myID;
        private int _round;
        private int _max = 100;

        private List<Walk> _myPlanedMoves = new List<Walk>();
        public void Init(int direction, int identificator)
        {
            _myFirstDirection = (Direction)direction;
            _myID = identificator;
            _round = 0;            
        }

        public int NextMove(int[,] gameSurrond)
        {
            _round++;
            InitOrUpdateMyState(gameSurrond);

            if (!_myPlanedMoves.Any(s=>s.Round ==_round))
            {
                _myPlanedMoves.AddRange(_planner.GetBestWalksToMe(_round, _myPosition, _myDirection, gameSurrond));
            }
            else
            {
                _planner.RepairSteps(_round,_myPlanedMoves, gameSurrond);
            }

                      
            var nextMove = _myPlanedMoves.Where(x => x.Round == _round).FirstOrDefault();
            if (nextMove!=null)
            {
                return (int)nextMove.Move;
            }
            else
            {
                return (int)Move.Straight;
            }
        }

        private void InitOrUpdateMyState(int[,] gameSurrond)
        {
            if (_round == 1)
            {
                _myDirection = _myFirstDirection;
                _max = (int)Math.Sqrt(gameSurrond.Length);
                _planner = new Planner(_max, _myID);

                for (int x = 0; x < _max; x++)
                {
                    for (int y = 0; y < _max; y++)
                    {
                        if (gameSurrond[x, y] == _myID)
                        {
                            _myPosition = new Position(x, y);
                        }
                    }
                }
            }
            else
            {
                var lastMove = _myPlanedMoves.OrderByDescending(m => m.Round).First();
                _myPosition = lastMove.Position;
                _myDirection = lastMove.Direction;
            }            
        }

        public string MyName()
        {
            return "Jardík";
        }      
    }

       
}
