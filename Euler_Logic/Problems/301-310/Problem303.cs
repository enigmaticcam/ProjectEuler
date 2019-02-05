using System;

namespace Euler_Logic.Problems {
    public class Problem303 : ProblemBase {
        private int _lastBase10;
        private ulong _lastDigit;

        /*
            If a number is no good, then find the highest digit that's above 2. Add the equivalent power of 10 to make it 10 (and remove
            the remainder). Check if the result is good. If not, do it again until it is. Then, determine the next multiple of the
            number and check it.
         */

        public override string ProblemName {
            get { return "303: Multiples with small digits"; }
        }

        public override string GetAnswer() {
            return Sum(10000).ToString();
        }

        private ulong Sum(ulong max) {
            ulong sum = 0;
            for (ulong num = 1; num <= max; num++) {
                sum += FindSmallest(num) / num;
            }
            return sum;
        }

        private ulong FindSmallest(ulong num) {
            if (IsGood(num)) {
                return num;
            }
            var composite = num;
            do {
                do {
                    var base10 = (ulong)Math.Pow(10, _lastBase10);
                    composite -= composite % base10;
                    composite += (10 - _lastDigit) * base10;
                } while (!IsGood(composite));
                var remainder = composite % num;
                if (remainder != 0) {
                    composite = num - remainder + composite;
                }
                if (IsGood(composite)) {
                    return composite;
                }
            } while (true);
        }

        private bool IsGood(ulong num) {
            var base10 = 0;
            bool isGood = true;
            do {
                var remainder = num % 10;
                if (remainder > 2) {
                    _lastDigit = remainder;
                    _lastBase10 = base10;
                    isGood = false;
                }
                num /= 10;
                base10++;
            } while (num > 0);
            return isGood;
        }
    }
}