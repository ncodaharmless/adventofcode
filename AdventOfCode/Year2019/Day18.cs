using AdventOfCode.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{


    class CharMap : GridMap<char>
    {
     
        public Func<char, bool> IsTarget = (c) => false;

        public Dictionary<Point, char> Targets;

        public Tuple<GridDistanceMap, GridMap<int>> CorridorDistance(Point from, Func<char, bool> isWall)
        {
            Targets = new Dictionary<Point, char>();
            var distMap = new GridDistanceMap(Width, Height);
            var keysReq = new GridMap<int>(Width, Height);
            CorridorDistance(distMap, keysReq, isWall, from, 0, string.Empty);
            return new Tuple<GridDistanceMap, GridMap<int>>(distMap, keysReq);
        }
        private void CorridorDistance(GridDistanceMap dist, GridMap<int> keyReq, Func<char, bool> isWall, Point from, int distance, string keysRequired)
        {
            if (from.X < 0 || from.Y < 0 || from.X >= Width || from.Y >= Height) return;
            int index = from.Y * Width + from.X;
            char cell = this[index];
            if (isWall(cell)) return;
            if (dist[index] > distance)
            {
                dist[index] = distance;
                keyReq[index] = keysRequired.ToLowerCaseCharMask();
                if (char.IsLetter(cell) && char.IsUpper(cell))
                    keysRequired = (keysRequired + char.ToLower(cell));
                if (IsTarget(cell))
                {
                    if (!Targets.ContainsKey(from))
                        Targets.Add(from, cell);
                }
                CorridorDistance(dist, keyReq, isWall, from.Left(), distance + 1, keysRequired);
                CorridorDistance(dist, keyReq, isWall, from.Right(), distance + 1, keysRequired);
                CorridorDistance(dist, keyReq, isWall, from.Up(), distance + 1, keysRequired);
                CorridorDistance(dist, keyReq, isWall, from.Below(), distance + 1, keysRequired);
            }
        }

        public CharMap(CharMap cloneSource) : base(cloneSource.Width, cloneSource.Height)
        {
            cloneSource._Data.CopyTo(_Data, 0);
            IsTarget = cloneSource.IsTarget;
        }

        public CharMap(string[] lines) : base(lines[0].Length, lines.Length)
        {
            _Data = new char[Height * Width];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _Data[y * Width + x] = lines[y][x];
        }
    }

    class Day18
    {
        #region Input
        const string Input = @"
#################################################################################
#...#..j..........#.........#...........#.......#........n......#.....#........m#
#.#.#.#.###########.###.#####.#######O#####.###.#.#########.#.###.#.###.###.###.#
#.#.#.#...............#.Z.......#...#...#...#.#.#.#.......#.#.....#.....#.X.#...#
#.#.#.###########################P#.#.#.#.###.#.###.#####.###.###########.###.###
#.#.#.#.......#.........#......b..#.#.#.#.#.......#...#.#...#.....#.......#.....#
#.#.#.#.###.###.#######.#.#########.###.#.#######.###.#.###.#####.#.#############
#.#.#.#.#...#...#..l#...#.#.......#.#...#.....#.......#.#...#.....#.........#..k#
#.###.#.###.#.#####.#.###.#######.#.#.###.###.#########.#.###.#####.#######.#.#.#
#...#.#...#.#.#...#...#...#.....#.#...#.#...#...#.....#.#.#...#...#...#...#...#.#
#.#.#.#.#.###.#.#U###.###.#.###.#.#####.###.###.#.###.#.#.#.###.#.#####.#.#####F#
#.#.#.#.#...#...#...#.....#.#...#.......#...#g#...#.#.#...#.#...#...#.E.#.....#.#
#.#.#.#.###.#######.#######.###.###Y#.###.###.#####.#.###.###.#####.#.#######.#.#
#.#...#...#...#.....#......a..#...#.#...#.....#.....#...#.....#.......#.....#...#
#.#####D#####.#.#######.#####.###.###.#.#####.#.###.###.#################.#####.#
#.M...#.#...#.#.#c....#...#...#.#...#.#.#...#.#.#.#.#.#.#...............#...#...#
#####.###.#.#.#H#.###.#####.###.###.#.#.#.###.#.#.#.#.#.#.###########.#.###.#.###
#...#.....#.#.#.#...#...#...#.....#.#.#.#...#.#.#.....#.......#.....#.#.#...#.#.#
#.#.#######.#.#.#.#####.#.###.#.###.###.#.#.#.#.#####.#########.###.###.#.###.#.#
#.#.#....x#.#.#...#.....#.#...#.#..i#...#.#.#.#...#...#.........#.....#.#...#...#
#.###K#.#.#.#.#####.#####.#.###.#.###.#####.#.###.#####.###.#########.#.#.#.###.#
#...#.#.#.#.#.....#..r#...#...#.#.#.....#...#.....#.....#...#.......#...#.#.....#
#.#.#.#.###.#.###.#####.#####.#.#.#.###.#.#####.###.#####.#########.#####.#####.#
#.#.#.#.....#.#.........#.....#.#.#...#.#.......#...#...#.#.........#.....#...#.#
###.#.#########.#########.#####.#.#####.#####.###.#.#.#.###.#########.#####.#.#.#
#...#...#.....#.#...........#...#.....#.#..t#.#.#.#.#.#.....#...#.........#.#.#.#
#.#####.#.#.#.#.#####.#####.#########.#.#.#.#.#.#.#.#.#######.###.#########.#.###
#.......#.#.#.#.....#...#.#.....#...#.#.#.#.#...#.#.#.....#.......#...#...#.#...#
#.#########.#.###.#.###.#.#####.#.#.#.#.#.#.#####.#.#####.#########.#.#.#.#.###.#
#.#.....#...#...#.#.#...#...#...#.#...#.#.#.......#.#...#.......#q..#...#...#...#
#.###.#.#.#####.###.###.#.###.###.#####.#.#########.###.#######.#.###########.###
#.....#.#...#.#...#...#.#...#...#...#...#...#...#.#.....#.......#.#...#.....#...#
#######.#.#.#.###.#.#.#.###.###.###.###.#.#.#.#.#.#####.#.#######.#.###.#.#####.#
#.....#.#.#.#.....#.#.#.......#...#.....#.#.#.#.#.#.....#.#...#...#...#.#.....#.#
#.#.###.#.#.###.#####.#####.###.#.#####.#.#.#.#.#.#.#####.#.#.#.#####.#.#####.#.#
#.#.#...#.#...#.#...#.#.....#...#...#...#.#...#.#...#...#...#...#.....#.#.....#.#
###.#.###.###.#.#.#.#.#######.#######.###.#####.#.###.#.#########.#.###.#.###.#.#
#...#.#...#...#.#.#.#...#.....#.....#.#.#.#...#.#.#...#.......#...#.....#...#.#.#
#.###.#####.###.#.#.###.#.#####.###.#.#.#.###.#.###.#######.###.###########.###.#
#...........#.....#.....#.........#...........#...........#...............#w....#
#######################################.@.#######################################
#.....#.....#.......#.........#...................#.#...................#...#...#
#.###.###.#.#.#####.#.#.#####.#.#####.#.#.#######.#.#.###############.#.#.#.#.#.#
#...#.....#...#.#...#.#...#.#.#.#.#...#.#.......#...#...#...#...#...#.#...#...#.#
###.#####.#####.#.#######.#.#.#.#.#.###.#.#####.###.###.#.###.#.#.#.#.#########.#
#...#...#.#...#.#.....#...#...#...#...#.#.#...#...#.#...#.#...#...#.#.........#.#
#.###.#.###.#.#.#####.#.###.#########.#.#.###.###.###.###.#.#######.###########.#
#.....#.....#.#.....#...#.#.#.......#.#.#.......#.....#...#.#...................#
#############.#####.#####.#.#.#####.#.#.#######.#######.#.#.###.###############.#
#...........#.....#.......#.#...#.#.#.#.#.....#.........#.#...#.#.........#...#.#
#########.#.#####.#.###.###.###.#.#.#.###.#.#.#######T#######.###.#######.#.###.#
#........f#.#...#.#...#.....#...#...#...#.#.#.#.....#.#.......#...#...#...#.#...#
#.#######.###.#.#.#####.#####.###.#####.#.#.#.#.###.#.#.#######.#####.#.###.#.###
#.#.#.....#...#.#.....#.......#...#.....#.#.#.#.#.#...#.#...#.......#.#.#.......#
#.#.#.#####.###.#.###.#########.###.#.###.#.###.#.#####.#.#.#.#####.#.#.#######.#
#.#.#...#...#.#.#...#.#...#...#.#.#.#...#.#...#.#...#...#.#...#...#...#.#.....#.#
#.#.###.#.###.#.###.#.###.#.#.#.#.#.###.#####.#.#.#.#.###.#####.#.#####.#.###.#.#
#.#.....#.#...#...#.#...#...#.#.#...#.#.#.....#.#.#.#.......#...#.#...#....s#.#v#
#.#.#####.#.#.###.###.#.#.###.#.###.#.#.#.#####.###.#####.###.###.#.#.#######.#.#
#.#...#...#.#...#...#.#.#...#.#...#..y#.#.#.........#...#.#...#.....#.........#.#
#.###.#.#######.###.###.#####.###.#####.#.#.#####.###.#.###.#########.###########
#.#...#.........#.#...#.....#...#.#.....#...#...#.#...#...#.#.......#.#.........#
#.#.###########.#.#########.#.###.#.#########.#.#.#.#####.#.#######R###.#######.#
#.#.#.....Q...#...#.........#...#.#.#...#...#.#.#.#.#...#.....#...#.....#...#.C.#
#.###.#.#########.#.###.#######.#.#.#.#.#.#.#.#.#.#.###.#####.#.#.#########.#.###
#...#.#...#...G.#.#...#.#.#.....#.#...#.#.#.#.#.#.#...#...#...#.#.....#.....#.#.#
#.#.#.###.#.###.#.#####.#.#.#.###.#####.#.#.#.#.#.###.#.#.#.#####.###.#.#####.#.#
#.#.#...#.#.#...#.#.....#.#.#.#...#...I.#.#...#.#...#.#.#.#.#.....#.#...#...#...#
#.#S###.#.#.#.#.#.#.#####.#.#.#.###.###.#.#####.#####.#.#.#.###.###.###.#.#.###.#
#.#.....#...#.#.#...#.......#...#...#.#.#.....#.......#.#.#...#.#.#...#.#.#.....#
#.###########.#######.###########.###.#.#.###.#########.#.###.#.#.#.#.#.#.#######
#e#.......#...#.....#.#.......#...#.#...#.#...#...#.#...#.......#...#...#.#.....#
#.#.#######.#.#.#.###.#.###.###.#.#.#.###N#.###.#.#.#.#########.#####.###.#.###.#
#.#.#.......#.#.#.....#...#.#...#.#.....#.#.#...#.#.#.#..p..#...#...#...#h..#.#.#
#.#.#.#########W#########.#.#.###.#####.###.#.###.#.#.#.###.#####.#B#########.#.#
#...#d....#...#.....#...#.#.....#.....#.#...#.#.#.#.#.#...#.#.....#.#.......#...#
###.#####.#.#.#####.#.#.###.#######.#.#.#.###.#.#.#.#.###.#.#.#####.#.#####.#.###
#...#...#...#.....#...#.V.#.#.....#.#.#.#.#...#z#.#...#...#.#...#.#...#.#.L.#.#.#
#.#####.#######.#########.###.###.###.#.#.#.###J#.#####.###.###.#.#####.#.###.#.#
#.............#...............#.....A.#.#.......#........o#.............#....u..#
#################################################################################";
        #endregion

        private CharMap _Map;
        readonly Point StartLocation;
        private GridMap<GridDistanceMap> _OpenDoorDistances;
        private GridMap<int>[] _KeysRequired;
        string _AllKeys;
        Point[] _KeyLocations;
        int currentShortest = int.MaxValue;

        public Day18(string input = Input)
        {
            _Map = new CharMap(input.SplitLine());
            _Map.IsTarget = (c) => char.IsLetter(c) && char.IsLower(c);

            _OpenDoorDistances = new GridMap<GridDistanceMap>(_Map.Width, _Map.Height);

            _KeyLocations = new Point[26];
            _KeysRequired = new GridMap<int>[26];
            foreach (Point keyPos in _Map.FindAll(m => char.IsLetter(m) && char.IsLower(m)))
            {
                char key = _Map[keyPos];
                _AllKeys += key;
                _KeyLocations[key.ToLowerCaseInt()] = keyPos;
                var result = _Map.CorridorDistance(keyPos, w => w == '#');
                _OpenDoorDistances[keyPos] = result.Item1;
                _KeysRequired[key.ToLowerCaseInt()] = result.Item2;
            }

            StartLocation = _Map.FindFirst('@').Value;
        }



        internal int Part1()
        {
            int totalDistance = ShortedPathForAllKeys(StartLocation, 0, 0, _AllKeys);

            return totalDistance;
        }

        private int ShortedPathForAllKeys(Point currentLocation, int distance, int keysAquired, string remainingKeys)
        {
            if (remainingKeys.Length == 0)
            {
                if (currentShortest > distance)
                    currentShortest = distance;
                return distance;
            }
            int shortestToFinish = int.MaxValue;
            Parallel.ForEach(remainingKeys, (keyChar) =>
            {
                Point keyLocation = _KeyLocations[keyChar.ToLowerCaseInt()];

                int keysRequired = _KeysRequired[keyChar.ToLowerCaseInt()][currentLocation];
                if (keysRequired == (keysAquired & keysRequired))
                {
                    int distToThisKey = _OpenDoorDistances[keyLocation][currentLocation] + distance;
                    if (distToThisKey < currentShortest)
                    {
                        int thisDist = ShortedPathForAllKeys(keyLocation, distToThisKey, keysAquired | keyChar.ToLowerCaseCharMask(), remainingKeys.Remove(remainingKeys.IndexOf(keyChar), 1));
                        if (thisDist < shortestToFinish)
                            shortestToFinish = thisDist;
                    }
                }
            });
            return shortestToFinish;
        }

        internal int Part2()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class TestDay18
    {
        [TestMethod]
        public void Example1()
        {
            var d = new Day18(@"
#########
#b.A.@.a#
#########");
            Assert.AreEqual(8, d.Part1());
        }

        [TestMethod]
        public void Example2()
        {
            var d = new Day18(@"
########################
#f.D.E.e.C.b.A.@.a.B.c.#
######################.#
#d.....................#
########################");
            Assert.AreEqual(86, d.Part1());
        }

        [TestMethod]
        public void Example3()
        {
            var d = new Day18(@"
########################
#...............b.C.D.f#
#.######################
#.....@.a.B.c.d.A.e.F.g#
########################");
            Assert.AreEqual(132, d.Part1());
        }

        [TestMethod]
        public void Example4()
        {
            var d = new Day18(@"
#################
#i.G..c...e..H.p#
########.########
#j.A..b...f..D.o#
########@########
#k.E..a...g..B.n#
########.########
#l.F..d...h..C.m#
#################");
            Assert.AreEqual(136, d.Part1());
        }

        [TestMethod]
        public void Example5()
        {
            var d = new Day18(@"
########################
#@..............ac.GI.b#
###d#e#f################
###A#B#C################
###g#h#i################
########################");
            Assert.AreEqual(81, d.Part1());
        }

        [TestMethod]
        public void Part1()
        {
            Assert.AreEqual(0, new Day18().Part1());
        }
        [TestMethod]
        public void Part2()
        {
            Assert.AreEqual(0, new Day18().Part2());
        }
    }

}
