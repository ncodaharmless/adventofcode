using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Day15
    {
        int mapHeight;
        int mapWidth;
        Unit[,] unitMap;
        int[,] distanceMap;
        public int GlobalTurnCount;
        public int GoblinCount;
        public int ElfCount;

        public Day15(string input = InputData)
        {
            var map = input.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim().ToCharArray()).ToArray();
            mapHeight = map.Length;
            mapWidth = map[0].Length;
            unitMap = new Unit[mapWidth, mapHeight];
            distanceMap = new int[mapWidth, mapHeight];
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    switch (map[y][x])
                    {
                        case 'E':
                            unitMap[x, y] = new Unit() { Type = 'E', X = x, Y = y };
                            ElfCount++;
                            break;
                        case 'G':
                            unitMap[x, y] = new Unit() { Type = 'G', X = x, Y = y };
                            GoblinCount++;
                            break;
                        case '#':
                            unitMap[x, y] = new Unit() { Type = '#', X = x, Y = y };
                            break;
                        case '.':
                            break;
                        default:
                            throw new NotImplementedException(map[y][x].ToString());
                    }
                }
            }

        }

        public int CalculateTotalHitpoints()
        {
            int totalHitpoints = 0;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    switch (unitMap[x, y]?.Type)
                    {
                        case 'E':
                        case 'G':
                            totalHitpoints += unitMap[x, y].Health;
                            break;
                    }
                }
            }
            return totalHitpoints;
        }

        public int CalculateBattleOutcome()
        {
            return CalculateTotalHitpoints() * GlobalTurnCount;
        }

        public string GetMapString(bool withHealth = true)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"After {GlobalTurnCount} rounds:");
            for (int y = 0; y < mapHeight; y++)
            {
                List<string> healths = new List<string>();
                for (int x = 0; x < mapWidth; x++)
                {
                    sb.Append(unitMap[x, y]?.Type.ToString() ?? ".");
                    if (withHealth && unitMap[x, y]?.Type == 'G' || unitMap[x, y]?.Type == 'E')
                        healths.Add($"{unitMap[x, y].Type}({unitMap[x, y].Health})");
                }
                if (withHealth)
                {
                    sb.Append("   " + string.Join(", ", healths));
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void Step()
        {
            GlobalTurnCount++;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    switch (unitMap[x, y]?.Type)
                    {
                        case 'E':
                        case 'G':
                            PerformMove(unitMap[x, y]);
                            break;
                    }
                }
            }
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    switch (unitMap[x, y]?.Type)
                    {
                        case 'E':
                        case 'G':
                            PerformAttack(unitMap[x, y]);
                            break;
                    }
                }
            }
        }

        public void RunUntilBattleIsOver(bool outputMap = false)
        {
            while (ElfCount > 0 && GoblinCount > 0)
            {
                Step();
                if (outputMap)
                    Console.WriteLine(GetMapString());
            }
        }

        private void PerformMove(Unit unit)
        {
            if (unit.TurnCounter == GlobalTurnCount) return;
            unit.TurnCounter = GlobalTurnCount;

            Array.Clear(distanceMap, 0, mapHeight * mapWidth);

            var target = MovesFromLocation(1, unit.X, unit.Y, unit.Type);
            if (target == null) return;

            int newX = unit.X;
            int newY = unit.Y;
            switch (target.Direction)
            {
                case Direction.Right:
                    newX++;
                    break;
                case Direction.Left:
                    newX--;
                    break;
                case Direction.Up:
                    newY--;
                    break;
                case Direction.Down:
                    newY++;
                    break;
            }

            // move
            if (unitMap[newX, newY] == null)
            {
                unitMap[unit.X, unit.Y] = null;
                unitMap[newX, newY] = unit;
                unit.X = newX;
                unit.Y = newY;
            }
            else if (unitMap[newX, newY] != target.Target)
                throw new NotSupportedException();
        }
        private void PerformAttack(Unit unit)
        {
            char attackType = unit.Type == 'G' ? 'E' : 'G';

            List<Unit> possibleTargets = new List<Unit>();
            //try up first
            if (unitMap[unit.X, unit.Y - 1]?.Type == attackType)
                possibleTargets.Add(unitMap[unit.X, unit.Y - 1]);
            //left
            if (unitMap[unit.X - 1, unit.Y]?.Type == attackType)
                possibleTargets.Add(unitMap[unit.X - 1, unit.Y]);
            //right
            if (unitMap[unit.X + 1, unit.Y]?.Type == attackType)
                possibleTargets.Add(unitMap[unit.X + 1, unit.Y]);
            //bottom
            if (unitMap[unit.X, unit.Y + 1]?.Type == attackType)
                possibleTargets.Add(unitMap[unit.X, unit.Y + 1]);
            // no target
            if (possibleTargets.Count == 0) return;

            var targetUnit = possibleTargets.OrderBy(t => t.Health).ThenBy(t => t.Y).ThenBy(t => t.X).First();

            // if we are beside a target, attack
            if (Math.Abs(targetUnit.X - unit.X) + Math.Abs(targetUnit.Y - unit.Y) == 1)
            {
                targetUnit.Health -= 3;
                if (targetUnit.Health <= 0)
                {
                    // unit killed
                    if (targetUnit.Type == 'G')
                        GoblinCount--;
                    else
                        ElfCount--;
                    unitMap[targetUnit.X, targetUnit.Y] = null;
                }
            }
        }

        private TargetInfo MovesFromLocation(int stepCount, int x, int y, char unitType)
        {
            distanceMap[x, y] = stepCount;
            TargetInfo up = RangeToTarget(stepCount, x, y - 1, unitType);
            TargetInfo left = RangeToTarget(stepCount, x - 1, y, unitType);
            TargetInfo right = RangeToTarget(stepCount, x + 1, y, unitType);
            TargetInfo down = RangeToTarget(stepCount, x, y + 1, unitType);
            int shortest = int.MaxValue;
            if (up != null) shortest = Math.Min(up.StepCount, shortest);
            if (down != null) shortest = Math.Min(down.StepCount, shortest);
            if (left != null) shortest = Math.Min(left.StepCount, shortest);
            if (right != null) shortest = Math.Min(right.StepCount, shortest);
            if (up != null && up.StepCount == shortest) { up.Direction = Direction.Up; return up; }
            if (left != null && left.StepCount == shortest) { left.Direction = Direction.Left; return left; }
            if (right != null && right.StepCount == shortest) { right.Direction = Direction.Right; return right; }
            if (down != null && down.StepCount == shortest) { down.Direction = Direction.Down; return down; }
            return null;
        }

        private TargetInfo RangeToTarget(int stepCount, int x, int y, char unitType)
        {
            stepCount++;
            Unit dest = unitMap[x, y];
            switch (dest?.Type)
            {
                case '#':
                    return null;
                case 'E':
                case 'G':
                    if (unitType == dest.Type)
                        return null;
                    return new TargetInfo() { StepCount = stepCount, Target = dest };
                default:
                    if (distanceMap[x, y] > 0 && distanceMap[x, y] < stepCount) return null;

                    return MovesFromLocation(stepCount, x, y, unitType);
            }
        }

        public void Part1()
        {
            RunUntilBattleIsOver();
            Console.WriteLine(CalculateBattleOutcome());
        }

        public enum Direction { Up, Down, Left, Right }

        public class TargetInfo
        {
            public int StepCount;
            public Unit Target;
            public Direction Direction;

            public override string ToString()
            {
                return $"{Target.Type} Steps: {StepCount}";
            }
        }

        public class Unit
        {
            public int X;
            public int Y;
            public int Health = 200;
            public char Type;
            public int TurnCounter;

            public override string ToString()
            {
                return $"{Type} ({X},{Y})";
            }
        }

        #region Input

        const string InputData = @"################################
###########...G...#.##..########
###########...#..G#..G...#######
#########.G.#....##.#GG..#######
#########.#.........G....#######
#########.#..............#######
#########.#...............######
#########.GG#.G...........######
########.##...............##..##
#####.G..##G.......E....G......#
#####.#..##......E............##
#####.#..##..........EG....#.###
########......#####...E.##.#.#.#
########.#...#######......E....#
########..G.#########..E...###.#
####.###..#.#########.....E.####
####....G.#.#########.....E.####
#.........#G#########......#####
####....###G#########......##..#
###.....###..#######....##..#..#
####....#.....#####.....###....#
######..#.G...........##########
######...............###########
####.....G.......#.#############
####..#...##.##..#.#############
####......#####E...#############
#.....###...####E..#############
##.....####....#...#############
####.########..#...#############
####...######.###..#############
####..##########################
################################";

        #endregion

    }

    [TestClass]
    public class TestDay15
    {

        [TestMethod]
        public void TestMappingOne()
        {
            var test = new Day15(@"
#########
#G..G..G#
#.......#
#.......#
#G..E..G#
#.......#
#.......#
#G..G..G#
#########
");
            test.Step();
            Assert.AreEqual(@"
After 1 rounds:
#########
#.G...G.#
#...G...#
#...E..G#
#.G.....#
#.......#
#G..G..G#
#.......#
#########
", test.GetMapString(false));
            test.Step();
            Assert.AreEqual(@"
After 2 rounds:
#########
#..G.G..#
#...G...#
#.G.E.G.#
#.......#
#G..G..G#
#.......#
#.......#
#########
", test.GetMapString(false));
            test.Step();
            Assert.AreEqual(@"
After 3 rounds:
#########
#.......#
#..GGG..#
#..GEG..#
#G..G...#
#......G#
#.......#
#.......#
#########
", test.GetMapString(false));
        }

        [TestMethod]
        public void TestCombatScene()
        {
            var test = new Day15(@"
#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######");
            test.Step();
            Assert.AreEqual(@"
After 1 rounds:
#######   
#..G..#   G(200)
#...EG#   E(197), G(197)
#.#G#G#   G(200), G(197)
#...#E#   E(197)
#.....#   
#######   
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 2 rounds:
#######   
#...G.#   G(200)
#..GEG#   G(200), E(188), G(194)
#.#.#G#   G(194)
#...#E#   E(194)
#.....#   
#######   
", test.GetMapString());
            while (test.GlobalTurnCount < 23)
                test.Step();
            Assert.AreEqual(@"
After 23 rounds:
#######   
#...G.#   G(200)
#..G.G#   G(200), G(131)
#.#.#G#   G(131)
#...#E#   E(131)
#.....#   
#######   
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 24 rounds:
#######   
#..G..#   G(200)
#...G.#   G(131)
#.#G#G#   G(200), G(128)
#...#E#   E(128)
#.....#   
#######   
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 25 rounds:
#######   
#.G...#   G(200)
#..G..#   G(131)
#.#.#G#   G(125)
#..G#E#   G(200), E(125)
#.....#   
#######   
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 26 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(122)
#...#E#   E(122)
#..G..#   G(200)
#######   
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 27 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(119)
#...#E#   E(119)
#...G.#   G(200)
#######   
", test.GetMapString());

            test.Step();
            Assert.AreEqual(@"
After 28 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(116)
#...#E#   E(113)
#....G#   G(200)
#######   
", test.GetMapString());

            while (test.GlobalTurnCount < 47)
                test.Step();
            Assert.AreEqual(@"
After 47 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(59)
#...#.#   
#....G#   G(200)
#######   
", test.GetMapString());

            Assert.AreEqual(47, test.GlobalTurnCount);
            Assert.AreEqual(0, test.ElfCount);
            Assert.AreEqual(4, test.GoblinCount);
            Assert.AreEqual(590, test.CalculateTotalHitpoints());
            Assert.AreEqual(27730, test.CalculateBattleOutcome());
        }

        [TestMethod]
        public void OtherTestCombat1()
        {
            var test = new Day15(@"
#######
#G..#E#
#E#E.E#
#G.##.#
#...#E#
#...E.#
#######
");
            test.RunUntilBattleIsOver(true);

            Assert.AreEqual(@"
After 37 rounds:
#######   
#...#E#   E(200)
#E#...#   E(197)
#.E##.#   E(185)
#E..#E#   E(200), E(200)
#.....#   
#######   
", test.GetMapString());
            Assert.AreEqual(982, test.CalculateTotalHitpoints());
        }

        [TestMethod]
        public void OtherTestCombat2()
        {
            var test = new Day15(@"
#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######
");
            test.RunUntilBattleIsOver(true);
            Assert.AreEqual(@"
After 46 rounds:
#######   
#.E.E.#   E(164), E(197)
#.#E..#   E(200)
#E.##.#   E(98)
#.E.#.#   E(200)
#...#.#   
#######   
", test.GetMapString());
            Assert.AreEqual(859, test.CalculateTotalHitpoints());
        }


        [TestMethod]
        public void OtherTestCombat3()
        {
            var test = new Day15(@"
#######
#E.G#.#
#.#G..#
#G.#.G#
#G..#.#
#...E.#
#######
");
            test.RunUntilBattleIsOver();
            Assert.AreEqual(@"
After 35 rounds:
#######   
#G.G#.#   G(200), G(98)
#.#G..#   G(200)
#..#..#   
#...#G#   G(95)
#...G.#   G(200)
#######   
", test.GetMapString());
            Assert.AreEqual(793, test.CalculateTotalHitpoints());
        }

        [TestMethod]
        public void OtherTestCombat4()
        {
            var test = new Day15(@"
#######
#.E...#
#.#..G#
#.###.#
#E#G#G#
#...#G#
#######
");
            test.RunUntilBattleIsOver();
            Assert.AreEqual(@"
After 54 rounds:
#######   
#.....#   
#.#G..#   G(200)
#.###.#   
#.#.#.#   
#G.G#G#   G(98), G(38), G(200)
#######   
", test.GetMapString());
            Assert.AreEqual(536, test.CalculateTotalHitpoints());
        }

        [TestMethod]
        public void OtherTestCombat5()
        {
            var test = new Day15(@"
#########
#G......#
#.E.#...#
#..##..G#
#...##..#
#...#...#
#.G...G.#
#.....G.#
#########
");
            test.RunUntilBattleIsOver();
            Assert.AreEqual(@"
After 20 rounds:
#########   
#.G.....#   G(137)
#G.G#...#   G(200), G(200)
#.G##...#   G(200)
#...##..#   
#.G.#...#   G(200)
#.......#   
#.......#   
#########   
", test.GetMapString());
            Assert.AreEqual(937, test.CalculateTotalHitpoints());
        }



    }
}
