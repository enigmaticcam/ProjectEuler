using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem95 : ProblemBase {
        private int[] _nums;

        public override string ProblemName {
            get { return "95: Amicable chains"; }
        }

        public override string GetAnswer() {
            BuildNums();
            return Solve().ToString();
        }

        private void BuildNums() {
            _nums = new int[1000000 + 1];
            for (int num = 1; num <= 1000000; num++) {
                for (int composite = 2; composite * num <= 1000000; composite++) {
                    _nums[composite * num] += num;
                }
            }
        }

        private int Solve() {
            HashSet<int> distinct = new HashSet<int>();
            int max = 0;
            int lowest = 0;
            for (int num = 2; num <= 1000000; num++) {
                distinct.Clear();
                distinct.Add(num);
                int next = _nums[num];
                int min = Math.Min(next, num);
                int length = 0;
                bool chainFound = false;
                while (next != 1) {
                    distinct.Add(next);
                    length++;
                    if (next < min) {
                        min = next;
                    }
                    if (next > 1000000) {
                        break;
                    }
                    if (_nums[next] == next) {
                        break;
                    }
                    next = _nums[next];
                    if (distinct.Contains(next)) {
                        if (next == num) {
                            chainFound = true;
                        }
                        break;
                    }
                }
                if (chainFound && length > max) {
                    max = length;
                    lowest = min;
                }
            }
            return lowest;
        }

    }
}
