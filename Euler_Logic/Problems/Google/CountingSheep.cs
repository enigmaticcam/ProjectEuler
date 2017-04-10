using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class ProblemGoogle1 : GoogleBase {
        public override string ProblemName {
            get { return "Google: Counting Sheep"; }
        }

        public override string GetAnswer() {
            Solve();
            DownloadOutputFile();
            return "done";
        }

        private void Solve() {
            foreach (string test in _tests) {
                CountTheSheep(Convert.ToInt32(test));
            }
        }

        private void CountTheSheep(int start) {
            if (start != 0) {
                HashSet<string> digits = new HashSet<string>();
                int num = start;
                do {
                    string text = num.ToString();
                    for (int index = 0; index < text.Length; index++) {
                        string digit = text.Substring(index, 1);
                        if (!digits.Contains(digit)) {
                            digits.Add(digit);
                        }
                        if (digits.Count == 10) {
                            break;
                        }
                    }
                    num += start;
                } while (digits.Count != 10);
                num -= start;
                _results.Add(num.ToString());
            } else {
                _results.Add("INSOMNIA");
            }
        }
    }
}
