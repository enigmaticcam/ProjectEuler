using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem97 : IProblem {
        public string ProblemName {
            get { return "97: Large non-Mersenne prime"; }
        }

        public string GetAnswer() {
            return CalcLargeNumber().ToString();
        }

        private ulong CalcLargeNumber() {
            ulong num = 2;
            for (ulong factor = 2; factor <= 7830457; factor++) {
                num = (num * 2) % 10000000000;
            }
            num = ((num * 28433) + 1) % 10000000000;
            return num;
        }
    }
}
