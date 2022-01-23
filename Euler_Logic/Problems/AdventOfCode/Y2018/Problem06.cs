using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem06 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 6"; }
        }

        public override string GetAnswer() {
            return Answer1();
        }

        public override string GetAnswer2() {
            return Answer2();
        }

        private string Answer1() {
            var points = GetPoints();
            GetGridSize(points);
            CalcDistances(points);
            return _distance.Values.Max().ToString();
        }

        private string Answer2() {
            var points = GetPoints();
            GetGridSize(points);
            return CalcRegion(points).ToString();
        }

        private Dictionary<Tuple<int, int>, int> _distance = new Dictionary<Tuple<int, int>, int>();
        private void CalcDistances(List<Tuple<int, int>> points) {
            for (int x = _gridMin.Item1; x <= _gridMax.Item1; x++) {
                for (int y = _gridMin.Item2; y <= _gridMax.Item2; y++) {
                    int shortestDistance = int.MaxValue;
                    Tuple<int, int> shortestPoint = null;
                    bool multipleFound = false;
                    foreach (var point in points) {
                        var dist = Math.Abs(x - point.Item1) + Math.Abs(y - point.Item2);
                        if (dist == shortestDistance) {
                            multipleFound = true;
                        } else if (dist < shortestDistance) {
                            shortestDistance = dist;
                            shortestPoint = point;
                            multipleFound = false;
                        }
                    }
                    if (!multipleFound) {
                        if (!_distance.ContainsKey(shortestPoint)) {
                            _distance.Add(shortestPoint, 1);
                        } else {
                            _distance[shortestPoint]++;
                        }
                    }
                }
            }
        }

        private int CalcRegion(List<Tuple<int, int>> points) {
            int count = 0;
            for (int x = _gridMin.Item1; x <= _gridMax.Item1; x++) {
                for (int y = _gridMin.Item2; y <= _gridMax.Item2; y++) {
                    int sum = 0;
                    foreach (var point in points) {
                        sum += Math.Abs(x - point.Item1) + Math.Abs(y - point.Item2);
                        if (sum >= 10000) {
                            break;
                        }
                    }
                    if (sum < 10000) {
                        count++;
                    }
                }
            }
            return count;
        }

        private Tuple<int, int> _gridMin;
        private Tuple<int, int> _gridMax;
        private void GetGridSize(List<Tuple<int, int>> points) {
            int lowestX = int.MaxValue;
            int lowestY = int.MaxValue;
            int highestX = 0;
            int highestY = 0;
            foreach (var point in points) {
                if (point.Item1 < lowestX) {
                    lowestX = point.Item1;
                }
                if (point.Item2 < lowestY) {
                    lowestY = point.Item2;
                }
                if (point.Item1 > highestX) {
                    highestX = point.Item1;
                }
                if (point.Item2 > highestY) {
                    highestY = point.Item2;
                }
            }
            _gridMin = new Tuple<int, int>(lowestX, lowestY);
            _gridMax = new Tuple<int, int>(highestX, highestY);
        }

        private List<Tuple<int, int>> GetPoints() {
            return Input().Select(line => {
                var split = line.Split(',');
                return new Tuple<int, int>(
                    Convert.ToInt32(split[0].Trim()),
                    Convert.ToInt32(split[1].Trim())
                );
            }).ToList();
        }
    }
}
