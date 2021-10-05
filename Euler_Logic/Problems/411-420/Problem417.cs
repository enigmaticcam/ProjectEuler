using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem417 : ProblemBase {
        public override string ProblemName {
            get { return "417: Reciprocal cycles II"; }
        }

        public override string GetAnswer() {
            var x = Test(1000000);
            return "";
        }

        private ulong Test(ulong max) {
            ulong sum = 0;
            for (ulong num = 3; num <= max; num += 3) {
                if (num % 5 != 0 && num % 2 != 0) {
                    var result = GetChainSize(num);
                    sum += result;
                }
            }
            return sum;
        }

        private ulong GetChainSize(ulong num) {
            ulong count = 1;
            var remainder = 10 % num;
            while (remainder != 1) {
                remainder = (remainder * 10) % num;
                count++;
            }
            return count;
        }
    }
}
