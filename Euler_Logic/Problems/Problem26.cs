using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem26 : IProblem {
        public string ProblemName {
            get { return "26: Reciprocal cycles"; }
        }

        public string GetAnswer() {
            return FindLongestCycle().ToString();
        }

        private int FindLongestCycle() {
            int bestScore = 0;
            int bestValue = 0;
            for (int i = 2; i < 1000; i++) {
                int size = GetChainSize(i);
                if (size > bestScore) {
                    bestScore = size;
                    bestValue = i;
                }
            }
            return bestValue;
        }

        private int GetChainSize(int num) {
            Dictionary<int, int> remainders = new Dictionary<int, int>();
            int remainder = 10;
            int quotient = 0;
            int count = 0;
            int lastRemainder = -1;
            do {
                while (remainder < num) {
                    remainder *= 10;
                    if (remainders.ContainsKey(remainder)) {
                        return count - remainders[remainder];
                    }
                    remainders.Add(remainder, count);
                    count++;
                }
                quotient = (int)(remainder / num);
                remainder -= (quotient * num);
                if (remainder == lastRemainder) {
                    return 1;
                } else if (remainder == 0) {
                    return remainders.Count + 1;
                } else if (remainders.ContainsKey(remainder)) {
                    return count - remainders[remainder];
                } else {
                    remainders.Add(remainder, count);
                    lastRemainder = remainder;
                    remainder *= 10;
                }
                count++;
            } while (true);
        }
    }
}
