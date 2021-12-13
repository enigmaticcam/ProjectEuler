using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem24 : AdventOfCodeBase {
        private Point[,] _grid;
        private bool[,] _bugs;
        private ulong _bits;
        private Dictionary<int, List<Point2>> _joins;
        private Dictionary<int, Dictionary<int, Point2>> _grid2;
        private List<Point2> _points;

        public override string ProblemName {
            get { return "Advent of Code 2019: 24"; }
        }

        public override string GetAnswer() {
            //GetGrid(Input_Test(1));
            return Answer2().ToString();
        }

        private ulong Answer1() {
            var hash = new HashSet<ulong>();
            hash.Add(_bits);
            do {
                RunCycle();
                if (hash.Contains(_bits)) {
                    return _bits;
                }
                hash.Add(_bits);
            } while (true);
        }

        private int Answer2() {
            SetJoins();
            GetGrid2(Input());
            SetGrid2(_points);
            AddBlankNeighbors();
            RunCycle2(200);
            return CountBugs();
        }

        private int CountBugs() {
            return _points.Where(x => x.IsBug).Count();
        }

        private void RunCycle2(int maxCount) {
            for (int count = 1; count <= maxCount; count++) {
                var newPoints = new List<Point2>();
                foreach (var point in _points) {
                    int bugCount = 0;
                    foreach (var join in _joins[point.Index]) {
                        var nextZ = point.Z + join.Z;
                        if (_grid2.ContainsKey(nextZ) && _grid2[nextZ].ContainsKey(join.Index) && _grid2[nextZ][join.Index].IsBug) bugCount++;
                    }
                    if ((point.IsBug && bugCount == 1) || (!point.IsBug && (bugCount == 1 || bugCount == 2))) {
                        newPoints.Add(new Point2() {
                            Index = point.Index,
                            IsBug = true,
                            Z = point.Z
                        });
                    }
                }
                _points = newPoints;
                SetGrid2(_points);
                AddBlankNeighbors();
            }
        }

        private void GetGrid2(List<string> input) {
            _points = new List<Point2>();
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    if (input[y][x] == '#') {
                        var point = new Point2() { Index = y * 5 + x + 1, Z = 0, IsBug = true };
                        _points.Add(point);
                    }
                }
            }
        }

        private void SetGrid2(List<Point2> points) {
            _grid2 = new Dictionary<int, Dictionary<int, Point2>>();
            _grid2.Add(0, new Dictionary<int, Point2>());
            foreach (var point in points) {
                if (point.IsBug) {
                    if (!_grid2.ContainsKey(point.Z)) {
                        _grid2.Add(point.Z, new Dictionary<int, Point2>());
                    }
                    _grid2[point.Z].Add(point.Index, point);
                }
            }
        }

        private void AddBlankNeighbors() {
            var newPoints = new List<Point2>();
            foreach (var point in _points) {
                foreach (var join in _joins[point.Index]) {
                    var nextZ = point.Z + join.Z;
                    if (!_grid2.ContainsKey(nextZ)) {
                        _grid2.Add(nextZ, new Dictionary<int, Point2>());
                    }
                    if (!_grid2[nextZ].ContainsKey(join.Index)) {
                        var newPoint = new Point2() { Index = join.Index, Z = nextZ};
                        _grid2[nextZ].Add(join.Index, newPoint);
                        newPoints.Add(newPoint);
                    }
                }
            }
            _points.AddRange(newPoints);
        }

        private void SetJoins() {
            _joins = new Dictionary<int, List<Point2>>();
            Enumerable.Range(1, 25).ToList().ForEach(x => _joins.Add(x, new List<Point2>()));

            // 1
            _joins[1].Add(new Point2() { Index = 12, Z = -1 });
            _joins[1].Add(new Point2() { Index = 2, Z = 0 });
            _joins[1].Add(new Point2() { Index = 6, Z = 0 });
            _joins[1].Add(new Point2() { Index = 8, Z = -1 });

            // 2
            _joins[2].Add(new Point2() { Index = 1, Z = 0 });
            _joins[2].Add(new Point2() { Index = 3, Z = 0 });
            _joins[2].Add(new Point2() { Index = 7, Z = 0 });
            _joins[2].Add(new Point2() { Index = 8, Z = -1 });

            // 3
            _joins[3].Add(new Point2() { Index = 2, Z = 0 });
            _joins[3].Add(new Point2() { Index = 4, Z = 0 });
            _joins[3].Add(new Point2() { Index = 8, Z = 0 });
            _joins[3].Add(new Point2() { Index = 8, Z = -1 });

            // 4
            _joins[4].Add(new Point2() { Index = 3, Z = 0 });
            _joins[4].Add(new Point2() { Index = 5, Z = 0 });
            _joins[4].Add(new Point2() { Index = 9, Z = 0 });
            _joins[4].Add(new Point2() { Index = 8, Z = -1 });

            // 5
            _joins[5].Add(new Point2() { Index = 4, Z = 0 });
            _joins[5].Add(new Point2() { Index = 14, Z = -1 });
            _joins[5].Add(new Point2() { Index = 10, Z = 0 });
            _joins[5].Add(new Point2() { Index = 8, Z = -1 });

            // 6
            _joins[6].Add(new Point2() { Index = 12, Z = -1 });
            _joins[6].Add(new Point2() { Index = 7, Z = 0 });
            _joins[6].Add(new Point2() { Index = 11, Z = 0 });
            _joins[6].Add(new Point2() { Index = 1, Z = 0 });

            // 7
            _joins[7].Add(new Point2() { Index = 6, Z = 0 });
            _joins[7].Add(new Point2() { Index = 8, Z = 0 });
            _joins[7].Add(new Point2() { Index = 12, Z = 0 });
            _joins[7].Add(new Point2() { Index = 2, Z = 0 });

            // 8
            _joins[8].Add(new Point2() { Index = 7, Z = 0 });
            _joins[8].Add(new Point2() { Index = 3, Z = 0 });
            _joins[8].Add(new Point2() { Index = 9, Z = 0 });
            _joins[8].Add(new Point2() { Index = 1, Z = 1 });
            _joins[8].Add(new Point2() { Index = 2, Z = 1 });
            _joins[8].Add(new Point2() { Index = 3, Z = 1 });
            _joins[8].Add(new Point2() { Index = 4, Z = 1 });
            _joins[8].Add(new Point2() { Index = 5, Z = 1 });

            // 9
            _joins[9].Add(new Point2() { Index = 4, Z = 0 });
            _joins[9].Add(new Point2() { Index = 14, Z = 0 });
            _joins[9].Add(new Point2() { Index = 8, Z = 0 });
            _joins[9].Add(new Point2() { Index = 10, Z = 0 });

            // 10
            _joins[10].Add(new Point2() { Index = 9, Z = 0 });
            _joins[10].Add(new Point2() { Index = 5, Z = 0 });
            _joins[10].Add(new Point2() { Index = 15, Z = 0 });
            _joins[10].Add(new Point2() { Index = 14, Z = -1 });

            // 11
            _joins[11].Add(new Point2() { Index = 6, Z = 0 });
            _joins[11].Add(new Point2() { Index = 12, Z = 0 });
            _joins[11].Add(new Point2() { Index = 16, Z = 0 });
            _joins[11].Add(new Point2() { Index = 12, Z = -1 });

            // 12
            _joins[12].Add(new Point2() { Index = 7, Z = 0 });
            _joins[12].Add(new Point2() { Index = 11, Z = 0 });
            _joins[12].Add(new Point2() { Index = 17, Z = 0 });
            _joins[12].Add(new Point2() { Index = 1, Z = 1 });
            _joins[12].Add(new Point2() { Index = 6, Z = 1 });
            _joins[12].Add(new Point2() { Index = 11, Z = 1 });
            _joins[12].Add(new Point2() { Index = 16, Z = 1 });
            _joins[12].Add(new Point2() { Index = 21, Z = 1 });

            // 14
            _joins[14].Add(new Point2() { Index = 9, Z = 0 });
            _joins[14].Add(new Point2() { Index = 15, Z = 0 });
            _joins[14].Add(new Point2() { Index = 19, Z = 0 });
            _joins[14].Add(new Point2() { Index = 5, Z = 1 });
            _joins[14].Add(new Point2() { Index = 10, Z = 1 });
            _joins[14].Add(new Point2() { Index = 15, Z = 1 });
            _joins[14].Add(new Point2() { Index = 20, Z = 1 });
            _joins[14].Add(new Point2() { Index = 25, Z = 1 });

            // 15
            _joins[15].Add(new Point2() { Index = 10, Z = 0 });
            _joins[15].Add(new Point2() { Index = 14, Z = 0 });
            _joins[15].Add(new Point2() { Index = 20, Z = 0 });
            _joins[15].Add(new Point2() { Index = 14, Z = -1 });

            // 16
            _joins[16].Add(new Point2() { Index = 11, Z = 0 });
            _joins[16].Add(new Point2() { Index = 17, Z = 0 });
            _joins[16].Add(new Point2() { Index = 21, Z = 0 });
            _joins[16].Add(new Point2() { Index = 12, Z = -1 });

            // 17
            _joins[17].Add(new Point2() { Index = 12, Z = 0 });
            _joins[17].Add(new Point2() { Index = 16, Z = 0 });
            _joins[17].Add(new Point2() { Index = 18, Z = 0 });
            _joins[17].Add(new Point2() { Index = 22, Z = 0 });

            // 18
            _joins[18].Add(new Point2() { Index = 17, Z = 0 });
            _joins[18].Add(new Point2() { Index = 19, Z = 0 });
            _joins[18].Add(new Point2() { Index = 23, Z = 0 });
            _joins[18].Add(new Point2() { Index = 21, Z = 1 });
            _joins[18].Add(new Point2() { Index = 22, Z = 1 });
            _joins[18].Add(new Point2() { Index = 23, Z = 1 });
            _joins[18].Add(new Point2() { Index = 24, Z = 1 });
            _joins[18].Add(new Point2() { Index = 25, Z = 1 });

            // 19
            _joins[19].Add(new Point2() { Index = 14, Z = 0 });
            _joins[19].Add(new Point2() { Index = 24, Z = 0 });
            _joins[19].Add(new Point2() { Index = 18, Z = 0 });
            _joins[19].Add(new Point2() { Index = 20, Z = 0 });

            // 20
            _joins[20].Add(new Point2() { Index = 15, Z = 0 });
            _joins[20].Add(new Point2() { Index = 19, Z = 0 });
            _joins[20].Add(new Point2() { Index = 25, Z = 0 });
            _joins[20].Add(new Point2() { Index = 14, Z = -1 });

            // 21
            _joins[21].Add(new Point2() { Index = 16, Z = 0 });
            _joins[21].Add(new Point2() { Index = 22, Z = 0 });
            _joins[21].Add(new Point2() { Index = 12, Z = -1 });
            _joins[21].Add(new Point2() { Index = 18, Z = -1 });

            // 22
            _joins[22].Add(new Point2() { Index = 17, Z = 0 });
            _joins[22].Add(new Point2() { Index = 21, Z = 0 });
            _joins[22].Add(new Point2() { Index = 23, Z = 0 });
            _joins[22].Add(new Point2() { Index = 18, Z = -1 });

            // 23
            _joins[23].Add(new Point2() { Index = 18, Z = 0 });
            _joins[23].Add(new Point2() { Index = 22, Z = 0 });
            _joins[23].Add(new Point2() { Index = 24, Z = 0 });
            _joins[23].Add(new Point2() { Index = 18, Z = -1 });

            // 24
            _joins[24].Add(new Point2() { Index = 19, Z = 0 });
            _joins[24].Add(new Point2() { Index = 23, Z = 0 });
            _joins[24].Add(new Point2() { Index = 25, Z = 0 });
            _joins[24].Add(new Point2() { Index = 18, Z = -1 });

            // 25
            _joins[25].Add(new Point2() { Index = 20, Z = 0 });
            _joins[25].Add(new Point2() { Index = 24, Z = 0 });
            _joins[25].Add(new Point2() { Index = 14, Z = -1 });
            _joins[25].Add(new Point2() { Index = 18, Z = -1 });
        }

        private void RunCycle() {
            _bits = 0;
            var next = new bool[5, 5];
            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 5; y++) {
                    int bugCount = 0;
                    if (x > 0 && _bugs[x - 1, y]) bugCount++;
                    if (x < 4 && _bugs[x + 1, y]) bugCount++;
                    if (y > 0 && _bugs[x, y - 1]) bugCount++;
                    if (y < 4 && _bugs[x, y + 1]) bugCount++;
                    var bugExists = _bugs[x, y];
                    if ((bugExists && bugCount == 1) || (!bugExists && (bugCount == 1 || bugCount == 2))) {
                        next[x, y] = true;
                        _bits += _grid[x, y].Bit;
                    }
                }
            }
            _bugs = next;
        }

        private void GetGrid(List<string> input) {
            _grid = new Point[5, 5];
            _bugs = new bool[5, 5];
            ulong bit = 1;
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[0].Length; x++) {
                    var point = new Point() {
                        Bit = bit,
                        X = x,
                        Y = y
                    };
                    _grid[x, y] = point;
                    _bugs[x, y] = input[y][x] == '#';
                    bit *= 2;
                }
            }
        }

        private class Point {
            public int X { get; set; }
            public int Y { get; set; }
            public ulong Bit { get; set; }
        }

        public class Point2 {
            public int Index { get; set; }
            public int Z { get; set; }
            public bool IsBug { get; set; }
        }
    }
}
