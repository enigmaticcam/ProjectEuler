using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem145 : IProblem {
        private int[] _digits;
        private ulong _count;

        public string ProblemName {
            get { return "145: How many reversible numbers are there below one-billion?"; }
        }

        public string GetAnswer() {
            GetCountForAllDigits(9);
            return _count.ToString();
        }

        private void GetCountForAllDigits(int max) {
            for (int digit = 2; digit <= max; digit++) {
                _digits = new int[digit];
                GetCount(0, 0, true);
            }
        }

        private void GetCount(int digit, int remainder, bool isFirst) {
            if (digit <= _digits.GetUpperBound(0) / 2) {
                int start = 0;
                if (isFirst) {
                    start = 1;
                }
                for (int a = start; a <= 9; a++) {
                    if (digit != (_digits.GetUpperBound(0) + 1) / 2) {
                        for (int b = start; b <= 9; b++) {
                            _digits[digit] = a;
                            _digits[_digits.GetUpperBound(0) - digit] = b;
                            int sum = a + b + remainder;
                            int newRemainder = 0;
                            if (sum > 9) {
                                newRemainder = 1;
                            }
                            if (sum % 2 == 1) {
                                GetCount(digit + 1, newRemainder, false);
                            }
                        }
                    } else {
                        _digits[digit] = a;
                        int sum = a + a + remainder;
                        int newRemainder = 0;
                        if (sum > 9) {
                            newRemainder = 1;
                        }
                        if (sum % 2 == 1) {
                            GetCount(digit + 1, newRemainder, false);
                        }
                    }
                }
            } else {
                int sum = _digits[digit] + _digits[_digits.GetUpperBound(0) - digit] + remainder;
                int newRemainder = 0;
                if (sum > 9) {
                    newRemainder = 1;
                }
                if (sum % 2 == 1) {
                    if (digit < _digits.GetUpperBound(0)) {
                        GetCount(digit + 1, newRemainder, false);
                    } else {
                        _count++;
                    }
                }
            }
        }
           
    }
}
