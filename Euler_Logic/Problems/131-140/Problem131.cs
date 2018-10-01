using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem131 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();

        public override string ProblemName {
            get { return "131: Prime cube partnership"; }
        }

        public override string GetAnswer() {
            _primes.SievePrimes(999999);
            return Solve(100).ToString();
        }

        private int Solve(ulong max) {
            int lastPrimeIndex = 0;
            int total = 1;
            ulong lastN = 8;
            ulong cubeRoot = 12;
            ulong cubed = 12 * 12 * 12;
            do {
                int primeIndex = lastPrimeIndex;
                bool found = false;
                if (_primes[primeIndex] > max) {
                    return total;
                }
                do {
                    ulong prime = _primes[primeIndex];
                    if (prime > max) {
                        break;
                    }
                    ulong n = lastN;
                    ulong answer = 0;
                    int count = 0;
                    do {
                        answer = (prime * n * n) + (n * n * n);
                        if (answer == cubed) {
                            lastPrimeIndex = primeIndex + 1;
                            lastN = n + 1;
                            found = true;
                            total++;
                            break;
                        }
                        count++;
                        n++;
                    } while (answer <= cubed);
                    if (answer > cubed && count == 1) {
                        break;
                    }
                    primeIndex++;
                } while (!found);
                cubeRoot++;
                cubed = cubeRoot * cubeRoot * cubeRoot;
            } while (true);
        }
    }
}
