using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem03 : AdventOfCodeBase {
        private char[] _digitsOxygen;
        private char[] _digitsCO2;

        public override string ProblemName {
            get { return "Advent of Code 2021: 03"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> nums) {
            var counts = GetCounts(nums);
            int threshold = nums.Count / 2;
            ulong gamma = 0;
            ulong epsilon = 0;
            ulong power = 1;
            for (int index = counts.Length - 1; index >= 0; index--) {
                if (counts[index] >= threshold) {
                    gamma += power;
                } else {
                    epsilon += power;
                }
                power *= 2;
            }
            return epsilon * gamma;
        }

        private ulong Answer2(List<string> nums) {
            _digitsCO2 = new char[2] { '0', '1' };
            _digitsOxygen = new char[2] { '1', '0' };
            var oxygenRating = GetRating(nums, true);
            var co2Rating = GetRating(nums, false);
            return GetUlong(oxygenRating) * GetUlong(co2Rating);
        }

        private ulong GetUlong(string num) {
            ulong result = 0;
            ulong power = 1;
            for (int index = num.Length - 1; index >= 0; index--) {
                if (num[index] == '1') {
                    result += power;
                }
                power *= 2;
            }
            return result;
        }

        private string GetRating(List<string> nums, bool isOxygen) {
            var digits = _digitsCO2;
            if (isOxygen) {
                digits = _digitsOxygen;
            }
            var current = new List<string>(nums);
            var temp = new List<string>();
            int bitIndex = 0;
            while (current.Count > 1) {
                char digit = digits[0];
                var count = GetCountsSingle(current, bitIndex);
                var remaining = current.Count - count;
                if (remaining > count) {
                    digit = digits[1];
                }
                foreach (var num in current) {
                    if (num[bitIndex] == digit) {
                        temp.Add(num);
                    }
                }
                current = temp;
                temp = new List<string>();
                bitIndex++;
            }
            return current[0];
        }

        private int GetCountsSingle(List<string> nums, int index) {
            int count = 0;
            foreach (var num in nums) {
                count += (num[index] == '1' ? 1 : 0);
            }
            return count;
        }

        private int[] GetCounts(List<string> nums) {
            var counts = new int[nums[0].Length];
            for (int index = 0; index < nums[0].Length; index++) {
                counts[index] = GetCountsSingle(nums, index);
            }
            return counts;
        }

        private List<string> Input_Test1() {
            return new List<string>() {
                "00100",
                "11110",
                "10110",
                "10111",
                "10101",
                "01111",
                "00111",
                "11100",
                "10000",
                "11001",
                "00010",
                "01010"
            };
        }
    }
}
