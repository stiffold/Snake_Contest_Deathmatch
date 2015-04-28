namespace SnakeDeathmatch.Interface
{
    public interface IPlayerBehaviour2
    {
        void Init(int identifier, int playgroundSize, int x, int y, Direction direction);
        Move GetNextMove(int[,] playground);
        string Name { get; }
    }
}
