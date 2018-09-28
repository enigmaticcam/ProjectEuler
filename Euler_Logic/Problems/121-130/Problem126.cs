using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem126 : ProblemBase {
        private Dictionary<int, int> _counts = new Dictionary<int, int>();

        /*
            A simple brute force algorithm can be used on smaller variants to look
            for patterns. Assign each block to an x, y, z coordinate. To add a new
            layer, loop through each block and attempt to add a new block to all
            six sides. Don't add a block to a side if one already exist. You can use
            a three keyed dictionary for quick reference. Some time can be saved
            after looping through only the newly added blocks when adding a new layer.

            After brute forcing some numbers, I was able do determine the following 
            patterns:

            1. The first layer of any cuboid is 2(x * y + x * z + y * z)
            2. The second layer of any cuboid is 4 * x + 4 * y + 4 * z + first layer.
            3. All subsequent layers can be found by finding the difference between
            the two prior, adding 8, and adding that to the last layer.

            For example, if cuboid is 3,2,1:
            1 layer = 2(3 * 2 + 3 * 1 + 2 * 1) = 22
            2 layer = 4 * 3 + 4 * 2 + 4 * 1 + (layer 1) = 24 + 22 = 46
            3 layer = (layer 2) - (layer 1) + 8 + (layer 2) = 46 - 22 + 8 + 46 = 78
            4 layer = (layer 3) - (layer 2) + 8 + (layer 3) = 78 - 46 + 8 + 78 = 118

            So I simply looped through every unique variant of x, y, z where 
            z <= y <= x. For each varient, I calculated all layers until the value 
            exceeded some limit. I initially set the limit to 10000. Once that was done,
            I looked for some number that equaled 1000. None was found, so I set the
            limit to 20000, and viola!
         */

        public override string ProblemName {
            get { return "126: Cuboid layers"; }
        }

        public override string GetAnswer() {
            Solve(20000);
            return GetFirst().ToString();
        }

        private void Solve(int limit) {
            int x = 1;
            do {
                if (FirstLayerCount(x, 1, 1) > limit) {
                    break;
                }
                for (int y = 1; y <= x; y++) {
                    if (FirstLayerCount(x, y, 1) > limit) {
                        break;
                    }
                    for (int z = 1; z <= y; z++) {
                        var firstLayerCount = FirstLayerCount(x, y, z);
                        if (firstLayerCount > limit) {
                            break;
                        }
                        Count(x, y, z, firstLayerCount, limit);
                    }
                }
                x++;
            } while (true);
        }

        private int FirstLayerCount(int x, int y, int z) {
            return 2 * ((x * y) + (x * z) + (y * z));
        }

        private int GetFirst() {
            foreach (var key in _counts.Keys.OrderBy(x => x)) {
                if (_counts[key] == 1000) {
                    return key;
                }
            }
            return 0;
        }

        private void Count(int x, int y, int z, int firstLayerCount, int limit) {
            var first = firstLayerCount;
            var second = (x * 4) + (y * 4) + (z * 4) + first;
            AddCount(first);
            AddCount(second);
            int temp = 0;
            while (second <= limit) {
                temp = second;
                second = second - first + 8 + second;
                first = temp;
                AddCount(second);
            }
        }

        private void AddCount(int value) {
            if (!_counts.ContainsKey(value)) {
                _counts.Add(value, 1);
            } else {
                _counts[value]++;
            }
        }
    }
}
