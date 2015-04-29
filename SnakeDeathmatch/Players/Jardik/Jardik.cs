using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;


namespace SnakeDeathmatch.Players.Jardik
{
    public class Jardik : IPlayerBehaviour2
    {
        Direction _myDirection;
        Position _myPosition;
        Planner _planner;

        private int _myID;
        private int _round;
        private int _max = 100;

        private List<Walk> _myPlanedMoves = new List<Walk>();
        public void Init(int identificator, int max, int x, int y, SnakeDeathmatch.Interface.Direction direction)
        {
            _max = max;
            _myDirection = (Direction)direction;
            _myPosition = new Position(x, y);
            _myID = identificator;
            _round = 0;
            _planner = new Planner(_max, _myID);
        }

        public SnakeDeathmatch.Interface.Move GetNextMove(int[,] gameSurrond)
        {
            _round++;
            if (!_myPlanedMoves.Any(s=>s.Round ==_round))
            {
                _myPlanedMoves.AddRange(_planner.GetBestWalksToMe(_round, _myPosition, _myDirection, gameSurrond));
                //_myPlanedMoves.AddRange(_planner.GetVariant(_round, _myPosition, _myDirection, gameSurrond,WalkSetType.Quaker));
            }
            else
            {
                _planner.RepairSteps(_round+1,_myPlanedMoves, gameSurrond);
            }

                      
            var nextMove = _myPlanedMoves.Where(x => x.Round == _round).FirstOrDefault();
            if (nextMove!=null)
            {
                _myDirection = _myDirection.GetNewDirection(nextMove.Move);
                _myPosition.Update(_myDirection);
                return (SnakeDeathmatch.Interface.Move)nextMove.Move;                
            }
            else
            {
                return (SnakeDeathmatch.Interface.Move)Move.Straight;
            }
        }

        public string Name { get { return "Jardík"; } }
    }

       
}
