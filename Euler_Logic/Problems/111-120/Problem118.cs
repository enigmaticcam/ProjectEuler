using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem118 : IProblem {
        private Dictionary<ulong, bool> _isPrime = new Dictionary<ulong,bool>();
        private Dictionary<int, ulong> _bitsCounts = new Dictionary<int, ulong>();
        private List<int> _bitsList;
        private ulong _sum = 0;

        public string ProblemName {
            get { return "118: Pandigital prime sets"; }
        }

        public string GetAnswer() {
            FindDistinctSets(0, 0);
            _bitsList = _bitsCounts.Keys.ToList();
            FindFullSets(0, 0, 1);
            return _sum.ToString();
        }

        private void FindDistinctSets(int bits, ulong digits) {
            for (ulong num = 1; num <= 9; num++) {
                int numBits = (int)Math.Pow(2, num);
                if ((bits & numBits) == 0) {
                    digits = (digits * 10) + num;
                    bits += numBits;
                    if (IsPrime(digits)) {
                        if (_bitsCounts.ContainsKey(bits)) {
                            _bitsCounts[bits] += 1;
                        } else {
                            _bitsCounts.Add(bits, 1);
                        }
                    }
                    if (digits.ToString().Length < 9) {
                        FindDistinctSets(bits, digits);
                    }
                    bits -= numBits;
                    digits = (digits - num) / 10;
                }
            }
        }

        private void FindFullSets(int setIndex, int bits, ulong product) {
            for (int set = setIndex; set < _bitsList.Count; set++) {
                if ((bits & _bitsList[set]) == 0) {
                    bits += _bitsList[set];
                    product *= _bitsCounts[_bitsList[set]];
                    if (bits == 1022) {
                        _sum += product;
                    } else {
                        FindFullSets(set + 1, bits, product);
                    }
                    bits -= _bitsList[set];
                    product /= _bitsCounts[_bitsList[set]];
                }
            }
        }

        private bool IsPrime(ulong num) {
            if (!_isPrime.ContainsKey(num)) {
                bool isPrime = true;
                if (num == 1) {
                    isPrime = false;
                } else if (num == 2) {
                    isPrime = true;
                } else if (num % 2 == 0) {
                    isPrime = false;
                } else {
                    ulong max = (ulong)Math.Sqrt(num);
                    for (ulong factor = 3; factor <= max; factor += 2) {
                        if (num % factor == 0) {
                            isPrime = false;
                            break;
                        }
                    }
                }
                _isPrime.Add(num, isPrime);
            }
            return _isPrime[num];
        }
    }
}
