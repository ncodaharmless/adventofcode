using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Utils
{
    public class Misc
    {

        private static void WriteSubSets2(int[] input, int k, int numberCount)
        {
            List<List<int>> powerSet = GeneratePowersetR(input);

            foreach(var list in powerSet)
            {
                if (list.Count == 0)continue;
                int min = list.Min(i => i);
                int max = list.Max(i => i);
                if (max + min < k)
                    Console.WriteLine("{" + string.Join(",",list) +"}");
            }
        }

        private static List<List<int>> GeneratePowersetR(int[] input)
        {
            List<List<int>> results = new List<List<int>>();
            GeneratePowersetR(input, 0, new List<int>(), results);
            return results;
        }

        private static void GeneratePowersetR(int[] input, int pos, List<int> currentSet, List<List<int>> results)
        {
            if (pos < input.Length)
            {
                GeneratePowersetR(input, pos + 1, currentSet, results);
                List<int> newSet = new List<int>(currentSet);
                newSet.Add(input[pos]);
                results.Add(newSet);
                GeneratePowersetR(input, pos + 1, newSet, results);
            }
        }   

        private static List<List<int>> GeneratePowerset(int[] input)
        {
            List<List<int>> results = new List<List<int>>();
            foreach(int i in input)
            {
                int count = results.Count;
                for(int j = 0; j < count; j ++)
                {
                    List<int> newList = new List<int>(results[j]);
                    newList.Add(i);
                    results.Add(newList);
                }
                results.Add(new List<int>{ i});
            }
            return results;
        }

        private static void WriteSubSets(int[] input, int k, int numberCount){
            TryCombination(input, k, new int[1], 0, numberCount);
        }

        private static void TryCombination(int[] input, int k, int[] working, int workingIndex, int numberCount)
        {
            for(int s = 0; s < input.Length; s++)
            {
                // check if we have already used this index
                bool isUsed=false;
                for(int x = 0; x < workingIndex;x ++)
                    if (working[x] == s)isUsed=true;
                if (isUsed)continue;

                working[workingIndex] = s;
                if (workingIndex < numberCount-1)
                {
                    // haven't filled combo yet, move next level in
                    int[] test = new int[numberCount];
                    Array.Copy(working, test, numberCount);
                    TryCombination(input, k, test, workingIndex+1, numberCount);
                }
                else
                {
                    int min = input[working[0]];
                    int max = input[working[0]];
                    foreach(int i in working)
                    {
                        int val = input[i];
                        min = Math.Min(min,val);
                        max = Math.Max(max,val);
                    }
                    if (min + max <= k)
                    {
                        WriteArrayIndex(working,input);
                        // finished testing this level - try next level
                        int[] test = new int[numberCount+1];
                        Array.Copy(working, test, working.Length);
                        TryCombination(input, k, test, workingIndex+1, numberCount+1);
                    }
                }
            }
        }
        
        private static bool IsSubSets(int[] input, int k, int start, int finish){
            int min = int.MaxValue;
            int max = int.MinValue;
            for(int i = start; i <= finish; i++)
            {
                min = Math.Min(input[i],min);
                max = Math.Max(input[i],min);
            }
            bool isSubSet= (max + min <= k);
            if (isSubSet) WriteArray(input, start,finish);
            return isSubSet;
        }
        

        private static void WriteArrayIndex(int[] a, int[] vals)
        {
            foreach(int i in a)
            {
                Console.Write(vals[i]);
                Console.Write(",");
            }
            Console.WriteLine();
        }


        private static void WriteArray(int[] a, int start, int finish)
        {
            for(int i = start;i<=finish;i++)
            {
                Console.Write(a[i]);
                Console.Write(",");
            }
            Console.WriteLine();
        }

        private static void WriteArray(int[] a)
        {
            foreach(int i in a)
            {
                Console.Write(i);
                Console.Write(",");
            }
            Console.WriteLine();
        }

        private static void Quicksort(int[] a, int lo, int hi){
            if (lo < hi)
            {
                int p = Partition(a, lo, hi);
                WriteArray(a);
                Quicksort(a, lo, p-1);
                Quicksort(a, p+1, hi);
            }
        }

        private static int Partition(int[] a, int lo, int hi){
            int pivot = a[hi];
            int i = lo;
            int swap;
            for(int j = lo; j<hi;j++)
                if (a[j] < pivot)
                    {
                       if (i!=j){ swap = a[i];
                        a[i] = a[j];
                        a[j]= swap;
                       }
                        i++;
                    }

             swap = a[i];
            a[i] = a[hi];
            a[hi]= swap;
            return i;
        }

        private static int IsSimilar(string word1, string word2)
        {
            int differences = 0;
            int i = 0;
            int j = 0;
            bool differentLengths = word1.Length != word2.Length;
            string diffType = string.Empty;
            for(i = 0; i < word1.Length; i++)
            {
                if (differentLengths && (word1.Length < i+1 || word2.Length < j+1))
                {
                    diffType += ", end of word extra";
                    differences++;
                    break;
                }
                if (word1[i] != word2[j])
                {
                    //difference - could be extra char on end of either word, or different char, or missing char
                    // end of line difference
                    if (word1[i+1] == word2[j+1]) // if the next chars match, then it's just an in-place difference
                    {
                        diffType += $", char {word1[i]}<>{word2[j]}";
                        differences++;
                    }
                    else // this means missing char in word 1 or 2
                    {
                        if (word1[i] == word2[j+1]) // if char missing in word 1
                        {
                            diffType += $", w1 missing {word2[j]}";
                            i--; // don't progress word 1
                            differences++;
                        }
                        else
                        {
                            diffType += $", w2 missing {word1[i]}";
                            j--; // don't progress word 2
                            differences++;
                        }
                    }
                }
                j++;
            }
            Console.WriteLine($"Differences: {differences} {word1} {word2}"+diffType);
            return differences;
        }

        private static void FindPeriod(string input)
        {
            int repetition = 0;
            string pattern = input[0].ToString();
            for(int i = 1; i < input.Length;i++)
            {
                if (input.Length < i + pattern.Length){
                    break;
                }
                string compareTo = input.Substring(i, pattern.Length);
                if (compareTo == pattern)
                {
                    repetition++;
                    i += pattern.Length-1;
                }
                else
                {
                    // if we already thought we had the pattern, reset
                    if (repetition > 0)
                    {
                        pattern = input.Substring(0, i+1);
                        repetition = 0;
                    }
                    else
                        pattern += input[i];
                }
            }
            Console.WriteLine("Pattern: "+ pattern + " count: "+ (repetition+1).ToString());
        }
    }
}