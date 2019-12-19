using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{
    public abstract class GridMap<T>
    {
        public readonly int Width;
        public readonly int Height;
        protected T[] _Data;

        public T this[int i]
        {
            get
            {
                return _Data[i];
            }
            set
            {
                _Data[i] = value;
            }
        }

        public T this[int x, int y]
        {
            get
            {
                return _Data[y * Width + x];
            }
            set
            {
                _Data[y * Width + x] = value;
            }
        }

        public T this[Point point]
        {
            get
            {
                return _Data[point.Y * Width + point.X];
            }
            set
            {
                _Data[point.Y * Width + point.X] = value;
            }
        }

        public Point? FindFirst(T c)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (this[x, y].Equals(c))
                        return new Point(x, y);
            return null;
        }

        public IEnumerable<Point> FindAll(Func<T, bool> searchFor)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (searchFor(this[y * Width + x]))
                        yield return new Point(x, y);
        }

        public GridMap(int width, int height)
        {
            Width = width;
            Height = height;
            _Data = new T[Width * Height];
        }
    }
}
