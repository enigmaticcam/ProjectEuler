using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem143 : ProblemBase {
        private List<Tuple> _triangles = new List<Tuple>();
        private HashSet<ulong> _valids = new HashSet<ulong>();

        /*
            After digging into various web articles on the Fermat point, I discovered
            that for any point (T) in a triangle that minimizes the distance between 
            the vertices (p, q, r), the angles at point (T) for each of the lines
            to the vertices are all exactly the same: 120 degrees. This means we are
            dealing with strictly triangles that have one point with exactly 120
            degrees.

            Interestingly, there is a way to generate integer triangles having one angle
            with 120 degrees. Given two integers (m) and (n) where GCD(m, n) = 1,
            m % 3 != n % 3, and (m) > (n) > 0, then a unique triangle can be generated:

            a = m^2 + mn + n^2
            b = 2mn + n^2
            c = m^2 - n^2

            Given this, what I do is start generating new triangles with valid values
            for (m) and (n). For each new triangle generated, I go back through the
            list of previous triangles and attempt to match sides. There are four
            different ways to match sides: b1 to b2, b1 to c2, c1 to b2, c1 to c2.
            I match a side by taking the lowest common multiple (lcm) of the two sides
            and increasing both triangles by that ratio. Now I just have to see if a
            triangle exists having two sides equal to the sides of the triangles not
            matched.

            This can be done using the law of cosines. I initialize ignored this rule
            since it used the cosine function, but I later found that cos(120) = -1/2,
            so that means it can still be all integers! The equation would then be:
            
            x^2 = a^2 + b^2 + ab

            I just have to check if x^2 is a perfect square.

            I do this until I can no longer generate triangles that fit the limit.
         */

        public override string ProblemName {
            get { return "143: Investigating the Torricelli point of a triangle"; }
        }

        public override string GetAnswer() {
            FindAllValids(120000);
            return Solve().ToString();
        }

        private void FindAllValids(ulong max) {
            ulong n = 1;
            bool finish = false;
            do {
                ulong m = n + 1;
                do {
                    while (GCD.GetGCD(n, m) != 1 || (n % 3) == (m % 3)) {
                        m++;
                    }
                    ulong a = m * m + m * n + n * n;
                    ulong b = 2 * m * n + n * n;
                    ulong c = m * m - n * n;
                    if (b + c > max) {
                        if (m == n + 1) {
                            finish = true;
                        }
                        break;
                    }
                    Match1to2(a, b, c, max);
                    _triangles.Add(new Tuple(a, b, c));
                    m++;
                } while (true);
                if (finish) {
                    break;
                }
                n++;
            } while (true);
        }

        private void Match1to2(ulong a, ulong b, ulong c, ulong max) {
            ulong finalA = 0;
            ulong finalB = 0;
            ulong newB = 0;
            ulong newC = 0;
            ulong lcm = 0;
            for (int index = 0; index < _triangles.Count; index++) {
                var tri = _triangles[index];

                // b-b
                lcm = LCM.GetLCM(b, tri.B);
                finalA = lcm / b * a;
                finalB = lcm / tri.B * tri.A;
                newB = lcm / b * c;
                newC = lcm / tri.B * tri.C;
                Match2to3(lcm, newB, newC, finalA, finalB, index, max);

                // b-c
                lcm = LCM.GetLCM(b, tri.C);
                finalA = lcm / b * a;
                finalB = lcm / tri.C * tri.A;
                newB = lcm / b * c;
                newC = lcm / tri.C * tri.B;
                Match2to3(lcm, newB, newC, finalA, finalB, index, max);

                // c-b
                lcm = LCM.GetLCM(c, tri.B);
                finalA = lcm / c * a;
                finalB = lcm / tri.B * tri.A;
                newB = lcm / c * b;
                newC = lcm / tri.B * tri.C;
                Match2to3(lcm, newB, newC, finalA, finalB, index, max);

                // c-c
                lcm = LCM.GetLCM(c, tri.C);
                finalA = lcm / c * a;
                finalB = lcm / tri.C * tri.A;
                newB = lcm / c * b;
                newC = lcm / tri.C * tri.B;
                Match2to3(lcm, newB, newC, finalA, finalB, index, max);
            }
        }

        private void Match2to3(ulong a, ulong b, ulong c, ulong finalA, ulong finalB, int lastIndex, ulong max) {
            ulong finalC = b * b + c * c + b * c;
            ulong root = (ulong)Math.Sqrt(finalC);
            if (root * root == finalC) {
                ulong composite = 1;
                ulong value = a + b + c;
                while (value <= max) {
                    _valids.Add(value);
                    composite++;
                    value = (a * composite) + (b * composite) + (c * composite);
                }
            }
        }

        private ulong Solve() {
            ulong sum = 0;
            foreach (var valid in _valids) {
                sum += valid;
            }
            return sum;
        }

        private class Tuple {
            public Tuple() { }
            public Tuple(ulong a, ulong b, ulong c) {
                A = a;
                B = b;
                C = c;
            }
            public ulong A { get; set; }
            public ulong B { get; set; }
            public ulong C { get; set; }
        }
    }
}