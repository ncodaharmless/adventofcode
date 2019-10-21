using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Year2018
{
    public class Day06
    {
        class Coord
        {
            public int X;
            public int Y;

            public int DistanceTo(Coord c)
            {
                return Math.Abs(c.X - X) + Math.Abs(c.Y - Y);
            }
        }

        public void Run()
        {
            string[] input = System.IO.File.ReadAllLines("day06.txt");

            int minX = int.MaxValue;
            int maxX = 0;
            int minY = int.MaxValue;
            int maxY = 0;

            List<Coord> coords = new List<Coord>();
            foreach (string line in input)
            {
                Coord c = new Coord();
                c.X = int.Parse(line.Split(',')[0].Trim());
                c.Y = int.Parse(line.Split(',')[1].Trim());
                coords.Add(c);
                maxX = Math.Max(maxX, c.X);
                maxY = Math.Max(maxY, c.Y);
                minX = Math.Min(minX, c.X);
                minY = Math.Min(minY, c.Y);
            }

            Console.WriteLine($"Bounds {minX},{minY} {maxX},{maxY}");

            int largestArea = 0;

            int tol = 0;
            // area count per coordinate
            int[] areaCount = new int[coords.Count];
            for (int x = minX - tol; x < maxX + tol; x++)
                for (int y = minY - tol; y < maxY + tol; y++)
                {
                    bool equalDist = false;
                    int minDist = int.MaxValue;
                    Coord p = new Coord();
                    p.X = x;
                    p.Y = y;
                    int coordIndex = -1;
                    for (int i = 0; i < coords.Count; i++)
                    {
                        int thisDist = coords[i].DistanceTo(p);
                        if (thisDist < minDist)
                        {
                            equalDist = false;
                            minDist = thisDist;
                            coordIndex = i;
                        }
                        else if (thisDist == minDist)
                        {
                            equalDist = true;
                        }
                    }
                    if (!equalDist)
                    {
                        // attribute this coordinate to the square
                        areaCount[coordIndex]++;
                    }
                }

            for (int i = 0; i < areaCount.Length; i++)
            {
                Coord c = coords[i];
                // make sure coordinate is not infinite
                if (IsInfinite(coords, c)) continue;
                if (areaCount[i] > largestArea)
                {
                    largestArea = areaCount[i];
                }
            }

            Console.WriteLine($"Part 1 - Largest area = {largestArea}");

            int regionSize = 0;
            for (int x = minX - tol; x < maxX + tol; x++)
                for (int y = minY - tol; y < maxY + tol; y++)
                {
                    Coord p = new Coord();
                    p.X = x;
                    p.Y = y;
                    int totalDistance = 0;
                    for (int i = 0; i < coords.Count; i++)
                    {
                        totalDistance += coords[i].DistanceTo(p);
                    }
                    if (totalDistance < 10000)
                    {
                        // attribute this coordinate to the square
                        regionSize++;
                    }
                }
            Console.WriteLine($"Region Size = {regionSize}");
        }

        private bool IsInfinite(List<Coord> list, Coord c)
        {
            Coord left = new Coord();
            left.X = 0;
            left.Y = c.Y;
            if (list.OrderBy(x => x.DistanceTo(left)).First() == c) return true;
            left.X = 500000;
            left.Y = c.Y;
            if (list.OrderBy(x => x.DistanceTo(left)).First() == c) return true;
            left.X = c.X;
            left.Y = 0;
            if (list.OrderBy(x => x.DistanceTo(left)).First() == c) return true;
            left.X = c.X;
            left.Y = 500000;
            if (list.OrderBy(x => x.DistanceTo(left)).First() == c) return true;
            return false;
        }
    }
}