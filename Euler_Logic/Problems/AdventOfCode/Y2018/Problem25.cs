using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem25 : AdventOfCodeBase {
        private List<Point> _points;
        private int _nextGroup;

        public override string ProblemName {
            get { return "Advent of Code 2018: 25"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetPoints(input);
            BuildGroups();
            return _nextGroup;
        }

        private void BuildGroups() {
            foreach (var point in _points) {
                if (point.Group == 0) {
                    _nextGroup++;
                    point.Group = _nextGroup;
                    AddToGroup(point, _nextGroup);
                }
            }
        }

        private void AddToGroup(Point point, int group) {
            foreach (var next in _points) {
                if (next != point && next.Group == 0) {
                    var distance =
                        Math.Abs(point.A - next.A)
                        + Math.Abs(point.B - next.B)
                        + Math.Abs(point.C - next.C)
                        + Math.Abs(point.D - next.D);
                    if (distance <= 3) {
                        next.Group = group;
                        AddToGroup(next, group);
                    }
                }
            }
        }

        private void GetPoints(List<string> input) {
            _points = input.Select(line => {
                var split = line.Split(',');
                return new Point() {
                    A = Convert.ToInt32(split[0]),
                    B = Convert.ToInt32(split[1]),
                    C = Convert.ToInt32(split[2]),
                    D = Convert.ToInt32(split[3]),
                };
            }).ToList();
        }

        private class Point {
            public int A { get; set; }
            public int B { get; set; }
            public int C { get; set; }
            public int D { get; set; }
            public int Group { get; set; }
        }
    }
}
