using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem686 : ProblemBase {
        /*
            Just multiply by two until we get the answer. If we go beyond 64 digits, then just divide by 10.
            When checking if a power of 2 has the matching digits, I figure the difference of log(10) between
            the two and divide by that power of 10. This is faster than dividing by 10 until the numbers are
            the same (saves 30 seconds).
         */
        private PowerAll _powerOf10;
        private int _logOfL;

        public override string ProblemName {
            get { return "686: Powers of Two"; }
        }
        
        public override string GetAnswer() {
            _powerOf10 = new PowerAll(10);
            return BruteForce(123, 678910).ToString();
        }

        private ulong BruteForce(ulong l, ulong n) {
            _logOfL = (int)Math.Log10(l);
            ulong num = 2;
            ulong count = 0;
            ulong total = 1;
            do {
                while (ulong.MaxValue / num < 2) {
                    num /= 10;
                }
                num *= 2;
                total++;
                if (IsGood(l, num)) {
                    count++;
                    if (count == n) {
                        return total;
                    }
                }
            } while (true);
        }

        private bool IsGood(ulong l, ulong num) {
            if (l > num) {
                return false;
            }
            var y = (int)Math.Log10(num);
            num /= _powerOf10.GetPower(y - _logOfL);
            return num == l;
        }
    }
}
