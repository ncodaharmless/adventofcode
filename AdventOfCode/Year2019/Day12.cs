using AdventOfCode.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{


    class Day12
    {
        #region Input
        const string Input = @"
<x=3, y=-6, z=6>
<x=10, y=7, z=-9>
<x=-3, y=-7, z=9>
<x=-8, y=0, z=4>";
        #endregion

        public Moon[] Moons;

        public Day12(string input = Input)
        {
            Moons = input.SplitLine().Select(t => new Moon { Pos = new Vector3(t) }).ToArray();
        }

        public void ApplyGravity()
        {
            foreach (Moon a in Moons)
            {
                foreach (Moon b in Moons)
                {
                    if (a == b) continue;

                    if (a.Pos.X > b.Pos.X) a.Velocity.X--;
                    else if (a.Pos.X < b.Pos.X) a.Velocity.X++;

                    if (a.Pos.Y > b.Pos.Y) a.Velocity.Y--;
                    else if (a.Pos.Y < b.Pos.Y) a.Velocity.Y++;

                    if (a.Pos.Z > b.Pos.Z) a.Velocity.Z--;
                    else if (a.Pos.Z < b.Pos.Z) a.Velocity.Z++;
                }
            }
        }

        public void Move()
        {
            foreach (Moon a in Moons)
                a.Move();
        }

        public void Step()
        {
            ApplyGravity();
            Move();
        }

        internal int Part1()
        {
            for (int i = 0; i < 1000; i++)
            {
                Step();
            }
            return Moons.Sum(m => m.TotalEnergy);
        }

        static long LCM(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
        private static long LeastCommonMultiple(long[] numbers)
        {
            return numbers.Aggregate(LCM);
            /*
            long[] multiples = new long[numbers.Length];
            long interval = numbers.Min();
            long lcm = interval;
            while (true)
            {
                bool same = true;
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (multiples[i] != lcm)
                    {
                        same = false;
                        break;
                    }
                }
                if (same)
                    return lcm;

                lcm += interval;
                for (int i = 0; i < numbers.Length; i++)
                {
                    while (multiples[i] < lcm)
                        multiples[i] += numbers[i];
                }
            }*/
        }

        internal long Part2()
        {
            while (true)
            {
                for (int i = 0; i < 4; i++)
                    Moons[i].AddToHistoryAndCheckForCycle();
                bool foundAllCycle = true;
                for (int i = 0; i < 4; i++)
                {
                    if (!Moons[i].FoundCycle) { foundAllCycle = false; break; }
                }
                if (foundAllCycle) break;
                Step();
            }

            List<long> cycles = new List<long>();
            for (int i = 0; i < 4; i++)
            {
                cycles.Add(Moons[i].CycleFinderX.Cycle.Length);
                cycles.Add(Moons[i].CycleFinderY.Cycle.Length);
                cycles.Add(Moons[i].CycleFinderZ.Cycle.Length);
            }
            long lcm = LeastCommonMultiple(cycles.ToArray());

            return lcm;
        }

    }

    class Moon
    {
        public Vector3 Pos { get; set; }
        public Vector3 Velocity { get; set; } = new Vector3();

        public int PotentialEnergy => Pos.ManhattanDistance(new Vector3());
        public int KineticEnergy => Velocity.ManhattanDistance(new Vector3());
        public int TotalEnergy => PotentialEnergy * KineticEnergy;

        public List<Vector3> History { get; set; } = new List<Vector3>();

        public void Move()
        {
            Pos.Add(Velocity);
        }

        public CycleFinder CycleFinderX { get; } = new CycleFinder();
        public CycleFinder CycleFinderY { get; } = new CycleFinder();
        public CycleFinder CycleFinderZ { get; } = new CycleFinder();
        public void AddToHistoryAndCheckForCycle()
        {
            CycleFinderX.Add(Pos.X);
            CycleFinderY.Add(Pos.Y);
            CycleFinderZ.Add(Pos.Z);
        }

        public bool FoundCycle => CycleFinderX.Found && CycleFinderY.Found && CycleFinderX.Found;
    }

    [TestClass]
    public class TestDay12
    {
        [TestMethod]
        public void Example1()
        {
            var d = new Day12(@"
<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>");
            d.ApplyGravity();
            d.Move();
            Assert.AreEqual(new Vector3(3, -1, -1), d.Moons[0].Velocity);
            for (int i = 0; i < 9; i++)
            {
                d.ApplyGravity();
                d.Move();
            }
            Assert.AreEqual(36, d.Moons[0].TotalEnergy);
            Assert.AreEqual(45, d.Moons[1].TotalEnergy);
            Assert.AreEqual(80, d.Moons[2].TotalEnergy);
            Assert.AreEqual(18, d.Moons[3].TotalEnergy);
        }

        [TestMethod]
        public void Part1()
        {
            Console.WriteLine(new Day12().Part1());
        }
        [TestMethod]
        public void Part2()
        {
            Assert.AreEqual(356658899375688, new Day12().Part2());
        }
    }
}
