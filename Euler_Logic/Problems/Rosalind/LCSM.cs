using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.Rosalind {
    public class LCSM : RosalindBase {
        public override string ProblemName => "Rosalind: LCSM";

        public override string GetAnswer() {
            return Solve(Input());
        }

        private string Solve(List<string> input) {
            var strands = GetStrands(input);
            var perms = new List<string>() { "A", "C", "G", "T" };
            string last = "";
            do {
                for (int index = 0; index < perms.Count; index++) {
                    var perm = perms[index];
                    foreach (var strand in strands) {
                        if (strand.IndexOf(perm) == -1) {
                            perms.RemoveAt(index);
                            index--;
                            break;
                        }
                    }
                }
                if (perms.Count == 0) return last;
                last = perms.First();
                perms = GetNextPermutation(perms);
            } while (true);
        }

        private List<string> GetNextPermutation(List<string> permutations) {
            var next = new List<string>();
            foreach (var perm in permutations) {
                next.Add("A" + perm);
                next.Add("C" + perm);
                next.Add("G" + perm);
                next.Add("T" + perm);
            }
            return next;
        }

        private List<string> GetStrands(List<string> input) {
            var strands = new List<string>();
            var current = new StringBuilder();
            foreach (var line in input) {
                if (line[0] == '>' && current.Length > 0) {
                    strands.Add(current.ToString());
                    current.Clear();
                } else {
                    current.Append(line);
                }
            }
            return strands;
        }
    }
}
