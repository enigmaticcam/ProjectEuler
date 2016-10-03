using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem327 : IProblem {
        public string ProblemName {
            get { return "327: Rooms of Doom"; }
        }

        public string GetAnswer() {
            return Solve(3, 40, 30).ToString();
        }

        private ulong Solve(ulong min, ulong max, ulong r) {
            ulong sum = 0;
            for (ulong num = min; num <= max; num++) {
                sum += GetMinimum(num, r);
            }
            return sum;
        }

        private ulong GetMinimum(ulong c, ulong r) {
            ulong m = 2;
            for (ulong subR = 2; subR <= r; subR++) {
                if (c - 1 >= m) {
                    m += 1;
                } else {
                    ulong startWith = c - 1;
                    ulong need = m - startWith;
                    ulong gainPerTrip = c - 2;
                    ulong trips = need / gainPerTrip;
                    ulong allCards = trips * c;
                    ulong remaining = need % gainPerTrip;
                    if (remaining > 0) {
                        allCards += (remaining + 2);
                    }
                    allCards += c;
                    m = allCards;
                }
            }
            return m;
        }
    }
}
