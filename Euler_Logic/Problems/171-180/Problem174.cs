using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem174 : IProblem {
        private Dictionary<ulong, int> _counts = new Dictionary<ulong, int>();
        private Dictionary<int, int> _countOfCounts = new Dictionary<int, int>();

        public string ProblemName {
            get { return "174: Counting the number of 'hollow' square laminae that can form one, two, three, ... distinct arrangements"; }
        }

        public string GetAnswer() {
            BuildCounts(1000000);
            CountCounts();
            return Solve(10).ToString();
        }

        public void BuildCounts(ulong max) {
            ulong sum = 0;
            for (ulong num = 1; num <= max / 4; num++) {
                ulong subSum = (num + 1) * 4;
                ulong nextNum = subSum;
                while (subSum <= max) {
                    sum += 1;
                    if (_counts.ContainsKey(subSum)) {
                        _counts[subSum] += 1;
                    } else {
                        _counts.Add(subSum, 1);
                    }
                    nextNum = ((nextNum / 4) + 2) * 4;
                    subSum += nextNum;
                }
            }
        }

        public void CountCounts() {
            foreach (int count in _counts.Values) {
                if (_countOfCounts.ContainsKey(count)) {
                    _countOfCounts[count] += 1;
                } else {
                    _countOfCounts.Add(count, 1);
                }
            }
        }

        public int Solve(int maxCount) {
            int sum = 0;
            for (int num = 1; num <= maxCount; num++) {
                sum += _countOfCounts[num];
            }
            return sum;
        }
    }
}
