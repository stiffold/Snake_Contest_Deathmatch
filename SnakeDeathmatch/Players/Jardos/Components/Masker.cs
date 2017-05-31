using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardos.Components
{
    public class Masker
    {
        private List<MaskItem> _maskItems;

        public Masker()
        {
            _maskItems = GetAllMasks().ToList();
        }

        public bool Mask(Point p, int[,] playground)
        {
            foreach (var m in _maskItems)
            {
                bool can = true;
                foreach (var mp in m.MaskPoints)
                {
                    Point np = new Point(p.X + mp.X, p.Y + mp.Y);
                    if (np.IsValid() && playground[np.X, np.Y] == 0)
                    {
                        can = false;
                        break;
                    }
                }
                if (can)
                {
                    return true;
                }
            }
            return false;
        }

        private IEnumerable<MaskItem> GetAllMasks()
        {
            return CantEntryMasks().Select(m => GetMaskItemFromString(m, 3));
        }

        private IEnumerable<string> CantEntryMasks()
        {
            yield return "0X0" +
                         "X0X" +
                         "000";

            yield return "000" +
                         "X0X" +
                         "0X0";

            yield return "0X0" +
                         "X00" +
                         "0X0";

            yield return "0X0" +
                         "00X" +
                         "0X0";
            
            yield return "0XX" +
                         "X00" +
                         "X00";

            yield return "XX0" +
                         "00X" +
                         "00X";

            yield return "00X" +
                         "00X" +
                         "XX0";

            yield return "X00" +
                         "X00" +
                         "0XX";
        }

        private MaskItem GetMaskItemFromString(string s, int count)
        {
            var m = new MaskItem {MaskPoints = new List<Point>()};
            int diff = count - 1;

            for (int x = 0; x < count; x++)
            {
                for (int y = 0; y < count; y++)
                {
                    if (s.Substring((y*count)+x,1)=="X")
                    {
                        m.MaskPoints.Add(new Point(x - diff + 1, y - diff + 1));
                    }
                }
            }

            return m;
        }

        public static void Circle(int[,] array, Point p, double radius, int value)
        {
            for (int row = 0; row < ComputeHelper.MaxSize; row++)
            {
                for (int col = 0; col < ComputeHelper.MaxSize; col++)
                {
                    double d = Math.Sqrt((p.X - row) * (p.X - row) + (p.Y - col) * (p.Y - col));
                    if (d <= radius)
                    {
                        array[row, col] = value;
                    }
                }
            }
        }
    }


    internal class MaskItem
    {
        public List<Point> MaskPoints { get; set; }
    }
}
