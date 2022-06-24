using Euler_Logic.Helpers;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem108 : ProblemBase {

        /*
            For any number n, the distinct solutions for x and y is the sum of the number of coprime pairs of all divisors of n.
         */

        public override string ProblemName {
            get { return "108: Diophantine reciprocals I"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private ulong Solve() {
            ulong b = 2;
            int best = 0;
            do {
                var count = GetCount(b);
                if (count > best) {
                    best = count;
                }
                if (GetCount(b) >= 1000) {
                    return b;
                }
                b++;
            } while (true);
        }

        private int GetCount(ulong num) {
            int count = 0;
            List<ulong> divisors = new List<ulong>();
            for (ulong div = 2; div <= num / 2; div++) {
                if (num % div == 0) {
                    divisors.Add(div);
                    foreach (ulong next in divisors) {
                        if (GCDULong.GCD(next, div) == 1) {
                            count++;
                        }
                    }
                }
            }
            count += divisors.Count + 2;
            return count;
        }

    }
}
