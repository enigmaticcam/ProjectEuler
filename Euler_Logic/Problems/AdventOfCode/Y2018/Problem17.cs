using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem17 : AdventOfCodeBase {
        private int _count;
        private int _xOffset;
        private List<WaterVelocity> _water;
        private Grid _grid;
        private List<WaterVelocity> _waterToAdd;

        public override string ProblemName {
            get { return "Advent of Code 2018: 17"; }
        }

        public override string GetAnswer() {
            return Answer1();
        }

        public string Answer1() {
            Initialize();
            FillWater();
            return GetWaterCount().ToString();
        }

        private int GetWaterCount() {
            int count = 0;
            for (int x = 0; x <= _grid.Points.GetUpperBound(0); x++) {
                for (int y = 0; y <= _grid.Points.GetUpperBound(1); y++) {
                    if (_grid.Points[x, y].PointType == enumPointType.WaterFlowing || _grid.Points[x, y].PointType == enumPointType.WaterStagnant) {
                        count++;
                    }
                }
            }
            return count;
        }

        private void Initialize() {
            _count = 1;
            _grid = GetGrid(Input());
            _waterToAdd = new List<WaterVelocity>();
            _water = new List<WaterVelocity>();
            _water.Add(new WaterVelocity() {
                Direction = enumDirection.Down,
                X = 500 - _xOffset,
                Y = 0,
                Max = 0
            });
        }

        private void FillWater() {
            bool allDead = false;
            int count = 0;
            do {
                count++;
                if (count % 1000 == 0) {
                    bool stop = true;
                }
                if (count % 100 == 0) {
                    bool stop = true;
                }
                if (count % 50 == 0) {
                    bool stop = true;
                }
                if (count % 25 == 0) {
                    bool stop = true;
                }
                if (count % 10 == 0) {
                    bool stop = true;
                }
                if (count == 650) {
                    bool stop = true;
                }
                allDead = true;
                foreach (var water in _water) {
                    if (!water.IsDead) {
                        allDead = false;
                        if (water.Direction == enumDirection.Down) {
                            MoveWaterVertical(water);
                        } else {
                            MoveWaterHorizontal(water);
                        }
                    }
                }
                if (_waterToAdd.Count > 0) {
                    _water.AddRange(_waterToAdd);
                    _waterToAdd.Clear();
                }
            } while (!allDead);
            bool xyz = true;
        }

        private void MoveWaterVertical(WaterVelocity water) {
            if (water.Y + 1 <= _grid.Points.GetUpperBound(1)) {
                var point = _grid.Points[water.X, water.Y + 1];
                if (point.PointType == enumPointType.Blank) {
                    point.PointType = enumPointType.WaterFlowing;
                    water.Y++;
                } else {
                    water.IsDead = true;
                    water.Children = new WaterVelocity[2];
                    water.Children[0] = new WaterVelocity() {
                        Direction = enumDirection.Left,
                        Parent = water,
                        X = water.X,
                        Y = water.Y
                    };
                    water.Children[1] = new WaterVelocity() {
                        Direction = enumDirection.Right,
                        Parent = water,
                        X = water.X,
                        Y = water.Y
                    };
                    _waterToAdd.AddRange(water.Children);
                    MoveWaterHorizontal(water.Children[0]);
                    MoveWaterHorizontal(water.Children[1]);
                }
            } else {
                water.IsDead = true;
            }
        }
        
        private void MoveWaterHorizontal(WaterVelocity water) {
            int direction = -1;
            if (water.Direction == enumDirection.Right) {
                direction = 1;
            }
            var point = _grid.Points[water.X + direction, water.Y];
            if (_grid.Points[water.X, water.Y + 1].PointType == enumPointType.Blank) {
                water.IsDead = true;
                water.Children = new WaterVelocity[1];
                water.Children[0] = new WaterVelocity() {
                    Direction = enumDirection.Down,
                    Parent = water,
                    X = water.X,
                    Y = water.Y,
                    Max = water.Y
                };
                _waterToAdd.Add(water.Children[0]);
                MoveWaterVertical(water.Children[0]);
            } else if (point.PointType != enumPointType.Clay) {
                point.PointType = enumPointType.WaterFlowing;
                water.X += direction;
            } else {
                water.IsDead = true;
                Rollup(water);
            }
        }

        private void Rollup(WaterVelocity water) {
            bool finished = false;
            do {
                water = water.Parent;
                if (!HasChildrenAlive(water)) {
                    if (water.Direction == enumDirection.Down) {
                        if (ReactivateVertical(water)) {
                            finished = true;
                        }
                    } else if (ReactiveHorizontal(water)) {
                        finished = true;
                    }
                } else {
                    finished = true;
                }
            } while (!finished);
        }

        private bool HasChildrenAlive(WaterVelocity water) {
            if (water.Children == null) {
                return false;
            } else if (water.Children.Where(x => x.IsDead == false).Count() > 0) {
                return true;
            } else {
                foreach (var child in water.Children) {
                    if (HasChildrenAlive(child)) {
                        return true;
                    }
                }
                return false;
            }
        }

        private bool ReactivateVertical(WaterVelocity water) {
            if (water.Y > water.Max) {
                water.Y--;
                water.Children[0].X = water.X;
                water.Children[0].Y = water.Y;
                water.Children[0].IsDead = false;
                water.Children[1].X = water.X;
                water.Children[1].Y = water.Y;
                water.Children[1].IsDead = false;
                MoveWaterHorizontal(water.Children[0]);
                MoveWaterHorizontal(water.Children[1]);
                return true;
            }
            return false;
        }

        private bool ReactiveHorizontal(WaterVelocity water) {
            water.IsDead = false;
            MoveWaterHorizontal(water);
            return true;
        }

        private Grid GetGrid(List<string> input) {
            var hash = new Dictionary<Tuple<int, int>, Point>();
            foreach (var line in input) {
                var split = line.Split(' ');
                if (split[0][0] == 'x') {
                    var x = Convert.ToInt32(split[0].Replace("x=", "").Replace(",", ""));
                    var yRange = split[1].Replace("y=", "").Replace("..", " ").Split(' ').Select(num => Convert.ToInt32(num));
                    for (int y = yRange.First(); y <= yRange.Last(); y++) {
                        AddPoint(hash, x, y);
                    }
                } else {
                    var y = Convert.ToInt32(split[0].Replace("y=", "").Replace(",", ""));
                    var xRange = split[1].Replace("x=", "").Replace("..", " ").Split(' ').Select(num => Convert.ToInt32(num));
                    for (int x = xRange.First(); x <= xRange.Last(); x++) {
                        AddPoint(hash, x, y);
                    }
                }
            }
            var normalized = Normalize(hash);
            FillBlanks(normalized);
            return new Grid() {
                Points = normalized,
                Keys = Keys(normalized)
            };
        }

        private void AddPoint(Dictionary<Tuple<int, int>, Point> hash, int x, int y) {
            var key = new Tuple<int, int>(x, y);
            if (!hash.ContainsKey(key)) {
                var point = new Point();
                point.PointType = enumPointType.Clay;
                hash.Add(key, point);
            }
        }

        private Point[,] Normalize(Dictionary<Tuple<int, int>, Point> points) {
            int xMin = points.Keys.Select(x => x.Item1).Min();
            int xMax = points.Keys.Select(x => x.Item1).Max();
            int yMax = points.Keys.Select(x => x.Item2).Max();
            _xOffset = xMin - 100;
            var normalized = new Point[xMax - xMin + 201, yMax + 1];
            var keys = new Tuple<int, int>[normalized.GetUpperBound(0) + 1, normalized.GetUpperBound(1) + 1];
            foreach (var point in points) {
                point.Value.X = point.Key.Item1 - _xOffset;
                point.Value.Y = point.Key.Item2;
                point.Value.Key = new Tuple<int, int>(point.Value.X, point.Value.Y);
                normalized[point.Value.X, point.Value.Y] = point.Value;
            }
            return normalized;
        }

        private void FillBlanks(Point[,] points) {
            for (int x = 0; x <= points.GetUpperBound(0); x++) {
                for (int y = 0; y <=points.GetUpperBound(1); y++) {
                    if (points[x, y] == null) {
                        points[x, y] = new Point() {
                            Key = new Tuple<int, int>(x, y),
                            PointType = enumPointType.Blank,
                            X = x,
                            Y = y
                        };
                    }
                }
            }
        }

        private Tuple<int, int>[,] Keys(Point[,] points) {
            var keys = new Tuple<int, int>[points.GetUpperBound(0) + 1, points.GetUpperBound(1) + 1];
            for (int x = 0; x <= points.GetUpperBound(0); x++) {
                for (int y = 0; y <= points.GetUpperBound(1); y++) {
                    var key = new Tuple<int, int>(x, y);
                    keys[x, y] = key;
                }
            }
            return keys;
        }

        private string PrintOutput(int markX, int markY) {
            var alive = _water.Where(x => !x.IsDead);
            var text = new StringBuilder();
            for (int y = 0; y <= _grid.Points.GetUpperBound(1); y++) {
                string line = "";
                for (int x = 0; x <= _grid.Points.GetUpperBound(0); x++) {
                    string toAdd = "";
                    if (x == markX && y == markY) {
                        line += "*";
                    } else {
                        switch (_grid.Points[x, y].PointType) {
                            case enumPointType.Blank:
                                toAdd = ".";
                                break;
                            case enumPointType.Clay:
                                toAdd = "#";
                                break;
                            case enumPointType.WaterFlowing:
                                toAdd = "|";
                                break;
                            case enumPointType.WaterStagnant:
                                toAdd = "~";
                                break;
                        }
                    }
                    foreach (var water in alive) {
                        if (!water.IsDead && water.X == x && water.Y == y) {
                            switch (water.Direction) {
                                case enumDirection.Down:
                                    toAdd = "v";
                                    break;
                                case enumDirection.Left:
                                    toAdd = "<";
                                    break;
                                case enumDirection.Right:
                                    toAdd = ">";
                                    break;
                            }
                        }
                    }
                    line += toAdd;
                }
                text.AppendLine(line);
            }
            return text.ToString();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "x=495, y=2..7",
                "y=7, x=495..501",
                "x=501, y=3..7",
                "x=498, y=2..4",
                "x=506, y=1..2",
                "x=498, y=10..13",
                "x=504, y=10..13",
                "y=13, x=498..504"
            };
        }

        private enum enumPointType {
            Blank,
            Clay,
            WaterFlowing,
            WaterStagnant
        }

        private enum enumDirection {
            Left,
            Right,
            Down
        }

        private class WaterVelocity {
            public int X { get; set; }
            public int Y { get; set; }
            public enumDirection Direction { get; set; }
            public bool IsDead { get; set; }
            public WaterVelocity Parent { get; set; }
            public WaterVelocity[] Children { get; set; }
            public int Max { get; set; }
        }

        private class Point {
            public int X { get; set; }
            public int Y { get; set; }
            public Tuple<int, int> Key { get; set; }
            public enumPointType PointType { get; set; }
            public bool HasWater { get; set; }
        }

        private class Grid {
            public Point[,] Points { get; set; }
            public Tuple<int, int>[,] Keys { get; set; }
        }
    }
}
