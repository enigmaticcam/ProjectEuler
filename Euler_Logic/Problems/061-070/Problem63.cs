using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem63 : ProblemBase {
        private bool _endOfCounting = false;

        public override string ProblemName {
            get { return "63: Powerful digit counts"; }
        }

        public override string GetAnswer() {
            int count = 0;
            int digits = 1;
            do {
                count += CountDigits(digits);
                digits += 1;
            } while (!_endOfCounting);
            return count.ToString();
        }

        private int CountDigits(int power) {
            int number = 1;
            int count = 0;
            if (power == 28) {
                _endOfCounting = true;
                return 0;
            }
            do {
                decimal result = Convert.ToDecimal(Math.Pow(number, power));
                if (result.ToString().Length > power) {
                    if (number == 2 && power != 1) {
                        _endOfCounting = true;
                    }
                    return count;
                } else if (result.ToString().Length == power) {
                    count++;
                }
                number++;
            } while (true);
        }
    }
}
