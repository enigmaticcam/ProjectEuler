using System.Collections.Generic;
using System.Text;

namespace Euler_Logic.Problems.Rosalind {
    public class PROT : RosalindBase {
        public override string ProblemName => "Rosalind: PROT";

        public override string GetAnswer() {
            Solve(Input());
            return "done";
        }

        private void Solve(List<string> input) {
            var hash = GetHash();
            var text = new StringBuilder();
            for (int index = 0; index < input[0].Length - 3; index += 3) {
                var sub = input[0].Substring(index, 3);
                text.Append(hash[sub]);
            }
            SaveOutput(text.ToString());
        }

        private Dictionary<string, string> GetHash() {
            var hash = new Dictionary<string, string>();
            hash.Add("UUU", "F");
            hash.Add("UUC", "F");
            hash.Add("UUA", "L");
            hash.Add("UUG", "L");
            hash.Add("UCU", "S");
            hash.Add("UCC", "S");
            hash.Add("UCA", "S");
            hash.Add("UCG", "S");
            hash.Add("UAU", "Y");
            hash.Add("UAC", "Y");
            hash.Add("UAA", "S");
            hash.Add("UAG", "S");
            hash.Add("UGU", "C");
            hash.Add("UGC", "C");
            hash.Add("UGA", "S");
            hash.Add("UGG", "W");
            hash.Add("CUU", "L");
            hash.Add("CUC", "L");
            hash.Add("CUA", "L");
            hash.Add("CUG", "L");
            hash.Add("CCU", "P");
            hash.Add("CCC", "P");
            hash.Add("CCA", "P");
            hash.Add("CCG", "P");
            hash.Add("CAU", "H");
            hash.Add("CAC", "H");
            hash.Add("CAA", "Q");
            hash.Add("CAG", "Q");
            hash.Add("CGU", "R");
            hash.Add("CGC", "R");
            hash.Add("CGA", "R");
            hash.Add("CGG", "R");
            hash.Add("AUU", "I");
            hash.Add("AUC", "I");
            hash.Add("AUA", "I");
            hash.Add("AUG", "M");
            hash.Add("ACU", "T");
            hash.Add("ACC", "T");
            hash.Add("ACA", "T");
            hash.Add("ACG", "T");
            hash.Add("AAU", "N");
            hash.Add("AAC", "N");
            hash.Add("AAA", "K");
            hash.Add("AAG", "K");
            hash.Add("AGU", "S");
            hash.Add("AGC", "S");
            hash.Add("AGA", "R");
            hash.Add("AGG", "R");
            hash.Add("GUU", "V");
            hash.Add("GUC", "V");
            hash.Add("GUA", "V");
            hash.Add("GUG", "V");
            hash.Add("GCU", "A");
            hash.Add("GCC", "A");
            hash.Add("GCA", "A");
            hash.Add("GCG", "A");
            hash.Add("GAU", "D");
            hash.Add("GAC", "D");
            hash.Add("GAA", "E");
            hash.Add("GAG", "E");
            hash.Add("GGU", "G");
            hash.Add("GGC", "G");
            hash.Add("GGA", "G");
            hash.Add("GGG", "G");
            return hash;
        }
    }
}
