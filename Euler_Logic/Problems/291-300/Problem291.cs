using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem291 : ProblemBase {
        public override string ProblemName {
            get { return "291: Panaitopol Primes"; }
        }

        public override string GetAnswer() {
            Test();
            return "";
        }

        private void Test() {
            for (ulong x = 1; x <= 50; x++) {
                for (ulong y = 1; y <= x; y++) {
                    var xPower3 = x * x * x;
                    var yPower3 = y * y * y;
                    var top = (xPower3 * x) - (yPower3 * y);
                    var bottom = xPower3 + yPower3;
                    if (top % bottom == 0) {
                        var result = top / bottom;
                        if (result != 0) {
                            bool stop = true;
                        }
                        
                    }
                }
            }
        }
    }
}
