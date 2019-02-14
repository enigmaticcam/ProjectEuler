using System;

namespace Euler_Logic.Problems {
    public class Problem138 : ProblemBase {
        /*
            A quick brute force algorithm yields the first few triangles that satisfy the rule.
            Those are:

            (b,l,h)
            16,17,15
            272,305,273
            4896,5473,4895
            87840,98209,87841

            You can generate unique integer based right triangles where (m,n) are coprime and m > n: 
            a = m^2 - n^2
            b = 2mn
            c = m^2 + n^2

            I found that for each valid triangle, the (m,n) values used to generate it are as
            follows:

            (b,l,h) - (m,n)
            16,17,15 - 4,1
            272,305,273 - 17,4
            4896,5473,4895 - 72,17
            87840,98209,87841 - 305,72

            It can be observed that for each iteration, (m) roughly quadruples while (n) becomes
            the prior (m). So my algorithm starts with (m,n) = (4,1) and for each subsequent
            iteration:

            1. n = m
            2. m = 4m
            3. Find a, b, c
            4. Determine if the resulting triangle follows the rule
            5. If not, continue to increase m by 1 until it does
            6. Go back to step 1
         */

        public override string ProblemName {
            get { return "138: Special isosceles triangles"; }
        }

        public override string GetAnswer() {
            return Solve(12).ToString();
        }

        private ulong Solve(int total) {
            ulong sum = 0;
            int count = 0;
            ulong m = 4;
            ulong n = 1;
            do {
                var a = m * m - n * n;
                var b = m * n * 2;
                var c = m * m + n * n;
                var lower = Math.Min(a, b);
                var higher = Math.Max(a, b);
                if (higher - (lower * 2) == 1 || (lower * 2) - higher == 1) {
                    n = m;
                    m *= 4;
                    sum += c;
                    count++;
                    if (count == total) {
                        return sum;
                    }
                }
                m++;
            } while (true);
        }
    }
}
