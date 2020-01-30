using Euler_Logic.Helpers;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem250 : ProblemBase {
        /*
            This problem can be solved quite similarly to problem 249 using a DP solution. First we convert the entire set
            of 1^1, 2^2, 3^3, etc to remainders when divded by 250. Then we look through each number in the set, and for
            each number loop through all possible remainders 0-249. We want to determine how many possible ways there are
            to reach each possible remainder given all possible numbers in the set, first starting with just one number i
            the set, then the next two numbers, and so on.
         */

        public override string ProblemName {
            get { return "250: 250250"; }
        }

        public override string GetAnswer() {
            GetSet();
            return Solve().ToString();
        }

        private List<int> _set;
        private void GetSet() {
            _set = new List<int>();
            for (ulong num = 1; num <= 250250; num++) {
                _set.Add((int)Power.Exp(num, num, 250));
            }
        }

        private ulong Solve() {
            ulong mod = 10000000000000000;
            var sums = new ulong[2][];
            sums[1] = new ulong[250];
            foreach (var num in _set) {
                sums[0] = sums[1];
                sums[1] = new ulong[250];
                for (int max = 0; max < 250; max++) {
                    if (max == num) {
                        sums[1][max] += 1;
                    }
                    var result = max - num;
                    if (result < 0) {
                        result += 250;
                    }
                    sums[1][max] += sums[0][result] + sums[0][max];
                    sums[1][max] = sums[1][max] % mod;
                }
            }
            return sums[1][0];
        }


    }
}