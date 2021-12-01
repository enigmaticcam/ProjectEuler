using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem37 : ProblemBase {
        private PrimeSieve _primes;

        /*
            Brute force all primes up to 1000000. For each prime, test if it's still prime after removing digits on the right
            (by dividing by 10) and on the left (by moduls powers of 10).
         */

        public override string ProblemName {
            get { return "37: Truncatable primes"; }
        }

        public override string GetAnswer() {
            _primes = new PrimeSieve(1000000);
            return Solve().ToString();
        }

        private ulong Solve() {
            ulong sum = 0;
            int count = 0;
            foreach (var prime in _primes.Enumerate) {
                if (prime >= 11) {
                    if (CanTruncateLeft(prime) && CanTruncateRight(prime)) {
                        count++;
                        sum += prime;
                        if (count == 11) {
                            break;
                        }
                    }
                }
            }
            return sum;
        }

        private bool CanTruncateLeft(ulong num) {
            ulong powerOf10 = 10;
            do {
                if (!_primes.IsPrime(num % powerOf10)) {
                    return false;
                }
                powerOf10 *= 10;
            } while (powerOf10 < num);
            return true;
        }
        
        private bool CanTruncateRight(ulong num) {
            do {
                if (!_primes.IsPrime(num)) {
                    return false;
                }
                num /= 10;
            } while (num > 0);
            return true;
        }
    }
}
