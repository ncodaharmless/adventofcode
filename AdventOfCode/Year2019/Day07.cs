using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{
    #region IntcodeComputer
    class IntcodeComputer
    {
        internal int[] _OrigIntCodes;
        internal List<int> _IntCodes;
        private int _InstructionPointer = 0;
        internal int[] _ParamMode = new int[4];


        internal Queue<int> InputQueue = new Queue<int>();
        internal Queue<int> Output = new Queue<int>();
        public bool HasFinished { get; private set; }

        public IntcodeComputer(int[] input)
        {
            _OrigIntCodes = input;
            ReInit();
        }

        public void ReInit()
        {
            _IntCodes = new List<int>(_OrigIntCodes);
            _InstructionPointer = 0;
            _ParamMode = new int[4];
            InputQueue = new Queue<int>();
            Output = new Queue<int>();
            HasFinished = false;
        }

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

        // if it's opcode 3, then requires input
        public bool IsReady => InputQueue.Count > 0 || _IntCodes[_InstructionPointer] % 100 != 3;

        public bool RunNext()
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
                        Output.Enqueue(value1);
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
                    HasFinished = true;
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

    }

    #endregion

    class Day07
    {
        #region Input

        private const string Input = @"3,8,1001,8,10,8,105,1,0,0,21,38,55,72,93,118,199,280,361,442,99999,3,9,1001,9,2,9,1002,9,5,9,101,4,9,9,4,9,99,3,9,1002,9,3,9,1001,9,5,9,1002,9,4,9,4,9,99,3,9,101,4,9,9,1002,9,3,9,1001,9,4,9,4,9,99,3,9,1002,9,4,9,1001,9,4,9,102,5,9,9,1001,9,4,9,4,9,99,3,9,101,3,9,9,1002,9,3,9,1001,9,3,9,102,5,9,9,101,4,9,9,4,9,99,3,9,101,1,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,99,3,9,1001,9,1,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,101,1,9,9,4,9,99,3,9,101,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,101,1,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,99,3,9,1001,9,1,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,99,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,99";

        IntcodeComputer[] amps = new IntcodeComputer[5];

        public Day07(string input = Input)
        {
            int[] intCompCode = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => Convert.ToInt32(i)).ToArray();
            for (int i = 0; i < 5; i++)
                amps[i] = new IntcodeComputer(intCompCode);
        }

        #endregion

        private int RunPhase1(int[] phase)
        {
            int lastResult = 0;
            for (int i = 0; i < 5; i++)
                amps[i].ReInit();

            for (int i = 0; i < 5; i++)
            {
                amps[i].InputQueue.Enqueue(phase[i]);
                amps[i].InputQueue.Enqueue(lastResult);
                amps[i].RunToFinish();
                lastResult = amps[i].Output.Dequeue();
            }

            return lastResult;
        }

        internal int Part1()
        {
            int highestValue = 0;
            int[] highestPhase = null;
            for (int a = 0; a < 5; a++)
            {
                for (int b = 0; b < 5; b++)
                {
                    if (a == b) continue;
                    for (int c = 0; c < 5; c++)
                    {
                        if (a == c || b == c) continue;
                        for (int d = 0; d < 5; d++)
                        {
                            if (a == d || b == d || c == d) continue;
                            for (int e = 0; e < 5; e++)
                            {
                                if (a == e || b == e || c == e || d == e) continue;
                                int[] phase = new int[] { a, b, c, d, e };
                                int lastResult = RunPhase1(phase);
                                if (lastResult > highestValue)
                                {
                                    highestValue = lastResult;
                                    highestPhase = phase;
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine(string.Join(",", highestPhase));
            return highestValue;
        }

        private int RunPhase2(int[] phase)
        {
            for (int i = 0; i < 5; i++)
            {
                amps[i].ReInit();
                amps[i].InputQueue.Enqueue(phase[i]);
            }
            amps[0].InputQueue.Enqueue(0);
            for (int i = 0; i < 5; i++)
                amps[i].Output = amps[(i + 1) % 5].InputQueue;
            bool hasFinished = false;
            while (!hasFinished)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (amps[i].IsReady)
                        hasFinished = !amps[i].RunNext();
                }
            }

            return amps[4].Output.Dequeue();
        }

        internal int Part2()
        {
            int highestValue = 0;
            int[] highestPhase = null;
            for (int a = 0; a < 5; a++)
            {
                for (int b = 0; b < 5; b++)
                {
                    if (a == b) continue;
                    for (int c = 0; c < 5; c++)
                    {
                        if (a == c || b == c) continue;
                        for (int d = 0; d < 5; d++)
                        {
                            if (a == d || b == d || c == d) continue;
                            for (int e = 0; e < 5; e++)
                            {
                                if (a == e || b == e || c == e || d == e) continue;
                                int[] phase = new int[] { a + 5, b + 5, c + 5, d + 5, e + 5 };
                                int lastResult = RunPhase2(phase);
                                if (lastResult > highestValue)
                                {
                                    highestValue = lastResult;
                                    highestPhase = phase;
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine(string.Join(",", highestPhase));
            return highestValue;
        }
    }

    [TestClass]
    public class TestDay07
    {
        [TestMethod]
        public void Example1()
        {
            Assert.AreEqual(43210, new Day07("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0").Part1());
        }

        [TestMethod]
        public void Example2()
        {
            Assert.AreEqual(54321, new Day07(@"3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0").Part1());
        }

        [TestMethod]
        public void Example3()
        {
            Assert.AreEqual(65210, new Day07(@"3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0").Part1());
        }

        [TestMethod]
        public void Example4()
        {
            Assert.AreEqual(139629729, new Day07(@"3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5").Part2());
        }

        [TestMethod]
        public void Example5()
        {
            Assert.AreEqual(18216, new Day07(@"3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10").Part2());
        }

        [TestMethod]
        public void Part1()
        {
            Console.WriteLine(new Day07().Part1());
        }
        [TestMethod]
        public void Part2()
        {
            Console.WriteLine(new Day07().Part2());
        }
    }

}
