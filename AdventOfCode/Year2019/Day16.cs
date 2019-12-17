using AdventOfCode.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{


    class Day16
    {
        #region Input
        const string Input = @"59709511599794439805414014219880358445064269099345553494818286560304063399998657801629526113732466767578373307474609375929817361595469200826872565688108197109235040815426214109531925822745223338550232315662686923864318114370485155264844201080947518854684797571383091421294624331652208294087891792537136754322020911070917298783639755047408644387571604201164859259810557018398847239752708232169701196560341721916475238073458804201344527868552819678854931434638430059601039507016639454054034562680193879342212848230089775870308946301489595646123293699890239353150457214490749319019572887046296522891429720825181513685763060659768372996371503017206185697";
        #endregion

        public int[] Numbers;

        public Day16(string input = Input)
        {
            Numbers = input.Select(c => Convert.ToInt32(c.ToString())).ToArray();
        }

        internal string Part1()
        {
            for (int i = 0; i < 100; i++)
                Phase();
            return Output(0, 8);
        }

        internal string Part2()
        {
            List<int> numbers2 = new List<int>();
            for (int i = 0; i < 10000; i++)
                numbers2.AddRange(Numbers);
            numbers2.ToArray();

            int resultOffset = Convert.ToInt32(string.Join("", numbers2.Take(7)));

            Numbers = numbers2.Skip(resultOffset).ToArray();

            for (int i = 0; i < 100; i++)
                Phase(0);
            return Output().Substring(0, 8);
        }

        public void Phase(int offset = 0)
        {
            int numbersLength = Numbers.Length;
            int[] pattern = new int[] { 0, 1, 0, -1 };
            int[] output = new int[numbersLength];
            for (int i = offset; i < numbersLength; i++)
            {
                int result = 0;
                int patternIndex = 0;
                int repeatCount = i;
                for (int j = i; j < numbersLength; j++)
                {
                    repeatCount++;
                    if (repeatCount == (i + 1))
                    {
                        patternIndex = (patternIndex + 1) % pattern.Length;
                        repeatCount = 0;
                    }

                    result += pattern[patternIndex] * Numbers[j];
                }
                output[i] = Math.Abs(result % 10);
            }
            Numbers = output;
        }

        public string Output(int offset = 0, int length = 8)
        {
            return string.Join("", Numbers.Skip(offset).Take(length));
        }
    }

    [TestClass]
    public class TestDay16
    {
        [TestMethod]
        public void Example1()
        {
            var d = new Day16("12345678");
            d.Phase();
            Assert.AreEqual("48226158", d.Output());
            d.Phase();
            Assert.AreEqual("34040438", d.Output());
            d.Phase();
            Assert.AreEqual("03415518", d.Output());
            d.Phase();
            Assert.AreEqual("01029498", d.Output());
        }

        [TestMethod]
        public void Part1()
        {
            Assert.AreEqual("34841690", new Day16().Part1());
        }
        [TestMethod, Ignore]
        public void Part2()
        {
            Assert.AreEqual(0, new Day16().Part2());
        }
    }

}
