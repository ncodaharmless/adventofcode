using System;
using System.Collections.Generic;

namespace AdventOfCode.Utils
{
    public class Powerset
    {
        public void Run()
        {
            Console.WriteLine("Linear");
            var result = GeneratePowerset(new int[] { 0, 2, 45 });
            foreach (var item in result)
                Console.WriteLine(string.Join(",", item));

            Console.WriteLine("Recursive");
            result = GeneratePowersetRecursive(new int[] { 0, 2, 45 });
            foreach (var item in result)
                Console.WriteLine(string.Join(",", item));
        }

        public static List<List<int>> GeneratePowerset(int[] input)
        {
            List<List<int>> result = new List<List<int>>();
            foreach (int inputItem in input)
            {
                int resultCount = result.Count;
                for (int i = 0; i < resultCount; i++)
                {
                    List<int> newList = new List<int>(result[i]);
                    newList.Add(inputItem);
                    result.Add(newList);
                }
                result.Add(new List<int>(new int[] { inputItem }));
            }

            return result;
        }

        public static List<List<int>> GeneratePowersetRecursive(int[] input)
        {
            List<List<int>> result = new List<List<int>>();
            PowerSetForItem(input, 0, new List<int>(), result);
            return result;
        }

        private static void PowerSetForItem(int[] input, int inputIndex, List<int> workingSet, List<List<int>> result)
        {
            if (inputIndex < input.Length)
            {
                List<int> newItem = new List<int>(workingSet);
                newItem.Add(input[inputIndex]);
                result.Add(newItem);
                PowerSetForItem(input, inputIndex + 1, newItem, result);

                PowerSetForItem(input, inputIndex + 1, workingSet, result);
            }
        }
    }
}