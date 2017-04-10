using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem108 : ProblemBase {
        public override string ProblemName {
            get { return "108: Diophantine reciprocals I"; }
        }

        public override string GetAnswer() {
            return FindLeastValue(300).ToString();
        }

        private decimal FindLeastValue(decimal maxCount) {
            decimal number = 2;
            do {
                decimal count = GetCount(number);
                if (count > maxCount) {
                    return number;
                }
                number++;
            } while (true);
        }

        private List<string> _results = new List<string>();
        private decimal GetCount(decimal denominator) {
            decimal count = 0;
            decimal max = denominator + (denominator / 2);
            for (decimal num = denominator + 1; num <= max; num++) {
                if ((denominator * num) % (num - denominator) == 0) {
                    count++;
                }
            }
            return count;
        }

        private decimal LowestMultiple(decimal a, decimal b) {
            decimal num = a;
            while (num % b != 0) {
                num += a;
            }
            return num;
        }

        private decimal CountTest(decimal num) {
            decimal count = 2;
            for (decimal x = 2; x <= (decimal)Math.Sqrt((double)num); x++) {
                if (num % x == 0) {
                    count += 2;
                }
            }
            return count;
        }

    }
}
