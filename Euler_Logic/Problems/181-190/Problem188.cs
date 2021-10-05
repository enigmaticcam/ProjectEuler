using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem188 : ProblemBase {
        /*
            By this point, I already found an easy function that can quickly calculate the mod
            of an exponent, even when the exponent is very large. I don't quite know how it works,
            but it's easily fast enough to solve this problem.
         */

        public override string ProblemName {
            get { return "188: The hyperexponentiation of a number"; }
        }

        public override string GetAnswer() {
            return Solve(1777, 1855).ToString();
        }

        private ulong Solve(ulong root, int maxCount) {
            ulong mod = (ulong)Math.Pow(10, 8);
            ulong x = 1;
            for (int count = 1; count <= maxCount; count++) {
                x = Power.Exp(root, x, mod);
            }
            return x;
        }
    }
}