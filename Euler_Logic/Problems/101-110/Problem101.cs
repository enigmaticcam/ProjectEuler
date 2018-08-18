using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem101 : ProblemBase {
        private List<ulong> _previous = new List<ulong>();
        private List<ulong> _increments = new List<ulong>() { 0 };
        public override string ProblemName {
            get { return "101: Optimum polynomial"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
        }

        /*
            Let's take a look at the equation from 1 to 4:

            1 - 1
            2 - 683
            3 - 44287
            4 - 838861

            You can reduce a polynomial by means of recursive subtractions, like so:

                        Recursion 1             Recution 2              Recursion 3
            1 - 1
            2 - 683     682=(683-1)
            3 - 44287   43604=(44287-683)       42922=(43604-682)
            4 - 838861  794574=(838861-44287)   750970=(794574-43604)   708048=(750970-42922)

            If this in fact were the full equation, then there would be no more subtraction recursion levels, and the next iteration 
            of recursion 3 would repeat. Assuming that, we can therefore infer the values for the previous recursions where n = 5 and 
            then infer the final value of the original equation for n + 1 by backtracking, like so:

            1 - 1
            2 - 683     682=(683-1)
            3 - 44287   43604=(44287-683)       42922=(43604-682)
            4 - 838861  794574=(838861-44287)   750970=(794574-43604)   708048=(750970-42922)
            5 - 3092453 2253592                 1459018                 708048=(duplicated from above)

            If we calculate by means of the equation n = 5, we get: 8138021. Since this is a different value, we know it's incorrect. Add
            3092453 to our sum, and continue until our inferred value for n + 1 actually equals n + 1;
         */

        private ulong Solve() {
            int n = 1;
            ulong last = 0;
            ulong sum = 1;
            ulong next = 1;
            do {
                _previous.Add(next);
                if (n > 1) {
                    last = Resolve(n, n);
                    if (last != next) {
                        sum += last;
                    }
                }
                n++;
                next = NextN(n);
            } while (last != next);
            return sum - last;
        }

        private ulong Resolve(int max, int n) {
            if (max == n) {
                _increments.Add(0);
                for (int index = 0; index < _previous.Count; index++) {
                    _increments[index] = _previous[index];
                }
                return Resolve(max, n - 1) + _previous[n - 1];
            } else {
                for (int index = 0; index < n; index++) {
                    _increments[index] = _increments[index + 1] - _increments[index];
                }
                if (n == 1) {
                    return _increments[0];
                } else {
                    ulong last = _increments[n - 1];
                    ulong next = Resolve(max, n - 1);
                    return last + next;
                }
            }
        }

        private ulong NextN(int n) {
            return
                1
                - (ulong)n
                + (ulong)Math.Pow(n, 2)
                - (ulong)Math.Pow(n, 3)
                + (ulong)Math.Pow(n, 4)
                - (ulong)Math.Pow(n, 5)
                + (ulong)Math.Pow(n, 6)
                - (ulong)Math.Pow(n, 7)
                + (ulong)Math.Pow(n, 8)
                - (ulong)Math.Pow(n, 9)
                + (ulong)Math.Pow(n, 10);
        }

        private ulong NextNTest(int n) {
            return (ulong)Math.Pow(n, 3);
        }
    }
}