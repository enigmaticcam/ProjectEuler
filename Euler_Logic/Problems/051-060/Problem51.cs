using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem51 : ProblemBase {
        private Dictionary<decimal, bool> _primes = new Dictionary<decimal, bool>();
        private decimal _lowestPrime;

        public override string ProblemName {
            get { return "51: Prime Digit Replacements"; }
        }

        public override string GetAnswer() {
            return FindBestFamily(8).ToString();
        }

        private decimal FindBestFamily(int maxPrimeFamily) {
            decimal num = 11;
            do {
                if (IsPrime(num)) {
                    string text = num.ToString();
                    double max = Math.Pow(2, text.Length) - 1;
                    for (int bits = 1; bits <= max; bits++) {
                        if (FindFamily(text, bits) == maxPrimeFamily) {
                            return _lowestPrime;
                        }
                    }
                }
                num += 2;
            } while (true);
        }

        private int FindFamily(string num, int bits) {
            for (int index = 0; index < num.Length; index++) {
                if (((int)Math.Pow(2, index) & bits) == (int)Math.Pow(2, index)) {
                    num = num.Remove(index, 1).Insert(index, "*");
                }
            }
            int count = 0;
            bool firstTime = true;
            for (int digit = 0; digit <= 9; digit++) {
                decimal replaced = Convert.ToDecimal(num.Replace("*", digit.ToString()));
                if (digit == 0 && (bits & 1) == 1) {
                    // do nothing
                } else {
                    if (firstTime) {
                        _lowestPrime = replaced;
                        firstTime = false;
                    }
                    if (IsPrime(replaced)) {
                        count++;
                    }
                }
            }
            return count;
        }

        private bool IsPrime(decimal num) {
            if (_primes.ContainsKey(num)) {
                return _primes[num];
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
