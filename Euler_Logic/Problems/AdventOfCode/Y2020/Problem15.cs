using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem15 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2020: 15";

        public override string GetAnswer() {
            return Answer1(Input(), 30000000).ToString();
        }

        private int Answer1(List<string> input, int turnCount) {
            var nums = input[0].Split(',').Select(num => Convert.ToInt32(num)).ToList();
            return GetNumber(nums, turnCount);
        }

        private int GetNumber(List<int> nums, int turnCount) {
            var hash = new Dictionary<int, Number>();
            for (int index = 0; index < nums.Count - 1; index++) {
                hash.Add(nums[index], new Number() {
                    IndexMinus1 = index + 1,
                    IndexMinus2 = 0,
                    JustAdded = false
                });
            }
            int lastNumber = nums[nums.Count - 1];
            hash.Add(lastNumber, new Number() { IndexMinus1 = nums.Count, JustAdded = true });
            for (int turn = nums.Count + 1; turn <= turnCount; turn++) {
                if (hash[lastNumber].JustAdded) {
                    hash[lastNumber].JustAdded = false;
                    lastNumber = 0;
                } else {
                    var number = hash[lastNumber];
                    lastNumber = number.IndexMinus1 - number.IndexMinus2;
                }
                if (!hash.ContainsKey(lastNumber)) {
                    hash.Add(lastNumber, new Number() { IndexMinus1 = turn, JustAdded = true });
                } else {
                    var number = hash[lastNumber];
                    number.IndexMinus2 = number.IndexMinus1;
                    number.IndexMinus1 = turn;
                }
            }
            return lastNumber;
        }

        private List<string> Test1Input() {
            return new List<string>() { "0,3,6" };
        }

        private class Number {
            public int IndexMinus1 { get; set; }
            public int IndexMinus2 { get; set; }
            public bool JustAdded { get; set; }
        }
    }
}
