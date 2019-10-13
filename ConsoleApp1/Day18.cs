using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Day18
    {
        public int TotalTrees => Map.SelectMany(m => m).Where(m => m == '|').Count();
        public int TotalLumberyards => Map.SelectMany(m => m).Where(m => m == '#').Count();

        private int GlobalMinuteCount;
        private char[][] Map;
        private char[][] NewMap;
        private int mapHeight;
        private int mapWidth;

        public Day18(string input = SampleInput)
        {
            Map = input.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim().ToCharArray()).ToArray();
            mapHeight = Map.Length;
            mapWidth = Map[0].Length;
            NewMap = input.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim().ToCharArray()).ToArray();
        }

        public void Step()
        {
            GlobalMinuteCount++;
            for (int y = 0; y < mapHeight; y++)
            {
                int fromY = Math.Max(0, y - 1);
                int toY = Math.Min(y + 1, mapHeight - 1);
                for (int x = 0; x < mapWidth; x++)
                {
                    char tileChar = Map[y][x];
                    int treeCount = 0;
                    int lumberCount = 0;
                    int toX = Math.Min(x + 1, mapWidth - 1);
                    for (int xx = Math.Max(0, x - 1); xx <= toX; xx++)
                    {
                        for (int yy = fromY; yy <= toY; yy++)
                        {
                            if (xx != x || yy != y)
                            {
                                char m = Map[yy][xx];
                                if (m == '|')
                                    treeCount++;
                                else if (m == '#')
                                    lumberCount++;
                            }
                        }
                    }
                    if (tileChar == '.' && treeCount >= 3)
                        tileChar = '|';
                    else if (tileChar == '|' && lumberCount >= 3)
                        tileChar = '#';
                    else if (tileChar == '#' && (lumberCount == 0 || treeCount == 0))
                        tileChar = '.';
                    NewMap[y][x] = tileChar;
                }
            }
            var tmp = Map;
            Map = NewMap;
            NewMap = tmp;
        }

        public void Part1()
        {
            for (int i = 0; i < 10; i++)
                Step();
            Console.WriteLine(TotalLumberyards * TotalTrees);
        }

        public void Part2(int count = 1000000000)
        {
            for (int i = 0; i < count; i++)
                Step();
            Console.WriteLine(TotalLumberyards * TotalTrees);
        }

        public string GetMapString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"After {GlobalMinuteCount} minutes:");
            for (int y = 0; y < mapHeight; y++)
                sb.AppendLine(new String(Map[y]));
            return sb.ToString();
        }

        #region Input

        const string SampleInput = @"
............|#.|..|..|#.#.....#.#||.#.#...#.|#....
|...|.##...#.|..||#.##..###...|..#||#|.....|..|...
#.|.#..||#..|.|..|..|.....##....#.##||..#|.|.....|
.##||.|..#.......###||.#.#.|..##.#|..#...#.....#.|
....#|##|.|.|...|||..|...#.......|#..|#|.|..|#|...
|....|...#.....|#||...#.....#....#|....|.....|....
.#||..|##.##|..||.#.....#...|##..|#...#.#.|.#.|##.
..#|##..|......#|..#.|#.##....|.....|..#.#...|...#
#....#||.|..|..#..#.|........|.|.....||||.|..|....
|.#|.|...#..|||#.##|#.........|...#..#.|#.....||.|
.#.##.........|...|.|.#..|.|##...|.|#.||....||.#..
#.#.....##|.#|..###..|........|.....|.....|#.||.#|
|..||....#..###.|..#..#.#.......##.#.|.#..|.|.#|.#
|...##.#..#|..|..|......#..#......|..|..#.....|.||
.......||#|.|...##.#.##..|#.|.|#|....##||...#.#.|#
..|###..|.....#|.|.|.|....|....#.#..##|...||......
|.|#|.#|||#.|#|#..|...#.|..|..#.#...#..#.#.#.#..#.
...#.###...#............#..|#...||#|..#..|###...|.
.|......|...#....#.......|...##.|.|..##.#...#||||.
#...|.#|.|.......|##.#...|#.|.........#.|.|##.||..
|.|#..#.||#|.|..|..|#........#..#.....#.#..#.#.|..
#..|.|...#|#..#|....|...|..|#|....|..##|#|......||
....||...|....|...#|..|.#|.....#.|..||..|...|.....
#......#.#..##.|#.|||.|#||||.|...#...|#.|#...||...
.|...|...|.#..|#.|####|.......|.#||.|..|..|..#.#..
.#.|##.......|||...#..#.#....#..|..#.###........#.
#.#..|.##..|...#....#.#.|...|.#.|.....|.|......|#.
#.||.#......#.....##.|...|#..#.#|..#..|..#..#.#.|.
|...#|#...|....#|##..|....||....#.||||.|#...|.....
.|...|.|...|...|..|..#|...||||#.#...|.#....##...|.
.|.#...#|...#||.#....|##.#|..#|||......|.|..||...|
|#...|.|..|.#...#..||......|.|.##.....||#|#.#|..|.
..#.|.###......|.|.###|...#.#|#||....#..|....|#..|
|....|.|.|#.|.#..|.|.#.#.||..##|..|.||#|..|...||..
#.|#.|....|#|#.|..#...|...|.#.#.....#...#|###.#..#
...###|.#....#..#..|.|....#|.|.###.|...|..|.##.|#.
...#....#|.#|#.|#.|#|.|...|.....#..|......|.||###.
.||#|.|.#.|..|||.#.|..#.....#..|..###||#|||...#..#
###||#........#|####.|..|#....#|.|.|.|#|.|.#|.##.|
|..#.#|....#....|.#.|..|...|..#..|....#.###.|.#|#.
.#..##|##...#.....#||||#.#...||..|........|#......
.#.|||#.|...#.#.|.|..|......#.#....##|..|#||....##
...##..|....|#.||......##...#..||##.##.#..#.|##|..
..#.||...|#..||#....#|.##.#|.........#...||.|#|#|#
#.......|.|#...##.#..#|#.|.|.|.##|...#...|...#.#..
.....||......|...#..|......||.|#..#....##|...#...#
..#|.#....|.......#.#..##......|.#.|..|..#..#.#.||
.|.....#...#......#...#..||||#|.....||..|....#|...
#...#.||..#......|.##|#.....|..|..|..|.......|.||.
..|#.|......||.|.||.|##|||.|..#.......#|||.#.|.|#|";

        #endregion
    }

    [TestClass]
    public class TestDay18
    {
        [TestMethod]
        public void TestSample()
        {
            var test = new Day18(@"
.#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|.");

            test.Step();
            Assert.AreEqual(@"
After 1 minutes:
.......##.
......|###
.|..|...#.
..|#||...#
..##||.|#|
...#||||..
||...|||..
|||||.||.|
||||||||||
....||..|.
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 2 minutes:
.......#..
......|#..
.|.|||....
..##|||..#
..###|||#|
...#|||||.
|||||||||.
||||||||||
||||||||||
.|||||||||
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 3 minutes:
.......#..
....|||#..
.|.||||...
..###|||.#
...##|||#|
.||##|||||
||||||||||
||||||||||
||||||||||
||||||||||
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 4 minutes:
.....|.#..
...||||#..
.|.#||||..
..###||||#
...###||#|
|||##|||||
||||||||||
||||||||||
||||||||||
||||||||||
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 5 minutes:
....|||#..
...||||#..
.|.##||||.
..####|||#
.|.###||#|
|||###||||
||||||||||
||||||||||
||||||||||
||||||||||
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 6 minutes:
...||||#..
...||||#..
.|.###|||.
..#.##|||#
|||#.##|#|
|||###||||
||||#|||||
||||||||||
||||||||||
||||||||||
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 7 minutes:
...||||#..
..||#|##..
.|.####||.
||#..##||#
||##.##|#|
|||####|||
|||###||||
||||||||||
||||||||||
||||||||||
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 8 minutes:
..||||##..
..|#####..
|||#####|.
||#...##|#
||##..###|
||##.###||
|||####|||
||||#|||||
||||||||||
||||||||||
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 9 minutes:
..||###...
.||#####..
||##...##.
||#....###
|##....##|
||##..###|
||######||
|||###||||
||||||||||
||||||||||
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 10 minutes:
.||##.....
||###.....
||##......
|##.....##
|##.....##
|##....##|
||##.####|
||#####|||
||||#|||||
||||||||||
", test.GetMapString());

            Assert.AreEqual(37, test.TotalTrees);
            Assert.AreEqual(31, test.TotalLumberyards);
        }

        [TestMethod]
        public void PerfTest()
        {
            var test = new Day18();
            for (int i = 0; i < 100000; i++)
                test.Step();
        }
    }
}
