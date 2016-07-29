using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem357 : IProblem {
        private bool[] _notPrimes;

        public string ProblemName {
            get { return "357: Prime generating integers"; }
        }

        public string GetAnswer() {
            return Calc(100000000).ToString();
        }

        private ulong Calc(ulong max) {
            SievePrimes(max);
            ulong sum = 0;
            bool isPrimeGenerating;
            for (ulong n = 2; n <= max; n++) {
                if (!_notPrimes[n]) {
                    isPrimeGenerating = true;
                    ulong sqrt = (ulong)Math.Sqrt(n - 1);
                    for (ulong d = 1; d <= sqrt; d++) {
                        if ((n - 1) % d == 0 && _notPrimes[d + ((n - 1) / d)]) {
                            isPrimeGenerating = false;
                            break;
                        }
                    }
                    if (isPrimeGenerating) {
                        sum += n - 1;
                    }
                }
            }
            return sum;
        }

        private void SievePrimes(ulong max) {
            _notPrimes = new bool[max + 2];
            for (ulong num = 2; num <= max + 1; num++) {
                if (!_notPrimes[num]) {
                    for (ulong composite = num * 2; composite <= max + 1; composite += num) {
                        _notPrimes[composite] = true;
                    }
                }
            }
        }
    }
}
