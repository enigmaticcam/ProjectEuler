using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem19 : AdventOfCodeBase {
        private IntComputer _computer;

        public override string ProblemName {
            get { return "Advent of Code 2019: 19"; }
        }

        public override string GetAnswer() {
            _computer = new IntComputer();
            _computer.SingleOutputOnly = true;
            return Answer1().ToString();
        }

        private long Answer1() {
            var grid = new long[50, 50];
            int x = 0;
            int y = 0;
            for (int count = 1; count <= 50 * 50; count++) {
                grid[x, y] = GetValueAtCoordinate(x, y);
                x++;
                if (x == 50) {
                    x = 0;
                    y++;
                }
            }
            return CountOnes(grid);
        }

        private int Answer2(int size) {
            int x = 0;
            int y = 0;
            size--;
            do {
                x = FindNext1InRow(x, y);
                if (x - size >= 0) {
                    var result = GetValueAtCoordinate(x - size, y + size);
                    if (result == 1) {
                        return (x - size) * 10000 + y;
                    }
                }
                y++;
            } while (true);
        }

        private int FindNext1InRow(int x, int y) {
            bool found = false;
            do {
                var result = GetValueAtCoordinate(x, y);
                if (!found) {
                    if (result == 1) {
                        found = true;
                    }
                } else {
                    if (result == 0) {
                        return x - 1;
                    }
                }
                x++;
            } while (true);
        }

        private long GetValueAtCoordinate(int x, int y) {
            bool inputY = false;
            _computer.Run(
                instructions: Input(),
                input: () => {
                    if (inputY) {
                        return y;
                    } else {
                        inputY = true;
                        return x;
                    }
                },
                outputCaller: () => { });
            return _computer.LastOutput;
        }

        private long CountOnes(long[,] grid) {
            long count = 0;
            for (int x = 0; x < 50; x++) {
                for (int y = 0; y < 50; y++) {
                    count += grid[x, y];
                }
            }
            return count;
        }

        private string PrintOutput(long[,] grid) {
            var text = new StringBuilder();
            for (int y = 0; y <= 49; y++) {
                for (int x = 0; x <= 49; x++) {
                    text.Append(grid[x, y]);
                }
                text.AppendLine();
            }
            return text.ToString();
        }
    }
}
