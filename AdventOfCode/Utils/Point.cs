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
            return X << 16 + Y;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }

        public static Direction RotateLeft(Direction facing)
        {
            switch (facing)
            {
                case Direction.Up:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Up;
                default: throw new NotSupportedException();
            }
        }

        public static Direction RotateRight(Direction facing)
        {
            switch (facing)
            {
                case Direction.Up:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Up;
                default: throw new NotSupportedException();
            }
        }

        public Point MoveDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Up();
                case Direction.Down:
                    return Below();
                case Direction.Left:
                    return Left();
                case Direction.Right:
                    return Right();

                default: throw new NotSupportedException();
            }
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
