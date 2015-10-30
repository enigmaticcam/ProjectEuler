using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem6 : IProblem {
        public string ProblemName {
            get { return "6: Sum square difference"; }
        }

        public string GetAnswer() {
            return FindSumSquareDiff(100).ToString();
        }

        private decimal FindSumSquareDiff(decimal count) {
            decimal sumOfSquare = 0;
            decimal squareOfSum = 0;
            for (decimal i = 1; i <= count; i++) {
                sumOfSquare += i * i;
                squareOfSum += i;
            }
            squareOfSum = squareOfSum * squareOfSum;
            return squareOfSum - sumOfSquare;
        }
    }
}
