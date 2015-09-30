using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem57 : IProblem {
        private List<int> _denominator = new List<int>();
        private List<int> _numerator = new List<int>();

        public string ProblemName {
            get { return "57: Square root convergents"; }
        }

        public string GetAnswer() {
            return GetTotalCount(1000).ToString();
        }

        private int GetTotalCount(int max) {
            int count = 1;
            int total = 0;
            _denominator.Add(2);
            _numerator.Add(3);
            do {
                AddDigits(_denominator, _numerator);
                List<int> temp = new List<int>();
                AddDigits(_numerator, temp);
                _numerator = new List<int>();
                AddDigits(_denominator, _numerator);
                _denominator = temp;
                AddDigits(_denominator, _numerator);
                if (_numerator.Count > _denominator.Count) {
                    total++;
                }
                count++;
            } while (count < max);
            return total;
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
