using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.ClockworkMole;

namespace SnakeDeathMatchTest.Players.ClockworkMole
{
    [TestClass]
    public class FloodStrategyTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var playGround = new Playground(20, 6);
            playGround.CurrentArray = new int[20, 20];

            //sloupec 10, radek 10-6, nakresli caru
            playGround.CurrentArray.SelectItemsAndEnumerate()
                .Where(x => x.Y >= 10 && x.Y <= 16 && x.X == 10)
                .Apply(x => x.Value = 6).ToArray();

            // nastavit hlavu smerem vzhuru na zacatek cary
            Position position = new Position(10, 10, Direction.Top);

            var sut = new FloodStrategy();
            sut.GetNextMove(playGround, position);


        }

        [TestMethod]
        public void TestMethodPast()
        {
            var playGround = new Playground(20, 6);
            playGround.CurrentArray = new int[20, 20];

            // 5,2 -> 5,4
            playGround.CurrentArray.SelectItemsAndEnumerate()
                .Where(item => item.Y >= 2 && item.Y <= 4)
                .Where(item => item.X == 5)
                .Apply(x => x.Value = 1).Execute();

            // 5,10 -> 5,12
            playGround.CurrentArray.SelectItemsAndEnumerate()
                .Where(item => item.Y >= 10 && item.Y <= 12)
                .Where(item => item.X == 5)
                .Apply(x => x.Value = 1).Execute();

            // 15,6 -> 15,8
            playGround.CurrentArray.SelectItemsAndEnumerate()
                .Where(item => item.Y >= 6 && item.Y <= 8)
                .Where(item => item.X == 15)
                .Apply(x => x.Value = 1).Execute();


            //6,5 -> 14,5
            playGround.CurrentArray.SelectItemsAndEnumerate()
                .Where(x => x.Y == 5 && x.X >= 6 && x.X <= 14)
                .Apply(x => x.Value = 1).Execute();

            //6,9 -> 14,9
            playGround.CurrentArray.SelectItemsAndEnumerate()
                .Where(item => item.Y == 9)
                .Where(item => item.X >= 6 && item.X <= 14)
                .Apply(x => x.Value = 1).Execute();


            Position position = new Position(6, 6, Direction.BottomRight);
            playGround.CurrentArray[5, 5] = 6;
            playGround.CurrentArray[6, 6] = 6;

            //ocas
            playGround.CurrentArray.SelectItemsAndEnumerate()
                .Where(item => item.Y >= 2 && item.Y <= 4)
                .Where(item => item.X == 4)
                .Apply(x => x.Value = 6).Execute();

            var sut = new FloodStrategy();
            sut.GetNextMove(playGround, position);


        }

    }
}
