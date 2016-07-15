using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem345 : IProblem {
        private ulong[][] _grid;
        private Dictionary<ulong, ulong> _bestFull = new Dictionary<ulong, ulong>();
        private Dictionary<ulong, ulong> _bestPartial = new Dictionary<ulong, ulong>();

        public string ProblemName {
            get { return "345: Matrix Sum"; }
        }

        public string GetAnswer() {
            return Solve().ToString();
        }

        private ulong Solve() {
            BuildTestGrid();
            return NextColumn(0, 0, 0);
        }

        private ulong NextColumn(ulong path, int col, ulong sum) {
            ulong bestScore = 0;
            for (int row = 0; row < _grid.Length; row++) {
                ulong rowBit = (ulong)Math.Pow(2, row);
                if ((path & rowBit) == 0) {
                    ulong currentScore = 0;
                    path += rowBit;
                    sum += _grid[row][col];
                    if (_bestFull.ContainsKey(path)) {
                        currentScore = sum + _bestPartial[path];
                    } else {
                        if (col < _grid[row].Length - 1) {
                            currentScore = NextColumn(path, col + 1, sum);
                        } else {
                            currentScore = sum;
                        }
                        _bestFull.Add(path, currentScore);
                    }
                    if (currentScore > _bestFull[path]) {
                        _bestFull[path] = currentScore;
                    }
                    if (currentScore > bestScore) {
                        bestScore = currentScore;
                    }
                    path -= rowBit;
                    sum -= _grid[row][col];
                }
            }
            _bestPartial.Add(path, bestScore);
            return bestScore;
        }

        private void BuildTestGrid() {
            _grid = new ulong[5][];
            _grid[0] = new List<ulong> { 7, 53, 183, 439, 863 }.ToArray();
            _grid[1] = new List<ulong> { 497, 383, 563, 79, 973 }.ToArray();
            _grid[2] = new List<ulong> { 287, 63, 343, 169, 583 }.ToArray();
            _grid[3] = new List<ulong> { 627, 343, 773, 959, 943 }.ToArray();
            _grid[4] = new List<ulong> { 767, 473, 103, 699, 303 }.ToArray();
        }
    }
}
