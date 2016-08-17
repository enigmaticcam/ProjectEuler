using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem88 : IProblem {
        private int[] _numbers = new int[12000];
        private Dictionary<int, Dictionary<int, Dictionary<int, bool>>> _hash = new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();

        public string ProblemName {
            get { return "88: Product-sum numbers"; }
        }

        public string GetAnswer() {
            return Solve(12000).ToString();
        }

        private int Solve(int max) {
            HashSet<int> numsFound = new HashSet<int>();
            int sum = 0;
            for (int digits = 2; digits <= max; digits++) {
                int k = digits;
                do {
                    k++;
                } while (!FindLowest(digits, k, k));
                if (!numsFound.Contains(k)) {
                    sum += k;
                    numsFound.Add(k);
                }
            }
            return sum;
        }

        private bool FindLowest(int digitsRemaining, int sum, int mult) {
            for (int num = 1; num <= sum; num++) {
                if (mult % num == 0) {
                    int newSum = sum - num;
                    int newMult = mult / num;
                    if (digitsRemaining == 2) {
                        if (newSum == newMult) {
                            return true;
                        }
                    } else {
                        if (!_hash.ContainsKey(digitsRemaining - 1)) {
                            _hash.Add(digitsRemaining - 1, new Dictionary<int, Dictionary<int, bool>>());
                        }
                        if (!_hash[digitsRemaining - 1].ContainsKey(newSum)) {
                            _hash[digitsRemaining - 1].Add(newSum, new Dictionary<int, bool>());
                        }
                        if (!_hash[digitsRemaining - 1][newSum].ContainsKey(newMult)) {
                            _hash[digitsRemaining - 1][newSum].Add(newMult, FindLowest(digitsRemaining - 1, newSum, newMult));
                        }
                        if (_hash[digitsRemaining - 1][newSum][newMult]) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
