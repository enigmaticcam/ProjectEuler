using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem12 : ProblemBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 12"; }
        }

        public override string GetAnswer() {
            return Answer1(new List<string>() {
                "0 <-> 2",
                "1 <-> 1",
                "2 <-> 0, 3, 4",
                "3 <-> 2, 4",
                "4 <-> 2, 3, 6",
                "5 <-> 6",
                "6 <-> 4, 5"
            });
            return "";
        }

        private string Answer1(List<string> input) {
            Dictionary<int, HashSet<int>> entries = new Dictionary<int, HashSet<int>>();
            HashSet<int> good = new HashSet<int>(GetChainEntriesFromLine(input[0]));
            HashSet<int> bad = new HashSet<int>();

            foreach (string line in input) {
                int value = GetValueFromLine(line);
                List<int> chain = GetChainEntriesFromLine(line);
                foreach (int number in chain) {
                    if (!entriesContainingZero.Contains(number)) {

                    }
                }

            }
            return "";
        }

        private int GetValueFromLine(string line) {
            return Convert.ToInt32(line.Substring(0, line.IndexOf(' ')));
        }

        private List<int> GetChainEntriesFromLine(string line) {
            int start = line.IndexOf('>') + 1;
            return line.Substring(start, line.Length - start).Split(',').Select(x => Convert.ToInt32(x.Trim())).ToList();
        }
    }
}
