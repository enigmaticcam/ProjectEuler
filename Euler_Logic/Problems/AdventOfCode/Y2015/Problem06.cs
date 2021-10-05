using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem06 : AdventOfCodeBase {
        private enum enumMethod {
            Toggle,
            Off,
            On
        }

        public override string ProblemName => "Advent of Code 2015: 6";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var commands = GetCommands(input);
            var grid = Perform1(commands);
            return CountOn(grid);
        }

        private long Answer2(List<string> input) {
            var commands = GetCommands(input);
            var grid = Perform2(commands);
            return CountBrightness(grid);
        }

        private bool[,] Perform1(List<Command> commands) {
            var grid = new bool[1000, 1000];
            foreach (var command in commands) {
                Func<bool, bool> setLight = null;
                switch (command.Method) {
                    case enumMethod.Off:
                        setLight = light => false;
                        break;
                    case enumMethod.On:
                        setLight = light => true;
                        break;
                    case enumMethod.Toggle:
                        setLight = light => !light;
                        break;
                }
                for (int x = command.StartX; x <= command.EndX; x++) {
                    for (int y = command.StartY; y <= command.EndY; y++) {
                        grid[x, y] = setLight(grid[x, y]);
                    }
                }
            }
            return grid;
        }

        private long[,] Perform2(List<Command> commands) {
            var grid = new long[1000, 1000];
            foreach (var command in commands) {
                Func<long, long> setLight = null;
                switch (command.Method) {
                    case enumMethod.Off:
                        setLight = light => Math.Max(light - 1, 0);
                        break;
                    case enumMethod.On:
                        setLight = light => light + 1;
                        break;
                    case enumMethod.Toggle:
                        setLight = light => light + 2;
                        break;
                }
                for (int x = command.StartX; x <= command.EndX; x++) {
                    for (int y = command.StartY; y <= command.EndY; y++) {
                        grid[x, y] = setLight(grid[x, y]);
                    }
                }
            }
            return grid;
        }

        private int CountOn(bool[,] grid) {
            int count = 0;
            for (int x = 0; x <= grid.GetUpperBound(0); x++) {
                for (int y = 0; y <= grid.GetUpperBound(1); y++) {
                    count += grid[x, y] ? 1 : 0;
                }
            }
            return count;
        }

        private long CountBrightness(long[,] grid) {
            long sum = 0;
            for (int x = 0; x <= grid.GetUpperBound(0); x++) {
                for (int y = 0; y <= grid.GetUpperBound(1); y++) {
                    sum += grid[x, y];
                }
            }
            return sum;
        }

        private List<Command> GetCommands(List<string> input) {
            return input.Select(line => {
                var command = new Command();
                var split = line.Split(' ');
                int startIndex = 0;
                int endIndex = 0;
                if (split[0] == "toggle") {
                    command.Method = enumMethod.Toggle;
                    startIndex = 1;
                    endIndex = 3;
                } else {
                    startIndex = 2;
                    endIndex = 4;
                    if (split[1] == "off") {
                        command.Method = enumMethod.Off;
                    } else {
                        command.Method = enumMethod.On;
                    }
                }
                GetXY(split[startIndex]);
                command.StartX = _x;
                command.StartY = _y;
                GetXY(split[endIndex]);
                command.EndX = _x;
                command.EndY = _y;
                return command;
            }).ToList();
        }

        private int _x;
        private int _y;
        private void GetXY(string text) {
            var split = text.Split(',');
            _x = Convert.ToInt32(split[0]);
            _y = Convert.ToInt32(split[1]);
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "turn on 0,0 through 999,999",
                "toggle 0,0 through 999,0",
                "turn off 499,499 through 500,500"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "turn on 0,0 through 0,0",
                "toggle 0,0 through 999,999"
            };
        }

        private class Command {
            public enumMethod Method { get; set; }
            public int StartX { get; set; }
            public int StartY { get; set; }
            public int EndX { get; set; }
            public int EndY { get; set; }
        }
    }
}
