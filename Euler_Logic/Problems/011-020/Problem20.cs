using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem20 : IProblem {
        private List<int> _digits = new List<int>();

        public string ProblemName {
            get { return "20: Factorial digit sum"; }
        }

        public string GetAnswer() {
            BuildDigits(100);
            return SumDigits().ToString();
        }

        private void BuildDigits(int max) {
            SetInitialDigits(max);
            for (int num = max - 1; num >= 1; num--) {
                if (num > 1) {
                    List<int> product = new List<int>(_digits);
                    for (int term = 1; term < num; term++) {
                        AddDigits(product, _digits);
                    }
                }
            }
        }

        private void SetInitialDigits(int max) {
            string maxAsString = max.ToString();
            for (int i = 0; i < maxAsString.Length; i++) {
                _digits.Add(Convert.ToInt32(maxAsString.Substring(maxAsString.Length - i - 1, 1)));
            }
        }

        private int SumDigits() {
            int sum = 0;
            for (int i = 0; i < _digits.Count; i++) {
                sum += _digits[i];
            }
            return sum;
        }

        private void AddDigits(List<int> from, List<int> to) {
            int carryOver = 0;
            int digit = 0;
            while (digit < from.Count || carryOver > 0) {
                if (to.Count <= digit) {
                    to.Add(0);
                }
                int sum = 0;
                if (digit < from.Count) {
                    sum = from[digit] + to[digit] + carryOver;
                } else {
                    sum = to[digit] + carryOver;
                }
                string sumText = sum.ToString();
                if (sumText.Length > 1) {
                    to[digit] = Convert.ToInt32(sumText.Substring(1, 1));
                    carryOver = Convert.ToInt32(sumText.Substring(0, 1));
                } else {
                    to[digit] = sum;
                    carryOver = 0;
                }
                digit++;
            }
        }
    }
}
