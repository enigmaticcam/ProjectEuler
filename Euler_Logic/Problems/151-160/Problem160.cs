using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem160 : ProblemBase {
        private Dictionary<ulong, ulong> _primeFactors = new Dictionary<ulong, ulong>();

        public override string ProblemName {
            get { return "160: Factorial trailing digits"; }
        }

        public override string GetAnswer() {
            //ulong max = 20;
            ulong max = 1000000000000;
            return Solve(max).ToString();
            return "";
        }

        private ulong Solve(ulong max) {
            ulong result = 1;
            var beyond = max / 100000;
            for (ulong num = 1; num <= 99999; num++) {
                if (num % 10 != 0) {
                    var temp = num;
                    while (temp <= max) {
                        result *= num;
                        while (result % 10 == 0) {
                            result /= 10;
                        }
                        result %= 100000;
                        temp *= 10;
                    }
                    result *= Power.Exp(num, beyond, 100000);
                    result %= 100000;
                }
            }
            return result;
        }

        

        private ulong BruteForce(ulong n) {
            ulong num = 1;
            for (ulong next = 2; next <= n; next++) {
                num *= next;
                while (num % 10 == 0) {
                    num /= 10;
                }
                num %= 100000;
            }
            return num;
        }
    }
}
