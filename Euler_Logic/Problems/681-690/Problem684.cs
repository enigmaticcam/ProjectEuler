using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem684 : ProblemBase {
        public override string ProblemName {
            get { return "684: Inverse Digit Sum"; }
        }

        private ulong _mod;
        public override string GetAnswer() {
            _mod = 1000000007;
            BuildF();
            Solve();
            return "";
        }

        private void Solve() {
            ulong sum = 0;
            for (int i = 2; i <= 90; i++) {
                sum += S(_f[i]);
            }
        }

        //private ulong S(ulong n) {
        //    ulong sum = 0;
        //    for (ulong num = 1; num <= n; num++) {
        //        sum += s(num);
        //    }
        //    return sum;
        //}

        private ulong _lastNum = 1;
        private ulong _lastSum;
        private ulong S(ulong n) {
            for (; _lastNum <= n; _lastNum++) {
                _lastSum += s(_lastNum);
            }
            return _lastSum;
        }

        private ulong s(ulong n) {
            ulong count = n / 9;
            ulong num = n - (count * 9);
            while (count > 0) {
                num = (num * 10 + 9) % _mod;
                count--;
            }
            return num;
        }

        private ulong DigitSum(ulong n) {
            ulong sum = 0;
            while (n != 0) {
                sum += n % 10;
                n /= 10;
            }
            return sum;
        }

        private ulong[] _f;
        private void BuildF() {
            _f = new ulong[91];
            _f[1] = 1;
            for (int i = 2; i <= 90; i++) {
                _f[i] = _f[i - 1] + _f[i - 2];
            }
        }
    }
}
