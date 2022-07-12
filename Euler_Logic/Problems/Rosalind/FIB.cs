using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.Rosalind {
    public class FIB : RosalindBase {
        public override string ProblemName => "Rosalind: FIB";

        public override string GetAnswer() {
            return Solve(Input()).ToString();
        }

        private ulong Solve(List<string> input) {
            var nk = GetNK(input[0]);
            ulong baby = 0;
            ulong adult = 1;
            for (int month = 2; month <= nk.Item1; month++) {
                ulong subAdult = baby + adult;
                ulong subBaby = adult * nk.Item2;
                baby = subBaby;
                adult = subAdult;
            }
            return adult;
        }

        private Tuple<int, ulong> GetNK(string line) {
            var split = line.Split(' ');
            return new Tuple<int, ulong>(Convert.ToInt32(split[0]), Convert.ToUInt64(split[1]));
        }
    }
}
