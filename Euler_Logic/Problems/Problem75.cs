using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem75 : IProblem {
        private Dictionary<double, double> _triangleCount = new Dictionary<double, double>();
        private HashSet<double> _squares = new HashSet<double>();
        private List<double> _squaresList = new List<double>();

        public string ProblemName {
            get { return "75: Singular Integer Right Triangles"; }
        }

        public string GetAnswer() {
            double max = 1500000;
            GenerateSquares(max);
            for (int i = 0; i < _squares.Count; i++) {
                LookForIntegerTriangles(i, max);
            }
            int count = 0;
            foreach (double value in _triangleCount.Keys) {
                if (_triangleCount[value] == 1) {
                    count++;
                }
            }
            return count.ToString();
        }

        private void GenerateSquares(double max) {
            double actualMax = max;
            double a = 1;
            do {
                _squares.Add(a * a);
                _squaresList.Add(a * a);
                a++;
            } while (a <= actualMax);
        }

        public void LookForIntegerTriangles(int start, double max) {
            double a = _squaresList[start];
            double b = 0;
            double c = 0;
            if (start < _squaresList.Count - 1) {
                b = _squaresList[start];
                c = a + b;
                while (a + b >= _squaresList[start + 1] && Math.Sqrt(a) + Math.Sqrt(b) + Math.Sqrt(c) <= max && start < _squaresList.Count - 1) {
                    if (_squares.Contains(c)) {
                        double final = Math.Sqrt(a) + Math.Sqrt(b) + Math.Sqrt(c);
                        if (!_triangleCount.ContainsKey(final)) {
                            _triangleCount.Add(final, 1);
                        } else {
                            _triangleCount[final] += 1;
                        }
                    }
                    start++;
                    if (start < _squaresList.Count - 1) {
                        b = _squaresList[start];
                        c = a + b;
                    }
                }
            }
        }
    }
}
