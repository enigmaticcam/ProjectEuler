using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem293 : ProblemBase {
        private List<double> _primes = new List<double>();
        private double[] _powers;
        private HashSet<double> _admissables = new HashSet<double>();
        private HashSet<double> _fortunates = new HashSet<double>();
        private Dictionary<double, bool> _primesFound = new Dictionary<double, bool>();

        public override string ProblemName {
            get { return "293: Pseudo-Fortunate Numbers"; }
        }

        public override string GetAnswer() {
            double max = 1000000000;
            FindPrimes(max);
            FindAdmissableNumbers(max, 0, 1);
            return FindFortunates().ToString();
        }

        private void FindPrimes(double max) {
            List<double> primes = new List<double>();
            primes.Add(2);
            primes.Add(3);
            primes.Add(5);
            primes.Add(7);
            primes.Add(11);
            primes.Add(13);
            primes.Add(17);
            primes.Add(19);
            primes.Add(23);

            double total = 1;
            foreach (double prime in primes) {
                total *= prime;
                if (total <= max) {
                    _primes.Add(prime);
                } else {
                    break;
                }
            }

            _powers = new double[_primes.Count];
            _powers[0] = 1;
        }

        private void FindAdmissableNumbers(double max, int powerIndex, double currentNum) {
            do {
                double nextPower = Math.Pow(_primes[powerIndex], _powers[powerIndex]);
                currentNum *= nextPower;
                if (currentNum > max) {
                    break;
                }
                if (powerIndex == _powers.Count() - 1) {
                    _admissables.Add(currentNum);
                } else {
                    FindAdmissableNumbers(max, powerIndex + 1, currentNum);
                }
                currentNum /= nextPower;
                if (powerIndex == 0) {
                    _powers[powerIndex]++;
                } else if (_powers[powerIndex - 1] > 0) {
                    _powers[powerIndex]++;
                } else {
                    break;
                }
            } while (true);
            _powers[powerIndex] = 0;
        }

        private double FindFortunates() {
            double sum = 0;
            foreach (double admissable in _admissables) {
                double num = admissable + 3;
                while (!IsPrime(num)) {
                    num += 1;
                }
                double fortunate = num - admissable;
                if (!_fortunates.Contains(fortunate)) {
                    sum += fortunate;
                    _fortunates.Add(fortunate);
                }
            }
            return sum;
        }

        private bool IsPrime(double num) {
            if (_primesFound.ContainsKey(num)) {
                return _primesFound[num];
            } else {
                double max = Math.Sqrt(num);
                for (ulong factor = 2; factor <= max; factor++) {
                    if (num % factor == 0) {
                        _primesFound.Add(num, false);
                        return false;
                    }
                }
                _primesFound.Add(num, true);
                return true;
            }
        }
    }
}
