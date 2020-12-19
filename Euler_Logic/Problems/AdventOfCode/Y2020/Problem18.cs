using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem18 : AdventOfCodeBase {
        private enum enumOperator {
            Add,
            Multiply
        }

        public override string ProblemName => "Advent of Code 2020: 18";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            ulong num = 0;
            foreach (var line in input) {
                num += (ulong)Solve(line.Split(' '), false);
            }
            return num;
        }

        private ulong Answer2(List<string> input) {
            ulong num = 0;
            foreach (var line in input) {
                num += (ulong)Solve(line.Split(' '), true);
            }
            return num;
        }

        private ulong Solve(string[] split, bool usePrecedence) {
            var part = new Part() { Nums = new List<ulong>(), Operators = new List<enumOperator>() };
            int index = 0;
            do {
                while (split[index][0] == '(') {
                    var child = new Part() {
                        Nums = new List<ulong>(),
                        Operators = new List<enumOperator>(),
                        Parent = part
                    };
                    part = child;
                    split[index] = split[index].Substring(1);
                }
                if (split[index] == "*") {
                    part.Operators.Add(enumOperator.Multiply);
                } else if (split[index] == "+") {
                    part.Operators.Add(enumOperator.Add);
                } else {
                    int lastDigit = split[index].IndexOf(')');
                    if (lastDigit == -1) {
                        lastDigit = split[index].Length;
                    }
                    part.Nums.Add(Convert.ToUInt64(split[index].Substring(0, lastDigit)));
                    while (split[index].Last() == ')') {
                        var num = Evaluate(part, usePrecedence);
                        part = part.Parent;
                        part.Nums.Add(num);
                        split[index] = split[index].Substring(0, split[index].Length - 1);
                    }
                }
                index++;
            } while (index < split.Length);
            var result = Evaluate(part, usePrecedence);
            return result;
        }

        private ulong Evaluate(Part part, bool usePrecedence) {
            if (usePrecedence) {
                return EvaluatePrecedence(part);
            } else {
                return EvaluateNoPrecedence(part);
            }
        }

        private ulong EvaluateNoPrecedence(Part part) {
            var result = part.Nums[0];
            for (int index = 1; index < part.Nums.Count; index++) {
                if (part.Operators[index - 1] == enumOperator.Add) {
                    result += part.Nums[index];
                } else {
                    result *= part.Nums[index];
                }
            }
            return result;
        }

        private ulong EvaluatePrecedence(Part part) {
            for (int index = 0; index < part.Operators.Count; index++) {
                if (part.Operators[index] == enumOperator.Add) {
                    part.Nums[index] += part.Nums[index + 1];
                    part.Nums[index + 1] = 0;
                    part.Nums.RemoveAt(index + 1);
                    part.Operators.RemoveAt(index);
                    index--;
                }
            }
            return EvaluateNoPrecedence(part);
        }

        private List<string> TestInput() {
            return new List<string>() {
                "1 + 2 * 3 + 4 * 5 + 6",
                "1 + (2 * 3) + (4 * (5 + 6))",
                "2 * 3 + (4 * 5)",
                "5 + (8 * 3 + 9 + 3 * 4 * 3)",
                "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",
                "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"
            };
        }

        private class Part {
            public Part Parent { get; set; }
            public List<ulong> Nums { get; set; }
            public List<enumOperator> Operators { get; set; }
        }
    }
}
