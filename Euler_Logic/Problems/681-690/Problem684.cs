using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem684 : ProblemBase {
        public override string ProblemName {
            get { return "684: Inverse Digit Sum"; }
        }

        private ulong _mod;
        public override string GetAnswer() {
            _mod = 1000000007;
            return Solve().ToString();
            return "";
        }

        private ulong DigitSum(ulong n) {
            ulong sum = 0;
            while (n != 0) {
                sum += n % 10;
                n /= 10;
            }
            return sum;
        }

        private ulong s(ulong n) {
            if (n < 10) {
                return n;
            }
            //ulong count = n / 9;
            //ulong num = n - (count * 9);
            //for (ulong next = 1; next <= count; next++) {
            //    num *= 10;
            //    num += 9;
            //}
            //return num;
            ulong count = n / 9;
            ulong remainder = n - (count * 9);
            ulong num = Power.Exp(10, count, _mod);
            num += num * remainder;
            if (num == 0) {
                num = _mod;
            } else {
                num -= 1;
            }
            return num;
        }

        private ulong Test(ulong n) {
            ulong num = 1;
            do {
                if (DigitSum(num) == n) {
                    return num;
                }
                num++;
            } while (true);
        }

        private ulong _maxNumber;
        private ulong _maxSum;
        private ulong S(ulong n) {
            for (; _maxNumber <= n; _maxNumber++) {
                _maxSum += s(n);
            }
            return _maxSum;
        }

        private ulong Solve() {
            ulong num1 = 0;
            ulong num2 = 1;
            ulong total = 0;
            for (int count = 2; count <= 90; count++) {
                num2 += num1;
                num1 = num2 - num1;
                total = (total + S(num2)) % _mod;
            }
            return total;
        }
    }
}
