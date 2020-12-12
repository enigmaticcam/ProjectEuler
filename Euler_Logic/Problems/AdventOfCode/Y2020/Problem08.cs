using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem08 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2020: 8";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private long Answer1(List<string> input) {
            var commands = GetCommands(input);
            return FindInfiniteValue(commands);
        }

        private long Answer2(List<string> input) {
            var commands = GetCommands(input);
            return FindBadCommand(commands.ToList());
        }

        private long FindBadCommand(List<Command> commands) {
            for (int index = 0; index < commands.Count(); index++) {
                var command = commands.ElementAt(index);
                if (command.Name == "jmp") {
                    command.Name = "nop";
                    var result = FindInfiniteValue(commands);
                    if (_didTerminateCorrectly) {
                        return result;
                    }
                    command.Name = "jmp";
                } else if (command.Name == "nop") {
                    command.Name = "jmp";
                    var result = FindInfiniteValue(commands);
                    if (_didTerminateCorrectly) {
                        return result;
                    }
                    command.Name = "nop";
                }
            }
            return 0;
        }

        private bool _didTerminateCorrectly;
        private long FindInfiniteValue(IEnumerable<Command> commands) {
            _didTerminateCorrectly = false;
            long acc = 0;
            int line = 0;
            var visited = new bool[commands.Count()];
            do {
                if (visited[line]) {
                    return acc;
                }
                visited[line] = true;
                var command = commands.ElementAt(line);
                switch (command.Name) {
                    case "acc":
                        acc += command.Value;
                        line++;
                        break;
                    case "jmp":
                        line += (int)command.Value;
                        break;
                    default:
                        line++;
                        break;
                }
                if (line == commands.Count()) {
                    _didTerminateCorrectly = true;
                    return acc;
                }
            } while (true);
        }

        private IEnumerable<Command> GetCommands(List<string> input) {
            return input.Select(line => {
                var ins = new Command();
                ins.Name = line.Substring(0, 3);
                ins.Value = Convert.ToInt64(line.Substring(4, line.Length - 4));
                return ins;
            });
        }

        private List<string> TestInput() {
            return new List<string>() {
                "nop +0",
                "acc +1",
                "jmp +4",
                "acc +3",
                "jmp -3",
                "acc -99",
                "acc +1",
                "jmp -4",
                "acc +6"
            };
        }

        private class Command {
            public string Name { get; set; }
            public long Value { get; set; }
        }
    }
}
