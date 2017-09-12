using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem347 : ProblemBase {
        private bool[] _notPrimes;
        private List<double> _primes = new List<double>();

        public override string ProblemName {
            get { return "347: Largest integer divisible by two primes"; }
        }

        public override string GetAnswer() {
            double max = 10000000;
            SievePrimes(max);
            return Solve(max).ToString();
        }

        private void SievePrimes(double max) {
            _notPrimes = new bool[(int)max + 1];
            for (int num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    _primes.Add(num);
                    for (int composite = 2; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
            _notPrimes = null;
        }

        private double Solve(double max) {
            double sum = 0;
            for (int x = 0; x < _primes.Count; x++) {
                for (int y = x + 1; y < _primes.Count; y++) {
                    if (_primes[y] * _primes[x] <= max) {
                        double xPower = 1;
                        double xResult = Math.Pow(_primes[y], xPower);
                        double highest = 0;
                        while (xResult <= max) {
                            double remaining = max / xResult;
                            int nextPower = (int)Math.Log(remaining, _primes[x]);
                            if (nextPower == 0) {
                                break;
                            }
                            double yResult = Math.Pow(_primes[x], nextPower);
                            double result = yResult * xResult;
                            if (result > highest) {
                                highest = result;
                            }
                            xPower++;
                            xResult = Math.Pow(_primes[y], xPower);
                        }
                        sum += highest;
                    } else {
                        break;
                    }
                }
            }
            return sum;
        }
    }
}
