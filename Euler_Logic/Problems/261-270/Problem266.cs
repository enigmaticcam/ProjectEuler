using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem266 : ProblemBase {
        public override string ProblemName {
            get { return "266: Pseudo Square Root"; }
        }

        public override string GetAnswer() {
            return "";
        }

        private ulong Solve1(ulong maxPrime) {
            var primes = new PrimeSieve(maxPrime);
            ulong num = 1;
            foreach (var prime in primes.Enumerate) {
                num *= prime;
            }
            var root = (ulong)Math.Sqrt(num);
            for (ulong d = root; d >= 1; d--) {
                if (num % d == 0) {
                    return d;
                }
            }
            return 1;
        }
    }
}
