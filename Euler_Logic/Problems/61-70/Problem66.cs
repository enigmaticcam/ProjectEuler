using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Euler_Logic.Helpers;

namespace Euler_Logic.Problems {
    public class Problem66 : IProblem {
        private decimal _denominator;
        private decimal _numerator;

        public string ProblemName {
            get { return "66: Diophantine equation"; }
        }

        public string GetAnswer() {
            return FindLargestValue(7).ToString();
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

        private double FindMinimumValue(double num) {
            double m = 0;
            double d = 1;
            double a = (int)Math.Sqrt(num);
            List<double> aAll = new List<double>();
            aAll.Add(a);
            do {
                m = (d * a) - m;
                d = (num - (m * m)) / d;
                a = (int)(((int)(Math.Sqrt(num)) + m) / d);
                aAll.Add(a);
                GetFraction(aAll, num);
                if ((_numerator * _numerator) - ((decimal)num * (_denominator * _denominator)) == 1) {
                    return (double)_numerator;
                }
            } while (true);
        }

        private void GetFraction(List<double> a, double num) {
            _denominator = (decimal)a[a.Count - 1];
            _numerator = 1;
            _numerator += _denominator * (decimal)a[a.Count - 2];
            for (int index = a.Count - 2; index > 0; index--) {
                decimal temp = _numerator;
                _numerator = _denominator;
                _denominator = temp;
                _numerator += _denominator * (decimal)a[index - 1];
            }
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
