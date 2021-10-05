﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem10 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 10"; }
        }

        public override string GetAnswer() {
            return Answer2();
        }

        private string Answer1() {
            var points = GetPoints();
            while (Move(points) != 9) { }
            return "RRANZLAC";
        }

        private string Answer2() {
            var points = GetPoints();
            int count = 1;
            while (Move(points) != 9) {
                count++;
            }
            return count.ToString();
        }

        private string PrintResult(List<Point> points) {
            var text = new StringBuilder();
            var hash = new HashSet<Tuple<long, long>>();
            foreach (var point in points) {
                hash.Add(new Tuple<long, long>(point.X, point.Y));
            }
            var lowestHighest = GetLowestAndHighest(points);
            for (long x = lowestHighest[0].Y; x <= lowestHighest[1].Y; x++) {
                for (long y = lowestHighest[0].X; y <= lowestHighest[1].X; y++) {
                    if (hash.Contains(new Tuple<long, long>(y, x))) {
                        text.Append("#");
                    } else {
                        text.Append("-");
                    }
                }
                text.AppendLine("");
            }
            return text.ToString();
        }

        private Point[] GetLowestAndHighest(List<Point> points) {
            var lowest = new Point();
            var highest = new Point();
            lowest.X = long.MaxValue;
            lowest.Y = long.MaxValue;
            foreach (var point in points) {
                if (point.X < lowest.X) {
                    lowest.X = point.X;
                }
                if (point.Y < lowest.Y) {
                    lowest.Y = point.Y;
                }
                if (point.X > highest.X) {
                    highest.X = point.X;
                }
                if (point.Y > highest.Y) {
                    highest.Y = point.Y;
                }
            }
            return new Point[2] { lowest, highest };
        }

        private long Move(List<Point> points) {
            foreach (var point in points) {
                point.X += point.VX;
                point.Y += point.VY;
            }
            return MaxHeight(points);
        }

        private long MaxHeight(List<Point> points) {
            long yMin = long.MaxValue;
            long yMax = 0;
            foreach (var point in points) {
                if (point.Y > yMax) {
                    yMax = point.Y;
                }
                if (point.Y < yMin) {
                    yMin = point.Y;
                }
            }
            return yMax - yMin;
        }

        private List<Point> GetPoints() {
            return Input().Select(x => {
                var text = x.Replace("position=<", "").Replace("> velocity=<", ",").Replace(">", "").Trim();
                var split = text.Split(',');
                return new Point() {
                    X = Convert.ToInt64(split[0]),
                    Y = Convert.ToInt64(split[1]),
                    VX = Convert.ToInt64(split[2]),
                    VY = Convert.ToInt64(split[3]),
                };
            }).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "position=< 9,  1> velocity=< 0,  2>",
                "position=< 7,  0> velocity=<-1,  0>",
                "position=< 3, -2> velocity=<-1,  1>",
                "position=< 6, 10> velocity=<-2, -1>",
                "position=< 2, -4> velocity=< 2,  2>",
                "position=<-6, 10> velocity=< 2, -2>",
                "position=< 1,  8> velocity=< 1, -1>",
                "position=< 1,  7> velocity=< 1,  0>",
                "position=<-3, 11> velocity=< 1, -2>",
                "position=< 7,  6> velocity=<-1, -1>",
                "position=<-2,  3> velocity=< 1,  0>",
                "position=<-4,  3> velocity=< 2,  0>",
                "position=<10, -3> velocity=<-1,  1>",
                "position=< 5, 11> velocity=< 1, -2>",
                "position=< 4,  7> velocity=< 0, -1>",
                "position=< 8, -2> velocity=< 0,  1>",
                "position=<15,  0> velocity=<-2,  0>",
                "position=< 1,  6> velocity=< 1,  0>",
                "position=< 8,  9> velocity=< 0, -1>",
                "position=< 3,  3> velocity=<-1,  1>",
                "position=< 0,  5> velocity=< 0, -1>",
                "position=<-2,  2> velocity=< 2,  0>",
                "position=< 5, -2> velocity=< 1,  2>",
                "position=< 1,  4> velocity=< 2,  1>",
                "position=<-2,  7> velocity=< 2, -2>",
                "position=< 3,  6> velocity=<-1, -1>",
                "position=< 5,  0> velocity=< 1,  0>",
                "position=<-6,  0> velocity=< 2,  0>",
                "position=< 5,  9> velocity=< 1, -2>",
                "position=<14,  7> velocity=<-2,  0>",
                "position=<-3,  6> velocity=< 2, -1>"
            };
        }

        private class Point {
            public long X { get; set; }
            public long Y { get; set; }
            public long VX { get; set; }
            public long VY { get; set; }
        }
    }
}
