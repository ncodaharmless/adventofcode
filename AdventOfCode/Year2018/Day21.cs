﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2018
{
    class Day21
    {
        public static void RunDay21(bool part1 = false)
        {
            var program = new Day19(@"#ip 1
seti 123 0 2
bani 2 456 2
eqri 2 72 2
addr 2 1 1
seti 0 0 1
seti 0 3 2
bori 2 65536 5
seti 4843319 1 2
bani 5 255 4
addr 2 4 2
bani 2 16777215 2
muli 2 65899 2
bani 2 16777215 2
gtir 256 5 4
addr 4 1 1
addi 1 1 1
seti 27 4 1
seti 0 7 4
addi 4 1 3
muli 3 256 3
gtrr 3 5 3
addr 3 1 1
addi 1 1 1
seti 25 0 1
addi 4 1 4
seti 17 0 1
setr 4 1 5
seti 7 3 1
eqrr 2 0 4
addr 4 1 1
seti 5 3 1");
            // part1-breakpoing on "eqrr"
            //program.Registers[0] = 8797248;
            HashSet<int> x = new HashSet<int>();
            while (true)
            {
                if (program.ProgramLines[program.InstructionPointer].Opcode == "eqrr")
                {
                    //part1 is the program.Registers[2] value!
                    if (part1)
                    {
                        Console.WriteLine(program.Registers[2]);
                        return;
                    }
                    x.Add(program.Registers[2]);
                }
                program.Step();
            }

        }

    }
}
