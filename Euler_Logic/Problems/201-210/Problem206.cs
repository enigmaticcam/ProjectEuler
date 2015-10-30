using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem206 : IProblem {
        public string ProblemName {
            get { return "206: Concealed Square"; }
        }

        public string GetAnswer() {
            return FindConcealedSquare().ToString();
        }

        private ulong FindConcealedSquare() {
            ulong min = (ulong)Math.Sqrt(1020304050607080900);
            ulong max = (ulong)Math.Sqrt(1929394959697989990);
            min = FindNextMinDivisibleBy10(min);
            for (ulong num = min; num <= max; num += 10) {
                ulong squared = num * num;
                if ((squared % 10) == 0
                    && (squared % 1000) / 100 == 9
                    && (squared % 100000) / 10000 == 8
                    && (squared % 10000000) / 1000000 == 7
                    && (squared % 1000000000) / 100000000 == 6
                    && (squared % 100000000000) / 10000000000 == 5
                    && (squared % 10000000000000) / 1000000000000 == 4
                    && (squared % 1000000000000000) / 100000000000000 == 3
                    && (squared % 100000000000000000) / 10000000000000000 == 2
                    && (squared % 10000000000000000000) / 1000000000000000000 == 1) {
                        return num;
                }
            }
            return 0;
        }

        private ulong FindNextMinDivisibleBy10(ulong num) {
            while (num % 10 != 0) {
                num++;
            }
            return num;
        }
    }
}
