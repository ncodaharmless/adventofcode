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

        public Moon[] OriginalMoons;
        public Moon[] Moons;

        public Day12(string input = Input)
        {
            OriginalMoons = input.SplitLine().Select(t => new Moon { Pos = new Vector3(t) }).ToArray();
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

        internal int Part2()
        {
            throw new NotImplementedException();
        }

    }

    class Moon
    {
        public Vector3 Pos { get; set; }
        public Vector3 Velocity { get; set; } = new Vector3();

        public int PotentialEnergy => Pos.ManhattanDistance(new Vector3());
        public int KineticEnergy => Velocity.ManhattanDistance(new Vector3());
        public int TotalEnergy => PotentialEnergy * KineticEnergy;

        public void Move()
        {
            Pos.Add(Velocity);
        }
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
            Console.WriteLine(new Day12().Part2());
        }
    }
}
