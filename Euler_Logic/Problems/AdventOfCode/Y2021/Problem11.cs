using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem11 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, Point>> _grid;
        private int _maxX;
        private int _maxY;

        public override string ProblemName {
            get { return "Advent of Code 2021: 11"; }
        }

        public override string GetAnswer() {
            GetGrid(Input());
            return Answer2().ToString();
        }

        private int Answer1(int maxCount) {
            for (int count = 1; count <= maxCount; count++) {
                for (int x = 0; x <= _maxX; x++) {
                    for (int y = 0; y <= _maxY; y++) {
                        _grid[x][y].Value++;
                    }
                }
                for (int x = 0; x <= _maxX; x++) {
                    for (int y = 0; y <= _maxY; y++) {
                        AttemptFlash(x, y, count);
                    }
                }
                for (int x = 0; x <= _maxX; x++) {
                    for (int y = 0; y <= _maxY; y++) {
                        if (_grid[x][y].Value > 9) {
                            _grid[x][y].Value = 0;
                            _grid[x][y].FlashCount++;
                        }
                    }
                }
            }
            return GetFlashCount();
        }

        private int Answer2() {
            int cycle = 1;
            int all = (_maxX + 1) * (_maxY + 1);
            do {
                for (int x = 0; x <= _maxX; x++) {
                    for (int y = 0; y <= _maxY; y++) {
                        _grid[x][y].Value++;
                    }
                }
                for (int x = 0; x <= _maxX; x++) {
                    for (int y = 0; y <= _maxY; y++) {
                        AttemptFlash(x, y, cycle);
                    }
                }
                int total = 0;
                for (int x = 0; x <= _maxX; x++) {
                    for (int y = 0; y <= _maxY; y++) {
                        if (_grid[x][y].Value > 9) {
                            _grid[x][y].Value = 0;
                            total++;
                        }
                    }
                }
                if (total == all) {
                    return cycle;
                }
                cycle++;
            } while (true);
        }

        private int GetFlashCount() {
            int count = 0;
            for (int x = 0; x <= _maxX; x++) {
                count += _grid[x].Select(p => p.Value.FlashCount).Sum();
            }
            return count;
        }

        private void AttemptFlash(int x, int y, int currentCycle) {
            var point = _grid[x][y];
            if (point.Value > 9 && point.FlashCycle < currentCycle) {
                point.FlashCycle = currentCycle;
                if (x > 0) {
                    _grid[x - 1][y].Value++;
                    AttemptFlash(x - 1, y, currentCycle);
                    if (y > 0) {
                        _grid[x - 1][y - 1].Value++;
                        AttemptFlash(x - 1, y - 1, currentCycle);
                    }
                    if (y < _maxY) {
                        _grid[x - 1][y + 1].Value++;
                        AttemptFlash(x - 1, y + 1, currentCycle);
                    }
                }
                if (x < _maxX) {
                    _grid[x + 1][y].Value++;
                    AttemptFlash(x + 1, y, currentCycle);
                    if (y > 0) {
                        _grid[x + 1][y - 1].Value++;
                        AttemptFlash(x + 1, y - 1, currentCycle);
                    }
                    if (y < _maxY) {
                        _grid[x + 1][y + 1].Value++;
                        AttemptFlash(x + 1, y + 1, currentCycle);
                    }
                }
                if (y > 0) {
                    _grid[x][y - 1].Value++;
                    AttemptFlash(x, y - 1, currentCycle);
                }
                if (y < _maxY) {
                    _grid[x][y + 1].Value++;
                    AttemptFlash(x, y + 1, currentCycle);
                }
            }
        }

        private void GetGrid(List<string> input) {
            _grid = new Dictionary<int, Dictionary<int, Point>>();
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    var point = new Point() {
                        X = x,
                        Y = y,
                        Value = Convert.ToInt32(new string(new char[1] { input[x][y] }))
                    };
                    if (!_grid.ContainsKey(x)) {
                        _grid.Add(x, new Dictionary<int, Point>());
                    }
                    _grid[x].Add(y, point);
                }
            }
            _maxX = _grid.Keys.Max();
            _maxY = _grid[0].Keys.Max();
        }

        private string Output() {
            var text = new StringBuilder();
            for (int x = 0; x <= _maxX; x++) {
                for (int y = 0; y <= _maxY; y++) {
                    var point = _grid[x][y];
                    if (point.Value <= 9) {
                        text.Append(point.Value);
                    } else {
                        text.Append("*");
                    }
                }
                text.AppendLine();
            }
            return text.ToString();
        }

        private class Point {
            public int X { get; set; }
            public int Y { get; set; }
            public int Value { get; set; }
            public int FlashCycle { get; set; }
            public int FlashCount { get; set; }
        }
    }
}
