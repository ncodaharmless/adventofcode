using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{
    public class Vector3
    {
        public int X;
        public int Y;
        public int Z;

        public Vector3(int x,int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3()
        {
        }

        public int ManhattanDistance(Vector3 b)
        {
            return Math.Abs(X - b.X) + Math.Abs(Y - b.Y) + Math.Abs(Z - b.Z);
        }

        public override string ToString()
        {
            return $"{X},{Y},{Z}";
        }
    }
}
