using System.Collections.Generic;

namespace Euler_Logic.Problems.Rosalind {
    public class SUBS : RosalindBase {
        public override string ProblemName => "Rosalind: SUBS";

        public override string GetAnswer() {
            Solve(Input());
            return "done";
        }

        private void Solve(List<string> input) {
            var result = new List<int>();
            int index = 0;
            do {
                index = input[0].IndexOf(input[1], index);
                if (index == -1) break;
                result.Add(index + 1);
                index++;
            } while (true);
            SaveOutput(string.Join(" ", result));
        }

        
    }
}
