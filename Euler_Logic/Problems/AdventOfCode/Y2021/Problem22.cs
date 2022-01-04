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
            GetCuboids(Input_Test(2));
            return Answer2().ToString();
        }

        private long Answer2() {
            var result = FindOverlaps();
            return Count(result);
        }

        private List<Cuboid> FindOverlaps() {
            //var onlyOn = new List<Cuboid>();
            //onlyOn.Add(_cuboids[0]);
            //for (int index = 1; index < _cuboids.Count; index++) {
            //    var current = _cuboids[index];
            //    var toAdd = new List<Cuboid>();
            //    var toRemove = new List<Cuboid>();
            //    bool didSplit = false;
            //    for (int subIndex = 0; subIndex < onlyOn.Count; subIndex++) {
            //        var sub = onlyOn[subIndex];
            //        if (DoesOverlap(current, sub)) {
            //            didSplit = true;
            //            if (OverlapAll(current, sub)) {
            //                toRemove.Add(sub);
            //                if (current.On) toAdd.Add(current);
            //            } else if (OverlapAll(sub, current)) {
            //                if (!current.On) {
            //                    toAdd.AddRange(Split(sub, current));
            //                    toRemove.Add(sub);
            //                }
            //            } else {
            //                var split = Split(sub, current);
            //                toRemove.Add(sub);
            //                toAdd.AddRange(split.Where(x => x.On));
            //            }
            //        }
            //    }
            //    // if we did split it, we need to immediately test split cuboids against prior cuboids to maintain on/off sequence order
            //    if (!didSplit) {
            //        onlyOn.Add(current);
            //    } else {
            //        toRemove.ForEach(x => onlyOn.Remove(x));
            //        //onlyOn.AddRange(toAdd);
            //        _cuboids.AddRange(toAdd);
            //    }
            //}
            //return onlyOn;
            var onlyOn = new List<Cuboid>();
            onlyOn.Add(_cuboids[0]);
            for (int index = 1; index < _cuboids.Count; index++) {
                var current = _cuboids[index];
                if (index == 5) {
                    bool stop = true;
                }
                Converge(onlyOn, current);
                onlyOn.AddRange(GetAllChildren(current).Where(x => x.On));
                for (int sub = 0; sub < onlyOn.Count; sub++) {
                    if (onlyOn[sub].Children != null) {
                        onlyOn.RemoveAt(sub);
                        sub--;
                    }
                }
            }
            return onlyOn;
        }

        private void Converge(List<Cuboid> onlyOn, Cuboid current) {
            int count = 0;
            foreach (var prior in onlyOn) {
                var children = GetAllChildren(current).ToList();
                foreach (var child in children) {
                    if (count == 1087) {
                        bool stop = true;
                    }
                    if (DoesOverlap(child, prior)) {
                        if (OverlapAll(child, prior)) {
                            prior.Children = new List<Cuboid>();
                            if (child.On) prior.Children.Add(child);
                        } else if (OverlapAll(prior, child)) {
                            if (!child.On) {
                                prior.Children = new List<Cuboid>();
                                child.Children = Split(prior, child).ToList();
                                if (!Validate(prior, child, child.Children)) {
                                    bool stop = true;
                                }
                            }
                        } else {
                            prior.Children = new List<Cuboid>();
                            child.Children = Split(prior, child).ToList();
                            if (!Validate(prior, child, child.Children)) {
                                bool stop = true;
                            }
                        }
                    }
                    count++;
                }
            }
        }

        private IEnumerable<Cuboid> GetAllChildren(Cuboid cuboid) {
            if (cuboid.Children == null) {
                yield return cuboid;
            } else {
                foreach (var next in cuboid.Children) {
                    var children = GetAllChildren(next);
                    foreach (var child in children) {
                        yield return child;
                    }
                }
            }
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
                    On = _lower.On
                };
            }
            if (cuboid1.End.X != cuboid2.End.X) {
                SetLowerHigher(cuboid1, cuboid2, x => x.End.X);
                _lower.HadLowestEndX = true;
                yield return new Cuboid() {
                    Start = new Point(_lower.End.X + 1, _higher.Start.Y, _higher.Start.Z),
                    End = new Point(_higher.End.X, _higher.End.Y, _higher.End.Z),
                    On = _higher.On
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
                    On = _lower.On
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
                    On = _higher.On
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
                    On = _lower.On
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
                    On = _higher.On
                };
            }
            yield return new Cuboid() {
                Start = new Point(Math.Max(cuboid1.Start.X, cuboid2.Start.X), Math.Max(cuboid1.Start.Y, cuboid2.Start.Y), Math.Max(cuboid1.Start.Z, cuboid2.Start.Z)),
                End = new Point(Math.Min(cuboid1.End.X, cuboid2.End.X), Math.Min(cuboid1.End.Y, cuboid2.End.Y), Math.Min(cuboid1.End.Z, cuboid2.End.Z)),
                On = cuboid2.On
            };
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

        private bool Validate(Cuboid cuboid1, Cuboid cuboid2, IEnumerable<Cuboid> result) {
            var hash = new Dictionary<Tuple<long, long, long>, bool>();
            for (long x = cuboid1.Start.X; x <= cuboid1.End.X; x++) {
                for (long y = cuboid1.Start.Y; y <= cuboid1.End.Y; y++) {
                    for (long z = cuboid1.Start.Z; z <= cuboid1.End.Z; z++) {
                        var key = new Tuple<long, long, long>(x, y, z);
                        hash.Add(key, cuboid1.On);
                    }
                }
            }
            for (long x = cuboid2.Start.X; x <= cuboid2.End.X; x++) {
                for (long y = cuboid2.Start.Y; y <= cuboid2.End.Y; y++) {
                    for (long z = cuboid2.Start.Z; z <= cuboid2.End.Z; z++) {
                        var key = new Tuple<long, long, long>(x, y, z);
                        if (hash.ContainsKey(key)) {
                            hash[key] = cuboid2.On;
                        } else {
                            hash.Add(key, cuboid2.On);
                        }
                    }
                }
            }
            long sum = 0;
            foreach (var cuboid in result) {
                sum += Math.Abs(cuboid.End.X - cuboid.Start.X + 1) * Math.Abs(cuboid.End.Y - cuboid.Start.Y + 1) * Math.Abs(cuboid.End.Z - cuboid.Start.Z + 1);
            }
            return hash.Select(x => x.Value).Count() == sum;
        }

        private void Tests() {
            // Test TurnOffMiddle
            var large = new Cuboid() {
                Start = new Point(0, 0, 10),
                End = new Point(5, 5, 12),
                On = true
            };
            var small = new Cuboid() {
                Start = new Point(2, 2, 11),
                End = new Point(3, 3, 11)
            };
            var total = Count(large);
            var result = Split(large, small).ToList();
            var count = Count(result);
            if (count != 104) throw new Exception();
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
            if (cuboid.Children != null || !cuboid.On) {
                return 0;
            }
            return Math.Abs(cuboid.End.X - cuboid.Start.X + 1) * Math.Abs(cuboid.End.Y - cuboid.Start.Y + 1) * Math.Abs(cuboid.End.Z - cuboid.Start.Z + 1);
        }

        private int Answer1() {
            var grid = new Dictionary<long, Dictionary<long, Dictionary<long, bool>>>();
            foreach (var cuboid in _cuboids) {
                long startX = Math.Max(cuboid.Start.X, -50);
                long endX = Math.Min(cuboid.End.X, 50);
                for (long x = startX; x <= endX; x++) {
                    if (!grid.ContainsKey(x)) {
                        grid.Add(x, new Dictionary<long, Dictionary<long, bool>>());
                    }
                    long startY = Math.Max(cuboid.Start.Y, -50);
                    long endY = Math.Min(cuboid.End.Y, 50);
                    for (long y = startY; y <= endY; y++) {
                        if (!grid[x].ContainsKey(y)) {
                            grid[x].Add(y, new Dictionary<long, bool>());
                        }
                        long startZ = Math.Max(cuboid.Start.Z, -50);
                        long endZ = Math.Min(cuboid.End.Z, 50);
                        for (long z = startZ; z <= endZ; z++) {
                            if (!grid[x][y].ContainsKey(z)) {
                                grid[x][y].Add(z, cuboid.On);
                            } else {
                                grid[x][y][z] = cuboid.On;
                            }
                        }
                    }
                }
            }
            return Count(grid);
        }

        private int Count(Dictionary<long, Dictionary<long, Dictionary<long, bool>>> grid) {
            int count = 0;
            foreach (var x in grid) {
                foreach (var y in x.Value) {
                    foreach (var z in y.Value) {
                        if (z.Value) count++;
                    }
                }
            }
            return count;
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
            public bool IsDone { get; set; }
            public bool HadLowestStartX { get; set; }
            public bool HadLowestStartY { get; set; }
            public bool HadLowestEndX { get; set; }
            public bool HadLowestEndY { get; set; }
            public List<Cuboid> Children { get; set; }
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
