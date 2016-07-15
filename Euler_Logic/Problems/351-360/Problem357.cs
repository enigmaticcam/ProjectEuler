using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem357 : IProblem {
        private Dictionary<int, bool> _primes = new Dictionary<int, bool>();

        public string ProblemName {
            get { return "357: Prime generating integers"; }
        }

        public string GetAnswer() {
            return Calc(100000000).ToString();
        }

        private int Calc(int max) {
            int sum = 0;
            for (int n = 1; n <= max; n++) {
                bool isPrimeGenerating = true;
                for (int d = 1; d <= Math.Sqrt(n); d++) {
                    if (n % d == 0 && !IsPrime(d + (n / d))) {
                        isPrimeGenerating = false;
                        break;
                    }
                }
                if (isPrimeGenerating) {
                    sum += n;
                }
            }
            return sum;
        }

        private bool IsPrime(int num) {
            if (_primes.ContainsKey(num)) {
                return _primes[num];
            } else {
                for (int factor = 2; factor <= Math.Sqrt((double)num); factor++) {
                    if (num % factor == 0) {
                        _primes.Add(num, false);
                        return false;
                    }
                }
                _primes.Add(num, true);
                return true;
            }
        }
    }
}
