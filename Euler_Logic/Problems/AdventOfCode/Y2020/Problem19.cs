using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem19 : AdventOfCodeBase {
        private Dictionary<string, Rule> _rules;
        private Dictionary<string, string> _overrides;
        private List<string> _messages;

        public override string ProblemName => "Advent of Code 2020: 19";

        public override string GetAnswer() {
            _overrides = new Dictionary<string, string>();
            return Answer2(Test3Input()).ToString();
        }

        private int Answer1(List<string> input) {
            BuildRulesAndMessgaes(input);
            return CoundValidRules();
        }

        private int Answer2(List<string> input) {
            _overrides.Add("8", "8: 42 | 42 8");
            _overrides.Add("11", "11: 42 31 | 42 11 31");
            BuildRulesAndMessgaes(input);
            return CoundValidRules();
        }

        private int CoundValidRules() {
            int count = 0;
            foreach (var message in _messages) {
                if (message == "babbbbaabbbbbabbbbbbaabaaabaaa") {
                    bool stop = true;
                }
                var result = _rules["0"].DoesMatch(message, 0);
                if (result.DidMatch && result.LastIndex == message.Length - 1) {
                    count++;
                }
            }
            return count;
        }

        private void BuildRulesAndMessgaes(List<string> input) {
            _rules = new Dictionary<string, Rule>();
            int lineIndex = 0;
            foreach (var line in input) {
                if (line == "") {
                    break;
                }
                var rule = new Rule();
                var split = line.Split(' ');
                if (_overrides.ContainsKey(split[0].Replace(":", ""))) {
                    split = _overrides[split[0].Replace(":", "")].Split(' ');
                }
                if (split.Length == 2) {
                    if (split[1][0] == '"') {
                        rule.DoesMatch = (text, index) => new RuleResult() { DidMatch = index < text.Length && text[index] == split[1][1], LastIndex = index };
                    } else {
                        rule.DoesMatch = (text, index) => _rules[split[1]].DoesMatch(text, index);
                    }
                } else if (split.Length == 3) {
                    rule.DoesMatch = (text, index) => {
                        var a = _rules[split[1]].DoesMatch(text, index);
                        if (a.DidMatch) {
                            var b = _rules[split[2]].DoesMatch(text, a.LastIndex + 1);
                            return b;
                        } else {
                            return new RuleResult() { DidMatch = false };
                        }
                    };
                } else {
                    bool hasPipe = false;
                    for (int index = 1; index < split.Length; index++) {
                        if (split[index] == "|") {
                            hasPipe = true;
                            break;
                        }
                    }
                    if (!hasPipe) {
                        if (split.Length == 4) {
                            rule.DoesMatch = (text, index) => {
                                var a = _rules[split[1]].DoesMatch(text, index);
                                if (a.DidMatch) {
                                    var b = _rules[split[2]].DoesMatch(text, a.LastIndex + 1);
                                    if (b.DidMatch) {
                                        return _rules[split[3]].DoesMatch(text, b.LastIndex + 1);
                                    }
                                }
                                return new RuleResult() { DidMatch = false };
                            };
                        } else {
                            throw new Exception();
                        }
                    } else if (split.Length == 4) {
                        rule.DoesMatch = (text, index) => {
                            var a = _rules[split[1]].DoesMatch(text, index);
                            if (a.DidMatch) {
                                return a;
                            }
                            var b = _rules[split[3]].DoesMatch(text, index);
                            if (b.DidMatch) {
                                return b;
                            }
                            return new RuleResult() { DidMatch = false };
                        };
                    } else if (split.Length == 5) {
                        rule.DoesMatch = (text, index) => {
                            var a = _rules[split[1]].DoesMatch(text, index);
                            if (a.DidMatch) {
                                return a;
                            }
                            //a = _rules[split[3]].DoesMatch(text, index);
                            //if (a.DidMatch) {
                            //    var b = _rules[split[4]].DoesMatch(text, a.LastIndex + 1);
                            //    if (b.DidMatch) {
                            //        return b;
                            //    }
                            //}
                            return new RuleResult() { DidMatch = false };
                        };
                    } else if (split.Length == 6) {
                        rule.DoesMatch = (text, index) => {
                            var a = _rules[split[1]].DoesMatch(text, index);
                            if (a.DidMatch) {
                                var b = _rules[split[2]].DoesMatch(text, a.LastIndex + 1);
                                if (b.DidMatch) {
                                    return b;
                                }
                            }
                            a = _rules[split[4]].DoesMatch(text, index);
                            if (a.DidMatch) {
                                var b = _rules[split[5]].DoesMatch(text, a.LastIndex + 1);
                                if (b.DidMatch) {
                                    return b;
                                }
                            }
                            return new RuleResult() { DidMatch = false };
                        };
                    } else if (split.Length == 7) {
                        rule.DoesMatch = (text, index) => {
                            var a = _rules[split[1]].DoesMatch(text, index);
                            if (a.DidMatch) {
                                var b = _rules[split[2]].DoesMatch(text, a.LastIndex + 1);
                                if (b.DidMatch) {
                                    return b;
                                }
                            } else {
                                return new RuleResult() { DidMatch = false };
                            }
                            a = _rules[split[4]].DoesMatch(text, index);
                            if (a.DidMatch) {
                                var b = _rules[split[5]].DoesMatch(text, a.LastIndex + 1);
                                if (b.DidMatch) {
                                    var c = _rules[split[6]].DoesMatch(text, b.LastIndex + 1);
                                    if (c.DidMatch) {
                                        return c;
                                    }
                                }
                            }
                            return new RuleResult() { DidMatch = false };
                        };
                    } else {
                        throw new Exception();
                    }
                }
                _rules.Add(split[0].Replace(":", ""), rule);
                lineIndex++;
            }
            _messages = input.Skip(lineIndex + 1).ToList();
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "0: 1 2",
                "1: \"a\"",
                "2: 1 3 | 3 1",
                "3: \"b\"",
                "",
                "aab",
                "aba"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "0: 4 1 5",
                "1: 2 3 | 3 2",
                "2: 4 4 | 5 5",
                "3: 4 5 | 5 4",
                "4: \"a\"",
                "5: \"b\"",
                "",
                "ababbb",
                "bababa",
                "abbbab",
                "aaabbb",
                "aaaabbb"
            };
        }

        private List<string> Test3Input() {
            return new List<string>() {
                "42: 9 14 | 10 1",
                "9: 14 27 | 1 26",
                "10: 23 14 | 28 1",
                "1: \"a\"",
                "11: 42 31",
                "5: 1 14 | 15 1",
                "19: 14 1 | 14 14",
                "12: 24 14 | 19 1",
                "16: 15 1 | 14 14",
                "31: 14 17 | 1 13",
                "6: 14 14 | 1 14",
                "2: 1 24 | 14 4",
                "0: 8 11",
                "13: 14 3 | 1 12",
                "15: 1 | 14",
                "17: 14 2 | 1 7",
                "23: 25 1 | 22 14",
                "28: 16 1",
                "4: 1 1",
                "20: 14 14 | 1 15",
                "3: 5 14 | 16 1",
                "27: 1 6 | 14 18",
                "14: \"b\"",
                "21: 14 1 | 1 14",
                "25: 1 1 | 1 14",
                "22: 14 14",
                "8: 42",
                "26: 14 22 | 1 20",
                "18: 15 15",
                "7: 14 5 | 1 21",
                "24: 14 1",
                "",
                "abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa",
                "bbabbbbaabaabba",
                "babbbbaabbbbbabbbbbbaabaaabaaa",
                "aaabbbbbbaaaabaababaabababbabaaabbababababaaa",
                "bbbbbbbaaaabbbbaaabbabaaa",
                "bbbababbbbaaaaaaaabbababaaababaabab",
                "ababaaaaaabaaab",
                "ababaaaaabbbaba",
                "baabbaaaabbaaaababbaababb",
                "abbbbabbbbaaaababbbbbbaaaababb",
                "aaaaabbaabaaaaababaa",
                "aaaabbaaaabbaaa",
                "aaaabbaabbaaaaaaabbbabbbaaabbaabaaa",
                "babaaabbbaaabaababbaabababaaab",
                "aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba"
            };
        }

        private class Rule {
            public Func<string, int, RuleResult> DoesMatch { get; set; }
        }

        private class RuleResult {
            public bool DidMatch { get; set; }
            public int LastIndex { get; set; }
        }
    }
}
