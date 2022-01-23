using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem22 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, enumNodeStatus>> _grid;

        private enum enumNodeStatus {
            Clean,
            Weakened,
            Infected,
            Flagged
        }

        public override string ProblemName {
            get { return "Advent of Code 2017: 22"; }
        }

        public override string GetAnswer() {
            return Answer1(Input(), 10000).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input(), 10000000).ToString();
        }

        private int Answer1(List<string> input, int maxBurst) {
            GetGrid(input);
            return CountInfected(maxBurst, true);
        }

        private int Answer2(List<string> input, int maxBurst) {
            GetGrid(input);
            return CountInfected(maxBurst, false);
        }

        private int CountInfected(int maxBurst, bool isSimple) {
            int count = 0;
            int x = _grid.Count / 2;
            int y = x;
            var directions = new Tuple<int, int>[4] {
                new Tuple<int, int>(0, -1),
                new Tuple<int, int>(1, 0),
                new Tuple<int, int>(0, 1),
                new Tuple<int, int>(-1, 0)
            };
            int direction = 0;
            for (int burst = 1; burst <= maxBurst; burst++) {
                if (isSimple) {
                    direction = GetDirectionSimple(_grid[x][y], direction);
                } else {
                    direction = GetDirectionComplex(_grid[x][y], direction);
                }
                if (isSimple) {
                    _grid[x][y] = (enumNodeStatus)(((int)_grid[x][y] + 2) % 4);
                } else {
                    _grid[x][y] = (enumNodeStatus)(((int)_grid[x][y] + 1) % 4);
                }
                count += (_grid[x][y] == enumNodeStatus.Infected ? 1 : 0);
                x += directions[direction].Item1;
                y += directions[direction].Item2;
                if (!_grid.ContainsKey(x)) {
                    _grid.Add(x, new Dictionary<int, enumNodeStatus>());
                }
                if (!_grid[x].ContainsKey(y)) {
                    _grid[x].Add(y, enumNodeStatus.Clean);
                }
            }
            return count;
        }

        private int GetDirectionComplex(enumNodeStatus node, int direction) {
            switch (node) {
                case enumNodeStatus.Clean:
                    direction--;
                    if (direction < 0) {
                        direction += 4;
                    }
                    return direction;
                case enumNodeStatus.Weakened:
                    return direction;
                case enumNodeStatus.Infected:
                    return (direction + 1) % 4;
                case enumNodeStatus.Flagged:
                    return (direction + 2) % 4;
            }
            return 0;
        }

        private int GetDirectionSimple(enumNodeStatus node, int direction) {
            if (node == enumNodeStatus.Infected) {
                return (direction + 1) % 4;
            } else {
                direction--;
                if (direction < 0) {
                    direction += 4;
                }
                return direction;
            }
        }

        private void GetGrid(List<string> input) {
            _grid = new Dictionary<int, Dictionary<int, enumNodeStatus>>();
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    if (!_grid.ContainsKey(x)) {
                        _grid.Add(x, new Dictionary<int, enumNodeStatus>());
                    }
                    _grid[x].Add(y, input[y][x] == '#' ? enumNodeStatus.Infected : enumNodeStatus.Clean);
                }
            }
        }
    }
}
