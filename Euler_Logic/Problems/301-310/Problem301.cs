using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem301 : ProblemBase {

        public override string ProblemName {
            get { return "301: Nim"; }
        }

        public override string GetAnswer() {
            return Solve(30).ToString();
        }

        public int Solve(int maxPower) {
            int count = 0;
            int max = (int)Math.Pow(2, maxPower);
            for (int num = 1; num <= max; num++) {
                int bits = num | (num * 2);
                if ((bits ^ (num * 3)) == 0) {
                    count++;
                }
            }
            return count;
        }
    }
}
