using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem700 : ProblemBase {
        /*
            I used an online calculator to find the moduler inverse between 1504170715041707 and 4503599627370517, which is 3451657199285664. Given this, we know that
            3451657199285664 * 1504170715041707 % 4503599627370517 = 1. We can continue adding the inverse until we find a number less than 1504170715041707, and then
            again and again. It would take too long to solve the problem this way though, so FindReasonable will continue to do this until we get some number that's
            reasonable, then we just bruteforce up to that number.
         */

        public override string ProblemName {
            get { return "700: Eulercoin"; }
        }

        public override string GetAnswer() {
            var x = FindReasonable(3451657199285664, 4503599627370517, 100000000);
            var y = FindRest(1504170715041707, 4503599627370517, x.Item2);
            return (x.Item1 + y).ToString();
        }

        private BigInteger FindRest(BigInteger num, BigInteger mod, ulong max) {
            BigInteger sum = num;
            BigInteger best = num;
            var current = num;
            for (ulong count = 2; count < max; count++) {
                current = (current + num) % mod;
                if (current < best) {
                    best = current;
                    sum += current;
                }
            }
            return sum;
        }
       

        private Tuple<ulong, ulong> FindReasonable(ulong inverse, ulong mod, ulong threshold) {
            ulong last = inverse;
            ulong num = inverse;
            ulong n = 1;
            ulong sum = 1;
            do {
                num += inverse;
                n++;
                if (num > mod) {
                    num %= mod;
                    if (num < last) {
                        sum += n;
                        if (num < threshold) return new Tuple<ulong, ulong>(sum, num);
                        last = num;
                    }
                }
            } while (true);
        }
    }
}
