using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem03 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 3";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var sacks = GetRucksacks(input);
            SetShared(sacks);
            return sacks
                .Select(x => x.Priority)
                .Sum();
        }

        private int Answer2(List<string> input) {
            var sacks = GetRucksacks(input);
            var groups = GetGroups(sacks);
            SetShared(groups);
            return groups
                .Select(x => x.Priority)
                .Sum();
        }

        private void SetShared(List<Group> groups) {
            foreach (var group in groups) {
                SetShared(group);
            }
        }

        private void SetShared(Group group) {
            var hash = new Dictionary<char, int>();
            int powerOf2 = 1;
            foreach (var set in group.Set) {
                foreach (var digit in set) {
                    if (!hash.ContainsKey(digit)) {
                        if (powerOf2 == 1)
                            hash.Add(digit, powerOf2);
                    } else {
                        hash[digit] |= powerOf2;
                    }
                }
                powerOf2 *= 2;
            }
            group.Shared = hash.First(x => x.Value == 7).Key;
            group.Priority = GetPriority(group.Shared);
        }

        private List<Group> GetGroups(List<Rucksack> sacks) {
            var list = new List<Group>();
            var group = new Group() { Set = new string[3] };
            int count = 0;
            foreach (var sack in sacks) {
                group.Set[count] = sack.Full;
                if (count == 2) {
                    list.Add(group);
                    group = new Group() { Set = new string[3] };
                    count = 0;
                } else {
                    count++;
                }
            }
            return list;
        }

        private void SetShared(List<Rucksack> sacks) {
            int index = 0;
            foreach (var sack in sacks) {
                SetShared(sack, index);
                index++;
            }
        }

        private void SetShared(Rucksack sack, int index) {
            var hash = new HashSet<char>(sack.Set1.ToArray());
            foreach (var digit in sack.Set2) {
                if (hash.Contains(digit)) {
                    sack.Shared = digit;
                    sack.Priority = GetPriority(sack.Shared);
                    break;
                }
            }
        }

        private int GetPriority(char shared) {
            var ascii = (int)shared;
            if (ascii <= 90) {
                return ascii - ((int)'Z' - 52);
            } else {
                return ascii - ((int)'z' - 26);
            }
        }

        private List<Rucksack> GetRucksacks(List<string> input) {

            return input.Select(x => {
                var len = x.Length / 2;
                var set1 = x.Substring(0, len);
                var set2 = x.Substring(len);
                return new Rucksack() { Set1 = set1, Set2 = set2, Full = x };
            }).ToList();
        }

        private class Group {
            public string[] Set { get; set; }
            public char Shared { get; set; }
            public int Priority { get; set; }
        }

        private class Rucksack {
            public string Set1 { get; set; }
            public string Set2 { get; set; }
            public string Full { get; set; }
            public char Shared { get; set; }
            public int Priority { get; set; }
        }
    }
}
