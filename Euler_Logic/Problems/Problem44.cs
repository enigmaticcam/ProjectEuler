using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem44 : IProblem {
        private HashSet<int> _pentagonals = new HashSet<int>();
        private int _n;

        public string ProblemName {
            get { return "44: Pentagon Numbers"; }
        }

        public string GetAnswer() {
            do {
                int next = GetNextPentagonal();
                foreach (int pentagonal in _pentagonals) {
                    if (pentagonal > next / 2) {
                        break;
                    }
                    int diff = next - pentagonal;
                    if (_pentagonals.Contains(diff) && _pentagonals.Contains(diff - pentagonal)) {
                        return Math.Abs(diff - pentagonal).ToString();
                    }
                }
            } while (true);
        }

        private int GetNextPentagonal() {
            _n++;
            int next = (_n * ((3 * _n) - 1)) / 2;
            _pentagonals.Add(next);
            return next;
        }
    }
}
