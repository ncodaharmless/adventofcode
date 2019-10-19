using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Utils
{
    public class Vector3
    {
        public int X;
        public int Y;
        public int Z;


        public int ManhattanDistance(Vector3 b)
        {
            return Math.Abs(X - b.X) + Math.Abs(Y - b.Y) + Math.Abs(Z - b.Z);
        }

    }
}
