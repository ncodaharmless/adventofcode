using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2018
{
    class Day14
    {
        public int elf1Index = 0;
        public int elf2Index = 1;
        List<int> recipies = new List<int>();

        public int Count => recipies.Count;

        public Day14(string input = "37")
        {
            recipies.AddRange(input.ToCharArray().Select(c => int.Parse(c.ToString())));
        }

        public void Step()
        {
            int newRecipie = recipies[elf1Index] + recipies[elf2Index];
            recipies.AddRange(GetDigits(newRecipie));
            elf1Index = (elf1Index + 1 + recipies[elf1Index]) % recipies.Count;
            elf2Index = (elf2Index + 1 + recipies[elf2Index]) % recipies.Count;
        }

        internal void Part1()
        {
            while (recipies.Count < 260321 + 10)
            {
                Step();
            }
            Console.WriteLine(GetCurrentRecipiesList().Replace(" ", "").Substring(260321));
        }

        public int ReceipiesUntil(string matchRecipiesString)
        {
            int[] match = matchRecipiesString.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();

            int startIndex = 0;
            while (true)
            {
                startIndex = recipies.Count - match.Length - 1;
                Step();
                if (startIndex >= 0)
                {
                    for (int i = startIndex; i <= recipies.Count - match.Length; i++)
                    {
                        bool matched = true;
                        for (int ri = 0; ri < match.Length; ri++)
                            if (recipies[i + ri] != match[ri])
                            { matched = false; break; }
                        if (matched)
                            return i;
                    }
                }
            }
        }

        internal void Part2()
        {
            int item = ReceipiesUntil("260321");

            Console.WriteLine(item);
        }

        static IEnumerable<int> GetDigits(int number)
        {
            return number.ToString().ToCharArray().Select(c => int.Parse(c.ToString()));
        }

        public string GetCurrentRecipiesList() => string.Join(" ", recipies);
    }

    [TestClass]
    public class TestDay14
    {
        [TestMethod]
        public void TestSample()
        {
            var test = new Day14("37");
            test.Step();
            Assert.AreEqual("3 7 1 0", test.GetCurrentRecipiesList());
            Assert.AreEqual(0, test.elf1Index);
            Assert.AreEqual(1, test.elf2Index);
            test.Step();
            Assert.AreEqual("3 7 1 0 1 0", test.GetCurrentRecipiesList());
            Assert.AreEqual(4, test.elf1Index);
            Assert.AreEqual(3, test.elf2Index);
            test.Step();
            Assert.AreEqual("3 7 1 0 1 0 1", test.GetCurrentRecipiesList());
            Assert.AreEqual(6, test.elf1Index);
            Assert.AreEqual(4, test.elf2Index);

            test.Step();
            Assert.AreEqual("3 7 1 0 1 0 1 2", test.GetCurrentRecipiesList());
            test.Step();
            Assert.AreEqual("3 7 1 0 1 0 1 2 4", test.GetCurrentRecipiesList());
            test.Step();
            Assert.AreEqual("3 7 1 0 1 0 1 2 4 5", test.GetCurrentRecipiesList());
            test.Step();
            Assert.AreEqual("3 7 1 0 1 0 1 2 4 5 1", test.GetCurrentRecipiesList());
            test.Step();
            Assert.AreEqual("3 7 1 0 1 0 1 2 4 5 1 5", test.GetCurrentRecipiesList());
            test.Step();
            Assert.AreEqual("3 7 1 0 1 0 1 2 4 5 1 5 8", test.GetCurrentRecipiesList());
            test.Step();
            Assert.AreEqual("3 7 1 0 1 0 1 2 4 5 1 5 8 9", test.GetCurrentRecipiesList());
            test.Step();
            Assert.AreEqual("3 7 1 0 1 0 1 2 4 5 1 5 8 9 1 6", test.GetCurrentRecipiesList());
            test.Step();
            Assert.AreEqual("3 7 1 0 1 0 1 2 4 5 1 5 8 9 1 6 7", test.GetCurrentRecipiesList());
            test.Step();
            test.Step();
            Assert.AreEqual("5158916779", test.GetCurrentRecipiesList().Replace(" ", "").Substring(9));
        }

        [TestMethod]
        [DataRow("51589", 9)]
        [DataRow("01245", 5)]
        [DataRow("92510", 18)]
        [DataRow("59414", 2018)]
        public void TestPart2(string input, int count)
        {
            var test = new Day14();
            Assert.AreEqual(count, test.ReceipiesUntil(input));
        }
    }
}
