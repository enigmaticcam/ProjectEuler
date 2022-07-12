using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem2 : ProblemBase {
        public override string ProblemName {
            get { return "2: Even Fibonacci numbers"; }
        }

        public override string GetAnswer() {
            return CountEvenFibs(4000000).ToString();
        }

        private ulong CountEvenFibs(ulong terms) {
            ulong sum = 0;
            ulong last = 1;
            ulong current = 2;
            do {
                if (current % 2 == 0) sum += current;
                current += last;
                last = current - last;
            } while (current <= terms);
            return sum;
        }
    }
}
