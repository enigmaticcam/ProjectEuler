using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem06 : AdventOfCodeBase {
        private List<Instruction> _instructions;

        public enum enumInstructionType {
            On,
            Off,
            Toggle
        }
        public override string ProblemName => "Advent of Code 2015: 6";

        public override string GetAnswer() {
            GetInstructions(Input());
            return Answer1().ToString();
        }

        public override string GetAnswer2() {
            GetInstructions(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            int lit = 0;
            var hash = new bool[1000, 1000];
            foreach (var instruction in _instructions) {
                for (int x = instruction.Start[0]; x <= instruction.End[0]; x++) {
                    for (int y = instruction.Start[1]; y <= instruction.End[1]; y++) {
                        if (instruction.InstructionType == enumInstructionType.Off) {
                            if (hash[x, y]) lit--;
                            hash[x, y] = false;
                        } else if (instruction.InstructionType == enumInstructionType.On) {
                            if (!hash[x, y]) lit++;
                            hash[x, y] = true;
                        } else {
                            hash[x, y] = !hash[x, y];
                            if (hash[x, y]) {
                                lit++;
                            } else {
                                lit--;
                            }
                        };
                    }
                }
            }
            return lit;
        }

        private int Answer2() {
            int lit = 0;
            var hash = new int[1000, 1000];
            foreach (var instruction in _instructions) {
                for (int x = instruction.Start[0]; x <= instruction.End[0]; x++) {
                    for (int y = instruction.Start[1]; y <= instruction.End[1]; y++) {
                        int value = hash[x, y];
                        if (instruction.InstructionType == enumInstructionType.Off) {
                            hash[x, y] = Math.Max(value - 1, 0);
                        } else if (instruction.InstructionType == enumInstructionType.On) {
                            hash[x, y]++;
                        } else {
                            hash[x, y] += 2;
                        };
                        lit += hash[x, y] - value;
                    }
                }
            }
            return lit;
        }

        private void GetInstructions(List<string> input) {
            _instructions = input.Select(x => {
                var instruction = new Instruction();
                var split = x.Split(' ');
                string start = null;
                string end = null;
                if (split[0] == "toggle") {
                    instruction.InstructionType = enumInstructionType.Toggle;
                    start = split[1];
                    end = split[3];
                } else if (split[1] == "on") {
                    instruction.InstructionType = enumInstructionType.On;
                    start = split[2];
                    end = split[4];
                } else {
                    instruction.InstructionType = enumInstructionType.Off;
                    start = split[2];
                    end = split[4];
                }
                split = start.Split(',');
                instruction.Start = new int[2] { Convert.ToInt32(split[0]), Convert.ToInt32(split[1]) };
                split = end.Split(',');
                instruction.End = new int[2] { Convert.ToInt32(split[0]), Convert.ToInt32(split[1]) };
                return instruction;
            }).ToList();
        }

        private class Instruction {
            public int[] Start { get; set; }
            public int[] End { get; set; }
            public enumInstructionType InstructionType { get; set; }
        }
    }
}
