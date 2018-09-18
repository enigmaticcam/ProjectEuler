using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem24 : ProblemBase {
        private List<uint> _digitsRemaining = new List<uint>();
        private FactorialWithHashUInt _factorial = new FactorialWithHashUInt();

        /*
            The total possible ways of arranging 10 digits is 10!. This means there are 9! ways of arranging the remaining 9 digits
            if we were to set the first digit to 0, and each subsequent first digit woult be an additional 9!. So we loop upward
            starting from 0 until we reach our maximum, in this case 9!*3. We therefore know the answer is somewhere between 9!*2
            and 9!*3, so we can immediately conclude the first digit is a 2. If the first digit is a 2, we know we've used 9!*2 ways,
            and so we have a remainder of 1000000 - (9!*2), or 274240. Now we begin with the second digit, knowing that each subsequent
            digit adds 8!. So we loop upward again starting from 0 (skipping 2 since we've used it already), continuing to add 8! until
            we exceed our remainder of 274240. We continue to do this for all remaining digits.
         */

        public override string ProblemName {
            get { return "24: Lexicographic permutations"; }
        }

        public override string GetAnswer() {
            Initialize();
            return Solve().ToString();
        }

        private void Initialize() {
            Enumerable.Range(0, 10).ToList().ForEach(x => _digitsRemaining.Add((uint)x));
        }

        private uint Solve() {
            uint result = 0;
            uint remaining = 1000000;
            for (uint digit = 1; digit < 10; digit++) {
                uint fact = _factorial.Get(10 - digit);
                uint total = fact;
                int digitIndex = 0;
                while (total < remaining) {
                    total += fact;
                    digitIndex++;
                }
                result = (result * 10) + _digitsRemaining[digitIndex];
                _digitsRemaining.RemoveAt(digitIndex);
                remaining -= total - fact;
            }
            result = (result * 10) + _digitsRemaining[0];
            return result;
        }
    }
}
