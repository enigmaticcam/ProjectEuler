using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem57 : IProblem {
        private ulong _lastDenominator = 2;

        public string ProblemName {
            get { return "57: Square root convergents"; }
        }

        public string GetAnswer() {
            return GetTotalCount(1000).ToString();
        }

        private int GetTotalCount(int max) {
            int count = 1;
            int total = 0;
            ulong denominator = 2;
            ulong numerator = 3;
            ulong temp = 0;
            do {
                numerator += denominator;
                temp = numerator;
                numerator = denominator;
                denominator = temp;
                numerator += denominator;
                if (numerator.ToString().Length > denominator.ToString().Length) {
                    total++;
                }
                count++;
            } while (count < max);
            return total;
        }

        private int Stripulong(string num) {
            if (num.Contains("E+")) {
                return num.IndexOf(".") + Convert.ToInt32(num.Substring(num.IndexOf("E+") + 2, num.Length - num.IndexOf("E+") - 2));
            } else if (num.Contains(".")) {
                return num.Substring(0, num.IndexOf(".")).Length;
            } else {
                return num.Length;
            }
        }

        //private ulong CanBeReduced(ulong denominator, ulong numerator) {
        //    for (ulong factor = Math.Sqrt(denominator); factor >= 2; factor--) {
        //        if (denominator % factor == 0 && numerator % factor == 0) {
        //            return factor;
        //        }
        //    }
        //    return 0;
        //}

        private ulong NextExpansion(ulong num) {
            return 1 / (2 + num);
        }


    }
}
