using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem15 : AdventOfCodeBase {
        private List<Ingredient> _ings;

        public override string ProblemName => "Advent of Code 2015: 15";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        private long Answer1(List<string> input) {
            GetIngredients(input);
            FindMaxes(100);
            FindBest(0, 100, new long[5]);
            return _best;
        }

        private void FindBest(int ingIndex, long remaining, long[] sums) {
            var ing = _ings[ingIndex];
            long max = Math.Min(remaining, ing.Max);
            if (ingIndex == _ings.Count - 1) {
                if (max == remaining) {
                    sums[0] += ing.Calories * remaining;
                    sums[1] += ing.Capacity * remaining;
                    sums[2] += ing.Durability * remaining;
                    sums[3] += ing.Flavor * remaining;
                    sums[4] += ing.Texture * remaining;
                    SetBestSum(sums);
                    sums[0] -= ing.Calories * remaining;
                    sums[1] -= ing.Capacity * remaining;
                    sums[2] -= ing.Durability * remaining;
                    sums[3] -= ing.Flavor * remaining;
                    sums[4] -= ing.Texture * remaining;
                }
            } else {
                for (long count = 1; count <= max; count++) {
                    sums[0] += ing.Calories * count;
                    sums[1] += ing.Capacity * count;
                    sums[2] += ing.Durability * count;
                    sums[3] += ing.Flavor * count;
                    sums[4] += ing.Texture * count;
                    FindBest(ingIndex + 1, remaining - count, sums);
                    sums[0] -= ing.Calories * count;
                    sums[1] -= ing.Capacity * count;
                    sums[2] -= ing.Durability * count;
                    sums[3] -= ing.Flavor * count;
                    sums[4] -= ing.Texture * count;
                }
            }
        }

        private long _best;
        private void SetBestSum(long[] sums) {
            if (sums[0] == 500) {
                long value = 1;
                var temp = sums[0];
                sums[0] = 1;
                foreach (var sum in sums) {
                    value *= (sum < 0 ? 0 : sum);
                }
                if (value > _best) {
                    _best = value;
                }
                sums[0] = temp;
            }
        }

        private void FindMaxes(int maxCount) {
            foreach (var ing in _ings) {
                ing.Max = long.MaxValue;
                if (ing.Capacity < 0) {
                    long highest = FindMax(maxCount, x => x.Capacity, ing);
                    if (highest < ing.Max) {
                        ing.Max = highest;
                    }
                }
                if (ing.Durability < 0) {
                    long highest = FindMax(maxCount, x => x.Durability, ing);
                    if (highest < ing.Max) {
                        ing.Max = highest;
                    }
                }
                if (ing.Flavor < 0) {
                    long highest = FindMax(maxCount, x => x.Flavor, ing);
                    if (highest < ing.Max) {
                        ing.Max = highest;
                    }
                }
                if (ing.Texture < 0) {
                    long highest = FindMax(maxCount, x => x.Texture, ing);
                    if (highest < ing.Max) {
                        ing.Max = highest;
                    }
                }
            }
        }

        private long FindMax(int maxCount, Func<Ingredient, long> property, Ingredient ing) {
            long highest = 0;
            foreach (var pair in _ings) {
                if (pair != ing && property(pair) > highest) {
                    highest = property(pair);
                }
            }
            var lcm = LCM.GetLCM(property(ing) * -1, highest);
            var limit = lcm / (property(ing) * -1);
            var max = maxCount / (limit + lcm / highest) * limit;
            return max;
        }

        private void GetIngredients(List<string> input) {
            _ings = input.Select(line => {
                var ing = new Ingredient();
                var split = line.Split(' ');
                ing.Id = split[0].Replace(":", "");
                ing.Capacity = Convert.ToInt32(split[2].Replace(",", ""));
                ing.Durability = Convert.ToInt32(split[4].Replace(",", ""));
                ing.Flavor = Convert.ToInt32(split[6].Replace(",", ""));
                ing.Texture = Convert.ToInt32(split[8].Replace(",", ""));
                ing.Calories = Convert.ToInt32(split[10].Replace(",", ""));
                return ing;
            }).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8",
                "Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3"
            };
        }

        private class Ingredient {
            public string Id { get; set; }
            public long Capacity { get; set; }
            public long Durability { get; set; }
            public long Flavor { get; set; }
            public long Texture { get; set; }
            public long Calories { get; set; }
            public long Max { get; set; }
        }
    }
}
