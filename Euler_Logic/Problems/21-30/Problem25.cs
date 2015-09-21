using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem25 : IProblem {
        private List<int> _digits = new List<int>();
        private List<int> _previous = new List<int>();

        public string ProblemName {
            get { return "25: 1000-digit Fibonacci number"; }
        }

        public string GetAnswer() {
            return FindFibDigit(1000).ToString();
        }

        private int FindFibDigit(int digits) {
            int index = 2;            
            _digits.Add(1);

            List<int> last = new List<int>(_digits);
            do {
                index++;
                _previous = new List<int>(_digits);
                AddDigits(last, _digits);
                last = new List<int>(_previous);
            } while (_digits.Count < digits);
            return index;
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
