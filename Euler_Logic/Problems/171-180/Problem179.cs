using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem179 : IProblem {
        private int[] _divisorCount;

        public string ProblemName {
            get { return "179: Consecutive positive divisors"; }
        }

        public string GetAnswer() {
            return SieveDivisors((int)Math.Pow(10, 7)).ToString();
        }

        private int SieveDivisors(int max) {
            int sum = 0;
            _divisorCount = new int[max + 1];
            for (int num = 1; num <= max; num++) {
                for (int composite = 1; composite * num <= max; composite++) {
                    _divisorCount[composite * num] += 1;
                }
                if (_divisorCount[num - 1] == _divisorCount[num]) {
                    sum++;
                }
            }
            return sum;
        }
    }
}
