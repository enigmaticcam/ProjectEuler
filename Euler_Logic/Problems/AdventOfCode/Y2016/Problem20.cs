using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem20 : AdventOfCodeBase {
        private List<Tuple<uint, uint>> _ranges;

        public override string ProblemName => "Advent of Code 2016: 20";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private uint Answer1(List<string> input) {
            GetRanges(input);
            return FindLowest();
        }

        private uint Answer2(List<string> input) {
            GetRanges(input);
            RemoveOverlap();
            return FindCount();
        }

        private uint FindLowest() {
            uint lowest = 0;
            bool keepGoing = false;
            do {
                keepGoing = false;
                foreach (var range in _ranges) {
                    if (lowest >= range.Item1 && lowest <= range.Item2) {
                        lowest = range.Item2 + 1;
                        keepGoing = true;
                        break;
                    }
                }
            } while (keepGoing);
            return lowest;
        }

        private uint FindCount() {
            uint count = uint.MaxValue;
            foreach (var range in _ranges) {
                count -= (range.Item2 - range.Item1) + 1;
            }
            return count + 1;
        }

        private void RemoveOverlap() {
            var overlap = FindOverlap();
            while (overlap.Item1 != -1) {
                var range1 = _ranges[overlap.Item1];
                var range2 = _ranges[overlap.Item2];
                var min = Math.Min(range1.Item1, range2.Item1);
                var max = Math.Max(range1.Item2, range2.Item2);
                _ranges.RemoveAt(Math.Max(overlap.Item1, overlap.Item2));
                _ranges.RemoveAt(Math.Min(overlap.Item1, overlap.Item2));
                _ranges.Add(new Tuple<uint, uint>(min, max));
                overlap = FindOverlap();
            }
        }

        private Tuple<int, int> FindOverlap() {
            for (int index1 = 0; index1 < _ranges.Count; index1++) {
                var range1 = _ranges[index1];
                for (int index2 = index1 + 1; index2 < _ranges.Count; index2++) {
                    var range2 = _ranges[index2];
                    if (range1.Item1 <= range2.Item2 && range1.Item2 >= range2.Item1) {
                        return new Tuple<int, int>(index1, index2);
                    }
                }
            }
            return new Tuple<int, int>(-1, -1);
        }

        private void GetRanges(List<string> input) {
            _ranges = input.Select(line => {
                var split = line.Split('-');
                return new Tuple<uint, uint>(Convert.ToUInt32(split[0]), Convert.ToUInt32(split[1]));
            }).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "5-8",
                "0-2",
                "4-7"
            };
        }
    }
}
