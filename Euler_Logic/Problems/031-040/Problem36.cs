using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem36 : ProblemBase {
        public override string ProblemName {
            get { return "36: Double-base palindromes"; }
        }

        public override string GetAnswer() {
            return GetPalindromicNums().ToString();
        }

        private int GetPalindromicNums() {
            int count = 0;
            for (int num = 1; num < 1000000; num++) {
                if (IsGood(num)) {
                    count += num;
                }
            }
            return count;
        }

        private bool IsGood(int num) {
            string asBinary = Convert.ToString(num, 2);
            if (IsPalindromic(num.ToString()) && IsPalindromic(asBinary)) {
                return true;
            } else {
                return false;
            }
        }

        private bool IsPalindromic(string text) {
            for (int index = 0; index < text.Length / 2; index++) {
                if (text.Substring(index, 1) != text.Substring(text.Length - index - 1, 1)) {
                    return false;
                }
            }
            return true;
        }
    }
}
