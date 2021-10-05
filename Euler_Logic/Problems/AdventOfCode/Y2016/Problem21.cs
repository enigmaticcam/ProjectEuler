using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem21 : AdventOfCodeBase {
        private List<Instruction> _instructions;
        private char[] _password = "abcdefgh".ToCharArray();
        private char[] _tempPassword;
        private Dictionary<int, int> _reverseReverseIndex;

        private enum enumInstructionType {
            SwapPosition,
            SwapLetter,
            Rotate,
            RotateIndex,
            Reverse,
            Move
        }

        public override string ProblemName => "Advent of Code 2016: 21";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private string Answer1(List<string> input) {
            _tempPassword = new char[_password.Length];
            GetInstructions(input);
            Perform();
            return new string(_password);
        }

        private string Answer2(List<string> input) {
            _password = "fbgdceah".ToCharArray();
            _tempPassword = new char[_password.Length];
            GetInstructions(input);
            SetReverseReverseIndex();
            PerformReverse();
            return new string(_password);
        }

        private void Perform() {
            foreach (var instruction in _instructions) {
                switch (instruction.InstructionType) {
                    case enumInstructionType.Move:
                        Move(instruction.ValueNum1, instruction.ValueNum2);
                        break;
                    case enumInstructionType.Reverse:
                        Reverse(instruction.ValueNum1, instruction.ValueNum2);
                        break;
                    case enumInstructionType.Rotate:
                        Rotate(instruction.ValueNum1, instruction.IsRotateRight);
                        break;
                    case enumInstructionType.RotateIndex:
                        RotateIndex(instruction.ValueText1);
                        break;
                    case enumInstructionType.SwapLetter:
                        SwapLetter(instruction.ValueText1, instruction.ValueText2);
                        break;
                    case enumInstructionType.SwapPosition:
                        SwapPosition(instruction.ValueNum1, instruction.ValueNum2);
                        break;
                }
            }
        }

        private void PerformReverse() {
            for (int index = _instructions.Count - 1; index >= 0; index--) {
                var instruction = _instructions[index];
                switch (instruction.InstructionType) {
                    case enumInstructionType.Move:
                        Move(instruction.ValueNum2, instruction.ValueNum1);
                        break;
                    case enumInstructionType.Reverse:
                        Reverse(instruction.ValueNum1, instruction.ValueNum2);
                        break;
                    case enumInstructionType.Rotate:
                        Rotate(instruction.ValueNum1, !instruction.IsRotateRight);
                        break;
                    case enumInstructionType.RotateIndex:
                        RotateIndexReverse(instruction.ValueText1);
                        break;
                    case enumInstructionType.SwapLetter:
                        SwapLetter(instruction.ValueText1, instruction.ValueText2);
                        break;
                    case enumInstructionType.SwapPosition:
                        SwapPosition(instruction.ValueNum2, instruction.ValueNum1);
                        break;
                }
            }
        }

        private void SetReverseReverseIndex() {
            _reverseReverseIndex = new Dictionary<int, int>();
            _reverseReverseIndex.Add(1, 7);
            _reverseReverseIndex.Add(3, 6);
            _reverseReverseIndex.Add(5, 5);
            _reverseReverseIndex.Add(7, 4);
            _reverseReverseIndex.Add(2, 2);
            _reverseReverseIndex.Add(4, 1);
            _reverseReverseIndex.Add(6, 0);
            _reverseReverseIndex.Add(0, 7);
        }

        private void SwapPosition(int index1, int index2) {
            var temp = _password[index1];
            _password[index1] = _password[index2];
            _password[index2] = temp;
        }

        private void SwapLetter(char digit1, char digit2) {
            for (int index = 0; index < _password.Length; index++) {
                if (_password[index] == digit1) {
                    _password[index] = digit2;
                } else if (_password[index] == digit2) {
                    _password[index] = digit1;
                }
            }
        }

        private void RotateIndexReverse(char digit) {
            for (int index = 0; index < _password.Length; index++) {
                if (_password[index] == digit) {
                    Rotate(_reverseReverseIndex[index], true);
                    break;
                }
            }
        }

        private void RotateIndex(char digit) {
            var rotateCount = 0;
            for (int index = 0; index < _password.Length; index++) {
                if (_password[index] == digit) {
                    rotateCount = index;
                    break;
                }
            }
            rotateCount = 1 + rotateCount + (rotateCount >= 4 ? 1 : 0);
            Rotate(rotateCount, true);
        }

        private void Rotate(int x, bool isRotateRight) {
            int direction = (isRotateRight ? -1 : 1);
            var maxCount = (x % _password.Length) * direction;
            for (int index = 0; index < _password.Length; index++) {
                var newIndex = (index + maxCount) % _password.Length;
                if (newIndex < 0) {
                    newIndex += _password.Length;
                }
                _tempPassword[index] = _password[newIndex];
            }
            for (int index = 0; index < _password.Length; index++) {
                _password[index] = _tempPassword[index];
            }
        }

        private void Reverse(int x, int y) {
            int maxCount = (y - x) / 2;
            for (int count = 0; count <= maxCount; count++) {
                var temp = _password[x + count];
                _password[x + count] = _password[y - count];
                _password[y - count] = temp;
            }
        }

        private void Move(int x, int y) {
            int direction = (x > y ? -1 : 1);
            for (int index = x + direction; index != y + direction; index += direction) {
                var temp = _password[index];
                _password[index] = _password[index - direction];
                _password[index - direction] = temp;
            }
        }

        private void GetInstructions(List<string> input) {
            _instructions = input.Select(line => {
                var instruction = new Instruction();
                var split = line.Split(' ');
                if (split[0] == "swap" && split[1] == "position") {
                    instruction.InstructionType = enumInstructionType.SwapPosition;
                    instruction.ValueNum1 = Convert.ToInt32(split[2]);
                    instruction.ValueNum2 = Convert.ToInt32(split[5]);
                } else if (split[0] == "swap" && split[1] == "letter") {
                    instruction.InstructionType = enumInstructionType.SwapLetter;
                    instruction.ValueText1 = split[2][0];
                    instruction.ValueText2 = split[5][0];
                } else if (split[0] == "rotate" && split[1] != "based") {
                    instruction.InstructionType = enumInstructionType.Rotate;
                    instruction.ValueNum1 = Convert.ToInt32(split[2]);
                    instruction.IsRotateRight = split[1] == "right";
                } else if (split[0] == "rotate" && split[1] == "based") {
                    instruction.InstructionType = enumInstructionType.RotateIndex;
                    instruction.ValueText1 = split[6][0];
                } else if (split[0] == "reverse") {
                    instruction.InstructionType = enumInstructionType.Reverse;
                    instruction.ValueNum1 = Convert.ToInt32(split[2]);
                    instruction.ValueNum2 = Convert.ToInt32(split[4]);
                } else {
                    instruction.InstructionType = enumInstructionType.Move;
                    instruction.ValueNum1 = Convert.ToInt32(split[2]);
                    instruction.ValueNum2 = Convert.ToInt32(split[5]);
                }
                return instruction;
            }).ToList();
        }

        private List<string> TestInput() {
            _password = "abcde".ToCharArray();
            return new List<string>() {
                "swap position 4 with position 0",
                "swap letter d with letter b",
                "reverse positions 0 through 4",
                "rotate left 1 step",
                "move position 1 to position 4",
                "move position 3 to position 0",
                "rotate based on position of letter b",
                "rotate based on position of letter d"
            };
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public int ValueNum1 { get; set; }
            public int ValueNum2 { get; set; }
            public char ValueText1 { get; set; }
            public char ValueText2 { get; set; }
            public bool IsRotateRight { get; set; }
        }
    }
}
