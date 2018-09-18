using System;
namespace Euler_Logic.Problems {
    public class Problem25 : ProblemBase {

        /*
            Fibonacci sequence can be represented as the golden ratio: (1 + sqrt(5)) / 2. Starting at 1, we simply multiply 
            by this ratio repeatedly. Anytime we get a number that exceeds 10, we divide by 10 and count how many times
            we divide by 10. Once we've divided by 10 1000 times, we return the index.
         */

        public override string ProblemName {
            get { return "25: 1000-digit Fibonacci number"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private int Solve() {
            decimal ratio = ((decimal)1 + (decimal)Math.Sqrt(5)) / (decimal)2;
            decimal num = 1;
            int index = 1;
            int digitCount = 1;
            do {
                num *= ratio;
                index++;
                while (num > 10) {
                    num /= 10;
                    digitCount++;
                }
            } while (digitCount < 1000);
            return index;
        }
    }
}
