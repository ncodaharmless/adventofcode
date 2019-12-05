using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{


    class Day05
    {
        #region Input

        private const string Input = @"3,225,1,225,6,6,1100,1,238,225,104,0,1101,65,39,225,2,14,169,224,101,-2340,224,224,4,224,1002,223,8,223,101,7,224,224,1,224,223,223,1001,144,70,224,101,-96,224,224,4,224,1002,223,8,223,1001,224,2,224,1,223,224,223,1101,92,65,225,1102,42,8,225,1002,61,84,224,101,-7728,224,224,4,224,102,8,223,223,1001,224,5,224,1,223,224,223,1102,67,73,224,1001,224,-4891,224,4,224,102,8,223,223,101,4,224,224,1,224,223,223,1102,54,12,225,102,67,114,224,101,-804,224,224,4,224,102,8,223,223,1001,224,3,224,1,224,223,223,1101,19,79,225,1101,62,26,225,101,57,139,224,1001,224,-76,224,4,224,1002,223,8,223,1001,224,2,224,1,224,223,223,1102,60,47,225,1101,20,62,225,1101,47,44,224,1001,224,-91,224,4,224,1002,223,8,223,101,2,224,224,1,224,223,223,1,66,174,224,101,-70,224,224,4,224,102,8,223,223,1001,224,6,224,1,223,224,223,4,223,99,0,0,0,677,0,0,0,0,0,0,0,0,0,0,0,1105,0,99999,1105,227,247,1105,1,99999,1005,227,99999,1005,0,256,1105,1,99999,1106,227,99999,1106,0,265,1105,1,99999,1006,0,99999,1006,227,274,1105,1,99999,1105,1,280,1105,1,99999,1,225,225,225,1101,294,0,0,105,1,0,1105,1,99999,1106,0,300,1105,1,99999,1,225,225,225,1101,314,0,0,106,0,0,1105,1,99999,108,226,226,224,102,2,223,223,1005,224,329,101,1,223,223,1107,226,677,224,1002,223,2,223,1005,224,344,101,1,223,223,8,226,677,224,102,2,223,223,1006,224,359,101,1,223,223,108,677,677,224,1002,223,2,223,1005,224,374,1001,223,1,223,1108,226,677,224,1002,223,2,223,1005,224,389,101,1,223,223,1007,677,677,224,1002,223,2,223,1006,224,404,1001,223,1,223,1108,677,677,224,102,2,223,223,1006,224,419,1001,223,1,223,1008,226,677,224,102,2,223,223,1005,224,434,101,1,223,223,107,677,677,224,102,2,223,223,1006,224,449,1001,223,1,223,1007,226,677,224,102,2,223,223,1005,224,464,101,1,223,223,7,677,226,224,102,2,223,223,1005,224,479,101,1,223,223,1007,226,226,224,102,2,223,223,1005,224,494,101,1,223,223,7,677,677,224,102,2,223,223,1006,224,509,101,1,223,223,1008,677,677,224,1002,223,2,223,1006,224,524,1001,223,1,223,108,226,677,224,1002,223,2,223,1006,224,539,101,1,223,223,8,226,226,224,102,2,223,223,1006,224,554,101,1,223,223,8,677,226,224,102,2,223,223,1005,224,569,1001,223,1,223,1108,677,226,224,1002,223,2,223,1006,224,584,101,1,223,223,1107,677,226,224,1002,223,2,223,1005,224,599,101,1,223,223,107,226,226,224,102,2,223,223,1006,224,614,1001,223,1,223,7,226,677,224,102,2,223,223,1005,224,629,1001,223,1,223,107,677,226,224,1002,223,2,223,1005,224,644,1001,223,1,223,1107,677,677,224,102,2,223,223,1006,224,659,101,1,223,223,1008,226,226,224,1002,223,2,223,1006,224,674,1001,223,1,223,4,223,99,226";

        internal int[] _OrigIntCodes;
        internal List<int> _IntCodes;
        private int _InstructionPointer = 0;
        internal int[] _ParamMode = new int[4];

        public Day05(string input = Input)
        {
            _OrigIntCodes = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => Convert.ToInt32(i)).ToArray();
            ReInit();
        }

        public void ReInit()
        {
            _IntCodes = new List<int>(_OrigIntCodes);
            _InstructionPointer = 0;
        }

        internal Queue<int> InputQueue = new Queue<int>();

        internal List<int> Output = new List<int>();

        int GetParamValue(int offset)
        {
            int mode = _ParamMode[offset];
            if (mode == 0)
                return _IntCodes[_IntCodes[_InstructionPointer + offset]];
            if (mode == 1)
                return _IntCodes[_InstructionPointer + offset];
            throw new NotSupportedException(mode.ToString());
        }

        void SetParamValue(int offset, int result)
        {
            int mode = _ParamMode[offset];
            if (mode == 0)
                _IntCodes[_IntCodes[_InstructionPointer + offset]] = result;
            else
                throw new NotSupportedException(mode.ToString());
        }

        bool RunNext()
        {
            int instructionOffset;
            int opCode = _IntCodes[_InstructionPointer];
            _ParamMode[1] = (opCode / 100) % 10;
            _ParamMode[2] = (opCode / 1000) % 10;
            _ParamMode[3] = (opCode / 10000) % 10;

            switch (opCode % 100)
            {
                case 1:
                    {
                        int value1 = GetParamValue(1);
                        int value2 = GetParamValue(2);
                        int result = value1 + value2;
                        SetParamValue(3, result);
                        instructionOffset = 4;
                    }
                    break;
                case 2:
                    {
                        int value1 = GetParamValue(1);
                        int value2 = GetParamValue(2);
                        int result = value1 * value2;
                        SetParamValue(3, result);
                        instructionOffset = 4;
                    }
                    break;
                case 3:
                    {
                        SetParamValue(1, InputQueue.Dequeue());
                        instructionOffset = 2;
                    }
                    break;
                case 4:
                    {
                        int value1 = GetParamValue(1);
                        if (value1 != 0)
                            Output.Add(value1);
                        instructionOffset = 2;
                    }
                    break;
                case 5: // jump-if-true
                    {
                        int value1 = GetParamValue(1);
                        int value2 = GetParamValue(2);
                        if (value1 != 0)
                        {
                            _InstructionPointer = value2;
                            instructionOffset = 0;
                        }
                        else
                            instructionOffset = 3;
                    }
                    break;
                case 6: // jump-if-false
                    {
                        int value1 = GetParamValue(1);
                        int value2 = GetParamValue(2);
                        if (value1 == 0)
                        {
                            _InstructionPointer = value2;
                            instructionOffset = 0;
                        }
                        else
                            instructionOffset = 3;
                    }
                    break;
                case 7: // less than
                    {
                        int value1 = GetParamValue(1);
                        int value2 = GetParamValue(2);
                        SetParamValue(3, value1 < value2 ? 1 : 0);
                        instructionOffset = 4;
                    }
                    break;
                case 8: // equals
                    {
                        int value1 = GetParamValue(1);
                        int value2 = GetParamValue(2);
                        SetParamValue(3, value1 == value2 ? 1 : 0);
                        instructionOffset = 4;
                    }
                    break;
                case 99:
                    return false;
                default:
                    throw new NotImplementedException(_IntCodes[_InstructionPointer].ToString());
            }

            _InstructionPointer += instructionOffset;
            return true;
        }

        internal void RunToFinish()
        {
            while (RunNext()) ;
        }

        #endregion

        internal int Part1()
        {
            InputQueue.Enqueue(1);
            RunToFinish();
            return Output.Single();
        }


        internal int Part2()
        {
            InputQueue.Enqueue(5);
            RunToFinish();
            return Output.Single();
        }
    }

    [TestClass]
    public class TestDay05
    {
        [TestMethod]
        public void Example1()
        {
            var d = new Day05("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9");
            d.InputQueue.Enqueue(0);
            d.RunToFinish();
            Assert.IsFalse(d.Output.Any());
        }
        [TestMethod]
        public void Example2()
        {
            var d = new Day05("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9");
            d.InputQueue.Enqueue(3);
            d.RunToFinish();
            Assert.AreEqual(1, d.Output.Single());
        }
        [TestMethod]
        public void Example3()
        {
            var d = new Day05("3,3,1105,-1,9,1101,0,0,12,4,12,99,1");
            d.InputQueue.Enqueue(0);
            d.RunToFinish();
            Assert.IsFalse(d.Output.Any());
        }

        [TestMethod]
        public void Example4()
        {
            var d = new Day05("3,3,1105,-1,9,1101,0,0,12,4,12,99,1");
            d.InputQueue.Enqueue(3);
            d.RunToFinish();
            Assert.AreEqual(1, d.Output.Single());
        }

        [TestMethod]
        public void Example5a()
        {
            var d = new Day05("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99");
            d.InputQueue.Enqueue(1);
            d.RunToFinish();
            Assert.AreEqual(999, d.Output.Single());
        }

        [TestMethod]
        public void Example5()
        {
            var d = new Day05("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99");
            d.InputQueue.Enqueue(7);
            d.RunToFinish();
            Assert.AreEqual(999, d.Output.Single());
        }
        [TestMethod]
        public void Example6()
        {
            var d = new Day05("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99");
            d.InputQueue.Enqueue(8);
            d.RunToFinish();
            Assert.AreEqual(1000, d.Output.Single());
        }

        [TestMethod]
        public void Example7()
        {
            var d = new Day05("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99");
            d.InputQueue.Enqueue(10);
            d.RunToFinish();
            Assert.AreEqual(1001, d.Output.Single());
        }

        [TestMethod]
        public void Part1()
        {
            Console.WriteLine(new Day05().Part1());
        }
        [TestMethod]
        public void Part2()
        {
            Console.WriteLine(new Day05().Part2());
        }
    }

}
