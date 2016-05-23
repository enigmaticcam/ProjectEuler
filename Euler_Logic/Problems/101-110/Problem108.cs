using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem108 : IProblem {
        public string ProblemName {
            get { return "108: Diophantine reciprocals I"; }
        }

        public string GetAnswer() {
            return GetCount(360).ToString();
            return FindLeastValue(11).ToString();
        }

        private decimal FindLeastValue(decimal maxCount) {
            decimal number = 2;
            do {
                //decimal count = GetCount(number);
                decimal count = CountTest(number);
                if (count > maxCount) {
                    return number;
                }
                number++;
            } while (true);
        }

        private List<string> _results = new List<string>();
        private decimal GetCount(decimal denominator) {
            decimal count = 0;
            for (decimal num = denominator + 1; num <= denominator * 2; num++) {
                decimal multiple = LowestMultiple(num, denominator);
                decimal reduced = (multiple / denominator) - 1;
                if (multiple % reduced == 0) {
                    count++;
                    //_results.Add(denominator + ":" + (num - denominator).ToString());
                    _results.Add(denominator + ":" + num);
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
