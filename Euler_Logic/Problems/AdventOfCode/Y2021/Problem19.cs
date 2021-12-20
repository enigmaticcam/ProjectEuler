using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem19 : AdventOfCodeBase {
        private List<Scanner> _scanners;

        public override string ProblemName {
            get { return "Advent of Code 2021: 19"; }
        }

        public override string GetAnswer() {
            GetScanners(Input_Test(1));
            return Answer1().ToString();
        }

        private int Answer1() {
            SetPointDiffs();
            var result = Compare(_scanners[1], _scanners[4]);
            return 0;
        }

        private Offset Compare(Scanner scanner1, Scanner scanner2) {
            var hash = new Dictionary<Tuple<int, int, int>, int>();
            foreach (var diff1 in scanner1.Diffs) {
                if (!diff1.NotOptimal) {
                    foreach (var diff2 in scanner2.Diffs) {
                        if (!diff2.NotOptimal) {
                            if (diff1.AbsX == diff2.AbsX && diff1.AbsY == diff2.AbsY && diff1.AbsZ == diff2.AbsZ) {
                                var offset = new Offset() {
                                    Vector = new int[3],
                                    Diff = new int[3]
                                };
                                if (diff1.AbsX == diff2.AbsX) {
                                    offset.Vector[0] = 0;
                                } else if (diff1.AbsX == diff2.AbsY) {
                                    offset.Vector[0] = 1;
                                } else {
                                    offset.Vector[0] = 2;
                                }
                                if (diff1.AbsY == diff2.AbsX) {
                                    offset.Vector[1] = 0;
                                } else if (diff1.AbsY == diff2.AbsY) {
                                    offset.Vector[1] = 1;
                                } else {
                                    offset.Vector[1] = 2;
                                }
                                if (diff1.AbsZ == diff2.AbsX) {
                                    offset.Vector[2] = 0;
                                } else if (diff1.AbsZ == diff2.AbsY) {
                                    offset.Vector[2] = 1;
                                } else {
                                    offset.Vector[2] = 2;
                                }

                                // Point 1 = point 1
                                for (int index = 0; index <= 2; index++) {
                                    if (diff1.Point1[index] - diff2.Point1[offset.Vector[index]] == diff1.Point2[index] - diff2.Point2[offset.Vector[index]]) {
                                        offset.Diff[index] = diff1.Point1[index] - diff2.Point1[offset.Vector[index]];
                                    } else {
                                        offset.Diff[index] = diff1.Point1[index] + diff2.Point1[offset.Vector[index]];
                                    }
                                }
                                var key = new Tuple<int, int, int>(offset.Diff[0], offset.Diff[1], offset.Diff[2]);
                                if (hash.ContainsKey(key)) {
                                    hash[key]++;
                                } else {
                                    hash.Add(key, 1);
                                }

                                // Point 1 = point 2
                                for (int index = 0; index <= 2; index++) {
                                    if (diff1.Point1[index] - diff2.Point2[offset.Vector[index]] == diff1.Point2[index] - diff2.Point1[offset.Vector[index]]) {
                                        offset.Diff[index] = diff1.Point1[index] - diff2.Point2[offset.Vector[index]];
                                    } else {
                                        offset.Diff[index] = diff1.Point1[index] + diff2.Point2[offset.Vector[index]];
                                    }
                                }
                                key = new Tuple<int, int, int>(offset.Diff[0], offset.Diff[1], offset.Diff[2]);
                                if (hash.ContainsKey(key)) {
                                    hash[key]++;
                                } else {
                                    hash.Add(key, 1);
                                }
                            }
                        }
                    }
                }
            }
            var test = hash.OrderByDescending(x => x.Value).ToList();
            return null;
        }

        private void SetPointDiffs() {
            foreach (var scanner in _scanners) {
                SetPointDiffs(scanner);
            }
        }

        private void SetPointDiffs(Scanner scanner) {
            scanner.Diffs = new List<PointDiff>();
            for (int index1 = 0; index1 < scanner.Points.Count; index1++) {
                var point1 = scanner.Points[index1];
                for (int index2 = index1 + 1; index2 < scanner.Points.Count; index2++) {
                    var point2 = scanner.Points[index2];
                    var diff = new PointDiff() {
                        Point1 = point1,
                        Point2 = point2,
                        X = point1[0] - point2[0],
                        Y = point1[1] - point2[1],
                        Z = point1[2] - point2[2],
                    };
                    var abs = new List<int>() { Math.Abs(diff.X), Math.Abs(diff.Y), Math.Abs(diff.Z) }.OrderBy(x => x);
                    diff.AbsX = abs.ElementAt(0);
                    diff.AbsY = abs.ElementAt(1);
                    diff.AbsZ = abs.ElementAt(2);
                    scanner.Diffs.Add(diff);
                    diff.NotOptimal = HasSameValues(point1) | HasSameValues(point2);
                }
            }
        }

        private bool HasSameValues(int[] vector) {
            return vector[0] == vector[1] || vector[0] == vector[2] || vector[1] == vector[2];
        }

        private void GetScanners(List<string> input) {
            _scanners = new List<Scanner>();
            int index = 0;
            do {
                var scanSplit = input[index].Split(' ');
                var scanner = new Scanner() {
                    Number = Convert.ToInt32(scanSplit[2]),
                    Points = new List<int[]>()
                };
                index++;
                do {
                    var pointSplit = input[index].Split(',');
                    var point = new int[3] {
                        Convert.ToInt32(pointSplit[0]),
                        Convert.ToInt32(pointSplit[1]),
                        Convert.ToInt32(pointSplit[2])
                    };
                    scanner.Points.Add(point);
                    index++;
                } while (index < input.Count && input[index] != "");
                index++;
                _scanners.Add(scanner);
            } while (index < input.Count);
        }

        public class PointDiff {
            public int[] Point1 { get; set; }
            public int[] Point2 { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int AbsX { get; set; }
            public int AbsY { get; set; }
            public int AbsZ { get; set; }
            public bool NotOptimal { get; set; }
        }

        public class Scanner {
            public int Number { get; set; }
            public List<int[]> Points { get; set; }
            public List<PointDiff> Diffs { get; set; }
        }

        public class Offset {
            public int[] Vector { get; set; }
            public int[] Diff { get; set; }
        }
    }
}
