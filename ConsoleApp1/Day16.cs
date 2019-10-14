using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Day16
    {
        internal static string[] opcodes = new string[] { "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "setr", "seti", "gtir", "gtri", "gtrr", "eqir", "eqri", "eqrr" };

        internal static void RunStatement(string opcode, int valueA, int valueB, int outputC, int[] registers)
        {
            switch (opcode)
            {
                case "addr":
                    registers[outputC] = registers[valueA] + registers[valueB];
                    break;
                case "addi":
                    registers[outputC] = registers[valueA] + valueB;
                    break;
                case "mulr":
                    registers[outputC] = registers[valueA] * registers[valueB];
                    break;
                case "muli":
                    registers[outputC] = registers[valueA] * valueB;
                    break;
                case "banr":
                    registers[outputC] = registers[valueA] & registers[valueB];
                    break;
                case "bani":
                    registers[outputC] = registers[valueA] & valueB;
                    break;
                case "borr":
                    registers[outputC] = registers[valueA] | registers[valueB];
                    break;
                case "bori":
                    registers[outputC] = registers[valueA] | valueB;
                    break;
                case "setr":
                    registers[outputC] = registers[valueA];
                    break;
                case "seti":
                    registers[outputC] = valueA;
                    break;
                case "gtir":
                    registers[outputC] = valueA > registers[valueB] ? 1 : 0;
                    break;
                case "gtri":
                    registers[outputC] = registers[valueA] > valueB ? 1 : 0;
                    break;
                case "gtrr":
                    registers[outputC] = registers[valueA] > registers[valueB] ? 1 : 0;
                    break;
                case "eqir":
                    registers[outputC] = valueA == registers[valueB] ? 1 : 0;
                    break;
                case "eqri":
                    registers[outputC] = registers[valueA] == valueB ? 1 : 0;
                    break;
                case "eqrr":
                    registers[outputC] = registers[valueA] == registers[valueB] ? 1 : 0;
                    break;
                default:
                    throw new NotImplementedException(opcode);
            }
        }

        public static void Part1()
        {
            int[] registersInitial = null;
            int[] test = null;
            int countMatching3 = 0;
            foreach (string line in SampleOpcodes.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.StartsWith("Before:"))
                    registersInitial = line.Substring(9).TrimEnd(']').Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
                else if (line.StartsWith("After:"))
                {
                    int[] result = line.Substring(9).TrimEnd(']').Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
                    int matchCount = MatchCount(registersInitial, test, result, out _);
                    if (matchCount >= 3)
                        countMatching3++;
                }
                else
                    test = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
            }
            Console.WriteLine(countMatching3);
        }

        public static void Part2()
        {
            int[] registersInitial = null;
            int[] test = null;
            Dictionary<string, List<int>> possibleCombinations = new Dictionary<string, List<int>>();
            foreach (string opcode in opcodes)
            {
                List<int> list = new List<int>(16);
                for (int i = 0; i < 16; i++)
                    list.Add(i);
                possibleCombinations.Add(opcode, list);
            }
            foreach (string line in SampleOpcodes.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.StartsWith("Before:"))
                    registersInitial = line.Substring(9).TrimEnd(']').Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
                else if (line.StartsWith("After:"))
                {
                    int[] result = line.Substring(9).TrimEnd(']').Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
                    foreach (string opcode in opcodes)
                    {
                        int[] registerClone = registersInitial.ToArray();
                        RunStatement(opcode, test[1], test[2], test[3], registerClone);
                        if (!registerClone.SequenceEqual(result))
                        {
                            possibleCombinations[opcode].Remove(test[0]);
                        }
                    }
                }
                else
                    test = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
            }
            Dictionary<int, string> combination = BuildCorrectCombination(new Dictionary<int, string>(), possibleCombinations);

            int[] registers = new int[4];
            foreach (string line in SampleProgram.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int[] lineItems = line.Trim().Split(new char[] { ' ' }).Select(i => int.Parse(i)).ToArray();
                RunStatement(combination[lineItems[0]], lineItems[1], lineItems[2], lineItems[3], registers);
            }
            Console.WriteLine(registers[0]);
        }

        private static Dictionary<int, string> BuildCorrectCombination(Dictionary<int, string> combination, Dictionary<string, List<int>> possibleCombinations)
        {
            if (combination.Count == 16)
            {
                if (IsOpcodeLookupCorrect(combination))
                    return combination;
                return null;
            }
            foreach (int i in possibleCombinations[opcodes[combination.Count]])
            {
                if (!combination.ContainsKey(i))
                {
                    var clone = new Dictionary<int, string>(combination);
                    clone.Add(i, opcodes[combination.Count]);
                    var result = BuildCorrectCombination(clone, possibleCombinations);
                    if (result != null) return result;
                }
            }
            return null;
        }

        private static bool IsOpcodeLookupCorrect(Dictionary<int, string> lookup)
        {
            int[] registersInitial = null;
            int[] test = null;
            foreach (string line in SampleOpcodes.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.StartsWith("Before:"))
                    registersInitial = line.Substring(9).TrimEnd(']').Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
                else if (line.StartsWith("After:"))
                {
                    int[] result = line.Substring(9).TrimEnd(']').Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
                    int[] registerClone = registersInitial.ToArray();
                    RunStatement(lookup[test[0]], test[1], test[2], test[3], registerClone);
                    if (!registerClone.SequenceEqual(result))
                    {
                        return false;
                    }
                }
                else
                    test = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
            }
            return true;
        }

        internal static int MatchCount(int[] registersInitial, int[] test, int[] expectedResult, out string lastMatch)
        {
            lastMatch = null;
            int matchCount = 0;
            foreach (string opcode in opcodes)
            {
                int[] registerClone = registersInitial.ToArray();
                RunStatement(opcode, test[1], test[2], test[3], registerClone);
                if (registerClone.SequenceEqual(expectedResult))
                {
                    lastMatch = opcode;
                    matchCount++;
                }
            }

            return matchCount;
        }

        #region Sample Input

        const string SampleOpcodes = @"
Before: [0, 1, 2, 1]
14 1 3 3
After:  [0, 1, 2, 1]

Before: [3, 2, 2, 3]
13 2 1 3
After:  [3, 2, 2, 1]

Before: [2, 2, 2, 2]
13 2 1 0
After:  [1, 2, 2, 2]

Before: [0, 1, 0, 3]
12 1 0 2
After:  [0, 1, 1, 3]

Before: [0, 1, 0, 3]
11 3 1 3
After:  [0, 1, 0, 0]

Before: [0, 1, 2, 1]
15 1 2 2
After:  [0, 1, 0, 1]

Before: [2, 1, 2, 0]
15 1 2 0
After:  [0, 1, 2, 0]

Before: [3, 1, 2, 0]
11 2 2 1
After:  [3, 1, 2, 0]

Before: [3, 2, 2, 1]
3 3 2 3
After:  [3, 2, 2, 1]

Before: [1, 0, 3, 3]
11 3 3 2
After:  [1, 0, 1, 3]

Before: [1, 0, 2, 1]
5 0 2 0
After:  [0, 0, 2, 1]

Before: [1, 1, 3, 0]
9 1 2 1
After:  [1, 0, 3, 0]

Before: [0, 1, 1, 1]
12 1 0 1
After:  [0, 1, 1, 1]

Before: [1, 3, 2, 2]
5 0 2 1
After:  [1, 0, 2, 2]

Before: [0, 1, 0, 3]
12 1 0 0
After:  [1, 1, 0, 3]

Before: [2, 3, 2, 2]
13 2 0 2
After:  [2, 3, 1, 2]

Before: [1, 1, 3, 1]
1 1 0 0
After:  [1, 1, 3, 1]

Before: [3, 2, 3, 3]
11 3 3 0
After:  [1, 2, 3, 3]

Before: [0, 0, 2, 1]
6 0 0 3
After:  [0, 0, 2, 0]

Before: [1, 3, 0, 3]
4 0 2 3
After:  [1, 3, 0, 0]

Before: [1, 1, 2, 0]
5 0 2 3
After:  [1, 1, 2, 0]

Before: [1, 1, 1, 2]
1 1 0 1
After:  [1, 1, 1, 2]

Before: [3, 0, 1, 1]
10 3 3 3
After:  [3, 0, 1, 0]

Before: [3, 0, 2, 3]
11 3 3 3
After:  [3, 0, 2, 1]

Before: [1, 1, 0, 3]
2 1 3 1
After:  [1, 0, 0, 3]

Before: [1, 1, 2, 0]
1 1 0 2
After:  [1, 1, 1, 0]

Before: [1, 1, 2, 2]
5 0 2 2
After:  [1, 1, 0, 2]

Before: [0, 1, 2, 0]
12 1 0 1
After:  [0, 1, 2, 0]

Before: [0, 1, 2, 0]
12 1 0 2
After:  [0, 1, 1, 0]

Before: [0, 1, 0, 0]
12 1 0 2
After:  [0, 1, 1, 0]

Before: [2, 1, 2, 0]
15 1 2 2
After:  [2, 1, 0, 0]

Before: [1, 1, 2, 1]
15 1 2 2
After:  [1, 1, 0, 1]

Before: [1, 1, 2, 0]
1 1 0 0
After:  [1, 1, 2, 0]

Before: [2, 1, 0, 0]
7 2 0 2
After:  [2, 1, 1, 0]

Before: [0, 0, 3, 0]
0 3 2 0
After:  [1, 0, 3, 0]

Before: [1, 3, 0, 2]
4 0 2 1
After:  [1, 0, 0, 2]

Before: [1, 0, 2, 2]
5 0 2 2
After:  [1, 0, 0, 2]

Before: [1, 0, 2, 3]
5 0 2 0
After:  [0, 0, 2, 3]

Before: [0, 0, 3, 3]
13 3 2 1
After:  [0, 1, 3, 3]

Before: [0, 3, 1, 1]
6 0 0 2
After:  [0, 3, 0, 1]

Before: [3, 1, 2, 0]
15 1 2 2
After:  [3, 1, 0, 0]

Before: [1, 3, 3, 3]
13 3 2 0
After:  [1, 3, 3, 3]

Before: [1, 0, 2, 1]
5 0 2 3
After:  [1, 0, 2, 0]

Before: [3, 1, 3, 2]
9 1 2 0
After:  [0, 1, 3, 2]

Before: [0, 1, 3, 3]
12 1 0 1
After:  [0, 1, 3, 3]

Before: [3, 3, 0, 2]
0 2 3 2
After:  [3, 3, 1, 2]

Before: [0, 1, 3, 0]
12 1 0 2
After:  [0, 1, 1, 0]

Before: [3, 2, 2, 2]
11 2 2 3
After:  [3, 2, 2, 1]

Before: [3, 1, 3, 0]
9 1 2 3
After:  [3, 1, 3, 0]

Before: [0, 3, 2, 3]
8 2 2 2
After:  [0, 3, 2, 3]

Before: [2, 2, 1, 3]
2 2 3 0
After:  [0, 2, 1, 3]

Before: [2, 2, 2, 2]
8 2 2 0
After:  [2, 2, 2, 2]

Before: [0, 1, 3, 3]
2 1 3 0
After:  [0, 1, 3, 3]

Before: [1, 1, 3, 2]
1 1 0 2
After:  [1, 1, 1, 2]

Before: [0, 1, 2, 3]
15 1 2 1
After:  [0, 0, 2, 3]

Before: [1, 1, 2, 1]
5 0 2 2
After:  [1, 1, 0, 1]

Before: [3, 0, 3, 3]
13 3 2 2
After:  [3, 0, 1, 3]

Before: [1, 1, 2, 1]
14 1 3 0
After:  [1, 1, 2, 1]

Before: [0, 1, 1, 3]
12 1 0 3
After:  [0, 1, 1, 1]

Before: [1, 3, 2, 1]
8 2 2 3
After:  [1, 3, 2, 2]

Before: [1, 3, 2, 2]
5 0 2 0
After:  [0, 3, 2, 2]

Before: [0, 1, 1, 3]
12 1 0 2
After:  [0, 1, 1, 3]

Before: [1, 1, 2, 3]
15 1 2 1
After:  [1, 0, 2, 3]

Before: [1, 2, 2, 3]
5 0 2 2
After:  [1, 2, 0, 3]

Before: [3, 1, 0, 2]
10 3 3 2
After:  [3, 1, 0, 2]

Before: [2, 1, 0, 1]
14 1 3 3
After:  [2, 1, 0, 1]

Before: [2, 0, 1, 3]
2 2 3 0
After:  [0, 0, 1, 3]

Before: [1, 3, 3, 2]
10 3 3 2
After:  [1, 3, 0, 2]

Before: [0, 1, 2, 3]
15 1 2 2
After:  [0, 1, 0, 3]

Before: [3, 3, 1, 3]
2 2 3 2
After:  [3, 3, 0, 3]

Before: [1, 1, 1, 0]
1 1 0 1
After:  [1, 1, 1, 0]

Before: [2, 1, 3, 2]
7 3 2 2
After:  [2, 1, 1, 2]

Before: [1, 3, 1, 3]
2 2 3 2
After:  [1, 3, 0, 3]

Before: [2, 1, 3, 1]
9 1 2 3
After:  [2, 1, 3, 0]

Before: [1, 1, 0, 2]
0 2 3 0
After:  [1, 1, 0, 2]

Before: [3, 3, 1, 3]
13 3 0 1
After:  [3, 1, 1, 3]

Before: [1, 0, 0, 1]
4 0 2 3
After:  [1, 0, 0, 0]

Before: [2, 2, 2, 2]
13 2 0 1
After:  [2, 1, 2, 2]

Before: [0, 0, 3, 2]
6 0 0 0
After:  [0, 0, 3, 2]

Before: [1, 2, 0, 0]
4 0 2 3
After:  [1, 2, 0, 0]

Before: [1, 1, 0, 0]
1 1 0 2
After:  [1, 1, 1, 0]

Before: [3, 1, 2, 1]
15 1 2 1
After:  [3, 0, 2, 1]

Before: [0, 0, 3, 0]
0 3 2 3
After:  [0, 0, 3, 1]

Before: [2, 1, 3, 1]
14 1 3 1
After:  [2, 1, 3, 1]

Before: [0, 3, 3, 2]
6 0 0 3
After:  [0, 3, 3, 0]

Before: [1, 1, 1, 0]
1 1 0 0
After:  [1, 1, 1, 0]

Before: [0, 0, 0, 2]
11 0 0 3
After:  [0, 0, 0, 1]

Before: [3, 1, 0, 3]
2 1 3 0
After:  [0, 1, 0, 3]

Before: [3, 3, 3, 2]
7 3 2 2
After:  [3, 3, 1, 2]

Before: [1, 1, 3, 1]
1 1 0 3
After:  [1, 1, 3, 1]

Before: [2, 1, 3, 0]
9 1 2 1
After:  [2, 0, 3, 0]

Before: [2, 3, 0, 1]
7 2 0 0
After:  [1, 3, 0, 1]

Before: [0, 2, 3, 3]
13 3 2 2
After:  [0, 2, 1, 3]

Before: [0, 3, 1, 3]
2 2 3 2
After:  [0, 3, 0, 3]

Before: [2, 3, 2, 3]
11 3 2 0
After:  [0, 3, 2, 3]

Before: [1, 0, 1, 3]
2 2 3 2
After:  [1, 0, 0, 3]

Before: [3, 1, 1, 1]
14 1 3 0
After:  [1, 1, 1, 1]

Before: [1, 1, 2, 1]
1 1 0 3
After:  [1, 1, 2, 1]

Before: [1, 2, 2, 1]
3 3 2 0
After:  [1, 2, 2, 1]

Before: [0, 0, 3, 2]
7 3 2 0
After:  [1, 0, 3, 2]

Before: [1, 1, 2, 3]
5 0 2 2
After:  [1, 1, 0, 3]

Before: [3, 1, 2, 1]
3 3 2 1
After:  [3, 1, 2, 1]

Before: [1, 1, 1, 1]
1 1 0 1
After:  [1, 1, 1, 1]

Before: [3, 2, 2, 3]
8 2 2 1
After:  [3, 2, 2, 3]

Before: [3, 1, 1, 1]
14 1 3 3
After:  [3, 1, 1, 1]

Before: [2, 1, 3, 2]
10 3 3 2
After:  [2, 1, 0, 2]

Before: [0, 1, 0, 3]
6 0 0 1
After:  [0, 0, 0, 3]

Before: [0, 1, 2, 2]
12 1 0 2
After:  [0, 1, 1, 2]

Before: [0, 1, 2, 1]
15 1 2 1
After:  [0, 0, 2, 1]

Before: [2, 0, 2, 1]
3 3 2 1
After:  [2, 1, 2, 1]

Before: [0, 1, 2, 1]
12 1 0 3
After:  [0, 1, 2, 1]

Before: [1, 1, 2, 2]
10 3 3 2
After:  [1, 1, 0, 2]

Before: [2, 0, 3, 3]
8 3 3 2
After:  [2, 0, 3, 3]

Before: [1, 2, 0, 3]
4 0 2 1
After:  [1, 0, 0, 3]

Before: [1, 2, 0, 1]
4 0 2 2
After:  [1, 2, 0, 1]

Before: [0, 0, 3, 0]
0 3 2 2
After:  [0, 0, 1, 0]

Before: [1, 3, 3, 0]
0 3 2 0
After:  [1, 3, 3, 0]

Before: [0, 1, 3, 3]
2 1 3 2
After:  [0, 1, 0, 3]

Before: [0, 1, 3, 2]
6 0 0 3
After:  [0, 1, 3, 0]

Before: [1, 1, 1, 3]
1 1 0 2
After:  [1, 1, 1, 3]

Before: [1, 0, 0, 3]
4 0 2 0
After:  [0, 0, 0, 3]

Before: [3, 1, 3, 0]
9 1 2 2
After:  [3, 1, 0, 0]

Before: [1, 1, 1, 2]
1 1 0 0
After:  [1, 1, 1, 2]

Before: [1, 1, 2, 0]
5 0 2 0
After:  [0, 1, 2, 0]

Before: [0, 0, 0, 1]
6 0 0 3
After:  [0, 0, 0, 0]

Before: [0, 1, 2, 3]
11 3 2 1
After:  [0, 0, 2, 3]

Before: [1, 2, 2, 3]
5 0 2 3
After:  [1, 2, 2, 0]

Before: [0, 3, 2, 1]
6 0 0 3
After:  [0, 3, 2, 0]

Before: [0, 1, 3, 0]
9 1 2 2
After:  [0, 1, 0, 0]

Before: [1, 1, 1, 1]
10 2 3 1
After:  [1, 0, 1, 1]

Before: [1, 0, 2, 3]
8 3 3 3
After:  [1, 0, 2, 3]

Before: [2, 1, 2, 1]
8 2 2 0
After:  [2, 1, 2, 1]

Before: [2, 0, 0, 1]
7 2 0 2
After:  [2, 0, 1, 1]

Before: [1, 0, 2, 3]
5 0 2 2
After:  [1, 0, 0, 3]

Before: [1, 1, 3, 3]
13 3 2 3
After:  [1, 1, 3, 1]

Before: [1, 1, 0, 3]
1 1 0 0
After:  [1, 1, 0, 3]

Before: [2, 2, 2, 3]
13 2 1 3
After:  [2, 2, 2, 1]

Before: [1, 3, 2, 3]
5 0 2 1
After:  [1, 0, 2, 3]

Before: [1, 2, 2, 3]
11 2 2 0
After:  [1, 2, 2, 3]

Before: [3, 3, 2, 3]
8 3 3 1
After:  [3, 3, 2, 3]

Before: [2, 1, 2, 1]
14 1 3 1
After:  [2, 1, 2, 1]

Before: [1, 1, 2, 1]
3 3 2 1
After:  [1, 1, 2, 1]

Before: [2, 1, 2, 2]
8 2 2 0
After:  [2, 1, 2, 2]

Before: [3, 0, 3, 0]
0 3 2 2
After:  [3, 0, 1, 0]

Before: [1, 1, 3, 3]
9 1 2 1
After:  [1, 0, 3, 3]

Before: [1, 1, 0, 1]
1 1 0 2
After:  [1, 1, 1, 1]

Before: [3, 0, 0, 2]
0 2 3 2
After:  [3, 0, 1, 2]

Before: [1, 0, 0, 3]
4 0 2 1
After:  [1, 0, 0, 3]

Before: [2, 0, 3, 1]
10 3 3 3
After:  [2, 0, 3, 0]

Before: [0, 0, 0, 2]
0 2 3 1
After:  [0, 1, 0, 2]

Before: [0, 2, 2, 3]
2 1 3 2
After:  [0, 2, 0, 3]

Before: [2, 1, 3, 3]
9 1 2 0
After:  [0, 1, 3, 3]

Before: [1, 1, 2, 0]
5 0 2 2
After:  [1, 1, 0, 0]

Before: [0, 1, 1, 1]
12 1 0 0
After:  [1, 1, 1, 1]

Before: [2, 3, 3, 0]
0 3 2 2
After:  [2, 3, 1, 0]

Before: [2, 2, 0, 3]
7 2 0 1
After:  [2, 1, 0, 3]

Before: [2, 1, 0, 0]
7 2 0 0
After:  [1, 1, 0, 0]

Before: [0, 1, 1, 3]
11 3 2 2
After:  [0, 1, 0, 3]

Before: [3, 2, 1, 0]
7 3 0 3
After:  [3, 2, 1, 1]

Before: [1, 1, 2, 3]
15 1 2 2
After:  [1, 1, 0, 3]

Before: [1, 1, 1, 2]
1 1 0 3
After:  [1, 1, 1, 1]

Before: [3, 1, 2, 0]
15 1 2 0
After:  [0, 1, 2, 0]

Before: [3, 1, 2, 1]
14 1 3 1
After:  [3, 1, 2, 1]

Before: [2, 1, 3, 3]
9 1 2 2
After:  [2, 1, 0, 3]

Before: [2, 3, 2, 1]
3 3 2 3
After:  [2, 3, 2, 1]

Before: [1, 2, 3, 3]
13 3 2 3
After:  [1, 2, 3, 1]

Before: [3, 0, 2, 1]
10 3 3 1
After:  [3, 0, 2, 1]

Before: [1, 1, 2, 2]
11 2 2 2
After:  [1, 1, 1, 2]

Before: [0, 2, 2, 2]
11 2 2 3
After:  [0, 2, 2, 1]

Before: [3, 1, 1, 0]
7 3 0 1
After:  [3, 1, 1, 0]

Before: [1, 1, 0, 2]
4 0 2 3
After:  [1, 1, 0, 0]

Before: [2, 1, 0, 1]
14 1 3 0
After:  [1, 1, 0, 1]

Before: [2, 0, 2, 2]
10 3 3 1
After:  [2, 0, 2, 2]

Before: [0, 3, 0, 2]
6 0 0 2
After:  [0, 3, 0, 2]

Before: [0, 0, 2, 1]
10 3 3 0
After:  [0, 0, 2, 1]

Before: [2, 1, 1, 3]
2 1 3 1
After:  [2, 0, 1, 3]

Before: [0, 1, 0, 2]
12 1 0 1
After:  [0, 1, 0, 2]

Before: [0, 2, 1, 0]
11 0 0 2
After:  [0, 2, 1, 0]

Before: [2, 2, 2, 3]
2 2 3 1
After:  [2, 0, 2, 3]

Before: [2, 3, 0, 2]
10 3 3 0
After:  [0, 3, 0, 2]

Before: [2, 0, 2, 1]
3 3 2 3
After:  [2, 0, 2, 1]

Before: [3, 1, 3, 0]
0 3 2 2
After:  [3, 1, 1, 0]

Before: [3, 1, 2, 1]
3 3 2 0
After:  [1, 1, 2, 1]

Before: [0, 1, 2, 3]
15 1 2 0
After:  [0, 1, 2, 3]

Before: [0, 1, 2, 2]
12 1 0 1
After:  [0, 1, 2, 2]

Before: [3, 0, 3, 3]
13 3 2 3
After:  [3, 0, 3, 1]

Before: [1, 1, 3, 0]
1 1 0 0
After:  [1, 1, 3, 0]

Before: [3, 2, 2, 3]
2 1 3 1
After:  [3, 0, 2, 3]

Before: [3, 1, 3, 3]
9 1 2 2
After:  [3, 1, 0, 3]

Before: [2, 2, 1, 3]
8 3 3 2
After:  [2, 2, 3, 3]

Before: [2, 1, 3, 2]
11 2 1 0
After:  [0, 1, 3, 2]

Before: [3, 0, 3, 0]
0 3 2 1
After:  [3, 1, 3, 0]

Before: [2, 1, 2, 3]
11 3 2 2
After:  [2, 1, 0, 3]

Before: [1, 0, 3, 0]
0 3 2 2
After:  [1, 0, 1, 0]

Before: [1, 2, 2, 2]
5 0 2 0
After:  [0, 2, 2, 2]

Before: [0, 1, 1, 3]
11 3 1 3
After:  [0, 1, 1, 0]

Before: [1, 1, 3, 3]
8 3 3 1
After:  [1, 3, 3, 3]

Before: [3, 1, 0, 0]
7 3 0 1
After:  [3, 1, 0, 0]

Before: [1, 1, 2, 0]
1 1 0 1
After:  [1, 1, 2, 0]

Before: [3, 1, 3, 3]
9 1 2 1
After:  [3, 0, 3, 3]

Before: [1, 2, 2, 0]
5 0 2 1
After:  [1, 0, 2, 0]

Before: [2, 1, 1, 1]
14 1 3 2
After:  [2, 1, 1, 1]

Before: [0, 1, 1, 3]
6 0 0 1
After:  [0, 0, 1, 3]

Before: [3, 1, 3, 1]
13 2 3 0
After:  [0, 1, 3, 1]

Before: [1, 0, 2, 1]
3 3 2 2
After:  [1, 0, 1, 1]

Before: [2, 2, 3, 0]
0 3 2 3
After:  [2, 2, 3, 1]

Before: [2, 2, 0, 2]
7 2 0 0
After:  [1, 2, 0, 2]

Before: [3, 3, 3, 0]
0 3 2 1
After:  [3, 1, 3, 0]

Before: [0, 1, 3, 0]
0 3 2 1
After:  [0, 1, 3, 0]

Before: [0, 2, 2, 2]
10 3 3 1
After:  [0, 0, 2, 2]

Before: [1, 2, 3, 1]
13 2 3 3
After:  [1, 2, 3, 0]

Before: [1, 1, 2, 3]
11 2 2 2
After:  [1, 1, 1, 3]

Before: [3, 0, 2, 0]
11 2 2 3
After:  [3, 0, 2, 1]

Before: [3, 2, 3, 0]
0 3 2 3
After:  [3, 2, 3, 1]

Before: [1, 1, 3, 1]
1 1 0 1
After:  [1, 1, 3, 1]

Before: [1, 3, 0, 1]
4 0 2 2
After:  [1, 3, 0, 1]

Before: [0, 3, 1, 1]
10 3 3 3
After:  [0, 3, 1, 0]

Before: [0, 2, 2, 0]
8 2 2 1
After:  [0, 2, 2, 0]

Before: [0, 1, 3, 1]
12 1 0 0
After:  [1, 1, 3, 1]

Before: [1, 1, 3, 0]
9 1 2 0
After:  [0, 1, 3, 0]

Before: [0, 1, 2, 2]
6 0 0 3
After:  [0, 1, 2, 0]

Before: [2, 2, 2, 3]
11 3 1 0
After:  [0, 2, 2, 3]

Before: [1, 1, 3, 0]
1 1 0 1
After:  [1, 1, 3, 0]

Before: [2, 3, 2, 1]
3 3 2 0
After:  [1, 3, 2, 1]

Before: [3, 0, 3, 3]
13 3 2 1
After:  [3, 1, 3, 3]

Before: [1, 0, 2, 0]
11 2 2 2
After:  [1, 0, 1, 0]

Before: [2, 2, 3, 0]
0 3 2 2
After:  [2, 2, 1, 0]

Before: [0, 0, 2, 3]
6 0 0 2
After:  [0, 0, 0, 3]

Before: [3, 1, 3, 1]
9 1 2 0
After:  [0, 1, 3, 1]

Before: [1, 2, 1, 1]
10 2 3 0
After:  [0, 2, 1, 1]

Before: [0, 0, 1, 0]
6 0 0 3
After:  [0, 0, 1, 0]

Before: [0, 1, 3, 2]
9 1 2 3
After:  [0, 1, 3, 0]

Before: [3, 2, 2, 3]
2 1 3 2
After:  [3, 2, 0, 3]

Before: [0, 3, 2, 1]
3 3 2 0
After:  [1, 3, 2, 1]

Before: [1, 0, 3, 0]
0 3 2 3
After:  [1, 0, 3, 1]

Before: [1, 1, 1, 3]
1 1 0 1
After:  [1, 1, 1, 3]

Before: [1, 0, 2, 0]
5 0 2 1
After:  [1, 0, 2, 0]

Before: [0, 0, 0, 2]
10 3 3 3
After:  [0, 0, 0, 0]

Before: [0, 1, 2, 2]
15 1 2 0
After:  [0, 1, 2, 2]

Before: [0, 0, 1, 3]
8 3 3 2
After:  [0, 0, 3, 3]

Before: [1, 1, 2, 1]
3 3 2 2
After:  [1, 1, 1, 1]

Before: [0, 1, 1, 1]
6 0 0 3
After:  [0, 1, 1, 0]

Before: [0, 2, 1, 1]
6 0 0 2
After:  [0, 2, 0, 1]

Before: [3, 2, 2, 1]
13 2 1 1
After:  [3, 1, 2, 1]

Before: [2, 2, 2, 1]
3 3 2 0
After:  [1, 2, 2, 1]

Before: [0, 1, 3, 1]
9 1 2 0
After:  [0, 1, 3, 1]

Before: [0, 1, 3, 3]
12 1 0 0
After:  [1, 1, 3, 3]

Before: [0, 1, 2, 3]
11 2 2 1
After:  [0, 1, 2, 3]

Before: [2, 1, 2, 2]
15 1 2 2
After:  [2, 1, 0, 2]

Before: [2, 1, 3, 1]
9 1 2 0
After:  [0, 1, 3, 1]

Before: [3, 1, 2, 3]
2 1 3 1
After:  [3, 0, 2, 3]

Before: [2, 0, 3, 3]
13 3 2 2
After:  [2, 0, 1, 3]

Before: [2, 0, 2, 3]
8 2 2 3
After:  [2, 0, 2, 2]

Before: [0, 1, 2, 1]
3 3 2 3
After:  [0, 1, 2, 1]

Before: [3, 1, 3, 0]
0 3 2 0
After:  [1, 1, 3, 0]

Before: [1, 1, 0, 0]
1 1 0 0
After:  [1, 1, 0, 0]

Before: [1, 1, 3, 2]
9 1 2 3
After:  [1, 1, 3, 0]

Before: [2, 0, 2, 2]
13 2 0 1
After:  [2, 1, 2, 2]

Before: [0, 1, 2, 1]
3 3 2 2
After:  [0, 1, 1, 1]

Before: [2, 1, 2, 1]
15 1 2 0
After:  [0, 1, 2, 1]

Before: [2, 1, 1, 3]
2 2 3 2
After:  [2, 1, 0, 3]

Before: [1, 1, 3, 2]
9 1 2 1
After:  [1, 0, 3, 2]

Before: [2, 1, 0, 3]
7 2 0 1
After:  [2, 1, 0, 3]

Before: [0, 1, 3, 2]
12 1 0 0
After:  [1, 1, 3, 2]

Before: [1, 0, 0, 1]
4 0 2 2
After:  [1, 0, 0, 1]

Before: [1, 0, 0, 0]
4 0 2 3
After:  [1, 0, 0, 0]

Before: [3, 1, 3, 2]
9 1 2 1
After:  [3, 0, 3, 2]

Before: [2, 2, 0, 0]
7 2 0 1
After:  [2, 1, 0, 0]

Before: [1, 1, 0, 1]
1 1 0 0
After:  [1, 1, 0, 1]

Before: [3, 1, 0, 1]
10 3 3 2
After:  [3, 1, 0, 1]

Before: [0, 2, 2, 1]
3 3 2 2
After:  [0, 2, 1, 1]

Before: [0, 3, 2, 0]
6 0 0 3
After:  [0, 3, 2, 0]

Before: [0, 3, 0, 2]
0 2 3 1
After:  [0, 1, 0, 2]

Before: [0, 2, 3, 3]
8 3 3 2
After:  [0, 2, 3, 3]

Before: [0, 1, 3, 0]
12 1 0 1
After:  [0, 1, 3, 0]

Before: [0, 1, 1, 1]
14 1 3 2
After:  [0, 1, 1, 1]

Before: [1, 3, 0, 3]
4 0 2 0
After:  [0, 3, 0, 3]

Before: [3, 1, 3, 3]
13 3 0 3
After:  [3, 1, 3, 1]

Before: [0, 2, 3, 0]
6 0 0 1
After:  [0, 0, 3, 0]

Before: [1, 1, 2, 3]
15 1 2 3
After:  [1, 1, 2, 0]

Before: [0, 2, 0, 1]
10 3 3 1
After:  [0, 0, 0, 1]

Before: [1, 2, 0, 2]
4 0 2 1
After:  [1, 0, 0, 2]

Before: [0, 1, 1, 2]
10 3 3 2
After:  [0, 1, 0, 2]

Before: [3, 1, 1, 3]
2 2 3 2
After:  [3, 1, 0, 3]

Before: [1, 1, 1, 3]
8 3 3 2
After:  [1, 1, 3, 3]

Before: [3, 2, 2, 1]
3 3 2 1
After:  [3, 1, 2, 1]

Before: [1, 1, 3, 0]
9 1 2 3
After:  [1, 1, 3, 0]

Before: [3, 0, 2, 1]
3 3 2 0
After:  [1, 0, 2, 1]

Before: [0, 1, 0, 2]
12 1 0 2
After:  [0, 1, 1, 2]

Before: [1, 1, 1, 1]
14 1 3 1
After:  [1, 1, 1, 1]

Before: [0, 1, 0, 1]
14 1 3 2
After:  [0, 1, 1, 1]

Before: [2, 2, 2, 3]
8 2 2 3
After:  [2, 2, 2, 2]

Before: [0, 1, 3, 0]
0 3 2 2
After:  [0, 1, 1, 0]

Before: [3, 3, 1, 3]
2 2 3 3
After:  [3, 3, 1, 0]

Before: [0, 1, 2, 3]
2 1 3 0
After:  [0, 1, 2, 3]

Before: [0, 1, 3, 1]
13 2 3 2
After:  [0, 1, 0, 1]

Before: [3, 2, 3, 1]
10 3 3 1
After:  [3, 0, 3, 1]

Before: [0, 1, 2, 3]
12 1 0 2
After:  [0, 1, 1, 3]

Before: [1, 1, 3, 1]
14 1 3 0
After:  [1, 1, 3, 1]

Before: [1, 1, 0, 0]
4 0 2 0
After:  [0, 1, 0, 0]

Before: [1, 2, 3, 0]
0 3 2 3
After:  [1, 2, 3, 1]

Before: [0, 1, 0, 1]
12 1 0 3
After:  [0, 1, 0, 1]

Before: [0, 1, 1, 2]
12 1 0 0
After:  [1, 1, 1, 2]

Before: [0, 1, 3, 0]
0 3 2 3
After:  [0, 1, 3, 1]

Before: [3, 2, 3, 2]
7 3 2 2
After:  [3, 2, 1, 2]

Before: [3, 0, 3, 0]
0 3 2 0
After:  [1, 0, 3, 0]

Before: [1, 0, 2, 2]
5 0 2 3
After:  [1, 0, 2, 0]

Before: [0, 1, 0, 1]
14 1 3 1
After:  [0, 1, 0, 1]

Before: [1, 2, 2, 1]
3 3 2 3
After:  [1, 2, 2, 1]

Before: [1, 1, 2, 2]
1 1 0 1
After:  [1, 1, 2, 2]

Before: [0, 1, 0, 3]
8 3 3 3
After:  [0, 1, 0, 3]

Before: [0, 3, 2, 0]
8 2 2 1
After:  [0, 2, 2, 0]

Before: [1, 1, 0, 0]
1 1 0 1
After:  [1, 1, 0, 0]

Before: [3, 1, 1, 0]
7 3 0 3
After:  [3, 1, 1, 1]

Before: [1, 0, 2, 3]
5 0 2 1
After:  [1, 0, 2, 3]

Before: [1, 3, 2, 1]
5 0 2 1
After:  [1, 0, 2, 1]

Before: [2, 3, 3, 3]
13 3 2 1
After:  [2, 1, 3, 3]

Before: [2, 3, 2, 1]
8 2 2 2
After:  [2, 3, 2, 1]

Before: [1, 1, 2, 1]
10 3 3 3
After:  [1, 1, 2, 0]

Before: [1, 1, 1, 3]
1 1 0 0
After:  [1, 1, 1, 3]

Before: [0, 1, 3, 1]
9 1 2 2
After:  [0, 1, 0, 1]

Before: [0, 3, 0, 3]
6 0 0 1
After:  [0, 0, 0, 3]

Before: [1, 0, 1, 2]
10 3 3 1
After:  [1, 0, 1, 2]

Before: [2, 1, 3, 0]
0 3 2 1
After:  [2, 1, 3, 0]

Before: [1, 0, 3, 0]
0 3 2 0
After:  [1, 0, 3, 0]

Before: [3, 1, 2, 0]
15 1 2 1
After:  [3, 0, 2, 0]

Before: [2, 3, 3, 0]
0 3 2 3
After:  [2, 3, 3, 1]

Before: [2, 2, 2, 0]
8 2 2 3
After:  [2, 2, 2, 2]

Before: [3, 2, 3, 2]
7 3 2 1
After:  [3, 1, 3, 2]

Before: [2, 1, 3, 2]
9 1 2 0
After:  [0, 1, 3, 2]

Before: [1, 1, 3, 1]
9 1 2 0
After:  [0, 1, 3, 1]

Before: [0, 1, 1, 3]
11 0 0 3
After:  [0, 1, 1, 1]

Before: [3, 2, 0, 3]
11 3 3 0
After:  [1, 2, 0, 3]

Before: [3, 2, 3, 1]
13 2 3 2
After:  [3, 2, 0, 1]

Before: [2, 0, 0, 1]
7 2 0 1
After:  [2, 1, 0, 1]

Before: [1, 3, 0, 2]
0 2 3 2
After:  [1, 3, 1, 2]

Before: [0, 2, 2, 2]
6 0 0 3
After:  [0, 2, 2, 0]

Before: [1, 0, 0, 2]
4 0 2 3
After:  [1, 0, 0, 0]

Before: [2, 1, 2, 1]
3 3 2 3
After:  [2, 1, 2, 1]

Before: [1, 1, 2, 0]
1 1 0 3
After:  [1, 1, 2, 1]

Before: [3, 3, 3, 3]
13 3 2 3
After:  [3, 3, 3, 1]

Before: [0, 3, 1, 1]
6 0 0 0
After:  [0, 3, 1, 1]

Before: [0, 2, 1, 2]
11 0 0 1
After:  [0, 1, 1, 2]

Before: [1, 1, 0, 3]
1 1 0 2
After:  [1, 1, 1, 3]

Before: [0, 2, 2, 3]
6 0 0 0
After:  [0, 2, 2, 3]

Before: [1, 1, 3, 0]
1 1 0 2
After:  [1, 1, 1, 0]

Before: [1, 1, 2, 3]
1 1 0 2
After:  [1, 1, 1, 3]

Before: [1, 1, 3, 3]
13 3 2 0
After:  [1, 1, 3, 3]

Before: [1, 1, 3, 3]
1 1 0 1
After:  [1, 1, 3, 3]

Before: [1, 1, 3, 3]
1 1 0 2
After:  [1, 1, 1, 3]

Before: [1, 2, 0, 1]
4 0 2 3
After:  [1, 2, 0, 0]

Before: [0, 1, 2, 3]
12 1 0 0
After:  [1, 1, 2, 3]

Before: [3, 3, 0, 2]
0 2 3 0
After:  [1, 3, 0, 2]

Before: [2, 3, 0, 1]
7 2 0 3
After:  [2, 3, 0, 1]

Before: [0, 3, 3, 1]
13 2 3 2
After:  [0, 3, 0, 1]

Before: [3, 1, 2, 1]
3 3 2 2
After:  [3, 1, 1, 1]

Before: [0, 1, 2, 3]
2 1 3 3
After:  [0, 1, 2, 0]

Before: [1, 1, 1, 1]
14 1 3 2
After:  [1, 1, 1, 1]

Before: [1, 1, 0, 1]
14 1 3 0
After:  [1, 1, 0, 1]

Before: [1, 1, 0, 3]
11 3 1 2
After:  [1, 1, 0, 3]

Before: [2, 1, 2, 3]
2 2 3 3
After:  [2, 1, 2, 0]

Before: [0, 3, 2, 0]
8 2 2 3
After:  [0, 3, 2, 2]

Before: [0, 3, 0, 3]
6 0 0 3
After:  [0, 3, 0, 0]

Before: [1, 0, 2, 1]
5 0 2 2
After:  [1, 0, 0, 1]

Before: [3, 0, 3, 2]
10 3 3 2
After:  [3, 0, 0, 2]

Before: [1, 2, 3, 1]
13 2 3 0
After:  [0, 2, 3, 1]

Before: [3, 2, 1, 1]
10 2 3 2
After:  [3, 2, 0, 1]

Before: [3, 3, 2, 1]
3 3 2 2
After:  [3, 3, 1, 1]

Before: [2, 0, 1, 3]
2 2 3 1
After:  [2, 0, 1, 3]

Before: [2, 0, 2, 0]
11 2 2 0
After:  [1, 0, 2, 0]

Before: [1, 0, 0, 2]
4 0 2 0
After:  [0, 0, 0, 2]

Before: [1, 0, 0, 0]
4 0 2 1
After:  [1, 0, 0, 0]

Before: [1, 3, 2, 1]
5 0 2 3
After:  [1, 3, 2, 0]

Before: [0, 3, 1, 2]
11 0 0 1
After:  [0, 1, 1, 2]

Before: [1, 3, 2, 1]
3 3 2 3
After:  [1, 3, 2, 1]

Before: [3, 2, 3, 1]
13 2 3 0
After:  [0, 2, 3, 1]

Before: [3, 2, 1, 3]
2 2 3 2
After:  [3, 2, 0, 3]

Before: [0, 1, 0, 1]
14 1 3 0
After:  [1, 1, 0, 1]

Before: [3, 3, 2, 3]
8 3 3 0
After:  [3, 3, 2, 3]

Before: [1, 1, 2, 1]
5 0 2 1
After:  [1, 0, 2, 1]

Before: [0, 0, 2, 1]
3 3 2 0
After:  [1, 0, 2, 1]

Before: [2, 2, 0, 1]
7 2 0 3
After:  [2, 2, 0, 1]

Before: [1, 1, 0, 2]
1 1 0 3
After:  [1, 1, 0, 1]

Before: [1, 2, 0, 3]
4 0 2 2
After:  [1, 2, 0, 3]

Before: [0, 0, 2, 3]
8 2 2 1
After:  [0, 2, 2, 3]

Before: [1, 1, 0, 0]
4 0 2 1
After:  [1, 0, 0, 0]

Before: [2, 1, 2, 3]
15 1 2 1
After:  [2, 0, 2, 3]

Before: [2, 2, 1, 2]
10 3 3 2
After:  [2, 2, 0, 2]

Before: [0, 1, 2, 1]
15 1 2 3
After:  [0, 1, 2, 0]

Before: [2, 1, 2, 2]
10 3 3 1
After:  [2, 0, 2, 2]

Before: [3, 1, 0, 2]
10 3 3 0
After:  [0, 1, 0, 2]

Before: [1, 1, 3, 1]
1 1 0 2
After:  [1, 1, 1, 1]

Before: [2, 3, 2, 1]
3 3 2 2
After:  [2, 3, 1, 1]

Before: [1, 2, 2, 2]
5 0 2 1
After:  [1, 0, 2, 2]

Before: [1, 3, 2, 1]
3 3 2 0
After:  [1, 3, 2, 1]

Before: [0, 2, 2, 1]
6 0 0 1
After:  [0, 0, 2, 1]

Before: [1, 1, 2, 3]
1 1 0 3
After:  [1, 1, 2, 1]

Before: [0, 3, 2, 1]
3 3 2 3
After:  [0, 3, 2, 1]

Before: [2, 2, 2, 1]
13 2 1 1
After:  [2, 1, 2, 1]

Before: [0, 1, 3, 1]
12 1 0 3
After:  [0, 1, 3, 1]

Before: [0, 0, 3, 2]
6 0 0 1
After:  [0, 0, 3, 2]

Before: [1, 2, 2, 1]
3 3 2 2
After:  [1, 2, 1, 1]

Before: [0, 1, 2, 3]
6 0 0 1
After:  [0, 0, 2, 3]

Before: [3, 2, 2, 3]
8 2 2 3
After:  [3, 2, 2, 2]

Before: [2, 1, 1, 3]
8 3 3 3
After:  [2, 1, 1, 3]

Before: [1, 0, 1, 1]
10 2 3 0
After:  [0, 0, 1, 1]

Before: [0, 0, 3, 3]
8 3 3 3
After:  [0, 0, 3, 3]

Before: [0, 1, 0, 0]
12 1 0 0
After:  [1, 1, 0, 0]

Before: [2, 2, 1, 3]
11 3 2 1
After:  [2, 0, 1, 3]

Before: [1, 0, 3, 2]
7 3 2 1
After:  [1, 1, 3, 2]

Before: [2, 1, 2, 1]
15 1 2 3
After:  [2, 1, 2, 0]

Before: [3, 2, 0, 0]
7 3 0 2
After:  [3, 2, 1, 0]

Before: [1, 1, 2, 1]
3 3 2 0
After:  [1, 1, 2, 1]

Before: [2, 1, 3, 3]
8 3 3 2
After:  [2, 1, 3, 3]

Before: [1, 0, 2, 1]
3 3 2 1
After:  [1, 1, 2, 1]

Before: [2, 1, 1, 3]
2 1 3 2
After:  [2, 1, 0, 3]

Before: [0, 2, 2, 1]
3 3 2 3
After:  [0, 2, 2, 1]

Before: [2, 1, 0, 2]
7 2 0 0
After:  [1, 1, 0, 2]

Before: [3, 0, 2, 1]
3 3 2 2
After:  [3, 0, 1, 1]

Before: [3, 1, 2, 3]
15 1 2 2
After:  [3, 1, 0, 3]

Before: [1, 1, 2, 1]
14 1 3 2
After:  [1, 1, 1, 1]

Before: [0, 0, 3, 3]
8 3 3 0
After:  [3, 0, 3, 3]

Before: [2, 3, 0, 1]
10 3 3 1
After:  [2, 0, 0, 1]

Before: [1, 1, 0, 1]
14 1 3 3
After:  [1, 1, 0, 1]

Before: [1, 2, 2, 2]
13 2 1 1
After:  [1, 1, 2, 2]

Before: [1, 2, 0, 0]
4 0 2 0
After:  [0, 2, 0, 0]

Before: [3, 1, 0, 1]
10 3 3 3
After:  [3, 1, 0, 0]

Before: [1, 1, 3, 0]
0 3 2 0
After:  [1, 1, 3, 0]

Before: [3, 1, 3, 2]
9 1 2 3
After:  [3, 1, 3, 0]

Before: [1, 3, 3, 0]
0 3 2 3
After:  [1, 3, 3, 1]

Before: [1, 2, 0, 1]
4 0 2 0
After:  [0, 2, 0, 1]

Before: [0, 1, 3, 2]
7 3 2 0
After:  [1, 1, 3, 2]

Before: [0, 1, 2, 1]
10 3 3 1
After:  [0, 0, 2, 1]

Before: [1, 1, 3, 3]
1 1 0 3
After:  [1, 1, 3, 1]

Before: [2, 1, 3, 0]
9 1 2 2
After:  [2, 1, 0, 0]

Before: [0, 1, 3, 0]
11 0 0 2
After:  [0, 1, 1, 0]

Before: [1, 2, 2, 1]
5 0 2 3
After:  [1, 2, 2, 0]

Before: [0, 2, 2, 2]
6 0 0 2
After:  [0, 2, 0, 2]

Before: [0, 1, 2, 2]
12 1 0 3
After:  [0, 1, 2, 1]

Before: [1, 1, 0, 3]
4 0 2 3
After:  [1, 1, 0, 0]

Before: [1, 2, 0, 3]
2 1 3 3
After:  [1, 2, 0, 0]

Before: [2, 1, 2, 3]
15 1 2 0
After:  [0, 1, 2, 3]

Before: [3, 1, 3, 1]
14 1 3 2
After:  [3, 1, 1, 1]

Before: [0, 1, 3, 1]
6 0 0 3
After:  [0, 1, 3, 0]

Before: [1, 1, 0, 2]
1 1 0 2
After:  [1, 1, 1, 2]

Before: [0, 0, 0, 3]
8 3 3 1
After:  [0, 3, 0, 3]

Before: [2, 2, 3, 3]
2 1 3 3
After:  [2, 2, 3, 0]

Before: [0, 2, 1, 0]
6 0 0 2
After:  [0, 2, 0, 0]

Before: [3, 3, 2, 1]
3 3 2 1
After:  [3, 1, 2, 1]

Before: [0, 1, 2, 1]
3 3 2 0
After:  [1, 1, 2, 1]

Before: [3, 2, 2, 3]
13 2 1 0
After:  [1, 2, 2, 3]

Before: [1, 0, 2, 1]
3 3 2 3
After:  [1, 0, 2, 1]

Before: [1, 1, 2, 1]
1 1 0 1
After:  [1, 1, 2, 1]

Before: [1, 0, 3, 0]
0 3 2 1
After:  [1, 1, 3, 0]

Before: [2, 1, 2, 1]
3 3 2 1
After:  [2, 1, 2, 1]

Before: [2, 3, 2, 1]
8 2 2 0
After:  [2, 3, 2, 1]

Before: [3, 0, 3, 1]
13 2 3 1
After:  [3, 0, 3, 1]

Before: [0, 2, 2, 3]
11 2 2 2
After:  [0, 2, 1, 3]

Before: [0, 3, 1, 1]
6 0 0 1
After:  [0, 0, 1, 1]

Before: [0, 2, 3, 2]
6 0 0 3
After:  [0, 2, 3, 0]

Before: [1, 1, 2, 2]
1 1 0 2
After:  [1, 1, 1, 2]

Before: [2, 1, 3, 0]
9 1 2 0
After:  [0, 1, 3, 0]

Before: [0, 1, 2, 3]
8 3 3 3
After:  [0, 1, 2, 3]

Before: [1, 1, 3, 1]
9 1 2 3
After:  [1, 1, 3, 0]

Before: [3, 3, 1, 0]
7 3 0 3
After:  [3, 3, 1, 1]

Before: [0, 1, 0, 0]
12 1 0 1
After:  [0, 1, 0, 0]

Before: [0, 0, 1, 1]
6 0 0 3
After:  [0, 0, 1, 0]

Before: [1, 3, 2, 1]
3 3 2 1
After:  [1, 1, 2, 1]

Before: [1, 1, 3, 3]
11 3 1 0
After:  [0, 1, 3, 3]

Before: [0, 0, 0, 1]
6 0 0 1
After:  [0, 0, 0, 1]

Before: [2, 3, 0, 3]
7 2 0 2
After:  [2, 3, 1, 3]

Before: [1, 3, 1, 1]
10 2 3 0
After:  [0, 3, 1, 1]

Before: [1, 1, 0, 1]
1 1 0 1
After:  [1, 1, 0, 1]

Before: [1, 1, 2, 2]
15 1 2 2
After:  [1, 1, 0, 2]

Before: [1, 1, 0, 2]
0 2 3 1
After:  [1, 1, 0, 2]

Before: [3, 1, 2, 1]
14 1 3 3
After:  [3, 1, 2, 1]

Before: [3, 0, 3, 2]
7 3 2 3
After:  [3, 0, 3, 1]

Before: [0, 3, 2, 3]
2 2 3 3
After:  [0, 3, 2, 0]

Before: [1, 0, 0, 2]
0 2 3 2
After:  [1, 0, 1, 2]

Before: [2, 0, 1, 3]
2 2 3 2
After:  [2, 0, 0, 3]

Before: [1, 1, 1, 1]
14 1 3 0
After:  [1, 1, 1, 1]

Before: [1, 2, 2, 3]
2 1 3 0
After:  [0, 2, 2, 3]

Before: [0, 1, 0, 3]
12 1 0 1
After:  [0, 1, 0, 3]

Before: [3, 2, 3, 0]
7 3 0 0
After:  [1, 2, 3, 0]

Before: [1, 0, 2, 3]
8 2 2 1
After:  [1, 2, 2, 3]

Before: [0, 1, 1, 1]
12 1 0 2
After:  [0, 1, 1, 1]

Before: [1, 3, 2, 2]
5 0 2 2
After:  [1, 3, 0, 2]

Before: [0, 1, 3, 3]
9 1 2 3
After:  [0, 1, 3, 0]

Before: [2, 2, 0, 1]
7 2 0 1
After:  [2, 1, 0, 1]

Before: [2, 1, 3, 0]
9 1 2 3
After:  [2, 1, 3, 0]

Before: [1, 1, 0, 3]
4 0 2 0
After:  [0, 1, 0, 3]

Before: [1, 2, 3, 2]
10 3 3 0
After:  [0, 2, 3, 2]

Before: [1, 1, 0, 1]
1 1 0 3
After:  [1, 1, 0, 1]

Before: [0, 2, 2, 1]
10 3 3 0
After:  [0, 2, 2, 1]

Before: [0, 2, 1, 3]
2 1 3 0
After:  [0, 2, 1, 3]

Before: [1, 1, 0, 1]
4 0 2 1
After:  [1, 0, 0, 1]

Before: [2, 2, 3, 3]
2 1 3 1
After:  [2, 0, 3, 3]

Before: [1, 0, 2, 0]
5 0 2 3
After:  [1, 0, 2, 0]

Before: [3, 1, 1, 3]
2 1 3 1
After:  [3, 0, 1, 3]

Before: [2, 3, 0, 2]
0 2 3 2
After:  [2, 3, 1, 2]

Before: [0, 1, 1, 1]
14 1 3 0
After:  [1, 1, 1, 1]

Before: [0, 2, 3, 3]
6 0 0 3
After:  [0, 2, 3, 0]

Before: [0, 1, 3, 1]
14 1 3 3
After:  [0, 1, 3, 1]

Before: [0, 1, 2, 0]
15 1 2 2
After:  [0, 1, 0, 0]

Before: [0, 1, 1, 0]
12 1 0 2
After:  [0, 1, 1, 0]

Before: [3, 2, 2, 1]
3 3 2 2
After:  [3, 2, 1, 1]

Before: [1, 3, 0, 3]
4 0 2 1
After:  [1, 0, 0, 3]

Before: [0, 1, 3, 1]
14 1 3 1
After:  [0, 1, 3, 1]

Before: [1, 2, 0, 3]
4 0 2 0
After:  [0, 2, 0, 3]

Before: [1, 0, 3, 3]
8 3 3 3
After:  [1, 0, 3, 3]

Before: [1, 0, 0, 3]
4 0 2 2
After:  [1, 0, 0, 3]

Before: [1, 2, 2, 3]
11 3 1 3
After:  [1, 2, 2, 0]

Before: [1, 2, 2, 1]
5 0 2 1
After:  [1, 0, 2, 1]

Before: [0, 1, 2, 0]
15 1 2 0
After:  [0, 1, 2, 0]

Before: [0, 1, 2, 3]
15 1 2 3
After:  [0, 1, 2, 0]

Before: [1, 2, 3, 0]
0 3 2 1
After:  [1, 1, 3, 0]

Before: [1, 3, 0, 2]
4 0 2 2
After:  [1, 3, 0, 2]

Before: [1, 1, 2, 0]
15 1 2 0
After:  [0, 1, 2, 0]

Before: [3, 0, 2, 1]
3 3 2 1
After:  [3, 1, 2, 1]

Before: [2, 1, 1, 1]
14 1 3 3
After:  [2, 1, 1, 1]

Before: [0, 2, 3, 0]
0 3 2 3
After:  [0, 2, 3, 1]

Before: [0, 1, 0, 1]
14 1 3 3
After:  [0, 1, 0, 1]

Before: [2, 1, 2, 1]
14 1 3 0
After:  [1, 1, 2, 1]

Before: [1, 1, 1, 0]
1 1 0 3
After:  [1, 1, 1, 1]

Before: [0, 2, 2, 3]
8 3 3 2
After:  [0, 2, 3, 3]

Before: [1, 3, 0, 1]
4 0 2 1
After:  [1, 0, 0, 1]

Before: [2, 1, 3, 1]
11 2 1 0
After:  [0, 1, 3, 1]

Before: [1, 3, 0, 1]
4 0 2 0
After:  [0, 3, 0, 1]

Before: [0, 1, 1, 1]
14 1 3 1
After:  [0, 1, 1, 1]

Before: [1, 2, 0, 3]
8 3 3 1
After:  [1, 3, 0, 3]

Before: [1, 1, 2, 3]
1 1 0 1
After:  [1, 1, 2, 3]

Before: [1, 1, 1, 1]
14 1 3 3
After:  [1, 1, 1, 1]

Before: [0, 1, 0, 3]
6 0 0 0
After:  [0, 1, 0, 3]

Before: [1, 1, 2, 0]
15 1 2 3
After:  [1, 1, 2, 0]

Before: [0, 1, 1, 0]
12 1 0 3
After:  [0, 1, 1, 1]

Before: [2, 1, 2, 2]
15 1 2 3
After:  [2, 1, 2, 0]

Before: [1, 1, 3, 1]
9 1 2 2
After:  [1, 1, 0, 1]

Before: [1, 1, 0, 1]
4 0 2 0
After:  [0, 1, 0, 1]

Before: [0, 3, 1, 1]
10 2 3 0
After:  [0, 3, 1, 1]

Before: [1, 3, 2, 0]
5 0 2 2
After:  [1, 3, 0, 0]

Before: [1, 1, 3, 3]
9 1 2 0
After:  [0, 1, 3, 3]

Before: [1, 1, 2, 2]
5 0 2 1
After:  [1, 0, 2, 2]

Before: [0, 0, 3, 2]
7 3 2 1
After:  [0, 1, 3, 2]

Before: [0, 1, 3, 1]
9 1 2 1
After:  [0, 0, 3, 1]

Before: [3, 1, 0, 2]
0 2 3 3
After:  [3, 1, 0, 1]

Before: [0, 1, 0, 2]
12 1 0 0
After:  [1, 1, 0, 2]

Before: [0, 3, 2, 3]
8 3 3 1
After:  [0, 3, 2, 3]

Before: [2, 1, 2, 0]
13 2 0 0
After:  [1, 1, 2, 0]

Before: [3, 1, 1, 3]
13 3 0 1
After:  [3, 1, 1, 3]

Before: [1, 0, 2, 0]
11 2 2 0
After:  [1, 0, 2, 0]

Before: [1, 0, 1, 3]
2 2 3 0
After:  [0, 0, 1, 3]

Before: [1, 1, 2, 3]
2 1 3 0
After:  [0, 1, 2, 3]

Before: [0, 1, 3, 2]
12 1 0 1
After:  [0, 1, 3, 2]

Before: [0, 1, 3, 3]
9 1 2 2
After:  [0, 1, 0, 3]

Before: [3, 1, 3, 0]
0 3 2 1
After:  [3, 1, 3, 0]

Before: [3, 0, 2, 0]
7 3 0 0
After:  [1, 0, 2, 0]

Before: [0, 1, 1, 3]
2 2 3 0
After:  [0, 1, 1, 3]

Before: [1, 1, 3, 1]
13 2 3 2
After:  [1, 1, 0, 1]

Before: [1, 1, 2, 3]
5 0 2 0
After:  [0, 1, 2, 3]

Before: [0, 3, 2, 3]
11 0 0 3
After:  [0, 3, 2, 1]

Before: [1, 0, 2, 3]
2 2 3 2
After:  [1, 0, 0, 3]

Before: [1, 1, 0, 0]
1 1 0 3
After:  [1, 1, 0, 1]

Before: [1, 0, 0, 0]
4 0 2 2
After:  [1, 0, 0, 0]

Before: [0, 0, 1, 3]
11 3 2 1
After:  [0, 0, 1, 3]

Before: [0, 3, 2, 1]
3 3 2 1
After:  [0, 1, 2, 1]

Before: [2, 0, 2, 1]
13 2 0 2
After:  [2, 0, 1, 1]

Before: [1, 1, 0, 1]
14 1 3 2
After:  [1, 1, 1, 1]

Before: [0, 1, 2, 1]
10 3 3 3
After:  [0, 1, 2, 0]

Before: [0, 3, 2, 2]
10 3 3 3
After:  [0, 3, 2, 0]

Before: [3, 1, 3, 1]
9 1 2 1
After:  [3, 0, 3, 1]

Before: [1, 3, 0, 2]
4 0 2 3
After:  [1, 3, 0, 0]

Before: [0, 2, 2, 1]
6 0 0 0
After:  [0, 2, 2, 1]

Before: [1, 2, 2, 1]
5 0 2 2
After:  [1, 2, 0, 1]

Before: [0, 0, 2, 1]
3 3 2 2
After:  [0, 0, 1, 1]

Before: [2, 1, 3, 2]
9 1 2 3
After:  [2, 1, 3, 0]

Before: [1, 2, 2, 1]
8 2 2 0
After:  [2, 2, 2, 1]

Before: [3, 3, 2, 3]
11 2 2 2
After:  [3, 3, 1, 3]

Before: [1, 1, 0, 2]
4 0 2 2
After:  [1, 1, 0, 2]

Before: [0, 1, 1, 2]
6 0 0 2
After:  [0, 1, 0, 2]

Before: [1, 1, 2, 2]
15 1 2 3
After:  [1, 1, 2, 0]

Before: [2, 2, 1, 1]
10 3 3 0
After:  [0, 2, 1, 1]

Before: [2, 1, 1, 1]
14 1 3 1
After:  [2, 1, 1, 1]

Before: [3, 3, 0, 2]
0 2 3 3
After:  [3, 3, 0, 1]

Before: [1, 0, 3, 2]
7 3 2 3
After:  [1, 0, 3, 1]

Before: [2, 0, 3, 0]
0 3 2 3
After:  [2, 0, 3, 1]

Before: [0, 0, 1, 0]
6 0 0 1
After:  [0, 0, 1, 0]

Before: [1, 1, 3, 0]
9 1 2 2
After:  [1, 1, 0, 0]

Before: [0, 1, 2, 3]
11 0 0 2
After:  [0, 1, 1, 3]

Before: [1, 2, 1, 3]
2 1 3 1
After:  [1, 0, 1, 3]

Before: [2, 1, 2, 3]
15 1 2 3
After:  [2, 1, 2, 0]

Before: [0, 2, 2, 3]
8 2 2 0
After:  [2, 2, 2, 3]

Before: [1, 1, 2, 1]
3 3 2 3
After:  [1, 1, 2, 1]

Before: [1, 1, 2, 2]
15 1 2 1
After:  [1, 0, 2, 2]

Before: [1, 3, 0, 0]
4 0 2 2
After:  [1, 3, 0, 0]

Before: [1, 2, 2, 3]
2 1 3 1
After:  [1, 0, 2, 3]

Before: [0, 1, 0, 3]
12 1 0 3
After:  [0, 1, 0, 1]

Before: [1, 3, 2, 0]
5 0 2 1
After:  [1, 0, 2, 0]

Before: [1, 0, 2, 1]
5 0 2 1
After:  [1, 0, 2, 1]

Before: [1, 0, 2, 0]
5 0 2 2
After:  [1, 0, 0, 0]

Before: [2, 1, 2, 1]
14 1 3 3
After:  [2, 1, 2, 1]

Before: [1, 1, 3, 2]
1 1 0 0
After:  [1, 1, 3, 2]

Before: [0, 2, 2, 2]
6 0 0 0
After:  [0, 2, 2, 2]

Before: [0, 2, 0, 0]
6 0 0 0
After:  [0, 2, 0, 0]

Before: [1, 1, 3, 3]
1 1 0 0
After:  [1, 1, 3, 3]

Before: [1, 1, 3, 1]
14 1 3 1
After:  [1, 1, 3, 1]

Before: [3, 2, 2, 1]
3 3 2 0
After:  [1, 2, 2, 1]

Before: [0, 1, 2, 0]
12 1 0 0
After:  [1, 1, 2, 0]

Before: [1, 3, 2, 3]
5 0 2 0
After:  [0, 3, 2, 3]

Before: [0, 1, 3, 0]
9 1 2 1
After:  [0, 0, 3, 0]

Before: [1, 1, 2, 0]
5 0 2 1
After:  [1, 0, 2, 0]

Before: [3, 1, 2, 0]
7 3 0 1
After:  [3, 1, 2, 0]

Before: [1, 1, 3, 3]
2 1 3 2
After:  [1, 1, 0, 3]

Before: [2, 1, 3, 3]
9 1 2 1
After:  [2, 0, 3, 3]

Before: [1, 3, 2, 3]
5 0 2 3
After:  [1, 3, 2, 0]

Before: [1, 0, 2, 1]
3 3 2 0
After:  [1, 0, 2, 1]

Before: [1, 0, 0, 3]
4 0 2 3
After:  [1, 0, 0, 0]

Before: [0, 1, 2, 1]
11 0 0 0
After:  [1, 1, 2, 1]

Before: [1, 1, 2, 3]
1 1 0 0
After:  [1, 1, 2, 3]

Before: [2, 2, 2, 1]
3 3 2 1
After:  [2, 1, 2, 1]

Before: [1, 2, 3, 1]
13 2 3 2
After:  [1, 2, 0, 1]

Before: [2, 1, 2, 1]
3 3 2 0
After:  [1, 1, 2, 1]

Before: [1, 1, 2, 3]
2 1 3 1
After:  [1, 0, 2, 3]

Before: [0, 2, 2, 1]
3 3 2 0
After:  [1, 2, 2, 1]

Before: [3, 3, 2, 0]
7 3 0 0
After:  [1, 3, 2, 0]

Before: [1, 1, 1, 1]
1 1 0 0
After:  [1, 1, 1, 1]

Before: [1, 2, 0, 1]
4 0 2 1
After:  [1, 0, 0, 1]

Before: [1, 0, 2, 2]
5 0 2 1
After:  [1, 0, 2, 2]

Before: [1, 1, 0, 1]
4 0 2 3
After:  [1, 1, 0, 0]

Before: [2, 2, 0, 3]
8 3 3 3
After:  [2, 2, 0, 3]

Before: [1, 2, 0, 2]
4 0 2 2
After:  [1, 2, 0, 2]

Before: [0, 3, 3, 3]
13 3 2 3
After:  [0, 3, 3, 1]

Before: [2, 1, 2, 3]
13 2 0 3
After:  [2, 1, 2, 1]

Before: [2, 3, 0, 3]
8 3 3 0
After:  [3, 3, 0, 3]

Before: [1, 3, 2, 1]
5 0 2 0
After:  [0, 3, 2, 1]

Before: [2, 0, 2, 2]
8 2 2 3
After:  [2, 0, 2, 2]

Before: [1, 1, 0, 2]
4 0 2 0
After:  [0, 1, 0, 2]

Before: [3, 0, 1, 1]
10 2 3 3
After:  [3, 0, 1, 0]

Before: [1, 1, 3, 2]
9 1 2 0
After:  [0, 1, 3, 2]

Before: [1, 3, 0, 0]
4 0 2 0
After:  [0, 3, 0, 0]

Before: [3, 1, 1, 1]
14 1 3 2
After:  [3, 1, 1, 1]

Before: [3, 2, 3, 2]
10 3 3 2
After:  [3, 2, 0, 2]

Before: [2, 3, 1, 1]
10 2 3 3
After:  [2, 3, 1, 0]

Before: [1, 2, 2, 1]
5 0 2 0
After:  [0, 2, 2, 1]

Before: [1, 3, 0, 1]
4 0 2 3
After:  [1, 3, 0, 0]

Before: [0, 1, 3, 3]
12 1 0 3
After:  [0, 1, 3, 1]

Before: [3, 1, 3, 1]
14 1 3 3
After:  [3, 1, 3, 1]

Before: [2, 1, 2, 1]
3 3 2 2
After:  [2, 1, 1, 1]

Before: [2, 0, 3, 2]
7 3 2 2
After:  [2, 0, 1, 2]

Before: [0, 3, 2, 1]
3 3 2 2
After:  [0, 3, 1, 1]

Before: [1, 1, 0, 3]
1 1 0 3
After:  [1, 1, 0, 1]

Before: [0, 1, 1, 3]
12 1 0 1
After:  [0, 1, 1, 3]

Before: [0, 1, 3, 1]
12 1 0 2
After:  [0, 1, 1, 1]

Before: [2, 1, 3, 1]
9 1 2 1
After:  [2, 0, 3, 1]

Before: [1, 2, 2, 1]
3 3 2 1
After:  [1, 1, 2, 1]

Before: [2, 2, 0, 3]
2 1 3 1
After:  [2, 0, 0, 3]

Before: [1, 0, 0, 1]
4 0 2 0
After:  [0, 0, 0, 1]

Before: [3, 0, 2, 3]
8 3 3 3
After:  [3, 0, 2, 3]

Before: [1, 3, 2, 3]
5 0 2 2
After:  [1, 3, 0, 3]

Before: [3, 1, 3, 2]
9 1 2 2
After:  [3, 1, 0, 2]

Before: [1, 1, 0, 3]
1 1 0 1
After:  [1, 1, 0, 3]

Before: [1, 3, 0, 2]
4 0 2 0
After:  [0, 3, 0, 2]

Before: [0, 1, 2, 1]
12 1 0 1
After:  [0, 1, 2, 1]

Before: [1, 0, 0, 3]
8 3 3 0
After:  [3, 0, 0, 3]

Before: [3, 3, 0, 0]
7 3 0 1
After:  [3, 1, 0, 0]

Before: [1, 1, 3, 2]
7 3 2 2
After:  [1, 1, 1, 2]

Before: [2, 0, 1, 2]
10 3 3 1
After:  [2, 0, 1, 2]

Before: [1, 1, 0, 2]
1 1 0 1
After:  [1, 1, 0, 2]

Before: [3, 1, 0, 0]
7 3 0 2
After:  [3, 1, 1, 0]

Before: [2, 1, 0, 2]
0 2 3 3
After:  [2, 1, 0, 1]

Before: [1, 1, 1, 3]
1 1 0 3
After:  [1, 1, 1, 1]

Before: [1, 0, 2, 3]
5 0 2 3
After:  [1, 0, 2, 0]

Before: [2, 1, 3, 1]
14 1 3 2
After:  [2, 1, 1, 1]

Before: [3, 2, 0, 3]
11 3 1 0
After:  [0, 2, 0, 3]

Before: [2, 1, 2, 3]
8 2 2 0
After:  [2, 1, 2, 3]

Before: [0, 2, 0, 2]
11 0 0 0
After:  [1, 2, 0, 2]

Before: [2, 2, 0, 2]
10 3 3 1
After:  [2, 0, 0, 2]

Before: [1, 1, 3, 2]
1 1 0 1
After:  [1, 1, 3, 2]

Before: [2, 1, 2, 2]
15 1 2 0
After:  [0, 1, 2, 2]

Before: [2, 1, 2, 2]
10 3 3 2
After:  [2, 1, 0, 2]

Before: [0, 1, 3, 0]
12 1 0 0
After:  [1, 1, 3, 0]

Before: [1, 0, 3, 1]
10 3 3 0
After:  [0, 0, 3, 1]

Before: [0, 1, 2, 1]
3 3 2 1
After:  [0, 1, 2, 1]

Before: [2, 1, 1, 2]
10 3 3 3
After:  [2, 1, 1, 0]

Before: [0, 1, 3, 2]
7 3 2 2
After:  [0, 1, 1, 2]

Before: [1, 1, 1, 1]
1 1 0 2
After:  [1, 1, 1, 1]

Before: [2, 1, 2, 0]
15 1 2 3
After:  [2, 1, 2, 0]

Before: [1, 2, 0, 2]
4 0 2 3
After:  [1, 2, 0, 0]

Before: [1, 1, 3, 3]
9 1 2 2
After:  [1, 1, 0, 3]

Before: [3, 1, 1, 3]
13 3 0 3
After:  [3, 1, 1, 1]

Before: [0, 0, 1, 3]
2 2 3 1
After:  [0, 0, 1, 3]

Before: [3, 2, 0, 2]
0 2 3 3
After:  [3, 2, 0, 1]

Before: [1, 1, 1, 3]
2 1 3 1
After:  [1, 0, 1, 3]

Before: [1, 1, 3, 1]
9 1 2 1
After:  [1, 0, 3, 1]

Before: [1, 1, 3, 2]
1 1 0 3
After:  [1, 1, 3, 1]

Before: [1, 3, 2, 2]
10 3 3 1
After:  [1, 0, 2, 2]

Before: [0, 1, 3, 0]
9 1 2 0
After:  [0, 1, 3, 0]

Before: [3, 0, 2, 1]
3 3 2 3
After:  [3, 0, 2, 1]

Before: [3, 3, 2, 1]
3 3 2 0
After:  [1, 3, 2, 1]

Before: [2, 2, 2, 0]
13 2 1 1
After:  [2, 1, 2, 0]

Before: [2, 0, 3, 2]
7 3 2 3
After:  [2, 0, 3, 1]

Before: [1, 1, 1, 2]
1 1 0 2
After:  [1, 1, 1, 2]

Before: [1, 3, 3, 1]
10 3 3 0
After:  [0, 3, 3, 1]

Before: [2, 0, 2, 1]
8 2 2 1
After:  [2, 2, 2, 1]

Before: [1, 3, 2, 0]
5 0 2 0
After:  [0, 3, 2, 0]

Before: [1, 0, 2, 0]
5 0 2 0
After:  [0, 0, 2, 0]

Before: [1, 1, 2, 3]
5 0 2 1
After:  [1, 0, 2, 3]

Before: [0, 0, 3, 0]
0 3 2 1
After:  [0, 1, 3, 0]

Before: [1, 3, 0, 0]
4 0 2 1
After:  [1, 0, 0, 0]

Before: [0, 1, 3, 0]
12 1 0 3
After:  [0, 1, 3, 1]

Before: [3, 1, 1, 1]
14 1 3 1
After:  [3, 1, 1, 1]

Before: [0, 1, 2, 0]
15 1 2 3
After:  [0, 1, 2, 0]

Before: [3, 2, 1, 3]
13 3 0 0
After:  [1, 2, 1, 3]

Before: [3, 1, 2, 1]
15 1 2 0
After:  [0, 1, 2, 1]

Before: [1, 1, 2, 1]
1 1 0 2
After:  [1, 1, 1, 1]

Before: [0, 1, 1, 2]
11 0 0 1
After:  [0, 1, 1, 2]

Before: [3, 2, 0, 3]
8 3 3 0
After:  [3, 2, 0, 3]

Before: [2, 3, 2, 3]
11 2 2 2
After:  [2, 3, 1, 3]

Before: [0, 2, 3, 2]
7 3 2 1
After:  [0, 1, 3, 2]

Before: [0, 1, 2, 1]
8 2 2 1
After:  [0, 2, 2, 1]

Before: [0, 3, 1, 1]
6 0 0 3
After:  [0, 3, 1, 0]

Before: [0, 2, 3, 2]
7 3 2 3
After:  [0, 2, 3, 1]

Before: [0, 1, 2, 0]
6 0 0 3
After:  [0, 1, 2, 0]

Before: [0, 1, 0, 3]
6 0 0 2
After:  [0, 1, 0, 3]

Before: [1, 1, 3, 2]
9 1 2 2
After:  [1, 1, 0, 2]

Before: [0, 0, 2, 0]
11 2 2 1
After:  [0, 1, 2, 0]

Before: [3, 1, 3, 3]
9 1 2 3
After:  [3, 1, 3, 0]

Before: [0, 2, 3, 2]
6 0 0 2
After:  [0, 2, 0, 2]

Before: [3, 2, 2, 3]
13 3 0 0
After:  [1, 2, 2, 3]

Before: [0, 3, 3, 0]
6 0 0 3
After:  [0, 3, 3, 0]

Before: [0, 1, 1, 0]
12 1 0 0
After:  [1, 1, 1, 0]

Before: [1, 1, 3, 1]
14 1 3 2
After:  [1, 1, 1, 1]

Before: [3, 1, 2, 3]
15 1 2 0
After:  [0, 1, 2, 3]

Before: [1, 2, 2, 2]
5 0 2 2
After:  [1, 2, 0, 2]

Before: [3, 1, 1, 1]
10 2 3 1
After:  [3, 0, 1, 1]

Before: [2, 0, 2, 3]
8 3 3 3
After:  [2, 0, 2, 3]

Before: [2, 2, 2, 1]
3 3 2 3
After:  [2, 2, 2, 1]

Before: [1, 0, 0, 3]
8 3 3 1
After:  [1, 3, 0, 3]

Before: [3, 0, 2, 2]
10 3 3 2
After:  [3, 0, 0, 2]

Before: [3, 1, 2, 2]
15 1 2 0
After:  [0, 1, 2, 2]

Before: [1, 2, 2, 0]
5 0 2 3
After:  [1, 2, 2, 0]

Before: [3, 1, 0, 3]
13 3 0 1
After:  [3, 1, 0, 3]

Before: [0, 1, 0, 0]
12 1 0 3
After:  [0, 1, 0, 1]

Before: [1, 3, 3, 3]
13 3 2 3
After:  [1, 3, 3, 1]

Before: [0, 1, 2, 1]
12 1 0 2
After:  [0, 1, 1, 1]

Before: [3, 0, 2, 0]
7 3 0 2
After:  [3, 0, 1, 0]

Before: [1, 3, 2, 2]
5 0 2 3
After:  [1, 3, 2, 0]

Before: [1, 1, 2, 1]
1 1 0 0
After:  [1, 1, 2, 1]

Before: [3, 1, 3, 0]
9 1 2 1
After:  [3, 0, 3, 0]

Before: [3, 3, 0, 3]
13 3 0 1
After:  [3, 1, 0, 3]

Before: [2, 1, 2, 1]
15 1 2 1
After:  [2, 0, 2, 1]

Before: [3, 1, 3, 0]
9 1 2 0
After:  [0, 1, 3, 0]

Before: [1, 1, 2, 3]
5 0 2 3
After:  [1, 1, 2, 0]

Before: [0, 1, 0, 1]
12 1 0 1
After:  [0, 1, 0, 1]

Before: [2, 2, 2, 2]
13 2 1 1
After:  [2, 1, 2, 2]

Before: [1, 3, 0, 2]
0 2 3 1
After:  [1, 1, 0, 2]

Before: [2, 2, 0, 3]
8 3 3 1
After:  [2, 3, 0, 3]

Before: [2, 0, 0, 2]
0 2 3 2
After:  [2, 0, 1, 2]

Before: [0, 1, 3, 0]
6 0 0 0
After:  [0, 1, 3, 0]

Before: [0, 1, 0, 2]
0 2 3 1
After:  [0, 1, 0, 2]

Before: [0, 1, 0, 3]
11 3 3 3
After:  [0, 1, 0, 1]

Before: [1, 1, 3, 0]
0 3 2 1
After:  [1, 1, 3, 0]

Before: [1, 0, 0, 2]
4 0 2 2
After:  [1, 0, 0, 2]

Before: [2, 2, 2, 3]
13 2 0 0
After:  [1, 2, 2, 3]

Before: [0, 0, 0, 2]
6 0 0 1
After:  [0, 0, 0, 2]

Before: [0, 1, 3, 1]
9 1 2 3
After:  [0, 1, 3, 0]

Before: [1, 2, 2, 2]
5 0 2 3
After:  [1, 2, 2, 0]

Before: [1, 2, 2, 3]
13 2 1 1
After:  [1, 1, 2, 3]

Before: [1, 2, 1, 3]
2 2 3 3
After:  [1, 2, 1, 0]

Before: [0, 1, 2, 1]
15 1 2 0
After:  [0, 1, 2, 1]

Before: [1, 1, 2, 1]
15 1 2 0
After:  [0, 1, 2, 1]

Before: [3, 3, 2, 1]
8 2 2 0
After:  [2, 3, 2, 1]

Before: [1, 1, 0, 1]
4 0 2 2
After:  [1, 1, 0, 1]

Before: [2, 1, 2, 3]
2 1 3 1
After:  [2, 0, 2, 3]

Before: [0, 1, 3, 3]
11 0 0 2
After:  [0, 1, 1, 3]

Before: [0, 0, 2, 2]
11 2 2 3
After:  [0, 0, 2, 1]

Before: [1, 3, 2, 1]
5 0 2 2
After:  [1, 3, 0, 1]

Before: [0, 0, 0, 3]
11 0 0 2
After:  [0, 0, 1, 3]

Before: [3, 3, 2, 3]
13 3 0 0
After:  [1, 3, 2, 3]

Before: [2, 1, 3, 1]
14 1 3 3
After:  [2, 1, 3, 1]

Before: [3, 0, 2, 0]
11 2 2 0
After:  [1, 0, 2, 0]

Before: [0, 2, 0, 2]
6 0 0 1
After:  [0, 0, 0, 2]

Before: [3, 1, 2, 1]
14 1 3 0
After:  [1, 1, 2, 1]

Before: [1, 1, 1, 0]
1 1 0 2
After:  [1, 1, 1, 0]

Before: [0, 2, 2, 1]
3 3 2 1
After:  [0, 1, 2, 1]

Before: [0, 0, 2, 3]
11 2 2 2
After:  [0, 0, 1, 3]

Before: [3, 1, 1, 1]
10 3 3 2
After:  [3, 1, 0, 1]

Before: [2, 1, 0, 1]
14 1 3 1
After:  [2, 1, 0, 1]

Before: [1, 1, 2, 2]
15 1 2 0
After:  [0, 1, 2, 2]

Before: [1, 2, 1, 1]
10 3 3 1
After:  [1, 0, 1, 1]

Before: [1, 2, 0, 2]
0 2 3 0
After:  [1, 2, 0, 2]
";

        const string SampleProgram = @"5 2 3 2
5 0 1 3
5 3 3 0
2 3 2 3
6 3 1 3
6 3 1 3
12 3 1 1
3 1 0 0
5 3 3 3
5 3 1 2
5 2 1 1
5 2 3 1
6 1 2 1
6 1 1 1
12 1 0 0
3 0 1 1
6 0 0 0
1 0 3 0
5 2 2 3
5 0 1 2
0 2 3 0
6 0 2 0
12 1 0 1
5 2 3 0
5 2 0 2
11 0 3 3
6 3 1 3
12 3 1 1
3 1 3 0
5 0 1 2
5 0 1 1
5 1 2 3
6 3 2 3
6 3 3 3
12 0 3 0
6 1 0 3
1 3 2 3
0 2 3 2
6 2 1 2
12 2 0 0
3 0 3 1
5 1 2 0
5 2 1 2
8 2 3 0
6 0 2 0
6 0 1 0
12 0 1 1
3 1 1 3
5 3 3 2
5 2 0 0
5 2 1 1
7 0 2 2
6 2 3 2
12 2 3 3
3 3 2 0
5 1 3 1
6 1 0 2
1 2 3 2
5 2 1 3
4 1 3 1
6 1 3 1
12 0 1 0
3 0 3 2
6 0 0 3
1 3 1 3
5 1 0 1
5 1 1 0
5 3 1 1
6 1 1 1
12 2 1 2
3 2 2 1
5 2 2 3
5 0 2 2
0 2 3 0
6 0 2 0
12 0 1 1
3 1 1 0
5 2 0 2
5 3 3 3
5 2 3 1
5 1 3 1
6 1 2 1
6 1 1 1
12 1 0 0
3 0 0 2
6 3 0 0
1 0 2 0
6 3 0 3
1 3 0 3
5 3 1 1
13 0 1 3
6 3 2 3
6 3 2 3
12 3 2 2
3 2 2 3
6 2 0 2
1 2 1 2
13 0 1 1
6 1 3 1
12 1 3 3
3 3 0 1
6 0 0 2
1 2 0 2
5 1 3 3
10 0 3 0
6 0 1 0
6 0 1 0
12 0 1 1
5 2 0 2
5 1 1 0
5 3 3 3
3 0 2 3
6 3 3 3
12 1 3 1
3 1 2 2
5 1 1 3
5 0 3 1
6 2 0 0
1 0 2 0
10 0 3 3
6 3 1 3
12 3 2 2
3 2 0 3
5 3 1 0
5 2 3 1
5 1 1 2
15 0 1 2
6 2 1 2
12 3 2 3
5 0 0 2
5 0 3 1
5 1 3 0
6 0 2 2
6 2 1 2
12 3 2 3
3 3 0 2
5 1 1 3
5 3 1 0
1 3 1 1
6 1 3 1
12 2 1 2
3 2 3 0
5 3 1 2
5 3 2 1
5 2 1 3
15 1 3 3
6 3 3 3
12 3 0 0
3 0 0 3
5 2 0 0
9 1 2 2
6 2 2 2
12 2 3 3
5 3 3 0
6 3 0 2
1 2 2 2
13 2 0 2
6 2 3 2
12 2 3 3
6 2 0 2
1 2 2 2
5 0 2 1
14 2 0 2
6 2 1 2
12 3 2 3
3 3 0 1
5 1 1 2
5 2 2 3
15 0 3 3
6 3 2 3
12 1 3 1
3 1 0 0
5 3 0 1
5 2 1 3
5 0 0 2
0 2 3 2
6 2 2 2
12 2 0 0
3 0 1 2
6 2 0 3
1 3 0 3
5 2 3 1
5 3 3 0
8 1 3 3
6 3 3 3
12 2 3 2
3 2 3 1
5 1 2 0
5 2 0 2
5 0 3 3
3 0 2 2
6 2 1 2
12 1 2 1
3 1 2 3
6 3 0 2
1 2 2 2
5 2 3 1
3 0 2 0
6 0 1 0
12 0 3 3
3 3 0 1
5 2 2 3
5 2 3 0
6 3 0 2
1 2 0 2
11 0 3 3
6 3 2 3
6 3 3 3
12 1 3 1
6 3 0 2
1 2 3 2
5 1 3 3
10 0 3 0
6 0 2 0
12 1 0 1
3 1 0 2
5 0 1 3
5 3 3 1
5 1 2 0
5 3 0 3
6 3 3 3
6 3 1 3
12 3 2 2
3 2 2 0
5 1 0 1
5 3 0 2
5 3 1 3
6 1 2 2
6 2 2 2
12 2 0 0
5 0 2 3
5 3 2 1
5 3 0 2
0 3 2 2
6 2 1 2
12 0 2 0
3 0 3 1
5 1 3 0
6 0 0 3
1 3 2 3
5 2 2 2
3 0 2 0
6 0 3 0
12 0 1 1
6 2 0 3
1 3 1 3
6 2 0 0
1 0 3 0
14 2 0 3
6 3 3 3
6 3 1 3
12 1 3 1
3 1 1 2
6 1 0 3
1 3 0 3
5 1 2 0
6 1 0 1
1 1 1 1
12 1 0 3
6 3 1 3
6 3 2 3
12 2 3 2
3 2 2 1
6 3 0 0
1 0 3 0
5 0 1 3
5 3 0 2
0 3 2 2
6 2 2 2
12 1 2 1
3 1 1 0
5 2 1 2
5 3 1 1
13 2 1 1
6 1 2 1
12 1 0 0
3 0 3 1
5 2 3 3
5 3 3 0
15 0 3 0
6 0 1 0
12 0 1 1
3 1 1 3
5 1 1 0
5 2 2 1
5 3 0 2
14 1 2 0
6 0 1 0
6 0 3 0
12 3 0 3
3 3 3 0
5 0 0 2
5 1 3 1
5 2 0 3
5 3 2 2
6 2 2 2
12 2 0 0
5 3 3 2
5 2 2 1
6 1 0 3
1 3 1 3
14 1 2 1
6 1 2 1
12 1 0 0
3 0 2 3
5 2 1 2
5 2 3 1
5 1 3 0
3 0 2 2
6 2 1 2
12 3 2 3
5 2 1 2
6 3 0 1
1 1 0 1
3 0 2 1
6 1 3 1
6 1 3 1
12 1 3 3
3 3 0 0
5 3 3 2
5 3 1 1
5 0 0 3
0 3 2 3
6 3 3 3
12 0 3 0
3 0 1 1
5 1 0 3
5 0 2 2
5 1 1 0
12 3 3 2
6 2 2 2
12 1 2 1
3 1 1 2
5 2 2 3
5 2 1 0
5 1 3 1
11 0 3 0
6 0 1 0
6 0 2 0
12 2 0 2
3 2 1 3
6 2 0 2
1 2 1 2
5 3 2 0
6 0 0 1
1 1 2 1
15 0 1 0
6 0 3 0
12 3 0 3
3 3 2 1
5 2 1 3
6 1 0 2
1 2 3 2
5 2 2 0
11 0 3 2
6 2 1 2
12 2 1 1
3 1 3 2
5 2 3 1
5 3 2 3
5 3 3 0
15 3 1 1
6 1 1 1
12 2 1 2
5 2 2 3
5 1 2 1
4 1 3 3
6 3 1 3
12 3 2 2
5 0 1 1
5 1 1 3
6 0 0 0
1 0 2 0
12 3 3 3
6 3 2 3
6 3 3 3
12 2 3 2
3 2 2 1
5 0 0 2
6 3 0 3
1 3 3 3
5 3 2 0
9 3 2 3
6 3 3 3
12 1 3 1
5 3 0 2
5 0 3 3
0 3 2 3
6 3 3 3
6 3 2 3
12 3 1 1
5 2 1 0
6 3 0 2
1 2 0 2
5 0 3 3
8 0 3 0
6 0 1 0
12 0 1 1
3 1 0 2
5 1 3 1
5 2 0 0
6 3 0 3
1 3 1 3
10 0 3 1
6 1 2 1
12 1 2 2
5 1 0 0
6 1 0 1
1 1 2 1
12 3 0 0
6 0 3 0
12 0 2 2
3 2 2 1
5 2 2 0
5 3 2 2
5 2 2 3
14 0 2 0
6 0 3 0
12 0 1 1
3 1 3 2
5 3 0 1
5 3 2 0
15 1 3 1
6 1 3 1
12 2 1 2
3 2 2 1
5 0 2 3
5 3 2 2
5 1 3 0
0 3 2 2
6 2 2 2
12 1 2 1
3 1 1 3
5 3 1 2
6 2 0 1
1 1 2 1
5 2 2 0
14 1 2 1
6 1 3 1
12 1 3 3
3 3 1 2
5 2 2 3
5 3 1 1
13 0 1 1
6 1 1 1
12 1 2 2
3 2 3 3
5 2 0 1
5 3 1 2
14 1 2 2
6 2 1 2
12 2 3 3
5 0 1 2
6 1 0 1
1 1 3 1
15 1 0 0
6 0 1 0
6 0 3 0
12 3 0 3
3 3 2 1
5 0 0 3
6 1 0 0
1 0 3 0
7 2 0 0
6 0 2 0
6 0 3 0
12 0 1 1
3 1 2 0
5 1 2 1
5 1 1 3
5 3 2 2
6 3 2 2
6 2 2 2
12 0 2 0
3 0 0 2
5 0 2 1
5 2 2 3
6 0 0 0
1 0 2 0
11 0 3 0
6 0 1 0
12 0 2 2
5 1 0 1
5 3 1 0
4 1 3 0
6 0 2 0
12 2 0 2
3 2 3 1
5 2 3 0
5 3 3 3
5 0 1 2
9 3 2 0
6 0 1 0
6 0 1 0
12 0 1 1
5 0 2 0
5 2 1 2
5 2 3 3
8 2 3 2
6 2 2 2
12 2 1 1
3 1 1 0
5 2 3 2
5 3 3 1
5 0 2 3
13 2 1 3
6 3 3 3
12 3 0 0
3 0 0 2
5 2 3 0
5 1 2 3
12 3 3 3
6 3 3 3
12 3 2 2
3 2 0 3
5 2 0 2
5 1 3 0
12 0 0 0
6 0 3 0
12 0 3 3
3 3 2 1
5 1 2 3
5 3 1 0
6 1 0 2
1 2 0 2
5 2 0 0
6 0 1 0
6 0 1 0
12 0 1 1
6 0 0 0
1 0 2 0
5 0 0 3
5 1 0 2
8 0 3 3
6 3 2 3
12 3 1 1
3 1 2 3
6 1 0 0
1 0 3 0
5 2 2 1
5 2 2 2
14 2 0 2
6 2 2 2
12 3 2 3
3 3 1 1
5 2 3 2
5 2 2 3
14 2 0 0
6 0 1 0
12 1 0 1
3 1 3 0
5 0 1 1
5 0 0 3
2 3 2 2
6 2 3 2
12 2 0 0
3 0 2 1
5 3 2 3
5 3 1 2
6 1 0 0
1 0 0 0
5 2 3 3
6 3 1 3
12 1 3 1
3 1 3 0
6 1 0 3
1 3 0 3
5 2 2 1
5 0 0 2
8 1 3 3
6 3 1 3
6 3 2 3
12 3 0 0
5 3 0 2
5 0 2 3
5 1 1 1
0 3 2 3
6 3 1 3
12 3 0 0
3 0 0 3
5 2 1 1
5 1 0 0
6 0 2 0
6 0 1 0
6 0 3 0
12 0 3 3
3 3 0 0
6 0 0 2
1 2 1 2
5 3 1 1
5 1 1 3
9 1 2 2
6 2 3 2
12 0 2 0
3 0 2 3
6 1 0 2
1 2 2 2
5 1 2 0
13 2 1 1
6 1 2 1
12 1 3 3
3 3 1 0
5 2 3 3
6 1 0 1
1 1 1 1
5 3 1 2
6 1 2 1
6 1 1 1
6 1 1 1
12 0 1 0
5 0 3 3
5 1 1 1
0 3 2 2
6 2 3 2
12 2 0 0
3 0 1 2
5 2 0 0
5 2 1 1
5 2 1 3
11 0 3 1
6 1 3 1
12 2 1 2
3 2 3 0
5 3 3 3
6 0 0 1
1 1 3 1
5 2 3 2
13 2 1 3
6 3 2 3
12 3 0 0
3 0 3 1
5 0 1 0
5 0 2 3
2 3 2 0
6 0 3 0
12 0 1 1
6 0 0 2
1 2 1 2
5 2 3 0
5 2 2 3
8 0 3 3
6 3 2 3
12 1 3 1
3 1 0 0
6 2 0 2
1 2 2 2
6 3 0 3
1 3 2 3
5 3 0 1
8 2 3 1
6 1 3 1
12 0 1 0
3 0 3 1
6 2 0 3
1 3 1 3
5 3 3 0
13 2 0 3
6 3 1 3
12 3 1 1
3 1 1 3
5 0 3 1
5 3 3 2
5 2 3 0
7 0 2 0
6 0 1 0
12 0 3 3
3 3 2 1
5 1 2 0
5 2 3 3
6 3 0 2
1 2 0 2
0 2 3 3
6 3 2 3
6 3 1 3
12 1 3 1
3 1 3 3
5 2 3 0
5 1 2 2
5 3 3 1
15 1 0 0
6 0 3 0
12 0 3 3
3 3 2 2
6 3 0 3
1 3 2 3
5 0 2 0
5 0 0 1
5 3 0 1
6 1 3 1
6 1 1 1
12 2 1 2
5 3 3 0
5 1 3 3
5 3 2 1
1 3 1 0
6 0 2 0
12 0 2 2
3 2 0 3
5 2 3 2
5 1 1 0
5 1 1 1
3 0 2 0
6 0 1 0
12 0 3 3
3 3 1 0
5 0 0 1
5 0 0 2
5 1 2 3
1 3 1 3
6 3 2 3
6 3 2 3
12 0 3 0
3 0 0 1
5 1 3 2
5 1 3 3
5 2 1 0
4 3 0 2
6 2 1 2
12 1 2 1
5 3 1 2
5 3 1 3
5 2 3 0
6 0 2 0
12 1 0 1
3 1 0 2
5 2 0 3
5 0 0 1
5 3 3 0
15 0 3 1
6 1 2 1
12 1 2 2
3 2 1 3
5 2 3 2
5 3 3 1
5 2 3 0
13 2 1 1
6 1 3 1
12 3 1 3
3 3 2 0
5 1 1 2
5 1 3 3
5 2 2 1
12 3 3 2
6 2 3 2
12 0 2 0
3 0 3 3
5 3 2 2
5 2 1 0
5 3 0 1
13 0 1 1
6 1 2 1
12 3 1 3
3 3 2 2
6 0 0 3
1 3 2 3
5 1 0 1
8 0 3 0
6 0 3 0
12 0 2 2
3 2 2 3
6 2 0 0
1 0 3 0
6 3 0 2
1 2 2 2
6 1 0 1
1 1 2 1
15 0 1 2
6 2 3 2
12 3 2 3
3 3 2 2
5 1 2 3
14 1 0 0
6 0 3 0
12 0 2 2
3 2 3 0
5 1 2 2
5 0 1 3
5 3 2 1
9 1 2 2
6 2 2 2
12 0 2 0
3 0 2 1
5 2 0 0
5 2 0 3
6 3 0 2
1 2 3 2
7 0 2 2
6 2 3 2
12 2 1 1
3 1 1 2
6 2 0 3
1 3 1 3
6 3 0 1
1 1 3 1
13 0 1 3
6 3 2 3
12 2 3 2
3 2 2 1
6 1 0 3
1 3 0 3
5 2 0 2
2 3 2 2
6 2 2 2
12 1 2 1
3 1 1 0
5 0 3 1
5 1 3 3
5 0 0 2
12 3 3 1
6 1 2 1
12 0 1 0
3 0 2 1
5 2 2 3
6 2 0 2
1 2 3 2
5 2 3 0
8 0 3 0
6 0 2 0
12 1 0 1
3 1 1 0
6 0 0 1
1 1 2 1
14 1 2 1
6 1 3 1
6 1 1 1
12 1 0 0
5 1 3 2
5 1 3 3
5 0 2 1
1 3 1 2
6 2 2 2
12 0 2 0
3 0 1 1
6 2 0 0
1 0 2 0
5 3 0 2
5 0 2 3
8 0 3 0
6 0 1 0
12 1 0 1
3 1 0 0";

        #endregion
    }

    [TestClass]
    public class TestDay16
    {
        [TestMethod]
        public void TestSample()
        {
            Assert.AreEqual(3, Day16.MatchCount(new int[] { 3, 2, 1, 1 }, new int[] { 9, 2, 1, 2 }, new int[] { 3, 2, 2, 1 }, out _));
        }
    }
}
