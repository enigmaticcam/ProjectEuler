using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem49 : ProblemBase {
        public override string ProblemName {
            get { return "49: Prime Permutations"; }
        }

        public override string GetAnswer() {
            List<int> primes = GetPrimes();
            Dictionary<string, List<int>> permutations = GetPermutations(primes);
            foreach (string key in permutations.Keys) {
                List<int> nums = permutations[key];
                if (nums.Count >= 3 && nums[0] != 1487) {
                    for (int a = 0; a <= nums.Count - 3; a++) {
                        for (int b = a + 1; b <= nums.Count - 2; b++) {
                            for (int c = b + 1; c <= nums.Count - 1; c++) {
                                if (nums[c] - nums[b] == nums[b] - nums[a]) {
                                    return nums[a].ToString() + nums[b].ToString() + nums[c].ToString();
                                }
                            }
                        }
                    }
                }
            }
            return "";
        }

        private List<int> GetPrimes() {
            List<int> primes = new List<int>();
            for (int i = 1001; i <= 9999; i += 2) {
                bool hasFactor = false;
                for (int j = 3; j <= Math.Sqrt(i); j++) {
                    if (i % j == 0) {
                        hasFactor = true;
                        break;
                    }
                }
                if (!hasFactor) {
                    primes.Add(i);
                }
            }
            return primes;
        }

        private Dictionary<string, List<int>> GetPermutations(List<int> primes) {
            Dictionary<string, List<int>> permutations = new Dictionary<string, List<int>>();
            foreach (int prime in primes) {
                string primeAsText = prime.ToString();
                string permutationKey = "";
                for (int i = 0; i <= 9; i++) {
                    permutationKey += (primeAsText.Length - primeAsText.Replace(i.ToString(), "").Length).ToString();
                }                    
                if (!permutations.ContainsKey(permutationKey)) {
                    permutations.Add(permutationKey, new List<int>());
                }
                permutations[permutationKey].Add(prime);
            }
            return permutations;
        }
    }
}
