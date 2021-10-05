using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem396 : ProblemBase {
        public override string ProblemName {
            get { return "396: Weak Goodstein sequence"; }
        }

        public override string GetAnswer() {
            return "";
        }

        private ulong Solve1(ulong maxN) {
            ulong sum = 0;
            for (ulong n = 1; n < maxN; n++) {
                var result = GetCount(n);
                sum = (sum + result) % 1000000000;
            }
            return sum;
        }

        private ulong GetCount(ulong n) {
            ulong v = n;
            ulong b = 2;
            ulong count = 0;
            do {
                v = DecToNextBase(v, b);
                b++;
                count++;
            } while (v != 0);
            return count;
        }

        private ulong DecToNextBase(ulong n, ulong b) {
            ulong result = 0;
            ulong maxRoot = (ulong)Math.Log(n, b);
            ulong powerB = (ulong)Math.Pow(b, maxRoot);
            ulong powerBPlus1 = (ulong)Math.Pow(b + 1, maxRoot);
            for (ulong root = maxRoot; root > 0; root--) {
                var remainder = n / powerB;
                if (remainder > 0) {
                    result += remainder * powerBPlus1;
                    n -= remainder * powerB;
                }
                powerB /= b;
                powerBPlus1 /= b + 1;
            }
            result += n;
            return result - 1;
        }

        private ulong BaseToDec(ulong n, ulong b) {
            return 0;
        }
    }
}
