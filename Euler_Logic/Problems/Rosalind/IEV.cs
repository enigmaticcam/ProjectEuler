using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.Rosalind {
    public class IEV : RosalindBase {
        public override string ProblemName => "Rosalind: IEV";

        public override string GetAnswer() {
            return Solve(Input()).ToString();
        }

        private decimal Solve(List<string> input) {
            var counts = GetCounts(input[0]);
            decimal sum = 0;
            sum += counts[0] * 1;
            sum += counts[1] * 1;
            sum += counts[2] * 1;
            sum += counts[3] * (decimal)0.75;
            sum += counts[4] * (decimal)0.5;
            return sum * 2;
        }

        private decimal[] GetCounts(string input) {
            var split = input.Split(' ');
            return split.Select(x => Convert.ToDecimal(x)).ToArray();
        }
    }
}
