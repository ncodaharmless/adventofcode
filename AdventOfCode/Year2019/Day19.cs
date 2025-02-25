﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{


    class Day19
    {
        #region Input
        const string Input = @"109,424,203,1,21102,11,1,0,1106,0,282,21102,18,1,0,1105,1,259,1202,1,1,221,203,1,21102,31,1,0,1106,0,282,21101,38,0,0,1106,0,259,21002,23,1,2,22102,1,1,3,21102,1,1,1,21102,1,57,0,1105,1,303,2101,0,1,222,21002,221,1,3,20101,0,221,2,21101,0,259,1,21102,1,80,0,1105,1,225,21102,1,8,2,21101,91,0,0,1106,0,303,1202,1,1,223,21002,222,1,4,21102,1,259,3,21101,225,0,2,21101,225,0,1,21101,0,118,0,1105,1,225,21001,222,0,3,21101,0,48,2,21102,133,1,0,1106,0,303,21202,1,-1,1,22001,223,1,1,21102,1,148,0,1105,1,259,1201,1,0,223,20101,0,221,4,21001,222,0,3,21101,0,6,2,1001,132,-2,224,1002,224,2,224,1001,224,3,224,1002,132,-1,132,1,224,132,224,21001,224,1,1,21102,1,195,0,105,1,108,20207,1,223,2,21001,23,0,1,21101,-1,0,3,21101,0,214,0,1105,1,303,22101,1,1,1,204,1,99,0,0,0,0,109,5,2101,0,-4,249,21201,-3,0,1,22102,1,-2,2,21202,-1,1,3,21102,1,250,0,1106,0,225,21201,1,0,-4,109,-5,2106,0,0,109,3,22107,0,-2,-1,21202,-1,2,-1,21201,-1,-1,-1,22202,-1,-2,-2,109,-3,2106,0,0,109,3,21207,-2,0,-1,1206,-1,294,104,0,99,22101,0,-2,-2,109,-3,2106,0,0,109,5,22207,-3,-4,-1,1206,-1,346,22201,-4,-3,-4,21202,-3,-1,-1,22201,-4,-1,2,21202,2,-1,-1,22201,-4,-1,1,22102,1,-2,3,21101,0,343,0,1105,1,303,1105,1,415,22207,-2,-3,-1,1206,-1,387,22201,-3,-2,-3,21202,-2,-1,-1,22201,-3,-1,3,21202,3,-1,-1,22201,-3,-1,2,22101,0,-4,1,21101,384,0,0,1106,0,303,1106,0,415,21202,-4,-1,-4,22201,-4,-3,-4,22202,-3,-2,-2,22202,-2,-4,-4,22202,-3,-2,-3,21202,-4,-1,-2,22201,-3,-2,1,21201,1,0,-4,109,-5,2106,0,0";
        #endregion

        IntcodeComputerExt cmp;

        public Day19(string input = Input)
        {
            cmp = new IntcodeComputerExt(input);
        }

        internal int Part1(int size = 50)
        {
            int inBeam = 0;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    long result = ScanPoint(x, y);
                    if (result == 1) inBeam++;
                    Console.Write(result == 0 ? "#" : ".");
                }
                Console.WriteLine();
            }
            return inBeam;
        }

        private long ScanPoint(int x, int y)
        {
            cmp.ReInit();
            cmp.InputQueue.Enqueue(x);
            cmp.InputQueue.Enqueue(y);
            while (cmp.Output.Count == 0 && cmp.RunNext()) ;
            return cmp.Output.Dequeue();
        }

        internal int Part2()
        {
            int x = 50;
            int y = 20;
            while (true)
            {
                while (ScanPoint(x, y) == 0) x++;
                if (ScanPoint(x + 99, y) == 0) { y++; continue; }
                while (ScanPoint(x + 100, y) == 1) x++;
                if (ScanPoint(x, y + 99) == 1) break;
                y++;
            }
            return x * 10000 + y;
        }
    }

    [TestClass]
    public class TestDay19
    {
        [TestMethod]
        public void Example1()
        {
            var d = new Day19();
            Assert.AreEqual(5, d.Part1(10));
        }

        [TestMethod]
        public void Part1()
        {
            Assert.AreEqual(152, new Day19().Part1());
        }
        [TestMethod]
        public void Part2()
        {
            Assert.AreEqual(10730411, new Day19().Part2());
        }
    }

}
