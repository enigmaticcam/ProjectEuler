using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem21 : ProblemBase {

        public override string ProblemName {
            get { return "21: Amicable numbers"; }
        }

        public override string GetAnswer() {
            return CountAmicables().ToString();
        }

        private int CountAmicables() {
            int sum = 0;
            for (int i = 1; i < 10000; i++) {
                int divisorSum = GetDivisorSum(i);
                int other = GetDivisorSum(divisorSum);
                if (i != divisorSum && other == i) {
                    sum += i;
                }
            }
            return sum;
        }

        private int GetDivisorSum(int num) {
            int sum = 0;
            for (int a = 1; a <= Math.Sqrt(num); a++) {
                if (num % a == 0) {
                    if (a == Math.Sqrt(num) || a == 1) {
                        sum += a;
                    } else {
                        sum += a;
                        sum += num / a;
                    }
                }
            }
            return sum;
        }
    }
}
