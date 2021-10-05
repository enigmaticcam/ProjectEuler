using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem23 : AdventOfCodeBase {
        private List<Instruction> _instructions;
        private Dictionary<string, uint> _registers;

        private enum enumInstructionType {
            hlf,
            tpl,
            inc,
            jmp,
            jie,
            jio
        }

        public override string ProblemName => "Advent of Code 2015: 23";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private uint Answer1(List<string> input) {
            GetInstructions(input);
            SetRegisters();
            Perform();
            return _registers["b"];
        }

        private uint Answer2(List<string> input) {
            GetInstructions(input);
            SetRegisters();
            _registers["a"] = 1;
            Perform();
            return _registers["b"];
        }

        private void SetRegisters() {
            _registers = new Dictionary<string, uint>();
            _registers.Add("a", 0);
            _registers.Add("b", 0);
        }

        private void Perform() {
            int lineNum = 0;
            do {
                var next = _instructions[lineNum];
                int value = 0;
                switch (next.InstructionType) {
                    case enumInstructionType.hlf:
                        _registers[next.Value1] /= 2;
                        lineNum++;
                        break;
                    case enumInstructionType.tpl:
                        _registers[next.Value1] *= 3;
                        lineNum++;
                        break;
                    case enumInstructionType.inc:
                        _registers[next.Value1]++;
                        lineNum++;
                        break;
                    case enumInstructionType.jmp:
                        value = Convert.ToInt32(next.Value1);
                        lineNum += value;
                        break;
                    case enumInstructionType.jie:
                        if (_registers[next.Value1] % 2 == 0) {
                            value = Convert.ToInt32(next.Value2);
                            lineNum += value;
                        } else {
                            lineNum++;
                        }
                        break;
                    case enumInstructionType.jio:
                        if (_registers[next.Value1] == 1) {
                            value = Convert.ToInt32(next.Value2);
                            lineNum += value;
                        } else {
                            lineNum++;
                        }
                        break;
                }
            } while (lineNum < _instructions.Count && lineNum >= 0);
        }

        private void GetInstructions(List<string> input) {
            _instructions = input.Select(line => {
                var instruction = new Instruction();
                var split = line.Split(' ');
                switch (split[0]) {
                    case "hlf":
                        instruction.InstructionType = enumInstructionType.hlf;
                        break;
                    case "tpl":
                        instruction.InstructionType = enumInstructionType.tpl;
                        break;
                    case "inc":
                        instruction.InstructionType = enumInstructionType.inc;
                        break;
                    case "jmp":
                        instruction.InstructionType = enumInstructionType.jmp;
                        break;
                    case "jie":
                        instruction.InstructionType = enumInstructionType.jie;
                        break;
                    case "jio":
                        instruction.InstructionType = enumInstructionType.jio;
                        break;
                }
                instruction.Value1 = split[1].Replace(",", "");
                if (split.Length == 3) {
                    instruction.Value2 = split[2];
                }
                return instruction;
            }).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "inc a",
                "jio a, +2",
                "tpl a",
                "inc a"
            };
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public string Value1 { get; set; }
            public string Value2 { get; set; }
        }
    }
}
