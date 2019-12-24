using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{


    class Day24
    {
        #region Input
        const string Input = @".#.#.
.##..
.#...
.###.
##..#";
        #endregion

        public string Bugs;

        public Day24(string input = Input)
        {
            Bugs = input.Replace("\r\n", "");
        }

        public void Generation()
        {
            string result = string.Empty;
            for (int i = 0; i < Bugs.Length; i++)
            {
                int bugsSurrounding = 0;
                int column = i % 5;
                if (i - 5 >= 0 && Bugs[i - 5] == '#')
                    bugsSurrounding++;
                if (column > 0 && Bugs[i - 1] == '#')
                    bugsSurrounding++;
                if (column < 4 && Bugs[i + 1] == '#')
                    bugsSurrounding++;
                if (i + 5 < Bugs.Length && Bugs[i + 5] == '#')
                    bugsSurrounding++;
                if (Bugs[i] == '#')
                    result += bugsSurrounding == 1 ? '#' : '.';
                else
                    result += bugsSurrounding == 1 || bugsSurrounding == 2 ? '#' : '.';
            }
            Bugs = result;
        }

        internal long Part1()
        {
            HashSet<string> history = new HashSet<string>();
            history.Add(Bugs);
            while (true)
            {
                Generation();
                if (history.Contains(Bugs))
                {
                    long biodiversity = 0;
                    for (int i = 0; i < Bugs.Length; i++)
                        if (Bugs[i] == '#')
                            biodiversity += (long)Math.Pow(2, i);
                    return biodiversity;
                }
                history.Add(Bugs);
            }
        }

        internal int Part2()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class TestDay24
    {
        [TestMethod]
        public void Example1()
        {
            var d = new Day24(@"....#
#..#.
#..##
..#..
#....");
            d.Generation(); // after 1 min
            Assert.AreEqual(@"#..#.
####.
###.#
##.##
.##..".Replace("\r\n", ""), d.Bugs);
            d.Generation(); // after 2 min
            Assert.AreEqual(@"#####
....#
....#
...#.
#.###".Replace("\r\n", ""), d.Bugs);
            d.Generation(); // after 3 min
            Assert.AreEqual(@"#....
####.
...##
#.##.
.##.#".Replace("\r\n", ""), d.Bugs);
            d.Generation(); // after 4 min
            Assert.AreEqual(@"####.
....#
##..#
.....
##...".Replace("\r\n", ""), d.Bugs);

            Assert.AreEqual(2129920, d.Part1());
        }

        [TestMethod]
        public void Part1()
        {
            Assert.AreEqual(32513278, new Day24().Part1());
        }
        [TestMethod]
        public void Part2()
        {
            Assert.AreEqual(0, new Day24().Part2());
        }
    }

}
