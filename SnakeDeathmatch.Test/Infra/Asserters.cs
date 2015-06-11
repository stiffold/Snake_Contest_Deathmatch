using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.SoulEater.MK2;

namespace SoulEaterTest.Infrastructure
{
    public static class GameGroundAsserter
    {
        
    }

    public static class PointClassAsserter
    {
        public static PointClass AllPathsAreValidExcept(this PointClass gameGroundPoint, params Direction[] unvalidDirections)
        {

            foreach (var path in gameGroundPoint.PathsFromPoint)
            {
                if (unvalidDirections.Contains(path.Direction))
                {
                    Assert.AreEqual(path.PathState, PathState.DeathInThisRound, string.Format("Bylo očekáváno, že cesta {0} bude nevalidní.", path));
                }
                else
                {
                    Assert.AreEqual(path.PathState, PathState.Ok, string.Format("Bylo očekáváno, že cesta {0} bude validní.", path));
                }
            }

            return gameGroundPoint;
        }

        public static PointClass IsUnused(this PointClass point)
        {
            Assert.IsTrue(point.IsUsed == false, string.Format("Bylo očekáváno, že bod [{0},{1}] nezabraný"));// todo tp

            return point;
        }

        public static PointClass IsUsed(this PointClass point)
        {
            Assert.IsTrue(point.IsUsed, string.Format("Bylo očekáváno, že bod [{0},{1}] zabraný"));// todo tp

            return point;
        }
    }
}
