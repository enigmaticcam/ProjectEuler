using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem04 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 4";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var pairs = GetPairs(input);
            int count = 0;
            foreach (var pair in pairs) {
                if (DoesFullContain(pair))
                    count++;
            }
            return count;
        }

        private int Answer2(List<string> input) {
            var pairs = GetPairs(input);
            int count = 0;
            foreach (var pair in pairs) {
                if (DoesOverlap(pair))
                    count++;
            }
            return count;
        }

        private bool DoesOverlap(Range[] pair) {
            return pair[0].Start <= pair[1].End && pair[0].End >= pair[1].Start;
        }

        private bool DoesFullContain(Range[] pair) {
            if (pair[0].Start <= pair[1].Start && pair[0].End >= pair[1].End) {
                return true;
            } else if (pair[1].Start <= pair[0].Start && pair[1].End >= pair[0].End) {
                return true;
            }
            return false;
        }

        private List<Range[]> GetPairs(List<string> input) {
            return input.Select(line => {
                var split = line.Split(',');
                return new Range[] {
                    GetRange(split[0]),
                    GetRange(split[1])
                };
            }).ToList();
        }

        private Range GetRange(string text) {
            var split = text.Split('-');
            return new Range() {
                Start = Convert.ToInt32(split[0]),
                End = Convert.ToInt32(split[1])
            };
        }

        private class Range {
            public int Start { get; set; }
            public int End { get; set; }
        }
    }
}
