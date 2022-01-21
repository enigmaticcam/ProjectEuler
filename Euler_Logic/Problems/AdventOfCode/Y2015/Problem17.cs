using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem17 : AdventOfCodeBase {
        private int _maxSize = 150;

        public override string ProblemName => "Advent of Code 2015: 17";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var nums = GetNums(input);
            return Solve(nums, _maxSize);
        }

        private int Answer2(List<string> input) {
            return TryAll(input);
        }

        private int TryAll(List<string> input) {
            int count = 0;
            var nums = GetNums(input);
            for (int a = 0; a < nums.Count; a++) {
                for (int b = a + 1; b < nums.Count; b++) {
                    for (int c = b + 1; c < nums.Count; c++) {
                        for (int d = c + 1; d < nums.Count; d++) {
                            if (nums[a] + nums[b] + nums[c] + nums[d] == 150) {
                                count++;
                            }
                        }
                    }
                }
            }
            return count;
        }

        private int Solve(List<int> sizes, int maxSize) {
            var sums = new int[maxSize];
            for (int size = 0; size < sizes.Count; size++) {
                var newSums = new int[maxSize];
                for (int max = 1; max <= maxSize; max++) {
                    int weight = 0;
                    var stop = Math.Min(max, sizes[size]);
                    while (weight <= stop) {
                        if (weight == max) {
                            newSums[max - 1] += 1;
                        } else {
                            newSums[max - 1] += sums[max - weight - 1];
                        }
                        weight += sizes[size];
                    }
                }
                sums = newSums;
            }
            return sums[maxSize - 1];
        }

        private List<int> GetNums(List<string> input) {
            return input.Select(x => Convert.ToInt32(x)).ToList();
        }
    }
}
