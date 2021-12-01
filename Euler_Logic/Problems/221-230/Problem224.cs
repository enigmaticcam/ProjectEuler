using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem224 : ProblemBase {

        /*
            According to http://mathworld.wolfram.com/PythagoreanTriple.html, pythagorian triplets can be derived using a matrix transformation.
            Any single triplet can generate three more. It's only a question of what base triplet(s) to start from. I randomly guess 2,2,3 and
            that worked!
         */

        public override string ProblemName {
            get { return "224: Almost right-angled triangles II"; }
        }

        public override string GetAnswer() {
            return Solve(75000000).ToString();
        }

        private ulong Solve(ulong maxP) {
            ulong count = 0;
            var list = new List<Triple>();
            list.Add(new Triple(2, 2, 3));
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