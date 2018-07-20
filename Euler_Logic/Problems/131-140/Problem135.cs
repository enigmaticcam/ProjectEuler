namespace Euler_Logic.Problems {
    public class Problem135 : ProblemBase {
        private int[] _results;
        private int _count;

        public override string ProblemName {
            get { return "135: Same differences"; }
        }

        /*
            It seems that for any instance of n, the maximum value of y is n. So for all values of n, first find the highest possible value n can be, which is:
            x = (n * 2) - 1, y = n, z = 1. Then find the next highest. Subtract that from highest to get the increment. Using that increment, find the next
            value that's below or equal to max. If that value is more than 0, start with that value and then keep subtracting the increment until we go below 1.
            For each time we do that, check to see if we've gotten this number before. I couldn't use a typical dictionary, it's too slow. Instead, I used an array
            of integers for all n, starting at 0. Every time we see a number, we increment the matching index by 1. Increasing from 9 to 10, we know that
            is the first time seeing that number at 10, so we can add to our count. Increasing from 10 to 11 means we now know that number is more than 10, so we subtract
            from the count. Any other number we ignore. Return the count.
         */

        public override string GetAnswer() {
            int max = 1000000;
            int distinctCount = 10;
            _results = new int[max];
            BuildResults((ulong)max, distinctCount);
            return _count.ToString();
        }

        private void BuildResults(ulong max, int distinctCount) {
            for (ulong number = 2; number <= max; number++) {

                // Find highest
                ulong y = number;
                ulong x = y * 2 - 1;
                ulong z = 1;
                ulong highest = (x * x) - (y * y) - (z * z);

                // Find next highest
                x -= 1;
                z += 1;
                ulong nextHighest = (x * x) - (y * y) - (z * z);
                ulong increment = highest - nextHighest;

                if (highest > max) {
                    highest -= (((highest - max) / increment) + 1) * increment;
                }
                while (highest < max && highest > 0) {
                    _results[highest]++;
                    int result = _results[highest];
                    if (result == distinctCount) {
                        _count++;
                    } else if (result == distinctCount + 1) {
                        _count--;
                    }
                    highest -= increment;
                }
            }
        }
    }
}