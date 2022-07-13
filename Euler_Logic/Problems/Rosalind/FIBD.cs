using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.Rosalind {
    public class FIBD : RosalindBase {
        public override string ProblemName => "Rosalind: FIBD";

        public override string GetAnswer() {
            return Solve(Input()).ToString();
        }

        private ulong Solve(List<string> input) {
            var nm = GetNM(input[0]);
            var life = new ulong[nm.Item2];
            life[0] = 1;
            for (int month = 2; month <= nm.Item1; month++) {
                var temp = new ulong[nm.Item2];
                for (int index = 1; index < life.Length; index++) {
                    temp[0] += life[index];
                    temp[index] = life[index - 1];
                }
                life = temp;
            }
            return Sum(life);
        }

        private ulong Sum(ulong[] life) {
            ulong sum = 0;
            for (int index = 0; index < life.Length; index++) {
                sum += life[index];
            }
            return sum;
        }

        private Tuple<int, ulong> GetNM(string line) {
            var split = line.Split(' ');
            return new Tuple<int, ulong>(Convert.ToInt32(split[0]), Convert.ToUInt64(split[1]));
        }
    }
}
