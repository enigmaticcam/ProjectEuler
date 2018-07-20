namespace Euler_Logic.Problems {
    public class Problem136 : ProblemBase {
        private int[] _results;
        private int _count;

        public override string ProblemName {
            get { return "136: Singleton difference"; }
        }

        /*
            It seems that for any instance of n, the maximum value of y is n. So for all values of n, first find the highest possible value n can be, which is:
            x = (n * 2) - 1, y = n, z = 1. Then find the next highest. Subtract that from highest to get the increment. Using that increment, find the next
            value that's below or equal to max. If that value is more than 0, start with that value and then keep subtracting the increment until we go below 1.
            For each time we do that, check to see if we've gotten this number before. I couldn't use a typical hashset, it's too slow. Instead, I used an array
            of integers for all n, starting at 0. Every time we see a number, we increment the matching index by 1. Increasing from 1 to 0, we know that
            is the first time seeing that number, so we can add to our count. Increasing from 1 to 2 means we now know that number is duplicated, so we subtract
            from the count. Any other number we ignore. Return the count.
         */

        public override string GetAnswer() {
            int max = 50000000;
            _results = new int[max];
            BuildResults((ulong)max);
            return _count.ToString();
        }

        private void BuildResults(ulong max) {
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
                    if (result == 1) {
                        _count++;
                    } else if (result == 2) {
                        _count--;
                    }
                    highest -= increment;
                }
            }
        }
    }
}