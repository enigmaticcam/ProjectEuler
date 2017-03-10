using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem407 : IProblem {
        public string ProblemName {
            get { return "407: Idempotents"; }
        }

        public string GetAnswer() {
            return Solve().ToString();
        }

        private ulong Solve() {
            ulong sum = 0;
            for (ulong num = 2; num <= 100; num++) {
                ulong max = 0;
                for (ulong divisor = 1; divisor <= num / 2; divisor++) {
                    ulong result = ((divisor % num) * (divisor % num)) % num;
                    if (result > max) {
                        if (result == num - 1) {
                            max = result;
                            break;
                        } else {
                            max = result;
                        }
                    }
                }
                sum += max;
            }
            return sum;
        }
    }
}
