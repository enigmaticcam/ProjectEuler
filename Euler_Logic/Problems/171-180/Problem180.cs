using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem180 : ProblemBase {
        public override string ProblemName {
            get { return "180: Rational zeros of a function of three variables"; }
        }

        public override string GetAnswer() {
            return "";
        }

        private void Test() {
            for (double x = 2; x <= 35; x++) {
                for (double y = 2; y <= 35; y++) {
                    for (double z = 2; z <= 35; z++) {
                        IsGoldenTriple(x, y, z);
                    }
                }
            }
        }

        List<string> _golden = new List<string>();
        private void IsGoldenTriple(double subX, double subY, double subZ) {
            var x = 1 / subX;
            var y = 1 / subY;
            var z = 1 / subZ;
            for (double n = -50; n <= 50; n++) {
                var f1 = Math.Pow(x, n + 1) + Math.Pow(y, n + 1) - Math.Pow(z, n + 1);
                var f2 = (x * y + y * z + z * x) * (Math.Pow(x, n - 1) + Math.Pow(y, n - 1) - Math.Pow(z, n - 1));
                var f3 = x * y * z * (Math.Pow(x, n - 2) + Math.Pow(y, n - 2) - Math.Pow(z, n - 2));
                var fn = f1 + f2 - f3;
                if (fn == 0 && (f1 != 0 || f2 != 0 || f3 != 0)) {
                    _golden.Add(n + ":" + subX + ":" + subY + ":" + subZ);
                    break;
                }
            }
        }
    }
}
