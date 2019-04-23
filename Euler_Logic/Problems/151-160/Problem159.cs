using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem159 : ProblemBase {
        private PrimeSieve _primes;
        private Dictionary<ulong, ulong> _maxRootSum = new Dictionary<ulong, ulong>();
        private Dictionary<ulong, ulong> _rootSums = new Dictionary<ulong, ulong>();

        /*
            mdrs(n) can be found by looping through each divisor of (n), and for each
            divisor (d), calculating x = mdrs(d) + mdrs(n/d). Whichever (d) produces
            the highest of (x) is the answer for (n).

            Some optimizations can be made. The function that calculates the
            digital root sum of a number, as well as mdrs(n), can both save their 
            results in a dictionary. Also, not every divisor of (n) needs to be found. 
            Rather, only the divisors up to sqrt(n). Also, if (n) is a prime number,
            then the mdrs(n) equals the digital root sum of (n).
         */

        public override string ProblemName {
            get { return "159: Digital root sums of factorisations"; }
        }

        public override string GetAnswer() {
            return Solve(1000000).ToString();
        }

        private ulong Solve(ulong maxN) {
            ulong sum = 0;
            _primes = new PrimeSieve(maxN);
            for (ulong n = 2; n < maxN; n++) {
                sum += GetMaxDigitalRootSum(n);
            }
            return sum;
        }

        private ulong GetMaxDigitalRootSum(ulong n) {
            if (!_maxRootSum.ContainsKey(n)) {
                var bestRootSum = DigitalRootSum(n);
                if (!_primes.IsPrime(n)) {
                    var maxD = (ulong)Math.Sqrt(n);
                    for (ulong d = 2; d <= maxD; d++) {
                        if (n % d == 0) {
                            var next = GetMaxDigitalRootSum(n / d);
                            var dRootSum = DigitalRootSum(d);
                            if (dRootSum + next > bestRootSum) {
                                bestRootSum = dRootSum + next;
                            }
                        }
                    }
                }
                _maxRootSum.Add(n, bestRootSum);
            }
            return _maxRootSum[n];
        }

        private ulong DigitalRootSum(ulong n) {
            if (!_rootSums.ContainsKey(n)) {
                ulong sum = 0;
                ulong remainder = n;
                do {
                    do {
                        sum += remainder % 10;
                        remainder /= 10;
                    } while (remainder > 0);
                    if (sum < 10) {
                        _rootSums.Add(n, sum);
                        break;
                    }
                    remainder = sum;
                    sum = 0;
                } while (true);
            }
            return _rootSums[n];
        }
    }
}