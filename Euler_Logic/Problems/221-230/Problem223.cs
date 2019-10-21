using Euler_Logic.Helpers;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem223 : ProblemBase {
        private PrimeSieve _primes;

        /*
            -----------------------------------NEW METHOD-----------------------------------
            According to http://mathworld.wolfram.com/PythagoreanTriple.html, pythagorian triplets can be derived using a matrix transformation.
            Any single triplet can generate three more. It's only a question of what base triplet(s) to start from.
       
            -----------------------------------OLD METHOD-----------------------------------
            a^2 + b^2 = c^2 can be rewritten:
           
            a^2 + b^2 = c^2
            a^2 - 1 = c^2 - b^2
            a^2 - 1 = (c + b)(c - b)

            For all legal values of (a), factorize a^2-1 to find all of its divisors. For each set of divisors that equals a^2-1, find (c) and (b)
            using following rationalization given two divisors (d1, d2):

            b = (d1 - d2) / 2
            d1 = d2 - c

            Not very optimal, takes about 10-15 minutes. I prime sieve all primes up to 1 billion to make factorization more efficient.
         */

        public override string ProblemName {
            get { return "223: Almost right-angled triangles I"; }
        }

        public override string GetAnswer() {
            return Solve(25000000).ToString();
        }

        private ulong Solve(ulong maxP) {
            ulong count = 0;
            var list = new List<Triple>();
            list.Add(new Triple(1, 1, 1));
            list.Add(new Triple(1, 2, 2));
            do {
                var next = new List<Triple>();
                foreach (var tri in list) {
                    var newTri = new Triple(
                        a: 2 * tri.C + tri.B - 2 * tri.A,
                        b: 2 * tri.C + 2 * tri.B - tri.A,
                        c: 3 * tri.C + 2 * tri.B - 2 * tri.A);
                    if (newTri.P <= maxP) {
                        next.Add(newTri);
                    }
                    newTri = new Triple(
                        a: 2 * tri.C + tri.B + 2 * tri.A,
                        b: 2 * tri.C + 2 * tri.B + tri.A,
                        c: 3 * tri.C + 2 * tri.B + 2 * tri.A);
                    if (newTri.P <= maxP) {
                        next.Add(newTri);
                    }
                    if (tri.A != tri.B) {
                        newTri = new Triple(
                        a: 2 * tri.C - 2 * tri.B + tri.A,
                        b: 2 * tri.C - tri.B + 2 * tri.A,
                        c: 3 * tri.C - 2 * tri.B + 2 * tri.A);
                        if (newTri.P <= maxP) {
                            next.Add(newTri);
                        }
                    }
                }
                count += (ulong)list.Count;
                list = next;
            } while (list.Count > 0);
            return count;
        }

        private class Triple {
            public Triple() { }
            public Triple(ulong a, ulong b, ulong c) {
                A = a;
                B = b;
                C = c;
            }

            public ulong A { get; set; }
            public ulong B { get; set; }
            public ulong C { get; set; }
            public ulong P {
                get { return A + B + C; }


            }
        }
    }
}