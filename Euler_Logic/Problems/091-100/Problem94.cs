using System;

namespace Euler_Logic.Problems {
    public class Problem94 : ProblemBase {
        private ulong _sum = 0;

        public override string ProblemName {
            get { return "94: Almost equilateral triangles"; }
        }

        public override string GetAnswer() {
            Solve(1000000000);
            return _sum.ToString();
        }

        private void Solve(ulong max) {
            ulong num = 2;
            do {
                if (TryTriangle(num - 1, num)) {
                    _sum += num * 3 - 1;
                }
                if (num * 3 + 1 <= max) {
                    if (TryTriangle(num + 1, num)) {
                        _sum += num * 3 + 1;
                    }
                }
                num += 1;
            } while (num * 3 - 1 <= max);
        }

        private bool TryTriangle(ulong oneSide, ulong twoSide) {
            ulong a = (4 * (twoSide * twoSide)) - (oneSide * oneSide);
            ulong b = (ulong)Math.Sqrt(a);
            return b * b == a;
        }
    }
}