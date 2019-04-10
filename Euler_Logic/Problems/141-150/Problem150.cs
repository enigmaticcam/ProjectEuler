using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem150 : ProblemBase {
        private List<long> _nums = new List<long>();
        private Dictionary<int, Tuple> _triSums = new Dictionary<int, Tuple>();
        private List<int> _rowStarts = new List<int>();

        /*
            Any length (n) of values in a row counts towards some triangle. So a lot of
            time can therefore be saved by suming the values of length (n) in a row
            at a start position only once. For example, suppose we had the following
            numbers in the 3rd row:

            15, -56, -32

            Starting with a length of 1, each cell would represent a single value.
            A length of 2 starting at 15 would be equal to a length of 1 starting at
            15 plus -56. A length of 2 starting at -56 would be equal to a length of 1
            starting at -56 + -32. And alength of 3 starting at 15 would be equal to a
            length of 2 starting at 15 + -32. This way numbers are only added together
            once.

            Finally, as we do this per row, each sum length (n) can be appended to an
            existing triangle at row (n - length). I keep an ongoing sum of each possible
            triangle at all positions: the current sum and the best sum. As I append
            new rows to each triangle, I test if adding this new sum results in a lower
            sum for that triangle. If so, then save it as the best sum.
         */

        public override string ProblemName {
            get { return "150: Searching a triangular array for a sub-triangle having minimum-sum"; }
        }

        public override string GetAnswer() {
            BuildNums();
            BuildRowStarts();
            BuildTriSums(1000);
            return Solve().ToString();
        }

        private long Solve() {
            return _triSums.Select(x => x.Value.BestSum).Min();
        }

        private void BuildNums() {
            long t = 0;
            long twoTo20 = (long)Math.Pow(2, 20);
            long twoTo19 = (long)Math.Pow(2, 19);
            for (int k = 1; k <= 500500; k++) {
                t = (615949 * t + 797807) % twoTo20;
                _nums.Add(t - twoTo19);
            }
        }

        private void BuildRowStarts() {
            int rowStart = 0;
            int offset = 1;
            for (int row = 0; row < 1000; row++) {
                _rowStarts.Add(rowStart);
                rowStart += offset;
                offset++;
            }
        }

        private void BuildTriSums(int maxRowCount) {
            int rowStart = 0;
            int offset = 1;
            int rowLength = 1;
            for (int row = 0; row < maxRowCount; row++) {
                long[] sums = new long[rowLength];
                for (int length = 1; length <= rowLength; length++) {
                    for (int index = 0; index <= rowLength - length; index++) {
                        int cell = rowStart + index;
                        if (length == 1) {
                            sums[index] = _nums[cell];
                            _triSums.Add(rowStart + index, new Tuple(_nums[cell], _nums[cell]));
                        } else {
                            var nextToAdd = _nums[cell + length - 1];
                            sums[index] += nextToAdd;
                            var tri = _triSums[_rowStarts[row - (length - 1)] + index];
                            tri.CurrentSum += sums[index];
                            if (tri.CurrentSum < tri.BestSum) {
                                tri.BestSum = tri.CurrentSum;
                            }
                        }
                    }
                }
                rowStart += offset;
                offset += 1;
                rowLength++;
            }
        }

        private class Tuple {
            public long CurrentSum { get; set; }
            public long BestSum { get; set; }

            public Tuple() { }
            public Tuple(long currentSum, long bestSum) {
                CurrentSum = currentSum;
                BestSum = bestSum;
            }
        }
    }
}