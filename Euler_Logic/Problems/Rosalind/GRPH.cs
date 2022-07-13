using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.Rosalind {
    public class GRPH : RosalindBase {
        public override string ProblemName => "Rosalind: GRPH";

        public override string GetAnswer() {
            Solve(Input());
            return "done";
        }

        private void Solve(List<string> input) {
            var strands = GetStrands(input);
            FindEdges(strands);
            var result = new List<string>();
            foreach (var strand in strands) {
                foreach (var edge in strand.Edges) {
                    result.Add(strand.Name + " " + edge.Name);
                }
            }
            SaveOutput(result);
        }

        private void FindEdges(List<Strand> strands) {
            foreach (var strand1 in strands) {
                foreach (var strand2 in strands) {
                    if (strand1 != strand2 && strand1.Text.Substring(strand1.Text.Length - 3) == strand2.Text.Substring(0, 3)) strand1.Edges.Add(strand2);
                }
            }
        }

        private List<Strand> GetStrands(List<string> input) {
            var strands = new List<Strand>();
            Strand strand = null;
            foreach (var line in input) {
                if (line[0] == '>') {
                    strand = new Strand() { Edges = new List<Strand>() };
                    strands.Add(strand);
                    strand.Name = line.Substring(1);
                } else {
                    strand.Text += line;
                }
            }
            return strands;
        }

        private class Strand {
            public string Name { get; set; }
            public string Text { get; set; }
            public List<Strand> Edges { get; set; }
        }
    }
}
