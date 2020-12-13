using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem10 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2020: 10";

        public override string GetAnswer() {
            var nums = GetNums(Input());
            return Answer2(nums).ToString();
        }

        private ulong Answer1(List<ulong> nums) {
            var counts = new Dictionary<ulong, ulong>();
            counts.Add(1, 0);
            counts.Add(3, 1);
            counts[nums[0]]++;
            for (int index = 1; index < nums.Count; index++) {
                counts[nums[index] - nums[index - 1]]++;
            }
            return counts[1] * counts[3];
        }

        private ulong Answer2(List<ulong> nums) {
            nums.Insert(0, 0);
            var count = new ulong[nums.Count];
            count[nums.Count - 1] = 1;
            for (int index = nums.Count - 1; index >= 0; index--) {
                for (int next = index - 1; next >= 0; next--) {
                    if (nums[index] - nums[next] <= 3) {
                        count[next] += count[index];
                    }
                }
            }
            return count[0];
        }

        private List<ulong> GetNums(List<string> input) {
            return input.Select(num => Convert.ToUInt64(num)).OrderBy(num => num).ToList();
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "16",
                "10",
                "15",
                "5",
                "1",
                "11",
                "7",
                "19",
                "6",
                "12",
                "4"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "1",
                "2",
                "3",
                "4",
                "7",
                "8",
                "9",
                "10",
                "11",
                "14",
                "17",
                "18",
                "19",
                "20",
                "23",
                "24",
                "25",
                "28",
                "31",
                "32",
                "33",
                "34",
                "35",
                "38",
                "39",
                "42",
                "45",
                "46",
                "47",
                "48",
                "49"
            };
        }
    }
}
