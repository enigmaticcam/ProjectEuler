using System;

namespace Euler_Logic.Problems {
    public class Problem719 : ProblemBase {
        public override string ProblemName => "719: Number Splitting";

        public override string GetAnswer() {
            return Solve((ulong)Math.Pow(10, 12)).ToString();
        }

        private ulong Solve(ulong max) {
            ulong sum = 0;
            ulong stop = (ulong)Math.Sqrt(max);
            for (ulong num = 2; num <= stop; num++) {
                if (IsGood(num * num, num)) sum += num * num;
            }
            return sum;
        }

        private bool IsGood(ulong num, ulong remaining) {
            ulong next = 0;
            ulong powerOf10 = 1;
            do {
                next += (num % 10) * powerOf10;
                num /= 10;
                if (next == remaining && num == 0) return true;
                if (next > remaining) return false;
                if (num == 0) return false;
                if (IsGood(num, remaining - next)) return true;
                powerOf10 *= 10;
            } while (true);
        }
    }
}
