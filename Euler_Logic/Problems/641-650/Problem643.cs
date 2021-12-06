using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem643 : ProblemBase {
        public override string ProblemName {
            get { return "643: 2-Friendly"; }
        }

        public override string GetAnswer() {
            BuildTwos();
            return BruteForce(100).ToString();
            return "";
        }

        private HashSet<ulong> _twos = new HashSet<ulong>();
        private void BuildTwos() {
            ulong num = 2;
            for (ulong power = 1; power <= 63; power++) {
                _twos.Add(num);
                num *= 2;
            }
        }

        private ulong BruteForce(ulong n) {
            ulong count = 0;
            for (ulong p = 1; p <= n; p++) {
                for (ulong q = p + 1; q <= n; q++) {
                    var gcd = GCD.GetGCD(p, q);
                    if (_twos.Contains(gcd)) {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
