using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem5 : IProblem {
        public string ProblemName {
            get { return "5: Smallest multiple"; }
        }

        public string GetAnswer() {
            return FindSmallestMultiple(20).ToString();
        }

        private int FindSmallestMultiple(int max) {
            int num = max;
            do {
                bool isGood = true;
                for (int i = 1; i <= max; i++) {
                    if (num % i != 0) {
                        isGood = false;
                        break;
                    }
                }
                if (isGood) {
                    return num;
                }
                num++;
            } while (true);
        }
    }
}
