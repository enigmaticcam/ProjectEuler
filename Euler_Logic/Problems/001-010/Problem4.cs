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
            int max = (int)Math.Pow(10, digits) - 1;
            for (int num = max * max; num >= min * min; num--) {
                if (IsPalindrome(num.ToString())) {
                    int subMax = (int)Math.Sqrt(num);
                    for (int sub = min; sub <= subMax; sub++) {
                        if (num % sub == 0 && num / sub >= min && num / sub <= max) return num;
                    }
                }
            }
            return 0;
        }

        private bool IsPalindrome(string text) {
            for (int index = 0; index < text.Length / 2; index++) {
                if (text[index] != text[text.Length - index - 1]) return false;
            }
            return true;
        }
    }
}
