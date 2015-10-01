using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem66 : IProblem {

        public string ProblemName {
            get { return "66: Diophantine equation"; }
        }

        public string GetAnswer() {
            return FindLargestValue(1000).ToString();
        }

        private double FindLargestValue(double max) {
            double bestValue = 0;
            double bestScore = 0;
            for (double num = 2; num <= max; num++) {
                if (!IsSquare(num)) {
                    double minimum = FindMinimumValue(num);
                    if (minimum > bestScore) {
                        bestScore = minimum;
                        bestValue = num;
                    }
                }
            }
            return bestValue;
        }

        private double FindMinimumValue(double D) {
            double y = 1;
            do {
                double num = D * (y * y) + 1;
                if (IsSquare(num)) {
                    return (double)Math.Sqrt(num);
                }
                y++;
            } while (true);
        }

        private bool IsSquare(double num) {
            double root = Math.Sqrt(num);
            if (root.ToString().Contains(".")) {
                return false;
            } else {
                return true;
            }
        }
    }
}
