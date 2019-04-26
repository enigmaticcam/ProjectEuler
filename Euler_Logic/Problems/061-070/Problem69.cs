using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem69 : ProblemBase {
        private PrimeSieve _primes;

        /*
            According to the wiki on euler's totient function (https://en.wikipedia.org/wiki/Euler%27s_totient_function), if the prime factors
            of (n) are (p1, p2, p3...pr), then the totient value can be calculated: n(1 - (1/p1))(1 - (1/p2))...(1 - (1/pr)). So what I do
            is create a starting fraction for all numbers up to 1000000. Then I loop through all prime numbers, and for each prime number,
            I multiply all of its composites by (p - 1)/p. That will give me the totient value for all numbers. I then just loop through them
            all to find the highest value for n/t.
         */

        public override string ProblemName {
            get { return "69: Totient maximum"; }
        }

        public override string GetAnswer() {
            ulong max = 1000000;
            _primes = new PrimeSieve(max);
            BuildPractions(max);
            FindTotiens(max);
            return FindHighest(max).ToString();
        }

        private Dictionary<ulong, Fraction> _fractions = new Dictionary<ulong, Fraction>();
        private void BuildPractions(ulong max) {
            for (ulong num = 2; num <= max; num++) {
                _fractions.Add(num, new Fraction(num, 1));
            }
        }

        private void FindTotiens(ulong max) {
            foreach (var prime in _primes.Enumerate) {
                for (ulong composite = 2; composite * prime <= max; composite++) {
                    _fractions[composite * prime].Multiply(prime - 1, prime);
                }
            }
        }

        private ulong FindHighest(ulong max) {
            ulong highestNum = 0;
            decimal highestValue = 0;
            for (ulong num = 4; num <= max; num++) {
                if (!_primes.IsPrime(num)) {
                    var fract = _fractions[num];
                    decimal value = (decimal)num / (decimal)fract.X;
                    if (value > highestValue) {
                        highestValue = value;
                        highestNum = num;
                    }
                }
            }
            return highestNum;
        }
    }
}
