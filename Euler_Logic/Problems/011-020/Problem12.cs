using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem12 : ProblemBase {
        public override string ProblemName {
            get { return "12: Highly divisible triangular number"; }
        }

        public override string GetAnswer() {
            return FindMostFactors(500).ToString();
        }

        private double FindMostFactors(double factorCount) {
            double num = 0;
            double count = 0;
            do {
                num += 1;
                count += num;
                double numFactorCount = GetFactorCount(count);
                if (numFactorCount > factorCount) {
                    return count;
                }
            } while (true);
        }

        private double GetFactorCount(double num) {
            if (num == 36) {
                bool stophere = true;
            }
            double count = 0;
            for (double i = 1; i <= Math.Sqrt(num); i++) {
                if (num % i == 0) {
                    if (i == Math.Sqrt(num)) {
                        count += 1;
                    } else {
                        count += 2;
                    }
                }
            }
            return count;
        }
    }
}
