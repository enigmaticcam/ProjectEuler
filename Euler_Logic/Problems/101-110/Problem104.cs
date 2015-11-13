using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem104 : IProblem {
        double _phi = (1 + Math.Sqrt(5)) / 2;

        public string ProblemName {
            get { return "104: Pandigital Fibonacci ends"; }
        }

        public string GetAnswer() {
            return FindPandigitalFib().ToString();
        }

        private int FindPandigitalFib() {
            int index = 2;
            int lastTenDigits = 1;
            int lastTenDigitsPrev = 1;
            do {
                index++;
                lastTenDigits = lastTenDigitsPrev + lastTenDigits;
                lastTenDigitsPrev = lastTenDigits - lastTenDigitsPrev;
                lastTenDigits = lastTenDigits % 1000000000;
                if (IsPandigital(lastTenDigits.ToString()) && IsPandigital(GetFirstNineDigits(index))) {
                    return index;
                }                
            } while (true);
        }

        private bool IsPandigital(string number) {
            for (int digit = 1; digit <= 9; digit++) {
                if (number.Replace(digit.ToString(), "").Length != number.Length - 1) {
                    return false;
                }
            }
            return true;
        }

        private string GetFirstNineDigits(double index) {
            double log = (index * Math.Log10(_phi)) - Math.Log10(Math.Sqrt(5));
            double number = Math.Pow(10, log - (ulong)log + 8);
            return ((ulong)number).ToString();
        }

        
    }
}
