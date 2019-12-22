using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{


    class Day22
    {
        #region Input
        const string Input = @"cut 9002
deal with increment 17
cut -4844
deal with increment 26
cut -4847
deal with increment 74
deal into new stack
deal with increment 75
deal into new stack
deal with increment 64
cut 9628
deal with increment 41
cut 9626
deal with increment 40
cut -7273
deal into new stack
deal with increment 20
deal into new stack
cut 2146
deal with increment 7
cut -3541
deal with increment 25
cut -1343
deal with increment 42
cut -2608
deal with increment 75
cut -9258
deal into new stack
cut -2556
deal into new stack
cut -5363
deal into new stack
cut -8143
deal with increment 15
cut -9309
deal with increment 65
cut -5470
deal with increment 9
deal into new stack
deal with increment 64
cut 137
deal with increment 40
deal into new stack
cut 5042
deal with increment 74
cut 4021
deal with increment 39
cut -5108
deal with increment 50
cut -6608
deal with increment 64
cut 4438
deal with increment 48
cut 7916
deal with increment 23
cut -6677
deal with increment 27
cut -1758
deal with increment 32
cut -3104
deal with increment 37
cut 9453
deal with increment 20
deal into new stack
deal with increment 6
cut 8168
deal with increment 69
cut 6704
deal with increment 45
cut -9423
deal with increment 2
cut -3498
deal with increment 39
deal into new stack
cut 6051
deal with increment 42
cut 7140
deal into new stack
deal with increment 73
cut -8201
deal into new stack
deal with increment 13
cut 2737
deal with increment 3
cut -4651
deal into new stack
deal with increment 30
cut -1505
deal with increment 59
deal into new stack
deal with increment 55
cut 8899
deal with increment 39
cut 8775
deal with increment 57
cut -1919
deal with increment 39
cut -3845
deal with increment 8
cut -4202";
        #endregion

        public List<int> _Cards = new List<int>();

        public Day22(string input = Input, int cardCount = 10007)
        {
            for (int i = 0; i < cardCount; i++)
                _Cards.Add(i);
            foreach (string line in input.SplitLine())
            {
                CardAction(line);
            }
        }

        private void CardAction(string line)
        {
            if (line.StartsWith("deal into new stack"))
            {
                _Cards.Reverse();
            }
            else if (line.StartsWith("deal with increment "))
            {
                int increment = Convert.ToInt32(line.Substring("deal with increment ".Length));
                int[] replacement = new int[_Cards.Count];
                int offset = 0;
                int length = _Cards.Count;
                for (int i = 0; i < length; i++)
                {
                    replacement[offset % length] = _Cards[i];
                    offset += increment;
                }
                _Cards = new List<int>(replacement);
            }
            else if (line.StartsWith("cut "))
            {
                int count = Convert.ToInt32(line.Substring("cut ".Length));
                if (count < 0)
                {
                    var from = _Cards.GetRange(_Cards.Count + count, -count);
                    _Cards.RemoveRange(_Cards.Count + count, -count);
                    _Cards.InsertRange(0, from);
                }
                else
                {
                    var from = _Cards.GetRange(0, count);
                    _Cards.RemoveRange(0, count);
                    _Cards.AddRange(from);
                }
            }
            else
                throw new NotSupportedException(line);
        }

        internal int Part1()
        {
            return _Cards.IndexOf(2019);
        }

        internal int Part2()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class TestDay22
    {
        [TestMethod]
        public void Example0()
        {
            var d = new Day22(@"deal with increment 3", 10);
            Assert.AreEqual("0 7 4 1 8 5 2 9 6 3", string.Join(" ", d._Cards));
        }
        [TestMethod]
        public void Example1()
        {
            var d = new Day22(@"deal with increment 7
deal into new stack
deal into new stack", 10);
            Assert.AreEqual("0 3 6 9 2 5 8 1 4 7", string.Join(" ", d._Cards));
        }
        [TestMethod]
        public void Example2()
        {
            var d = new Day22(@"cut 6
deal with increment 7
deal into new stack", 10);
            Assert.AreEqual("3 0 7 4 1 8 5 2 9 6", string.Join(" ", d._Cards));
        }
        [TestMethod]
        public void Example3()
        {
            var d = new Day22(@"deal with increment 7
deal with increment 9
cut -2", 10);
            Assert.AreEqual("6 3 0 7 4 1 8 5 2 9", string.Join(" ", d._Cards));
        }
        [TestMethod]
        public void Example4()
        {
            var d = new Day22(@"deal into new stack
cut -2
deal with increment 7
cut 8
cut -4
deal with increment 7
cut 3
deal with increment 9
deal with increment 3
cut -1", 10);
            Assert.AreEqual("9 2 5 8 1 4 7 0 3 6", string.Join(" ", d._Cards));
        }

        [TestMethod]
        public void Part1()
        {
            Assert.AreEqual(2604, new Day22().Part1());
        }
        [TestMethod]
        public void Part2()
        {
            Assert.AreEqual(0, new Day22().Part2());
        }
    }

}
