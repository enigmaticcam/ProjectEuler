using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem168 : ProblemBase {
        public override string ProblemName {
            get { return "168: Number Rotations"; }
        }

        public override string GetAnswer() {
            return BruteForce(10000000).ToString();
        }

        private int BruteForce(ulong max) {
            var found = new List<ulong>();
            ulong log = 10;
            ulong nextLog = 100;
            for (ulong num = 11; num <= max; num++) {
                if (num == nextLog) {
                    log = nextLog;
                    nextLog *= 10;
                }
                var remainder = num % 10;
                if (remainder != 0 && remainder >= num / log) {
                    var rotate = (remainder * log) + (num / 10);
                    if (rotate % num == 0) {
                        found.Add(num);
                    }
                }
            }
            return found.Count;
        }
    }
}
