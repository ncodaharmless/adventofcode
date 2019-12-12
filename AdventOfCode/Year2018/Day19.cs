using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2018
{
    class Day19
    {
        public int InstructionPointer;
        public int InstructionPointerRegister;
        public int[] Registers = new int[6];
        public ProgramLine[] ProgramLines;

        public Day19(string input = SampleInput)
        {
            string[] lines = input.SplitLine();
            InstructionPointerRegister = Convert.ToInt32(lines[0].Substring(3).Trim());
            ProgramLines = lines.Skip(1).ToArray().Select(l => new ProgramLine()
            {
                Opcode = l.Split(' ')[0],
                A = Convert.ToInt32(l.Split(' ')[1]),
                B = Convert.ToInt32(l.Split(' ')[2]),
                C = Convert.ToInt32(l.Split(' ')[3])
            }).ToArray();

        }

        public class ProgramLine
        {
            public string Opcode;
            public int A;
            public int B;
            public int C;
        }

        public void Step()
        {
            Registers[InstructionPointerRegister] = InstructionPointer;
            var p = ProgramLines[InstructionPointer];
            Day16.RunStatement(p.Opcode, p.A, p.B, p.C, Registers);
            InstructionPointer = Registers[InstructionPointerRegister] + 1;
            if (InstructionPointer < 0 || InstructionPointer >= ProgramLines.Length)
            {
                Console.WriteLine("Value at register 0 = " + Registers[0]);
                throw new Exception();
            }
        }

        public void Part1()
        {
            while (true) Step();
        }

        public void Part2()
        {
            Registers[0] = 1;
            while (true) Step(); //TODO: doesn't work???
        }

        #region SampleInput

        const string SampleInput = @"#ip 4
addi 4 16 4
seti 1 4 3
seti 1 3 5
mulr 3 5 1
eqrr 1 2 1
addr 1 4 4
addi 4 1 4
addr 3 0 0
addi 5 1 5
gtrr 5 2 1
addr 4 1 4
seti 2 9 4
addi 3 1 3
gtrr 3 2 1
addr 1 4 4
seti 1 6 4
mulr 4 4 4
addi 2 2 2
mulr 2 2 2
mulr 4 2 2
muli 2 11 2
addi 1 2 1
mulr 1 4 1
addi 1 7 1
addr 2 1 2
addr 4 0 4
seti 0 8 4
setr 4 3 1
mulr 1 4 1
addr 4 1 1
mulr 4 1 1
muli 1 14 1
mulr 1 4 1
addr 2 1 2
seti 0 3 0
seti 0 6 4";

        #endregion
    }

    [TestClass]
    public class Day19Test
    {
        [TestMethod]
        public void TestSample()
        {
            var test = new Day19(@"#ip 0
seti 5 0 1
seti 6 0 2
addi 0 1 0
addr 1 2 3
setr 1 0 0
seti 8 0 4
seti 9 0 5");
            Assert.AreEqual(0, test.InstructionPointer);
            test.Step();
            Assert.AreEqual(1, test.InstructionPointer);
            test.Step();
            Assert.AreEqual(2, test.InstructionPointer);
            test.Step();
            Assert.AreEqual(4, test.InstructionPointer);
            test.Step();
            Assert.AreEqual(6, test.InstructionPointer);
        }
    }
}
