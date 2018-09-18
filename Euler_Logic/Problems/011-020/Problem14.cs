using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem14 : ProblemBase {
        private Dictionary<ulong, int> _counts = new Dictionary<ulong, int>();

        /*
            For any number x:

            If x is odd, count(x) = 1 + count(3x + 1)
            If x is even, count(x) = 1 + count(x / 2)

            Save the results in a dictionary as we loop from 2 to 1000000 so we don't recalculate numbers more than once.
         */

        public override string ProblemName {
            get { return "14: Longest Collatz sequence"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private ulong Solve() {
            _counts.Add(1, 1);
            int bestCount = 0;
            ulong bestNum = 0;
            for (ulong num = 2; num < 1000000; num++) {
                int count = GetCount(num);
                if (count > bestCount) {
                    bestCount = count;
                    bestNum = num;
                }
            }
            return bestNum;
        }

        private int GetCount(ulong num) {
            if (!_counts.ContainsKey(num)) {
                int count = 1;
                if (num % 2 == 0) {
                    count += GetCount(num / 2);
                } else {
                    count += GetCount((3 * num) + 1);
                }
                _counts.Add(num, count);
            }
            return _counts[num];
        }
    }
}