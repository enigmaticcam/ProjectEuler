using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem25 : AdventOfCodeBase {
        private List<Instruction> _instructions;
        private Dictionary<string, int> _registers;

        private enum enumInstructionType {
            cpy,
            inc,
            dec,
            jnz,
            tgl,
            outx
        }

        public override string ProblemName => "Advent of Code 2016: 25";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetInstructions(input);
            SetRegisters();
            return FindBest();
        }

        private int FindBest() {
            int value = -1;
            var hash = new HashSet<int>();
            do {
                hash.Add(_clockCount);
                _clockCount = 0;
                _last = 1;
                value++;
                _registers["a"] = value;
                _registers["b"] = 0;
                _registers["c"] = 0;
                _registers["d"] = 0;
            } while (!Perform());
            return value;
        }

        private int _clockCount;
        private bool Perform() {
            int index = 0;
            int value = 0;
            do {
                var instruction = _instructions[index];
                switch (instruction.InstructionType) {
                    case enumInstructionType.cpy:
                        int valueToCopy = 0;
                        if (instruction.IsValue1Register) {
                            valueToCopy = _registers[instruction.RegisterValue1];
                        } else {
                            valueToCopy = instruction.Value1;
                        }
                        _registers[instruction.RegisterValue2] = valueToCopy;
                        index++;
                        break;
                    case enumInstructionType.dec:
                        _registers[instruction.RegisterValue1]--;
                        index++;
                        break;
                    case enumInstructionType.inc:
                        _registers[instruction.RegisterValue1]++;
                        index++;
                        break;
                    case enumInstructionType.jnz:
                        value = 0;
                        if (instruction.IsValue1Register) {
                            value = _registers[instruction.RegisterValue1];
                        } else {
                            value = instruction.Value1;
                        }
                        if (value != 0) {
                            index += instruction.Value2;
                        } else {
                            index++;
                        }
                        break;
                    case enumInstructionType.outx:
                        value = 0;
                        if (instruction.IsValue1Register) {
                            value = _registers[instruction.RegisterValue1];
                        } else {
                            value = instruction.Value1;
                        }
                        if (IsClockSignal(value)) {
                            _clockCount++;
                        } else {
                            return false;
                        }
                        if (_clockCount == 8) {
                            return true;
                        }
                        index++;
                        break;
                }
            } while (index < _instructions.Count);
            return false;
        }

        private int _last;
        private bool IsClockSignal(int nextValue) {
            if (_last == 0 && nextValue == 1) {
                _last = nextValue;
                return true;
            } else if (_last == 1 && nextValue == 0) {
                _last = nextValue;
                return true;
            }
            return false;
        }

        private void GetInstructions(List<string> input) {
            _instructions = input.Select(line => {
                var instruction = new Instruction();
                var split = line.Split(' ');
                int value = 0;
                bool isNum = false;
                switch (split[0]) {
                    case "cpy":
                        instruction.InstructionType = enumInstructionType.cpy;
                        value = 0;
                        isNum = int.TryParse(split[1], out value);
                        if (isNum) {
                            instruction.Value1 = value;
                        } else {
                            instruction.RegisterValue1 = split[1];
                            instruction.IsValue1Register = true;
                        }
                        isNum = int.TryParse(split[2], out value);
                        if (isNum) {
                            instruction.Value2 = value;
                        } else {
                            instruction.RegisterValue2 = split[2];
                            instruction.IsValue2Register = true;
                        }
                        break;
                    case "inc":
                        instruction.InstructionType = enumInstructionType.inc;
                        instruction.RegisterValue1 = split[1];
                        break;
                    case "dec":
                        instruction.InstructionType = enumInstructionType.dec;
                        instruction.RegisterValue1 = split[1];
                        break;
                    case "jnz":
                        instruction.InstructionType = enumInstructionType.jnz;
                        value = 0;
                        isNum = int.TryParse(split[1], out value);
                        if (isNum) {
                            instruction.Value1 = value;
                        } else {
                            instruction.RegisterValue1 = split[1];
                            instruction.IsValue1Register = true;
                        }
                        instruction.Value2 = Convert.ToInt32(split[2]);
                        break;
                    case "out":
                        instruction.InstructionType = enumInstructionType.outx;
                        instruction.RegisterValue1 = split[1];
                        instruction.IsValue1Register = true;
                        break;
                }
                return instruction;
            }).ToList();
        }

        private void SetRegisters() {
            _registers = new Dictionary<string, int>();
            _registers.Add("a", 0);
            _registers.Add("b", 0);
            _registers.Add("c", 0);
            _registers.Add("d", 0);
        }

        private List<string> TestInput() {
            return new List<string>() {
                "cpy 2 a",
                "tgl a",
                "tgl a",
                "tgl a",
                "cpy 1 a",
                "dec a",
                "dec a"
            };
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public bool IsValue1Register { get; set; }
            public bool IsValue2Register { get; set; }
            public string RegisterValue1 { get; set; }
            public string RegisterValue2 { get; set; }
            public int Value1 { get; set; }
            public int Value2 { get; set; }
        }
    }
}
