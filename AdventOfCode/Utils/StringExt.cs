using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    static class StringExt
    {
        public static string[] SplitComma(this string input)
        {
            return input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public static string[] SplitLine(this string input)
        {
            return input.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string Sort(this string input)
        {
            char[] characters = input.ToArray();
            Array.Sort(characters);
            return new string(characters);
        }

        /// <summary>
        /// Bit mask for lower case a-z letters
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int LowerCaseCharMask(this string key)
        {
            int result = 0;
            foreach (char c in key)
            {
                int i = 1 << (c - 'a');
                result = result | i;
            }
            return result;
        }

        /// <summary>
        /// Bit mask for lower case a-z letters
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int LowerCaseCharMask(this char key)
        {
            return 1 << (key - 'a');
        }

    }
}
