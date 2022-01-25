using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem17 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, enumNodeType>> _grid;
        private int _highestY;
        private int _lowestY;
        private int _highestX;
        private int _lowestX;

        private enum enumNodeType {
            Empty,
            Wall,
            WaterStagnant,
            WaterFlowing
        }

        public override string ProblemName {
            get { return "Advent of Code 2018: 17"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetGrid(input);
            SetHighestLowest();
            Vertical(500, 0);
            return GetCount(false);
        }

        private int Answer2(List<string> input) {
            GetGrid(input);
            SetHighestLowest();
            Vertical(500, 0);
            return GetCount(true);
        }

        private int GetCount(bool onlyStagnant) {
            int count = 0;
            foreach (var x in _grid) {
                foreach (var y in x.Value) {
                    if (y.Key >= _lowestY && y.Key <= _highestY) {
                        if (y.Value == enumNodeType.WaterStagnant) {
                            count++;
                        } else if (y.Value == enumNodeType.WaterFlowing && !onlyStagnant) {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        private bool Vertical(int startX, int startY) {
            int x = startX;
            int y = startY;
            while (y <= _highestY && GetNode(x, y) == enumNodeType.Empty) {
                SetNode(x, y, enumNodeType.WaterFlowing);
                y++;
            }
            if (y <= _highestY) {
                y--;
                bool left;
                bool right;
                bool foundFlowing = LookForFlowing(x - 1, y, -1);
                foundFlowing = foundFlowing || LookForFlowing(x + 1, y, 1);
                if (!foundFlowing) {
                    do {
                        SetNode(x, y, enumNodeType.WaterStagnant);
                        left = Horizontal(x - 1, y, -1);
                        right = Horizontal(x + 1, y, 1);
                        if (!left || !right) {
                            SetNode(x, y, enumNodeType.WaterFlowing);
                            SetRowToFlowing(x, y);
                        }
                        y--;
                        foundFlowing = LookForFlowing(x - 1, y, -1);
                        foundFlowing = foundFlowing || LookForFlowing(x + 1, y, 1);
                    } while (left && right && y > startY && !foundFlowing);
                    if (GetNode(x, y - 1) == enumNodeType.WaterStagnant) {
                        SetNode(x, y, enumNodeType.WaterStagnant);
                    }
                } 
            }
            return y <= startY;
        }

        private void SetRowToFlowing(int x, int y) {
            int step = 1;
            bool leftStop = false;
            bool rightStop = false;
            do {
                if (!leftStop && GetNode(x - step, y) == enumNodeType.WaterStagnant) {
                    SetNode(x - step, y, enumNodeType.WaterFlowing);
                } else if (!leftStop) {
                    leftStop = true;
                }
                if (!rightStop && GetNode(x + step, y) == enumNodeType.WaterStagnant) {
                    SetNode(x + step, y, enumNodeType.WaterFlowing);
                } else if (!rightStop) {
                    rightStop = true;
                }
                step++;
            } while (!leftStop || !rightStop);
        }

        private bool LookForFlowing(int startX, int startY, int direction) {
            int x = startX;
            int y = startY;
            var node = GetNode(x, y + 1);
            if (node == enumNodeType.WaterStagnant || node == enumNodeType.WaterFlowing) {
                do {
                    x += direction;
                } while (GetNode(x, y) == enumNodeType.Empty && GetNode(x, y + 1) != enumNodeType.Empty);
                if (GetNode(x, y) == enumNodeType.WaterFlowing) {
                    return true;
                }
            }
            return false;
        }

        private bool Horizontal(int startX, int startY, int direction) {
            int x = startX;
            int y = startY;
            if (GetNode(x, y + 1) == enumNodeType.WaterStagnant) {
                do {
                    x += direction;
                } while (GetNode(x, y) == enumNodeType.Empty && GetNode(x, y + 1) != enumNodeType.Empty);
                if (GetNode(x, y) != enumNodeType.Wall) {
                    if (GetNode(x, y) == enumNodeType.WaterFlowing) {
                        return false;
                    }
                    if (GetNode(x - direction, y + 1) == enumNodeType.WaterStagnant) {
                        return false;
                    }
                }
                x = startX;
            }
            do {
                var next = GetNode(x, y);
                if (next == enumNodeType.Wall) {
                    return true;
                }
                SetNode(x, y, enumNodeType.WaterStagnant);
                if (GetNode(x, y + 1) == enumNodeType.Empty) {
                    var result = Vertical(x, y + 1);
                    if (!result) {
                        return false;
                    }
                }
                x += direction;
            } while (true);
        }

        private void SetHighestLowest() {
            _highestY = 0;
            _highestX = _grid.Keys.Max();
            _lowestY = int.MaxValue;
            _lowestX = _grid.Keys.Min();
            foreach (var points in _grid) {
                var highest = points.Value.Keys.Max();
                if (highest > _highestY) {
                    _highestY = highest;
                }
                var lowest = points.Value.Keys.Min();
                if (lowest < _lowestY) {
                    _lowestY = lowest;
                }
            }
        }

        private void GetGrid(List<string> input) {
            _grid = new Dictionary<int, Dictionary<int, enumNodeType>>();
            foreach (var line in input) {
                var split = line.Split(',');
                bool isNum1X = split[0][0] == 'x';
                var num1 = Convert.ToInt32(split[0].Replace("x=", "").Replace("y=", ""));
                var subSplit = split[1].Replace("..", ",").Split(',');
                var num2 = Convert.ToInt32(subSplit[0].Replace("x=", "").Replace("y=", ""));
                var num3 = Convert.ToInt32(subSplit[1]);
                for (int axis = num2; axis <= num3; axis++) {
                    if (isNum1X) {
                        SetNode(num1, axis, enumNodeType.Wall);
                    } else {
                        SetNode(axis, num1, enumNodeType.Wall);
                    }
                }
            }
        }

        private enumNodeType GetNode(int x, int y) {
            if (_grid.ContainsKey(x) && _grid[x].ContainsKey(y)) {
                return _grid[x][y];
            } else {
                return enumNodeType.Empty;
            }
        }

        private void SetNode(int x, int y, enumNodeType node) {
            if (!_grid.ContainsKey(x)) {
                _grid.Add(x, new Dictionary<int, enumNodeType>());
            }
            if (!_grid[x].ContainsKey(y)) {
                _grid[x].Add(y, node);
            } else {
                _grid[x][y] = node;
            }
        }
    }
}
