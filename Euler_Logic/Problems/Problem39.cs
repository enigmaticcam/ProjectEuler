using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem39 : IProblem {
        private Dictionary<double, int> _counts = new Dictionary<double, int>();

        public string ProblemName {
            get { return "39: Integer right triangles"; }
        }

        public string GetAnswer() {
            GetBestRightTriangle();
            return BestValue().ToString();
        }

        private void GetBestRightTriangle() {
            double a = 1;
            double b = 2;
            double c = Math.Sqrt((a * a) + (b * b));
            do {
                if (a == 30 && b == 40 && c == 50) {
                    bool stophere = true;
                }
                if (c == (int)c) {
                    if (_counts.ContainsKey(a + b + c)) {
                        _counts[a + b + c] += 1;
                    } else {
                        _counts.Add(a + b + c, 1);
                    }
                }

                b += 1;
                c = Math.Sqrt((a * a) + (b * b));
                if (a + b + c > 1000) {
                    a += 1;
                    b = a + 1;
                    c = Math.Sqrt((a * a) + (b * b));
                }
            } while (a + b + c <= 1000);
        }

        private double BestValue() {
            int bestCount = 0;
            double bestValue = 0;
            foreach (double value in _counts.Keys) {
                if (_counts[value] > bestCount) {
                    bestCount = _counts[value];
                    bestValue = value;
                }
            }
            return bestValue;
        }
    }
}
