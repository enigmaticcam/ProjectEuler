using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem32 : ProblemBase {
        private List<string> _perms = new List<string>();
        private HashSet<int> _unique = new HashSet<int>();

        public override string ProblemName {
            get { return "32: Pandigital products"; }
        }

        public override string GetAnswer() {
            GeneratePerms();
            return GetSums().ToString();
        }

        private int GetSums() {
            int sum = 0;
            foreach (string perm in _perms) {
                for (int a = 1; a < perm.Length - 1; a++) {
                    for (int b = a + 1; b < perm.Length; b++) {
                        int aAsNum = Convert.ToInt32(perm.Substring(0, a));
                        int bAsNum = Convert.ToInt32(perm.Substring(a, b - a));
                        int c = Convert.ToInt32(perm.Substring(b, perm.Length - b));
                        int result = aAsNum * bAsNum;
                        if (result == c) {
                            if (!_unique.Contains(result)) {
                                sum += result;
                                _unique.Add(result);
                            }
                        }
                    }
                }
            }
            return sum;
        }

        private void GeneratePerms() {
            _perms.Add("1");
            for (int digit = 2; digit <= 9; digit++) {
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
