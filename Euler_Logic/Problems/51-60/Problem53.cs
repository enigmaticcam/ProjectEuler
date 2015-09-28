using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem53 : IProblem {
        private Dictionary<double, double> _factorials = new Dictionary<double, double>();

        public string ProblemName {
            get { return "53: Combinatoric selections"; }
        }

        public string GetAnswer() {
            GenerateFactorials();
            return CountAboveMillion().ToString();
        }

        private int CountAboveMillion() {
            int count = 0;
            for (double n = 1; n <= 100; n++) {
                for (double r = 1; r <= n; r++) {
                    if (IsAboveMillion(n, r)) {
                        count++;
                    }
                }
            }
            return count;
        }

        private bool IsAboveMillion(double n, double r) {
            double result = _factorials[n] / (_factorials[r] * _factorials[(n - r)]);
            if (result > 1000000) {
                return true;
            } else {
                return false;
            }
        }

        private void GenerateFactorials() {
            double factorial = 1;
            for (double factor = 0; factor <= 100; factor++) {
                if (factor == 0) {
                    _factorials.Add(0, 1);
                } else {
                    factorial *= factor;
                    _factorials.Add(factor, factorial);
                }
            }
        }
    }
}
