using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem18 : AdventOfCodeBase {
        private enumNodeType[,] _grid;

        private enum enumNodeType {
            Open,
            Trees,
            Lumber
        }

        public override string ProblemName {
            get { return "Advent of Code 2018: 18"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetGrid(input);
            Perform(10);
            return GetValue();
        }

        private int Answer2(List<string> input) {
            GetGrid(input);
            PerformModular(1000000000);
            return GetValue();
        }

        private void PerformModular(int maxMinute) {
            // Once you get to around 520, the whole board repeats every 28 minutes
            Perform(520);
            int next = (maxMinute - 520) % 28;
            Perform(next);
        }

        private int GetValue() {
            int trees = 0;
            int lumbers = 0;
            for (int x = 1; x < _grid.GetUpperBound(0); x++) {
                for (int y = 1; y < _grid.GetUpperBound(1); y++) {
                    trees += (_grid[x, y] == enumNodeType.Trees ? 1 : 0);
                    lumbers += (_grid[x, y] == enumNodeType.Lumber ? 1 : 0);
                }
            }
            return trees * lumbers;
        }

        private void Perform(int maxMinute) {
            for (int minute = 1; minute <= maxMinute; minute++) {
                var tempGrid = new enumNodeType[_grid.GetUpperBound(0) + 1, _grid.GetUpperBound(1) + 1];
                for (int x = 1; x < _grid.GetUpperBound(0); x++) {
                    for (int y = 1; y < _grid.GetUpperBound(1); y++) {
                        int sideTrees = 0;
                        int sideLumber = 0;
                        sideTrees += (_grid[x - 1, y] == enumNodeType.Trees ? 1 : 0);
                        sideTrees += (_grid[x + 1, y] == enumNodeType.Trees ? 1 : 0);
                        sideTrees += (_grid[x, y - 1] == enumNodeType.Trees ? 1 : 0);
                        sideTrees += (_grid[x + 1, y - 1] == enumNodeType.Trees ? 1 : 0);
                        sideTrees += (_grid[x - 1, y - 1] == enumNodeType.Trees ? 1 : 0);
                        sideTrees += (_grid[x, y + 1] == enumNodeType.Trees ? 1 : 0);
                        sideTrees += (_grid[x + 1, y + 1] == enumNodeType.Trees ? 1 : 0);
                        sideTrees += (_grid[x - 1, y + 1] == enumNodeType.Trees ? 1 : 0);
                        sideLumber += (_grid[x - 1, y] == enumNodeType.Lumber ? 1 : 0);
                        sideLumber += (_grid[x + 1, y] == enumNodeType.Lumber ? 1 : 0);
                        sideLumber += (_grid[x, y - 1] == enumNodeType.Lumber ? 1 : 0);
                        sideLumber += (_grid[x + 1, y - 1] == enumNodeType.Lumber ? 1 : 0);
                        sideLumber += (_grid[x - 1, y - 1] == enumNodeType.Lumber ? 1 : 0);
                        sideLumber += (_grid[x, y + 1] == enumNodeType.Lumber ? 1 : 0);
                        sideLumber += (_grid[x + 1, y + 1] == enumNodeType.Lumber ? 1 : 0);
                        sideLumber += (_grid[x - 1, y + 1] == enumNodeType.Lumber ? 1 : 0);
                        var node = _grid[x, y];
                        if (node == enumNodeType.Open) {
                            if (sideTrees >= 3) {
                                tempGrid[x, y] = enumNodeType.Trees;
                            }
                        } else if (node == enumNodeType.Trees) {
                            if (sideLumber >= 3) {
                                tempGrid[x, y] = enumNodeType.Lumber;
                            } else {
                                tempGrid[x, y] = enumNodeType.Trees;
                            }
                        } else {
                            if (sideLumber >= 1 && sideTrees >= 1) {
                                tempGrid[x, y] = enumNodeType.Lumber;
                            }
                        }
                    }
                }
                _grid = tempGrid;
            }
        }

        private void GetGrid(List<string> input) {
            _grid = new enumNodeType[input[0].Length + 2, input.Count + 2];
            for (int y = 1; y <= input.Count; y++) {
                for (int x = 1; x <= input[y - 1].Length; x++) {
                    var digit = input[y - 1][x - 1];
                    if (digit == '.') {
                        _grid[x, y] = enumNodeType.Open;
                    } else if (digit == '|') {
                        _grid[x, y] = enumNodeType.Trees;
                    } else {
                        _grid[x, y] = enumNodeType.Lumber;
                    }
                }
            }
        }

        private List<string> TestInput() {
            return new List<string>() {
                ".#.#...|#.",
                ".....#|##|",
                ".|..|...#.",
                "..|#.....#",
                "#.#|||#|#|",
                "...#.||...",
                ".|....|...",
                "||...#|.#|",
                "|.||||..|.",
                "...#.|..|."
            };
        }
    }
}
