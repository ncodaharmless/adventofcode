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
    }
}
