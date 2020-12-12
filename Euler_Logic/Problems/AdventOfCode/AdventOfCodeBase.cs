using System.Collections.Generic;
using System.IO;

namespace Euler_Logic.Problems.AdventOfCode {
    public abstract class AdventOfCodeBase : ProblemBase {
        public List<string> Input() {
            var input = new List<string>();
            using (var reader = new StreamReader("c:\\temp\\input.txt")) {
                while (!reader.EndOfStream) {
                    input.Add(reader.ReadLine());
                }
                reader.Close();
            }
            return input;
        }
    }
}
