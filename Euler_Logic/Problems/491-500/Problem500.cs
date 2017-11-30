using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem500 : ProblemBase {
        public override string ProblemName {
            get { return "500: Problem 500!!!"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private int Solve() {
            int num = 2;
            for (int count = 3; count <= 500500; count++) {
                num = (count * num) % 500500507;
            }
            return num;
        }
    }
}
