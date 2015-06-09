using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.SoulEater;
using SnakeDeathmatch.Players.SoulEater.MK2;
using SoulEaterTest.Infrastructure;

namespace SoulEaterTest
{
    [TestClass]
    public class GameGroundTestClass
    {
        /*
 00 10 20 30 40
 01 11 21 31 41
 02 12 22 32 42
 03 13 23 33 43
 04 14 24 34 44            
 */

        [TestMethod]
        public void GameGroundInitTest()
        {
            GameGroundMK2 gameground = new GameGroundMK2(3,5);

            gameground.Init();

            /*
             00 10 20
             01 11 21
             02 12 22
             */

            gameground[0, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight, Direction.Left, Direction.BottomLeft).IsUnused();
            gameground[0, 1].AllPathsAreValidExcept(Direction.Left, Direction.BottomLeft, Direction.TopLeft).IsUnused();
            gameground[0, 2].AllPathsAreValidExcept(Direction.Left, Direction.TopLeft, Direction.BottomLeft, Direction.Bottom, Direction.BottomRight).IsUnused();

            gameground[1, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight).IsUnused();
            gameground[1, 1].AllPathsAreValidExcept().IsUnused();
            gameground[1, 2].AllPathsAreValidExcept(Direction.BottomLeft, Direction.Bottom, Direction.BottomRight).IsUnused();

            gameground[2, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight, Direction.Right, Direction.BottomRight).IsUnused();
            gameground[2, 1].AllPathsAreValidExcept(Direction.TopRight, Direction.Right, Direction.BottomRight).IsUnused();
            gameground[2, 2].AllPathsAreValidExcept(Direction.TopRight, Direction.Right, Direction.BottomRight, Direction.BottomLeft, Direction.Bottom).IsUnused();

            #region assert cest [1,1]
            foreach (var path in gameground[1, 1].PathsFromPoint)
            {
                Assert.IsTrue(path.Direction == path.OpositePath.Direction.GetOpositeDirection());
                Assert.IsTrue(path.OpositePath.PointTo == gameground[1, 1]);

                if (path.Direction == Direction.Bottom)
                {
                    Assert.AreEqual(path.PointTo, gameground[1, 2]);
                }
                else if (path.Direction == Direction.BottomLeft)
                {
                    Assert.AreEqual(path.PointTo, gameground[0, 2]);
                }
                else if (path.Direction == Direction.BottomRight)
                {
                    Assert.AreEqual(path.PointTo, gameground[2, 2]);
                }
                else if (path.Direction == Direction.Left)
                {
                    Assert.AreEqual(path.PointTo, gameground[0, 1]);
                }
                else if (path.Direction == Direction.Right)
                {
                    Assert.AreEqual(path.PointTo, gameground[2, 1]);
                }
                else if (path.Direction == Direction.Top)
                {
                    Assert.AreEqual(path.PointTo, gameground[1, 0]);
                }
                else if (path.Direction == Direction.TopLeft)
                {
                    Assert.AreEqual(path.PointTo, gameground[0, 0]);
                }
                else if (path.Direction == Direction.TopRight)
                {
                    Assert.AreEqual(path.PointTo, gameground[2, 0]);
                }
            }
            #endregion

            foreach (var path in gameground[0, 1].PathsFromPoint)
            {
                if (path.Direction == Direction.Bottom)
                {
                    Assert.AreEqual(path.PointTo, gameground[0, 2]);
                    Assert.IsTrue(path.Direction == path.OpositePath.Direction.GetOpositeDirection());
                    Assert.IsTrue(path.OpositePath.PointTo == gameground[0, 1]);
                }
                else if (path.Direction == Direction.BottomLeft)
                {
                    Assert.IsTrue(path.PointTo.IsUsed);
                }
                else if (path.Direction == Direction.BottomRight)
                {
                    Assert.AreEqual(path.PointTo, gameground[1, 2]);
                    Assert.IsTrue(path.Direction == path.OpositePath.Direction.GetOpositeDirection());
                    Assert.IsTrue(path.OpositePath.PointTo == gameground[0, 1]);
                }
                else if (path.Direction == Direction.Left)
                {
                    Assert.IsTrue(path.PointTo.IsUsed);
                }
                else if (path.Direction == Direction.Right)
                {
                    Assert.AreEqual(path.PointTo, gameground[1, 1]);
                    Assert.IsTrue(path.Direction == path.OpositePath.Direction.GetOpositeDirection());
                    Assert.IsTrue(path.OpositePath.PointTo == gameground[0, 1]);
                }
                else if (path.Direction == Direction.Top)
                {
                    Assert.AreEqual(path.PointTo, gameground[0, 0]);
                    Assert.IsTrue(path.Direction == path.OpositePath.Direction.GetOpositeDirection());
                    Assert.IsTrue(path.OpositePath.PointTo == gameground[0, 1]);
                }
                else if (path.Direction == Direction.TopLeft)
                {
                    Assert.IsTrue(path.PointTo.IsUsed);
                }
                else if (path.Direction == Direction.TopRight)
                {
                    Assert.AreEqual(path.PointTo, gameground[1, 0]);
                    Assert.IsTrue(path.Direction == path.OpositePath.Direction.GetOpositeDirection());
                    Assert.IsTrue(path.OpositePath.PointTo == gameground[0, 1]);
                }
            }
        }

        [TestMethod]
        public void GameGroundEmptyUpdate()
        {
            GameGroundMK2 gameground = new GameGroundMK2(3,5);

            gameground.Init();

            /*
             00 10 20
             01 11 21
             02 12 22
             */

            gameground.Update(
                new int[3, 3]
                {
                    {0,0,0},
                    {0,0,0},
                    {0,0,0}
                }
                );

            gameground[0, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight, Direction.Left, Direction.BottomLeft).IsUnused();
            gameground[0, 1].AllPathsAreValidExcept(Direction.Left, Direction.BottomLeft, Direction.TopLeft).IsUnused();
            gameground[0, 2].AllPathsAreValidExcept(Direction.Left, Direction.TopLeft, Direction.BottomLeft, Direction.Bottom, Direction.BottomRight).IsUnused();

            gameground[1, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight).IsUnused();
            gameground[1, 1].AllPathsAreValidExcept().IsUnused();
            gameground[1, 2].AllPathsAreValidExcept(Direction.BottomLeft, Direction.Bottom, Direction.BottomRight).IsUnused();

            gameground[2, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight, Direction.Right, Direction.BottomRight).IsUnused();
            gameground[2, 1].AllPathsAreValidExcept(Direction.TopRight, Direction.Right, Direction.BottomRight).IsUnused();
            gameground[2, 2].AllPathsAreValidExcept(Direction.TopRight, Direction.Right, Direction.BottomRight, Direction.BottomLeft, Direction.Bottom).IsUnused();
        }

        [TestMethod]
        public void GameGroundNearBorderUpdate()
        {
            GameGroundMK2 gameground = new GameGroundMK2(3,5);

            gameground.Init();

            /*
             00 10 20
             01 11 21
             02 12 22
             */

            gameground.Update(
                new int[3, 3]
                {
                    {0,0,0},
                    {1,0,0},
                    {0,0,0}
                }.SwapXWithY()
                );


            gameground[0, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight, Direction.Left, Direction.BottomLeft, Direction.Bottom).IsUnused();
            gameground[0, 1].IsUsed();
            gameground[0, 2].AllPathsAreValidExcept(Direction.Left, Direction.BottomLeft, Direction.Bottom, Direction.BottomRight, Direction.Top, Direction.TopLeft).IsUnused();

            gameground[1, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight, Direction.BottomLeft).IsUnused();
            gameground[1, 1].AllPathsAreValidExcept(Direction.Left).IsUnused();
            gameground[1, 2].AllPathsAreValidExcept(Direction.BottomLeft, Direction.Bottom, Direction.BottomRight, Direction.TopLeft).IsUnused();

            gameground[2, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight, Direction.Right, Direction.BottomRight).IsUnused();
            gameground[2, 1].AllPathsAreValidExcept(Direction.TopRight, Direction.Right, Direction.BottomRight).IsUnused();
            gameground[2, 2].AllPathsAreValidExcept(Direction.TopRight, Direction.Right, Direction.BottomRight, Direction.BottomLeft, Direction.Bottom).IsUnused();
        }

        [TestMethod]
        public void GameGroundUnsurroundedPointUpdate()
        {
            GameGroundMK2 gameground = new GameGroundMK2(3,5);

            gameground.Init();

            /*
             00 10 20
             01 11 21
             02 12 22
             */

            gameground.Update(
                new int[3, 3]
                {
                    {0,0,0},
                    {0,1,0},
                    {0,0,0}
                }.SwapXWithY()
                );


            gameground[0, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight, Direction.Left, Direction.BottomLeft, Direction.BottomRight).IsUnused();
            gameground[0, 1].AllPathsAreValidExcept(Direction.Left, Direction.BottomLeft, Direction.TopLeft, Direction.Right).IsUnused();
            gameground[0, 2].AllPathsAreValidExcept(Direction.Left, Direction.TopLeft, Direction.BottomLeft, Direction.Bottom, Direction.BottomRight, Direction.TopRight).IsUnused();

            gameground[1, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight, Direction.Bottom).IsUnused();
            gameground[1, 1].IsUsed();
            gameground[1, 2].AllPathsAreValidExcept(Direction.BottomLeft, Direction.Bottom, Direction.BottomRight, Direction.Top).IsUnused();

            gameground[2, 0].AllPathsAreValidExcept(Direction.Top, Direction.TopLeft, Direction.TopRight, Direction.Right, Direction.BottomRight, Direction.BottomLeft).IsUnused();
            gameground[2, 1].AllPathsAreValidExcept(Direction.TopRight, Direction.Right, Direction.BottomRight, Direction.Left).IsUnused();
            gameground[2, 2].AllPathsAreValidExcept(Direction.TopRight, Direction.Right, Direction.BottomRight, Direction.BottomLeft, Direction.Bottom, Direction.TopLeft).IsUnused();
        }


        [TestMethod]
        public void test()
        {
            GameGroundMK2 gameground = new GameGroundMK2(6, 5);

            gameground.Init();

            /*
             00 10 20
             01 11 21
             02 12 22
             */

            gameground.Update(
                new int[6, 6]
                {
                    {1,0,0,0,0,0},
                    {0,0,0,0,0,0},
                    {0,0,0,0,0,0},
                    {0,0,0,0,0,0},
                    {0,0,0,0,0,0},
                    {0,0,0,0,0,0},
                }.SwapXWithY()
                );

            gameground.Update(
                new int[6, 6]
                {
                    {1,0,0,0,0,0},
                    {1,0,0,0,0,0},
                    {0,0,0,0,0,0},
                    {0,0,0,0,0,0},
                    {0,0,0,0,0,0},
                    {0,0,0,0,0,0},
                }.SwapXWithY()
                );

            var strategy = new BasicRecursiveStrategy(5, true);

            var myPoint = gameground[0, 5];
            var myDirection = Direction.Top;

            strategy.GetNextMoveAndUpdateMyNextPositionAndDirection(gameground, ref myPoint,ref myDirection);
        }

        
    }
}
