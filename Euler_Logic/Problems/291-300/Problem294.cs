using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem294 : ProblemBase {
        public override string ProblemName {
            get { return "294: Sum of digits - experience #23"; }
        }

        public override string GetAnswer() {
            FindTwentyThreeFact();
            FindDigitSums(1, 0, new int[9]);
            return "";
        }

        private Dictionary<int, BigInteger> _factorial = new Dictionary<int, BigInteger>();
        private void FindTwentyThreeFact() {
            BigInteger num = 1;
            for (BigInteger next = 1; next <= 23; next++) {
                num *= next;
                _factorial.Add((int)next, num);
            }
        }

        private void FindDigitSums(int currentNum, int sum, int[] digits) {
            int max = (23 - sum) / currentNum;
            var tempSum = sum;
            for (int num = 1; num <= max; num++) {
                sum += currentNum;
                digits[currentNum - 1]++;
                if (sum == 23) {
                    FoundCombo(digits);
                } else if (currentNum < 9) {
                    FindDigitSums(currentNum + 1, sum, digits);
                }
            }
            digits[currentNum - 1] = 0;
            if (currentNum < 9) {
                FindDigitSums(currentNum + 1, tempSum, digits);
            }
            digits[currentNum - 1] = 0;
        }

        private void FoundCombo(int[] digits) {
            
        }

        private ulong BruteForceS(ulong n) {
            ulong count = 0;
            ulong max = (ulong)Math.Pow(10, n);
            for (ulong k = 23; k <= max; k += 23) {
                if (BruteForceD(k) == 23) {
                    count++;
                }
            }
            return count;
        }

        private ulong BruteForceD(ulong n) {
            ulong sum = 0;
            while (n > 0) {
                sum += n % 10;
                n /= 10;
            }
            return sum;
        }
    }
}
