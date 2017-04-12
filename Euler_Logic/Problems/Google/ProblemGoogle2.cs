using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class ProblemGoogle2 : GoogleBase {

        public override string ProblemName {
            get { return "Google: Revenge of the Pancakes"; }
        }

        public override string GetAnswer() {
            Solve();
            DownloadOutputFile();
            return "done";
        }

        private void Solve() {
            foreach (string stack in _tests) {
                FindMinimumFlipCount(stack);
            }
        }

        private void FindMinimumFlipCount(string stack) {
            int flipCount = 0;
            for (int index = 1; index < stack.Length; index++) {
                if (stack.Substring(index, 1) != stack.Substring(index - 1, 1)) {
                    flipCount++;
                }
            }
            if (stack.Substring(stack.Length - 1, 1) == "-") {
                flipCount++;
            }
            _results.Add(flipCount.ToString());
        }
    }
}
