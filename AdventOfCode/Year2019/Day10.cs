using AdventOfCode.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{


    class Day10
    {
        #region Input
        const string Input = @"
.#..#..##.#...###.#............#.
.....#..........##..#..#####.#..#
#....#...#..#.......#...........#
.#....#....#....#.#...#.#.#.#....
..#..#.....#.......###.#.#.##....
...#.##.###..#....#........#..#.#
..#.##..#.#.#...##..........#...#
..#..#.......................#..#
...#..#.#...##.#...#.#..#.#......W
......#......#.....#.............
.###..#.#..#...#..#.#.......##..#
.#...#.................###......#
#.#.......#..####.#..##.###.....#
.#.#..#.#...##.#.#..#..##.#.#.#..
##...#....#...#....##....#.#....#
......#..#......#.#.....##..#.#..
##.###.....#.#.###.#..#..#..###..
#...........#.#..#..#..#....#....
..........#.#.#..#.###...#.....#.
...#.###........##..#..##........
.###.....#.#.###...##.........#..
#.#...##.....#.#.........#..#.###
..##..##........#........#......#
..####......#...#..........#.#...
......##...##.#........#...##.##.
.#..###...#.......#........#....#
...##...#..#...#..#..#.#.#...#...
....#......#.#............##.....
#......####...#.....#...#......#.
...#............#...#..#.#.#..#.#
.#...#....###.####....#.#........
#.#...##...#.##...#....#.#..##.#.
.#....#.###..#..##.#.##...#.#..##";
        #endregion

        Point[] Asteroids;
        int Width;
        int Height;

        public Day10(string input = Input)
        {
            string[] lines = input.SplitLine();
            List<Point> points = new List<Point>();
            Width = lines[0].Length;
            Height = lines.Length;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (lines[y][x] == '#')
                        points.Add(new Point(x, y));
            Asteroids = points.ToArray();
        }

        internal int Part1()
        {
            Point best;
            int bestCount;
            FindBestAsteroid(out best, out bestCount);
            Console.WriteLine(best);
            OutputGrid(best);
            return bestCount;
        }

        internal void OutputGrid(Point p)
        {
            for (int y = 0; y < Height; y++)
            {
                Console.WriteLine();
                for (int x = 0; x < Width; x++)
                {
                    if (p.X == x && p.Y == y)
                        Console.Write("O");
                    else if (Asteroids.Any(a => a.X == x && a.Y == y))
                        Console.Write(IsBlocked(p, new Point(x, y)) ? "H" : "A");
                    else
                        Console.Write(".");
                }
            }
        }

        internal Point BestPoint()
        {
            Point best;
            int bestCount;
            FindBestAsteroid(out best, out bestCount);
            return best;
        }

        private void FindBestAsteroid(out Point best, out int bestCount)
        {
            best = new Point();
            bestCount = 0;
            foreach (var from in Asteroids)
            {
                int thisCount = 0;
                foreach (var to in Asteroids)
                {
                    if (!to.Equals(from) && !IsBlocked(from, to))
                        thisCount++;
                }
                if (thisCount > bestCount)
                {
                    best = from;
                    bestCount = thisCount;
                }
            }
        }

        private bool IsBlocked(Point from, Point to)
        {
            int minX = Math.Min(from.X, to.X);
            int maxX = Math.Max(from.X, to.X);
            int minY = Math.Min(from.Y, to.Y);
            int maxY = Math.Max(from.Y, to.Y);
            int dx = to.X - from.X;
            int dy = to.Y - from.Y;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                // horiz test
                for (int x = minX + 1; x < maxX; x++)
                {
                    double delta = from.Y + (x - from.X) * dy / (double)dx;
                    if (delta % 1 == 0)
                    {
                        Point test = new Point(x, (int)delta);
                        if (Asteroids.Any(a => a.X == test.X && a.Y == test.Y))
                            return true;
                    }
                }
            }
            else
            {
                // vert test
                for (int y = minY + 1; y < maxY; y++)
                {
                    double delta = from.X + (y - from.Y) * dx / (double)dy;
                    if (delta % 1 == 0)
                    {
                        Point test = new Point((int)delta, y);
                        if (Asteroids.Any(a => a.X == test.X && a.Y == test.Y))
                            return true;
                    }
                }
            }
            return false;
        }

        internal int Part2(int px, int py)
        {
            Point point = new Point(px, py);
            var anglesToPoint = new Dictionary<Point, double>();
            foreach (var asteroid in Asteroids)
            {
                anglesToPoint.Add(asteroid, point.AngleTo(asteroid));
            }

            List<Point> sorted = Asteroids.OrderBy(a => anglesToPoint[a]).ThenBy(a => point.ManhattanDist(a)).ToList();

            double angle = 270; // up
            int i = 0;
            while (anglesToPoint[sorted[i]] < 270) i++;
            Point lastRemoved = new Point();
            for (int c = 0; c < 200; c++)
            {
                angle = anglesToPoint[sorted[i]];
                lastRemoved = sorted[i];
                sorted.RemoveAt(i); i--;
                Point next;
                do
                {
                    i = (i + 1) % sorted.Count;
                    next = sorted[i];
                } while (angle == anglesToPoint[next]);
            }

            return lastRemoved.X * 100 + lastRemoved.Y;
        }
    }

    [TestClass]
    public class TestDay10
    {
        [TestMethod]
        public void Example1()
        {
            var d = new Day10(@"
.#..#
.....
#####
....#
...##");
            Assert.AreEqual(new Point(3, 4), d.BestPoint());
            Assert.AreEqual(8, d.Part1());
        }

        [TestMethod]
        public void Example2()
        {
            var d = new Day10(@"
......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####");
            Assert.AreEqual(new Point(5, 8), d.BestPoint());
            Assert.AreEqual(33, d.Part1());
        }

        [TestMethod]
        public void Example3()
        {
            var d = new Day10(@"
#.#...#.#.
.###....#.
.#....#...
##.#.#.#.#
....#.#.#.
.##..###.#
..#...##..
..##....##
......#...
.####.###.");
            Assert.AreEqual(new Point(1, 2), d.BestPoint());
            Assert.AreEqual(35, d.Part1());
        }

        [TestMethod]
        public void Example4()
        {
            var d = new Day10(@"
.#..#..###
####.###.#
....###.#.
..###.##.#
##.##.#.#.
....###..#
..#.#..#.#
#..#.#.###
.##...##.#
.....#.#..");
            Assert.AreEqual(new Point(6, 3), d.BestPoint());
            Assert.AreEqual(41, d.Part1());
        }

        [TestMethod]
        public void Example5()
        {
            var d = new Day10(@"
.#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##");
            Assert.AreEqual(new Point(11, 13), d.BestPoint());
            Assert.AreEqual(210, d.Part1());
        }

        [TestMethod]
        public void Example6()
        {
            var d = new Day10(@"
.#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##");
            Assert.AreEqual(802, d.Part2(11, 13));
        }

        [TestMethod]
        public void Part1()
        {
            Assert.AreEqual(263, new Day10().Part1());
        }

        [TestMethod]
        public void Part2()
        {
            Assert.AreEqual(1110, new Day10().Part2(23, 29));
        }
    }

}
