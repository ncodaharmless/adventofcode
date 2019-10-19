using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Utils
{
    public class Vector4
    {
        public int X;
        public int Y;
        public int Z;
        public int T;

        public Vector4(int x,int y, int z, int t)
        {
            X = x;
            Y = y;
            Z = z;
            T = t;
        }

        public Vector4(string commaDelimited)
        {
            string[] split = commaDelimited.Trim().Split(',');
            X = int.Parse(split[0]);
            Y = int.Parse(split[1]);
            Z = int.Parse(split[2]);
            T = int.Parse(split[3]);
        }

        public Vector4()
        {
        }

        public int ManhattanDistance(Vector4 b)
        {
            return Math.Abs(X - b.X) + Math.Abs(Y - b.Y) + Math.Abs(Z - b.Z) + Math.Abs(T - b.T);
        }

        public override string ToString()
        {
            return $"{X},{Y},{Z},{T}";
        }
    }
}
