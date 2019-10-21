using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Year2018
{
    class Day22
    {
        public Point targetPoint;
        public int caveSystemDepth;

        int areaWidth;
        int areaHeight;
        int[][] erosionLevel;

        public Day22(int depth = 4002, int px = 5, int py = 746)
        {
            caveSystemDepth = depth;
            targetPoint = new Point(px, py);
            const int extra = 10;
            areaWidth = px + 1 + extra;
            areaHeight = py + 1 + extra;
            erosionLevel = new int[areaWidth][];
            for (int x = 0; x < areaWidth; x++)
            {
                erosionLevel[x] = new int[areaHeight];
                for (int y = 0; y < areaHeight; y++) erosionLevel[x][y] = ErosionLevel(new Point(x, y));
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


        enum Tool { None, Torch, Climbing }

        int[,] timeTakenNone;
        int[,] timeTakenTorch;
        int[,] timeTakenClimb;
        internal int FastestTime()
        {
            timeTakenNone = new int[areaWidth, areaHeight];
            timeTakenTorch = new int[areaWidth, areaHeight];
            timeTakenClimb = new int[areaWidth, areaHeight];

            timeTakenNone[targetPoint.X, targetPoint.Y] = int.MaxValue;
            timeTakenTorch[targetPoint.X, targetPoint.Y] = 1;
            timeTakenClimb[targetPoint.X, targetPoint.Y] = 8;
            PopulateTimesToTravel(targetPoint.Up());

            // normalise
            for (int i = 0; i < 100; i++)
            {
                for (int x = 0; x < areaWidth; x++)
                    for (int y = 0; y < areaHeight; y++)
                    {
                        timeTakenNone[x, y] = FastestTimeWithTool(new Point(x, y), Tool.None);
                        timeTakenClimb[x, y] = FastestTimeWithTool(new Point(x, y), Tool.Climbing);
                        timeTakenTorch[x, y] = FastestTimeWithTool(new Point(x, y), Tool.Torch);
                    }

            }

            return Math.Min(timeTakenTorch[0, 0], Math.Min(timeTakenNone[0, 0], timeTakenClimb[0, 0]));
        }

        private void PopulateTimesToTravel(Point p)
        {
            if (p.X < 0 || p.Y < 0 || p.X >= areaWidth || p.Y >= areaHeight || (p.X == targetPoint.X && p.Y == targetPoint.Y)) return;
            int timeNone = FastestTimeWithTool(p, Tool.None);
            int timeClimb = FastestTimeWithTool(p, Tool.Climbing);
            int timeTorch = FastestTimeWithTool(p, Tool.Torch);
            if (timeTakenNone[p.X, p.Y] != timeNone || timeTakenClimb[p.X, p.Y] != timeClimb || timeTakenTorch[p.X, p.Y] != timeTorch)
            {
                timeTakenNone[p.X, p.Y] = timeNone;
                timeTakenClimb[p.X, p.Y] = timeClimb;
                timeTakenTorch[p.X, p.Y] = timeTorch;

                PopulateTimesToTravel(p.Left());
                PopulateTimesToTravel(p.Right());
                PopulateTimesToTravel(p.Up());
                PopulateTimesToTravel(p.Below());
            }

        }

        private int FastestTimeWithTool(Point p, Tool t)
        {
            return Math.Min(Math.Min(
                FastestTimeWithTool(p, Tool.Climbing, t),
                FastestTimeWithTool(p, Tool.None, t))
                , FastestTimeWithTool(p, Tool.Torch, t));
        }
        private int FastestTimeWithTool(Point p, Tool fromTool, Tool t)
        {
            if (NeedToolChange(p, fromTool)) return int.MaxValue;

            var fastP = p.Left();
            int fastestTime = TimeFromPoint(fastP, fromTool, t);
            if (fastestTime > TimeFromPoint(p.Up(), fromTool, t))
            {
                fastestTime = TimeFromPoint(p.Up(), fromTool, t);
                fastP = p.Up();
            }
            if (fastestTime > TimeFromPoint(p.Right(), fromTool, t))
            {
                fastestTime = TimeFromPoint(p.Right(), fromTool, t);
                fastP = p.Right();
            }
            if (fastestTime > TimeFromPoint(p.Below(), fromTool, t))
            {
                fastestTime = TimeFromPoint(p.Below(), fromTool, t);
                fastP = p.Below();
            }
            return fastestTime;
        }

        private int TimeFromPoint(Point from, Tool fromTool, Tool toTool)
        {
            if (from.X < 0 || from.Y < 0 || from.X >= areaWidth || from.Y >= areaHeight) return int.MaxValue;

            int fromTime;
            switch (fromTool)
            {
                case Tool.Climbing:
                    fromTime = timeTakenClimb[from.X, from.Y];
                    break;
                case Tool.Torch:
                    fromTime = timeTakenTorch[from.X, from.Y];
                    break;
                default:
                    fromTime = timeTakenNone[from.X, from.Y];
                    break;
            }
            if (fromTime == 0 || fromTime == int.MaxValue) return int.MaxValue;

            return fromTime + (fromTool != toTool ? 8 : 1);
        }

        private bool NeedToolChange(Point to, Tool current)
        {
            switch (RegionType(erosionLevel[to.X][to.Y]))
            {
                case 0:
                    return (current != Tool.None);
                case 1:
                    return (current != Tool.Torch);
                case 2:
                    return (current != Tool.Climbing);
                default:
                    throw new NotImplementedException();
            }
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

            Assert.AreEqual(45, test.FastestTime());
        }

        [TestMethod]
        public void Part1()
        {
            Console.WriteLine(new Day22().TotalRisk());
        }

        [TestMethod]
        public void Part2()
        {
            Console.WriteLine(new Day22().FastestTime());
        }
    }
}
