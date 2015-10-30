using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem64 : IProblem {
        private HashSet<string> _seenBefore = new HashSet<string>();

        public string ProblemName {
            get { return "64: Odd period square roots"; }
        }

        public string GetAnswer() {
            return CountOddPeriods(10000).ToString();
        }

        private int CountOddPeriods(int max) {
            int count = 0;
            for (int num = 2; num <= max; num++) {
                if (!IsSquare(num) && IsOddPeriod(num)) {
                    count++;
                }
            }
            return count;
        }

        private bool IsSquare(int num) {
            double root = Math.Sqrt(num);
            if (root.ToString().Contains(".")) {
                return false;
            } else {
                return true;
            }
        }

        private bool IsOddPeriod(int num) {
            _seenBefore = new HashSet<string>();
            int m = 0;
            int d = 1;
            int a = (int)Math.Sqrt(num);
            int count = 0;
            do {
                m = (d * a) - m;
                d = (num - (m * m)) / d;
                a = ((int)Math.Sqrt(num) + m) / d;
                count++;
            } while (!HasSeenBefore(m, d, a));
            count--;
            if (count % 2 == 0) {
                return false;
            } else {
                return true;
            }
        }

        private bool HasSeenBefore(int m, int d, int a) {
            string key = m.ToString() + ":" + d.ToString() + ":" + a.ToString();
            if (_seenBefore.Contains(key)) {
                return true;
            } else {
                _seenBefore.Add(key);
                return false;
            }
        }
    }
}
