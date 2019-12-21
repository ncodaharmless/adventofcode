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
                CorridorDistanceRecursive(map, map.TranslatePoint(from, Direction.Left), distance + 1);
                CorridorDistanceRecursive(map, map.TranslatePoint(from, Direction.Right), distance + 1);
                CorridorDistanceRecursive(map, map.TranslatePoint(from, Direction.Up), distance + 1);
                CorridorDistanceRecursive(map, map.TranslatePoint(from, Direction.Down), distance + 1);
            }
        }

        struct PointCheck
        {
            public Point Point;
            public int Distance;
        }

        /// <summary>
        /// Bredth first search
        /// </summary>
        /// <param name="map"></param>
        /// <param name="from"></param>
        public void CalculateCorridorDistance(ITraverseMap map, Point from)
        {
            var queue = new Queue<PointCheck>();
            queue.Enqueue(new PointCheck() { Point = from });
            while (queue.Count > 0)
            {
                PointCheck pc = queue.Dequeue();
                if (pc.Point.X < 0 || pc.Point.Y < 0 || pc.Point.X >= map.Width || pc.Point.Y >= map.Height) continue;

                if (!map.CanTraverseTo(pc.Point)) continue;

                int index = pc.Point.Y * map.Width + pc.Point.X;
                if (this[index] > pc.Distance)
                {
                    this[index] = pc.Distance;
                    queue.Enqueue(new PointCheck() { Point = map.TranslatePoint(pc.Point, Direction.Up), Distance = pc.Distance + 1 });
                    queue.Enqueue(new PointCheck() { Point = map.TranslatePoint(pc.Point, Direction.Down), Distance = pc.Distance + 1 });
                    queue.Enqueue(new PointCheck() { Point = map.TranslatePoint(pc.Point, Direction.Left), Distance = pc.Distance + 1 });
                    queue.Enqueue(new PointCheck() { Point = map.TranslatePoint(pc.Point, Direction.Right), Distance = pc.Distance + 1 });
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

    public interface ITraverseMap
    {
        int Width { get; }
        int Height { get; }

        bool CanTraverseTo(Point point);

        /// <summary>
        /// for teleportation
        /// </summary>
        Point TranslatePoint(Point point, Direction dir);
    }

}
