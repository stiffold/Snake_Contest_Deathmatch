using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;


namespace SnakeDeathmatch.Players.Jardik
{
    public class Jardik : IPlayerBehaviour2
    {
        Direction _myDirection;
        Position _myPosition;
        Planner _planner;
        Others _others;

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
            _others = new Others();
        }

        public SnakeDeathmatch.Interface.Move GetNextMove(int[,] gameSurrond)
        {
            _round++;
            _others.Update(gameSurrond, _round);
            _myPlanedMoves = _planner.UpdateMoves(_myPlanedMoves, _round, _myPosition, _myDirection, gameSurrond, _others.OthersList.Where(x => x.Live).ToList());
                      
            var nextMove = _myPlanedMoves.FirstOrDefault(x => x.Round == _round);
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

        [ToDebug]
        public Planner Planner { get { return _planner; } }

        
    }

       
}
