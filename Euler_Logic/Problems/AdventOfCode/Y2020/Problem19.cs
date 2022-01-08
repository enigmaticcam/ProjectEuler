using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem19 : AdventOfCodeBase {
        private Dictionary<int, Rule> _rules;
        private List<List<Rule>> _tiers;
        private List<string> _messages;
        private Dictionary<int, HashSet<string>> _valids;
        private int _maxLength;

        public override string ProblemName => "Advent of Code 2020: 19";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetRulesAndMessages(input);
            SetMaxLength();
            SetTiers();
            BuildValids();
            return FinalCount();
        }

        private int Answer2(List<string> input) {
            GetRulesAndMessages(input);
            SetMaxLength();
            SetTiers();
            BuildValids();
            return AddRecursionRules();
        }

        private int AddRecursionRules() {
            int count = 0;
            foreach (var message in _messages) {
                if (RecursionRule8("", message)) count++;
            }
            return count;
        }

        private bool RecursionRule8(string current, string message) {
            foreach (var message1 in _valids[42]) {
                var temp = current + message1;
                if (message.IndexOf(temp) == 0) {
                    var result = Recursion11("", message.Substring(temp.Length));
                    if (result) return true;
                    result = RecursionRule8(temp, message);
                    if (result) return true;
                }
            }
            return false;
        }

        private bool Recursion11(string current, string message) {
            foreach (var message1 in _valids[42]) {
                var temp1 = message1 + current;
                if (message.IndexOf(temp1) >= 0) {
                    foreach (var message2 in _valids[31]) {
                        var temp2 = temp1 + message2;
                        if (message == temp2) {
                            return true;
                        } else if (message.IndexOf(temp2) >= 0) {
                            var result = Recursion11(temp2, message);
                            if (result) return true;
                        }
                    }
                }
            }
            return false;
        }

        private int FinalCount() {
            int count = 0;
            var rule0 = _valids[0];
            foreach (var message in _messages) {
                if (rule0.Contains(message)) count++;
            }
            return count;
        }

        private void BuildValids() {
            _valids = new Dictionary<int, HashSet<string>>();
            _valids.Add(_tiers[0][0].Id, new HashSet<string>());
            _valids.Add(_tiers[0][1].Id, new HashSet<string>());
            _valids[_tiers[0][0].Id].Add(_tiers[0][0].Value);
            _valids[_tiers[0][1].Id].Add(_tiers[0][1].Value);
            for (int tier = 1; tier < _tiers.Count; tier++) {
                foreach (var rule in _tiers[tier]) {
                    var hash = new HashSet<string>();
                    foreach (var text in BuildValids(rule.Side1)) {
                        hash.Add(text);
                    }
                    if (rule.Side2.Count > 0) {
                        foreach (var text in BuildValids(rule.Side2)) {
                            hash.Add(text);
                        }
                    }
                    _valids.Add(rule.Id, hash);
                }
            }
        }

        private IEnumerable<string> BuildValids(List<int> side) {
            if (side.Count == 1) {
                return new List<string>(_valids[side[0]]);
            } else {
                var join = 
                    from a in _valids[side[0]]
                    from b in _valids[side[1]]
                    select a + b;
                return join;
            }
        }

        private void SetTiers() {
            _tiers = new List<List<Rule>>();
            var rules = new List<Rule>(_rules.Values);
            var hash = new Dictionary<int, int>();
            List<Rule> currentTier = null;
            do {
                currentTier = new List<Rule>();
                for (int index = 0; index < rules.Count; index++) {
                    var rule = rules[index];
                    bool toAdd = true;
                    if (!rule.IsValue) {
                        foreach (var prior in rule.Side1.Union(rule.Side2)) {
                            if (!hash.ContainsKey(prior) || hash[prior] == _tiers.Count) {
                                toAdd = false;
                                break;
                            }
                        }
                    }
                    if (toAdd) {
                        currentTier.Add(rule);
                        hash.Add(rule.Id, _tiers.Count);
                        rules.RemoveAt(index);
                        index--;
                    }
                }
                _tiers.Add(currentTier);
            } while (rules.Count > 0);
        }

        private void SetMaxLength() {
            _maxLength = _messages.Select(x => x.Length).Max();
        }

        private void GetRulesAndMessages(List<string> input) {
            _rules = new Dictionary<int, Rule>();
            _messages = new List<string>();
            string line = "";
            int index = 0;
            do {
                line = input[index];
                if (line == "") {
                    break;
                }
                var rule = new Rule() { Side1 = new List<int>(), Side2 = new List<int>() };
                var split = line.Split(' ');
                rule.Id = Convert.ToInt32(split[0].Replace(":", ""));
                var side = rule.Side1;
                for (int s = 1; s < split.Length; s++) {
                    if (split[s] == "|") {
                        side = rule.Side2;
                    } else if (split[s] == "\"a\"") {
                        rule.IsValue = true;
                        rule.Value = "a";
                    } else if (split[s] == "\"b\"") {
                        rule.IsValue = true;
                        rule.Value = "b";
                    } else {
                        side.Add(Convert.ToInt32(split[s]));
                    }
                }
                _rules.Add(rule.Id, rule);
                index++;
            } while (true);
            index++;
            for (; index < input.Count; index++) {
                _messages.Add(input[index]);
            }
        }

        private class Rule {
            public int Id { get; set; }
            public List<int> Side1 { get; set; }
            public List<int> Side2 { get; set; }
            public string Value { get; set; }
            public bool IsValue { get; set; }
        }
    }
}
