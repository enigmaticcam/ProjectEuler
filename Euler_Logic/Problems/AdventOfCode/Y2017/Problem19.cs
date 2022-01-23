using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem19 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, enumGridType>> _grid;
        private Dictionary<int, Dictionary<int, char>> _letters;
        private char[] _pickupLetters;
        private int _steps;

        private enum enumGridType {
            Path,
            Intersection,
            Letter
        }

        public override string ProblemName {
            get { return "Advent of Code 2017: 19"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private string Answer1(List<string> input) {
            GetGrid(input);
            PickupLetters();
            return new string(_pickupLetters);
        }

        private int Answer2(List<string> input) {
            GetGrid(input);
            PickupLetters();
            return _steps;
        }

        private void PickupLetters() {
            int x = GetStartX();
            int y = 0;
            var directionX = new List<int>() { 0, 1, 0, -1 };
            var directionY = new List<int>() { -1, 0, 1, 0 };
            int direction = 2;
            _pickupLetters = new char[CountLetters()];
            int letterIndex = 0;
            do {
                _steps++;
                switch (_grid[x][y]) {
                    case enumGridType.Intersection:
                        int lastDirection = direction;
                        while (!_grid.ContainsKey(x + directionX[direction]) || !_grid[x + directionX[direction]].ContainsKey(y + directionY[direction])) {
                            do {
                                direction = (direction + 1) % 4;
                            } while ((lastDirection + 2) % 4 == direction);
                        }
                        break;
                    case enumGridType.Letter:
                        _pickupLetters[letterIndex] = _letters[x][y];
                        letterIndex++;
                        break;
                }
                x += directionX[direction];
                y += directionY[direction];
            } while (_grid.ContainsKey(x) && _grid[x].ContainsKey(y));
        }

        private int GetStartX() {
            foreach (var x in _grid) {
                if (x.Value.ContainsKey(0)) {
                    return x.Key;
                }
            }
            return -1;
        }

        private int CountLetters() {
            int count = 0;
            foreach (var x in _letters) {
                count += x.Value.Count;
            }
            return count;
        }

        private void GetGrid(List<string> input) {
            _letters = new Dictionary<int, Dictionary<int, char>>();
            _grid = new Dictionary<int, Dictionary<int, enumGridType>>();
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    switch (input[y][x]) {
                        case ' ':
                            break;
                        case '-':
                        case '|':
                            if (!_grid.ContainsKey(x)) {
                                _grid.Add(x, new Dictionary<int, enumGridType>());
                            }
                            _grid[x].Add(y, enumGridType.Path);
                            break;
                        case '+':
                            if (!_grid.ContainsKey(x)) {
                                _grid.Add(x, new Dictionary<int, enumGridType>());
                            }
                            _grid[x].Add(y, enumGridType.Intersection);
                            break;
                        default:
                            if (!_grid.ContainsKey(x)) {
                                _grid.Add(x, new Dictionary<int, enumGridType>());
                            }
                            _grid[x].Add(y, enumGridType.Letter);
                            if (!_letters.ContainsKey(x)) {
                                _letters.Add(x, new Dictionary<int, char>());
                            }
                            _letters[x].Add(y, input[y][x]);
                            break;
                    }
                }
            }
        }
    }
}
