using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem94 : IProblem {
        public string ProblemName {
            get { return "94: Almost equilateral triangles"; }
        }

        public string GetAnswer() {
            Solve();
            return "";
        }

        private void Solve() {
            double num = 3;
            do {


                num += 2;
            } while (num * 2 + num + 1 <= 1000000000);
        }
    }
}
