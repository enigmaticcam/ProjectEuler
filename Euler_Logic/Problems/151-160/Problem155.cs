using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem155 : ProblemBase {
        private Dictionary<int, Dictionary<int, HashSet<int>>> _counts = new Dictionary<int, Dictionary<int, HashSet<int>>>();
        private Dictionary<int, HashSet<int>> _valid = new Dictionary<int, HashSet<int>>();

        /*
            Ignore the fact that the capacitors use 60 capacitance. Instead, let's just use 1. When (n) is 3, then
            the total ways would be:

            Using 1 capacitor:
            1/1 = 1

            Using 2 capacitors:
            2/1 = 1+1
            1/2 = 1/(1+1)

            Using 3 capacitors:
            3/1 = 1+1+1
            1/3 = 1/(1+1+1)
            3/2 = 1+1/(1+1)
            2/3 = 1/(1+1/(1+1))

            We can therefore determine when using 4 capacitors by looping through all the ways using 1 and 3 capacitors.
            For each combination, we consider the parallel and series of the two. For parallel we add the two sets, and 
            for series we add the inverse of the two sets. See below for 4:

            4/1 = 1+(3/1)
            1/4 = 1/(3/1)
            4/3 = 1+(1/3)
            3/4 = 1/(4/3)
            5/2 = 1+(3/2)
            2/5 = 1/(5/2)
            5/3 = 1+(2/3)
            3/5 = 1/(5/3)

            Adding the count of this new list to the count of the prior lists gives us 15 when (n) is 4. We contintue to
            do this up to 18. However, we make sure that we always reduce the resulting fraction and ensure the reduced 
            fraction has never been used before. To assist with this, I use a dictionary of fractions where the key is 
            the numerator and the value is a list of all distinct denominators.
         */

        public override string ProblemName {
            get { return "155: Counting Capacitor Circuits"; }
        }

        public override string GetAnswer() {
            return Solve(18).ToString();
        }

        private int Solve(int max) {
            _counts.Add(1, new Dictionary<int, HashSet<int>>());
            _counts[1].Add(1, new HashSet<int>() { 1 });
            for (int count = 2; count <= max; count++) {
                _counts.Add(count, new Dictionary<int, HashSet<int>>());
                for (int a = 1; a < count; a++) {
                    int b = count - a;
                    foreach (var top1 in _counts[a]) {
                        foreach (var bottom1 in top1.Value) {
                            foreach (var top2 in _counts[b]) {
                                foreach (var bottom2 in top2.Value) {
                                    var y = LCM.GetLCM(bottom1, bottom2);
                                    var x = (top1.Key * y / bottom1) + (top2.Key * y / bottom2);
                                    AddXOverY(count, x, y);
                                }
                            }
                        }
                    }
                }
            }
            return _valid.Select(x => x.Value.Count).Sum();
        }

        private void AddXOverY(int count, int x, int y) {
            // x / y
            var addX = x;
            var addY = y;
            var gcd = GCD.GetGCD(addX, addY);
            if (gcd != 1) {
                addX /= gcd;
                addY /= gcd;
            }
            AddCount(count, addX, addY);
            AddValid(addX, addY);

            // y / x
            addX = y;
            addY = x;
            gcd = GCD.GetGCD(addX, addY);
            if (gcd != 1) {
                addX /= gcd;
                addY /= gcd;
            }
            AddCount(count, addX, addY);
            AddValid(addX, addY);
        }

        private void AddCount(int count, int addX, int addY) {
            if (!_counts[count].ContainsKey(addX)) {
                _counts[count].Add(addX, new HashSet<int>());
            }
            _counts[count][addX].Add(addY);
        }

        private void AddValid(int addX, int addY) {
            if (!_valid.ContainsKey(addX)) {
                _valid.Add(addX, new HashSet<int>());
            }
            _valid[addX].Add(addY);
        }
    }
}