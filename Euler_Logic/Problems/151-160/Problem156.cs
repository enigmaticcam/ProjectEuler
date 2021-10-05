using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem156 : ProblemBase {
        public override string ProblemName {
            get { return "156: Counting Digits"; }
        }

        public override string GetAnswer() {
            BuildFHash();
            Test(2);
            LookFor(1);
            return "";
        }

        private Dictionary<ulong, ulong> _fHash = new Dictionary<ulong, ulong>();
        private void BuildFHash() {
            ulong maxLog = (ulong)Math.Log10(ulong.MaxValue);
            ulong power = 10;
            _fHash.Add(9, 1);
            ulong f = 1;
            for (ulong powerOf10 = 1; powerOf10 < maxLog - 1; powerOf10++) {
                f = f * 10 + power;
                power *= 10;
                _fHash.Add(power - 1, f);
            }
        }

        private void Test(ulong d) {
            ulong tenPower = 10;
            ulong n = d * 10 - 1;
            ulong f = _fHash[9] * d;
            do {
                n += tenPower;
                f += _fHash[tenPower - 1] + tenPower;
            } while (true);
            bool stop = true;
        }

        private ulong CountUpTo(ulong upTo, ulong d) {
            ulong count = 0;
            for (ulong n = 0; n <= upTo; n++ ) {
                count += DigitCount(n, d);
            }
            return count;
        }

        List<ulong> _nums = new List<ulong>();
        private void LookFor(ulong d) {
            ulong n = 0;
            ulong f = 0;
            ulong sum = 0;
            do {
                var count = DigitCount(n, d);
                f += count;
                //if (n == 100000) {
                //    bool stop = true;
                //}
                if (f == n) {
                    _nums.Add(n);
                    sum += n;
                }
                n++;
            } while (true);
        }

        private ulong DigitCount(ulong n, ulong d) {
            ulong count = 0;
            while (n > 0) {
                if (n % 10 == d) {
                    count++;
                }
                n /= 10;
            }
            return count;
        }
    }
}
