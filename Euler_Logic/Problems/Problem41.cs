using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem41 : IProblem {
        private List<string> _perms = new List<string>();
        private decimal _best;

        public string ProblemName {
            get { return "41: Pandigital prime"; }
        }

        public string GetAnswer() {
            GeneratePerms();
            return _best.ToString();
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
                GetLargestPerm();
            }
        }

        private void GetLargestPerm() {
            foreach (string perm in _perms) {
                decimal permAsDecimal = Convert.ToDecimal(perm);
                if (permAsDecimal > _best && IsPrime(permAsDecimal)) {
                    _best = permAsDecimal;
                }
            }
        }

        private bool IsPrime(decimal num) {
            if (num == 2) {
                return true;
            } else if (num % 2 == 0) {
                return false;
            } else {
                for (ulong factor = 3; factor <= Math.Sqrt((double)num); factor += 2) {
                    if (num % factor == 0) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
