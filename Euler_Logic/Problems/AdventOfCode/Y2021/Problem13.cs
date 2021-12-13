using System;
using System.Collections.Generic;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem13 : AdventOfCodeBase {
        private List<Point> _points;
        private List<Fold> _folds;

        public override string ProblemName {
            get { return "Advent of Code 2021: 13"; }
        }

        public override string GetAnswer() {
            GetPointsAndFolds(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            DoFold(1);
            return _points.Count;
        }

        private string Answer2() {
            DoFold(_folds.Count);
            return "CJHAZHKU";
        }

        private string Output() {
            int maxX = 0;
            int maxY = 0;
            var hash = new Dictionary<int, HashSet<int>>();
            foreach (var point in _points) {
                if (!hash.ContainsKey(point.X)) {
                    hash.Add(point.X, new HashSet<int>());
                }
                hash[point.X].Add(point.Y);
                if (point.X > maxX) {
                    maxX = point.X;
                }
                if (point.Y > maxY) {
                    maxY = point.Y;
                }
            }
            var output = new StringBuilder();
            for (int y = 0; y <= maxY; y++) {
                for (int x = 0; x <= maxX; x++) {
                    if (hash.ContainsKey(x) && hash[x].Contains(y)) {
                        output.Append("#");
                    } else {
                        output.Append(".");
                    }
                }
                output.AppendLine();
            }
            return output.ToString();
        }

        private void DoFold(int maxCount) {
            for (int count = 1; count <= maxCount; count++) {
                var fold = _folds[count - 1];
                GetFoldedPoints(fold);
                var temp = new List<Point>();
                foreach (var point in _points) {
                    if (point.IsFolded) {
                        temp.Add(new Point() {
                            X = (fold.IsX ? fold.Value - (point.X - fold.Value) : point.X),
                            Y = (!fold.IsX ? fold.Value - (point.Y - fold.Value) : point.Y)
                        });
                    } else {
                        temp.Add(point);
                    }
                }
                _points = MakeDistinct(temp);
            }
        }

        private List<Point> MakeDistinct(List<Point> points) {
            var hash = new HashSet<Tuple<int, int>>();
            var newPoints = new List<Point>();
            foreach (var point in points) {
                var key = new Tuple<int, int>(point.X, point.Y);
                if (!hash.Contains(key)) {
                    hash.Add(key);
                    newPoints.Add(point);
                    point.IsFolded = false;
                }
            }
            return newPoints;
        }

        private void GetFoldedPoints(Fold fold) {
            foreach (var point in _points) {
                if (fold.IsX && point.X > fold.Value) {
                    point.IsFolded = true;
                } else if (!fold.IsX && point.Y > fold.Value) {
                    point.IsFolded = true;
                }
            }
        }

        private void GetPointsAndFolds(List<string> input) {
            _points = new List<Point>();
            _folds = new List<Fold>();
            foreach (var line in input) {
                if (line.Length > 0) {
                    if (line[0] != 'f') {
                        var split = line.Split(',');
                        _points.Add(new Point() {
                            X = Convert.ToInt32(split[0]),
                            Y = Convert.ToInt32(split[1])
                        });
                    } else {
                        var fold = new Fold();
                        var split = line.Split(' ');
                        fold.IsX = split[2][0] == 'x';
                        fold.Value = Convert.ToInt32(split[2].Substring(2, split[2].Length - 2));
                        _folds.Add(fold);
                    }
                }
            }
        }

        private class Fold {
            public bool IsX { get; set; }
            public int Value { get; set; }
        }

        private class Point {
            public int X { get; set; }
            public int Y { get; set; }
            public bool IsFolded { get; set; }
        }
    }
}
