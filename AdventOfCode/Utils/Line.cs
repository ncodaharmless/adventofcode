using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{
    class Line
    {
        public Point From { get; }
        public Point To { get; }

        public Line(Point from, Point to)
        {
            From = from;
            To = to;
        }

    }

    /// <summary>
    /// Vertical or horizontal line only
    /// We can cheat on the intersection tests, since they will only intersect if one is horiz and the other is vert
    /// And the intersection is the vert line Y and horiz line X
    /// </summary>
    class AxisLine : Line
    {
        public static AxisLine Up(Point from, int distance)
        {
            return new AxisLine(from, new Point(from.X, from.Y - distance));
        }

        public static AxisLine Down(Point from, int distance)
        {
            return new AxisLine(from, new Point(from.X, from.Y + distance));
        }

        public static AxisLine Left(Point from, int distance)
        {
            return new AxisLine(from, new Point(from.X - distance, from.Y));
        }

        public static AxisLine Right(Point from, int distance)
        {
            return new AxisLine(from, new Point(from.X + distance, from.Y));
        }

        public AxisLine(Point from, Point to) : base(from, to)
        {
            if (from.X != to.X && from.Y != to.Y) throw new Exception("Not axis aligned");
        }
        public bool ContainsPoint(Point p)
        {
            return p.X >= Math.Min(From.X, To.X) && p.X <= Math.Max(From.X, To.X)
                && p.Y >= Math.Min(From.Y, To.Y) && p.Y <= Math.Max(From.Y, To.Y);
        }
        public bool Intersects(AxisLine line)
        {
            if (IsHoriz == line.IsHoriz) return false;

            Point point = Intersection(line);
            return ContainsPoint(point) && line.ContainsPoint(point);
        }

        public int Length => Math.Abs(From.X - To.X) + Math.Abs(From.Y - To.Y);

        public bool IsHoriz => From.Y == To.Y;

        public Point Intersection(AxisLine line)
        {
            if (From.X == To.X) // this is vert, line is horiz
                return new Point(From.X, line.From.Y);
            // this is horiz, line is vert
            return new Point(line.From.X, From.Y);
        }
    }
}
