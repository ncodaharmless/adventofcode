using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Year2018
{
    class Day10
    {
        private Star[] stars;
        private int stepCount;

        public Day10(string[] args)
        {
            stars = File.ReadAllLines(args.FirstOrDefault() ?? "day10.txt").Select(s => Star.Parse(s)).ToArray();

            int lastWidth = int.MaxValue;
            while (true)
            {
                int thisWidth = Width();
                if (lastWidth < thisWidth)
                {
                    StepBack();
                    Print();
                    break;
                }
                lastWidth = thisWidth;
                Step();
            }
            Console.WriteLine($"Waited {stepCount}");
            Console.ReadLine();
        }

        private void Print()
        {
            int minX = stars.Select(s => s.X).Min();
            int maxX = stars.Select(s => s.X).Max();
            int minY = stars.Select(s => s.Y).Min();
            int maxY = stars.Select(s => s.Y).Max();
            for (int y = minY; y <= maxY; y++)
            {
                for (int i = minX; i <= maxX; i++)
                {
                    Console.Write(stars.Any(s => s.X == i && s.Y == y) ? "*" : " ");
                }
                Console.WriteLine();
            }
        }

        private void Step()
        {
            stepCount++;
            foreach (Star star in stars)
                star.Step();
        }

        private void StepBack()
        {
            stepCount--;
            foreach (Star star in stars)
                star.StepBack();
        }

        private int Width()
        {
            return stars.Select(s => s.X).Max() - stars.Select(s => s.X).Min();
        }

        public class Star
        {
            public int X;
            public int Y;
            public int DX;
            public int DY;

            public static Star Parse(string input)
            {
                Star star = new Star();
                var match = Regex.Match(input, "position=<([-0-9 ]+),([-0-9 ]+)> velocity=<([-0-9 ]+),([-0-9 ]+)>");
                if (!match.Success) throw new NotSupportedException(input);
                if (match.Groups.Count != 5) throw new NotSupportedException();
                star.X = int.Parse(match.Groups[1].Value);
                star.Y = int.Parse(match.Groups[2].Value);
                star.DX = int.Parse(match.Groups[3].Value);
                star.DY = int.Parse(match.Groups[4].Value);
                return star;
            }

            public void Step()
            {
                X += DX;
                Y += DY;
            }

            public void StepBack()
            {
                X -= DX;
                Y -= DY;
            }
        }
    }
}
