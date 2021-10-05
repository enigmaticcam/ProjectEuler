using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem19 : AdventOfCodeBase {
        private Dictionary<string, List<string>> _translation;
        private Dictionary<int, Dictionary<string, string>> _reverseTranslation;
        private string _molecule;
        private Dictionary<string, int> _total;

        public override string ProblemName => "Advent of Code 2015: 19";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetInput(input);
            return CountDistinct();
        }

        private int Answer2(List<string> input) {
            _total = new Dictionary<string, int>();
            GetInput(input);
            GetReverseTranslation();
            return MakeMolecule();
        }

        private int MakeMolecule() {
            var order = _reverseTranslation.Keys.OrderByDescending(x => x);
            int steps = 0;
            do {
                bool keepGoing = true;
                foreach (var num in order) {
                    foreach (var keyValue in _reverseTranslation[num]) {
                        var index = _molecule.IndexOf(keyValue.Key);
                        if (index > -1) {
                            if (keyValue.Key.Length != keyValue.Value.Length) {
                                var before = _molecule.Length;
                                _molecule = _molecule.Replace(keyValue.Key, keyValue.Value);
                                var after = _molecule.Length;
                                steps += (before - after) / (keyValue.Key.Length - keyValue.Value.Length);
                            } else {
                                do {
                                    steps++;
                                    index = _molecule.IndexOf(keyValue.Key, index + 1);
                                } while (index != -1);
                                _molecule = _molecule.Replace(keyValue.Key, keyValue.Value);
                            }
                            keepGoing = false;
                        }
                    }
                    if (!keepGoing) {
                        break;
                    }
                }
            } while (_molecule != "e");
            return steps;
        }

        private void GetReverseTranslation() {
            _reverseTranslation = new Dictionary<int, Dictionary<string, string>>();
            foreach (var keyValue in _translation) {
                foreach (var value in keyValue.Value) {
                    if (!_reverseTranslation.ContainsKey(value.Length)) {
                        _reverseTranslation.Add(value.Length, new Dictionary<string, string>());
                    }
                    _reverseTranslation[value.Length].Add(value, keyValue.Key);
                }
            }
        }

        private int CountDistinct() {
            var hash = new HashSet<string>();
            foreach (var keyValue in _translation) {
                int index = _molecule.IndexOf(keyValue.Key);
                StringBuilder final = null;
                while (index > -1) {
                    var left = _molecule.Substring(0, index);
                    var right = _molecule.Substring(index + keyValue.Key.Length);
                    foreach (var value in keyValue.Value) {
                        final = new StringBuilder();
                        final.Append(left);
                        final.Append(value);
                        final.Append(right);
                        hash.Add(final.ToString());
                    }
                    index = _molecule.IndexOf(keyValue.Key, index + 1);
                }
            }
            return hash.Count;
        }

        private void GetInput(List<string> input) {
            _translation = new Dictionary<string, List<string>>();
            foreach (var line in input) {
                if (line == "") {
                    break;
                }
                var split = line.Split(' ');
                if (!_translation.ContainsKey(split[0])) {
                    _translation.Add(split[0], new List<string>());
                }
                _translation[split[0]].Add(split[2]);
            }
            _molecule = input.Last();
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "H => HO",
                "H => OH",
                "O => HH",
                "",
                "HOH"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "H => HO",
                "H => OH",
                "O => HH",
                "",
                "HOHOHO"
            };
        }

        private List<string> Test3Input() {
            return new List<string>() {
                "e => H",
                "e => O",
                "H => HO",
                "H => OH",
                "O => HH",
                "",
                "HOH"
            };
        }

        private List<string> Test4Input() {
            return new List<string>() {
                "e => H",
                "e => O",
                "H => HO",
                "H => OH",
                "O => HH",
                "",
                "HOHOHO"
            };
        }
    }
}
