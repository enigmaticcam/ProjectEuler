using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Euler_Logic.Helpers;

namespace Euler_Logic.Problems {
    public class Problem66 : ProblemBase {
        private BigInteger _denominator;
        private BigInteger _numerator;

        public override string ProblemName {
            get { return "66: Diophantine equation"; }
        }

        public override string GetAnswer() {
            return FindLargestValue(1000).ToString();
        }

        private double FindLargestValue(double max) {
            double bestValue = 0;
            BigInteger bestScore = 0;
            for (double num = 2; num <= max; num++) {
                if (!IsSquare(num)) {
                    BigInteger minimum = FindMinimumValue(num);
                    if (minimum > bestScore) {
                        bestScore = minimum;
                        bestValue = num;
                    }
                }
            }
            return bestValue;
        }

        private BigInteger FindMinimumValue(double num) {
            double m = 0;
            double d = 1;
            double a = (int)Math.Sqrt(num);
            List<BigInteger> aAll = new List<BigInteger>();
            aAll.Add((BigInteger)a);
            do {
                m = (d * a) - m;
                d = (num - (m * m)) / d;
                a = (int)(((int)(Math.Sqrt(num)) + m) / d);
                aAll.Add((BigInteger)a);
                GetFraction(aAll, num);
                if ((_numerator * _numerator) - ((BigInteger)num * (_denominator * _denominator)) == 1) {
                    return _numerator;
                }
            } while (true);
        }

        private void GetFraction(List<BigInteger> a, double num) {
            _denominator = a[a.Count - 1];
            _numerator = 1;
            _numerator += _denominator * a[a.Count - 2];
            for (int index = a.Count - 2; index > 0; index--) {
                BigInteger temp = _numerator;
                _numerator = _denominator;
                _denominator = temp;
                _numerator += _denominator * a[index - 1];
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
