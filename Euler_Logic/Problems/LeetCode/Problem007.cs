using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.LeetCode {
    public class Problem007 : LeetCodeBase {
        public override string ProblemName => "Leet Code 7: Reverse Integer";

        public override string GetAnswer() {
            Check(Reverse(123), 321);
            Check(Reverse(-123), -321);
            Check(Reverse(120), 21);
            return "";
        }

        public int Reverse(int x) {
            var value = (long)x;
            long result = 0;
            while (value != 0) {
                result = result * 10 + value % 10;
                value /= 10;
            }
            if (result > (long)int.MaxValue || result < (long)int.MinValue) {
                return 0;
            } else {
                return (int)result;
            }
        }
    }
}
