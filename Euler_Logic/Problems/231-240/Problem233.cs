using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem233 : ProblemBase {
        public override string ProblemName {
            get { return "233: Lattice points on a circle"; }
        }

        public override string GetAnswer() {
            //Solve3();
            return Solve2(12).ToString();
            return Solve1(5).ToString();
            return "";
        }

        private void Solve3() {
            ulong n = 2;
            do {
                if (Solve2(n) == 420) {
                    bool stop = true;
                }
                n += 2;
            } while (true);
        }

        private ulong Solve2(ulong n) {
            //ulong Ax = 0;
            //ulong Ay = 0;
            //ulong Bx = 0;
            //ulong By = n;
            //ulong Cx = n;
            //ulong Cy = 0;

            //var d = 2 * (Ax * (By - Cy) + Bx * (Cy - Ay) + Cx * (Ay - By));
            //var Rx = ((Ax * Ax + Ay * Ay) * (By - Cy) + (Bx * Bx + By * By) * (Cy - Ay) + (Cx * Cx + Cy * Cy) * (Ay - By)) / d;
            //var Ry = ((Ax * Ax + Ay * Ay) * (Cx - Bx) + (Bx * Bx + By * By) * (Ax - Cx) + (Cx * Cx + Cy * Cy) * (Bx - Ax)) / d;
            //var rSquared = (Ax - Rx) * (Ax - Rx) + (Ay - Ry) * (Ay - Ry);
            var rSquared = n * (n / 2);
            return Solve1(rSquared);
        }

        private List<string> _nums = new List<string>();
        private ulong Solve1(ulong n) {
            ulong count = 0;
            ulong max = (ulong)Math.Sqrt(n);
            for (ulong x = 1; x <= max; x++) {
                var ySquared = n - x * x;
                var root = (ulong)Math.Sqrt(ySquared);
                if (root * root == ySquared) {
                    _nums.Add(x + "\t" + root);
                    count++;
                }
            }
            return count * 4;
        }
    }
}
