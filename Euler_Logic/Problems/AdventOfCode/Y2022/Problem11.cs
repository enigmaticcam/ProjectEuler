using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem11 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 11";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            var monkeys = GetMonkeys(input);
            var product = GetProduct(monkeys);
            Process(20, monkeys, true, product);
            return GetMonkeyBusiness(monkeys);
        }

        private ulong Answer2(List<string> input) {
            var monkeys = GetMonkeys(input);
            var product = GetProduct(monkeys);
            Process(10000, monkeys, false, product);
            return GetMonkeyBusiness(monkeys);
        }

        private ulong GetProduct(List<Monkey> monkeys) {
            ulong product = 1;
            foreach (var monkey in monkeys) {
                if ((product % monkey.TestValue) != 0)
                    product *= monkey.TestValue;
            }
            return product;
        }

        private ulong GetMonkeyBusiness(List<Monkey> monkeys) {
            var list = monkeys
                .OrderByDescending(x => x.Inspected)
                .Take(2)
                .ToList();
            return list[0].Inspected * list[1].Inspected;
        }

        private void Process(int totalRounds, List<Monkey> monkeys, bool littleWorry, ulong product) {
            for (int round = 1; round <= totalRounds; round++) {
                for (int index = 0; index < monkeys.Count; index++) {
                    PerformMonkey(monkeys, index, littleWorry, product);
                }
            }
        }

        private void PerformMonkey(List<Monkey> monkeys, int monkeyIndex, bool littleWorry, ulong product) {
            var monkey = monkeys[monkeyIndex];
            while (monkey.Items.Count > 0) {
                monkey.Inspected++;
                var item = monkey.Items.First.Value;
                monkey.Items.RemoveFirst();
                item = monkey.Operation(item);
                if (littleWorry) {
                    item /= 3;
                } else {
                    item %= product;
                }
                if ((item % monkey.TestValue) == 0) {
                    monkeys[monkey.TestTrue].Items.AddLast(item);
                } else {
                    monkeys[monkey.TestFalse].Items.AddLast(item);
                }
            }
        }

        private List<Monkey> GetMonkeys(List<string> input) {
            var monkeys = new List<Monkey>();
            int index = 0;
            do {
                var monkey = new Monkey();
                monkeys.Add(monkey);
                GetMonkeys_Items(input[index + 1], monkey);
                GetMonkeys_Operation(input[index + 2], monkey);
                GetMonkeys_TestValue(input[index + 3], monkey);
                GetMonkeys_TestTrueFalse(input[index + 4], input[index + 5], monkey);
                index += 7;
            } while (index < input.Count);
            return monkeys;
        }

        private void GetMonkeys_Items(string text, Monkey monkey) {
            text = text.Substring(18);
            var split = text.Split(',');
            monkey.Items = new LinkedList<ulong>(split.Select(x => Convert.ToUInt64(x)));
        }

        private void GetMonkeys_Operation(string text, Monkey monkey) {
            text = text.Substring(text.IndexOf("old ") + 4);
            if (text == "* old") {
                monkey.Operation = old => old * old;
            } else {
                var split = text.Split(' ');
                var value = Convert.ToUInt64(split.Last());
                if (split[0] == "+") {
                    monkey.Operation = old => old + value;
                } else {
                    monkey.Operation = old => old * value;
                }
            }
        }

        private void GetMonkeys_TestValue(string text, Monkey monkey) {
            var split = text.Split(' ');
            monkey.TestValue = Convert.ToUInt64(split.Last());
        }

        private void GetMonkeys_TestTrueFalse(string text1, string text2, Monkey monkey) {
            var split = text1.Split(' ');
            monkey.TestTrue = Convert.ToInt32(split.Last());
            split = text2.Split(' ');
            monkey.TestFalse = Convert.ToInt32(split.Last());
        }

        public class Monkey {
            public LinkedList<ulong> Items { get; set; }
            public Func<ulong, ulong> Operation { get; set; }
            public ulong TestValue { get; set; }
            public int TestTrue { get; set; }
            public int TestFalse { get; set; }
            public ulong Inspected { get; set; }
        }
    }
}
