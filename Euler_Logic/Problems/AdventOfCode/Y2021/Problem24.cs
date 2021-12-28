using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem24 : AdventOfCodeBase {
        private List<Instruction> _instructions;
        private Dictionary<string, long> _variables;
        private long[] _input;
        private int _inputIndex;
        private Random _random;

        private enum enumInstructionType {
            Inp,
            Add,
            Mul,
            Div,
            Mod,
            Eql
        }

        public override string ProblemName {
            get { return "Advent of Code 2021: 24"; }
        }

        public override string GetAnswer() {
            _variables = new Dictionary<string, long>();
            GetInstructions(Input());
            SetVariables();
            return Answer1().ToString();
        }

        private List<string> _answers = new List<string>();
        private string Answer1() {
            _random = new Random();
            _input = new long[14];
            SetInitializeInput();
            //SetAllTo1();
            do {
                _inputIndex = 0;
                RandomizeInput();
                //ResetVariables();
                _var.Reset();
                //RunInstructions();
                RunManual();
                //_var.Validate(_variables);
                if (_var.Z == 0) {
                    _answers.Add(ConvertToString());
                }
                //Subtract1FromInput(13);
                //Add1ToInput(13);
            } while (true);
            return ConvertToString();
        }

        private void RandomizeInput() {
            for (int index = 0; index < 14; index++) {
                _input[index] = _random.Next(1, 10);
            }
        }

        private void SetAllTo1() {
            for (int index = 0; index < 14; index++) {
                _input[index] = 1;
            }
        }

        private class Variables {
            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }
            public void Reset() {
                X = 0;
                Y = 0;
                Z = 0;
            }
            public void Validate(Dictionary<string, long> variables) {
                if (variables["x"] != X || variables["y"] != Y || variables["z"] != Z) {
                    bool stop = true;
                }
            }
        }

        private Variables _var = new Variables();
        private void RunManual() {
            Manual(_input[0], 1, 12, 6);
            Manual(_input[1], 1, 11, 12);
            Manual(_input[2], 1, 10, 5);
            Manual(_input[3], 1, 10, 10);
            Manual(_input[4], 26, -16, 7);
            Manual(_input[5], 1, 14, 0);
            Manual(_input[6], 1, 12, 4);
            Manual(_input[7], 26, -4, 12);
            Manual(_input[8], 1, 15, 14);
            Manual(_input[9], 26, -7, 13);
            Manual(_input[10], 26, -8, 10);
            Manual(_input[11], 26, -4, 11);
            Manual(_input[12], 26, -15, 9);
            Manual(_input[13], 26, -8, 9);
        }
        private void Manual(long w, long a, long b, long c) {
            if ((_var.Z % 26) + b != w) {
                _var.X = 1;
                _var.Y = 26;
            } else {
                _var.X = 0;
                _var.Y = 1;
            }
            _var.Z /= a;
            _var.Z *= _var.Y;
            _var.Y = (w + c) * _var.X;
            _var.Z += _var.Y;
        }

        private string ConvertToString() {
            return string.Join("", _input);
        }

        private void Subtract1FromInput(int index) {
            if (_input[index] > 0) {
                _input[index]--;
            } else {
                _input[index] = 9;
                Subtract1FromInput(index - 1);
            }
        }

        private void Add1ToInput(int index) {
            if (_input[index] < 9) {
                _input[index]++;
            } else {
                _input[index] = 0;
                Add1ToInput(index - 1);
            }
        }

        private void SetInitializeInput() {
            for (int index = 0; index < 14; index++) {
                _input[index] = 9;
            }
        }

        private void ResetVariables() {
            _variables["w"] = 0;
            _variables["x"] = 0;
            _variables["y"] = 0;
            _variables["z"] = 0;
        }

        private void SetVariables() {
            _variables.Add("w", 0);
            _variables.Add("x", 0);
            _variables.Add("y", 0);
            _variables.Add("z", 0);
        }

        private void AddToInput(long num) {
            for (int index = 13; index >= 0; index--) {
                _input[index] = num % 10;
                num /= 10;
            }
            _inputIndex = 0;
        }

        private void RunInstructions() {
            foreach (var instruction in _instructions) {
                RunInstruction(instruction);
            }
        }

        private void RunInstruction(Instruction instruction) {
            switch (instruction.InstructionType) {
                case enumInstructionType.Inp:
                    RunInstructionInput(instruction);
                    break;
                case enumInstructionType.Add:
                    RunInstructionAdd(instruction);
                    break;
                case enumInstructionType.Mul:
                    RunInstructionMultiply(instruction);
                    break;
                case enumInstructionType.Div:
                    RunInstructionDivide(instruction);
                    break;
                case enumInstructionType.Eql:
                    RunInstructionEqual(instruction);
                    break;
                case enumInstructionType.Mod:
                    RunInstructionMod(instruction);
                    break;
            }
        }

        private void RunInstructionInput(Instruction instruction) {
            _variables[instruction.Variable1] = _input[_inputIndex];
            _inputIndex++;
        }

        private void RunInstructionAdd(Instruction instruction) {
            var value = GetValue(instruction);
            _variables[instruction.Variable1] += value;
        }

        private void RunInstructionMultiply(Instruction instruction) {
            var value = GetValue(instruction);
            _variables[instruction.Variable1] *= value;
        }

        private void RunInstructionDivide(Instruction instruction) {
            var value = GetValue(instruction);
            _variables[instruction.Variable1] /= value;
        }

        private void RunInstructionMod(Instruction instruction) {
            var value = GetValue(instruction);
            _variables[instruction.Variable1] %= value;
        }

        private void RunInstructionEqual(Instruction instruction) {
            var value = GetValue(instruction);
            _variables[instruction.Variable1] = value == _variables[instruction.Variable1] ? 1 : 0;
        }

        private long GetValue(Instruction instruction) {
            long value = instruction.Value2;
            if (instruction.IsValue2Variable) {
                value = _variables[instruction.Variable2];
            }
            return value;
        }

        private void GetInstructions(List<string> input) {
            _instructions = input.Select(x => {
                var instruction = new Instruction();
                var split = x.Split(' ');
                if (x[0] == 'i') {
                    instruction.InstructionType = enumInstructionType.Inp;
                } else if (x[0] == 'a') {
                    instruction.InstructionType = enumInstructionType.Add;
                } else if (x[0] == 'm' && x[1] == 'u') {
                    instruction.InstructionType = enumInstructionType.Mul;
                } else if (x[0] == 'd') {
                    instruction.InstructionType = enumInstructionType.Div;
                } else if (x[0] == 'm' && x[1] == 'o') {
                    instruction.InstructionType = enumInstructionType.Mod;
                } else {
                    instruction.InstructionType = enumInstructionType.Eql;
                }
                instruction.Variable1 = split[1];
                if (instruction.InstructionType != enumInstructionType.Inp) {
                    if ((int)split[2][0] <= 57) {
                        instruction.Value2 = Convert.ToInt64(split[2]);
                    } else {
                        instruction.Variable2 = split[2];
                        instruction.IsValue2Variable = true;
                    }
                }
                return instruction;
            }).ToList();
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public string Variable1 { get; set; }
            public long Value2 { get; set; }
            public string Variable2 { get; set; }
            public bool IsValue2Variable { get; set; }
        }
    }
}
