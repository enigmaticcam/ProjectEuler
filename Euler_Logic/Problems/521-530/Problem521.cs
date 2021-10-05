using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem521 : ProblemBase {
        private PrimeSieve _primes;

        public override string ProblemName {
            get { return "521: Smallest prime factor"; }
        }

        public override string GetAnswer() {
            return Solve2((ulong)Math.Pow(10, 12)).ToString();
        }

        private ulong _sum = 0;
        private ulong Solve2(ulong max) {
            _primes = new PrimeSieve((ulong)Math.Sqrt(max));
            IncludeExclude(true, 0, 1, 1, max);
            return _sum;
        }

        private void IncludeExclude(bool add, int startPrime, ulong lowestPrime, ulong prod, ulong max) {
            for (int index = startPrime; index < _primes.Count; index++) {
                var prime = _primes[index];
                if (max / prime >= prod) {
                    var next = (max / (prime * prod));
                    if (add) {
                        _sum += next * (lowestPrime == 1 ? prime : lowestPrime);
                    } else {
                        _sum -= next * prime;
                    }
                    IncludeExclude(!add, index + 1, (lowestPrime == 1 ? prime : lowestPrime), prime * prod, max);
                } else {
                    break;
                }
            }
        }

        private ulong Solve1(ulong max) {
            ulong sum = 0;
            var primes = new PrimeSieve((ulong)Math.Sqrt(max));
            for (ulong num = 2; num <= max; num++) {
                ulong add = num;
                foreach (var prime in primes.Enumerate) {
                    if (num % prime == 0) {
                        add = prime;
                        break;
                    }
                }
                sum = (sum + add) % 1000000000;
            }
            return sum;
        }
    }
}
