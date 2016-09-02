using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem118 : IProblem {
        private Dictionary<ulong, bool> _isPrime = new Dictionary<ulong,bool>();
        private Dictionary<ulong, int> _numToBits = new Dictionary<ulong, int>();
        private List<ulong> _nums = new List<ulong>();
        private ulong _sum = 0;

        public string ProblemName {
            get { return "118: Pandigital prime sets"; }
        }

        public string GetAnswer() {
            FindDistinctSets(0, 0);
            FindFullSets(0, 0);
            return _sum.ToString();
        }

        private void FindDistinctSets(int bits, ulong digits) {
            for (ulong num = 1; num <= 9; num++) {
                int numBits = (int)Math.Pow(2, num);
                if ((bits & numBits) == 0) {
                    digits = (digits * 10) + num;
                    bits += numBits;
                    if (IsPrime(digits)) {
                        _numToBits.Add(digits, bits);
                        _nums.Add(digits);
                    }
                    if (digits.ToString().Length < 9) {
                        FindDistinctSets(bits, digits);
                    }
                    bits -= numBits;
                    digits = (digits - num) / 10;
                }
            }
        }

        private void FindFullSets(int setIndex, int bits) {
            for (int set = setIndex; set < _nums.Count; set++) {
                if ((bits & _numToBits[_nums[set]]) == 0) {
                    bits += _numToBits[_nums[set]];
                    if (bits == 1022) {
                        _sum += 1;
                    } else {
                        FindFullSets(set + 1, bits);
                    }
                    bits -= _numToBits[_nums[set]];
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
