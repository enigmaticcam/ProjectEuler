using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.Rosalind {
    public class IPRB : RosalindBase {
        public override string ProblemName => "Rosalind: IPRB";

        public override string GetAnswer() {
            return Solve(Input());
        }

        private string Solve(List<string> input) {
            var counts = GetCounts(input[0]);
            decimal ratio = 0;
            ratio += (counts.HomoRecessive / counts.Sum) * ((counts.HomoRecessive - 1) / (counts.Sum - 1));
            ratio += ((counts.HomoRecessive / counts.Sum) * (counts.Hetero / (counts.Sum - 1)) * (decimal)0.5) * 2;
            ratio += (counts.Hetero / counts.Sum) * ((counts.Hetero - 1) / (counts.Sum - 1)) * (decimal)0.25;
            return (1 - ratio).ToString();
        }

        private Counts GetCounts(string input) {
            var split = input.Split(' ');
            var counts = new Counts() {
                HomoDominant = Convert.ToDecimal(split[0]),
                Hetero = Convert.ToDecimal(split[1]),
                HomoRecessive = Convert.ToDecimal(split[2])
            };
            counts.Sum = counts.HomoDominant + counts.HomoRecessive + counts.Hetero;
            return counts;
        }

        private class Counts {
            public decimal HomoDominant { get; set; }
            public decimal HomoRecessive { get; set; }
            public decimal Hetero { get; set; }
            public decimal Sum { get; set; }
        }
    }
}
