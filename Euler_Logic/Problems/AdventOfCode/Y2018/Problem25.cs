using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private List<string> Test1Input() {
            return new List<string>() {
                "-1,2,2,0",
                "0,0,2,-2",
                "0,0,0,-2",
                "-1,2,0,0",
                "-2,-2,-2,2",
                "3,0,2,-1",
                "-1,3,2,2",
                "-1,0,-1,0",
                "0,2,1,-2",
                "3,0,0,0"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "1,-1,0,1",
                "2,0,-1,0",
                "3,2,-1,0",
                "0,0,3,1",
                "0,0,-1,-1",
                "2,3,-2,0",
                "-2,2,0,0",
                "2,-2,0,-1",
                "1,-1,0,-1",
                "3,2,0,2"
            };
        }

        private List<string> Test3Input() {
            return new List<string>() {
                "1,-1,-1,-2",
                "-2,-2,0,1",
                "0,2,1,3",
                "-2,3,-2,1",
                "0,2,3,-2",
                "-1,-1,1,-2",
                "0,-2,-1,0",
                "-2,2,3,-1",
                "1,2,2,0",
                "-1,-2,0,-2"
            };
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
