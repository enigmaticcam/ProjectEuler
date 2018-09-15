using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem106 : ProblemBase {
        private HashSet<uint> _hash = new HashSet<uint>();
        private ulong _sum;
        private FactorialWithHashULong _factorial = new FactorialWithHashULong();

        /*
            Given the rules on the set of numbers, we are obviously only comparing pairs of subsets
            of equal size. Consider the following two subsets:

            1 2 3 4 (numbers)
            1 1 2 2 (subsets)

            Clearly these two subsets can never be equal, because subset 2 contains all numbers that
            are higher than subset 1. Now examine this subset:

            1 2 3 4 (numbers)
            1 2 2 1 (subsets)

            These two subsets could in fact have equal numbers as seen by the example 1+4=2+3.
            Interestingly, how the two subsets interleave is all that matters. Consider:

            1 2 3 4 5 6 (numbers)
            1 0 2 0 2 1 (subsets)

            It can be proven that these two subsets can be equal as well. Notice if you removed
            the unused numbers (2, 4), the remaining matches exactly the previous sequence. So then
            it's just a matter of two questions:

            1. How many possible ways are there to interleave the two sets in a way they could
            be equal? Find this for each of pairs length 2, 3, 4, 5, and 6.
            2. For each occurrence above, how many possibilities are there to perform such an
            interleave given a set of length 12?

            For question 1, we pick a random number for the highest number, say 100. Then as we
            loop down and add numbers to each set, we add the next highest possible number, 99, 98, etc.
            Once the total number of the set that does not include the highest number is more than
            the set that does, we start adding the lowest possible numbers, 4, 3, 2, etc. If the sum
            of the set that doesn't include the highest number is at least equal to the other set,
            then that is a valid pattern.

            For example, given the interleave 1-2-2-1:

            1   2  2  1 (subsets)
            100 99 98 1 (numbers)

            For question 2, it's simply the count of selecting the length of the two subsets (r) combined
            from the full length set (n). That equation is: n! / (r! - (n - r)!)
         */

        public override string ProblemName {
            get { return "106: Special subset sums: meta-testing"; }
        }

        public override string GetAnswer() {
            Solve(12);
            return _sum.ToString();
        }

        private void Solve(int max) {
            for (int count = 2; count <= max / 2; count++) {
                Count(count, -1, new int[count * 2], 0, (ulong)max);
            }
        }

        private void Count(int current, int lastIndex, int[] groups, uint num, ulong max) {
            if (current == 0) {
                if (!_hash.Contains(num) && !_hash.Contains((uint)Math.Pow(2, groups.Count()) - 1 - num)) {
                    bool x = IsGood(groups);
                    if (x) {
                        var total = _factorial.Get(max) / (_factorial.Get((ulong)groups.Length) * _factorial.Get(max - (ulong)groups.Length));
                        _sum += total;
                    }
                    _hash.Add(num);
                }
            } else {
                for (int index = lastIndex + 1; index < groups.Length; index++) {
                    if (groups[index] == 0) {
                        groups[index] = 1;
                        num += (uint)Math.Pow(2, index);
                        Count(current - 1, index, groups, num, max);
                        groups[index] = 0;
                        num -= (uint)Math.Pow(2, index);
                    }
                }
            }
        }

        private bool IsGood(int[] groups) {
            int[] sum = new int[2];
            int start = 100;
            int maxGroup = groups.Last();
            sum[maxGroup] = 100;
            for (int index = groups.Count() - 2; index >= 0; index--) {
                if (sum[maxGroup] < sum[(maxGroup + 1) % 2]) {
                    start = index + 1;
                } else {
                    start -= 1;
                }
                sum[groups[index]] += start;
            }
            return sum[maxGroup] <= sum[(maxGroup + 1) % 2];
        }
    }
}
