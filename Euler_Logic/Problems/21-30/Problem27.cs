using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem27 : IProblem {
        private Dictionary<decimal, bool> _primes = new Dictionary<decimal, bool>();

        public string ProblemName {
            get { return "27: Quadratic primes"; }
        }

        public string GetAnswer() {
            return LoopAllNums().ToString();
        }

        private decimal LoopAllNums() {
            int bestScore = int.MinValue;
            decimal bestNum = 0;
            for (decimal a = 1; a < 1000; a++) {
                for (decimal b = 1; b < 1000; b++) {
                    int score = GetConsecutiveCount(a, b);
                    if (score > bestScore) {
                        bestScore = score;
                        bestNum = a * b;
                    }

                    score = GetConsecutiveCount(a * -1, b);
                    if (score > bestScore) {
                        bestScore = score;
                        bestNum = a * b * -1;
                    }

                    score = GetConsecutiveCount(a, b * -1);
                    if (score > bestScore) {
                        bestScore = score;
                        bestNum = a * b * -1;
                    }

                    score = GetConsecutiveCount(a * -1, b * -1);
                    if (score > bestScore) {
                        bestScore = score;
                        bestNum = a * b;
                    }
                }
            }
            return bestNum;
        }

        private int GetConsecutiveCount(decimal a, decimal b) {
            decimal n = 0;
            int count = 0;
            do {
                decimal num = (n * n) + (a * n) + b;
                if (IsPrime(num)) {
                    count++;
                } else {
                    return count;
                }
                n++;
            } while (true);
        }

        private bool IsPrime(decimal num) {
            if (_primes.ContainsKey(num)) {
                return _primes[num];
            } else if (num < 1) {
                _primes.Add(num, false);
                return false;
            } else if (num == 2) {
                _primes.Add(num, true);
                return true;
            } else if (num % 2 == 0) {
                _primes.Add(num, false);
                return false;
            } else {
                for (ulong factor = 3; factor <= Math.Sqrt((double)num); factor += 2) {
                    if (num % factor == 0) {
                        _primes.Add(num, false);
                        return false;
                    }
                }
            }
            _primes.Add(num, true);
            return true;
        }
    }
}
