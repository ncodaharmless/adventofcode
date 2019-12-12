using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Year2018
{
    class Day24
    {
        public List<UnitGroup> UnitGroups;

        public Day24(string input = SampleInput)
        {
            UnitGroups = ParseUnits(input);
        }

        private static List<UnitGroup> ParseUnits(string input)
        {
            bool isImmuneSystem = true;
            List<UnitGroup> units = new List<UnitGroup>();
            int immuneCount = 0;
            int infectionCount = 0;

            foreach (string line in input.SplitLine())
            {
                if (line.StartsWith("Immune System:"))
                    isImmuneSystem = true;
                else if (line.StartsWith("Infection:"))
                    isImmuneSystem = false;
                else
                {
                    var match = Regex.Match(line, "([0-9]+) units each with ([0-9]+) hit points (.*)with an attack that does ([0-9]+) (\\w+) damage at initiative ([0-9]+)");
                    if (!match.Success) throw new NotSupportedException(line);
                    AttackType[] weakness = new AttackType[0];
                    AttackType[] immunity = new AttackType[0];
                    if (match.Groups[3].Value.Length > 0)
                    {
                        foreach (string item in match.Groups[3].Value.TrimStart('(').TrimEnd(')').Split(';'))
                        {
                            string itemT = item.Trim().TrimStart('(').TrimEnd(')').Trim();
                            if (itemT.StartsWith("weak to "))
                            {
                                string[] parts = itemT.Substring("weak to ".Length).Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                weakness = parts.Select(p => (AttackType)Enum.Parse(typeof(AttackType), p, true)).ToArray();
                            }
                            else if (itemT.StartsWith("immune to "))
                            {
                                string[] parts = itemT.Substring("immune to ".Length).Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                immunity = parts.Select(p => (AttackType)Enum.Parse(typeof(AttackType), p, true)).ToArray();
                            }
                            else throw new NotSupportedException(itemT);
                        }
                    }
                    units.Add(new UnitGroup()
                    {
                        IsImmuneSystem = isImmuneSystem,
                        Count = int.Parse(match.Groups[1].Value),
                        UnitHitpoints = int.Parse(match.Groups[2].Value),
                        AttackDamage = int.Parse(match.Groups[4].Value),
                        AttackType = (AttackType)Enum.Parse(typeof(AttackType), match.Groups[5].Value, true),
                        Initiative = int.Parse(match.Groups[6].Value),
                        Weakness = weakness,
                        Immunity = immunity,
                        GroupNumber = isImmuneSystem ? (++immuneCount) : (++infectionCount),
                    });
                }

            }

            return units;
        }

        #region

        const string SampleInput = @"
Immune System:
228 units each with 8064 hit points (weak to cold) with an attack that does 331 cold damage at initiative 8
284 units each with 5218 hit points (immune to slashing, fire; weak to radiation) with an attack that does 160 radiation damage at initiative 10
351 units each with 4273 hit points (immune to radiation) with an attack that does 93 bludgeoning damage at initiative 2
2693 units each with 9419 hit points (immune to radiation; weak to bludgeoning) with an attack that does 30 cold damage at initiative 17
3079 units each with 4357 hit points (weak to radiation, cold) with an attack that does 13 radiation damage at initiative 1
906 units each with 12842 hit points (immune to fire) with an attack that does 100 fire damage at initiative 6
3356 units each with 9173 hit points (immune to fire; weak to bludgeoning) with an attack that does 24 radiation damage at initiative 9
61 units each with 9474 hit points with an attack that does 1488 bludgeoning damage at initiative 11
1598 units each with 10393 hit points (weak to fire) with an attack that does 61 cold damage at initiative 20
5022 units each with 6659 hit points (immune to bludgeoning, fire, cold) with an attack that does 12 radiation damage at initiative 15

Infection:
120 units each with 14560 hit points (weak to radiation, bludgeoning; immune to cold) with an attack that does 241 radiation damage at initiative 18
8023 units each with 19573 hit points (immune to bludgeoning, radiation; weak to cold, slashing) with an attack that does 4 bludgeoning damage at initiative 4
3259 units each with 24366 hit points (weak to cold; immune to slashing, radiation, bludgeoning) with an attack that does 13 slashing damage at initiative 16
4158 units each with 13287 hit points with an attack that does 6 fire damage at initiative 12
255 units each with 26550 hit points with an attack that does 167 bludgeoning damage at initiative 5
5559 units each with 21287 hit points with an attack that does 5 slashing damage at initiative 13
2868 units each with 69207 hit points (weak to bludgeoning; immune to fire) with an attack that does 33 cold damage at initiative 14
232 units each with 41823 hit points (immune to bludgeoning) with an attack that does 359 bludgeoning damage at initiative 3
729 units each with 41762 hit points (weak to bludgeoning, fire) with an attack that does 109 fire damage at initiative 7
3690 units each with 36699 hit points with an attack that does 17 slashing damage at initiative 19";

        internal int CombatUntilFinished(bool justTeam2 = false)
        {
            while (true)
            {
                Combat();
                if (Stalemate) return 0;
                int team1 = 0;
                int team2 = 0;
                foreach (var unit in UnitGroups)
                    if (unit.IsImmuneSystem)
                        team1 += unit.Count;
                    else
                        team2 += unit.Count;
                if (team1 == 0 || team2 == 0)
                    if (justTeam2)
                        return team2;
                    else
                        return team1 + team2;
            }
        }

        int roundNumber;
        internal void Combat()
        {
            roundNumber++;
            foreach (var unit in UnitGroups)
                unit.InitRound();

            foreach (var unit in UnitGroups.OrderByDescending(g => g.EffectivePower).ThenByDescending(g => g.Initiative))
                unit.SelectTarget(UnitGroups);
            int unitsLost = 0;
            foreach (var unit in UnitGroups.OrderByDescending(g => g.Initiative))
                if (unit.Count > 0)
                    unitsLost += unit.AttackTarget();
            if (unitsLost == 0) Stalemate = true;
            UnitGroups.RemoveAll(u => u.Count <= 0);
        }

        public bool Stalemate { get; set; }

        #endregion
    }

    public enum AttackType
    {
        Cold,
        Radiation,
        Bludgeoning,
        Fire,
        Slashing,
    }

    public class UnitGroup
    {
        public bool IsImmuneSystem;
        public int Count;
        public int UnitHitpoints;
        public AttackType[] Weakness;
        public AttackType[] Immunity;
        public AttackType AttackType;
        public int AttackDamage;
        public int Initiative;
        public int EffectivePower => Count * AttackDamage;
        public int GroupNumber;

        private UnitGroup Target;
        private bool HasBeenTargetted;
        private string GroupName => IsImmuneSystem ? "Immune" : "Infection";

        public void InitRound()
        {
            Target = null;
            HasBeenTargetted = false;
        }

        public void SelectTarget(List<UnitGroup> groups)
        {
            Target = groups.Where(g => g.IsImmuneSystem != IsImmuneSystem && !g.HasBeenTargetted && CalculateAttackDamageTo(g) > 0).OrderByDescending(g => CalculateAttackDamageTo(g)).ThenByDescending(g => g.EffectivePower).ThenByDescending(g => g.Initiative).FirstOrDefault();
            if (Target != null)
                Target.HasBeenTargetted = true;
        }

        public int CalculateAttackDamageTo(UnitGroup defender)
        {
            if (defender.Immunity.Contains(AttackType)) return 0;
            if (defender.Weakness.Contains(AttackType)) return EffectivePower * 2;
            return EffectivePower;
        }

        public int AttackTarget()
        {
            if (Target == null) return 0;

            int damage = CalculateAttackDamageTo(Target);
            int unitsLost = Target.TakeDamage(damage);
            //Console.WriteLine($"{GroupName} group {GroupNumber} dealt {Target.GroupName} group {Target.GroupNumber} {damage} damage and lost {unitsLost} units");
            return unitsLost;
        }

        private int TakeDamage(int damage)
        {
            int unitsLost = damage / UnitHitpoints;
            unitsLost = Math.Min(unitsLost, Count);
            Count -= unitsLost;
            return unitsLost;
        }

        public override string ToString()
        {
            return $"{GroupName} {GroupNumber} (COUNT: {Count})";
        }
    }

    [TestClass]
    public class TestDay24
    {
        [TestMethod]
        public void TestSample()
        {
            var test = new Day24(@"Immune System:
17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2
989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3

Infection:
801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1
4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4");

            Assert.AreEqual(185832, test.UnitGroups[2].CalculateAttackDamageTo(test.UnitGroups[0]));
            Assert.AreEqual(185832, test.UnitGroups[2].CalculateAttackDamageTo(test.UnitGroups[1]));
            Assert.AreEqual(107640, test.UnitGroups[3].CalculateAttackDamageTo(test.UnitGroups[1]));
            Assert.AreEqual(76619, test.UnitGroups[0].CalculateAttackDamageTo(test.UnitGroups[2]));
            Assert.AreEqual(153238, test.UnitGroups[0].CalculateAttackDamageTo(test.UnitGroups[3]));
            Assert.AreEqual(24725, test.UnitGroups[1].CalculateAttackDamageTo(test.UnitGroups[3]));
            Assert.AreEqual(17, test.UnitGroups[0].Count);
            Assert.AreEqual(989, test.UnitGroups[1].Count);
            Assert.AreEqual(801, test.UnitGroups[2].Count);
            Assert.AreEqual(4485, test.UnitGroups[3].Count);
            test.Combat();
            Assert.AreEqual(3, test.UnitGroups.Count);
            Assert.AreEqual(905, test.UnitGroups[0].Count);
            Assert.AreEqual(797, test.UnitGroups[1].Count);
            Assert.AreEqual(4434, test.UnitGroups[2].Count);
            test.Combat();
            Assert.AreEqual(761, test.UnitGroups[0].Count);
            Assert.AreEqual(793, test.UnitGroups[1].Count);
            Assert.AreEqual(4434, test.UnitGroups[2].Count);
            test.Combat();
            Assert.AreEqual(618, test.UnitGroups[0].Count);
            Assert.AreEqual(789, test.UnitGroups[1].Count);
            Assert.AreEqual(4434, test.UnitGroups[2].Count);
            test.Combat();
            Assert.AreEqual(475, test.UnitGroups[0].Count);
            Assert.AreEqual(786, test.UnitGroups[1].Count);
            Assert.AreEqual(4434, test.UnitGroups[2].Count);
            test.Combat();
            Assert.AreEqual(333, test.UnitGroups[0].Count);
            Assert.AreEqual(784, test.UnitGroups[1].Count);
            Assert.AreEqual(4434, test.UnitGroups[2].Count);
            test.Combat();
            Assert.AreEqual(191, test.UnitGroups[0].Count);
            Assert.AreEqual(783, test.UnitGroups[1].Count);
            Assert.AreEqual(4434, test.UnitGroups[2].Count);
            test.Combat();
            Assert.AreEqual(49, test.UnitGroups[0].Count);
            Assert.AreEqual(782, test.UnitGroups[1].Count);
            Assert.AreEqual(4434, test.UnitGroups[2].Count);
            test.Combat();
            Assert.AreEqual(2, test.UnitGroups.Count);
            Assert.AreEqual(782, test.UnitGroups[0].Count);
            Assert.AreEqual(4434, test.UnitGroups[1].Count);
        }

        [TestMethod]
        public void Part1()
        {
            var test = new Day24();
            Assert.AreEqual(26753, test.CombatUntilFinished());
        }

        [TestMethod]
        public void Part2()
        {
            int boost = 0;
            while (true)
            {
                var test = new Day24();
                foreach (var u in test.UnitGroups)
                    if (u.IsImmuneSystem)
                        u.AttackDamage += boost;
                int countRemaining = test.CombatUntilFinished();
                if (!test.Stalemate && test.UnitGroups[0].IsImmuneSystem)
                {
                    // boost is 79, remaining units 1852
                    Console.WriteLine("Win with boost: " + boost + " : count " + countRemaining);
                    break;
                }
                boost += 1;
            }
        }
    }
}
