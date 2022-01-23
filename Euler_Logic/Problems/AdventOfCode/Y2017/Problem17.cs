using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem17 : AdventOfCodeBase {

        public override string ProblemName {
            get { return "Advent of Code 2017: 17"; }
        }

        public override string GetAnswer() {
            return Answer1(359).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(359).ToString();
        }

        private int Answer1(int input) {
            return BruteForce(input);
        }

        private int Answer2(int input) {
            return AfterZero(input);
        }

        private int AfterZero(int offset) {
            int numCount = 1;
            int index = 0;
            int numAfterZero = 0;
            for (int count = 1; count <= 50000000; count++) {
                index = (index + offset) % numCount;
                if (index == 0) {
                    numAfterZero = count;
                }
                index++;
                numCount++;
            }
            return numAfterZero;
        }

        private int BruteForce(int offset) {
            var nums = new List<int>() { 0 };
            int index = 0;
            for (int count = 1; count <= 2017; count++) {
                index = (index + offset) % nums.Count;
                nums.Insert(index + 1, count);
                index++;
            }
            return nums[(index + 1) % nums.Count];
        }
    }
}
