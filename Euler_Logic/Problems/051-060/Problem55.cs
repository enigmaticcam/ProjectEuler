using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem55 : ProblemBase {
        public override string ProblemName {
            get { return "55: Lynchrel Numbers"; }
        }

        public override string GetAnswer() {
            int lynchrel = 0;
            for (decimal i = 1; i < 10000; i++) {
                if (IsLynchrel(i)) {
                    lynchrel++;
                }
            }
            return lynchrel.ToString();
        }

        private bool IsLynchrel(decimal number) {            
            int countLimit = 50;
            int count = 1;
            decimal a = number;
            decimal b = 0;
            do {
                char[] aAsText = a.ToString().ToCharArray();
                char[] bAsText = new char[aAsText.Length];
                for (int i = 0; i < aAsText.Length; i++) {
                    bAsText[i] = aAsText[aAsText.Length - i - 1];
                }
                b = Convert.ToDecimal(new string(bAsText));
                decimal c = a + b;
                if (IsPalindrome(c)) {
                    return false;
                }
                count++;
                a = c;
            } while (count < countLimit);
            return true;
        }

        private bool IsPalindrome(decimal number) {
            string numberAsText = number.ToString();
            for (int i = 0; i <= numberAsText.Length / 2; i++) {
                if (numberAsText.Substring(i, 1) != numberAsText.Substring(numberAsText.Length - i - 1, 1)) {
                    return false;
                }
            }
            return true;
        }
    }
}
