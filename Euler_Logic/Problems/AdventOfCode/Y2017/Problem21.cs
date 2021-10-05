using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem21 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<ulong, ulong>> _translation;
        private Dictionary<int, ulong[]> _rotate;
        private Dictionary<int, ulong[]> _flip;

        public override string ProblemName {
            get { return "Advent of Code 2017: 21"; }
        }

        public override string GetAnswer() {
            //return Answer(TestInput(), 2).ToString();
            return Answer(Input(), 18).ToString();
        }

        private int Answer(List<string> input, int iterations) {
            GetTranslations(input);
            SetRotateAndFlip();
            SetAlternateTranslations();
            return CountPixels(iterations);
        }

        private int CountPixels(int maxCount) {
            int size = 3;
            var grid = GetGrid(482, size);
            for (int count = 1; count <= maxCount; count++) {
                int subSize = (size % 2 == 0 ? 2 : 3);
                var nextGrid = new bool[size + (size / subSize), size + (size / subSize)];
                for (int y = 0; y < size; y += subSize) {
                    for (int x = 0; x < size; x += subSize) {
                        var subNum = GetNum(grid, x, y, subSize);
                        var numToAdd = _translation[subSize][subNum];
                        SetGrid(nextGrid, numToAdd, x / subSize * (subSize + 1), y / subSize * (subSize + 1), subSize + 1);
                    }
                }
                grid = nextGrid;
                size = grid.GetUpperBound(0) + 1;
            }
            return CountOn(grid);
        }

        private int CountOn(bool[,] grid) {
            int count = 0;
            for (int x = 0; x <= grid.GetUpperBound(0); x++) {
                for (int y = 0; y <= grid.GetUpperBound(1); y++) {
                    count += (grid[x, y] ? 1 : 0);
                }
            }
            return count;
        }

        private void SetGrid(bool[,] grid, ulong num, int startX, int startY, int size) {
            ulong power = 1;
            for (int countY = 0; countY < size; countY++) {
                for (int countX = 0; countX < size; countX++) {
                    grid[startX + countX, startY + countY] = (num & power) == power;
                    power *= 2;
                }
            }
        }

        private ulong GetNum(bool[,] grid, int startX, int startY, int size) {
            ulong num = 0;
            ulong power = 1;
            for (int countY = 0; countY < size; countY++) {
                for (int countX = 0; countX < size; countX++) {
                    if (grid[startX + countX, startY + countY]) {
                        num += power;
                    }
                    power *= 2;
                }
            }
            return num;
        }

        private bool[,] GetGrid(ulong num, int size) {
            var grid = new bool[size, size];
            ulong power = 1;
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    grid[x, y] = (num & power) == power;
                    power *= 2;
                }
            }
            return grid;
        }

        private void SetAlternateTranslations() {
            var toAdd = new Dictionary<int, Dictionary<ulong, ulong>>();
            toAdd.Add(2, new Dictionary<ulong, ulong>());
            toAdd.Add(3, new Dictionary<ulong, ulong>());
            foreach (var size in _translation.Keys) {
                foreach (var translation in _translation[size]) {
                    var num = translation.Key;
                    var flip = Transform(num, _flip[size]);
                    if (!toAdd[size].ContainsKey(flip)) {
                        toAdd[size].Add(flip, translation.Value);
                    }
                    for (int count = 1; count <= 3; count++) {
                        num = Transform(num, _rotate[size]);
                        if (!toAdd[size].ContainsKey(num)) {
                            toAdd[size].Add(num, translation.Value);
                        }
                        flip = Transform(num, _flip[size]);
                        if (!toAdd[size].ContainsKey(flip)) {
                            toAdd[size].Add(flip, translation.Value);
                        }
                    }
                }
            }
            foreach (var size in toAdd) {
                foreach (var translation in size.Value) {
                    if (!_translation[size.Key].ContainsKey(translation.Key)) {
                        _translation[size.Key].Add(translation.Key, translation.Value);
                    }
                }
            }
        }

        private ulong Transform(ulong num, ulong[] transform) {
            ulong result = 0;
            ulong power = 1;
            foreach (var tDigit in transform) {
                result += ((num & tDigit) == 0 ? 0 : power);
                power *= 2;
            }
            return result;
        }

        private void SetRotateAndFlip() {
            _rotate = new Dictionary<int, ulong[]>();
            _flip = new Dictionary<int, ulong[]>();
            _rotate.Add(2, new ulong[4] { 4, 1, 8, 2 });
            _rotate.Add(3, new ulong[9] { 64, 8, 1, 128, 16, 2, 256, 32, 4 });
            _flip.Add(2, new ulong[4] { 2, 1, 8, 4 });
            _flip.Add(3, new ulong[9] { 4, 2, 1, 32, 16, 8, 256, 128, 64 });
        }

        private void GetTranslations(List<string> input) {
            _translation = new Dictionary<int, Dictionary<ulong, ulong>>();
            foreach (var line in input) {
                var split = line.Split(' ');
                var start = split[0].Split('/');
                var end = split[2].Split('/');
                int size = start.Length;
                if (!_translation.ContainsKey(size)) {
                    _translation.Add(size, new Dictionary<ulong, ulong>());
                }
                _translation[size].Add(GetBits(start), GetBits(end));
            }
        }

        private ulong GetBits(string[] text) {
            ulong power = 1;
            ulong bits = 0;
            foreach (var line in text) {
                for (int index = 0; index < line.Length; index++) {
                    if (line[index] == '#') {
                        bits += power;
                    }
                    power *= 2;
                }
            }
            return bits;
        }

        private List<string> TestInput() {
            return new List<string>() {
                "../.# => ##./#../...",
                ".#./..#/### => #..#/..../..../#..#"
            };
        }
    }
}
