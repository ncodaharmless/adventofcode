using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2018
{
    public class Day03
    {
        int[] data = new int[1000 * 1000];

        class Claim
        {
            public int Id;
            public int X;
            public int Y;
            public int Width;
            public int Height;
            public int Right;
            public int Bottom;

            public Claim(string line)
            {
                //#11 @ 62,484: 28x29
                string[] parts = line.Split(' ');
                Id = Convert.ToInt32(parts[0].TrimStart('#').Trim());
                X = Convert.ToInt32(parts[2].Split(',')[0]);
                Y = Convert.ToInt32(parts[2].Split(',')[1].TrimEnd(':'));
                Width = Convert.ToInt32(parts[3].Split('x')[0]);
                Height = Convert.ToInt32(parts[3].Split('x')[1]);
                Right = X + Width;
                Bottom = Y + Height;
            }

            public bool Intersects(Claim b)
            {
                return !(Right <= b.X || X >= b.Right || Bottom <= b.Y || Y >= b.Bottom);
            }

            public override string ToString()
            {
                return $"{X},{Y} {Right},{Bottom}";
            }
        }

        public void Run()
        {
            string[] lines = System.IO.File.ReadAllLines("day03.txt");

            List<Claim> list = new List<Claim>();
            foreach (string line in lines)
            {
                var claim = new Claim(line);
                for (int x = claim.X; x < claim.X + claim.Width; x++)
                    for (int y = claim.Y; y < claim.Y + claim.Height; y++)
                        Inc(x, y);
                list.Add(claim);
            }

            int overlap = 0;

            for (int i = 0; i < list.Count; i++)
            {
                bool intersect = false;
                for (int j = 0; j < list.Count; j++)
                {
                    if (j != i && list[i].Intersects(list[j]))
                    {
                        intersect = true;
                        break;
                    }
                }
                if (!intersect)
                {
                    Console.WriteLine("Claim Id = " + list[i].Id);
                }
            }
            foreach (int i in data)
                if (i > 1) overlap++;

            Console.WriteLine("Overlap " + overlap);
        }

        private int Get(int x, int y)
        {
            return data[x + y * 1000];
        }
        private void Inc(int x, int y)
        {
            data[x + y * 1000]++;
        }
    }
}