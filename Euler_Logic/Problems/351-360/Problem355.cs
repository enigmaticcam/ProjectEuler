using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem355 : ProblemBase {
        public override string ProblemName {
            get { return "355: Maximal coprime subset"; }
        }

        public override string GetAnswer() {
            BruteForce(50);
            return _best.ToString();
        }

        private Dictionary<Tuple<ulong, ulong>, ulong> _bestOf2 = new Dictionary<Tuple<ulong, ulong>, ulong>();
        private ulong Best(Tuple<ulong, ulong> p, ulong max) {
            if (!_bestOf2.ContainsKey(p)) {
                if (p.Item1 > max / 2 || p.Item2 > max / 2 || p.Item1 * p.Item2 > max) {
                    _bestOf2.Add(p, Best(p.Item1, max) + Best(p.Item2, max));
                } else {
                    ulong best = Best(p.Item1, max) + Best(p.Item2, max);
                    ulong higher = Math.Max(p.Item1, p.Item2);
                    ulong lower = Math.Min(p.Item1, p.Item2);
                    ulong num = higher;
                    do {
                        ulong next = max / num;
                        if (next * num > best) {
                            best = next * num;
                        }
                        num *= higher;
                    } while (num * lower < max);
                    _bestOf2.Add(p, best);
                }
            }
            return _bestOf2[p];
        }

        private Dictionary<ulong, ulong> _bestOf1 = new Dictionary<ulong, ulong>();
        private ulong Best(ulong p, ulong max) {
            if (!_bestOf1.ContainsKey(p)) {
                var log = (ulong)Math.Log(max, p);
                _bestOf1.Add(p, (ulong)Math.Pow(p, log));
            }
            return _bestOf1[p];
        }

        private void BruteForce(ulong max) {
            BruteForceRecursive(new bool[max + 1], 1, max);
        }

        private ulong _best = 0;
        private void BruteForceRecursive(bool[] nums, ulong current, ulong max) {
            bool isGood = true;
            ulong sum = current + 1;
            for (ulong num = 2; num < current; num++) {
                if (nums[num]) {
                    if (GCD.GetGCD(current, num) != 1) {
                        isGood = false;
                        break;
                    }
                    sum += num;
                }
            }
            if (isGood) {
                if (_best < sum) {
                    _best = sum;
                    if (_best == 275) {
                        bool stop = true;
                    }
                }
            }
            if (current < max) {
                if (isGood) {
                    nums[current] = true;
                    BruteForceRecursive(nums, current + 1, max);
                    nums[current] = false;
                }
                BruteForceRecursive(nums, current + 1, max);
            }
        }
    }
}
