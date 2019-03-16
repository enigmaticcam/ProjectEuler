using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem253 : ProblemBase {
        private Dictionary<string, Set> _hash = new Dictionary<string, Set>();

        /*
            Instead of considering various combinations of segment sizes, I looked at various combinations of
            the sizes of the spaces in between segments. So for 10 blocks, if you haven't connected anything
            then you have one blank equal to 10 with a maximum segment count of 0. If you then draw the piece 
            #5, you now have two sets of spaces: one that is lenth (5) and one that is length (4). Your maximum
            segment count is now 1.

            So for each possible combination of spaces, I ask the question: given this combination, what are
            the chances I will maintain my current max segment count, or increase my current max segment count.
            To calculate this, I use a recursive function that loops through each remaining piece, adjusts the
            spaces if that piece were drawn, and then get the chances of the resulting position by calling itself. 
            Based on the length of the space, these are the conditions that can occur:

            - Length 1 - this space is removed. Two segments are then connected, reducing my maximum segment
            count by 1
            - Length 2 - either piece will reduce the length of this segment without changing my maximum
            segment count
            - Length >2 - a piece can be removed at each end, simply reducing the length of this space. Or a
            piece in the middle can be removed, splitting the space and increasing the max segment count.

            As the recursive function dives deeper in the combination tree, a ratio is maintained. The chances
            of all children of a single combination are split between 100%, but the parent then splits its
            results by its parents children. A hash key is used to save work for unique combinations of spaces.
         */

        public override string ProblemName {
            get { return "253: Tidying up"; }
        }

        public override string GetAnswer() {
            int max = 40;
            var blanks = new decimal[max];
            blanks[0] = (decimal)max;
            BuildHash(blanks, 0, (decimal)max, 1);
            return Solve(max);
        }

        private string Solve(int max) {
            decimal sum = 0;
            var set = _hash[max.ToString()];
            foreach (var option in set.Options) {
                sum += option.Key * option.Value;
            }
            return Math.Round(sum, 6).ToString();
        }

        private Set BuildHash(decimal[] blanks, int maxIndex, decimal sum, decimal ratio) {
            var key = GetKey(blanks, maxIndex);
            if (!_hash.ContainsKey(key)) {
                var setToAdd = new Set();
                if (sum == 1) {
                    setToAdd.Options.Add(1, 1);
                } else {
                    Set subSet = null;
                    for (int index = 0; index <= maxIndex; index++) {
                        var blank = blanks[index];
                        if (blank == 1) {
                            // 1/x chance of removing blank completely
                            var temp = blanks[maxIndex];
                            blanks[index] = blanks[maxIndex];
                            subSet = BuildHash(blanks, maxIndex - 1, sum - 1, ratio * 1 / sum);
                            MergeSets(setToAdd, subSet, maxIndex + 1, 1 / sum);
                            blanks[index] = blank;
                            blanks[maxIndex] = temp;
                        } else if (blank == 2) {
                            // 2/x chance of reducing blank by 1
                            blanks[index] = 1;
                            subSet = BuildHash(blanks, maxIndex, sum - 1, ratio * 2 / sum);
                            MergeSets(setToAdd, subSet, maxIndex + 1, 2 / sum);
                            blanks[index] = blank;
                        } else {
                            // 2/x chance of reducing blank by 1
                            blanks[index]--;
                            subSet = BuildHash(blanks, maxIndex, sum - 1, ratio * 2 / sum);
                            MergeSets(setToAdd, subSet, maxIndex + 1, 2 / sum);
                            blanks[index] = blank;

                            // 1/x chance of splitting blank with same sides
                            if (blank % 2 == 1) {
                                blanks[index] = (decimal)((ulong)blanks[index] / 2);
                                blanks[maxIndex + 1] = blanks[index];
                                subSet = BuildHash(blanks, maxIndex + 1, sum - 1, ratio * 1 / sum);
                                MergeSets(setToAdd, subSet, maxIndex + 2, 1 / sum);
                                blanks[index] = blank;
                            }

                            // 2/x chance of splitting blank with different sides
                            decimal count = blank / 2 - 1;
                            for (decimal blank1 = 1; blank1 <= count; blank1++) {
                                blanks[index] = blank1;
                                blanks[maxIndex + 1] = blank - blank1 - 1;
                                subSet = BuildHash(blanks, maxIndex + 1, sum - 1, ratio * 2 / sum);
                                MergeSets(setToAdd, subSet, maxIndex + 2, 2 / sum);
                            }
                            blanks[index] = blank;
                        }
                    }
                }
                _hash.Add(key, setToAdd);
            }
            return _hash[key];
        }

        private void MergeSets(Set primeSet, Set setToAdd, int currentSegment, decimal ratio) {
            foreach (var option in setToAdd.Options) {
                var increase = Math.Max(currentSegment, option.Key);
                var result = ratio * option.Value;
                if (primeSet.Options.ContainsKey(increase)) {
                    primeSet.Options[increase] += result;
                } else {
                    primeSet.Options.Add(increase, result);
                }
            }
        }

        private string GetKey(decimal[] blanks, int maxIndex) {
            var sorted = blanks.Take(maxIndex + 1).OrderBy(x => x);
            var key = string.Join(",", sorted);
            return key;
        }

        private class Set {
            public Dictionary<int, decimal> Options { get; set; }

            public Set() {
                Options = new Dictionary<int, decimal>();
            }
        }
    }
}