using System.Collections.Generic;
using System.IO;

namespace Euler_Logic.Problems.AdventOfCode {
    public abstract class AdventOfCodeBase : ProblemBase {
        public override bool HasAnswer2 => true;

        public List<string> Input() {
            return LoadFile(@"c:\temp\input.txt");
        }

        public List<string> Input_Test(int testNum) {
            return LoadFile(@"c:\temp\test" + testNum + ".txt");
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
    }
}
