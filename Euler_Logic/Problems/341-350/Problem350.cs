using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem350 : ProblemBase {
        public override string ProblemName {
            get { return "350: Constraining the least greatest and the greatest least"; }
        }

        public override string GetAnswer() {
            return "";
        }

        private void BruteForce(ulong g, ulong l, int n) {
            RecurisveBruteForce(g, l, new ulong[n], 0);
        }

        private ulong _count = 0;
        private List<string> _answers = new List<string>();
        private void RecurisveBruteForce(ulong g, ulong l, ulong[] num, int index) {
            for (ulong nextNum = 1; nextNum <= l; nextNum++) {
                num[index] = nextNum;
                if (index < num.Length - 1) {
                    RecurisveBruteForce(g, l, num, index + 1);
                } else {
                    ulong subG = num[0];
                    ulong subL = num[0];
                    foreach (var sub in num) {
                        subG = GCD.GetGCD(subG, sub);
                        subL = LCM.GetLCM(subL, sub);
                    }
                    if (subG >= g && subL <= l) {
                        _count++;
                        _answers.Add(string.Join(",", num));
                    }
                }
            }
        }
    }
}
