using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem14 : ProblemBase {
        public override string ProblemName {
            get { return "14: Longest Collatz sequence"; }
        }

        public override string GetAnswer() {
            return GetLongestChain().ToString();
        }

        private ulong GetLongestChain() {
            ulong bestChainCount = 0;
            ulong bestChainNum = 0;
            for (ulong i = 2; i < 1000000; i++) {
                ulong chain = i;
                ulong count = 1;
                do {
                    if (chain % 2 == 0) {
                        chain = chain / 2;
                    } else {
                        chain = (3 * chain) + 1;
                    }
                    count++;
                } while (chain != 1);
                if (count > bestChainCount) {
                    bestChainCount = count;
                    bestChainNum = i;
                }
            }
            return bestChainNum;
        }
    }
}
