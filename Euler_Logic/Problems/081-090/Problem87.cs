using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem87 : IProblem {
        private HashSet<double> _primes = new HashSet<double>();
        private HashSet<double> _cubes = new HashSet<double>();
        private HashSet<double> _numbers = new HashSet<double>();

        public string ProblemName {
            get { return "87: Prime power triples"; }
        }

        public string GetAnswer() {
            double max = 50000000;
            SievePrimes(Math.Sqrt(max));
            return GetCount(max).ToString();
        }

        private void BuildCubes() {
            foreach (double prime in _primes) {
                _cubes.Add(prime * prime * prime);
            }
        }

        private double GetCount(double max) {
            foreach (double square in _primes) {
                double num1 = square * square;
                if (num1 < max) {
                    foreach (double cube in _primes) {
                        double num2 = cube * cube * cube;
                        if (num1 + num2 < max) {
                            foreach (double quad in _primes) {
                                double num3 = quad * quad * quad * quad;
                                if (num1 + num2 + num3 <= max) {
                                    _numbers.Add(num1 + num2 + num3);
                                } else {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return _numbers.Count();
        }

        private void SievePrimes(double max) {
            HashSet<double> numbers = new HashSet<double>();
            for (double num = 2; num <= max; num++) {
                if (!numbers.Contains(num)) {
                    _primes.Add(num);
                    double composite = num;
                    do {
                        numbers.Add(composite);
                        composite += num;
                    } while (composite <= max);
                }
            }
        }
    }
}
