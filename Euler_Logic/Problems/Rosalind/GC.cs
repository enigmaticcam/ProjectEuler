using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.Rosalind {
    public class GC : RosalindBase {
        public override string ProblemName => "Rosalind: GC";

        public override string GetAnswer() {
            Solve(Input());
            return "done";
        }

        private void Solve(List<string> input) {
            var strands = new List<Strand>();
            Strand current = null;
            foreach (var line in input) {
                if (line[0] == '>') {
                    current = new Strand();
                    current.Name = line.Substring(1);
                    strands.Add(current);
                } else {
                    current.Text += line;
                }
            }
            foreach (var strand in strands) {
                decimal count = strand.Text.Length - strand.Text.Replace("C", "").Replace("G", "").Length;
                count = (count / strand.Text.Length) * 100;
                strand.Ratio = count;
            }
            var best = strands.OrderByDescending(x => x.Ratio).First();
            SaveOutput(best.Name, best.Ratio.ToString());
        }

        private class Strand {
            public string Name { get; set; }
            public string Text { get; set; }
            public decimal Ratio { get; set; }
        }
    }
}
