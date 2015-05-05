using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Game;
using SnakeDeathmatch.Players.Fools;
using SnakeDeathmatch.Players.Vazba;

namespace SnakeDeathmatch.Tests
{
    public class CollisionTests
    {
        private const int PlaygroundSize = 10;
        
        private int _nextIdentifier = 1;

        private Player CreateStraightPlayer(Color color, int x, int y, Direction direction)
        {
            return new Player(new Position(x, y), direction, color, new StraightPlayer(direction), _nextIdentifier++, PlaygroundSize);
        }

        private Player CreatePlayer<TPlayerBehaviour>(Color color, int x, int y, Direction direction)
            where TPlayerBehaviour : IPlayerBehaviour2, new()
        {
            return new Player(new Position(x, y), direction, color, new TPlayerBehaviour(), _nextIdentifier++, PlaygroundSize);
        }


        public GameEngine Test01_BoundaryCollision_Top()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 4, 3, Direction.Top),
            });
        }

        public GameEngine Test02_BoundaryCollision_Bottom()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 5, 6, Direction.Bottom),
            });
        }

        public GameEngine Test03_BoundaryCollision_Left()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 3, 4, Direction.Left),
            });
        }

        public GameEngine Test04_BoundaryCollision_Right()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 6, 5, Direction.Right),
            });
        }

        public GameEngine Test05_HeadToTailCollision_Top()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 6, 7, Direction.Top),
                CreateStraightPlayer(Colors.Lime, 4, 3, Direction.Right),
            });
        }

        public GameEngine Test06_HeadToTailCollision_Bottom()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 6, 3, Direction.Bottom),
                CreateStraightPlayer(Colors.Lime, 4, 7, Direction.Right),
            });
        }

        public GameEngine Test07_HeadToTailCollision_Right()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 2, 6, Direction.Right),
                CreateStraightPlayer(Colors.Lime, 6, 4, Direction.Bottom),
            });
        }

        public GameEngine Test08_HeadToTailCollision_Left()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 7, 6, Direction.Left),
                CreateStraightPlayer(Colors.Lime, 3, 4, Direction.Bottom),
            });
        }

        public GameEngine Test09_HeadToTailCollision_Right_vs_Left()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 2, 4, Direction.Right),
                CreateStraightPlayer(Colors.DarkRed, 7, 4, Direction.Left),
            });
        }

        public GameEngine Test10_HeadToTailCollision_Bottom_vs_Top()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 4, 2, Direction.Bottom),
                CreateStraightPlayer(Colors.DarkRed, 4, 7, Direction.Top),
            });
        }

        public GameEngine Test11_HeadToTailCrossCollision_TopRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 1, 7, Direction.TopRight),
                CreateStraightPlayer(Colors.Lime, 6, 5, Direction.TopLeft),
            });
        }

        public GameEngine Test12_HeadToTailCrossCollision_TopLeft()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 6, 7, Direction.TopLeft),
                CreateStraightPlayer(Colors.Lime, 1, 5, Direction.TopRight),
            });
        }

        public GameEngine Test13_HeadToTailCrossCollision_BottomRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 1, 2, Direction.BottomRight),
                CreateStraightPlayer(Colors.Lime, 6, 4, Direction.BottomLeft),
            });
        }

        public GameEngine Test14_HeadToTailCrossCollision_BottomLeft()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 6, 2, Direction.BottomLeft),
                CreateStraightPlayer(Colors.Lime, 1, 4, Direction.BottomRight),
            });
        }

        public GameEngine Test15_HeadToHeadCollision_TopLeft_x_TopRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 3, 6, Direction.TopRight),
                CreateStraightPlayer(Colors.DarkRed, 7, 6, Direction.TopLeft),
            });
        }

        public GameEngine Test16_HeadToHeadCollision_BottomLeft_x_BottomRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 3, 4, Direction.BottomRight),
                CreateStraightPlayer(Colors.DarkRed, 7, 4, Direction.BottomLeft),
            });
        }

        public GameEngine Test17_HeadToHeadCollision_TopLeft_x_BottomLeft()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 6, 7, Direction.TopLeft),
                CreateStraightPlayer(Colors.DarkRed, 6, 3, Direction.BottomLeft),
            });
        }

        public GameEngine Test18_HeadToHeadCollision_TopRight_x_BottomRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 4, 7, Direction.TopRight),
                CreateStraightPlayer(Colors.DarkRed, 4, 3, Direction.BottomRight),
            });
        }

        public GameEngine Test19_HeadToHeadCrossCollision_TopLeft_x_TopRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 2, 6, Direction.TopRight),
                CreateStraightPlayer(Colors.DarkRed, 7, 6, Direction.TopLeft),
            });
        }

        public GameEngine Test20_HeadToHeadCrossCollision_BottomLeft_x_BottomRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 2, 4, Direction.BottomRight),
                CreateStraightPlayer(Colors.DarkRed, 7, 4, Direction.BottomLeft),
            });
        }

        public GameEngine Test21_HeadToHeadCrossCollision_TopLeft_x_BottomLeft()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 6, 7, Direction.TopLeft),
                CreateStraightPlayer(Colors.DarkRed, 6, 2, Direction.BottomLeft),
            });
        }

        public GameEngine Test22_HeadToHeadCrossCollision_TopRight_x_BottomRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 4, 7, Direction.TopRight),
                CreateStraightPlayer(Colors.DarkRed, 4, 2, Direction.BottomRight),
            });
        }

        public GameEngine Test23_HeadToHeadCollision_and_HeadToTailCrossCollision()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 6, 3, Direction.BottomLeft),
                CreateStraightPlayer(Colors.DarkRed, 6, 7, Direction.TopLeft),
                CreateStraightPlayer(Colors.Orange, 3, 7, Direction.TopRight),
            });
        }

        public GameEngine Test24_HeadToHeadCollision_and_HeadToTailCrossCollision_BigBang()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 1, 1, Direction.BottomRight),
                CreateStraightPlayer(Colors.DarkRed, 4, 1, Direction.Bottom),
                CreateStraightPlayer(Colors.Orange, 7, 1, Direction.BottomLeft),
                CreateStraightPlayer(Colors.DarkOrange, 7, 4, Direction.Left),
                CreateStraightPlayer(Colors.Pink, 7, 7, Direction.TopLeft),
                CreateStraightPlayer(Colors.LightPink, 4, 7, Direction.Top),
                CreateStraightPlayer(Colors.MediumVioletRed, 1, 7, Direction.TopRight),
                CreateStraightPlayer(Colors.Violet, 1, 4, Direction.Right)
            });
        }

        public GameEngine Test00()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreateStraightPlayer(Colors.Red, 0, 4, Direction.Right),
                CreatePlayer<VazbaPlayer>(Colors.Blue, 6, 3, Direction.TopLeft),
            });
        }
    }
}
