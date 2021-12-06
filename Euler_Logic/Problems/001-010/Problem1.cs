using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem1 : ProblemBase {
        public override string ProblemName {
            get { return "1: Multiples of 3 and 5"; }
        }

        public override string GetAnswer() {
            return CountMultiples(1000).ToString();
        }

        private int CountMultiples(int max) {
            int count = 0;
            for (int i = 1; i < max; i++) {
                if (i % 3 == 0 || i % 5 == 0) {
                    count += i;
                }
            }
            return count;
        }

        private string FindPlayerCombos(int playerCount) {
            var maxBits = Math.Pow(2, playerCount);
            var valid = new HashSet<ulong>();
            var maxGroupSize = playerCount / 2;
            ulong offset = 2 - (ulong)(playerCount % 2);
            for (ulong bits = 1; bits <= maxBits; bits += offset) {
                if (!valid.Contains(bits)) {
                    int bitCount = 0;
                    var tempBits = bits;
                    while (tempBits > 0) {
                        if ((tempBits & 1) == 1) bitCount++;
                        tempBits >>= 1;
                    }
                    if (bitCount == maxGroupSize) {
                        valid.Add(bits);
                    }
                }
            }
            return Output(valid, playerCount);
            //return valid.Count.ToString();
        }

        private string Output(HashSet<ulong> valids, int playerCount) {
            var output = new StringBuilder();
            int count = 0;
            ulong maxBit = (ulong)Math.Pow(2, playerCount - 1);
            foreach (var valid in valids) {
                count++;
                output.Append(count + "\t");
                for (ulong bit = 1; bit <= maxBit; bit *= 2) {
                    if ((valid & bit) == bit) {
                        output.Append("1" + "\t");
                    } else {
                        output.Append("2" + "\t");
                    }
                }
                output.AppendLine();
            }
            return output.ToString();
        }
    }
}
