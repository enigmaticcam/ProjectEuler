using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem164 : ProblemBase {
        private Dictionary<int, ulong> _counts = new Dictionary<int, ulong>();

        /*
            There are 165 ways to satisfy this problem with just 3 digits. Consider the distinct list of the last two digits of all these 165 numbers and the count
            from the list of 165 that contain those last two digits. For example, out of 165, there are 9 of them that end in 00: 100, 200, 300, etc. So 00 = 9.
            There are 3 that end in 51: 151, 251, and 351. So 51 = 3. Save these in a dictionary.

            To add a fourth digit, for each of the distinct two-digit endings of the 165, we check to see if we can append all numbers 0-9 at the end. Where we can,
            we create a new dictionary of counts based on the last two digits of this new number (the last digit of the two-digit number, and the new 0-9 digit
            we are appending), and add the sum from what the sum was for the two digit number. For example, since 00 has a count of 9, and since we can append
            all 0-9 digits at the end of 00, then in our new dictionary of counts: 00 = 9, 01 = 9, 02 = 9, 03 = 9, etc. Of course, these numbers might change
            as we add new ones (e.g., if 10 = 8, then add 8 to 00, 8 to 01, 8 to 02, etc). Replace the current dictionary of counts with the new one. Continue
            to do this until we've used all digits.
         */

        public override string ProblemName {
            get { return "164: Numbers for which no three consecutive digits have a sum greater than a given value"; }
        }

        public override string GetAnswer() {
            BuildCounts();
            return Solve(20).ToString();
        }

        private ulong Solve(int digits) {
            for (int digit = 4; digit <= digits; digit++) {
                Dictionary<int, ulong> nextCounts = new Dictionary<int, ulong>();
                foreach (var combo in _counts) {
                    int num = combo.Key;
                    int x = num % 10;
                    num /= 10;
                    int y = num;
                    for (int z = 0; x + y + z <= 9; z++) {
                        int key = z + (x * 10);
                        if (!nextCounts.ContainsKey(key)) {
                            nextCounts.Add(key, combo.Value);
                        } else {
                            nextCounts[key] += combo.Value;
                        }
                    }
                }
                _counts = nextCounts;
            }
            return GetSum();
        }

        private void BuildCounts() {
            for (int num = 100; num <= 999; num++) {
                int next = num;
                int x = next % 10;
                next /= 10;
                int y = next % 10;
                next /= 10;
                if (x + y + next <= 9) {
                    int countToAdd = (x * 10) + y;
                    if (!_counts.ContainsKey(countToAdd)) {
                        _counts.Add(countToAdd, 1);
                    } else {
                        _counts[countToAdd]++;
                    }
                }
            }
        }

        private ulong GetSum() {
            ulong sum = 0;
            foreach (var combo in _counts) {
                sum += combo.Value;
            }
            return sum;
        }
    }
}