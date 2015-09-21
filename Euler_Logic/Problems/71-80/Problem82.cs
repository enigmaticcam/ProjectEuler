using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem82 : IProblem {
        private decimal[][] _grid;
        private decimal[][] _best;

        public string ProblemName {
            get { return "82: Path sum: three ways"; }
        }

        public string GetAnswer() {
            LoadTestGrid();
            Initialize();
            return FindMaxPathSum().ToString();
        }

        private void Initialize() {
            _best = new decimal[_grid.GetUpperBound(0) + 1][];
            for (int x = 0; x <= _grid.GetUpperBound(0); x++) {
                _best[x] = new decimal[_grid[x].GetUpperBound(0) + 1];
            }
        }

        private decimal FindMaxPathSum() {
            decimal best = decimal.MaxValue;
            for (int row = 0; row <= _grid.GetUpperBound(0); row++) {
                decimal sum = MaxPathSum(row, _grid[row].GetUpperBound(0));
                if (sum < best) {
                    best = sum;
                }
            }
            return best;
        }

        private decimal MaxPathSum(int x, int y) {
            if (_best[x][y] != 0) {
                return _best[x][y];
            } else {
                decimal fromUp = decimal.MaxValue;
                decimal fromLeft = decimal.MaxValue;
                decimal fromDown = decimal.MaxValue;
                if (x > 0 && y != _grid[x].GetUpperBound(0) && y != 0) {
                    fromUp = MaxPathSum(x - 1, y);
                }
                if (y > 0) {
                    fromLeft = MaxPathSum(x, y - 1);
                }
                if (x < _grid.GetUpperBound(0) && y != _grid[x].GetUpperBound(0) && y != 0) {
                    fromDown = MaxPathSum(x + 1, y);
                }
                if (fromUp < fromLeft && fromUp < fromDown && fromUp != decimal.MaxValue) {
                    _best[x][y] = fromUp + _grid[x][y];
                } else if (fromLeft < fromDown && fromLeft != decimal.MaxValue) {
                    _best[x][y] = fromLeft + _grid[x][y];
                } else if (fromDown != decimal.MaxValue) {
                    _best[x][y] = fromDown + _grid[x][y];
                } else {
                    _best[x][y] = _grid[x][y];
                }
                return _best[x][y];
            }
        }

        private void LoadTestGrid() {
            _grid = new decimal[5][];
            _grid[0] = "131,673,234,103,18".Split(',').Select(decimal.Parse).ToList().ToArray();
            _grid[1] = "201,96,342,965,150".Split(',').Select(decimal.Parse).ToList().ToArray();
            _grid[2] = "630,803,746,422,111".Split(',').Select(decimal.Parse).ToList().ToArray();
            _grid[3] = "537,699,497,121,956".Split(',').Select(decimal.Parse).ToList().ToArray();
            _grid[4] = "805,732,524,37,331".Split(',').Select(decimal.Parse).ToList().ToArray();
        }


    }
}
