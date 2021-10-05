using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem23 : AdventOfCodeBase {
        private List<Instruction> _instructions;
        private Dictionary<string, int> _registers;
        private Dictionary<enumInstructionType, enumInstructionType> _toggles;

        private enum enumInstructionType {
            cpy,
            inc,
            dec,
            jnz,
            tgl
        }

        public override string ProblemName => "Advent of Code 2016: 23";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetInstructions(input);
            SetRegisters();
            _registers["a"] = 7;
            SetToggles();
            return Perform(false);
        }

        private int Answer2(List<string> input) {
            GetInstructions(input);
            SetRegisters();
            _registers["a"] = 12;
            SetToggles();
            return Perform(true);
        }

        private int Perform(bool performSpecialInstruction) {
            int index = 0;
            int value = 0;
            do {
                if (performSpecialInstruction && index == 4) {
                    _registers["a"] += _registers["b"] * _registers["d"];
                    _registers["d"] = 0;
                    index = 10;
                }
                var instruction = _instructions[index];
                switch (instruction.InstructionType) {
                    case enumInstructionType.cpy:
                        int valueToCopy = 0;
                        if (instruction.IsValue1Register) {
                            valueToCopy = _registers[instruction.RegisterValue1];
                        } else {
                            valueToCopy = instruction.Value1;
                        }
                        if (_registers.ContainsKey(instruction.RegisterValue2)) {
                            _registers[instruction.RegisterValue2] = valueToCopy;
                        }
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
                            if (instruction.IsValue2Register) {
                                index += _registers[instruction.RegisterValue2];
                            } else {
                                index += instruction.Value2;
                            }
                        } else {
                            index++;
                        }
                        break;
                    case enumInstructionType.tgl:
                        value = _registers[instruction.RegisterValue1];
                        int indexToChange = value + index;
                        if (indexToChange < _instructions.Count) {
                            _instructions[indexToChange].InstructionType = _toggles[_instructions[indexToChange].InstructionType];
                        }
                        index++;
                        break;
                }
            } while (index < _instructions.Count);
            return _registers["a"];
        }

        private void SetToggles() {
            _toggles = new Dictionary<enumInstructionType, enumInstructionType>();
            _toggles.Add(enumInstructionType.cpy, enumInstructionType.jnz);
            _toggles.Add(enumInstructionType.dec, enumInstructionType.inc);
            _toggles.Add(enumInstructionType.inc, enumInstructionType.dec);
            _toggles.Add(enumInstructionType.jnz, enumInstructionType.cpy);
            _toggles.Add(enumInstructionType.tgl, enumInstructionType.inc);
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
                        instruction.RegisterValue2 = split[2];
                        instruction.IsValue2Register = true;
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
                        isNum = int.TryParse(split[2], out value);
                        if (isNum) {
                            instruction.Value2 = value;
                        } else {
                            instruction.RegisterValue2 = split[2];
                            instruction.IsValue2Register = true;
                        }
                        break;
                    case "tgl":
                        instruction.InstructionType = enumInstructionType.tgl;
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
