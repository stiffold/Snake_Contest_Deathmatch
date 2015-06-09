using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.ClockworkMole
{
    public static class CollisionHelper
    {
        public static bool IsCollision(Playground playground, Position position, Move move)
        {
            var newPotentialPosition = position.Move(move);

            return IsCollision(playground, position, newPotentialPosition);
        }

        public static bool IsCollision(Playground playground, Position position, Position newPosition)
        {
            // boundary collision
            if (newPosition.X < 0 || newPosition.Y < 0)
                return true;
            if (newPosition.X >= playground._playgroundsize || newPosition.Y >= playground._playgroundsize)
                return true;

            if (playground.TestCollissionArray[newPosition.X, newPosition.Y] != 0)
                return true;

            //croscollision
            if (newPosition.Direction == Direction.BottomLeft
                || newPosition.Direction == Direction.BottomRight
                || newPosition.Direction == Direction.TopLeft
                || newPosition.Direction == Direction.TopRight)

                if (playground.TestCollissionArray[newPosition.X, position.Y] != 0 &&
                    playground.TestCollissionArray[position.X, newPosition.Y] != 0)
                    return true;

            if (position.X != newPosition.X && position.Y != newPosition.Y)
            {
                if (playground.TestCollissionArray[newPosition.X, position.Y] != 0 && playground.TestCollissionArray[position.X, newPosition.Y] != 0)
                    return true;
            }

            return false;
        }


    }
}
