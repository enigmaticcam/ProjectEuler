using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem145 : IProblem {
        private Dictionary<int, ulong> _howManyByDigit = new Dictionary<int, ulong>();
        private Dictionary<int, ulong> _howManyByDigitWithRemainder = new Dictionary<int, ulong>();

        public string ProblemName {
            get { return "145: How many reversible numbers are there below one-billion?"; }
        }

        public string GetAnswer() {
            HowManySingleDigit();
            HowManyDoubleDigit();
            HowManyAllDigits(3);
            return "";
        }

        private void HowManyAllDigits(int maxDigit) {
            ulong count = 0;
            for (ulong a = 1; a <= 9; a++) {
                for (ulong b = 1; b <= 9; b++) {
                    if ((a + b) % 2 != 0) {
                        if ((a + b) > 9) {
                            count += _howManyByDigitWithRemainder[1];
                        } else {
                            count += _howManyByDigit[1];
                        }
                    }
                }
            }
        }

        private void HowManySingleDigit() {
            _howManyByDigit.Add(1, 0);
            _howManyByDigitWithRemainder.Add(1, 0);
        }

        private void HowManyDoubleDigit() {
            ulong count = 0;
            ulong countWithRemainder = 0;
            for (ulong a = 1; a <= 9; a++) {
                for (ulong b = 1; b <= 9; b++) {
                    if ((a + b) % 2 != 0) {
                        count++;
                    }
                    if ((a + b + 1) % 2 != 0) {
                        countWithRemainder++;
                    }
                }
            }
            _howManyByDigit.Add(2, count);
            _howManyByDigitWithRemainder.Add(2, countWithRemainder);
        }

        private bool HasAllOdds(string number) {
            for (int index = 0; index < number.Length; index++) {
                if (Convert.ToInt32(number.Substring(index, 1)) % 2 == 0) {
                    return false;
                }
            }
            return true;
        }
           
    }
}
