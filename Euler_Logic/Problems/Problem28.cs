using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem28 : IProblem {
        public string ProblemName {
            get { return "28: Number spiral diagonals"; }
        }

        public string GetAnswer() {
            return GetSpiralDiagonals(1001).ToString();
        }

        private decimal GetSpiralDiagonals(int maxSize) {
            int sum = 1;
            int digit = 1;
            for (int size = 3; size <= maxSize; size += 2) {
                for (int side = 1; side <= 4; side++) {
                    digit += size - 1;
                    sum += digit;
                }
            }
            return sum;
        }
    }
}
