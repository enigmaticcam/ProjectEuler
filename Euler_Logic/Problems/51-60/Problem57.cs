using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem57 : IProblem {
        private double _lastDenominator = 2;

        public string ProblemName {
            get { return "57: Square root convergents"; }
        }

        public string GetAnswer() {
            return GetTotalCount(1000).ToString();
        }

        private int GetTotalCount(int max) {
            int count = 0;
            int total = 0;
            double expansion = 0.5;
            double answer = 1.5;
            double denominator = 2;
            double numerator = 3;
            do {
                if (count == 800) {
                    bool stophere = true;
                }
                double reducedFactor = CanBeReduced(denominator, numerator);
                if (reducedFactor > 0 && StripDecimal((numerator / reducedFactor).ToString()).Length > StripDecimal((denominator / reducedFactor).ToString()).Length) {
                    total++;
                } else if (StripDecimal(numerator.ToString()).Length > StripDecimal(denominator.ToString()).Length) {
                    total++;
                }
                numerator = (answer + (double)2) * denominator;
                expansion = NextExpansion(expansion);
                answer = (double)1 + expansion;
                denominator = numerator / answer;

                count++;
            } while (count <= max);
            return total;
        }

        private string StripDecimal(string num) {
            if (num.IndexOf(".") == -1) {
                return num;
            } else {
                return num.Substring(0, num.IndexOf("."));
            }
        }

        private double CanBeReduced(double denominator, double numerator) {
            for (double factor = Math.Sqrt(denominator); factor >= 2; factor--) {
                if (denominator % factor == 0 && numerator % factor == 0) {
                    return factor;
                }
            }
            return 0;
        }

        private double NextExpansion(double num) {
            return 1 / (2 + num);
        }


    }
}
