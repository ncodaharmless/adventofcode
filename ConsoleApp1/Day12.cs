using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Day12
    {
        private const string main_input = @"
initial state: #.......##.###.#.#..##..##..#.#.###..###..##.#.#..##....#####..##.#.....########....#....##.#..##...
..... => .
#.... => .
..### => .
##..# => #
.###. => #
...## => .
#.#.. => .
..##. => .
##.#. => #
..#.. => .
.#... => #
##.## => .
....# => .
.#.#. => .
#..#. => #
#.### => .
.##.# => #
.#### => .
.#..# => .
####. => #
#...# => #
.#.## => #
#..## => .
..#.# => #
#.##. => .
###.. => .
##### => #
###.# => #
...#. => #
#.#.# => #
.##.. => .
##... => #";

        List<WillPlantRule> rules = new List<WillPlantRule>();
        PlantPotCollection plants = new PlantPotCollection();

        public Day12(string input = main_input)
        {
            var lines = input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string initial = lines[0].Substring(14).Trim();
            for (int i = 0; i < initial.Length; i++)
                if (initial[i] == '#')
                    plants.Add(i);

            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i].Trim().EndsWith("#"))
                    rules.Add(new WillPlantRule(lines[i]));
            }

        }

        public void Part1()
        {
            StepGenerations(20);
            Console.WriteLine(Total);
        }

        public void Part2()
        {
            StepGenerations(50000000000);
            Console.WriteLine(Total);
        }

        public long Total { get; set; }

        public void StepGenerations(long count)
        {
            int remainSizeCount = 0;
            long lastSizeIncrease = 0;
            Total = plants.Total;
            for (long i = 0; i < count; i++)
            {
                plants.StepGeneration(rules);
                int total = plants.Total;
                long sizeIncrease = total - Total;
                if (sizeIncrease > 0 && sizeIncrease == lastSizeIncrease)
                {
                    if (remainSizeCount > 1000)
                    {
                        Total = total + (count - i - 1) * sizeIncrease;
                        return;
                    }
                    remainSizeCount++;
                }
                else
                {
                    remainSizeCount = 0;
                }
                lastSizeIncrease = sizeIncrease;
                Total = total;
            }
        }

        class PlantPotCollection
        {
            HashSet<int> plants = new HashSet<int>(1000);
            List<int> newGeneration = new List<int>(1000);

            public void Add(int pot)
            {
                plants.Add(pot);
            }

            public int Total => plants.Sum();

            public void StepGeneration(List<WillPlantRule> rules)
            {
                newGeneration.Clear();
                foreach (int potNumber in plants)
                {
                    for (int i = -2; i <= 2; i++)
                        if (WillHavePlantNextGen(rules, potNumber + i))
                            newGeneration.Add(potNumber + i);
                }
                plants.Clear();
                foreach (int i in newGeneration)
                    if (!plants.Contains(i))
                        plants.Add(i);
            }

            public bool WillHavePlantNextGen(List<WillPlantRule> rules, int potNumber)
            {
                foreach (var p in rules)
                {
                    if (
                        p.PlantLeftLeft == plants.Contains(potNumber - 2)
                        && p.PlantLeft == plants.Contains(potNumber - 1)
                        && p.PlantSelf == plants.Contains(potNumber)
                        && p.PlantRight == plants.Contains(potNumber + 1)
                        && p.PlantRightRight == plants.Contains(potNumber + 2)
                        )
                        return true;
                }
                return false;
            }

        }

        class WillPlantRule
        {
            public byte[] Data;
            public bool PlantLeftLeft;
            public bool PlantLeft;
            public bool PlantSelf;
            public bool PlantRight;
            public bool PlantRightRight;

            public WillPlantRule(string ruleLine)
            {
                PlantLeftLeft = ruleLine[0] == '#';
                PlantLeft = ruleLine[1] == '#';
                PlantSelf = ruleLine[2] == '#';
                PlantRight = ruleLine[3] == '#';
                PlantRightRight = ruleLine[4] == '#';
                Data = new byte[5];
                Data[0] = (byte)(ruleLine[0] == '#' ? 1 : 0);
                Data[1] = (byte)(ruleLine[1] == '#' ? 1 : 0);
                Data[2] = (byte)(ruleLine[2] == '#' ? 1 : 0);
                Data[3] = (byte)(ruleLine[3] == '#' ? 1 : 0);
                Data[4] = (byte)(ruleLine[4] == '#' ? 1 : 0);
            }

            public override string ToString()
            {
                return (PlantLeftLeft ? "#" : ".") + (PlantLeft ? "#" : ".") + (PlantSelf ? "#" : ".") + (PlantRight ? "#" : ".") + (PlantRightRight ? "#" : ".");
            }
        }
    }

    [TestClass]
    public class TestDay12
    {
        [TestMethod]
        public void TestSample()
        {
            var day12 = new Day12(@"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #");
            day12.StepGenerations(20);
            Assert.AreEqual(325, day12.Total);
        }

        [TestMethod]
        public void TestSample_Duration()
        {
            var day12 = new Day12(@"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #");
            day12.StepGenerations(200000);
        }
    }
}
