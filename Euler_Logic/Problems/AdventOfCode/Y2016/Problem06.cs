using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem06 : AdventOfCodeBase {
        private List<Dictionary<char, int>> _counts;

        public override string ProblemName => "Advent of Code 2016: 6";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private string Answer1(List<string> input) {
            PerformCounts(input);
            return GetMessageMostLikely();
        }

        private string Answer2(List<string> input) {
            PerformCounts(input);
            return GetMessageLeastLikely();
        }

        private void PerformCounts(List<string> input) {
            _counts = new List<Dictionary<char, int>>();
            for (int count = 0; count < input[0].Length; count++) {
                _counts.Add(new Dictionary<char, int>());
            }
            foreach (var line in input) {
                for (int index = 0; index < line.Length; index++) {
                    var digit = line[index];
                    if (!_counts[index].ContainsKey(digit)) {
                        _counts[index].Add(digit, 1);
                    } else {
                        _counts[index][digit]++;
                    }
                }
            }
        }

        private string GetMessageMostLikely() {
            var message = new char[_counts.Count];
            for (int index = 0; index < _counts.Count; index++) {
                var order = _counts[index].OrderByDescending(x => x.Value);
                message[index] = order.First().Key;
            }
            return new string(message);
        }

        private string GetMessageLeastLikely() {
            var message = new char[_counts.Count];
            for (int index = 0; index < _counts.Count; index++) {
                var order = _counts[index].OrderBy(x => x.Value);
                message[index] = order.First().Key;
            }
            return new string(message);
        }
    }
}
