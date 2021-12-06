using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem325 : ProblemBase {
        public override string ProblemName {
            get { return "325: Stone Game II"; }
        }

        public override string GetAnswer() {
            return BruteForce(1000).ToString();
        }

        private ulong BruteForce(ulong n) {
            ulong sum = 0;
            for (ulong a = 1; a <= n; a++) {
                for (ulong b = a + 1; b <= n; b++) {
                    var result = BruteForceCanForceWin(a, b);
                    if (!result) {
                        sum += a + b;
                    }
                }
            }
            return sum;
        }

        private Dictionary<ulong, Dictionary<ulong, bool>> _results = new Dictionary<ulong, Dictionary<ulong, bool>>();
        private bool BruteForceCanForceWin(ulong a, ulong b) {
            if (!_results.ContainsKey(a)) {
                _results.Add(a, new Dictionary<ulong, bool>());
            }
            if (!_results[a].ContainsKey(b)) {
                var result = false;
                if (b % a == 0) {
                    result = true;
                } else {
                    for (ulong m = a; m <= b; m += a) {
                        if (!BruteForceCanForceWin(Math.Min(b - m, a), Math.Max(b - m, a))) {
                            result = true;
                            break;
                        }
                    }
                }
                _results[a].Add(b, result);
            }
            return _results[a][b];
        }
    }
}
