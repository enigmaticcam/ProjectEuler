using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem09 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2020: 9";

        public override string GetAnswer() {
            return Answer2(Input(), 25).ToString();
        }

        private ulong Answer1(List<string> input, int preamble) {
            var nums = GetNumbers(input, preamble);
            GetSums(nums, preamble);
            return FindFirstInvalid(nums, preamble);
        }

        private ulong Answer2(List<string> input, int preamble) {
            var nums = GetNumbers(input, preamble);
            GetSums(nums, preamble);
            var lookingFor = FindFirstInvalid(nums, preamble);
            return FindContiguous(nums, preamble, lookingFor);
        }

        private ulong FindContiguous(List<Number> nums, int preamble, ulong lookingFor) {
            int length = 1;
            ulong[] counts = new ulong[nums.Count];
            do {
                var nextCount = new ulong[nums.Count];
                for (int index = 0; index < nums.Count - length + 1; index++) {
                    nextCount[index] = counts[index] + nums[index + length - 1].Value;
                    if (nextCount[index] == lookingFor && length != 1) {
                        return GetSmallestPlusLargest(index, index + length - 1, nums);
                    }
                }
                counts = nextCount;
                length++;
            } while (true);
        }

        private ulong GetSmallestPlusLargest(int start, int end, List<Number> nums) {
            ulong smallest = ulong.MaxValue;
            ulong largest = 0;
            for (int index = start; index <= end; index++) {
                if (nums[index].Value < smallest) {
                    smallest = nums[index].Value;
                }
                if (nums[index].Value > largest) {
                    largest = nums[index].Value;
                }
            }
            return smallest + largest;
        }

        private ulong FindFirstInvalid(List<Number> nums, int preamble) {
            for (int index = preamble; index < nums.Count; index++) {
                bool isGood = false;
                for (int prior = 1; prior < preamble; prior++) {
                    for (int check = 1; check <= prior; check++) {
                        if (nums[index].Value == nums[index - prior - 1].Sums[check - 1]) {
                            isGood = true;
                        }
                    }
                }
                if (!isGood) {
                    return nums[index].Value;
                }
            }
            return 0;
        }

        private void GetSums(List<Number> nums, int preamble) {
            for (int index = 0; index < nums.Count - 1; index++) {
                var num = nums[index];
                for (int next = 1; next < preamble; next++) {
                    if (index + next < nums.Count) {
                        num.Sums[next - 1] = num.Value + nums[index + next].Value;
                    } else {
                        break;
                    }
                }
            }
        }

        private List<Number> GetNumbers(List<string> input, int preamble) {
            return input.Select(num => {
                var number = new Number();
                number.Value = Convert.ToUInt64(num);
                number.Sums = new ulong[preamble - 1];
                return number;
            }).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "35",
                "20",
                "15",
                "25",
                "47",
                "40",
                "62",
                "55",
                "65",
                "95",
                "102",
                "117",
                "150",
                "182",
                "127",
                "219",
                "299",
                "277",
                "309",
                "576"
            };
        }

        private class Number {
            public ulong Value { get; set; }
            public ulong[] Sums { get; set; }
        }
    }
}
