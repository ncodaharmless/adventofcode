﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Utils;
using System.IO;

namespace AdventOfCode.Year2019
{


    class Day15
    {
        #region Input
        const string Input = @"3,1033,1008,1033,1,1032,1005,1032,31,1008,1033,2,1032,1005,1032,58,1008,1033,3,1032,1005,1032,81,1008,1033,4,1032,1005,1032,104,99,101,0,1034,1039,1001,1036,0,1041,1001,1035,-1,1040,1008,1038,0,1043,102,-1,1043,1032,1,1037,1032,1042,1105,1,124,101,0,1034,1039,101,0,1036,1041,1001,1035,1,1040,1008,1038,0,1043,1,1037,1038,1042,1106,0,124,1001,1034,-1,1039,1008,1036,0,1041,1001,1035,0,1040,1001,1038,0,1043,1002,1037,1,1042,1105,1,124,1001,1034,1,1039,1008,1036,0,1041,102,1,1035,1040,101,0,1038,1043,1002,1037,1,1042,1006,1039,217,1006,1040,217,1008,1039,40,1032,1005,1032,217,1008,1040,40,1032,1005,1032,217,1008,1039,1,1032,1006,1032,165,1008,1040,33,1032,1006,1032,165,1101,0,2,1044,1106,0,224,2,1041,1043,1032,1006,1032,179,1101,1,0,1044,1106,0,224,1,1041,1043,1032,1006,1032,217,1,1042,1043,1032,1001,1032,-1,1032,1002,1032,39,1032,1,1032,1039,1032,101,-1,1032,1032,101,252,1032,211,1007,0,43,1044,1105,1,224,1101,0,0,1044,1106,0,224,1006,1044,247,1002,1039,1,1034,1002,1040,1,1035,102,1,1041,1036,1001,1043,0,1038,101,0,1042,1037,4,1044,1105,1,0,13,30,60,64,5,28,36,24,67,12,1,67,32,39,14,78,29,17,38,88,79,9,62,25,15,18,88,25,7,81,38,41,10,69,86,32,11,33,1,10,22,84,14,92,48,79,10,3,62,33,61,13,93,78,20,63,68,17,80,34,12,8,23,61,90,51,17,84,37,46,64,25,3,73,19,45,99,41,62,21,77,8,17,89,9,13,84,75,85,14,53,60,6,29,76,63,14,23,63,61,93,72,17,41,28,94,5,3,19,47,57,55,14,34,38,79,85,40,13,22,99,67,72,15,62,15,6,63,3,90,2,87,20,84,15,50,70,27,18,78,21,70,48,52,2,99,92,55,3,46,41,93,99,88,13,39,4,45,71,3,96,1,91,59,31,53,23,25,82,32,50,16,60,38,78,34,59,30,15,51,92,3,22,26,62,60,37,42,74,28,21,76,7,24,70,18,40,11,81,41,9,73,62,12,66,81,9,3,74,62,11,6,56,16,34,20,78,79,1,97,17,39,87,15,12,77,94,28,22,66,45,59,39,2,6,52,6,72,49,17,92,15,86,18,92,79,67,20,22,72,10,72,3,52,26,77,78,41,97,36,59,88,24,57,12,38,90,53,14,38,67,2,36,44,93,99,10,41,49,3,16,7,63,32,11,15,81,12,91,39,62,19,83,6,91,28,19,80,38,23,63,31,71,14,58,8,21,71,21,21,81,38,26,32,29,82,52,28,72,54,97,41,65,96,75,1,48,28,80,66,25,47,49,29,87,51,12,50,70,36,60,81,29,77,76,55,25,40,45,83,91,26,72,99,12,47,11,20,27,52,9,98,17,99,27,37,62,25,3,15,73,66,22,5,85,5,20,98,20,38,62,78,21,16,59,28,98,38,31,2,40,46,87,14,48,33,80,48,36,27,56,21,1,50,83,3,61,92,20,52,16,50,10,86,9,98,39,56,25,50,42,39,91,81,56,25,70,44,24,15,99,4,20,55,12,98,27,65,20,77,97,76,36,42,87,6,11,79,65,16,65,44,13,90,13,48,79,13,95,60,19,55,24,66,4,53,11,23,68,14,97,53,45,14,16,93,18,29,83,5,6,77,19,70,97,34,20,70,52,11,74,14,72,10,36,44,33,45,19,38,36,77,5,37,51,1,55,17,2,48,23,18,2,34,90,97,24,30,51,66,33,70,51,37,31,51,37,65,55,18,8,66,4,65,62,26,93,29,88,3,75,73,24,23,67,1,13,68,7,36,87,62,48,1,31,45,28,62,86,24,98,1,59,49,37,26,62,36,44,66,18,17,97,92,40,36,65,80,84,5,84,6,79,87,36,31,96,15,71,96,2,72,11,81,95,94,41,54,31,58,25,74,24,51,81,38,32,73,22,96,40,62,22,59,74,39,25,86,2,55,20,61,40,37,88,69,1,60,42,18,31,54,13,27,19,93,34,41,99,33,89,20,16,52,84,32,94,31,6,61,25,1,61,1,38,78,87,39,31,39,26,68,42,36,2,94,66,2,67,30,80,2,95,65,40,54,50,33,11,23,97,89,1,31,56,9,35,49,92,55,23,84,48,91,20,7,72,25,55,3,85,3,16,40,90,22,99,44,38,86,98,11,76,26,76,13,82,80,24,93,4,15,64,95,58,15,85,25,57,29,66,3,66,19,98,57,24,44,59,35,76,48,31,92,33,94,68,56,41,45,15,46,5,68,15,65,34,73,49,68,17,78,28,80,24,59,26,74,21,52,1,94,5,61,41,88,37,56,1,49,0,0,21,21,1,10,1,0,0,0,0,0,0";
        #endregion

        IntcodeComputerExt cmp;

        public Day15(string input = Input)
        {
            cmp = new IntcodeComputerExt(input);
        }

        private void Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    cmp.InputQueue.Enqueue(1);
                    break;
                case Direction.Down:
                    cmp.InputQueue.Enqueue(2);
                    break;
                case Direction.Left:
                    cmp.InputQueue.Enqueue(3);
                    break;
                case Direction.Right:
                    cmp.InputQueue.Enqueue(4);
                    break;
            }
        }

        enum MapAreaCode
        {
            Wall = 0,
            Space = 1,
            OxygenSystem = 2,
        }


        Dictionary<Point, MapAreaCode> area = new Dictionary<Point, MapAreaCode>();
        Point currentPoint = new Point();
        SearchMode mode = SearchMode.FindFirstBound;
        Direction facing = Direction.Left;
        enum SearchMode
        {
            FindFirstBound,
            FindBounds,
            Scanline,
        }

        private Direction NextMoveDirection()
        {
            if (mode == SearchMode.FindFirstBound)
            {
                Point forwardPoint = currentPoint.MoveDirection(facing);
                if (CanMoveTo(forwardPoint))
                {
                    return facing;
                }
                facing = Direction.Right;
                mode = SearchMode.FindBounds;
            }
            if (mode == SearchMode.FindBounds)
            {
                Direction testDirection = Point.RotateLeft(facing);
                while (!CanMoveTo(currentPoint.MoveDirection(testDirection)))
                    testDirection = Point.RotateRight(testDirection);
                facing = testDirection;
                return testDirection;
            }
            throw new NotSupportedException();
        }

        private bool CanMoveTo(Point p)
        {
            return !area.ContainsKey(p) || area[p] == MapAreaCode.Space;
        }

        private Point FindOxygenSystem()
        {
            Point attemptToMoveTo = new Point();
            while (true)
            {
                if (!cmp.IsReady)
                {
                    Direction dir = NextMoveDirection();
                    Move(dir);
                    attemptToMoveTo = currentPoint.MoveDirection(dir);
                }
                if (!cmp.RunNext())
                    break;
                if (cmp.Output.Count > 0)
                {
                    //System.Threading.Thread.Sleep(10);
                    MapAreaCode result = (MapAreaCode)cmp.Output.Dequeue();
                    if (!area.ContainsKey(attemptToMoveTo))
                    {
                        area.Add(attemptToMoveTo, result);
                        if (result == MapAreaCode.Wall)
                        {
                            //Console.SetCursorPosition(attemptToMoveTo.X + DrawOffsetX, attemptToMoveTo.Y + DrawOffsetY);
                            //Console.Write("#");
                        }
                    }
                    if (result == MapAreaCode.OxygenSystem)
                        return attemptToMoveTo;
                    else if (result == MapAreaCode.Space)
                    {
                        //Console.SetCursorPosition(currentPoint.X + DrawOffsetX, currentPoint.Y + DrawOffsetY);
                        //Console.Write(" ");
                        currentPoint = attemptToMoveTo;
                        //Console.SetCursorPosition(currentPoint.X + DrawOffsetX, currentPoint.Y + DrawOffsetY);
                        //Console.Write("D");
                    }
                }
            }
            throw new Exception("Not found");
        }

        private void OutputMap()
        {
            int drawMinX = area.Keys.Select(m => m.X).Min();
            int drawMinY = area.Keys.Select(m => m.Y).Min();
            int drawMaxX = area.Keys.Select(m => m.X).Max();
            int drawMaxY = area.Keys.Select(m => m.Y).Max();
            using (var str = new StreamWriter(@"c:\temp\out.txt"))
            {
                for (int y = drawMinY; y <= drawMaxY; y++)
                {
                    for (int x = drawMinX; x <= drawMaxX; x++)
                    {
                        Point p = new Point(x, y);
                        if (x == 0 && y == 0)
                            str.Write("D");
                        else
                        if (area.ContainsKey(p))
                        {
                            switch (area[p])
                            {
                                case MapAreaCode.OxygenSystem:
                                    str.Write("O");
                                    break;
                                case MapAreaCode.Wall:
                                    str.Write("#");
                                    break;
                                default:
                                    str.Write(" ");
                                    break;
                            }
                        }
                        else
                            str.Write("?");
                    }
                    str.WriteLine();
                }
            }
        }

        Dictionary<Point, int> distanceToO = new Dictionary<Point, int>();

        internal int Part1()
        {
            Point oxygenPoint = FindOxygenSystem();
            OutputMap();

            SetDistanceTo(oxygenPoint, 0);

            return distanceToO[new Point(0, 0)];
        }

        private void SetDistanceTo(Point p, int distance)
        {
            if (area.TryGetValue(p, out MapAreaCode mapPoint) && mapPoint != MapAreaCode.Wall)
            {
                if (!distanceToO.ContainsKey(p) || distanceToO[p] > distance)
                {
                    if (!distanceToO.ContainsKey(p))
                        distanceToO.Add(p, distance);
                    else
                        distanceToO[p] = distance;
                    SetDistanceTo(p.Left(), distance + 1);
                    SetDistanceTo(p.Right(), distance + 1);
                    SetDistanceTo(p.Up(), distance + 1);
                    SetDistanceTo(p.Below(), distance + 1);
                }
            }
        }

        internal int Part2()
        {
            Point oxygenPoint = FindOxygenSystem();
            OutputMap();

            SetDistanceTo(oxygenPoint, 0);

            return distanceToO.Values.Max();
        }
    }

    [TestClass]
    public class TestDay15
    {
        [TestMethod]
        public void Part1()
        {
            Assert.AreEqual(380, new Day15().Part1());
        }
        [TestMethod]
        public void Part2()
        {
            Assert.AreEqual(410, new Day15().Part2());
        }
    }

}
