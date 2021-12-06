using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem01 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2021: 01"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            return GetCount(input.Select(x => Convert.ToInt32(x)));
        }

        private int Answer2(List<string> input) {
            var nums = new int[input.Count];
            for (int index = 0; index < input.Count; index++) {
                var intNum = Convert.ToInt32(input[index]);
                nums[index] = intNum;
                if (index > 0) {
                    nums[index - 1] += intNum;
                }
                if (index > 1) {
                    nums[index - 2] += intNum;
                }
            }
            return GetCount(nums);
        }

        private int GetCount(IEnumerable<int> nums) {
            int last = 0;
            bool start = true;
            int count = 0;
            foreach (var num in nums) {
                if (start) {
                    start = false;
                } else {
                    if (num > last) {
                        count++;
                    }
                }
                last = num;
            }
            return count;
        }
    }
}
