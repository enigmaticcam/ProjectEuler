using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem265 : ProblemBase {
        private Dictionary<ulong, List<ulong>> _chains = new Dictionary<ulong, List<ulong>>();
        private ulong _sum = 0;
        private int _offset;

        /*
            First I build a dictionary of which numbers will chain to other numbers. This is easy to do.
            I simply compare (x) to (y) where (x != y). Take the all but the first digits of (x) and
            compare to all but the last digits of (y). If they are the same, then they can chain.

            Then I loop through all possibilities of chains starting with 0. I use a boolean array to
            ensure a number is not used more than once. If I use all numbers then I have a solution.

            Finally I calculate the value of each solution. For whatever reason this was tricky and took
            some time to figure out how to do correctly. Essentially I loop through the numbers (I do
            this backwards so I don't have to figure out which power of 2 to start with) and check if
            the last digit in binary is a 1. If it is, multiply it by the current power of 2. If not,
            then move on. Increase the power of 2 by 2 each round. Sum the result.
         */

        public override string ProblemName {
            get { return "265: Binary Circles"; }
        }

        public override string GetAnswer() {
            ulong maxPowerOf2 = 5;
            _offset = (int)maxPowerOf2 - 1;
            BuildChains(maxPowerOf2);
            Solve(maxPowerOf2);
            return _sum.ToString();
        }

        private void BuildChains(ulong maxPowerOf2) {
            var max = (ulong)Math.Pow(2, maxPowerOf2) - 1;
            var mask = (ulong)Math.Pow(2, maxPowerOf2 - 1) - 1;
            for (ulong num = 0; num <= max; num++) {
                _chains.Add(num, new List<ulong>());
                for (ulong chain = 0; chain <= max; chain++) {
                    if (num != chain) {
                        var x = num & mask;
                        var y = (chain >> 1) & mask;
                        if (x == y) {
                            _chains[num].Add(chain);
                        }
                    }
                }
            }
        }

        private void Solve(ulong maxPowerOf2) {
            var max = (ulong)Math.Pow(2, maxPowerOf2) - 1;
            var selected = new bool[max + 1];
            var final = new ulong[max];
            selected[0] = true;
            LookForCircles(0, selected, max, final, 0);
        }

        private void LookForCircles(ulong last, bool[] selected, ulong remaining, ulong[] final, int count) {
            foreach (var num in _chains[last]) {
                if (!selected[num]) {
                    selected[num] = true;
                    final[count] = num;
                    if (remaining > 1) {
                        LookForCircles(num, selected, remaining - 1, final, count + 1);
                    } else if (_chains[num].Contains(0)) {
                        AddToSum(final);
                    }
                    selected[num] = false;
                }
            }
        }

        private void AddToSum(ulong[] final) {
            ulong powerOf2 = 1;
            ulong sum = 0;
            for (int index = final.Length - 1; index >= 0; index--) {
                ulong num = final[index];
                num >>= _offset;
                if (num == 1) {
                    sum += powerOf2;
                }
                powerOf2 *= 2;
            }
            _sum += sum;
        }
    }
}