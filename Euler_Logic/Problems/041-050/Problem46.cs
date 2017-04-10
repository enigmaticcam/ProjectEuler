using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem46 : ProblemBase {
        private decimal _primeNum;
        private HashSet<decimal> _primes = new HashSet<decimal>();
        private Dictionary<decimal, bool> _isPrime = new Dictionary<decimal, bool>();
        private decimal _squareIndex;
        private decimal _squareNum;
        private HashSet<decimal> _squares = new HashSet<decimal>();

        public override string ProblemName {
            get { return "46: Goldbach's other conjecture"; }
        }

        public override string GetAnswer() {
            _primeNum = 3;
            _primes.Add(2);
            return FindCombposite().ToString();
        }

        private decimal FindCombposite() {
            decimal num = 1;
            do {
                num += 2;
                if (!IsPrime(num)) {
                    CalcNextPrime(num);
                    CalcNextSquare(num);
                    bool isGood = true;
                    foreach (decimal prime in _primes) {
                        if (prime < num) {
                            if (_squares.Contains(num - prime)) {
                                isGood = false;
                                break;
                            }
                        }
                    }
                    if (isGood) {
                        return num;
                    }
                }
            } while (true);
        }

        private void CalcNextSquare(decimal max) {
            do {
                _squareIndex++;
                _squareNum = _squareIndex * _squareIndex * 2;
                _squares.Add(_squareNum);
            } while (_squareNum < max);
        }

        private void CalcNextPrime(decimal max) {
            do {
                _primeNum += 2;
                if (IsPrime(_primeNum)) {
                    _primes.Add(_primeNum);
                }
            } while (_primeNum < max);
        }

        private bool IsPrime(decimal num) {
            if (_isPrime.ContainsKey(num)) {
                return _isPrime[num];
            } else if (num == 2) {
                _isPrime.Add(num, true);
                return true;
            } else if (num % 2 == 0) {
                _isPrime.Add(num, false);
                return false;
            } else {
                for (ulong factor = 3; factor <= Math.Sqrt((double)num); factor += 2) {
                    if (num % factor == 0) {
                        _isPrime.Add(num, false);
                        return false;
                    }
                }
            }
            _isPrime.Add(num, true);
            return true;
        }
    }
}
