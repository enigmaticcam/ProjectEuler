using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Euler_Logic.Problems.Rosalind {
    public abstract class RosalindBase : ProblemBase {
        public List<string> Input() {
            return LoadFile(@"C:\Temp\Rosalind\input.txt");
        }

        private List<string> LoadFile(string file) {
            var input = new List<string>();
            using (var reader = new StreamReader(file)) {
                while (!reader.EndOfStream) {
                    input.Add(reader.ReadLine());
                }
                reader.Close();
            }
            return input;
        }

        public void SaveOutput(List<string> output) {
            using (var writer = new StreamWriter(@"C:\Temp\Rosalind\output.txt")) {
                foreach (var line in output) {
                    writer.WriteLine(line);
                }
            }
        }

        public void SaveOutput(string output) {
            SaveOutput(new List<string>() { output });
        }

        public void SaveOutput(string line1, string line2) {
            SaveOutput(new List<string>() { line1, line2 });
        }
    }
}
