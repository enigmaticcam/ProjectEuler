using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem565 : ProblemBase {
        private ulong[] _sums;
        private List<ulong> _found = new List<ulong>();

        public override string ProblemName {
            get { return "565: Divisibility of sum of divisors"; }
        }

        public override string GetAnswer() {
            return Solve(100000, 2017).ToString();
        }

        private ulong Solve(ulong max, ulong mod) {
            _sums = new ulong[max + 1];
            ulong sum = 0;
            for (ulong num = 1; num <= max; num++) {
                for (ulong composite = 1; composite * num <= max; composite++) {
                    _sums[composite * num] += num;
                }
                if (_sums[num] % mod == 0) {
                    sum += num;
                    _found.Add(num);
                }
            }
            return sum;
        }
    }
}
