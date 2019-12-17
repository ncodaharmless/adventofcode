using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{


    class Day04
    {
        #region Input
        #endregion

        public Day04()
        {
        }

        public static bool IsValidPassword(int number, bool partB = false)
        {
            string str = number.ToString();
            int adjacentCount = 0;
            bool hasAdjacent = false;
            for (int i = 1; i < str.Length; i++)
            {
                if (str[i - 1] == str[i])
                {
                    adjacentCount++;
                }
                else
                {
                    if (partB)
                    {
                        if (adjacentCount == 1)
                            hasAdjacent = true;
                    }
                    else if (adjacentCount > 0)
                        hasAdjacent = true;
                    adjacentCount = 0;
                }
                if (str[i - 1] > str[i])
                    return false;
            }
            if (partB)
            {
                if (adjacentCount == 1)
                    hasAdjacent = true;
            }
            else if (adjacentCount > 0)
                hasAdjacent = true;
            if (!hasAdjacent) return false;

            return true;
        }

        internal int Part1()
        {
            int from = 130254;
            int to = 678275;
            int validCount = 0;
            for (int i = from; i <= to; i++)
                if (IsValidPassword(i))
                    validCount++;
            return validCount;
        }

        internal int Part2()
        {
            int from = 130254;
            int to = 678275;
            int validCount = 0;
            for (int i = from; i <= to; i++)
                if (IsValidPassword(i, partB: true))
                    validCount++;
            return validCount;
        }
    }

    [TestClass]
    public class TestDay04
    {
        [TestMethod]
        public void Example1()
        {
            Assert.IsTrue(Day04.IsValidPassword(111111));
            Assert.IsFalse(Day04.IsValidPassword(223450));
            Assert.IsFalse(Day04.IsValidPassword(123789));
        }

        [TestMethod]
        public void Example2()
        {
            Assert.IsTrue(Day04.IsValidPassword(112233, partB: true));
            Assert.IsFalse(Day04.IsValidPassword(123444, partB: true));
            Assert.IsTrue(Day04.IsValidPassword(111122, partB: true));
        }

        [TestMethod]
        public void Part1()
        {
            Assert.AreEqual(2090, new Day04().Part1());
        }
        [TestMethod]
        public void Part2()
        {
            Assert.AreEqual(1419, new Day04().Part2());
        }
    }

}