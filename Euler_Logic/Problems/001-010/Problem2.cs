using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem2 : ProblemBase {
        public override string ProblemName {
            get { return "2: Even Fibonacci numbers"; }
        }

        public override string GetAnswer() {
            return CountEvenFibs(4000000).ToString();
        }

        private int CountEvenFibs(int terms) {
            int current = 2;
            int previous = 1;
            int count = 0;
            while (current <= terms) {
                if (current % 2 == 0) {
                    count += current;
                }
                current = current += previous;
                previous = current - previous;
            }
            return count;
        }
    }
}
