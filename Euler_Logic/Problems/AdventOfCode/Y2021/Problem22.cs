using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem22 : AdventOfCodeBase {
        private List<Cuboid> _cuboids;
        private Dictionary<Cuboid, HashSet<Cuboid>> _overlapHash;

        public override string ProblemName {
            get { return "Advent of Code 2021: 22"; }
        }

        public override string GetAnswer() {
            GetCuboids(Input());
            return Answer2().ToString();
        }

        private long Answer2() {
            _overlapHash = new Dictionary<Cuboid, HashSet<Cuboid>>();
            FindOverlaps();
            return 0;
        }

        private void FindOverlaps() {

        }

        private ulong Recurisve(int currentIndex, Point start, Point end) {
            var cuboid = _cuboids[currentIndex];
            foreach (var overlap in cuboid.Overlaps) {
                if (!_overlapHash.ContainsKey(overlap)) {
                    _overlapHash.Add(overlap, new HashSet<Cuboid>());
                }
                if (!_overlapHash[overlap].Contains(cuboid)) {

                }
            }
            return 0;
        }

        private bool DoesOverlap(Cuboid cuboid1, Cuboid cuboid2) {
            return cuboid1.Start.X <= cuboid2.End.X
                && cuboid1.Start.Y <= cuboid2.End.Y
                && cuboid1.Start.Z <= cuboid2.End.Z
                && cuboid1.End.X >= cuboid2.Start.X
                && cuboid1.End.Y >= cuboid2.Start.Y
                && cuboid1.End.Z >= cuboid2.Start.Z;
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
                var cuboid = new Cuboid() { Overlaps = new List<Cuboid>() };
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
            public List<Cuboid> Overlaps { get; set; }
        }

        private class Point {
            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }
        }
    }
}
