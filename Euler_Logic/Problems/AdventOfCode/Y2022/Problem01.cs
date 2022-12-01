using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem01 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 1";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private long Answer1(List<string> input) {
            var all = GetCalories(input);
            return all
                .OrderByDescending(x => x)
                .First();
        }

        private long Answer2(List<string> input) {
            var all = GetCalories(input);
            return all
                .OrderByDescending(x => x)
                .Take(3)
                .Sum();
        }

        private List<long> GetCalories(List<string> input) {
            long calories = 0;
            var all = new List<long>();
            foreach (var line in input) {
                if (line == "") {
                    all.Add(calories);
                    calories = 0;
                } else {
                    calories += Convert.ToInt64(line);
                }
            }
            all.Add(calories);
            return all;
        }
    }
}
