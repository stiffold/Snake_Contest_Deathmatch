# Snake_Contest_Deathmatch
Jedná se o souboj algoritmů chování jednotlivých hadů. Kdo zůstane nejdéle vyhrál.
## Pravidla hry
- zatáčí se o 45° (o 180° se had otočí na 4 tahy)
- hrací plocha je 100x100
- na začátku hry se had objeví na náhodné pozici s náhodným směrem
- detekují se tyto typy kolize
 - kolize s hracím polem
 - kolize s hadem kolmo i diagonálně
 - kolize hlav - nerozhodně pro oba hady

## Jak se připojit
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
##Ostatní
Soubory ReSharperu a obsah Debug složky by měl být ignorován při commitu
