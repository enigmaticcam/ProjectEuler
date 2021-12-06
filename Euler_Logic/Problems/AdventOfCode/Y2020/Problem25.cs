using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem25 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2020: 25";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            var nums = input.Select(num => Convert.ToUInt64(num)).ToList();
            var size1 = FindLoopSize(nums[0]);
            var size2 = FindLoopSize(nums[1]);
            if (size1 < size2) {
                return Transform(nums[1], size1);
            } else {
                return Transform(nums[0], size2);
            }
        }

        private int FindLoopSize(ulong target) {
            ulong subject = 7;
            ulong num = 1;
            ulong mod = 20201227;
            int size = 0;
            while (num != target) {
                num *= subject;
                num %= mod;
                size++;
            }
            return size;
        }

        private ulong Transform(ulong subject, int size) {
            ulong num = 1;
            ulong mod = 20201227;
            for (int count = 1; count <= size; count++) {
                num *= subject;
                num %= mod;
            }
            return num;
        }

        private List<string> TestInput() {
            return new List<string>() {
                "5764801",
                "17807724"
            };
        }
    }
}
