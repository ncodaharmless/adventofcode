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

        public Vector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3()
        {
        }

        public void Add(Vector3 a)
        {
            X += a.X;
            Y += a.Y;
            Z += a.Z;
        }

        public Vector3(string parseInput)
        {
            foreach (var line in parseInput.TrimStart('<').TrimEnd('>').Replace(" ", "").SplitComma())
            {
                switch (line.Substring(0, 2))
                {
                    case "x=":
                        X = Convert.ToInt32(line.Substring(2));
                        break;
                    case "y=":
                        Y = Convert.ToInt32(line.Substring(2));
                        break;
                    case "z=":
                        Z = Convert.ToInt32(line.Substring(2));
                        break;
                    default:
                        throw new NotSupportedException(line);
                }
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3 p)
            {
                return p.X == X && p.Y == Y && p.Z == Z;
            }
            return base.Equals(obj);
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
