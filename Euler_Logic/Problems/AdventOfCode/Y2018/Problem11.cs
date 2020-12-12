using System;
using System.Collections.Generic;
namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem11 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 11"; }
        }

        public override string GetAnswer() {
            return Answer2();
        }

        private string Answer1() {
            var grid = GetGrid(9110);
            var highest = FindHighest(grid);
            return highest.Item1 + "," + highest.Item2;
        }

        private string Answer2() {
            var grid = GetGrid(9110);
            var highest = FindHighestVaried(grid);
            return highest.Item1 + "," + highest.Item2 + "," + highest.Item3;
        }

        private Tuple<int, int> FindHighest(int[,] grid) {
            int highest = 0;
            int bestX = 0;
            int bestY = 0;
            for (int x = 0; x <= 297; x++) {
                for (int y = 0; y <= 297; y++) {
                    int sum = 0;
                    for (int xOffset = 0; xOffset <= 2; xOffset++) {
                        for (int yOffset = 0; yOffset <= 2; yOffset++) {
                            sum += grid[x + xOffset, y + yOffset];
                        }
                    }
                    if (sum > highest) {
                        highest = sum;
                        bestX = x + 1;
                        bestY = y + 1;
                    }
                }
            }
            return new Tuple<int, int>(bestX, bestY);
        }

        private Dictionary<Tuple<int, int, int>, int> _squares;
        private Tuple<int, int, int> FindHighestVaried(int[,] grid) {
            _squares = new Dictionary<Tuple<int, int, int>, int>();
            int highest = 0;
            int bestX = 0;
            int bestY = 0;
            int bestLength = 0;
            var valueGrid = new int[300, 300];
            for (int length = 0; length <= 299; length++) {
                for (int x = 1; x <= 300 - length; x++) {
                    for (int y = 1; y <= 300 - length; y++) {
                        var value = GetSquare(x, y, length, grid);
                        if (value > highest) {
                            highest = value;
                            bestX = x;
                            bestY = y;
                            bestLength = length + 1;
                        }
                    }
                }
            }
            return new Tuple<int, int, int>(bestX, bestY, bestLength);
        }

        private int GetSquare(int x, int y, int length, int[,] grid) {
            var key = new Tuple<int, int, int>(x, y, length);
            if (!_squares.ContainsKey(key)) {
                int value = 0;
                if (length == 0) {
                    value = grid[x - 1, y - 1];
                } else if (length == 1) {
                    value = grid[x - 1, y - 1] + grid[x, y - 1] + grid[x - 1, y] + grid[x, y];
                } else {
                    value = _squares[new Tuple<int, int, int>(x, y, length - 1)]
                        + _squares[new Tuple<int, int, int>(x + 1, y + 1, length - 1)]
                        - _squares[new Tuple<int, int, int>(x + 1, y + 1, length - 2)]
                        + grid[x - 1 + length, y - 1]
                        + grid[x - 1, y - 1 + length];
                }
                _squares.Add(key, value);
            }
            return _squares[key];
        }

        private int[,] GetGrid(int serialNumber) {
            var grid = new int[300, 300];
            for (int x = 1; x <= 300; x++) {
                for (int y = 1; y <= 300; y++) {
                    grid[x - 1, y - 1] = GetPower(x, y, serialNumber);
                }
            }
            return grid;
        }

        private int GetPower(int x, int y, int serialNumber) {
            var power = x + 10;
            power *= y;
            power += serialNumber;
            power *= x + 10;
            power = (power / 100) % 10;
            power -= 5;
            return power;
        }
    }
}
