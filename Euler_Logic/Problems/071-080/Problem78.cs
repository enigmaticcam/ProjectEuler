using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem78 : IProblem {
        private List<int> _p = new List<int>();
        private List<int> _sign = new List<int>();

        public string ProblemName {
            get { return "78: Coin Partitions"; }
        }

        public string GetAnswer() {
            BuildSigns();
            return Solve().ToString();
        }

        private void BuildSigns() {
            _sign.Add(1);
            _sign.Add(1);
            _sign.Add(-1);
            _sign.Add(-1);
        }

        public int Solve() {
            int num = 1;
            int count = 1;
            _p.Add(1);
            _p.Add(1);
            while (count % 1000000 != 0) {
                num++;
                count = 0;
                int k = 1;
                int sign = 0;
                int m = 1;
                while (k <= num) {
                    count += (_p[num - k] * _sign[sign]) % 1000000;
                    if (m > 0) {
                        m *= -1;
                    } else {
                        m = (m * -1) + 1;
                    }
                    k = (m * ((3 * m) - 1)) / 2;
                    sign = (sign + 1) % _sign.Count;
                }
                _p.Add(count);
            }
            return num;
        }
       
    }
}
