using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem700 : ProblemBase {
        public override string ProblemName {
            get { return "700: Eulercoin"; }
        }

        public override string GetAnswer() {
            return BruteForce().ToString();
        }

        private ulong BruteForce() {
            ulong n = 1;
            ulong lowest = ulong.MaxValue;
            ulong sum = 0;
            do {
                var num = (1504170715041707 * n) % 4503599627370517;
                if (num < lowest) {
                    sum += num;
                    lowest = num;
                }
                n++;
            } while (true);
        }
    }
}
