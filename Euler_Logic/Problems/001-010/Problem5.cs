using Euler_Logic.Helpers;

namespace Euler_Logic.Problems {
    public class Problem5 : ProblemBase {
        private LCMULong _lcm = new LCMULong();

        /*
            The smallest possible number that is evenly divisible by all numbers 1 to 20 is the lowest common multiple of all numbers 1 to 20.
            The LCM of any two numbers x and y is: x / GCD(x, y) * y. To find the GCD of any two numbers, we simply divide the larger by the smaller,
            set the larger to be the remainder, and continue until we get zero.

            Simply: LCM(1-20) = LCM(20, LCM(19, LCM(18, ..., LCM(2))))
         */

        public override string ProblemName {
            get { return "5: Smallest multiple"; }
        }

        public override string GetAnswer() {
            return Solve(20).ToString();
        }

        private ulong Solve(ulong max) {
            ulong lcm = max;
            for (ulong num = max - 1; num >= 2; num--) {
                lcm = _lcm.LCM(num, lcm);
            }
            return lcm;
        }
    }
}