using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem09 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 9"; }
        }

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        public long Answer1() {
            return RunIntComputerNew(1);
        }

        public long Answer2() {
            return RunIntComputerNew(2);
        }

        private long RunIntComputerNew(int input) {
            var computer = new IntComputer();
            computer.Run(
                instructions: Input(),
                input: () => input,
                outputCaller: () => { });
            return computer.Output.Last();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "109, 1, 203, 2, 204, 2, 99"
            };
        }
    }
}
