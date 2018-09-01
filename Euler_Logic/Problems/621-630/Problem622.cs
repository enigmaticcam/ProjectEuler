using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem622 : ProblemBase {

        public override string ProblemName {
            get { return "622: Riffle Shuffles"; }
        }

        public override string GetAnswer() {
            Solve();
            return "";
        }

        private void Solve() {
            int deckSize = 2;
            StringBuilder results = new StringBuilder();
            do {
                ulong count = Try(deckSize);
                results.Append(deckSize + ":" + count + ",");
                deckSize += 2;
                if (deckSize == 66) {
                    bool stophere = true;
                }
            } while (true);
        }

        private ulong Try(int deckSize) {
            int two = 2;
            ulong count = 0;
            do {
                count++;
                if (two <= deckSize / 2) {
                    two = ((two - 1) * 2) + 1;
                } else {
                    two = (two - (deckSize / 2)) * 2;
                }
                if (two == 2) {
                    return count;
                }
            } while (true);
        }
    }
}
