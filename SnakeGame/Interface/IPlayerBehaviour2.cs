namespace SnakeGame.Interface
{
    public interface IPlayerBehaviour2
    {
        void Init(int playerId, int playgroundSize, int x, int y, Direction direction);
        Move GetNextMove(int[,] playground);
        string Name { get; }
    }
}
