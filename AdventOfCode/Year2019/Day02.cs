using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{
    class Day02
    {
        #region Input

        private const string Input = @"1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,9,1,19,1,9,19,23,1,23,5,27,2,27,10,31,1,6,31,35,1,6,35,39,2,9,39,43,1,6,43,47,1,47,5,51,1,51,13,55,1,55,13,59,1,59,5,63,2,63,6,67,1,5,67,71,1,71,13,75,1,10,75,79,2,79,6,83,2,9,83,87,1,5,87,91,1,91,5,95,2,9,95,99,1,6,99,103,1,9,103,107,2,9,107,111,1,111,6,115,2,9,115,119,1,119,6,123,1,123,9,127,2,127,13,131,1,131,9,135,1,10,135,139,2,139,10,143,1,143,5,147,2,147,6,151,1,151,5,155,1,2,155,159,1,6,159,0,99,2,0,14,0";

        internal int[] _OrigIntCodes;
        internal List<int> _IntCodes;
        private int _InstructionPointer = 0;

        public Day02(string input = Input)
        {
            _OrigIntCodes = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => Convert.ToInt32(i)).ToArray();
            ReInit();
        }

        public void ReInit()
        {
            _IntCodes = new List<int>(_OrigIntCodes);
            _InstructionPointer = 0;
        }

        bool RunNext()
        {
            switch (_IntCodes[_InstructionPointer])
            {
                case 1:
                    {
                        int value1 = _IntCodes[_IntCodes[_InstructionPointer + 1]];
                        int value2 = _IntCodes[_IntCodes[_InstructionPointer + 2]];
                        int result = value1 + value2;
                        _IntCodes[_IntCodes[_InstructionPointer + 3]] = result;
                    }
                    break;
                case 2:
                    {
                        int value1 = _IntCodes[_IntCodes[_InstructionPointer + 1]];
                        int value2 = _IntCodes[_IntCodes[_InstructionPointer + 2]];
                        int result = value1 * value2;
                        _IntCodes[_IntCodes[_InstructionPointer + 3]] = result;
                    }
                    break;
                case 99:
                    return false;
                default:
                    throw new NotImplementedException(_IntCodes[_InstructionPointer].ToString());
            }

            _InstructionPointer += 4;
            return true;
        }

        int RunToFinish()
        {
            while (RunNext()) ;
            return _IntCodes[0];
        }

        #endregion

        internal int Part1()
        {
            _IntCodes[1] = 12;
            _IntCodes[2] = 2;
            return RunToFinish();
        }

        internal int Part2()
        {
            const int MatchResult = 19690720;
            for (int a = 0; a <= 99; a++)
            {
                for (int b = 0; b <= 99; b++)
                {
                    ReInit();
                    _IntCodes[1] = a;
                    _IntCodes[2] = b;
                    if (RunToFinish() == MatchResult)
                    {
                        return 100 * a + b;
                    }
                }
            }
            throw new NotSupportedException();
        }

        [TestClass]
        public class TestDay02
        {
            [TestMethod]
            public void ExampleDay2()
            {
                var d = new Day02("1,9,10,3,2,3,11,0,99,30,40,50");
                Assert.IsTrue(d.RunNext());
                Assert.AreEqual(70, d._IntCodes[3]);
                Assert.IsTrue(d.RunNext());
                Assert.AreEqual(3500, d._IntCodes[0]);
                Assert.IsFalse(d.RunNext());
            }

            [TestMethod]
            public void Part1()
            {
                Console.WriteLine(new Day02().Part1());
            }
            [TestMethod]
            public void Part2()
            {
                Console.WriteLine(new Day02().Part2());
            }
        }
    }
}
