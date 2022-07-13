using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.Rosalind {
    public class CONS : RosalindBase {
        public override string ProblemName => "Rosalind: CONS";

        public override string GetAnswer() {
            Solve(Input());
            return "done";
        }

        private void Solve(List<string> input) {
            var strands = GetStrands(input);
            var results = GetResults(strands);
            var final = new List<string>();
            final.Add(GetConsensus(results));
            results.ForEach(x => final.Add(GetFinal(x)));
            SaveOutput(final);
        }

        private List<string> GetStrands(List<string> input) {
            var strands = new List<string>();
            var strand = new StringBuilder();
            foreach (var line in input) {
                if (line[0] == '>') {
                    if (strand.Length != 0) strands.Add(strand.ToString());
                    strand.Clear();
                } else {
                    strand.Append(line);
                }
            }
            strands.Add(strand.ToString());
            return strands;
        }

        private List<Result> GetResults(List<string> strands) {
            var results = new List<Result>();
            results.Add(GetResults('A', strands));
            results.Add(GetResults('C', strands));
            results.Add(GetResults('G', strands));
            results.Add(GetResults('T', strands));
            return results;
        }

        private Result GetResults(char digit, List<string> strands) {
            var result = new Result() { Digit = digit, Counts = new List<int>() };
            for (int colIndex = 0; colIndex < strands[0].Length; colIndex++) {
                int count = 0;
                for (int strandIndex = 0; strandIndex < strands.Count; strandIndex++) {
                    if (strands[strandIndex][colIndex] == digit) count++;
                }
                result.Counts.Add(count);
            }
            return result;
        }

        private string GetConsensus(List<Result> results) {
            var consensus = new List<char>();
            for (int index = 0; index < results[0].Counts.Count; index++) {
                var best = results.OrderByDescending(x => x.Counts[index]).First();
                consensus.Add(best.Digit);
            }
            return new string(consensus.ToArray());
        }

        private string GetFinal(Result result) {
            var final = new string(new char[1] { result.Digit });
            final += ": ";
            final += string.Join(" ", result.Counts);
            return final;
        }

        private class Result {
            public char Digit { get; set; }
            public List<int> Counts { get; set; }
        }
    }
}
