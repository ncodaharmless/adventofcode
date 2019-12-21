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

        public void CalculateCorridorDistance(ITraverseMap map, Point from)
        {
            CorridorDistance(map, from, 0);
        }

        private void CorridorDistance(ITraverseMap map, Point from, int distance)
        {
            if (from.X < 0 || from.Y < 0 || from.X >= Width || from.Y >= Height) return;

            if (!map.CanTraverseTo(from)) return;

            int index = from.Y * Width + from.X;
            if (this[index] > distance)
            {
                this[index] = distance;
                CorridorDistance(map, map.TranslatePoint(from, Direction.Left), distance + 1);
                CorridorDistance(map, map.TranslatePoint(from, Direction.Right), distance + 1);
                CorridorDistance(map, map.TranslatePoint(from, Direction.Up), distance + 1);
                CorridorDistance(map, map.TranslatePoint(from, Direction.Down), distance + 1);
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
        bool CanTraverseTo(Point point);

        /// <summary>
        /// for teleportation
        /// </summary>
        Point TranslatePoint(Point point, Direction dir);
    }

}
