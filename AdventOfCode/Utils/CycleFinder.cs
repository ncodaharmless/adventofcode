using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{
    class CycleFinder
    {
        private List<int> _History = new List<int>();

        public bool Found { get; private set; }

        public int[] Cycle => _History.Take(_CycleLength).ToArray();

        private int _CycleLength = 1;

        public int MinCycleSize = 4;

        public void Add(int value)
        {
            if (Found) return;

            int verifyIndex = _History.Count % _CycleLength;
            _History.Add(value);
            if (_History[verifyIndex] != value)
            {
                _CycleLength++;
                while (_CycleLength < _History.Count)
                {
                    bool nextCycleOk = true; ;
                    for (int i = 0; i < _CycleLength; i++)
                    {
                        if (i + _CycleLength >= _History.Count) break;
                        if (_History[i] != _History[i + _CycleLength])
                        {
                            nextCycleOk = false;
                            break;
                        }
                    }
                    if (nextCycleOk) break;
                    _CycleLength++;
                }
            }
            else if (_CycleLength >= MinCycleSize && _History.Count >= _CycleLength * 2)
            {
                Found = true;
                return;
            }

        }
    }

    [TestClass]
    public class CycleFinderTest
    {
        [TestMethod]
        [DataRow("123412341234", "1234")]
        [DataRow("121231212312123", "12123")]
        [DataRow("112112112112", "112")]
        [DataRow("121312131213", "1213")]
        [DataRow("121231212312123", "12123")]
        [DataRow("122121212122121212122121212", "122121212")]
        [DataRow("123214643234211232146432342112321464323421", "12321464323421")]
        public void TestCycle(string input, string expectedCycle)
        {
            CycleFinder cycle = new CycleFinder();
            cycle.MinCycleSize = 3;
            int index = 0;
            while (!cycle.Found)
            {
                cycle.Add(Convert.ToInt32(input[index].ToString()));
                index++;
            }
            Assert.AreEqual(expectedCycle, new string(cycle.Cycle.Select(c => c.ToString()[0]).ToArray()));
        }
    }
}
