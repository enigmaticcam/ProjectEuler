using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem3 : ProblemBase {
        private PrimeSieve _primes;

        public override string ProblemName {
            get { return "3: Largest prime factor"; }
        }

        public override string GetAnswer() {
            return LPF(600851475143).ToString();
        }

        private ulong LPF(ulong num) {
            ulong max = (ulong)Math.Sqrt(num);
            _primes = new PrimeSieve(max);
            for (int index = _primes.Count - 1; index >= 0; index--) {
                if (num % _primes[index] == 0) {
                    return _primes[index];
                }
            }
            return 0;
        }
    }
}
