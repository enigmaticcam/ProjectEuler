using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem06 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2020: 6";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            int count = 0;
            var hash = new HashSet<char>();
            foreach (var line in input) {
                if (line == "") {
                    hash = new HashSet<char>();
                } else {
                    foreach (var digit in line) {
                        if (!hash.Contains(digit)) {
                            count++;
                            hash.Add(digit);
                        }
                    }
                }
            }
            return count;
        }

        private int Answer2(List<string> input) {
            int count = 0;
            HashSet<char> hash = null;
            foreach (var line in input) {
                if (line == "") {
                    count += hash.Count;
                    hash = null;
                } else if (hash == null) {
                    hash = new HashSet<char>(line.ToCharArray());
                } else {
                    var next = new HashSet<char>();
                    foreach (var digit in line) {
                        if (hash.Contains(digit)) {
                            next.Add(digit);
                        }
                    }
                    hash = next;
                }
            }
            return count + hash.Count;
        }

        private List<string> TestInput() {
            return new List<string>() {
                "abc",
                "",
                "a",
                "b",
                "c",
                "",
                "ab",
                "ac",
                "",
                "a",
                "a",
                "a",
                "a",
                "",
                "b"
            };
        }
    }
}
