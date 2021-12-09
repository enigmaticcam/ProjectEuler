using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem09 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, Point>> _heights;
        private List<int> _basinCounts;

        public override string ProblemName {
            get { return "Advent of Code 2021: 09"; }
        }

        public override string GetAnswer() {
            GetHeights(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            int risk = 0;
            var maxX = _heights.Keys.Max();
            var maxY = _heights[0].Keys.Max(); 
            for (int x = 0; x <= maxX; x++) {
                for (int y = 0; y <= maxY; y++) {
                    var point = _heights[x][y];
                    int adjacentCount = 0;
                    int lowCount = 0;
                    if (x > 0) {
                        adjacentCount++;
                        if (point.Height < _heights[x - 1][y].Height) lowCount++;
                    }
                    if (x < maxX) {
                        adjacentCount++;
                        if (point.Height < _heights[x + 1][y].Height) lowCount++;
                    }
                    if (y > 0) {
                        adjacentCount++;
                        if (point.Height < _heights[x][y - 1].Height) lowCount++;
                    }
                    if (y < maxY) {
                        adjacentCount++;
                        if (point.Height < _heights[x][y + 1].Height) lowCount++;
                    }
                    if (adjacentCount == lowCount) {
                        risk += 1 + point.Height;
                    }
                }
            }
            return risk;
        }

        private int Answer2() {
            _basinCounts = new List<int>();
            var maxX = _heights.Keys.Max();
            var maxY = _heights[0].Keys.Max();
            int basinId = 0;
            for (int x = 0; x <= maxX; x++) {
                for (int y = 0; y <= maxY; y++) {
                    var point = _heights[x][y];
                    if (point.Height != 9 && point.BasinId == 0) {
                        basinId++;
                        _basinCounts.Add(1);
                        AddToBasinRecurisve(x, y, basinId, maxX, maxY);
                    }
                }
            }

            var top3 = _basinCounts.OrderByDescending(x => x).Take(3).ToList();
            return top3[0] * top3[1] * top3[2];
        }

        private void AddToBasinRecurisve(int x, int y, int basinId, int maxX, int maxY) {
            var point = _heights[x][y];
            point.BasinId = basinId;
            if (x > 0) {
                var nextPoint = _heights[x - 1][y];
                if (nextPoint.Height != 9 && nextPoint.BasinId == 0) {
                    _basinCounts[_basinCounts.Count - 1]++;
                    AddToBasinRecurisve(x - 1, y, basinId, maxX, maxY);
                }
            }
            if (x < maxX) {
                var nextPoint = _heights[x + 1][y];
                if (nextPoint.Height != 9 && nextPoint.BasinId == 0) {
                    _basinCounts[_basinCounts.Count - 1]++;
                    AddToBasinRecurisve(x + 1, y, basinId, maxX, maxY);
                }
            }
            if (y > 0) {
                var nextPoint = _heights[x][y - 1];
                if (nextPoint.Height != 9 && nextPoint.BasinId == 0) {
                    _basinCounts[_basinCounts.Count - 1]++;
                    AddToBasinRecurisve(x, y - 1, basinId, maxX, maxY);
                }
            }
            if (y < maxY) {
                var nextPoint = _heights[x][y + 1];
                if (nextPoint.Height != 9 && nextPoint.BasinId == 0) {
                    _basinCounts[_basinCounts.Count - 1]++;
                    AddToBasinRecurisve(x, y + 1, basinId, maxX, maxY);
                }
            }
        }

        private void GetHeights(List<string> input) {
            _heights = new Dictionary<int, Dictionary<int, Point>>();
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    var point = new Point() { X = x, Y = y, Height = Convert.ToInt32(new string(new char[1] { input[y][x] })) };
                    if (!_heights.ContainsKey(x)) {
                        _heights.Add(x, new Dictionary<int, Point>());
                    }
                    _heights[x].Add(y, point);
                }
            }
        }

        private class Point {
            public int Height { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int BasinId { get; set; }
        }
    }
}
