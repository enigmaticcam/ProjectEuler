using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem148 : ProblemBase {
        private Dictionary<ulong, ulong> _counts = new Dictionary<ulong, ulong>();
        private ulong[] _powersOf7;

        /*
            Each entry in pascal's triangle can be calculated using binomial coefficients. For example, the binomial coefficients
            for row 6 would be (m,n): (6,0) (6,1) (6,2) (6,3) (6,4) (6,5) (6,6)

            According to Lucas's Theorem (https://en.wikipedia.org/wiki/Lucas%27s_theorem), a binomial coefficent (m,n) is divisible
            by prime (p) if and only if at least one of the base digits of (n) is greater than the corresponding digit of (m).
            Let's use (2597, 48) as an example. 2597 in base 7 is 10400, and 48 in base 7 is 66. Looking at just the last digit in
            both numbers, it can be seen that 6 > 0. So therefore (2597, 48) is divisible by 7.

            Now let's look at (2597, 49). 2597 in base 7 is 10400, and 49 in base 7 is 100. There is no corresponding digit pair
            between these two numbers where (n) is more than (m), so this is NOT divisible by 7. Of all possible values for (n), 
            these are the only few that are not divisible by 7:
            0
            100
            200
            300
            400
            10000
            10100
            10200
            10300
            10400

            So (2597, n) = 10. This can be quickly calculated by multiplying each digit + 1. What I do then is maintain a list of 
            base 7 digits in an array. As I examine each row, I increase the lowest digit by 1. When that digit reaches 7, I reset 
            it back to 0 and increase the next digit. Then I multiply each digit + 1 to get the count of numbers not divisible by
            7 for that row.

            Interestingly, not every row has to be calculated this way. A brute force approach of the first few 100 rows shows that 
            if a row is divisible by 7 and the count of numbers not divisible by 7 for that row is (x), then the next row is (x * 2),
            and the next is (x * 3), all the way to (x * 7). So the total for all 7 rows is (x * 28).

            This can be further reduced by observing that for every row divisible by 49, if the count of numbers not divisible by 7
            for that row is (x), then the 7th row past that will be (x * 2), the 14th row will be (x * 3), up to (x * 7) on the 42nd
            row. So we really only need to examine the base 7 digits of every 49th row, and the subsequent 48 rows can be inferred.
         */

        public override string ProblemName {
            get { return "148: Exploring Pascal's triangle"; }
        }

        public override string GetAnswer() {
            _powersOf7 = new ulong[9];
            return Solve(1000000000).ToString();
        }

        private ulong Solve(ulong rowCount) {
            ulong row = 0;
            ulong sum = 0;
            ulong last = 0;
            ulong count = 0;
            ulong sub = 0;
            do {
                if (row % 49 == 0) {
                    count = NextRow(row);
                    last = count;
                    sub = 1;
                } else {
                    sub++;
                    count = sub * last;
                }
                if (rowCount - 1 >= row + 7) {
                    sum += 28 * count;
                } else {
                    ulong x = 1;
                    do {
                        sum += count * x;
                        x++;
                        row++;
                    } while (row < rowCount);
                }
                row += 7;
            } while (row < rowCount);
            return sum;
        }

        private ulong NextRow(ulong line) {
            if (line == 0) {
                return 1;
            }
            int index = 0;
            while (_powersOf7[index] == 6) {
                _powersOf7[index] = 0;
                index++;
            }
            _powersOf7[index]++;
            ulong sum = 1;
            for (index = 0; index < _powersOf7.Length; index++) {
                sum *= _powersOf7[index] + 1;
            }
            return sum;
        }
    }
}