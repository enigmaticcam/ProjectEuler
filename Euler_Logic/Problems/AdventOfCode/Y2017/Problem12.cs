using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem12 : AdventOfCodeBase {
        private Dictionary<int, HashSet<int>> _hash;

        public override string ProblemName {
            get { return "Advent of Code 2017: 12"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            BuildHash(input);
            return FindContaining(0).Count;
        }

        private int Answer2(List<string> input) {
            BuildHash(input);
            return FindGroups();
        }

        private int FindGroups() {
            int count = 0;
            var visited = new HashSet<int>(_hash.Keys);
            var group = FindContaining(0);
            do {
                foreach (var num in group) {
                    visited.Remove(num);
                }
                if (visited.Count > 0) {
                    group = FindContaining(visited.First());
                }
                count++;
            } while (visited.Count > 0);
            return count;
        }

        private HashSet<int> FindContaining(int start) {
            var current = new List<int>() { start };
            var seenBefore = new HashSet<int>();
            do {
                var nextCurrent = new List<int>();
                foreach (var num in current) {
                    foreach (var next in _hash[num]) {
                        if (!seenBefore.Contains(next)) {
                            nextCurrent.Add(next);
                            seenBefore.Add(next);
                        }
                    }
                }
                current = nextCurrent;
            } while (current.Count > 0);
            return seenBefore;
        }

        private void BuildHash(List<string> input) {
            _hash = new Dictionary<int, HashSet<int>>();
            input.ForEach(line => {
                var split = line.Split(' ');
                var num1 = Convert.ToInt32(split[0]);
                for (int index = 2; index < split.Length; index++) {
                    AddToHash(num1, Convert.ToInt32(split[index].Trim().Replace(",", "")));
                }
            });
        }

        private void AddToHash(int num1, int num2) {
            if (!_hash.ContainsKey(num1)) {
                _hash.Add(num1, new HashSet<int>());
            }
            if (!_hash.ContainsKey(num2)) {
                _hash.Add(num2, new HashSet<int>());
            }
            _hash[num1].Add(num2);
            _hash[num2].Add(num1);
        }

        private List<string> TestInput() {
            return new List<string>() {
                "0 <-> 2",
                "1 <-> 1",
                "2 <-> 0, 3, 4",
                "3 <-> 2, 4",
                "4 <-> 2, 3, 6",
                "5 <-> 6",
                "6 <-> 4, 5"
            };
        }
    }
}
