using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem10 : ProblemBase {

        public override string ProblemName {
            get { return "10: Summation of primes"; }
        }

        public override string GetAnswer() {
            return SievePrimes(2000000).ToString();
        }

        private double SievePrimes(double max) {
            double sum = 0;
            HashSet<double> numbers = new HashSet<double>();
            for (double num = 2; num <= max; num++) {
                if (!numbers.Contains(num)) {
                    sum += num;
                    double composite = num;
                    do {
                        numbers.Add(composite);
                        composite += num;
                    } while (composite <= max);
                }
            }
            return sum;
        }
    }
}
