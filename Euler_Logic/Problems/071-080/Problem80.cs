using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem80 : IProblem {

        public string ProblemName {
            get { return "80: Square root digital expansion"; }
        }

        public string GetAnswer() {
            return FindTotalSum().ToString();
        }

        private int FindTotalSum() {
            int sum = 0;
            for (double number = 1; number <= 100; number++) {
                double root = Math.Sqrt(number);
                if (root.ToString().Contains(".")) {
                    sum += SumOfDigits(GenerateSquareRoot(number.ToString(), 100));
                }
            }
            return sum;
        }

        private string GenerateSquareRoot(string number, int maxDigits) {
            int point = number.IndexOf('.');
            if (point == -1) {
                point = number.Length;
            } else if ((number.Length - point - 1) % 2 != 0) {
                number += "0";
            }
            if (point % 2 != 0) {
                number = "0" + number;
                point += 1;
            }
            number = number.Replace(".", "");

            BigInteger twenty = 20;
            BigInteger hundred = 100;
            string digits = "";
            BigInteger c = Convert.ToInt32(number.Substring(0, 2));
            int index = 2;
            BigInteger p = 0;
            BigInteger x = FindGreatestX(p, c);
            digits += x.ToString();
            BigInteger y = x * ((twenty * p) + x);
            if (number.Length - 1 <= index) {
                c = (c - y) * hundred;
            } else {
                c = Convert.ToInt32(number.Substring(index, 2)) + ((c - y) * hundred);
            }
            while (c != 0 && digits.Length - point < maxDigits) {
                index += 2;
                p = BigInteger.Parse(digits);
                x = FindGreatestX(p, c);
                digits += x.ToString();
                y = x * ((twenty * p) + x);
                if (number.Length - 1 <= index) {
                    BigInteger temp = (c - y) * hundred;
                    c = (c - y) * hundred;
                } else {
                    c = BigInteger.Parse(number.Substring(index, 2)) + ((c - y) * hundred);
                }
            }
            return digits.Substring(0, 100);
        }

        private int SumOfDigits(string digits) {
            int sum = 0;
            for (int index = 0; index < digits.Length; index++) {
                sum += Convert.ToInt32(digits.Substring(index, 1));
            }
            return sum;
        }

        private BigInteger FindGreatestX(BigInteger p, BigInteger c) {
            BigInteger twenty = 20;
            for (BigInteger digit = 0; digit <= 9; digit++) {
                BigInteger y = digit * ((twenty * p) + digit);
                if (y == c) {
                    return digit;
                } else if (y > c) {
                    return digit - 1;
                }
            }
            return 9;
        }
    }
}
