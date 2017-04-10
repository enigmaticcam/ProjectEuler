using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem24 : ProblemBase {
        List<string> _permutations = new List<string>();

        public override string ProblemName {
            get { return "24: Lexicographic permutations"; }
        }

        public override string GetAnswer() {
            BuildPermutations();
            return GetPermutation(1000000);
        }

        private string GetPermutation(int index) {
            _permutations.Sort();
            return _permutations[index - 1];
        }

        private void BuildPermutations() {
            _permutations.Add("0");
            for (int a = 1; a <= 9; a++) {
                List<string> tempPerms = new List<string>();
                for (int b = 0; b < _permutations.Count; b++) {
                    for (int c = 0; c <= _permutations[b].Length; c++) {
                        tempPerms.Add(_permutations[b].Insert(c, a.ToString()));
                    }
                }
                _permutations = tempPerms;
            }
        }
    }
}
