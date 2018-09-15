using System;
using System.Numerics;

namespace Euler_Logic.Problems {
    public class Problem359 : ProblemBase {
        /*
            Prime factoring 71328803586048 yields 2^27 * 3^12, so it's easy to generate all divisors by looping through every
            possible power of 2 and 3.

            I ended up using BigInteger because I didn't want to use % all over the place and there are few enough divisor
            possibilities to worry about performance. The only issue is when floor is 1 and the room is an odd number, so
            just account for that.

            Anyways, after bruteforcing the first 10 floors for the first 10 rooms, some patterns emerge. The first
            person in any room is always:

            if r % 2 == 0 then n = (f / 2) * f
            if r % 2 == 1 then n = ((f - 1) / 2) * (f - 1) + (f - 1)

            Once you know n for the first room of a given floor, it's just a matter of calculating the pattern based on whether
            that floor is an odd or even number. As the rooms increase on a floor, the rate at which n increases for each floor 
            alternates between two interwoven patterns. The patterns differ slightly depending if you are on an odd or even floor.
            So effectively we just calculate answer for all divisors and return the sum.
         */

        public override string ProblemName {
            get { return "359: Hilbert's New Hotel"; }
        }

        public override string GetAnswer() {
            return FindDivisors().ToString();
        }

        private ulong FindDivisors() {
            ulong remainder = 100000000;
            ulong sum = 0;
            for (int two = 0; two <= 27; two++) {
                for (int three = 0; three <= 12; three++) {
                    ulong divisor = (ulong)Math.Pow(2, two) * (ulong)Math.Pow(3, three);
                    sum += (ulong)(GetN(divisor, 71328803586048 / divisor) % remainder);
                    sum %= remainder;
                }
            }
            return sum;
        }

        private BigInteger GetN(BigInteger f, BigInteger r) {
            if (f == 1) {
                return ((r * r) / 2) + (r / 2) + (r % 2 == 1 ? 1 : 0);
            }

            // Get n for first room of floor f
            BigInteger firstRoom = 0;
            if (f % 2 == 0) {
                firstRoom = (f / 2) * f;
            } else {
                firstRoom = ((f - 1) / 2) * (f - 1) + (f - 1);
            }

            BigInteger set1 = 0;
            BigInteger set2 = 0;
            if (f % 2 == 1) {

                // Get set 1
                set1 = (r / 2) * (r / 2);

                // Get set 2
                BigInteger a = ((r - 1) / 2) + f - 1;
                BigInteger b = a;
                a *= a;
                BigInteger c = (f - 1) * (f - 1) + (f - 1);
                set2 = a + b - c;

            } else {

                // Get set 1
                BigInteger a = (r - 1) / 2;
                set1 = a * a + a;

                // Get set 2
                a = (r / 2) + f;
                a *= a;
                BigInteger b = f * f;
                set2 = a - b;
            }
            return (firstRoom + set1 + set2);
        }
    }
}