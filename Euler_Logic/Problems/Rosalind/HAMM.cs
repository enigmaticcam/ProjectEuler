using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.Rosalind {
    public class HAMM : RosalindBase {
        public override string ProblemName => "Rosalind: DNA";

        public override string GetAnswer() {
            return Solve(Input()).ToString();
        }

        private int Solve(List<string> input) {
            int count = 0;
            for (int index = 0; index < input[0].Length; index++) {
                if (input[0][index] != input[1][index]) count++;
            }
            return count;
        }
    }
}
