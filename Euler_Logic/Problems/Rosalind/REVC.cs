using System.Collections.Generic;

namespace Euler_Logic.Problems.Rosalind {
    public class REVC : RosalindBase {
        public override string ProblemName => "Rosalind: REVC";

        public override string GetAnswer() {
            return Solve(Input());
        }

        private string Solve(List<string> input) {
            var next = new char[input[0].Length];
            for (int index = 0; index < next.Length; index++) {
                switch (input[0][index]) {
                    case 'A':
                        next[next.Length - index - 1] = 'T';
                        break;
                    case 'T':
                        next[next.Length - index - 1] = 'A';
                        break;
                    case 'C':
                        next[next.Length - index - 1] = 'G';
                        break;
                    case 'G':
                        next[next.Length - index - 1] = 'C';
                        break;
                }
            }
            return new string(next);
        }
    }
}
