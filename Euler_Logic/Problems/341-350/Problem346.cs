using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem346 : IProblem {
        private Dictionary<ulong, int> _counts = new Dictionary<ulong, int>();

        public string ProblemName {
            get { return "346: Strong Repunits"; }
        }

        public string GetAnswer() {
            return Solve(1000000000000).ToString();
        }

        private ulong Solve(ulong max) {
            ulong total = 1;
            ulong baseNum = 2;
            ulong stop = (ulong)Math.Sqrt((ulong)max);
            do {
                ulong power = baseNum;
                ulong sum = baseNum + 1;
                bool first = true;
                do {
                    if (_counts.ContainsKey(sum)) {
                        _counts[sum]++;
                        if (_counts[sum] == 2) {
                            total += sum;
                        }
                    } else {
                        if (!first) {
                            total += sum;
                            _counts.Add(sum, 2);
                        } else {
                            _counts.Add(sum, 1);
                        }
                    }
                    power *= baseNum;
                    sum += power;
                    first = false;
                } while (sum <= max);
                baseNum++;
            } while (baseNum <= stop);
            return total;
        }
    }
}
