using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem51 : IProblem {
        private Dictionary<decimal, bool> _primes = new Dictionary<decimal, bool>();
        private HashSet<decimal> _primesAlreadyTried = new HashSet<decimal>();

        public string ProblemName {
            get { return "51: Prime Digit Replacements"; }
        }

        public string GetAnswer() {
            return "";
        }

        private decimal FindBestFamily() {
            decimal num = 11;
            do {
                if (IsPrime(num)) {
                    string text = num.ToString();
                    double max = Math.Pow(2, text.Length - 1);
                    for (int bits = 1; bits <= max; bits++) {

                    }
                }

                num++;
            } while (true);
            return 0;
        }

        private void FindFamily(decimal num, int bits) {

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
