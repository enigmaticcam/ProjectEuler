using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem659 : ProblemBase {
        public override string ProblemName {
            get { return "659: Largest prime"; }
        }

        public override string GetAnswer() {
            return BruteForce().ToString();
        }

        private ulong BruteForce() {
            var mods = new Dictionary<ulong, ulong>();
            var primes = new PrimeSieve(100000000);
            foreach (var prime in primes.Enumerate) {
                var square = (prime - 1) / 2;
                var mod = prime - ((square * square) % prime);
                if (mod <= 10000000) {
                    if (!mods.ContainsKey(mod)) {
                        mods.Add(mod, prime);
                    } else {
                        mods[mod] = prime;
                    }
                }
            }
            ulong sum = 0;
            mods.Keys.ToList().ForEach(x => {
                sum += x;
                sum %= 1000000000000000000;
            });
            return sum;
        }

        private ulong P(ulong k) {
            ulong highest = 0;
            var primes = new PrimeSieve(100000);
            foreach (var prime in primes.Enumerate) {
                for (ulong n = 1; n < prime; n++) {
                    var value1 = n * n + k;
                    var value2 = (n + 1) * (n + 1) + k;
                    if (value1 % prime == 0 && value2 % prime == 0) {
                        highest = prime;
                    }
                }
            }
            return highest;
        }
    }
}
