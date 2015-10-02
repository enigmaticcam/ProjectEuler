using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem66 : IProblem {
        private decimal _denominator;
        private decimal _numerator;

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

        private List<int> Multiply(List<int> product1, List<int> product2) {
            // Multiple digit by digit
            List<List<int>> adds = new List<List<int>>();
            for (int i = 0; i < product1.Count; i++) {
                List<int> add = new List<int>();
                for (int k = 0; k < i; k++) {
                    add.Add(0);
                }
                int remainder = 0;
                for (int j = 0; j < product2.Count; j++) {
                    int prod = product1[i] * product2[j] + remainder;
                    if (prod > 9) {
                        string prodAsString = prod.ToString();
                        prod = Convert.ToInt32(prodAsString.Substring(1, 1));
                        remainder = Convert.ToInt32(prodAsString.Substring(0, 1));
                    }
                    add.Add(prod);
                }
                if (remainder > 0) {
                    add.Add(remainder);
                }
                adds.Add(add);
            }

            List<int> finalReturn = new List<int>();
            foreach (int num in adds[0]) {
                finalReturn.Add(num);
            }
            for (int i = 1; i < adds.Count; i++) {
                int digitCount = Math.Max(adds[i].Count, finalReturn.Count);
                int remainder = 0;
                for (int j = 0; j < digitCount; j++) {
                    int sum = 0;
                    if (adds[i].Count - 1 < j) {
                        sum = finalReturn[j] + remainder;
                    } else if (finalReturn.Count - 1 < j) {
                        sum = adds[i][j] + remainder;
                        finalReturn.Add(0);
                    } else {
                        sum = adds[i][j] + finalReturn[j] + remainder;
                    }
                    if (sum > 9) {
                        string sumAsString = sum.ToString();
                        finalReturn[j] = Convert.ToInt32(sumAsString.Substring(1, 1));
                        remainder = Convert.ToInt32(sumAsString.Substring(0, 1));
                    } else {
                        finalReturn[j] = sum;
                        remainder = 0;
                    }
                }
                if (remainder > 0) {
                    finalReturn.Add(remainder);
                }
            }

            return finalReturn;
        }
    }
}
