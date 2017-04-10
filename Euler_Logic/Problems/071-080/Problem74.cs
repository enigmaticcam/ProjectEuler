using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem74 : ProblemBase {

        public override string ProblemName {
            get { return "74: Digit Factorial Chains"; }
        }

        public override string GetAnswer() {
            int total = 0;
            for (int i = 1; i < 1000000; i++) {
                int count = GetChainCount(i);
                if (count == 60) {
                    total++;
                }
            }
            return total.ToString();
        }

        private int GetChainCount(int number) {
            HashSet<int> chain = new HashSet<int>();
            int newNumber = number;
            int oldNumber = number;
            chain.Add(number);
            do {
                newNumber = FindFactorialSum(oldNumber);
                if (oldNumber == newNumber) {
                    return Math.Max(1, chain.Count);
                }
                if (chain.Contains(newNumber)) {
                    return chain.Count;
                }
                chain.Add(newNumber);
                oldNumber = newNumber;
                
            } while (true);
        }

        private int FindFactorialSum(int number) {
            int factorial = 0;
            char[] digits = number.ToString().ToCharArray();
            foreach (char digit in digits) {
                factorial += FindFactorial(int.Parse(digit.ToString()));
            }
            return factorial;
        }

        private int FindFactorial(int number) {
            if (number <= 1) {
                return 1;
            } else {
                return number * FindFactorial(number - 1);
            }
        }
    }
}
