using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem70 : ProblemBase {
        private PrimeSieve _primes;

        public override string ProblemName {
            get { return "70: Totient permutation"; }
        }

        /*
            According to the wiki on euler's totient function (https://en.wikipedia.org/wiki/Euler%27s_totient_function), if the prime factors
            of (n) are (p1, p2, p3...pr), then the totient value can be calculated: n(1 - (1/p1))(1 - (1/p2))...(1 - (1/pr)). So what I do
            is create a starting fraction for all numbers up to 1000000. Then I loop through all prime numbers, and for each prime number,
            I multiply all of its composites by (p - 1)/p. That will give me the totient value for all numbers. I then just loop through them
            all to find which ones return a permutation, and of those return the one with the lowest value n/t(n).
         */

        public override string GetAnswer() {
            ulong max = 10000000;
            _primes = new PrimeSieve(max);
            BuildFractions(max);
            FindTotients(max);
            return FindLowestPermutation(max).ToString();
        }

        private Dictionary<ulong, Fraction> _fractions = new Dictionary<ulong, Fraction>();
        private void BuildFractions(ulong max) {
            for (ulong num = 2; num <= max; num++) {
                _fractions.Add(num, new Fraction(num, 1));
            }
        }

        private void FindTotients(ulong max) {
            foreach (var prime in _primes.Enumerate) {
                for (ulong composite = 1; composite * prime <= max; composite++) {
                    _fractions[composite * prime].Multiply(prime - 1, prime);
                }
            }
        }

        private ulong FindLowestPermutation(ulong max) {
            ulong lowestNum = 0;
            decimal lowestValue = decimal.MaxValue;
            for (ulong num = 4; num <= max; num++) {
                var fract = _fractions[num];
                if (IsPermutation(num, fract.X)) {
                    decimal value = (decimal)num / (decimal)fract.X;
                    if (value < lowestValue) {
                        lowestValue = value;
                        lowestNum = num;
                    }
                }
            }
            return lowestNum;
        }

        private bool IsPermutation(ulong num1, ulong num2) {
            string x = num1.ToString();
            string y = num2.ToString();
            for (int index = 0; index < x.Length; index++) {
                if (x.Replace(x.Substring(index, 1), "").Length != y.Replace(x.Substring(index, 1), "").Length) {
                    return false;
                }
            }
            return true;
        }
    }
}

