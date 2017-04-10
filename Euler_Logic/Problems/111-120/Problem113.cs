using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem113 : ProblemBase {
        private int[,] _counts;

        public override string ProblemName {
            get { return "113: Non-bouncy numbers"; }
        }

        public override string GetAnswer() {
            Solve(6);
            return "done";
        }

        private void Solve(int digits) {
            FindMax(digits);
        }

        private void FindMax(int digits) {
            _counts = new int[digits, 10];
            for (int num = 0; num <= 9; num++) {
                _counts[1, num] = num;
            }
            
            for (int digit = 1; digit < digits; digit++) {

            }
        }
    }
}
