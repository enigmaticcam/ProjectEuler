using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem3 : ProblemBase {
        private Dictionary<ulong, bool> _primes = new Dictionary<ulong, bool>();

        public override string ProblemName {
            get { return "3: Largest prime factor"; }
        }

        public override string GetAnswer() {
            return LargestPrimeFactor(600851475143).ToString(); ;
        }

        private ulong LargestPrimeFactor(ulong num) {
            ulong largest = 0;
            for (ulong factor = 2; factor <= Math.Sqrt((double)num); factor++) {
                if (num % factor == 0 && IsPrime(factor)) {
                    largest = factor;
                }
            }
            return largest;
        }

        private bool IsPrime(ulong num) {
            if (_primes.ContainsKey(num)) {
                return _primes[num];
            } else {
                for (ulong factor = 2; factor <= Math.Sqrt((double)num); factor++) {
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
