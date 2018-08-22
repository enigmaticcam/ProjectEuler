
namespace Euler_Logic.Problems {
    public class Problem113 : ProblemBase {
        private ulong[,] _increasing;
        private ulong[,] _decreasing;

        public override string ProblemName {
            get { return "113: Non-bouncy numbers"; }
        }

        /*
            This problem is easily solved by means of dynamic programming. For each digit 1-100 we ask what the non-bouncy count
            is for each value 1-9. See the chart below for first two digits.

            Digit   Value   Inc. Count  Dec. Count  Not Bouncy Count
            1       1       1           1           1
            1       2       1           1           1
            1       3       1           1           1
            1       4       1           1           1
            1       5       1           1           1
            1       6       1           1           1
            1       7       1           1           1
            1       8       1           1           1
            1       9       1           1           1
            2       1       9           2           10
            2       2       8           3           10
            2       3       7           4           10
            2       4       6           5           10
            2       5       5           6           10
            2       6       4           7           10
            2       7       3           8           10
            2       8       2           9           10
            2       9       1           10          10

            It can be seen that the first values for digit 1 are all 1. This is obvious, as all numbers 1-9 are both increasing and decreasing.
            The increasing count for Digit 2 value 1 is the sum of Increasing Counts for all values for Digit 1  with a value equal or greater 
            than 1. The decreasing count for Digit 2 value 1 is the sum of Decreasing Counts for all values for Digit 1 that are equal or less
            than 1, + 1 (counting the 0 just once). Therefore, the Not Bouncy Count is the sum of the increasing count + the decreasing count,
            - 1 (removing the single number that is both increasing and decreasing to 1, which is 1).

            We continue this logic and sum the total of the non bouncy count up to Digit 100 Value 9.
            
         */

        public override string GetAnswer() {
            int digitCount = 100;
            Initialize(digitCount);
            return Solve(digitCount).ToString();
        }

        private void Initialize(int digitCount) {
            _increasing = new ulong[digitCount, 9];
            _decreasing = new ulong[digitCount, 9];
            for (int index = 0; index < 9; index++) {
                _increasing[0, index] = 1;
                _decreasing[0, index] = 1;
            }
        }

        private ulong Solve(int digitCount) {
            ulong sum = 9;
            for (int digit = 1; digit < digitCount; digit++) {
                for (int value = 0; value < 9; value++) {
                    ulong increasing = 0;
                    for (int index = value; index < 9; index++) {
                        increasing += _increasing[digit - 1, index];
                    }
                    ulong decreasing = 1;
                    for (int index = value; index >= 0; index--) {
                        decreasing += _decreasing[digit - 1, index];
                    }
                    _increasing[digit, value] = increasing;
                    _decreasing[digit, value] = decreasing;
                    sum += increasing + decreasing - 1;
                }
            }
            return sum;
        }
    }
}
