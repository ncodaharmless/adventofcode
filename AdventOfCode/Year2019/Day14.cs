﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Utils;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2019
{


    class Day14
    {
        #region Input
        const string Input = @"2 MLVWS, 8 LJNWK => 1 TNFQ
1 BWXQJ => 2 BMWK
1 JMGP, 3 WMJW => 9 JQCF
8 BWXQJ, 10 BJWR => 6 QWSLS
3 PLSH, 1 TNFQ => 6 CTPTW
11 GQDJG, 5 BMWK, 1 FZCK => 7 RQCNC
1 VWSRH => 7 PTGXM
104 ORE => 7 VWSRH
1 PTGXM, 13 WMJW, 1 BJGD => 7 KDHF
12 QWSLS, 3 PLSH, 4 HFBPX, 2 DFTH, 11 BCTRK, 4 JPKWB, 4 MKMRC, 3 XQJZQ => 6 BDJK
1 JQCF, 3 CVSC => 2 KRQHC
128 ORE => 7 QLRXZ
32 CXLWB, 18 TZWD => 1 HFQBG
31 KDHF => 9 BWXQJ
21 MLVWS => 9 LJNWK
3 QLRXZ => 5 CXLWB
3 LQWDR, 2 WSDH, 5 JPKWB, 1 RSTQC, 2 BJWR, 1 ZFNR, 16 QWSLS => 4 JTDT
3 BWXQJ, 14 JMGP => 9 MSTS
1 KXMKM, 2 LFCR => 9 DKWLT
6 CVSC => 3 FWQVP
6 XBVH, 1 HFBPX, 2 FZCK => 9 DFTH
9 MSTS => 2 BCTRK
1 PLSH, 28 MSTS => 2 FDKZ
10 XBVH, 5 BJWR, 2 FWQVP => 6 ZFNR
2 CVSC => 6 XBVH
1 BWXQJ, 2 KXMKM => 3 XQJZQ
1 VWSRH, 1 TZWD => 4 WMJW
14 CTPTW, 19 JMGP => 8 GRWK
13 NLGS, 1 PTGXM, 3 HFQBG => 5 BLVK
2 PTGXM => 7 NLGS
123 ORE => 3 DLPZ
2 ZNRPX, 35 DKWLT => 3 WSDH
1 TZWD, 1 BLVK, 9 BWXQJ => 2 MKDQF
2 DLPZ => 2 MLVWS
8 MKDQF, 4 JQCF, 12 VLMQJ => 8 VKCL
1 KRQHC => 7 BJWR
1 GRWK, 2 FWQVP => 9 LFCR
2 MSTS => 2 GQDJG
132 ORE => 9 TZWD
1 FWQVP => 8 RHKZW
43 FDKZ, 11 BJWR, 63 RHKZW, 4 PJCZB, 1 BDJK, 13 RQCNC, 8 JTDT, 3 DKWLT, 13 JPKWB => 1 FUEL
1 LFCR, 5 DFTH => 1 RSTQC
10 GQDJG => 8 KPTF
4 BWXQJ, 1 MKDQF => 7 JMGP
10 FGNPM, 23 DFTH, 2 CXLWB, 6 KPTF, 3 DKWLT, 10 MKDQF, 1 MJSG, 6 RSTQC => 8 PJCZB
8 VWSRH, 1 DLPZ => 7 BJGD
2 BLVK => 9 HBKH
16 LQWDR, 3 MSTS => 9 HFBPX
1 TNFQ, 29 HFQBG, 4 BLVK => 2 KXMKM
11 CVSC => 8 MJSG
3 LFCR => 6 FGNPM
11 HFQBG, 13 MKDQF => 1 FZCK
11 BWXQJ, 1 QLRXZ, 1 TNFQ => 9 KBTWZ
7 XQJZQ, 6 VKCL => 7 LQWDR
1 LJNWK, 4 HBKH => 1 CVSC
4 PLSH, 2 WSDH, 2 KPTF => 5 JPKWB
1 KPTF => 8 MKMRC
5 NLGS, 2 KDHF, 1 KBTWZ => 2 VLMQJ
4 MLVWS, 1 WMJW, 8 LJNWK => 1 PLSH
3 VKCL => 7 ZNRPX";
        #endregion

        internal Dictionary<string, Reaction> _Reactions;

        public Day14(string input = Input)
        {
            _Reactions = input.SplitLine().Select(r => Reaction.Parse(r)).ToDictionary(r => r.Produces, r => r);
        }

        internal class RequiredUsedWasted
        {
            public long Required;
            public long Reactions;
            public long Wasted;

            public override string ToString()
            {
                return $"Req {Required}, ({Reactions}) Waste {Wasted}";
            }
        }

        public long RequiredToProduce(string toProduce, string raw, long count)
        {
            Dictionary<string, RequiredUsedWasted> rawRequired = new Dictionary<string, RequiredUsedWasted>();
            rawRequired.Add("ORE", new RequiredUsedWasted());
            foreach (string key in _Reactions.Keys)
                rawRequired.Add(key, new RequiredUsedWasted());
            _Reactions[toProduce].RequiredToProduce(_Reactions, count, rawRequired);
            return rawRequired[raw].Required;
        }

        internal long Part1()
        {
            return RequiredToProduce("FUEL", "ORE", 1);
        }

        internal long Part2()
        {
            long starting = Trillion / RequiredToProduce("FUEL", "ORE", 1);
            return RequiredInRange(starting, RequiredToProduce("FUEL", "ORE", 1) * 10);
        }

        const long Trillion = 1000000000000;

        internal long RequiredInRange(long startingValue, long range)
        {
            long required = RequiredToProduce("FUEL", "ORE", startingValue);
            if (required < Trillion)
            {
                if (range < 2)
                {
                    return startingValue;
                }
                return RequiredInRange(startingValue + (range / 2), range / 2);
            }
            else
            {
                if (range < 2)
                {
                    return startingValue - 1;
                }
                return RequiredInRange(startingValue - (range / 2), range / 2);
            }
        }

        internal class Reaction
        {
            public Dictionary<string, long> Requires { get; } = new Dictionary<string, long>();
            public string Produces { get; set; }
            public long ProducesCount { get; set; }

            public static Reaction Parse(string inputLine)
            {
                var re = new Reaction();
                string[] parts = inputLine.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var req in parts[0].SplitComma())
                {
                    var reg = Regex.Match(req, "([0-9]+) ([A-Z]+)");
                    Assert.IsTrue(reg.Success);
                    long requiresCount = Convert.ToInt32(reg.Groups[1].Value);
                    string requires = reg.Groups[2].Value;
                    re.Requires.Add(requires, requiresCount);
                }
                var r = Regex.Match(parts[1], "([0-9]+) ([A-Z]+)");
                Assert.IsTrue(r.Success);
                re.ProducesCount = Convert.ToInt32(r.Groups[1].Value);
                re.Produces = r.Groups[2].Value;
                return re;
            }

            internal void RequiredToProduce(Dictionary<string, Reaction> reactions, long toProduceCount, Dictionary<string, RequiredUsedWasted> requiredCount)
            {
                requiredCount[Produces].Required += toProduceCount;
                long requiredToAddCount = (toProduceCount - requiredCount[Produces].Wasted);
                if (requiredToAddCount > 0)
                {
                    long newReactionsRequired = requiredToAddCount / ProducesCount;
                    if (newReactionsRequired * ProducesCount < requiredToAddCount)
                        newReactionsRequired++;
                    foreach (var requirement in Requires)
                    {
                        if (requirement.Key == "ORE")
                        {
                            requiredCount["ORE"].Required += requirement.Value * newReactionsRequired;
                        }
                        else
                            reactions[requirement.Key].RequiredToProduce(reactions, requirement.Value * newReactionsRequired, requiredCount);
                    }
                    requiredCount[Produces].Reactions += newReactionsRequired;
                }
                requiredCount[Produces].Wasted = (ProducesCount * requiredCount[Produces].Reactions) - requiredCount[Produces].Required;
            }
        }
    }

    [TestClass]
    public class TestDay14
    {
        [TestMethod]
        public void Example1()
        {
            var d = new Day14(@"
10 ORE => 10 A
1 ORE => 1 B
7 A, 1 B => 1 C
7 A, 1 C => 1 D
7 A, 1 D => 1 E
7 A, 1 E => 1 FUEL");
            Assert.AreEqual(10, d.RequiredToProduce("A", "ORE", 1));
            Assert.AreEqual(10, d.RequiredToProduce("A", "ORE", 10));
            Assert.AreEqual(20, d.RequiredToProduce("A", "ORE", 15));
            Assert.AreEqual(1, d.RequiredToProduce("B", "ORE", 1));
            Assert.AreEqual(28, d.RequiredToProduce("FUEL", "A", 1));
            Assert.AreEqual(31, d.Part1());

        }

        [TestMethod]
        public void Part1()
        {
            Assert.AreEqual(1046184, new Day14().Part1());
        }
        [TestMethod]
        public void Part2()
        {
            Assert.AreEqual(1639374, new Day14().Part2());
        }
    }

}
