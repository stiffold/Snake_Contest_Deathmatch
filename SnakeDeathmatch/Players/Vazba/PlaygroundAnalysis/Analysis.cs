using SnakeDeathmatch.Debugger;

namespace SnakeDeathmatch.Players.Vazba.PlaygroundAnalysis
{
    public class Analysis
    {
        [ToDebug]
        public int PlaygroundSize { get; private set; }

        [ToDebug]
        public DangerZone DangerZone { get; private set; }

        [ToDebug]
        public DeathField DeathField { get; private set; }

        public Analysis(int playgroundSize)
        {
            PlaygroundSize = playgroundSize;
            DangerZone = new DangerZone(playgroundSize, maxDepth: 5);
            DeathField = new DeathField(playgroundSize);
        }

        public void Update(Snakes snakes)
        {
            DangerZone.Update(snakes);
            DeathField.Update(snakes);
        }
    }
}
