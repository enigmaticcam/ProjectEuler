using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem13 : AdventOfCodeBase {
        private Dictionary<string, Dictionary<string, int>> _hash;
        private PowerAll _powerOf2;

        public override string ProblemName => "Advent of Code 2015: 13";

        public override string GetAnswer() {
            _powerOf2 = new PowerAll(2);
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var happs = GetHappinesses(input);
            BuildHash(happs);
            TryAll(0, _hash.Keys.Count, new string[_hash.Keys.Count]);
            return _best;
        }

        private int Answer2(List<string> input) {
            var happs = GetHappinesses(input);
            BuildHash(happs);
            AddMe();
            TryAll(0, _hash.Keys.Count, new string[_hash.Keys.Count]);
            return _best;
        }

        private void AddMe() {
            var newOne = new Dictionary<string, int>();
            foreach (var value in _hash) {
                value.Value.Add("me", 0);
                newOne.Add(value.Key, 0);
            }
            _hash.Add("me", newOne);
        }

        private void TryAll(ulong bits, int remaining, string[] table) {
            for (int index = 0; index < _hash.Keys.Count; index++) {
                var bit = _powerOf2.GetPower(index);
                if ((bits & bit) == 0) {
                    bits += bit;
                    table[remaining - 1] = _hash.Keys.ElementAt(index);
                    if (remaining > 1) {
                        TryAll(bits, remaining - 1, table);
                    } else {
                        IsBest(table);
                    }
                    bits -= bit;
                }
            }
        }

        private int _best = int.MinValue;
        private void IsBest(string[] table) {
            int sum = 0;
            for (int index = 1; index < table.Length - 1; index++) {
                sum += _hash[table[index]][table[index + 1]];
                sum += _hash[table[index]][table[index - 1]];
            }
            sum += _hash[table[0]][table[1]];
            sum += _hash[table[0]][table.Last()];
            sum += _hash[table.Last()][table[table.Length - 2]];
            sum += _hash[table.Last()][table[0]];
            if (sum > _best) {
                _best = sum;
            }
        }

        private void BuildHash(List<Happiness> happs) {
            _hash = new Dictionary<string, Dictionary<string, int>>();
            foreach (var happ in happs) {
                if (!_hash.ContainsKey(happ.Id)) {
                    _hash.Add(happ.Id, new Dictionary<string, int>());
                };
                _hash[happ.Id].Add(happ.NextToId, happ.Happy);
            }
        }

        private List<Happiness> GetHappinesses(List<string> input) {
            return input.Select(line => {
                var happ = new Happiness();
                var split = line.Split(' ');
                happ.Id = split[0];
                happ.NextToId = split[10].Replace(".", "");
                happ.Happy = Convert.ToInt32(split[3]);
                if (split[2] == "lose") {
                    happ.Happy *= -1;
                }
                return happ;
            }).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "Alice would gain 54 happiness units by sitting next to Bob.",
                "Alice would lose 79 happiness units by sitting next to Carol.",
                "Alice would lose 2 happiness units by sitting next to David.",
                "Bob would gain 83 happiness units by sitting next to Alice.",
                "Bob would lose 7 happiness units by sitting next to Carol.",
                "Bob would lose 63 happiness units by sitting next to David.",
                "Carol would lose 62 happiness units by sitting next to Alice.",
                "Carol would gain 60 happiness units by sitting next to Bob.",
                "Carol would gain 55 happiness units by sitting next to David.",
                "David would gain 46 happiness units by sitting next to Alice.",
                "David would lose 7 happiness units by sitting next to Bob.",
                "David would gain 41 happiness units by sitting next to Carol."
            };
        }

        private class Happiness {
            public string Id { get; set; }
            public string NextToId { get; set; }
            public int Happy { get; set; }
        }
    }
}
