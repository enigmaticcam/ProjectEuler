using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem4 : ProblemBase {
        public override string ProblemName {
            get { return "4: Largest palindrome product"; }
        }

        public override string GetAnswer() {
            return FindLargestPalindrome(3).ToString();
        }

        private int FindLargestPalindrome(int digits) {
            int min = (int)Math.Pow(10, digits - 1);
            int max = (int)Math.Pow(10, digits);
            int best = 0;
            for (int a = min; a < max; a++) {
                for (int b = a; b < max; b++) {
                    int num = a * b;
                    if (IsPalindrome(num.ToString()) && num > best) {
                        best = num;
                    }
                }
            }
            return best;
        }

        private bool IsPalindrome(string text) {
            for (int i = 0; i < text.Length / 2; i++) {
                if (text.Substring(i, 1) != text.Substring(text.Length - i - 1, 1)) {
                    return false;
                }
            }
            return true;
        }
    }
}
