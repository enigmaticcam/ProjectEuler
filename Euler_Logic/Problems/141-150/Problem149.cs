using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem149 : ProblemBase {
        private List<long> _numbers = new List<long>();
        private long _best = long.MinValue;

        public override string ProblemName {
            get { return "149: Searching for a maximum-sum subsequence"; }
        }

        /*
            Consider a single one-dimensional array of numbers instead of a 2x2 matrix. To get the highest sum of any adjacent numbers,
            start with the first number and set it as the highest sum. Then add it to the second number. If that sum is higher, set it
            as the highest sum. Add to the third number and check if the new sum is higher. Do this until the end of the sequence is 
            reached. Now start at the first number again, but this time subtract each subsequent number from the best sum found from
            the previous best sum. Whichever number gives you the highest sum will give you the final highst sum of any adjacent numbers
            in the sequence.

            So then, do this for all rows, columns, diagonals, and anti-diagonals. Turns out the answer isn't in any diagonal, so I
            didn't even have to write the logic to transcribe all the diagonals to a single 1-d array.
         */

        public override string GetAnswer() {
            GenerateNumbers();
            SolveHorizontal();
            SolveVertical();
            return _best.ToString();
        }

        private void GenerateNumbers() {
            for (long k = 1; k <= 55; k++) {
                _numbers.Add(((100003 - (200003 * k) + (300007 * k * k * k)) % 1000000) - 500000);
            }
            for (long k = 56; k <= 4000000; k++) {
                _numbers.Add(((_numbers[(int)k - 25] + _numbers[(int)k - 56] + 1000000) % 1000000) - 500000);
            }
        }

        private void SolveHorizontal() {
            for (int x = 0; x < 2000; x++) {
                long[] data = new long[2000];
                for (int y = 0; y < 2000; y++) {
                    data[y] = _numbers[x * 2000 + y];
                }
                SolveSingleRow(data);
            }
        }

        private void SolveVertical() {
            for (int x = 0; x < 2000; x++) {
                long[] data = new long[2000];
                for (int y = 0; y < 2000; y++) {
                    data[y] = _numbers[y * 2000 + x];
                }
                SolveSingleRow(data);
            }
        }

        private void SolveForwardDiagonal() {
            // lawl don't need
        }

        private void SolveBackwardDiagonal() {
            // lawl don't need
        }

        private void SolveSingleRow(long[] nums) {
            long sum = 0;
            long bestSum = long.MinValue;
            long bestIndex = 0;
            for (int index = 0; index < nums.Length; index++) {
                sum += nums[index];
                if (sum >= bestSum) {
                    bestSum = sum;
                    bestIndex = index;
                }
            }
            sum = bestSum;
            for (int index = 0; index < bestIndex; index++) {
                sum -= nums[index];
                if (sum > bestSum) {
                    bestSum = sum;
                }
            }
            if (bestSum > _best) {
                _best = bestSum;
            }
        }
    }
}
