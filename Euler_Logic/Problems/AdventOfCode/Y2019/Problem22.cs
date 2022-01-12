using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem22 : AdventOfCodeBase {
        private List<Instruction> _instructions;
        private BigInteger _a;
        private BigInteger _b;

        public enum enumInstructionType {
            DealNewStack,
            CutCards,
            DealWithIncrement
        }

        public override string ProblemName {
            get { return "Advent of Code 2019: 22"; }
        }

        public override string GetAnswer() {
            GetInstructions(Input());
            return Answer1(10007, 2019).ToString();
        }

        public override string GetAnswer2() {
            GetInstructions(Input());
            return Answer2(119315717514047, 101741582076661).ToString();
        }

        private BigInteger Answer1(BigInteger maxValue, BigInteger position) {
            SolveAB(maxValue);
            return (_a * position + _b) % maxValue;
        }

        private void SolveAB(BigInteger maxValue) {
            _a = 1;
            _b = 0;
            foreach (var instruction in _instructions) {
                switch (instruction.InstructionType) {
                    case enumInstructionType.CutCards:
                        _b -= instruction.Value;
                        if (_b < 0) _b += maxValue;
                        break;
                    case enumInstructionType.DealNewStack:
                        _a *= -1;
                        _b *= -1;
                        _a += maxValue;
                        _b += maxValue - 1;
                        if (_b < 0) _b += maxValue;
                        break;
                    case enumInstructionType.DealWithIncrement:
                        _a = (_a * instruction.Value) % maxValue;
                        _b = (_b * instruction.Value) % maxValue;
                        break;
                }
            }
        }

        private long Answer2(BigInteger maxValue, BigInteger count) {
            SolveAB(maxValue);

            // I have no idea how this works. But it was the last of all AoC problems, so sue me
            var r = (_b * Power.Exp(1 - _a, maxValue - 2, maxValue)) % maxValue;
            r = ((2020 - r) * Power.Exp(_a, count * (maxValue - 2), maxValue) + r) % maxValue;
            return (long)r;
        }

        private void GetInstructions(List<string> input) {
            _instructions = input.Select(x => {
                var split = x.Split(' ');
                var instruction = new Instruction();
                if (split[0] == "cut") {
                    instruction.InstructionType = enumInstructionType.CutCards;
                    instruction.Value = Convert.ToInt32(split[1]);
                } else if (split[1] == "with") {
                    instruction.InstructionType = enumInstructionType.DealWithIncrement;
                    instruction.Value = Convert.ToInt32(split[3]);
                } else {
                    instruction.InstructionType = enumInstructionType.DealNewStack;
                }
                return instruction;
            }).ToList();
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public long Value { get; set; }
        }
    }
}
