using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class ProblemAny : ProblemBase {
        public override string ProblemName => "Any";

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private ulong Solve() {
            var hash = new Dictionary<ulong, ulong>();
            var primes = new PrimeSieve(30000);
            ulong sum = 0;
            for (ulong num = 2; num < 10000; num++) {
                if (!hash.ContainsKey(num)) {
                    var x = DivisorSum(num, primes);
                    hash.Add(num, x);
                    ulong y = 0;
                    if (hash.ContainsKey(x)) {
                        y = hash[x];
                    } else {
                        y = DivisorSum(x, primes);
                        hash.Add(x, y);
                    }
                    if (num == y && num != x) sum += num + x;
                }
            }
            return sum;
        }

        private ulong DivisorSum(ulong num, PrimeSieve primes) {
            ulong original = num;
            ulong sum = 1;
            foreach (var prime in primes.Enumerate) {
                if (num % prime == 0) {
                    ulong sub = 1;
                    ulong power = prime;
                    do {
                        sub += power;
                        power *= prime;
                        num /= prime;
                    } while (num % prime == 0);
                    sum *= sub;
                    if (num == 1) return sum - original;
                }
            }
            return 0;
        }

    }
}
