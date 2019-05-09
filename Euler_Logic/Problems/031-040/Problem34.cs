using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem34 : ProblemBase {

        /*
            Build a dictionary of all the possible factorials 0-9. Then loop from 3 to 9999999, and for each number
            find the sum of the factorial digits. You can get each individual digit of a number by returning n % 10,
            and then dividing n by 10.
         */

        public override string ProblemName {
            get { return "34: Digit factorials"; }
        }

        public override string GetAnswer() {
            BuildFactorials();
            return Solve().ToString();
        }

        private Dictionary<ulong, ulong> _factorials = new Dictionary<ulong, ulong>();
        private void BuildFactorials() {
            ulong fact = 1;
            _factorials.Add(0, 1);
            _factorials.Add(1, 1);
            for (ulong next = 2; next <= 9; next++) {
                fact *= next;
                _factorials.Add(next, fact);
            }
        }

        private ulong Solve() {
            ulong total = 0;
            for (ulong num = 3; num <= 9999999; num++) {
                ulong sum = 0;
                ulong temp = num;
                do {
                    sum += _factorials[temp % 10];
                    temp /= 10;
                } while (temp > 0);
                if (sum == num) {
                    total += num;
                }
            }
            return total;
        }
    }
}
