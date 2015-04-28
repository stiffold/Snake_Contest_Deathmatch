using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Game;

namespace SnakeDeathmatch.Tests
{
    public class CollisionTests
    {
        private const int PlaygroundSize = 10;
        
        private int _nextIdentifier = 1;

        private Player CreatePlayer<TPlayerBehaviour>(Color color, int x, int y, Direction direction)
            where TPlayerBehaviour : IPlayerBehavior, new()
        {
            return new Player(new Position(x, y), direction, color, new TPlayerBehaviour(), _nextIdentifier++, PlaygroundSize);
        }


        public GameEngine Test01_BoundaryCollision_Top()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopPlayer>(Colors.Red, 4, 3, Direction.Top),
            });
        }

        public GameEngine Test02_BoundaryCollision_Bottom()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomPlayer>(Colors.Red, 5, 6, Direction.Bottom),
            });
        }

        public GameEngine Test03_BoundaryCollision_Left()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.LeftPlayer>(Colors.Red, 3, 4, Direction.Left),
            });
        }

        public GameEngine Test04_BoundaryCollision_Right()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.RightPlayer>(Colors.Red, 6, 5, Direction.Right),
            });
        }

        public GameEngine Test05_HeadToTailCollision_Top()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopPlayer>(Colors.Red, 6, 7, Direction.Top),
                CreatePlayer<SnakeDeathmatch.Players.Fools.RightPlayer>(Colors.Lime, 4, 3, Direction.Right),
            });
        }

        public GameEngine Test06_HeadToTailCollision_Bottom()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomPlayer>(Colors.Red, 6, 3, Direction.Bottom),
                CreatePlayer<SnakeDeathmatch.Players.Fools.RightPlayer>(Colors.Lime, 4, 7, Direction.Right),
            });
        }

        public GameEngine Test07_HeadToTailCollision_Right()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.RightPlayer>(Colors.Red, 2, 6, Direction.Right),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomPlayer>(Colors.Lime, 6, 4, Direction.Bottom),
            });
        }

        public GameEngine Test08_HeadToTailCollision_Left()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.LeftPlayer>(Colors.Red, 7, 6, Direction.Left),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomPlayer>(Colors.Lime, 3, 4, Direction.Bottom),
            });
        }

        public GameEngine Test09_HeadToTailCollision_Right_vs_Left()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.RightPlayer>(Colors.Red, 2, 4, Direction.Right),
                CreatePlayer<SnakeDeathmatch.Players.Fools.LeftPlayer>(Colors.DarkRed, 7, 4, Direction.Left),
            });
        }

        public GameEngine Test10_HeadToTailCollision_Bottom_vs_Top()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomPlayer>(Colors.Red, 4, 2, Direction.Bottom),
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopPlayer>(Colors.DarkRed, 4, 7, Direction.Top),
            });
        }

        public GameEngine Test11_HeadToTailCrossCollision_TopRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopRightPlayer>(Colors.Red, 1, 7, Direction.TopRight),
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopLeftPlayer>(Colors.Lime, 6, 5, Direction.TopLeft),
            });
        }

        public GameEngine Test12_HeadToTailCrossCollision_TopLeft()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopLeftPlayer>(Colors.Red, 6, 7, Direction.TopLeft),
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopRightPlayer>(Colors.Lime, 1, 5, Direction.TopRight),
            });
        }

        public GameEngine Test13_HeadToTailCrossCollision_BottomRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomRightPlayer>(Colors.Red, 1, 2, Direction.BottomRight),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomLeftPlayer>(Colors.Lime, 6, 4, Direction.BottomLeft),
            });
        }

        public GameEngine Test14_HeadToTailCrossCollision_BottomLeft()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomLeftPlayer>(Colors.Red, 6, 2, Direction.BottomLeft),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomRightPlayer>(Colors.Lime, 1, 4, Direction.BottomRight),
            });
        }

        public GameEngine Test15_HeadToHeadCollision_TopLeft_x_TopRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopRightPlayer>(Colors.Red, 3, 6, Direction.TopRight),
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopLeftPlayer>(Colors.DarkRed, 7, 6, Direction.TopLeft),
            });
        }

        public GameEngine Test16_HeadToHeadCollision_BottomLeft_x_BottomRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomRightPlayer>(Colors.Red, 3, 4, Direction.BottomRight),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomLeftPlayer>(Colors.DarkRed, 7, 4, Direction.BottomLeft),
            });
        }

        public GameEngine Test17_HeadToHeadCollision_TopLeft_x_BottomLeft()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopLeftPlayer>(Colors.Red, 6, 7, Direction.TopLeft),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomLeftPlayer>(Colors.DarkRed, 6, 3, Direction.BottomLeft),
            });
        }

        public GameEngine Test18_HeadToHeadCollision_TopRight_x_BottomRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopRightPlayer>(Colors.Red, 4, 7, Direction.TopRight),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomRightPlayer>(Colors.DarkRed, 4, 3, Direction.BottomRight),
            });
        }

        public GameEngine Test19_HeadToHeadCrossCollision_TopLeft_x_TopRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopRightPlayer>(Colors.Red, 2, 6, Direction.TopRight),
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopLeftPlayer>(Colors.DarkRed, 7, 6, Direction.TopLeft),
            });
        }

        public GameEngine Test20_HeadToHeadCrossCollision_BottomLeft_x_BottomRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomRightPlayer>(Colors.Red, 2, 4, Direction.BottomRight),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomLeftPlayer>(Colors.DarkRed, 7, 4, Direction.BottomLeft),
            });
        }

        public GameEngine Test21_HeadToHeadCrossCollision_TopLeft_x_BottomLeft()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopLeftPlayer>(Colors.Red, 6, 7, Direction.TopLeft),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomLeftPlayer>(Colors.DarkRed, 6, 2, Direction.BottomLeft),
            });
        }

        public GameEngine Test22_HeadToHeadCrossCollision_TopRight_x_BottomRight()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopRightPlayer>(Colors.Red, 4, 7, Direction.TopRight),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomRightPlayer>(Colors.DarkRed, 4, 2, Direction.BottomRight),
            });
        }

        public GameEngine Test23_HeadToHeadCollision_and_HeadToTailCrossCollision()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomLeftPlayer>(Colors.Red, 6, 3, Direction.BottomLeft),
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopLeftPlayer>(Colors.DarkRed, 6, 7, Direction.TopLeft),
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopRightPlayer>(Colors.Orange, 3, 7, Direction.TopRight),
            });
        }

        public GameEngine Test24_HeadToHeadCollision_and_HeadToTailCrossCollision_BigBang()
        {
            return new GameEngine(PlaygroundSize, new[]
            {
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomRightPlayer>(Colors.Red, 1, 1, Direction.BottomRight),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomPlayer>(Colors.DarkRed, 4, 1, Direction.Bottom),
                CreatePlayer<SnakeDeathmatch.Players.Fools.BottomLeftPlayer>(Colors.Orange, 7, 1, Direction.BottomLeft),
                CreatePlayer<SnakeDeathmatch.Players.Fools.LeftPlayer>(Colors.DarkOrange, 7, 4, Direction.Left),
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopLeftPlayer>(Colors.Pink, 7, 7, Direction.TopLeft),
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopPlayer>(Colors.LightPink, 4, 7, Direction.Top),
                CreatePlayer<SnakeDeathmatch.Players.Fools.TopRightPlayer>(Colors.MediumVioletRed, 1, 7, Direction.TopRight),
                CreatePlayer<SnakeDeathmatch.Players.Fools.RightPlayer>(Colors.Violet, 1, 4, Direction.Right)
            });
        }
    }
}
