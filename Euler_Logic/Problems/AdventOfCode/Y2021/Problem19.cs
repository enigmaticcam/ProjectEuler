using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem19 : AdventOfCodeBase {
        private List<Scanner> _scanners;
        private List<Combo> _combos;

        public override string ProblemName {
            get { return "Advent of Code 2021: 19"; }
        }

        public override string GetAnswer() {
            GetScanners(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            _temp = new int[3];
            _diff = new int[3];
            SetPointDiffs();
            SetCombos();
            CompareAll();
            return GetCount();
        }

        private int Answer2() {
            _temp = new int[3];
            _diff = new int[3];
            SetPointDiffs();
            SetCombos();
            CompareAll();
            _scanners[0].Offset = new int[3] { 0, 0, 0 };
            return GetHighestDistance();
        }

        private int GetHighestDistance() {
            int best = 0;
            for (int index1 = 0; index1 < _scanners.Count; index1++) {
                var scanner1 = _scanners[index1];
                for (int index2 = index1 + 1; index2 < _scanners.Count; index2++) {
                    var scanner2 = _scanners[index2];
                    var distance = Math.Abs(scanner1.Offset[0] - scanner2.Offset[0]) + Math.Abs(scanner1.Offset[1] - scanner2.Offset[1]) + Math.Abs(scanner1.Offset[2] - scanner2.Offset[2]);
                    if (distance > best) {
                        best = distance;
                    }
                }
            }
            return best;
        }

        private void CompareAll() {
            int count = 1;
            _scanners[0].IsDone = true;
            do {
                foreach (var scanner1 in _scanners) {
                    foreach (var scanner2 in _scanners) {
                        if (scanner1 != scanner2 && scanner1.IsDone && !scanner2.IsDone) {
                            var result = Compare(scanner1, scanner2);
                            if (result) {
                                count++;
                                scanner2.IsDone = true;
                            }
                        }
                    }
                }
            } while (count < _scanners.Count);
        }

        private int GetCount() {
            var hash = new HashSet<Tuple<int, int, int>>();
            foreach (var scanner in _scanners) {
                foreach (var point in scanner.Points) {
                    hash.Add(new Tuple<int, int, int>(point[0], point[1], point[2]));
                }
            }
            return hash.Count;
        }

        private string Output() {
            var hash = new HashSet<Tuple<int, int, int>>();
            foreach (var scanner in _scanners) {
                if (scanner.IsDone) {
                    foreach (var point in scanner.Points) {
                        hash.Add(new Tuple<int, int, int>(point[0], point[1], point[2]));
                    }
                }
            }
            var text = new StringBuilder();
            foreach (var key in hash) {
                text.AppendLine(key.Item1 + "," + key.Item2 + "," + key.Item3);
            }
            return text.ToString();
        }

        private void SetCombos() {
            _combos = new List<Combo>();
            SetCombos(new Combo() { NegOrPos = new int[3] { 1, 1, 1}, Vector = new int[3] { 0, 1, 2 } });
            SetCombos(new Combo() { NegOrPos = new int[3] { -1, -1, 1 }, Vector = new int[3] { 0, 1, 2 } });
            SetCombos(new Combo() { NegOrPos = new int[3] { 1, -1, 1 }, Vector = new int[3] { 1, 0, 2 } });
            SetCombos(new Combo() { NegOrPos = new int[3] { -1, 1, 1 }, Vector = new int[3] { 1, 0, 2 } });
            SetCombos(new Combo() { NegOrPos = new int[3] { 1, 1, -1 }, Vector = new int[3] { 2, 1, 0 } });
            SetCombos(new Combo() { NegOrPos = new int[3] { -1, 1, 1 }, Vector = new int[3] { 2, 1, 0 } });
        }

        private void SetCombos(Combo start) {
            for (int count = 1; count <= 4; count++) {
                var combo = new Combo() {
                    NegOrPos = start.NegOrPos.ToArray(),
                    Vector = start.Vector.ToArray()
                };
                combo.NegOrPos[1] = start.NegOrPos[2] * -1;
                combo.NegOrPos[2] = start.NegOrPos[1];
                combo.Vector[1] = start.Vector[2];
                combo.Vector[2] = start.Vector[1];
                start = combo;
                _combos.Add(combo);
            }
        }

        private bool Compare(Scanner scanner1, Scanner scanner2) {
            var counts = new Dictionary<Tuple<int, int, int>, int>();
            foreach (var point in scanner1.Points) {
                counts.Add(new Tuple<int, int, int>(point[0], point[1], point[2]), 1);
            }
            foreach (var point1 in scanner1.Points) {
                foreach (var point2 in scanner2.Points) {
                    foreach (var combo in _combos) {
                        int count = TryCombo(scanner2, point1, point2, combo, counts);
                        if (count >= 12) {
                            scanner2.Offset = _diff.ToArray();
                            Translate(scanner2, combo, _diff);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void Translate(Scanner scanner, Combo combo, int[] diff) {
            foreach (var point in scanner.Points) {
                //var old = point.ToList();
                _temp[0] = combo.NegOrPos[0] == -1 ? _diff[0] - point[combo.Vector[0]] : _diff[0] + point[combo.Vector[0]];
                _temp[1] = combo.NegOrPos[1] == -1 ? _diff[1] - point[combo.Vector[1]] : _diff[1] + point[combo.Vector[1]];
                _temp[2] = combo.NegOrPos[2] == -1 ? _diff[2] - point[combo.Vector[2]] : _diff[2] + point[combo.Vector[2]];
                point[0] = _temp[0];
                point[1] = _temp[1];
                point[2] = _temp[2];
                if (point[0] == -612 && point[1] == -1695 && point[2] == 449) {
                    bool stop = true;
                }
            }
        }

        private int[] _temp;
        private int[] _diff;
        private int TryCombo(Scanner scanner2, int[] point1, int[] point2, Combo combo, Dictionary<Tuple<int, int, int>, int> counts) {
            int total = 0;
            _diff[0] = point1[0] - (point2[combo.Vector[0]] * combo.NegOrPos[0]);
            _diff[1] = point1[1] - (point2[combo.Vector[1]] * combo.NegOrPos[1]);
            _diff[2] = point1[2] - (point2[combo.Vector[2]] * combo.NegOrPos[2]);
            if (_diff[0] == -20 && _diff[1] == -1133 && _diff[2] == 1061) {
                bool stop = true;
            }
            foreach (var point in scanner2.Points) {
                _temp[0] = combo.NegOrPos[0] == -1 ? _diff[0] - point[combo.Vector[0]] : _diff[0] + point[combo.Vector[0]];
                _temp[1] = combo.NegOrPos[1] == -1 ? _diff[1] - point[combo.Vector[1]] : _diff[1] + point[combo.Vector[1]];
                _temp[2] = combo.NegOrPos[2] == -1 ? _diff[2] - point[combo.Vector[2]] : _diff[2] + point[combo.Vector[2]];
                if (counts.ContainsKey(new Tuple<int, int, int>(_temp[0], _temp[1], _temp[2]))) total++;
            }
            return total;
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
            public bool IsDone { get; set; }
            public int[] Offset { get; set; }
        }

        private class Combo {
            public int[] Vector { get; set; }
            public int[] NegOrPos { get; set; }
        }
    }
}
