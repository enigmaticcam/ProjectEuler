using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem14 : AdventOfCodeBase {
        private List<Reaction> _reactions;
        private List<ulong> _bucket;
        private Dictionary<string, int> _hash;
        private int _oreIndex;
        private int _fuelIndex;

        public override string ProblemName {
            get { return "Advent of Code 2019: 14"; }
        }

        public override string GetAnswer() {
            _reactions = new List<Reaction>();
            _bucket = new List<ulong>();
            _hash = new Dictionary<string, int>();
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            _reactions = new List<Reaction>();
            _bucket = new List<ulong>();
            _hash = new Dictionary<string, int>();
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            BuildReactions(input);
            return CountOre(_fuelIndex, 1);
        }

        private ulong Answer2(List<string> input) {
            BuildReactions(input);
            var highest = FindFirstMillion(1000000000000);
            return FindAnswer(1000000000000, highest);
        }

        private ulong FindAnswer(ulong lookingFor, ulong highest) {
            ulong amount = 500000;
            highest -= amount;
            ulong result;
            do {
                amount /= 2;
                ResetBucket();
                result = CountOre(_fuelIndex, highest);
                if (result > lookingFor) {
                    highest -= amount;
                } else {
                    highest += amount;
                }
                if (amount == 1) break;
            } while (true);
            ResetBucket();
            result = CountOre(_fuelIndex, highest);
            if (result > lookingFor) {
                do {
                    highest--;
                    ResetBucket();
                    result = CountOre(_fuelIndex, highest);
                    if (result < lookingFor) return highest;
                } while (true);
            } else {
                do {
                    highest++;
                    ResetBucket();
                    result = CountOre(_fuelIndex, highest);
                    if (result > lookingFor) return highest - 1;
                } while (true);
            }
        }

        private ulong FindFirstMillion(ulong lookingFor) {
            ulong count = 1000000;
            do {
                ResetBucket();
                var ore = CountOre(_fuelIndex, count);
                if (ore >= lookingFor) return count;
                count += 1000000;
            } while (true);
        }

        private void ResetBucket() {
            foreach (var reaction in _reactions) {
                _bucket[reaction.Index] = 0;
            }
        }

        private ulong CountOre(int chemical, ulong count) {
            if (chemical == _oreIndex) {
                return count;
            } else {
                var reaction = _reactions[chemical];
                if (_bucket[chemical] > count) {
                    _bucket[chemical] -= count;
                    count = 0;
                } else {
                    count -= _bucket[chemical];
                    _bucket[chemical] = 0;
                }
                var remaining = count / reaction.Count;
                if ((count % reaction.Count) != 0) {
                    remaining++;
                    _bucket[chemical] += (remaining * reaction.Count) - count;
                }
                ulong sum = 0;
                foreach (var ingredient in reaction.Ingredients) {
                    sum += CountOre(ingredient.Index, remaining * ingredient.Count);
                }
                return sum;
            }
        }

        private void BuildReactions(List<string> input) {
            int space;
            foreach (var reaction in input) {
                var middle = reaction.IndexOf("=>");
                var left = reaction.Substring(0, middle - 1).Split(',');
                var right = reaction.Substring(middle + 2, reaction.Length - middle - 2).Trim();
                space = right.IndexOf(' ');
                var name = right.Substring(space, right.Length - space).Trim();
                var next = _reactions[GetHashIndex(name)];
                next.Count = Convert.ToUInt64(right.Substring(0, space));
                next.Chemical = name;
                foreach (var ingredient in left) {
                    var toAdd = new Ingredient();
                    var trimmed = ingredient.Trim();
                    space = trimmed.IndexOf(' ');
                    toAdd.Count = Convert.ToUInt64(trimmed.Substring(0, space));
                    toAdd.Chemical = trimmed.Substring(space, trimmed.Length - space).Trim();
                    toAdd.Index = GetHashIndex(toAdd.Chemical);
                    next.Ingredients.Add(toAdd);
                }
            }
        }

        private int GetHashIndex(string chemical) {
            if (!_hash.ContainsKey(chemical)) {
                _hash.Add(chemical, _reactions.Count);
                var reaction = new Reaction() {
                    Chemical = chemical,
                    Index = _reactions.Count,
                    Ingredients = new List<Ingredient>()
                };
                _reactions.Add(reaction);
                _bucket.Add(0);
                if (chemical == "ORE") {
                    _oreIndex = reaction.Index;
                    reaction.Count = 1;
                }
                if (chemical == "FUEL") _fuelIndex = reaction.Index;
            }
            return _hash[chemical];
        }

        private class Reaction {
            public string Chemical { get; set; }
            public ulong Count { get; set; }
            public List<Ingredient> Ingredients { get; set; }
            public int Index { get; set; }
        }

        private class Ingredient {
            public string Chemical { get; set; }
            public ulong Count { get; set; }
            public int Index { get; set; }
        }
    }
}
