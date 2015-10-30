using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem73 : IProblem {
        private int _numeratorBegin;
        private int _numeratorEnd;
        private int _denominatorBegin;
        private int _denominatorEnd;

        public string ProblemName {
            get { return "73: Counting fractions in a range"; }
        }

        public string GetAnswer() {
            _numeratorBegin = 1;
            _denominatorBegin = 3;
            _numeratorEnd = 1;
            _denominatorEnd = 2;
            return GetBetweenCount(12000).ToString();
        }

        private ulong GetBetweenCount(int max) {
            ulong count = 0;
            HashSet<int> seived = new HashSet<int>();
            for (int denominator = 2; denominator <= max; denominator++) {
                seived.Clear();
                for (int numerator = 1; numerator < denominator; numerator++) {
                    if (!seived.Contains(numerator)) {
                        if ((denominator % numerator != 0 || numerator == 1)
                            && IsFirstHigher(numerator, denominator, _numeratorBegin, _denominatorBegin)
                            && IsFirstHigher(_numeratorEnd, _denominatorEnd, numerator, denominator)) {
                            count++;
                        }
                        if (denominator % numerator == 0 && numerator != 1) {
                            int composite = numerator;
                            do {
                                seived.Add(composite);
                                composite += numerator;
                            } while (composite < denominator);
                        }
                    }
                }
            }
            return count;
        }



        private bool IsFirstHigher(int numerator1, int denominator1, int numerator2, int denominator2) {
            int commonDenominator = denominator1 * denominator2;
            numerator1 = (commonDenominator / denominator1) * numerator1;
            numerator2 = (commonDenominator / denominator2) * numerator2;
            if (numerator1 > numerator2) {
                return true;
            } else {
                return false;
            }
        }
    }
}
