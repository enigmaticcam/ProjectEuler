using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.Rosalind {
    public class DNA : RosalindBase {
        public override string ProblemName => "Rosalind: DNA";

        public override string GetAnswer() {
            return Solve(Input());
        }

        private string Solve(List<string> input) {
            var counts = new int[4];
            counts[0] = input[0].Length - input[0].Replace("A", "").Length;
            counts[1] = input[0].Length - input[0].Replace("C", "").Length;
            counts[2] = input[0].Length - input[0].Replace("G", "").Length;
            counts[3] = input[0].Length - input[0].Replace("T", "").Length;
            return string.Join(" ", counts);
        }
    }
}
