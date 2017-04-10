using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem23 : ProblemBase {
        private List<int> _abundants = new List<int>();
        private HashSet<int> _abundantSums = new HashSet<int>();

        public override string ProblemName {
            get { return "23: Non-abundant sums"; }
        }

        public override string GetAnswer() {
            BuildAbundants();
            FindAbundantSums();
            return NonAbundantsSum().ToString();
        }

        private int NonAbundantsSum() {
            int sum = 0;
            for (int a = 1; a <= 28123; a++) {
                if (!_abundantSums.Contains(a)) {
                    sum += a;
                }
            }
            return sum;
        }

        private void FindAbundantSums() {
            for (int a = 0; a < _abundants.Count; a++) {
                for (int b = a; b < _abundants.Count; b++) {
                    if (_abundants[a] + _abundants[b] > 28123) {
                        break;
                    } else {
                        _abundantSums.Add(_abundants[a] + _abundants[b]);
                    }
                }
            }
        }

        private void BuildAbundants() {
            for (int i = 2; i <= 28123; i++) {
                if (IsAbundant(i)) {
                    _abundants.Add(i);
                }
            }
        }

        private bool IsAbundant(int num) {
            int sum = 0;
            for (int i = 1; i <= Math.Sqrt(num); i++) {
                if (num % i == 0) {
                    sum += i;
                    if (i != Math.Sqrt(num) && i != 1) {
                        sum += num / i;
                    }
                    if (sum > num) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
