using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{

    public class GridDistanceMap : GridMap<int>
    {
        public int Distance(Point point)
        {
            return this[point];
        }

        public GridDistanceMap(int width, int height) : base(width, height)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    this[x, y] = int.MaxValue;
        }

        /// <summary>
        /// Depth first search
        /// </summary>
        /// <param name="map"></param>
        /// <param name="from"></param>
        public void CalculateCorridorDistanceRecursive(ITraverseMap map, Point from)
        {
            CorridorDistanceRecursive(map, from, 0);
        }

        private void CorridorDistanceRecursive(ITraverseMap map, Point from, int distance)
        {
            if (from.X < 0 || from.Y < 0 || from.X >= Width || from.Y >= Height) return;

            if (!map.CanTraverseTo(from)) return;

            int index = from.Y * Width + from.X;
            if (this[index] > distance)
            {
                this[index] = distance;
                CorridorDistanceRecursive(map, map.TranslatePoint(from.MoveDirection(Direction.Left)), distance + 1);
                CorridorDistanceRecursive(map, map.TranslatePoint(from.MoveDirection(Direction.Right)), distance + 1);
                CorridorDistanceRecursive(map, map.TranslatePoint(from.MoveDirection(Direction.Up)), distance + 1);
                CorridorDistanceRecursive(map, map.TranslatePoint(from.MoveDirection(Direction.Down)), distance + 1);
            }
        }

        /// <summary>
        /// Bredth first search
        /// </summary>
        /// <param name="map"></param>
        /// <param name="from"></param>
        public void CalculateCorridorDistance<T>(ITraverseMap map, Point from) where T : TraversePointCheck, new()
        {
            var queue = new Queue<TraversePointCheck>();
            queue.Enqueue(new T() { Point = from });
            while (queue.Count > 0)
            {
                TraversePointCheck pc = queue.Dequeue();
                if (pc.Point.X < 0 || pc.Point.Y < 0 || pc.Point.X >= map.Width || pc.Point.Y >= map.Height) continue;

                pc.Point = map.TranslatePoint(pc.Point);

                if (!map.CanTraverseTo(pc)) continue;

                int index = pc.Point.Y * map.Width + pc.Point.X;
                if (this[index] > pc.Distance)
                {
                    this[index] = pc.Distance;
                    queue.Enqueue(pc.MoveTo(Direction.Up));
                    queue.Enqueue(pc.MoveTo(Direction.Down));
                    queue.Enqueue(pc.MoveTo(Direction.Left));
                    queue.Enqueue(pc.MoveTo(Direction.Right));
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                    sb.Append(this[x, y] == int.MaxValue ? "##" : this[x, y].ToString("D2"));
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    public class TraversePointCheck
    {
        public Point Point;
        public int Distance;

        public virtual TraversePointCheck Clone()
        {
            return new TraversePointCheck() { Point = Point, Distance = Distance };
        }

        public virtual TraversePointCheck MoveTo(Direction direction)
        {
            TraversePointCheck t = Clone();
            t.Point = t.Point.MoveDirection(direction);
            t.Distance++;
            return t;
        }
    }

    public interface ITraverseMap
    {
        int Width { get; }
        int Height { get; }

        bool CanTraverseTo(Point point);
        bool CanTraverseTo(TraversePointCheck pc);

        /// <summary>
        /// for teleportation
        /// </summary>
        Point TranslatePoint(Point point);
    }

}
