using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem173 : IProblem {
        public string ProblemName {
            get { return "173: Using up to one million tiles how many different 'hollow' square laminae can be formed?"; }
        }

        public string GetAnswer() {
            return Solve(1000000).ToString();
        }

        public ulong Solve(ulong max) {
            ulong sum = 0;
            for (ulong num = 1; num <= max / 4; num++) {
                ulong subSum = (num + 1) * 4;
                ulong nextNum = subSum;
                while (subSum <= max) {
                    sum += 1;
                    nextNum = ((nextNum / 4) + 2) * 4;
                    subSum += nextNum;
                }
            }
            return sum;
        }
    }
}
