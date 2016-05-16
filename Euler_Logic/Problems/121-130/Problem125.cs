using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem125 : IProblem {
        private HashSet<decimal> _palindromes = new HashSet<decimal>();

        public string ProblemName {
            get { return "125: Palindromic sums"; }
        }

        public string GetAnswer() {
            GeneratePalindromes(100000000);
            return GetPalindromeSum().ToString();
        }

        private void GeneratePalindromes(decimal max) {
            decimal start = 1;
            do {
                decimal sum = start * start;
                decimal next = start + 1;
                do {
                    sum += (next * next);
                    if (IsPalindrome(sum) && sum < max) {
                        _palindromes.Add(sum);
                    }
                    next++;
                } while (sum < max);

                start++;
            } while (start * start < max);
        }

        private bool IsPalindrome(decimal num) {
            string numAsString = num.ToString();
            for (int index = 0; index < numAsString.Length; index++) {
                if (numAsString.Substring(index, 1) != numAsString.Substring(numAsString.Length - index - 1, 1)) {
                    return false;
                }
            }
            return true;
        }

        private decimal GetPalindromeSum() {
            decimal sum = 0;
            foreach (decimal pal in _palindromes) {
                sum += pal;
            }
            return sum;
        }
    }
}
