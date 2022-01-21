using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem03 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2016: 3"; }
        }

        public override string GetAnswer() {
            return Answer1().ToString();
        }

        public override string GetAnswer2() {
            return Answer2().ToString();
        }

        public int Answer1() {
            var triangles = GetTriangles1();
            int count = 0;
            triangles.ForEach(x => count += (IsTriangle(x) ? 1 : 0));
            return count;
        }

        public int Answer2() {
            var triangles = GetTriangles2();
            int count = 0;
            triangles.ForEach(x => count += (IsTriangle(x) ? 1 : 0));
            return count;
        }

        private bool IsTriangle(int[] tri) {
            return tri[0] + tri[1] > tri[2]
                && tri[0] + tri[2] > tri[1]
                && tri[1] + tri[2] > tri[0];
        }

        private List<int[]> GetTriangles1() {
            return Input().Select(x => {
                var tri = new int[3];
                tri[0] = Convert.ToInt32(x.Substring(0, 5).Trim());
                tri[1] = Convert.ToInt32(x.Substring(5, 5).Trim());
                tri[2] = Convert.ToInt32(x.Substring(10, 5).Trim());
                return tri;
            }).ToList();
        }

        private List<int[]> GetTriangles2() {
            var sets = Input();
            int index = 0;
            var tris = new List<int[]>();
            do {
                for (int col = 0; col <= 10; col += 5) {
                    var tri = new int[3];
                    tri[0] = Convert.ToInt32(sets[index].Substring(col, 5).Trim());
                    tri[1] = Convert.ToInt32(sets[index + 1].Substring(col, 5).Trim());
                    tri[2] = Convert.ToInt32(sets[index + 2].Substring(col, 5).Trim());
                    tris.Add(tri);
                }
                index += 3;
            } while (index < sets.Count - 1);
            return tris;
        }
    }
}
