using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem166 : ProblemBase {
        private Dictionary<int, Dictionary<int, List<int>>> _firstDigit = new Dictionary<int, Dictionary<int, List<int>>>();
        private Dictionary<int, Dictionary<int, List<int>>> _middleDigits = new Dictionary<int, Dictionary<int, List<int>>>();

        /*
            It's a really ugly way to do this, but what I do is build two references: one
            dictionary that holds the list of all valid numbers where the first digit is
            equal to (x) and the sum of all digits is equal to (y), and one dictionary
            that holds the list of all valid numbers where the two middle digits are
            equal to (x) and the sum of all digits are equal to (y).

            Then I begin looping through all numbers from 0 to 9999. For each number (n),
            I get the sum (s). Then I loop through all numbers where the sum is (s) and
            the first digit is the same as the first digit in (n). That's my right
            diagonal. For each of those, I loop through all numbers where the sum is (s)
            and the first digit is the same as the last digit in (n). That's my left
            diagonal.

            At this point I have the top row and both diagonals. I then loop through all
            possible values for row 2 having the same sum as (s) and the two middle digits
            are derived from my two diagonals, and for each of those i loop through all
            possible value for row 3 having the same sum and the two middle digits also
            derived from my two diagonals. I can now compute the middle values for row 4.
            If those valus are within 0-9, then all I have to check is the sums of the
            far left and far right column. If those are equal to (s) then I found a valid
            matrix.
         */

        public override string ProblemName {
            get { return "166: Criss Cross"; }
        }

        public override string GetAnswer() {
            BuildRefs();
            return Solve().ToString();
        }

        private ulong Solve() {
            ulong count = 0;
            for (int row1 = 0; row1 <= 9999; row1++) {
                var sum = GetSum(row1);
                foreach (var downRight in _firstDigit[row1 / 1000][sum]) {
                    foreach (var downLeft in _firstDigit[row1 % 10][sum]) {
                        var sum1 = ((downRight / 100) % 10) * 10;
                        sum1 += (downLeft / 100) % 10;
                        var sum2 = (downRight / 10) % 10;
                        sum2 += ((downLeft / 10) % 10) * 10;
                        if (_middleDigits.ContainsKey(sum1)
                            && _middleDigits.ContainsKey(sum2)
                            && _middleDigits[sum1].ContainsKey(sum)
                            && _middleDigits[sum2].ContainsKey(sum)) {
                            foreach (var row2 in _middleDigits[sum1][sum]) {
                                foreach (var row3 in _middleDigits[sum2][sum]) {
                                    var leftMiddle = sum - ((row1 / 100) % 10) - ((row2 / 100) % 10) - ((row3 / 100) % 10);
                                    if (leftMiddle >= 0 && leftMiddle <= 9) {
                                        var rightMiddle = sum - ((row1 / 10) % 10) - ((row2 / 10) % 10) - ((row3 / 10) % 10);
                                        if (rightMiddle >= 0 && rightMiddle <= 9) {
                                            var row4Sum = leftMiddle + rightMiddle + (downRight % 10) + (downLeft % 10);
                                            if (row4Sum == sum) {
                                                var row4 = (downLeft % 10) * 1000 + leftMiddle * 100 + rightMiddle * 10 + (downRight % 10);
                                                if (row1 / 1000 + row2 / 1000 + row3 / 1000 + row4 / 1000 == sum
                                                    && (row1 % 10) + (row2 % 10) + (row3 % 10) + (row4 % 10) == sum) {
                                                    count++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return count;
        }

        private void BuildRefs() {
            for (int n = 0; n <= 9999; n++) {
                var sum = GetSum(n);
                var firstDigit = n / 1000;
                var middleDigits = (n / 10) % 100;
                if (!_firstDigit.ContainsKey(firstDigit)) {
                    _firstDigit.Add(firstDigit, new Dictionary<int, List<int>>());
                }
                if (!_firstDigit[firstDigit].ContainsKey(sum)) {
                    _firstDigit[firstDigit].Add(sum, new List<int>());
                }
                _firstDigit[firstDigit][sum].Add(n);
                if (!_middleDigits.ContainsKey(middleDigits)) {
                    _middleDigits.Add(middleDigits, new Dictionary<int, List<int>>());
                }
                if (!_middleDigits[middleDigits].ContainsKey(sum)) {
                    _middleDigits[middleDigits].Add(sum, new List<int>());
                }
                _middleDigits[middleDigits][sum].Add(n);
            }
        }

        private int GetSum(int n) {
            int sum = 0;
            while (n > 0) {
                var remainder = n % 10;
                sum += remainder;
                n /= 10;
            }
            return sum;
        }
    }
}