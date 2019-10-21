using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Year2018
{
    public class Day05
    {
        public void Run()
        {
            string line = System.IO.File.ReadAllText("day05.txt").TrimEnd(' ', '\n', '\r');
            //string line = "dabAcCaCBAcCcaDA";
            Console.WriteLine(line.Length);

            string result = React(line);

            int min = 5000;
            for (char i = 'a'; i <= 'z'; i++)
            {
                int len = React(result.Replace(Char.ToUpper(i).ToString(), "").Replace(Char.ToLower(i).ToString(), "")).Length;
                if (len < min)
                    min = len;
            }
            //string result = new string(q.ToArray());
            //int q = React(line);
            //Console.WriteLine(q);
            Console.WriteLine(min);
        }

        private string React(string line)
        {
            var chars = line.ToCharArray().ToList();
            int i = 1;
            while (i < chars.Count)
            {
                if (i > 0 && chars[i] != chars[i - 1] && Char.ToLowerInvariant(chars[i]) == Char.ToLowerInvariant(chars[i - 1]))
                {
                    chars.RemoveAt(i);
                    chars.RemoveAt(i - 1);
                    i--; // re-check the current index
                }
                else i++;
            }
            return new string(chars.ToArray());
        }

        private int React2(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            bool removed = true;
            while (removed)
            {
                for (int i = 0; i < sb.Length - 1; i++)
                {
                    char a = sb[i];
                    char b = sb[i + 1];
                    if (((int)a ^ (int)b) == 32)
                    {
                        sb.Remove(i, 2);
                        removed = true;
                        break;
                    }
                    removed = false;
                }
            }
            return sb.Length;
        }
    }
}