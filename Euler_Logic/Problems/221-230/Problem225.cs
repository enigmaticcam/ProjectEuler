using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem225 : ProblemBase {

        /*
            The sequence can be easily caluclated indefinitely by using t(n) = (t(n - 1) + t(n - 2) + t(n - 3)) % x,
            where x is some odd number. So I use a 3-tiered dictionary to hold all the unique sequences of 3 numbers.
            I stop when (1) a sequence of 3 numbers is found a second time, thus x cannot be a dividend of the sequence,
            or (2) when a 0 is found, in which case x can be a dividend.
         */

        public override string ProblemName {
            get { return "225: Tribonacci non-divisors"; }
        }

        public override string GetAnswer() {
            return Solve(124).ToString();
        }

        private ulong Solve(int maxCount) {
            ulong num = 27;
            int count = 0;
            do {
                if (DoesNotDivide(num)) {
                    count++;
                }
                num += 2;
            } while (count < maxCount);
            return num - 2;

        }

        private bool DoesNotDivide(ulong num) {
            var hash = new Dictionary<ulong, Dictionary<ulong, HashSet<ulong>>>();
            ulong[] values = new ulong[3];
            values[0] = 1;
            values[1] = 1;
            values[2] = 1;
            ulong index = 0;
            do {
                var step1 = (index + 1) % 3;
                var step2 = (index + 2) % 3;
                var step3 = (index + 3) % 3;
                values[index] = (values[step1] + values[step2] + values[step3]) % num;
                if (values[index] == 0) {
                    return false;
                }
                if (!hash.ContainsKey(values[step1])) {
                    hash.Add(values[step1], new Dictionary<ulong, HashSet<ulong>>());
                }
                if (!hash[values[step1]].ContainsKey(values[step2])) {
                    hash[values[step1]].Add(values[step2], new HashSet<ulong>());
                }
                if (hash[values[step1]][values[step2]].Contains(values[step3])) {
                    return true;
                } else {
                    hash[values[step1]][values[step2]].Add(values[step3]);
                }
                index = (index + 1) % 3;
            } while (true);
        }
    }
}
