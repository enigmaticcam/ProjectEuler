using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem14 : AdventOfCodeBase {
        private LinkedList<char> _template;
        private Dictionary<char, Dictionary<char, char>> _rules;

        public override string ProblemName {
            get { return "Advent of Code 2021: 14"; }
        }

        public override string GetAnswer() {
            GetTemplateAndRules(Input());
            return Answer2(40).ToString();
        }

        private int Answer1() {
            AddSteps(10);
            return SubtractLeastFromMost();
        }

        private ulong Answer2(int total) {
            _hash = new Dictionary<char, Dictionary<char, Dictionary<int, Dictionary<char, ulong>>>>();
            var result = new Dictionary<char, ulong>();
            var node = _template.First;
            result.Add(node.Value, 1);
            do {
                var nextResult = DP(node.Value, node.Next.Value, total - 1);
                AddResultToResult(result, nextResult);
                node = node.Next;
                if (!result.ContainsKey(node.Value)) {
                    result.Add(node.Value, 1);
                } else {
                    result[node.Value]++;
                }
            } while (node.Next != null);
            ulong min = ulong.MaxValue;
            ulong max = 0;
            foreach (var value in result.Values) {
                if (value > max) {
                    max = value;
                }
                if (value < min) {
                    min = value;
                }
            }
            return max - min;
        }

        private Dictionary<char, Dictionary<char, Dictionary<int, Dictionary<char, ulong>>>> _hash;
        private Dictionary<char, ulong> DP(char digit1, char digit2, int remaining) {
            if (!_hash.ContainsKey(digit1)) {
                _hash.Add(digit1, new Dictionary<char, Dictionary<int, Dictionary<char, ulong>>>());
            }
            if (!_hash[digit1].ContainsKey(digit2)) {
                _hash[digit1].Add(digit2, new Dictionary<int, Dictionary<char, ulong>>());
            }
            if (!_hash[digit1][digit2].ContainsKey(remaining)) {
                var result = new Dictionary<char, ulong>();
                var between = _rules[digit1][digit2];
                result.Add(between, 1);
                if (remaining == 0) {
                    return result;
                }
                var next1 = DP(digit1, between, remaining - 1);
                var next2 = DP(between, digit2, remaining - 1);
                AddResultToResult(result, next1);
                AddResultToResult(result, next2);
                _hash[digit1][digit2].Add(remaining, result);
            }
            return _hash[digit1][digit2][remaining];
        }

        private void AddResultToResult(Dictionary<char, ulong> keep, Dictionary<char, ulong> toAdd) {
            foreach (var digit in toAdd) {
                if (!keep.ContainsKey(digit.Key)) {
                    keep.Add(digit.Key, digit.Value);
                } else {
                    keep[digit.Key] += digit.Value;
                }
            }
        }
        

        private int SubtractLeastFromMost() {
            var hash = new Dictionary<char, int>();
            foreach (var node in _template) {
                if (!hash.ContainsKey(node)) {
                    hash.Add(node, 1);
                } else {
                    hash[node]++;
                }
            }
            return hash.Select(x => x.Value).Max() - hash.Select(x => x.Value).Min();
        }

        private void AddSteps(int maxCount) {
            for (int count = 1; count <= maxCount; count++) {
                var node = _template.First;
                do {
                    var next = _rules[node.Value][node.Next.Value];
                    node = node.Next;
                    _template.AddBefore(node, next);
                } while (node.Next != null);
            }
        }

        private void GetTemplateAndRules(List<string> input) {
            _template = new LinkedList<char>();
            _rules = new Dictionary<char, Dictionary<char, char>>();
            foreach (var digit in input[0]) {
                _template.AddLast(digit);
            }
            for (int index = 2; index < input.Count; index++) {
                var split = input[index].Split(' ');
                if (!_rules.ContainsKey(split[0][0])) {
                    _rules.Add(split[0][0], new Dictionary<char, char>());
                }
                _rules[split[0][0]].Add(split[0][1], split[2][0]);
            }
        }
    }
}
