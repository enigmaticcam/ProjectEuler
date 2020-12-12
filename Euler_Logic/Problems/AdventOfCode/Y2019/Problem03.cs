using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem03 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 3"; }
        }

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        public int Answer1() {
            var wires = GetWires();
            FillPoints(wires);
            var intersections = GetIntersections(wires);
            return FindSmallestDistance(intersections);
        }

        public int Answer2() {
            var wires = GetWires();
            FillPoints(wires);
            var intersections = GetIntersections(wires);
            return FindSmallestRoute(wires, intersections);
        }

        private List<Point> GetIntersections(List<List<Line>> wires) {
            var intersections = new List<Point>();
            foreach (var wire1 in wires[0]) {
                foreach (var wire2 in wires[1]) {
                    if (wire1.Alignment != wire2.Alignment) {
                        var horiz = (wire1.Alignment == enumAlignment.Horizontal ? wire1 : wire2);
                        var vert = (wire1.Alignment == enumAlignment.Vertical ? wire1 : wire2);
                        if (horiz.Points[0].Y >= vert.MinY && horiz.Points[0].Y <= vert.MaxY
                            && vert.Points[0].X >= horiz.MinX && vert.Points[0].X <= horiz.MaxX
                            && (vert.Points[0].X != 0 || horiz.Points[0].Y != 0)) {
                            intersections.Add(new Point(vert.Points[0].X, horiz.Points[0].Y));
                        }
                    }
                }
            }
            return intersections;
        }

        private int FindSmallestDistance(List<Point> intersections) {
            int smallest = int.MaxValue;
            foreach (var intersection in intersections) {
                int distance = Math.Abs(intersection.X) + Math.Abs(intersection.Y);
                if (distance < smallest && distance != 0) {
                    smallest = distance;
                }
            }
            return smallest;
        }

        private int FindSmallestRoute(List<List<Line>> wires, List<Point> intersections) {
            FindSmallestRoute(wires[0], intersections);
            FindSmallestRoute(wires[1], intersections);
            return intersections.Select(x => x.Sum).Min();
        }

        private void FindSmallestRoute(List<Line> wires, List<Point> intersections) {
            int count = 0;
            int x = 0;
            int y = 0;
            foreach (var wire in wires) {
                foreach (var intersection in intersections) {
                    if (wire.Alignment == enumAlignment.Horizontal && intersection.Y >= wire.MinY && intersection.Y <= wire.MaxY) {
                        if (wire.Direction == enumDirection.Left) {
                            intersection.Sum += x - intersection.X + count;
                        } else {
                            intersection.Sum += intersection.X - x + count;
                        }
                    } else if (wire.Alignment == enumAlignment.Vertical && intersection.X >= wire.MinX && intersection.X <= wire.MaxX) {
                        if (wire.Direction == enumDirection.Down) {
                            intersection.Sum += y - intersection.Y + count;
                        } else {
                            intersection.Sum += intersection.Y - y + count;
                        }
                    }
                }
                x = wire.Points[1].X;
                y = wire.Points[1].Y;
                if (wire.Alignment == enumAlignment.Horizontal) {
                    count += Math.Abs(wire.Points[0].X - wire.Points[1].X);
                } else {
                    count += Math.Abs(wire.Points[0].Y - wire.Points[1].Y);
                }
            }
        }

        private void FillPoints(List<List<Line>> wires) {
            foreach (var wire in wires) {
                int x = 0;
                int y = 0;
                foreach (var next in wire) {
                    next.Points = new Point[2];
                    next.Points[0] = new Point(x, y);
                    switch (next.Direction) {
                        case enumDirection.Down:
                            y -= next.Count;
                            break;
                        case enumDirection.Left:
                            x -= next.Count;
                            break;
                        case enumDirection.Right:
                            x += next.Count;
                            break;
                        case enumDirection.Up:
                            y += next.Count;
                            break;
                    }
                    next.Points[1] = new Point(x, y);
                }
            }
        }

        private List<List<Line>> GetWires() {
            var wires = Input();
            return wires.Select(x => {
                var points = x.Split(',');
                var lines = new List<Line>();
                foreach (var point in points) {
                    var line = new Line();
                    switch (point[0]) {
                        case 'L':
                            line.Direction = enumDirection.Left;
                            line.Alignment = enumAlignment.Horizontal;
                            break;
                        case 'R':
                            line.Direction = enumDirection.Right;
                            line.Alignment = enumAlignment.Horizontal;
                            break;
                        case 'U':
                            line.Direction = enumDirection.Up;
                            line.Alignment = enumAlignment.Vertical;
                            break;
                        case 'D':
                            line.Direction = enumDirection.Down;
                            line.Alignment = enumAlignment.Vertical;
                            break;
                    }
                    line.Count = Convert.ToInt32(point.Substring(1, point.Length - 1));
                    lines.Add(line);
                }
                return lines;
            }).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                //"R75,D30,R83,U83,L12,D49,R71,U7,L72",
                //"U62,R66,U55,R34,D71,R55,D58,R83"
                //"R8,U5,L5,D3",
                //"U7,R6,D4,L4"
                "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
                "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"
            };
        }

        public enum enumDirection {
            Up,
            Down,
            Left,
            Right
        }

        public enum enumAlignment {
            Horizontal,
            Vertical
        }

        private class Line {
            public enumDirection Direction { get; set; }
            public enumAlignment Alignment { get; set; }
            public int Count { get; set; }
            public Point[] Points { get; set; }

            public int MinX {
                get { return Math.Min(Points[0].X, Points[1].X); }
            }

            public int MaxX {
                get { return Math.Max(Points[0].X, Points[1].X); }
            }

            public int MinY {
                get { return Math.Min(Points[0].Y, Points[1].Y); }
            }

            public int MaxY {
                get { return Math.Max(Points[0].Y, Points[1].Y); }
            }
        }

        private class Point {
            public Point(int x, int y) {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }
            public int Sum { get; set; }
        }
    }
}

