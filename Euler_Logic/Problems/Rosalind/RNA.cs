using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.Rosalind {
    public class RNA : RosalindBase {
        public override string ProblemName => "Rosalind: RNA";

        public override string GetAnswer() {
            return Solve(Input());
        }

        private string Solve(List<string> input) {
            return input[0].Replace("T", "U");
        }
    }
}
