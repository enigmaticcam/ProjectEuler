using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
     public class Problem50 : ProblemBase {
        private HashSet<decimal> _primes = new HashSet<decimal>();
        private Dictionary<int, decimal> _left = new Dictionary<int, decimal>();
        private Dictionary<int, decimal> _right = new Dictionary<int, decimal>();
        private decimal _primeSum = 2;

        public override string ProblemName {
            get { return "50: Consecutive prime sum"; }
        }

        public override string GetAnswer() {
            decimal max = 1000000;
            GeneratePrimes(max);
            return SliceAndDice(max).ToString();
        }

        private decimal SliceAndDice(decimal max) {
            _left.Add(0, 0);
            _right.Add(0, 0);
            for (int slice = 1; slice < _primes.Count; slice++) {
                for (int dice = 1; dice <= slice; dice++) {
                    decimal cut = _primeSum - GetRightSlice(dice) - GetLeftSlice(slice - dice);
                    if (cut < max && IsPrime(cut)) {
                        return cut;
                    }
                }
            }
            return 0;
        }

        private decimal GetRightSlice(int slice) {
            if (!_right.ContainsKey(slice)) {
                _right.Add(slice, _right[slice - 1] + _primes.ElementAt(_primes.Count - slice));
            }
            return _right[slice];
        }

        private decimal GetLeftSlice(int slice) {
            if (!_left.ContainsKey(slice)) {
                _left.Add(slice, _left[slice - 1] + _primes.ElementAt(slice - 1));
            }
            return _left[slice];
        }

        private void GeneratePrimes(decimal max) {
            _primes.Add(2);
            for (decimal num = 3; num < max; num++) {
                if (IsPrime(num)) {
                    _primeSum += num;
                    _primes.Add(num);
                    if (_primeSum > max) {
                        break;
                    }
                }
            }
        }

        private bool IsPrime(decimal num) {
            if (num == 2) {
                return true;
            } else if (num % 2 == 0) {
                return false;
            } else {
                for (ulong factor = 3; factor <= Math.Sqrt((double)num); factor += 2) {
                    if (num % factor == 0) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
