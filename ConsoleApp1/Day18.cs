﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private string[] Map;
        private int mapHeight;
        private int mapWidth;

        public Day18(string input = SampleInput)
        {
            Map = input.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            mapHeight = Map.Length;
            mapWidth = Map[0].Length;
        }

        public void Step()
        {
            GlobalMinuteCount++;
            List<string> newMap = new List<string>();
            for (int y = 0; y < mapHeight; y++)
            {
                string newMapRow = "";
                for (int x = 0; x < mapWidth; x++)
                {
                    char tileChar = Map[y][x];
                    int treeCount = 0;
                    int lumberCount = 0;
                    for (int xx = Math.Max(0, x - 1); xx <= Math.Min(x + 1, mapWidth - 1); xx++)
                    {
                        for (int yy = Math.Max(0, y - 1); yy <= Math.Min(y + 1, mapHeight - 1); yy++)
                        {
                            if (xx != x || yy != y)
                            {
                                switch (Map[yy][xx])
                                {
                                    case '|':
                                        treeCount++;
                                        break;
                                    case '#':
                                        lumberCount++;
                                        break;
                                }
                            }
                        }
                    }
                    if (tileChar == '.' && treeCount >= 3)
                        tileChar = '|';
                    else if (tileChar == '|' && lumberCount >= 3)
                        tileChar = '#';
                    else if (tileChar == '#' && (lumberCount == 0 || treeCount == 0))
                        tileChar = '.';
                    newMapRow += tileChar;
                }
                newMap.Add(newMapRow);
            }
            Map = newMap.ToArray();
        }

        public void Part1()
        {
            for (int i = 0; i < 10; i++)
                Step();
            Console.WriteLine(TotalLumberyards * TotalTrees);
        }

        public string GetMapString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"After {GlobalMinuteCount} minutes:");
            for (int y = 0; y < mapHeight; y++)
                sb.AppendLine(Map[y]);
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
    }
}
