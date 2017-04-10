using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem94 : ProblemBase {
        private ulong _sum = 0;

        public override string ProblemName {
            get { return "94: Almost equilateral triangles"; }
        }

        public override string GetAnswer() {
            Solve(1000000000);
            return _sum.ToString();
        }

        //private void Solve(double max) {
        //    double num = 3;
        //    do {
        //        TryTriangle(num, num, num + 1, max);
        //        TryTriangle(num, num, num - 1, max);
        //        num += 2;
        //    } while (num * 2 + num - 1 <= max);
        //}

        //private void TryTriangle(double a, double b, double c, double max) {
        //    double p = (a + b + c) / 2;
        //    double test = p * (p - a) * (p - b) * (p - c);
        //    double area = (ulong)Math.Sqrt(test);

        //    if (area * area == test) {
        //        ulong sum = (ulong)(a + b + c);
        //        if (sum <= max) {
        //            _sum += sum;
        //        }
        //    }
        //}

        private void Solve(ulong max) {
            ulong p = 16;
            do {

            } while (p <= max);
        }
    }
}
