using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem37 : IProblem {
        private Dictionary<decimal, bool> _primes = new Dictionary<decimal, bool>();
        private HashSet<string> _finished = new HashSet<string>();

        public string ProblemName {
            get { return "37: Truncatable primes"; }
        }

        public string GetAnswer() {
            return GetPrimeCount().ToString();
        }

        private int GetPrimeCount() {
            int count = 0;
            int sum = 0;
            int index = 11;
            do {
                List<string> truncates = GetTruncates(index.ToString());
                bool isGood = true;
                foreach (string num in truncates) {
                    decimal numAsDec = Convert.ToDecimal(num);
                    if (!IsPrime(numAsDec)) {
                        isGood = false;
                        break;
                    }
                }
                if (isGood) {
                    sum += index;
                    count++;
                }
                index += 2;
            } while (count < 11);
            return sum;
        }

        private List<string> GetTruncates(string text) {
            List<string> truncates = new List<string>();
            truncates.Add(text);
            for (int index = 1; index < text.Length; index++) {
                truncates.Add(text.Substring(0, index));
                truncates.Add(text.Substring(text.Length - index, index));
            }
            return truncates;
        }

        private bool IsPrime(decimal num) {
            if (num < 2) {
                return false;
            }
            if (_primes.ContainsKey(num)) {
                return _primes[num];
            }
            if (num == 2) {
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
