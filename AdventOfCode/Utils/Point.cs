using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{

    public struct Point
    {
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }

        public Point Below()
        {
            return new Point(X, Y + 1);
        }
        public Point Up()
        {
            return new Point(X, Y - 1);
        }
        public Point Left()
        {
            return new Point(X - 1, Y);
        }
        public Point Right()
        {
            return new Point(X + 1, Y);
        }

        public double AngleTo(Point p2)
        {
            return ToDegrees(Math.Atan2(p2.Y - Y, p2.X - X));
        }

        private double ToDegrees(double angle)
        {
            return ((angle * 180 / Math.PI) + 360) % 360;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point p)
            {
                return p.X == X && p.Y == Y;
            }
            return base.Equals(obj);
        }

        public int ManhattanDist(Point p)
        {
            return Math.Abs(p.X - X) + Math.Abs(p.Y - Y);
        }
    }

}
