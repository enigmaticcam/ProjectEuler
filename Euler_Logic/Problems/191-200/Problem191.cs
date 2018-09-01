using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem191 : ProblemBase {
        private ulong[] _zeroL;

        /*
            This can be solved via dynamic programming. We loop up from Day 1 to Day 30 summing up the following measures where n = day:

            a(n) = The total number of prize strings starting with "O"
            b(n) = The total number of prize strings starting with just one "A"
            c(n) = The total number of prize strings starting with just two "A"s
            d(n) = The total number of prize strings starting with "L" (and therefore having no other "L"s)

            The final answer is the sum of these four numbers at Day 30. Here is how to calculate these numbers:

            a(n) = a(n - 1) + b(n - 1) + c(n - 1) + d(n - 1)
            b(n) = a(n - 1) + d(n - 1)
            c(n) = b(n - 1)
            d(n) = ?

            I couldn't find an immediately obvious way of solving for d(n) this method. However, you can rephrase d(n) as the
            total number of ways of fitting blocks of either 1 or 2 size always having one empty space in between in space of
            length n. Funnily enough, I've already done this on problem 114. So we just borrow the logic from there to calculate d(n).

            I hard coded the values for n = 4 to make things easy. Then loop up from there to n = 30 and return the total.
         */

        public override string ProblemName {
            get { return "191: Prize Strings"; }
        }

        public override string GetAnswer() {
            int maxSize = 30;
            SolveFor0L(maxSize);
            return SolveForAll(maxSize).ToString();
        }

        private ulong SolveForAll(int maxSize) {
            Dictionary<bool, ulong> prizeWith0 = new Dictionary<bool, ulong>();
            Dictionary<bool, ulong> prizeWithNoAPrefix = new Dictionary<bool, ulong>();
            Dictionary<bool, ulong> prizeWithOneAPrefix = new Dictionary<bool, ulong>();
            Dictionary<bool, ulong> prizeWithZeroL = new Dictionary<bool, ulong>();
            prizeWith0.Add(false, 19);
            prizeWithNoAPrefix.Add(false, 12);
            prizeWithOneAPrefix.Add(false, 5);
            prizeWithZeroL.Add(false, 7);
            bool currentSet = true;
            for (int index = 5; index <= maxSize; index++) {
                prizeWith0[currentSet] = prizeWith0[!currentSet] + prizeWithNoAPrefix[!currentSet] + prizeWithOneAPrefix[!currentSet] + prizeWithZeroL[!currentSet];
                prizeWithNoAPrefix[currentSet] = prizeWith0[!currentSet] + prizeWithZeroL[!currentSet];
                prizeWithOneAPrefix[currentSet] = prizeWithNoAPrefix[!currentSet];
                prizeWithZeroL[currentSet] = _zeroL[index - 1] + 1;
                currentSet = !currentSet;
            }
            ulong answer = prizeWith0[!currentSet] + prizeWithNoAPrefix[!currentSet] + prizeWithOneAPrefix[!currentSet] + prizeWithZeroL[!currentSet];
            return answer;
        }

        private void SolveFor0L(int maxSize) {
            List<int> blocks = new List<int>();
            _zeroL = new ulong[maxSize + 1];
            for (int length = 1; length <= maxSize; length++) {
                for (int size = 1; size <= 2; size++) {
                    if (size <= length) {
                        _zeroL[length] += 1;
                        for (int position = 0; position < length - size; position++) {
                            _zeroL[length] += 1 + _zeroL[length - size - position - 1];
                        }
                    }
                }
            }
        }
    }
}