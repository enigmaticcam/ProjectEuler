using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem71 : ProblemBase {
        public override string ProblemName {
            get { return "71: Ordered Fractions"; }
        }

        public override string GetAnswer() {
            return GetFraction(1000000).ToString();
        }

        private decimal GetFraction(decimal max) {
            Fraction answer = new Fraction(3, 7);
            Fraction best = null;
            for (ulong denominator = 3; denominator <= max; denominator++) {
                if (denominator != answer.Denominator) {
                    ulong numerator = (answer.Numerator * denominator / answer.Denominator);
                    if (best == null) {
                        best = new Fraction(numerator, denominator);
                    } else {
                        if (IsFirstHigher(numerator, denominator, best.Numerator, best.Denominator) 
                            && IsFirstHigher(answer.Numerator, answer.Denominator, numerator, denominator)) {
                            
                            best.Numerator = numerator;
                            best.Denominator = denominator;
                        }
                    }
                }
            }
            ReduceFraction(best);
            return best.Numerator;
        }

        private bool IsFirstHigher(ulong numerator1, ulong denominator1, ulong numerator2, ulong denominator2) {
            ulong commonDenominator = denominator1 * denominator2;
            numerator1 = (commonDenominator / denominator1) * numerator1;
            numerator2 = (commonDenominator / denominator2) * numerator2;
            if (numerator1 > numerator2) {
                return true;
            } else {
                return false;
            }
        }

        private void ReduceFraction(Fraction fraction) {
            ulong num = (ulong)Math.Sqrt((double)fraction.Denominator);
            while (num > 1) {
                if (fraction.Denominator % num == 0 && fraction.Numerator % num == 0) {
                    fraction.Denominator = fraction.Denominator / num;
                    fraction.Numerator = fraction.Numerator / num;
                    break;
                }
                num--;
            }
        }

        private class Fraction {
            public ulong Numerator { get; set; }
            public ulong Denominator { get; set; }

            public Fraction(ulong numerator, ulong denominator) {
                this.Numerator = numerator;
                this.Denominator = denominator;
            }
        }
    }
}
