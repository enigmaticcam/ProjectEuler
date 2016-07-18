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
            BuildFullGrid();
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
                    if (_bestPartial.ContainsKey(path)) {
                        currentScore = sum + _bestPartial[path];
                    } else {
                        if (col < _grid[row].Length - 1) {
                            currentScore = NextColumn(path, col + 1, sum);
                        } else {
                            currentScore = sum;
                        }
                        if (!_bestFull.ContainsKey(path)) {
                            _bestFull.Add(path, currentScore);
                        }
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
            _bestPartial.Add(path, bestScore - sum);
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

        private void BuildFullGrid() {
            _grid = new ulong[15][];
            _grid[0] = new List<ulong> { 7, 53, 183, 439, 863, 497, 383, 563, 79, 973, 287, 63, 343, 169, 583 }.ToArray();
            _grid[1] = new List<ulong> { 627, 343, 773, 959, 943, 767, 473, 103, 699, 303, 957, 703, 583, 639, 913 }.ToArray();
            _grid[2] = new List<ulong> { 447, 283, 463, 29, 23, 487, 463, 993, 119, 883, 327, 493, 423, 159, 743 }.ToArray();
            _grid[3] = new List<ulong> { 217, 623, 3, 399, 853, 407, 103, 983, 89, 463, 290, 516, 212, 462, 350 }.ToArray();
            _grid[4] = new List<ulong> { 960, 376, 682, 962, 300, 780, 486, 502, 912, 800, 250, 346, 172, 812, 350 }.ToArray();
            _grid[5] = new List<ulong> { 870, 456, 192, 162, 593, 473, 915, 45, 989, 873, 823, 965, 425, 329, 803 }.ToArray();
            _grid[6] = new List<ulong> { 973, 965, 905, 919, 133, 673, 665, 235, 509, 613, 673, 815, 165, 992, 326 }.ToArray();
            _grid[7] = new List<ulong> { 322, 148, 972, 962, 286, 255, 941, 541, 265, 323, 925, 281, 601, 95, 973 }.ToArray();
            _grid[8] = new List<ulong> { 445, 721, 11, 525, 473, 65, 511, 164, 138, 672, 18, 428, 154, 448, 848 }.ToArray();
            _grid[9] = new List<ulong> { 414, 456, 310, 312, 798, 104, 566, 520, 302, 248, 694, 976, 430, 392, 198 }.ToArray();
            _grid[10] = new List<ulong> { 184, 829, 373, 181, 631, 101, 969, 613, 840, 740, 778, 458, 284, 760, 390 }.ToArray();
            _grid[11] = new List<ulong> { 821, 461, 843, 513, 17, 901, 711, 993, 293, 157, 274, 94, 192, 156, 574 }.ToArray();
            _grid[12] = new List<ulong> { 34, 124, 4, 878, 450, 476, 712, 914, 838, 669, 875, 299, 823, 329, 699 }.ToArray();
            _grid[13] = new List<ulong> { 815, 559, 813, 459, 522, 788, 168, 586, 966, 232, 308, 833, 251, 631, 107 }.ToArray();
            _grid[14] = new List<ulong> { 813, 883, 451, 509, 615, 77, 281, 613, 459, 205, 380, 274, 302, 35, 805 }.ToArray();
        }
    }
}
