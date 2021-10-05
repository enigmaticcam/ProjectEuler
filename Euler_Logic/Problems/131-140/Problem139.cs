using Euler_Logic.Helpers;

namespace Euler_Logic.Problems {
    public class Problem139 : ProblemBase {
        private GCDULong _gcd = new GCDULong();

        /*
            Euclid's formula for generating integer right-triangles can be found here: https://en.wikipedia.org/wiki/Pythagorean_triple. So for each of these,
            all we need to do is check if the hypotenuse can be divided evenly by the hole (second-shortest side minus shortest side). If it can, then also
            it works for all of the other composite triangles that use a/b/c as a base. For example, if 3/4/5 works (because 5 % (4 - 3) == 0), then
            so does 6/8/10, 9/12/15 and so on. 
         */

        public override string ProblemName {
            get { return "139: Pythagorean tiles"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private ulong Solve() {
            ulong sum = 0;
            ulong n = 1;
            ulong m = 2;
            do {
                if ((n % 2 == 0 || m % 2 == 0) && _gcd.GCD(n, m) == 1) {
                    ulong a = (m * m) - (n * n);
                    ulong b = 2 * m * n;
                    ulong c = (m * m) + (n * n);
                    if (a + b + c < 100000000) {
                        if (c % ((a > b ? a : b) - (a > b ? b : a)) == 0) {
                            sum++;
                            ulong x = a * 2;
                            ulong y = b * 2;
                            ulong z = c * 2;
                            while (x + y + z < 100000000) {
                                sum += 1;
                                x += a;
                                y += b;
                                z += c;
                            }
                        }

                        m++;
                    } else if (m == n + 1) {
                        break;
                    } else {
                        n++;
                        m = n + 1;
                    }
                } else {
                    m++;
                }
            } while (true);
            return sum;
        }
    }
}