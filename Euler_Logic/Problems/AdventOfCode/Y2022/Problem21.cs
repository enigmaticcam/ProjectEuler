using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem21 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 21";

        public override string GetAnswer() {
            return GetAnswer(Input()).ToString();
        }

        public override string GetAnswer2() {
            return GetAnswer2(Input()).ToString();
        }

        private long GetAnswer(List<string> input) {
            var state = new State();
            SetMonkeys(state, input);
            return Aggregate(state, "root", false);
        }

        private long GetAnswer2(List<string> input) {
            var state = new State();
            SetMonkeys(state, input);
            state.Monkeys["humn"].HasNumber = false;
            Aggregate(state, "root", true);
            return WorkBackwards(state);
        }

        private long WorkBackwards(State state) {
            var root = state.Monkeys["root"];
            var monkey1 = state.Monkeys[root.Monkey1];
            var monkey2 = state.Monkeys[root.Monkey2];
            if (!monkey1.HasNumber) {
                return WorkBackwards(state, monkey1.Name, monkey2.Number);
            } else {
                return WorkBackwards(state, monkey2.Name, monkey1.Number);
            }
        }

        private long WorkBackwards(State state, string name, long number) {
            var monkey = state.Monkeys[name];
            var monkey1 = state.Monkeys[monkey.Monkey1];
            var monkey2 = state.Monkeys[monkey.Monkey2];
            var withNum = monkey1;
            var withoutNum = monkey2;
            if (monkey2.HasNumber) {
                withNum = monkey2;
                withoutNum = monkey1;
            }
            long num = 0;
            if (monkey.Operator == enumOperator.Divide || monkey.Operator == enumOperator.Subtract) {
                if (monkey1.HasNumber) {
                    num = PerformOperator(monkey1.Number, number, monkey.Operator);
                } else {
                    num = PerformOperatorReverse(monkey2.Number, number, monkey.Operator);
                }
            } else {
                num = PerformOperatorReverse(number, withNum.Number, monkey.Operator);
            }
            if (withoutNum.Name == "humn") {
                return num;
            } else {
                return WorkBackwards(state, withoutNum.Name, num);
            }
        }

        private long Aggregate(State state, string name, bool ignoreHuman) {
            if (name == "humn" && ignoreHuman) return 0;
            var monkey = state.Monkeys[name];
            if (!monkey.HasNumber) {
                var num1 = Aggregate(state, monkey.Monkey1, ignoreHuman);
                var num2 = Aggregate(state, monkey.Monkey2, ignoreHuman);
                if (ignoreHuman) {
                    var monkey1 = state.Monkeys[monkey.Monkey1];
                    var monkey2 = state.Monkeys[monkey.Monkey2];
                    if (!monkey1.HasNumber || !monkey2.HasNumber) {
                        return 0;
                    }
                }
                monkey.Number = PerformOperator(num1, num2, monkey.Operator);
                monkey.HasNumber = true;
            }
            return monkey.Number;
        }

        private void SetMonkeys(State state, List<string> input) {
            var monkeys = input.Select(line => {
                var monkey = new Monkey();
                var split = line.Split(' ');
                monkey.Name = split[0].Replace(":", "");
                if (split.Length == 2) {
                    monkey.Number = Convert.ToInt64(split[1]);
                    monkey.HasNumber = true;
                } else {
                    monkey.Monkey1 = split[1];
                    monkey.Monkey2 = split[3];
                    monkey.Operator = GetOperator(split[2]);
                }
                return monkey;
            });
            state.Monkeys = monkeys.ToDictionary(x => x.Name, x => x);
        }

        private long PerformOperator(long value1, long value2, enumOperator op) {
            switch (op) {
                case enumOperator.Add: return value1 + value2;
                case enumOperator.Subtract: return value1 - value2;
                case enumOperator.Multiply: return value1 * value2;
                case enumOperator.Divide: return value1 / value2;
                default: throw new NotImplementedException();
            }
        }

        private long PerformOperatorReverse(long value1, long value2, enumOperator op) {
            switch (op) {
                case enumOperator.Add: return value1 - value2;
                case enumOperator.Subtract: return value1 + value2;
                case enumOperator.Multiply: return value1 / value2;
                case enumOperator.Divide: return value1 * value2;
                default: throw new NotImplementedException();
            }
        }

        private enumOperator GetOperator(string text) {
            switch (text) {
                case "+": return enumOperator.Add;
                case "-": return enumOperator.Subtract;
                case "*": return enumOperator.Multiply;
                case "/": return enumOperator.Divide;
                default: throw new NotImplementedException();
            }
        }

        private class State {
            public Dictionary<string, Monkey> Monkeys { get; set; }
        }

        private enum enumOperator {
            Add,
            Subtract,
            Multiply,
            Divide
        }

        private class Monkey {
            public string Name { get; set; }
            public bool HasNumber { get; set; }
            public long Number { get; set; }
            public string Monkey1 { get; set; }
            public string Monkey2 { get; set; }
            public enumOperator Operator { get; set; }
        }
    }
}
