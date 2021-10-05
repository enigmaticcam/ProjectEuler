using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem200 : ProblemBase {
        /*
            Not quite the fastest, runs in about 2.5 minutes. I read in the clarifications forum that you don't need
            more than the primes under 1 million. First I look for all squbes using primes less than 1 million where
            the sqube is below 1 million squared. And for each of those, I test if it contains 200, and if it does,
            add it to a list. That's 99.99% of the work. After that, it's just a matter of sorting the list in
            ascending order, and looking for prime proof numbers until I find 200 of them.
         */

        public override string ProblemName {
            get { return "200: Find the 200th prime-proof sqube containing the contiguous sub-string \"200\""; }
        }

        public override string GetAnswer() {
            return Solve2().ToString();
        }

        private PrimeSieve _primes;
        private ulong Solve2() {
            _primes = new PrimeSieve(1000000);
            LookFor200Squbes();
            return LookForPrimeProofs();
        }

        private List<ulong> _200s = new List<ulong>();
        private void LookFor200Squbes() {
            ulong max = _primes.Enumerate.Last() * _primes.Enumerate.Last();
            for (int index1 = 1; index1 < _primes.Count; index1++) {
                var prime1 = _primes[index1];
                if (prime1 * prime1 > max) {
                    break;
                }
                for (int index2 = 0; index2 < index1; index2++) {
                    var prime2 = _primes[index2];
                    if (prime2 * prime2 > max) {
                        break;
                    }
                    var num1 = prime1 * prime1 * prime1 * prime2 * prime2;
                    var num2 = prime1 * prime1 * prime2 * prime2 * prime2;
                    if (num1 <= max && Has200(num1)) {
                        _200s.Add(num1);
                    }
                    if (num2 <= max && Has200(num2)) {
                        _200s.Add(num2);
                    }
                }
            }
        }

        private ulong LookForPrimeProofs() {
            _200s = _200s.OrderBy(x => x).ToList();
            var final = new List<ulong>();
            foreach (var num in _200s) {
                if (IsPrimeProof(num)) {
                    final.Add(num);
                }
            }
            if (final.Count >= 200) {
                return final.ElementAt(199);
            }
            return 0;
        }

        private bool Has200(ulong num) {
            while (num >= 200) {
                if (num % 1000 == 200) {
                    return true;
                }
                num /= 10;
            }
            return false;
        }

        private bool IsPrimeProof(ulong num) {
            var sub = num;
            ulong tenPower = 1;
            do {
                var digit = sub % 10;
                for (ulong next = 1; next <= 9; next++) {
                    if (next != digit) {
                        var temp = num + ((next - digit) * tenPower);
                        var maxprime = (ulong)Math.Sqrt(temp);
                        bool isPrime = true;
                        foreach (var prime in _primes.Enumerate) {
                            if (prime >= maxprime) {
                                break;
                            }
                            if (temp % prime == 0) {
                                isPrime = false;
                                break;
                            }
                        }
                        if (isPrime) {
                            return false;
                        }
                    }
                }
                sub /= 10;
                tenPower *= 10;
            } while (sub > 0);
            return true;
        }

    }
}
