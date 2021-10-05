using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem23 : AdventOfCodeBase {
        private List<Instruction> _instructions;
        private Dictionary<string, long> _registers;
        private int _index;
        private int _mulCount;

        private enum enumInstructionType {
            set,
            sub,
            mul,
            jnz
        }

        public override string ProblemName {
            get { return "Advent of Code 2017: 23"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetInstructions(input);
            SetRegisters();
            Perform();
            return _mulCount;
        }

        private long Answer2(List<string> input) {
            GetInstructions(input);
            SetRegisters();
            _registers["a"] = 1;
            Perform();
            return _registers["h"];
        }

        private void Perform() {
            do {
                if (_index == 11) {
                    _registers["e"] = _registers["b"];
                    _registers["g"] = 0;
                    if (_registers["b"] % _registers["d"] == 0) {
                        _registers["f"] = 0;
                    }
                    _index = 20;
                }
                var instruction = _instructions[_index];
                switch (instruction.InstructionType) {
                    case enumInstructionType.jnz:
                        PerformJNZ(instruction);
                        break;
                    case enumInstructionType.mul:
                        PerformMUL(instruction);
                        break;
                    case enumInstructionType.set:
                        PerformSET(instruction);
                        break;
                    case enumInstructionType.sub:
                        PerformSUB(instruction);
                        break;
                }
            } while (_index < _instructions.Count);
        }

        private void PerformJNZ(Instruction instruction) {
            var value = (instruction.IsValueRegister1 ? _registers[instruction.Register1] : instruction.Value1);
            if (value != 0) {
                var offset = (instruction.IsValueRegister2 ? _registers[instruction.Register2] : instruction.Value2);
                _index += (int)offset;
            } else {
                _index++;
            }
        }

        private void PerformMUL(Instruction instruction) {
            var value = (instruction.IsValueRegister2 ? _registers[instruction.Register2] : instruction.Value2);
            _registers[instruction.Register1] *= value;
            _index++;
            _mulCount++;
        }

        private void PerformSET(Instruction instruction) {
            var value = (instruction.IsValueRegister2 ? _registers[instruction.Register2] : instruction.Value2);
            _registers[instruction.Register1] = value;
            _index++;
        }

        private void PerformSUB(Instruction instruction) {
            var value = (instruction.IsValueRegister2 ? _registers[instruction.Register2] : instruction.Value2);
            _registers[instruction.Register1] -= value;
            _index++;
        }

        private void SetRegisters() {
            _registers = new Dictionary<string, long>();
            _registers.Add("a", 0);
            _registers.Add("b", 0);
            _registers.Add("c", 0);
            _registers.Add("d", 0);
            _registers.Add("e", 0);
            _registers.Add("f", 0);
            _registers.Add("g", 0);
            _registers.Add("h", 0);
        }

        private void GetInstructions(List<string> input) {
            _instructions = input.Select(line => {
                var instruction = new Instruction();
                var split = line.Split(' ');
                switch (split[0]) {
                    case "set":
                        instruction.InstructionType = enumInstructionType.set;
                        break;
                    case "sub":
                        instruction.InstructionType = enumInstructionType.sub;
                        break;
                    case "mul":
                        instruction.InstructionType = enumInstructionType.mul;
                        break;
                    case "jnz":
                        instruction.InstructionType = enumInstructionType.jnz;
                        break;
                }
                int value = 0;
                bool isNum = int.TryParse(split[1], out value);
                if (isNum) {
                    instruction.Value1 = value;
                } else {
                    instruction.Register1 = split[1];
                    instruction.IsValueRegister1 = true;
                }
                if (split.Length == 3) {
                    isNum = int.TryParse(split[2], out value);
                    if (isNum) {
                        instruction.Value2 = value;
                    } else {
                        instruction.Register2 = split[2];
                        instruction.IsValueRegister2 = true;
                    }
                }
                return instruction;
            }).ToList();
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public long Value1 { get; set; }
            public long Value2 { get; set; }
            public string Register1 { get; set; }
            public string Register2 { get; set; }
            public bool IsValueRegister1 { get; set; }
            public bool IsValueRegister2 { get; set; }
        }
    }
}
