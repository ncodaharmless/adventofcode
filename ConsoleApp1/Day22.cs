using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleApp1
{
    class Day22
    {
        public Point targetPoint;
        public int caveSystemDepth;

        int[][] erosionLevel;

        public Day22(int depth = 4002, int px = 5, int py = 746)
        {
            caveSystemDepth = depth;
            targetPoint = new Point(px, py);
            erosionLevel = new int[px + 1][];
            for (int x = 0; x <= px; x++)
            {
                erosionLevel[x] = new int[py + 1];
                for (int y = 0; y <= py; y++) erosionLevel[x][y] = ErosionLevel(new Point(x, y));
            }
        }

        internal int ErosionLevel(Point p)
        {
            return (GeologicIndex(p) + caveSystemDepth) % 20183;
        }

        internal int PreCalcErosionLevel(Point p)
        {
            return erosionLevel[p.X][p.Y];
        }

        internal int GeologicIndex(Point p)
        {
            if (p.X == 0 && p.Y == 0) return 0;
            if (p.X == targetPoint.X && p.Y == targetPoint.Y) return 0;
            if (p.Y == 0) return p.X * 16807;
            if (p.X == 0) return p.Y * 48271;
            return PreCalcErosionLevel(p.Left()) * PreCalcErosionLevel(p.Up());
        }

        internal int TotalRisk()
        {
            int risk = 0;
            for (int x = 0; x <= targetPoint.X; x++)
                for (int y = 0; y <= targetPoint.Y; y++)
                    risk += RegionType(erosionLevel[x][y]);
            return risk;
        }

        internal static int RegionType(int erosionLevel)
        {
            return erosionLevel % 3;
        }
    }

    [TestClass]
    public class TestDay22
    {
        [TestMethod]
        public void Test()
        {
            var test = new Day22(510, 10, 10);
            Assert.AreEqual(510, test.ErosionLevel(new Point(0, 0)));

            Assert.AreEqual(16807, test.GeologicIndex(new Point(1, 0)));
            Assert.AreEqual(17317, test.ErosionLevel(new Point(1, 0)));

            Assert.AreEqual(48271, test.GeologicIndex(new Point(0, 1)));
            Assert.AreEqual(8415, test.ErosionLevel(new Point(0, 1)));

            Assert.AreEqual(145722555, test.GeologicIndex(new Point(1, 1)));
            Assert.AreEqual(1805, test.ErosionLevel(new Point(1, 1)));

            Assert.AreEqual(0, test.GeologicIndex(new Point(10, 10)));
            Assert.AreEqual(510, test.ErosionLevel(new Point(10, 10)));

            Assert.AreEqual(114, test.TotalRisk());
        }

        [TestMethod]
        public void Part1()
        {
            Console.WriteLine(new Day22().TotalRisk());
        }
    }
}
