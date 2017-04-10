using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem211 : ProblemBase {
        private ulong[] _counts;
        private HashSet<ulong> _squares = new HashSet<ulong>();

        public override string ProblemName {
            get { return "211: Divisor Square Sum"; }
        }

        public override string GetAnswer() {
            ulong max = 42;
            BuildSquares(max);
            _counts = new ulong[max + 1];
            return Solve(max).ToString();
        }

        private void BuildSquares(ulong max) {
            ulong number = 1;
            do {
                _squares.Add(number * number);
                number++;
            } while (number * number <= max);
        }

        private ulong Solve(ulong max) {
            ulong sum = 0;
            for (ulong divisor = 2; divisor < max; divisor++) {
                for (ulong composite = 1; composite * divisor <= max; composite++) {
                    _counts[composite * divisor] += divisor * divisor;
                }
                if (_squares.Contains(_counts[divisor] + 1)) {
                    sum += _counts[divisor] + 1;
                }
            }
            return sum;
        }
    }
}
