using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem04 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 3"; }
        }

        public override string GetAnswer() {
            return Answer1().ToString();
        }

        public override string GetAnswer2() {
            return Answer2().ToString();
        }

        public int Answer1() {
            int count = 0;
            for (int num = 136760; num <= 595730; num++) {
                if (IsGood1(num)) {
                    count++;
                }
            }
            return count;
        }

        public int Answer2() {
            _digitCounts = new int[11];
            int count = 0;
            for (int num = 136760; num <= 595730; num++) {
                if (IsGood2(num)) {
                    count++;
                }
            }
            return count;
        }

        private int[] _digitCounts;
        private bool IsGood2(int num) {
            int lastDigit = 10;
            for (int index = 0; index <= 10; index++) {
                _digitCounts[index] = 0;
            }
            while (num != 0) {
                var nextDigit = num % 10;
                if (nextDigit > lastDigit) {
                    return false;
                }
                _digitCounts[nextDigit]++;
                lastDigit = nextDigit;
                num /= 10;
            }
            return _digitCounts.Where(x => x == 2).Count() > 0;
        }

        private bool IsGood1(int num) {
            bool hasDupe = false;
            int lastDigit = 10;
            while (num != 0) {
                var nextDigit = num % 10;
                if (nextDigit == lastDigit) {
                    hasDupe = true;
                }
                if (nextDigit > lastDigit) {
                    return false;
                }
                lastDigit = nextDigit;
                num /= 10;
            }
            return hasDupe;
        }
    }
}
