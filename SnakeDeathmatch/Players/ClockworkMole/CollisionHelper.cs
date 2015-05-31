using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.ClockworkMole
{
    public static class CollisionHelper
    {
        public static bool IsCollision(Playground playground, Position position, Move move)
        {

            var newPotentialPosition = position.Move(move);

            // boundary collision
            if (newPotentialPosition.X < 0 || newPotentialPosition.Y < 0)
                return true;
            if (newPotentialPosition.X >= playground._playgroundsize || newPotentialPosition.Y >= playground._playgroundsize)
                return true;

            if (playground.TestCollissionArray[newPotentialPosition.X, newPotentialPosition.Y] != 0)
                return true;

            //croscollision
            if (newPotentialPosition.Direction == Direction.BottomLeft
                || newPotentialPosition.Direction == Direction.BottomRight
                || newPotentialPosition.Direction == Direction.TopLeft
                || newPotentialPosition.Direction == Direction.TopRight)

                if (playground.TestCollissionArray[newPotentialPosition.X, position.Y] != 0 && playground.TestCollissionArray[position.X, newPotentialPosition.Y] != 0)
                    return true;

            return false;
        }


    }
}
