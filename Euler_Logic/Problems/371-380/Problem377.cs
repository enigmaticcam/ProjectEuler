using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem377 : ProblemBase {
        private Dictionary<ulong, SumCount> _sums = new Dictionary<ulong, SumCount>();

        public override string ProblemName {
            get { return "377: Sum of digits, experience 13"; }
        }

        public override string GetAnswer() {
            var x = GetSum(5).Sum.ToString();
            return x;
        }

        private SumCount GetSum(ulong num) {
            if (!_sums.ContainsKey(num)) {
                if (num == 1) {
                    _sums.Add(1, new SumCount() { Count = 1, Sum = 1 });
                } else {
                    var sum = new SumCount();
                    for (ulong x = Math.Min(num, 9); x >= 1; x--) {

                        var next = GetSum(num - x);
                        sum.Count += next.Count;
                        sum.Sum += (next.Sum * 10) + (x * next.Count);
                    }
                    _sums.Add(num, sum);
                }
            }
            return _sums[num];
        }

        private class SumCount {
            public ulong Count { get; set; }
            public ulong Sum { get; set; }
        }
    }
}
