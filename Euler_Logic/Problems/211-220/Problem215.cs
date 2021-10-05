using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem215 : ProblemBase {
        private List<ulong> _singles = new List<ulong>();
        private Dictionary<int, List<int>> _pairCount = new Dictionary<int, List<int>>();
        private PowerAll _powerOf2 = new PowerAll(2);

        /*
            This can be solved using basic DP. First I calculate all the possible ways of arranging bricks of sizes 2 and 3
            in a single row. I store the result as a bitmask where each bit represents a border number. Then, I find all
            the different ways a single row can pair against another row. Simply perform bitwise AND between all 
            combinations of singles. Store a list for each single row which contains a list of all the other single rows 
            that can pair with it. Finally, the DP part is just a matter of counting. Loop through all the pair counts to 
            determine how many ways to end on a single. Obviously we start with a count of 1 for each single. But since
            some singles only pair with others, the counts will change for each row. For example, if there are 10 ways of 
            ending on single #3, then for each possible pair starting with single #3, we add 10 to that single. Sum the
            counts of the last row to get the answer.
         */

        public override string ProblemName {
            get { return "215: Crack-free Walls"; }
        }

        public override string GetAnswer() {
            GenerateSingles(32);
            BuildPairCounts();
            return Solve(10).ToString();
        }

        private void GenerateSingles(int length) {
            var maxOfTwo = length / 2;
            if (length % 3 == 0) {
                AddToSingle(GetThrees(length / 3));
            }
            for (int twoCount = 1; twoCount <= maxOfTwo; twoCount++) {
                if ((length - (twoCount * 2)) % 3 == 0) {
                    int total = (length - (twoCount * 2)) / 3 + twoCount;
                    var sums = GetThrees(total);
                    RecursiveSingle(1, total, twoCount, sums);
                }
            }
        }

        private void BuildPairCounts() {
            for (int index1 = 0; index1 < _singles.Count; index1++) {
                var single1 = _singles[index1];
                _pairCount.Add(index1, new List<int>());
                for (int index2 = 0; index2 < _singles.Count; index2++) {
                    var single2 = _singles[index2];
                    if ((single1 & single2) == 0) {
                        _pairCount[index1].Add(index2);
                    }
                }
            }
        }

        private ulong Solve(int height) {
            var d = new ulong[_pairCount.Count];
            for (int index = 0; index < d.Length; index++) {
                d[index] = 1;
            }
            for (int remainingHeight = 2; remainingHeight <= height; remainingHeight++) {
                var nextD = new ulong[_pairCount.Count];
                foreach (var pair in _pairCount) {
                    foreach (var target in pair.Value) {
                        nextD[target] += d[pair.Key];
                    }
                }
                d = nextD;
            }
            ulong sum = 0;
            d.ToList().ForEach(x => sum += x);
            return sum;
        }

        private void RecursiveSingle(int current, int total, int remainingTwo, int[] sums) {
            for (int next = current; next <= total; next++) {
                sums[next - 1] = 2;
                if (next < total && remainingTwo != 1) {
                    RecursiveSingle(next + 1, total, remainingTwo - 1, sums);
                } else if (remainingTwo == 1) {
                    AddToSingle(sums);
                }
                sums[next - 1] = 3;
            }
        }

        private void AddToSingle(int[] single) {
            int sum = 0;
            ulong hash = 0;
            for (int index = 0; index < single.Length - 1; index++) {
                sum += single[index];
                hash += _powerOf2.GetPower(sum);
            }
            _singles.Add(hash);
        }

        private int[] GetThrees(int length) {
            var threes = new int[length];
            for (int index = 0; index < length; index++) {
                threes[index] = 3;
            }
            return threes;
        }
    }
}
