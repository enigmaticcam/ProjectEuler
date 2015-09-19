using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem52 : IProblem {
        public string ProblemName {
            get { return "52: Permuted multiples"; }
        }

        public string GetAnswer() {
            return FindSmallest().ToString();
        }

        private int FindSmallest() {
            int num = 1;
            do {
                bool isGood = true;
                for (int factor = 2; factor <= 6; factor++) {
                    if (!IsPermuted(num, num * factor)) {
                        isGood = false;
                        break;
                    }
                }
                if (isGood) {
                    return num;
                }
                num++;
            } while (true);
        }

        private bool IsPermuted(int num, int result) {
            string numAsText = num.ToString();
            string resultAsText = result.ToString();
            for (int index = 0; index < numAsText.Length; index++) {
                string replace = numAsText.Substring(index, 1);
                if (numAsText.Replace(replace, "").Length != resultAsText.Replace(replace, "").Length) {
                    return false;
                }
            }
            return true;
        }
    }
}
