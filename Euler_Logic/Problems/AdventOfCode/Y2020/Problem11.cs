using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem11 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2020: 11";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var grid = GetGrid(input);
            ulong count = 0;
            while (Perform(grid) > 0) {
                count++;
            }
            return CountOccupied(grid);
        }

        private int Perform(Grid grid) {
            int count = 0;
            var nextGrid = new bool[grid.FullLayout.GetUpperBound(0) + 1, grid.FullLayout.GetUpperBound(1) + 1];
            for (int col = 1; col < grid.FullLayout.GetUpperBound(0); col++) {
                for (int row = 1; row < grid.FullLayout.GetUpperBound(1); row++) {
                    nextGrid[col, row] = grid.OccupiedLayout[col, row];
                    if (grid.FullLayout[col, row] == 'L'
                        && !grid.OccupiedLayout[col, row]
                        && !grid.OccupiedLayout[col + 1, row]
                        && !grid.OccupiedLayout[col + 1, row + 1]
                        && !grid.OccupiedLayout[col + 1, row - 1]
                        && !grid.OccupiedLayout[col, row + 1]
                        && !grid.OccupiedLayout[col, row - 1]
                        && !grid.OccupiedLayout[col - 1, row]
                        && !grid.OccupiedLayout[col - 1, row + 1]
                        && !grid.OccupiedLayout[col - 1, row - 1]) {
                        nextGrid[col, row] = true;
                        count++;
                    } 
                    if (grid.FullLayout[col, row] == 'L' && grid.OccupiedLayout[col, row]) {
                        int sum = 0;
                        sum += (grid.OccupiedLayout[col + 1, row] ? 1 : 0);
                        sum += (grid.OccupiedLayout[col + 1, row - 1] ? 1 : 0);
                        sum += (grid.OccupiedLayout[col + 1, row + 1] ? 1 : 0);
                        sum += (grid.OccupiedLayout[col, row - 1] ? 1 : 0);
                        sum += (grid.OccupiedLayout[col, row + 1] ? 1 : 0);
                        sum += (grid.OccupiedLayout[col - 1, row] ? 1 : 0);
                        sum += (grid.OccupiedLayout[col - 1, row - 1] ? 1 : 0);
                        sum += (grid.OccupiedLayout[col - 1, row + 1] ? 1 : 0);
                        if (sum >= 4) {
                            nextGrid[col, row] = false;
                            count++;
                        }
                    }
                }
            }
            grid.OccupiedLayout = nextGrid;
            return count;
        }

        private int CountOccupied(Grid grid) {
            int count = 0;
            for (int col = 1; col < grid.FullLayout.GetUpperBound(0); col++) {
                for (int row = 1; row < grid.FullLayout.GetUpperBound(1); row++) {
                    if (grid.OccupiedLayout[col, row]) {
                        count++;
                    }
                }
            }
            return count;
        }

        private Grid GetGrid(List<string> input) {
            var grid = new Grid();
            grid.FullLayout = new char[input[0].Length + 2, input.Count + 2];
            grid.OccupiedLayout = new bool[input[0].Length + 2, input.Count + 2];
            for (int col = 0; col < input[0].Length; col++) {
                grid.FullLayout[col, 0] = 'L';
                grid.FullLayout[col, grid.FullLayout.GetUpperBound(1)] = 'L';
                grid.OccupiedLayout[col, 0] = false;
                grid.OccupiedLayout[col, grid.FullLayout.GetUpperBound(1)] = false;
                for (int row = 0; row < input.Count; row++) {
                    grid.FullLayout[col + 1, row + 1] = input[row][col];
                    if (input[row][col] == 'L') {
                        grid.OccupiedLayout[col + 1, row + 1] = false;
                    } else if (input[row][col] == '.') {
                        grid.OccupiedLayout[col + 1, row + 1] = false;
                    } else {
                        grid.OccupiedLayout[col + 1, row + 1] = true;
                    }
                    grid.FullLayout[0, row] = 'L';
                    grid.FullLayout[grid.FullLayout.GetUpperBound(0), row] = 'L';
                    grid.OccupiedLayout[0, row] = false;
                    grid.OccupiedLayout[grid.FullLayout.GetUpperBound(0), row] = false;
                }
            }
            return grid;
        }

        private List<string> TestInput() {
            return new List<string>() {
                "L.LL.LL.LL",
                "LLLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLLL",
                "L.LLLLLL.L",
                "L.LLLLL.LL"
            };
        }

        private class Grid {
            public char[,] FullLayout { get; set; }
            public bool[,] OccupiedLayout { get; set; }
        }
    }
}
