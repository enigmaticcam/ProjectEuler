using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem40 : IProblem {
        private HashSet<int> _checks = new HashSet<int>();

        public string ProblemName {
            get { return "40: Champernowne's constant"; }
        }

        public string GetAnswer() {
            _checks.Add(1);
            _checks.Add(10);
            _checks.Add(100);
            _checks.Add(1000);
            _checks.Add(10000);
            _checks.Add(100000);
            _checks.Add(1000000);
            return GetConstant().ToString();
        }

        public int GetConstant() {
            IEnumerable<int> checks = _checks.OrderBy(x => x);
            int num = 1;
            int digitCount = 0;
            int product = 1;
            int checkIndex = 0;
            do {
                digitCount += num.ToString().Length;
                if (digitCount >= checks.ElementAt(checkIndex)) {
                    int digit = Convert.ToInt32(num.ToString().Substring(num.ToString().Length - (digitCount - checks.ElementAt(checkIndex)) - 1, 1));
                    product *= digit;
                    checkIndex++;
                    if (checkIndex == _checks.Count) {
                        return product;
                    }
                }
                num++;
            } while (true);
        }

    }
}
