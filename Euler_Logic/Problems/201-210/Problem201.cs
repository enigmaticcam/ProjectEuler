using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem201 : ProblemBase {
        public override string ProblemName {
            get { return "201: Subsets with a unique sum"; }
        }

        public override string GetAnswer() {
            Test();
            return BruteForce(8).ToString();
            return "";
        }

        private void Test() {
            var result = new Dictionary<int, List<Tuple>>();
            for (int x = 1; x <= 50; x++) {
                for (int y = x + 1; y <= 50; y++) {
                    var sum = x * x + y * y;
                    if (!result.ContainsKey(sum)) {
                        result.Add(sum, new List<Tuple>());
                    }
                    result[sum].Add(new Tuple() { X = x, Y = y });
                }
            }
            var final = result.Where(x => x.Value.Count > 1).ToList();
            bool stop = true;
        }

        private class Tuple {
            public int X { get; set; }
            public int Y { get; set; }
        }

        private ulong BruteForce(ulong m) {
            Recursive(1, 0, m, m / 2);
            ulong sum = 0;
            _counts.Where(x => x.Value == 1).Select(x => x.Key).ToList().ForEach(x => sum += x);
            return sum;
        }

        private Dictionary<ulong, ulong> _counts = new Dictionary<ulong, ulong>();
        private void Recursive(ulong last, ulong sum, ulong m, ulong remain) {
            for (ulong next = last; next <= m; next++) {
                var nextSum = sum + (next * next);
                if (remain > 1) {
                    Recursive(next + 1, nextSum, m, remain - 1);
                } else if (remain == 1) {
                    if (!_counts.ContainsKey(nextSum)) {
                        _counts.Add(nextSum, 1);
                    } else {
                        _counts[nextSum]++;
                    }
                }
            }
        }
    }
}
