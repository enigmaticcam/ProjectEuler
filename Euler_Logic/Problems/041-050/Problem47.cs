using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem47 : ProblemBase {
        private PrimeSieve _primes;

        /*
            Assume the answer is <= 1000000. Create an array of integers up to 1000000. each array will represent the number
            of distinct factors for that index. Find all primes up to sqrt(1000000). For each prime, add one to every composite.
            Then loop through the array and find the first ocurrance were the value of 4 consecutive numbers are eqaul to 4.
         */

        public override string ProblemName {
            get { return "47: Distinct primes factors"; }
        }

        public override string GetAnswer() {
            return Solve2().ToString();
        }

        private int[] _counts;
        private int Solve2() {
            _primes = new PrimeSieve((ulong)Math.Sqrt(1000000));
            _counts = new int[1000001];
            foreach (var prime in _primes.Enumerate) {
                for (ulong composite = 2; composite * prime <= 1000000; composite++) {
                    _counts[composite * prime]++;
                }
            }
            int consecutiveCount = 0;
            for (int num = 2 * 3 * 5 * 7; num <= 1000000; num++) {
                if (_counts[num] == 4) {
                    consecutiveCount++;
                    if (consecutiveCount == 4) {
                        return num - 3;
                    }
                } else {
                    consecutiveCount = 0;
                }
            }
            return 0;
        }

        private ulong Solve1() {
            _primes = new PrimeSieve(1000000);
            ulong num = 2 * 3 * 5 * 7;
            int consecutiveCount = 0;
            do {
                ulong remainder = num;
                int primeCount = 0;
                foreach (var prime in _primes.Enumerate) {
                    if (remainder % prime == 0) {
                        primeCount++;
                        if (primeCount > 4) {
                            break;
                        }
                        do {
                            remainder /= prime;
                        } while (remainder % prime == 0);
                        if (prime == 1) {
                            break;
                        } else if (_primes.IsPrime(remainder)) {
                            primeCount++;
                            break;
                        }
                    }
                }
                if (primeCount == 4) {
                    consecutiveCount++;
                    if (consecutiveCount == 4) {
                        return num - 3;
                    }
                } else {
                    consecutiveCount = 0;
                }
                num++;
            } while (true);
        }
    }
}
