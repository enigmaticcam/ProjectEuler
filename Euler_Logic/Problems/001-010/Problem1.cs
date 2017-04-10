using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem1 : ProblemBase {
        public override string ProblemName {
            get { return "1: Multiples of 3 and 5"; }
        }

        public override string GetAnswer() {
            return CountMultiples(1000).ToString();
        }

        private int CountMultiples(int max) {
            int count = 0;
            for (int i = 1; i < max; i++) {
                if (i % 3 == 0 || i % 5 == 0) {
                    count += i;
                }
            }
            return count;
        }
    }
}
