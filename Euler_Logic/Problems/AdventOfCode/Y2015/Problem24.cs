using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem24 : AdventOfCodeBase {
        private PowerAll _powerOf2;
        private List<ulong> _weights;
        private List<Set> _singles;
        private ulong _setWeight;
        private ulong _best;

        public override string ProblemName => "Advent of Code 2015: 24";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            _powerOf2 = new PowerAll(2);
            _singles = new List<Set>();
            GetWeights(input, 3);
            FindSingleGroup(0, 0, _setWeight, 0, 1, 0);
            return FindBestOfSingles(3);
        }

        private ulong Answer2(List<string> input) {
            _powerOf2 = new PowerAll(2);
            _singles = new List<Set>();
            GetWeights(input, 4);
            FindSingleGroup(0, 0, _setWeight, 0, 1, 0);
            return FindBestOfSingles(4);
        }

        private void GetWeights(List<string> input, ulong groupCount) {
            _weights = input.Select(x => Convert.ToUInt64(x)).ToList();
            ulong sum = 0;
            _weights.ForEach(x => sum += x);
            _setWeight = sum / groupCount;
        }

        private void FindSingleGroup(ulong currentWeight, int currentIndex, ulong maxWeight, ulong bits, ulong product, int count) {
            for (int index = currentIndex; index < _weights.Count; index++) {
                var nextWeight = _weights[index] + currentWeight;
                if (nextWeight > maxWeight) {
                    break;
                }
                var bit = _powerOf2.GetPower(index);
                if (nextWeight == maxWeight) {
                    _singles.Add(new Set() {
                        Bits = bits + bit,
                        Count = count + 1,
                        Product = product * _weights[index]
                    });
                } else {
                    FindSingleGroup(nextWeight, index + 1, maxWeight, bits + bit, product * _weights[index], count + 1);
                }
            }
        }

        private ulong FindBestOfSingles(int groupCount) {
            var order = _singles.OrderBy(x => x.Count).ThenBy(x => x.Product).ToList();
            FindBestOfSinglesRecursive(order, 0, 3, 0, true);
            return _best;
        }

        private bool FindBestOfSinglesRecursive(List<Set> order, int currentIndex, int remaining, ulong bits, bool isFirst) {
            for (int index = currentIndex; index < order.Count; index++) {
                var set = order[index];
                if (isFirst) {
                    _best = set.Product;
                }
                if ((set.Bits & bits) == 0) {
                    if (remaining == 1) {
                        return true;
                    } else {
                        var result = FindBestOfSinglesRecursive(order, index + 1, remaining - 1, set.Bits + bits, false);
                        if (result) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private class Set {
            public ulong Bits { get; set; }
            public ulong Product { get; set; }
            public int Count { get; set; }
        }
    }
}
