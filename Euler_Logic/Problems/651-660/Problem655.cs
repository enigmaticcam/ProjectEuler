using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem655 : ProblemBase {
        public override string ProblemName {
            get { return "655: Divisible Palindromes"; }
        }

        public override string GetAnswer() {
            return BruteForce().ToString();
            return "";
        }

        private void Test() {
            BigInteger num = 10000019;
            BigInteger temp = num;
            BigInteger powerOf10 = 1;
            double lastLog = 0;
            ulong count = 0;
            do {
                if (IsPalindrome(temp)) {
                    count++;
                }
                var lastDigit = temp % 10;
                var log = BigInteger.Log10(temp);
                while (log > lastLog) {
                    lastLog++;
                    powerOf10 *= 10;
                }
                var firstDigit = temp / powerOf10;
                

            } while (true);
        }

        private bool IsPalindrome(BigInteger num) {
            return false;
        }

        private List<ulong> _answers = new List<ulong>();
        private ulong BruteForce() {
            ulong num = 1;
            ulong max = ulong.MaxValue;
            ulong count = 0;
            ulong mod = 10000019;
            do {

                ulong sub = num;
                ulong full = num;
                ulong part = num / 10;
                while (sub > 0) {
                    var next = sub % 10;
                    full = full * 10 + next;
                    part = part * 10 + next;
                    sub /= 10;
                }
                if (full % mod == 0 && full <= max) {
                    count++;
                    _answers.Add(full);
                }
                if (part % mod == 0 && part <= max) {
                    count++;
                    _answers.Add(part);
                }
                if (part > max) {
                    break;
                }
                num++;
            } while (true);
            return count;
        }
    }
}
