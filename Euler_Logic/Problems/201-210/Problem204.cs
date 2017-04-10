using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem204 : ProblemBase {
        private bool[] _notPrimes;
        private ulong _count = 0;

        public override string ProblemName {
            get { return "204: Generalised Hamming Numbers"; }
        }

        public override string GetAnswer() {
            BuildPrimes((ulong)Math.Pow(10, 8));
            return "done";
        }

        private void BuildPrimes(ulong max) {
            max = (max / 2) + 1;
            _notPrimes = new bool[max + 1];
            for (ulong num = 2; num < max; num++) {
                if (!_notPrimes[num]) {
                    if (num > 5) {
                        _count++;
                    }
                    for (ulong composite = 2; composite * num < max; composite++) {
                        _notPrimes[composite * num] = true;
                        if (num > 5) {
                            _count++;
                        }
                    }
                }
            }
        }
    }
}
