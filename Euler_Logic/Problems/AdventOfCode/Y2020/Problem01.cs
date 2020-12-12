using System;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem01 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2020: 1";

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        private int Answer1() {
            var nums = GetNums();
            for (int index1 = 0; index1 < nums.Length; index1++) {
                for (int index2 = index1 + 1; index2 < nums.Length; index2++) {
                    if (index1 != index2 && nums[index1] + nums[index2] == 2020) {
                        return nums[index1] * nums[index2];
                    }
                }
            }
            return 0;
        }

        private int Answer2() {
            var nums = GetNums();
            for (int index1 = 0; index1 < nums.Length; index1++) {
                for (int index2 = index1 + 1; index2 < nums.Length; index2++) {
                    if (index1 != index2) {
                        for (int index3 = index2 + 1; index3 < nums.Length; index3++) {
                            if (index3 != index1 && index3 != index2 && nums[index1] + nums[index2] + nums[index3] == 2020) {
                                return nums[index1] * nums[index2] * nums[index3];
                            }
                        }
                    }
                }
            }
            return 0;
        }

        private int[] GetNums() {
            return Input().Select(num => Convert.ToInt32(num)).ToArray();
        }
    }
}
