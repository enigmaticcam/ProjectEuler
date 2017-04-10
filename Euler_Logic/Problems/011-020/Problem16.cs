using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem16 : ProblemBase {
        private List<int> _digits = new List<int>();

        public override string ProblemName {
            get { return "16: Power digit sum"; }
        }

        public override string GetAnswer() {
            BuildDigits(1000);
            return PerformSum().ToString();
        }

        private void BuildDigits(int max) {
            _digits.Add(2);
            for (int i = 1; i < max; i++) {
                AddDigits(_digits, _digits);
            }
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

        private int PerformSum() {
            int sum = 0;
            for (int i = 0; i < _digits.Count; i++) {
                sum += _digits[i];
            }
            return sum;
        }
    }
}
