using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem22 : AdventOfCodeBase {
        private List<Cuboid> _cuboids;

        public override string ProblemName {
            get { return "Advent of Code 2021: 22"; }
        }

        public override string GetAnswer() {
            GetCuboids(Input());
            return Answer1().ToString();
        }

        public override string GetAnswer2() {
            GetCuboids(Input());
            return Answer2().ToString();
        }

        private long Answer2() {
            var result = GetCount();
            return result;
        }

        private void RemoveOutside50() {
            int index = 0;
            while (index < _cuboids.Count) {
                var cuboid = _cuboids[index];
                if (cuboid.Start.X < -50 || cuboid.Start.Y < -50 || cuboid.Start.Z < -50
                    || cuboid.End.X > 50 || cuboid.End.Y > 50 && cuboid.End.Z > 50) {
                    _cuboids.RemoveAt(index);
                } else {
                    index++;
                }
            }
        }

       private long GetCount() {
            long sum = 0;
            for (int index1 = 0; index1 < _cuboids.Count; index1++) {
                var cuboid1 = _cuboids[index1];
                if (cuboid1.On) {
                    var temp = new List<Cuboid>() { cuboid1 };
                    for (int index2 = index1 + 1; index2 < _cuboids.Count; index2++) {
                        var cuboid2 = _cuboids[index2];
                        int subIndex = 0;
                        while (subIndex < temp.Count) {
                            var sub = temp[subIndex];
                            if (OverlapAll(cuboid2, sub)) {
                                temp.RemoveAt(subIndex);
                            } else if (DoesOverlap(cuboid2, sub)) {
                                var split = Split(sub, cuboid2);
                                temp.RemoveAt(subIndex);
                                var range = split.Where(x => x.On && x.SplitParent == sub).ToList();
                                temp.InsertRange(subIndex, range);
                                subIndex += range.Count();
                            } else {
                                subIndex++;
                            }
                        }
                    }
                    sum += Count(temp);
                }
            }
            return sum;
        }

        private IEnumerable<Cuboid> Split(Cuboid cuboid1, Cuboid cuboid2) {
            cuboid1.HadLowestEndX = false;
            cuboid1.HadLowestEndY = false;
            cuboid1.HadLowestStartX = false;
            cuboid1.HadLowestStartY = false;

            cuboid2.HadLowestEndX = false;
            cuboid2.HadLowestEndY = false;
            cuboid2.HadLowestStartX = false;
            cuboid2.HadLowestStartY = false;
            if (cuboid1.Start.X != cuboid2.Start.X) {
                SetLowerHigher(cuboid1, cuboid2, x => x.Start.X);
                _lower.HadLowestStartX = true;
                yield return new Cuboid() {
                    Start = new Point(_lower.Start.X, _lower.Start.Y, _lower.Start.Z),
                    End = new Point(_higher.Start.X - 1, _lower.End.Y, _lower.End.Z),
                    On = _lower.On,
                    SplitParent = _lower
                };
            }
            if (cuboid1.End.X != cuboid2.End.X) {
                SetLowerHigher(cuboid1, cuboid2, x => x.End.X);
                _lower.HadLowestEndX = true;
                yield return new Cuboid() {
                    Start = new Point(_lower.End.X + 1, _higher.Start.Y, _higher.Start.Z),
                    End = new Point(_higher.End.X, _higher.End.Y, _higher.End.Z),
                    On = _higher.On,
                    SplitParent = _higher
                };
            }
            if (cuboid1.Start.Y != cuboid2.Start.Y) {
                SetLowerHigher(cuboid1, cuboid2, x => x.Start.Y);
                _lower.HadLowestStartY = true;
                var startX = _lower.Start.X;
                var endX = _lower.End.X;
                if (_lower.HadLowestStartX) startX = _higher.Start.X;
                if (!_lower.HadLowestEndX) endX = _higher.End.X;
                yield return new Cuboid() {
                    Start = new Point(startX, _lower.Start.Y, _lower.Start.Z),
                    End = new Point(endX, _higher.Start.Y - 1, _lower.End.Z),
                    On = _lower.On,
                    SplitParent = _lower
                };
            }
            if (cuboid1.End.Y != cuboid2.End.Y) {
                SetLowerHigher(cuboid1, cuboid2, x => x.End.Y);
                _lower.HadLowestEndY = true;
                var startX = _higher.Start.X;
                var endX = _higher.End.X;
                if (_higher.HadLowestStartX) startX = _lower.Start.X;
                if (!_higher.HadLowestEndX) endX = _lower.End.X;
                yield return new Cuboid() {
                    Start = new Point(startX, _lower.End.Y + 1, _higher.Start.Z),
                    End = new Point(endX, _higher.End.Y, _higher.End.Z),
                    On = _higher.On,
                    SplitParent = _higher
                };
            }
            if (cuboid1.Start.Z != cuboid2.Start.Z) {
                SetLowerHigher(cuboid1, cuboid2, x => x.Start.Z);
                var startX = _lower.Start.X;
                var endX = _lower.End.X;
                if (_lower.HadLowestStartX) startX = _higher.Start.X;
                if (!_lower.HadLowestEndX) endX = _higher.End.X;
                var startY = _lower.Start.Y;
                var endY = _lower.End.Y;
                if (_lower.HadLowestStartY) startY = _higher.Start.Y;
                if (!_lower.HadLowestEndY) endY = _higher.End.Y;
                yield return new Cuboid() {
                    Start = new Point(startX, startY, _lower.Start.Z),
                    End = new Point(endX, endY, _higher.Start.Z - 1),
                    On = _lower.On,
                    SplitParent = _lower
                };
            }
            if (cuboid1.End.Z != cuboid2.End.Z) {
                SetLowerHigher(cuboid1, cuboid2, x => x.End.Z);
                var startX = _higher.Start.X;
                var endX = _higher.End.X;
                if (_higher.HadLowestStartX) startX = _lower.Start.X;
                if (!_higher.HadLowestEndX) endX = _lower.End.X;
                var startY = _higher.Start.Y;
                var endY = _higher.End.Y;
                if (_higher.HadLowestStartY) startY = _lower.Start.Y;
                if (!_higher.HadLowestEndY) endY = _lower.End.Y;
                yield return new Cuboid() {
                    Start = new Point(startX, startY, _lower.End.Z + 1),
                    End = new Point(endX, endY, _higher.End.Z),
                    On = _higher.On,
                    SplitParent = _higher
                };
            }
        }

        private Cuboid _lower;
        private Cuboid _higher;
        private void SetLowerHigher(Cuboid cuboid1, Cuboid cuboid2, Func<Cuboid, long> axis) {
            _lower = cuboid1;
            _higher = cuboid2;
            if (axis(_higher) < axis(_lower)) {
                _lower = cuboid2;
                _higher = cuboid1;
            }
        }

        private bool OverlapAll(Cuboid cuboid1, Cuboid cuboid2) {
            return cuboid2.Start.X < cuboid1.Start.X
                && cuboid2.Start.Y < cuboid1.Start.Y
                && cuboid2.Start.Z < cuboid1.Start.Z
                && cuboid2.End.X > cuboid1.End.X
                && cuboid2.End.Y > cuboid1.End.Y
                && cuboid2.End.Z > cuboid1.End.Z;
        }

        private bool DoesOverlap(Cuboid cuboid1, Cuboid cuboid2) {
            return cuboid1.Start.X <= cuboid2.End.X
                && cuboid1.Start.Y <= cuboid2.End.Y
                && cuboid1.Start.Z <= cuboid2.End.Z
                && cuboid1.End.X >= cuboid2.Start.X
                && cuboid1.End.Y >= cuboid2.Start.Y
                && cuboid1.End.Z >= cuboid2.Start.Z;
        }

        private long Count(IEnumerable<Cuboid> cuboids) {
            long total = 0;
            foreach (var cuboid in cuboids) {
                var sub = Count(cuboid);
                total += sub;
            }
            return total;
        }

        private long Count(Cuboid cuboid) {
            return Math.Abs(cuboid.End.X - cuboid.Start.X + 1) * Math.Abs(cuboid.End.Y - cuboid.Start.Y + 1) * Math.Abs(cuboid.End.Z - cuboid.Start.Z + 1);
        }

        private long Answer1() {
            RemoveOutside50();
            var result = GetCount();
            return result;
        }

        private void GetCuboids(List<string> input) {
            _cuboids = input.Select(x => {
                var cuboid = new Cuboid();
                var split = x.Split(',');
                cuboid.On = split[0][1] == 'n';
                var subSplit = split[0].Split(' ');
                var xPoint = ParseCoordinate(subSplit[1]);
                var yPoint = ParseCoordinate(split[1]);
                var zPoint = ParseCoordinate(split[2]);
                cuboid.Start = new Point() {
                    X = xPoint.Item1,
                    Y = yPoint.Item1,
                    Z = zPoint.Item1
                };
                cuboid.End = new Point() {
                    X = xPoint.Item2,
                    Y = yPoint.Item2,
                    Z = zPoint.Item2
                };
                return cuboid;
            }).ToList();
        }

        private Tuple<int, int> ParseCoordinate(string line) {
            int x = 0;
            int y = 0;
            line = line.Replace("x=", "").Replace("y=", "").Replace("z=", "").Replace("..", ",");
            var split = line.Split(',');
            return new Tuple<int, int>(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]));
        }

        private class Cuboid {
            public Point Start { get; set; }
            public Point End { get; set; }
            public bool On { get; set; }
            public bool HadLowestStartX { get; set; }
            public bool HadLowestStartY { get; set; }
            public bool HadLowestEndX { get; set; }
            public bool HadLowestEndY { get; set; }
            public Cuboid SplitParent { get; set; }
        }

        private class Point {
            public Point() { }
            public Point(long x, long y, long z) {
                X = x;
                Y = y;
                Z = z;
            }

            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }
        }
    }
}
