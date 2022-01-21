using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem18 : AdventOfCodeBase {
        private bool[,] _grid;

        public override string ProblemName => "Advent of Code 2015: 18";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetGrid(input);
            Simulate(input.Count, 100, false);
            return CountOn(input.Count);
        }

        private int Answer2(List<string> input) {
            GetGrid(input);
            Simulate(input.Count, 100, true);
            return CountOn(input.Count);
        }

        private void TurnCornersOn(int length) {
            _grid[1, 1] = true;
            _grid[1, length] = true;
            _grid[length, 1] = true;
            _grid[length, length] = true;
        }

        private void Simulate(int length, int count, bool cornerAlwaysOn) {
            if (cornerAlwaysOn) {
                TurnCornersOn(length);
            }
            bool[,] nextGrid = null;
            for (int step = 1; step <= count; step++) {
                nextGrid = new bool[length + 2, length + 2];
                for (int x = 1; x <= length; x++) {
                    for (int y = 1; y <= length; y++) {
                        int neighborOn = 0;
                        neighborOn += (_grid[x - 1, y] ? 1 : 0);
                        neighborOn += (_grid[x - 1, y + 1] ? 1 : 0);
                        neighborOn += (_grid[x - 1, y - 1] ? 1 : 0);
                        neighborOn += (_grid[x + 1, y] ? 1 : 0);
                        neighborOn += (_grid[x + 1, y + 1] ? 1 : 0);
                        neighborOn += (_grid[x + 1, y - 1] ? 1 : 0);
                        neighborOn += (_grid[x, y + 1] ? 1 : 0);
                        neighborOn += (_grid[x, y - 1] ? 1 : 0);
                        if (_grid[x, y]) {
                            if (neighborOn == 2 || neighborOn == 3) {
                                nextGrid[x, y] = true;
                            }
                        } else {
                            if (neighborOn == 3) {
                                nextGrid[x, y] = true;
                            }
                        }
                    }
                }
                _grid = nextGrid;
                if (cornerAlwaysOn) {
                    TurnCornersOn(length);
                }
            }
        }

        private int CountOn(int length) {
            int count = 0;
            for (int x = 1; x <= length; x++) {
                for (int y = 1; y <= length; y++) {
                    count += (_grid[x, y] ? 1 : 0);
                }
            }
            return count;
        }

        private void GetGrid(List<string> input) {
            _grid = new bool[input.Count + 2, input.Count + 2];
            for (int x = 1; x <= input.Count; x++) {
                for (int y = 1; y <= input.Count; y++) {
                    _grid[x, y] = input[y - 1][x - 1] == '#';
                }
            }
        }
    }
}
