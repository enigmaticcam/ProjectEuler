using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem284 : ProblemBase {
        public override string ProblemName {
            get { return "284: Steady Squares"; }
        }

        public override string GetAnswer() {
            LookForSteadyS(8);
            return "";
        }

        private List<int> _steady = new List<int>();
        private void LookForSteadyS(int digitCount) {
            int min = (int)Math.Pow(14, digitCount - 1);
            int max = (int)Math.Pow(14, digitCount) - 1;
            for (int num = min; num <= max; num++) {
                var x = GetBase14(num, digitCount);
                var y = GetBase14(num * num, digitCount);
                bool isGood = true;
                for (int digit = 0; digit < digitCount; digit++) {
                    if (x[digit] != y[digit]) {
                        isGood = false;
                        break;
                    }
                }
                if (isGood) {
                    _steady.Add(num);
                }
            }
        }

        private int[] GetBase14(int num, int digitCount) {
            var maxDigits = (int)Math.Log(num, 14);
            var digits = new int[Math.Max(maxDigits + 1, digitCount)];
            var powerOf14 = (int)Math.Pow(14, maxDigits);
            for (int power = maxDigits; power >= 0; power--) {
                var divide = num / powerOf14;
                if (divide > 0) {
                    num -= divide * powerOf14;
                    digits[power] = divide;
                }
                powerOf14 /= 14;
            }
            return digits;
        }
    }
}
