using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem21 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 21"; }
        }

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        private int Answer1() {
            var comp = new IntComputer();
            comp.Run(
                instructions: Input(),
                input: () => GetInput(Instructions1),
                outputCaller: () => { });
            return (int)comp.LastOutput;
        }

        private int Answer2() {
            var comp = new IntComputer();
            comp.Run(
                instructions: Input(),
                input: () => GetInput(Instructions2),
                outputCaller: () => { });
            return (int)comp.LastOutput;
        }

        private string Output(IntComputer comp) {
            var output = new StringBuilder();
            foreach (var ascii in comp.Output) {
                output.Append((char)ascii);
            }
            return output.ToString();
        }

        private int _inputIndex;
        private int GetInput(string instructions) {
            var result = (int)instructions[_inputIndex];
            while (result == 13) {
                _inputIndex++;
                result = (int)instructions[_inputIndex];
            }
            _inputIndex++;
            return result;
        }

        private string Instructions1 => @"NOT A T
OR T J
NOT B T
OR T J
NOT C T
OR T J
AND D J
WALK
";

        private string Instructions2 => @"NOT A T
OR T J
NOT B T
OR T J
NOT C T
OR T J
AND D J
NOT J T
OR E T
OR H T
AND T J
RUN
";
    }
}
