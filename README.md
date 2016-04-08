# Snake_Contest_Deathmatch
Snake contest c#
Soubory ReSharperu a obsah Debug složky by měl být ignorován při commitu
## Jak se přípojit
#####1. Vytvořit složku/namespace pro nového hada v projektu SnakeDeathmatch/Players

#####2. Vytvořit class, která se bude jmenovat stejně jako had a bude realizovat interface IPlayerBehaviour2
```
 public class MySnake : IPlayerBehaviour2
    {
        private Point _myPoint;
        private Direction _myDirection;
        
        public string Name {
            get { return "MySnake"; }
        }

        public Move GetNextMove(int[,] playground)
        {
            return Move.Right;
        }

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _myPoint = new Point(x,y);
            _myDirection = direction;
        }
    }
```
#####3. Přidat hráče na hrací plochu v projektu NewGameUI MainForm.cs v metodě GetPlayers()
```
private IEnumerable<Player> GetPlayers()
        {
            var players = new List<Player>();
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.DeepPink, new SnakeDeathmatch.Players.MySnake.MySnake(), (int)PlayerId.MySnake, PlaygroundSizeInDots));
            return players;
        }
```
