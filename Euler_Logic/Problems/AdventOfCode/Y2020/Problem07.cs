using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem07 : AdventOfCodeBase {

        public override string ProblemName => "Advent of Code 2020: 7";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var bags = GetBags(input);
            var tree = GetTree(bags);
            return GetGoldBagCount(tree);
        }

        private ulong Answer2(List<string> input) {
            var bags = GetBags(input).ToDictionary(bag => bag.Id, bag => bag);
            var counts = new Dictionary<string, ulong>();
            foreach (var bag in bags.Values) {
                FindCounts(bag, bags, counts);
            }
            return counts["shiny gold"];
        }

        private ulong FindCounts(Bag bag, Dictionary<string, Bag> bags, Dictionary<string, ulong> counts) {
            if (!counts.ContainsKey(bag.Id)) {
                if (bag.ChildCount.Count == 0) {
                    counts.Add(bag.Id, 0);
                } else {
                    ulong sum = 0;
                    for (int index = 0; index < bag.ChildId.Count; index++) {
                        sum += bag.ChildCount[index] * (1 + FindCounts(bags[bag.ChildId[index]], bags, counts));
                    }
                    counts.Add(bag.Id, sum);
                }
            }
            return counts[bag.Id];
        }

        private int GetGoldBagCount(Dictionary<string, List<string>> tree) {
            var visited = new HashSet<string>();
            var current = new List<string>() { "shiny gold" };
            do {
                var next = new List<string>();
                foreach (var child in current) {
                    if (tree.ContainsKey(child)) {
                        foreach (var parent in tree[child]) {
                            if (!visited.Contains(parent)) {
                                visited.Add(parent);
                                next.Add(parent);
                            }
                        }
                    }
                }
                current = next;
            } while (current.Count > 0);
            return visited.Count;
        }

        private Dictionary<string, List<string>> GetTree(IEnumerable<Bag> bags) {
            var tree = new Dictionary<string, List<string>>();
            foreach (var bag in bags) {
                foreach (var child in bag.ChildId) {
                    if (!tree.ContainsKey(child)) {
                        tree.Add(child, new List<string>());
                    }
                    tree[child].Add(bag.Id);
                }
            }
            return tree;
        }

        private IEnumerable<Bag> GetBags(List<string> input) {
            return input.Select(line => {
                var bag = new Bag();
                bag.Initialize();
                var split = line.Split(' ');
                bag.Id = split[0] + " " + split[1];
                int index = 4;
                do {
                    if (split[index] != "no") {
                        bag.ChildCount.Add(Convert.ToUInt64(split[index]));
                        bag.ChildId.Add(split[index + 1] + " " + split[index + 2]);
                    }
                    index += 4;
                } while (index < split.Length);
                return bag;
            });
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "ight red bags contain 1 bright white bag, 2 muted yellow bags.",
                "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                "bright white bags contain 1 shiny gold bag.",
                "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
                "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                "faded blue bags contain no other bags.",
                "dotted black bags contain no other bags."
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "shiny gold bags contain 2 dark red bags.",
                "dark red bags contain 2 dark orange bags.",
                "dark orange bags contain 2 dark yellow bags",
                "dark yellow bags contain 2 dark green bags.",
                "dark green bags contain 2 dark blue bags.",
                "dark blue bags contain 2 dark violet bags.",
                "dark violet bags contain no other bags."
            };
        }

        private class Bag {
            public string Id { get; set; }
            public List<string> ChildId { get; set; }
            public List<ulong> ChildCount { get; set; }

            public void Initialize() {
                ChildId = new List<string>();
                ChildCount = new List<ulong>();
            }
        }
    }
}
