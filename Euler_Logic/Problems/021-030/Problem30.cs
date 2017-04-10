using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem30 : ProblemBase {
        public override string ProblemName {
            get { return "30: Digit fifth powers"; }
        }

        public override string GetAnswer() {
            return GetDigitPowers(5).ToString();
        }

        private double GetDigitPowers(double power) {
            double number = 10;
            double sum = 0;
            double total = 0;
            int digitCount = 0;
            double max = 0;
            do {
                string numberAsString = number.ToString();
                if (numberAsString.Length > digitCount) {
                    digitCount = numberAsString.Length;
                    max = GetMax(digitCount, power);
                }
                sum = GetPowerSum(numberAsString, power);
                if (sum == number) {
                    total += number;
                }
                number++;
            } while (number <= max);
            return total;
        }

        private double GetPowerSum(string number, double power) {
            double total = 0;
            for (int index = 0; index < number.Length; index++) {
                double digit = Convert.ToDouble(number.Substring(index, 1));
                total += Math.Pow(digit, power);
            }
            return total;
        }

        private double GetMax(double digitCount, double power) {
            double sum = 0;
            for (int digit = 1; digit <= digitCount; digit++) {
                sum += Math.Pow(9, power);
            }
            return sum;
        }
    }
}
