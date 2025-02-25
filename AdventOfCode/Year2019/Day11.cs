﻿using AdventOfCode.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{


    class Day11
    {
        #region Input
        const string Input = @"3,8,1005,8,324,1106,0,11,0,0,0,104,1,104,0,3,8,1002,8,-1,10,1001,10,1,10,4,10,1008,8,1,10,4,10,1001,8,0,29,1,1107,14,10,1006,0,63,1006,0,71,3,8,1002,8,-1,10,101,1,10,10,4,10,1008,8,1,10,4,10,1002,8,1,61,1,103,18,10,1006,0,14,1,105,7,10,3,8,1002,8,-1,10,101,1,10,10,4,10,1008,8,1,10,4,10,101,0,8,94,1006,0,37,1006,0,55,2,1101,15,10,3,8,1002,8,-1,10,1001,10,1,10,4,10,1008,8,0,10,4,10,101,0,8,126,2,1006,12,10,3,8,102,-1,8,10,101,1,10,10,4,10,1008,8,1,10,4,10,1001,8,0,152,3,8,102,-1,8,10,1001,10,1,10,4,10,108,0,8,10,4,10,101,0,8,173,1006,0,51,1006,0,26,3,8,102,-1,8,10,101,1,10,10,4,10,1008,8,0,10,4,10,1001,8,0,202,2,8,18,10,1,103,19,10,1,1102,1,10,1006,0,85,3,8,102,-1,8,10,1001,10,1,10,4,10,108,0,8,10,4,10,1001,8,0,238,2,1002,8,10,1006,0,41,3,8,102,-1,8,10,1001,10,1,10,4,10,108,0,8,10,4,10,101,0,8,267,2,1108,17,10,2,105,11,10,1006,0,59,1006,0,90,3,8,1002,8,-1,10,1001,10,1,10,4,10,1008,8,1,10,4,10,1001,8,0,304,101,1,9,9,1007,9,993,10,1005,10,15,99,109,646,104,0,104,1,21102,936735777688,1,1,21101,341,0,0,1105,1,445,21101,0,937264173716,1,21101,352,0,0,1106,0,445,3,10,104,0,104,1,3,10,104,0,104,0,3,10,104,0,104,1,3,10,104,0,104,1,3,10,104,0,104,0,3,10,104,0,104,1,21101,3245513819,0,1,21102,1,399,0,1105,1,445,21102,1,29086470235,1,21102,410,1,0,1105,1,445,3,10,104,0,104,0,3,10,104,0,104,0,21101,825544712960,0,1,21102,1,433,0,1106,0,445,21102,825460826472,1,1,21101,0,444,0,1106,0,445,99,109,2,22102,1,-1,1,21101,0,40,2,21101,0,476,3,21102,466,1,0,1105,1,509,109,-2,2105,1,0,0,1,0,0,1,109,2,3,10,204,-1,1001,471,472,487,4,0,1001,471,1,471,108,4,471,10,1006,10,503,1101,0,0,471,109,-2,2106,0,0,0,109,4,2101,0,-1,508,1207,-3,0,10,1006,10,526,21101,0,0,-3,21202,-3,1,1,21201,-2,0,2,21101,0,1,3,21101,0,545,0,1105,1,550,109,-4,2105,1,0,109,5,1207,-3,1,10,1006,10,573,2207,-4,-2,10,1006,10,573,21202,-4,1,-4,1106,0,641,21202,-4,1,1,21201,-3,-1,2,21202,-2,2,3,21101,0,592,0,1105,1,550,22101,0,1,-4,21101,1,0,-1,2207,-4,-2,10,1006,10,611,21102,1,0,-1,22202,-2,-1,-2,2107,0,-3,10,1006,10,633,22101,0,-1,1,21102,633,1,0,105,1,508,21202,-2,-1,-2,22201,-4,-2,-4,109,-5,2105,1,0";
        #endregion

        IntcodeComputerExt cmp;
        Point robotPosition = new Point();

        HashSet<Point> paintedWhite = new HashSet<Point>();
        HashSet<Point> painted = new HashSet<Point>();


        public Day11(string input = Input)
        {
            long[] intCompCode = input.SplitComma().Select(i => Convert.ToInt64(i)).ToArray();
            cmp = new IntcodeComputerExt(intCompCode);
        }

        private void RunPaint()
        {
            bool firstOutput = true;
            Direction facing = Direction.Up;
            while (true)
            {
                if (!cmp.IsReady)
                {
                    if (paintedWhite.Contains(robotPosition))
                        cmp.InputQueue.Enqueue(1);
                    else
                        cmp.InputQueue.Enqueue(0);
                }
                if (!cmp.RunNext()) break;
                while (cmp.Output.Count > 0)
                {
                    if (firstOutput)
                    {
                        long paintPanelColour = cmp.Output.Dequeue();
                        if (paintPanelColour == 0)
                            paintedWhite.Remove(robotPosition);
                        else if (!paintedWhite.Contains(robotPosition))
                            paintedWhite.Add(robotPosition);
                        if (!painted.Contains(robotPosition))
                            painted.Add(robotPosition);
                    }
                    else
                    {
                        long turn = cmp.Output.Dequeue();
                        if (turn == 0)
                            facing = Point.RotateLeft(facing);
                        else if (turn == 1)
                            facing = Point.RotateRight(facing);
                        else throw new NotSupportedException(turn.ToString());
                        robotPosition = robotPosition.MoveDirection(facing);
                    }
                    firstOutput = !firstOutput;
                }

            }
        }

        internal int Part1()
        {
            RunPaint();
            return painted.Count;
        }

        internal int Part2()
        {
            paintedWhite.Add(new Point());
            RunPaint();
            int minX = painted.Min(p => p.X);
            int maxX = painted.Max(p => p.X);
            int minY = painted.Min(p => p.Y);
            int maxY = painted.Max(p => p.Y);
            for (int y = minY; y <= maxY; y++)
            {
                Console.WriteLine();
                for (int x = minX; x <= maxX; x++)
                    Console.Write(paintedWhite.Contains(new Point(x, y)) ? "X" : " ");
            }
            return 0;
        }
    }

    [TestClass]
    public class TestDay11
    {
        [TestMethod]
        public void Part1()
        {
            Assert.AreEqual(2441, new Day11().Part1());
        }
        [TestMethod]
        public void Part2()
        {
            new Day11().Part2();
        }
    }

}
