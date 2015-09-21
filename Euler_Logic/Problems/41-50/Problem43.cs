using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem43 : IProblem {
        private List<string> _perms = new List<string>();

        public string ProblemName {
            get { return "43: Sub-string divisibility"; }
        }

        public string GetAnswer() {
            GeneratePerms();
            return SumSubStrings().ToString();
        }

        private decimal SumSubStrings() {
            decimal sum = 0;
            foreach (string perm in _perms) {
                if (Convert.ToInt32(perm.Substring(1, 3)) % 2 == 0
                    && Convert.ToInt32(perm.Substring(2, 3)) % 3 == 0
                    && Convert.ToInt32(perm.Substring(3, 3)) % 5 == 0
                    && Convert.ToInt32(perm.Substring(4, 3)) % 7 == 0
                    && Convert.ToInt32(perm.Substring(5, 3)) % 11 == 0
                    && Convert.ToInt32(perm.Substring(6, 3)) % 13 == 0
                    && Convert.ToInt32(perm.Substring(7, 3)) % 17 == 0
                    ) {
                        sum += Convert.ToDecimal(perm);
                }
            }

            return sum;
        }

        private void GeneratePerms() {
            _perms.Add("0");
            for (int digit = 1; digit <= 9; digit++) {
                List<string> temp = new List<string>();
                foreach (string perm in _perms) {
                    for (int index = 0; index <= perm.Length; index++) {
                        temp.Add(perm.Insert(index, digit.ToString()));
                    }
                }
                _perms = temp;
            }
        }
    }
}
