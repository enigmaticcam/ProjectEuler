using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem120 : IProblem {
        public string ProblemName {
            get { return "120: Square remainders"; }
        }

        public string GetAnswer() {
            return Solve().ToString();
        }

        private int Solve() {
            int sum = 0;
            for (int num = 3; num <= 1000; num++) {
                sum += FindHighest(num);
            }
            return sum;
        }

        private int FindHighest(int num) {
            HashSet<int> hash = new HashSet<int>();
            int remainder = 2;
            int highest = 2 * num;
            hash.Add(remainder);
            do {
                remainder = (remainder + 4) % num;
                if (hash.Contains(remainder)) {
                    return highest;
                } else {
                    hash.Add(remainder);
                    if (remainder * num > highest) {
                        highest = remainder * num;
                    }
                }
            } while (true);
        }
    }
}
