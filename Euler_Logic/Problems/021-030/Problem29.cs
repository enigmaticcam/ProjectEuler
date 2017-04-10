using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem29 : ProblemBase {
        public override string ProblemName {
            get { return "29: Distinct powers"; }
        }

        public override string GetAnswer() {
            return GetDistinctCount(100).ToString();
        }

        private double GetDistinctCount(int max) {
            HashSet<double> counts = new HashSet<double>();
            for (int a = 2; a <= max; a++) {
                for (int b = 2; b <= max; b++) {
                    counts.Add(Math.Pow(a, b));
                }
            }
            return counts.Count;
        }
    }
}
