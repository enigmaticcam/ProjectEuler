using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem304 : ProblemBase {
        private ulong _mod = 1234567891011;
        private bool[] _isNotPrime;
        private ulong _fibA;
        private ulong _fibB;

        /*
            Finding all the next primes is easy. I simply do a segmented prime sieve starting at 10^14. I experimented 
            with the length until I got something close but not less than 100,000 primes. The range for that is
            +4,000,000.
            
            F(n) % m is periodic. In other words, it follows a repeated sequence. The sequence starts with 0, 1. 
            So if you follow F(1) % m, F(2) % m, F(3) % m, etc. until you get 0, 1 again, then you have the 
            sequence. Once you know the sequence length (l), then F(n) % m = F(n % l) % m.

            However, the length for m = 1234567891011 is quite large, too large in fact to hold the entire sequence
            in memory. Instead, I first find the length (l), then find (n % l), and end the sequence once I get 
            there. Starting from there, I begin looking at the primes. Prime or not, I continue to find the next
            fib. But for each prime, I sum the total of the fib and do this until I've gotten all 100,000 primes.
         */

        public override string ProblemName {
            get { return "304: Primonacci"; }
        }

        public override string GetAnswer() {
            ulong start = (ulong)Math.Pow(10, 14);
            GetPrimeRange(start, 4000000);
            FindFibAAndB(start);
            return Solve().ToString();
        }

        private void GetPrimeRange(ulong start, ulong length) {
            _isNotPrime = new bool[length];
            var primes = new PrimeSieve((ulong)Math.Sqrt(start));
            foreach (var prime in primes.Enumerate) {
                var next = (start % prime == 0 ? 0 : prime - (start % prime));
                for (var composite = next; composite < length; composite += prime) {
                    _isNotPrime[composite] = true;
                }
            }
        }

        private void FindFibAAndB(ulong start) {
            var length = GetFibModPeriodLength();
            var stop = start % length;
            _fibA = 0;
            _fibB = 1;
            for (ulong next = 2; next <= stop; next++) {
                var c = (_fibA + _fibB) % _mod;
                _fibA = _fibB;
                _fibB = c;
            }
        }

        private ulong Solve() {
            int index = 0;
            int count = 0;
            ulong sum = 0;
            do {
                if (!_isNotPrime[index]) {
                    sum = (sum + _fibB) % _mod;
                    count++;
                }
                var c = (_fibA + _fibB) % _mod;
                _fibA = _fibB;
                _fibB = c;
                index++;
            } while (count < 100000);
            return sum;
        }

        private ulong GetFibModPeriodLength() {
            ulong length = 1;
            ulong a = 1;
            ulong b = 1;
            do {
                ulong c = (a + b) % _mod;
                a = b;
                b = c;
                length++;
            } while (a != 0 || b != 1);
            return length;
        }
    }
}
