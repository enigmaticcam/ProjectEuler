using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem16 : AdventOfCodeBase {
        private List<Dictionary<string, int>> _aunts;
        private Dictionary<string, int> _lookingFor;

        public override string ProblemName => "Advent of Code 2015: 16";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetAunts(input);
            GetLookingFor();
            return Reduce();
        }

        private int Reduce() {
            var reduced = Enumerable.Range(0, _aunts.Count - 1).ToList();
            foreach (var looking in _lookingFor) {
                var newReduced = new List<int>();
                foreach (var index in reduced) {
                    var aunt = _aunts[index];
                    if (!aunt.ContainsKey(looking.Key)) {
                        newReduced.Add(index);
                    } else {
                        if (looking.Key == "cats" || looking.Key == "trees") {
                            if (aunt[looking.Key] > looking.Value) {
                                newReduced.Add(index);
                            }
                        } else if (looking.Key == "pomeranians" || looking.Key == "goldfish") {
                            if (aunt[looking.Key] < looking.Value) {
                                newReduced.Add(index);
                            }
                        } else if (aunt[looking.Key] == looking.Value) {

                            newReduced.Add(index);
                        }
                    }
                }
                reduced = newReduced;
            }
            return reduced[0] + 1;
        }

        private void GetAunts(List<string> input) {
            _aunts = input.Select(line => {
                var keyValues = new Dictionary<string, int>();
                var split = line.Split(' ');
                int index = 2;
                while (index < split.Length) {
                    var key = split[index].Replace(":", "");
                    var value = split[index + 1].Replace(",", "");
                    keyValues.Add(key, Convert.ToInt32(value));
                    index += 2;
                }
                return keyValues;
            }).ToList();
        }

        private void GetLookingFor() {
            var hash = new Dictionary<string, int>();
            foreach (var line in LookingFor()) {
                var split = line.Split(' ');
                hash.Add(split[0].Replace(":", ""), Convert.ToInt32(split[1]));
            }
            _lookingFor = hash;
        }

        private List<string> LookingFor() {
            return new List<string>() {
                "children: 3",
                "cats: 7",
                "samoyeds: 2",
                "pomeranians: 3",
                "akitas: 0",
                "vizslas: 0",
                "goldfish: 5",
                "trees: 3",
                "cars: 2",
                "perfumes: 1"
            };
        }
    }
}
