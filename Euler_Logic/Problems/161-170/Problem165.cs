using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem165 : ProblemBase {
        private List<Line> _lines = new List<Line>();
        private Dictionary<long, Dictionary<long, Dictionary<long, HashSet<long>>>> _final = new Dictionary<long, Dictionary<long, Dictionary<long, HashSet<long>>>>();

        /*
            This can be solved with brute force algebra. The problem is that decimal precision isn't good enough, so have
            to handle everything as integer fractions.

            First, generate the lines. Then for each line, solve for (a) and (b) in the equation y = ax + b (ignore this
            step if the line is vertical, where its (x) endpoints are the same). Compare all lines against each
            other and determine the x/y intersect coordinate. Then check if the x/y intersect exists in both lines. This
            can be done by checking if the intersect (x) is between line1 and line2 (x), and if the intersect (y) is between
            line1 and line2 (y).

            Some exceptions. 
            - If both lines are horizontal or vertical, ignore. Either there is no intersection, or it's inifinite intersection
            - If one line is vertical, calculating the intersect is a bit different
            - If either line is horizontal, no need to compare intersect (y) coordinate. Same for (x) coordinate if it's vertical

            For each valid interior intersection, save into dictionary based on its fractions. For anything (-x/-y), convert to 
            (x/y) to ensure uniqueness.
         */

        public override string ProblemName {
            get { return "165: Intersections"; }
        }

        public override string GetAnswer() {
            GetLines(20000);
            CalcAandB();
            Solve();
            return _final.Select(a => a.Value.Select(b => b.Value.Select(c => c.Value.Count).Sum()).Sum()).Sum().ToString();
        }

        private void GetLines(int total) {
            long s = 290797;
            long t = 297;
            long[] random = new long[4];
            for (int count = 0; count < total; count++) {
                s = (s * s) % 50515093;
                t = s % 500;
                random[count % 4] = t;
                if (count % 4 == 3) {
                    _lines.Add(new Line() { x1 = random[0], y1 = random[1], x2 = random[2], y2 = random[3] });
                }
            }
        }

        private void CalcAandB() {
            foreach (var line in _lines) {
                if (line.x1 == line.x2) {
                    line.IsVertical = true;
                } else {
                    line.IsHorizontal = line.y1 == line.y2;
                    line.a = new Fraction() { x = line.y1 - line.y2, y = line.x1 - line.x2 };
                    line.a.Reduce();
                    var fract = new Fraction() { x = line.a.x, y = line.a.y };
                    fract.Mulitply(line.x1, 1);
                    line.b = new Fraction() { x = line.y1, y = 1 };
                    line.b.Subtract(fract.x, fract.y);
                }
            }
        }

        private void Solve() {
            for (int index1 = 0; index1 < _lines.Count; index1++) {
                for (int index2 = index1 + 1; index2 < _lines.Count; index2++) {
                    IsIntersect(_lines[index1], _lines[index2]);
                }
            }
        }

        private Fraction _intersectX = new Fraction();
        private Fraction _intersectY = new Fraction();
        private Fraction _intersectTemp = new Fraction();
        private void IsIntersect(Line line1, Line line2) {
            if (line1.IsHorizontal && line2.IsHorizontal) {
                // no intersect
            } else if (line1.IsVertical && line2.IsVertical) {
                // no intersect
            } else if (line1.IsVertical || line2.IsVertical) {
                var vertical = line1.IsVertical ? line1 : line2;
                var other = line1.IsVertical ? line2 : line1;
                _intersectX.x = vertical.x1;
                _intersectX.y = 1;
                _intersectY.x = other.a.x;
                _intersectY.y = other.a.y;
                _intersectY.Mulitply(vertical.x1, 1);
                _intersectY.Add(other.b.x, other.b.y);
                PerformCheck(line1, line2);
            } else {
                var lcm = LCM.GetLCM(Math.Abs(line1.a.y), Math.Abs(line2.a.y));
                var x = line1.a.x * lcm / line1.a.y;
                var y = line2.a.x * lcm / line2.a.y;
                if (x != y) {
                    _intersectTemp.x = line2.a.x;
                    _intersectTemp.y = line2.a.y;
                    _intersectTemp.Subtract(line1.a.x, line1.a.y);
                    _intersectX.x = line1.b.x;
                    _intersectX.y = line1.b.y;
                    _intersectX.Subtract(line2.b.x, line2.b.y);
                    _intersectX.Divide(_intersectTemp.x, _intersectTemp.y);
                    _intersectY.x = line1.a.x;
                    _intersectY.y = line1.a.y;
                    _intersectY.Mulitply(_intersectX.x, _intersectX.y);
                    _intersectY.Add(line1.b.x, line1.b.y);
                    PerformCheck(line1, line2);
                }
            }
        }

        private bool PerformCheck(Line line1, Line line2) {
            if (_intersectX.x < 0 && _intersectX.y < 0) {
                _intersectX.x *= -1;
                _intersectX.y *= -1;
            }
            if (_intersectY.x < 0 && _intersectY.y < 0) {
                _intersectY.x *= -1;
                _intersectY.y *= -1;
            }
            var lowerX1 = Math.Min(line1.x1, line1.x2);
            var higherX1 = Math.Max(line1.x1, line1.x2);
            var lowerX2 = Math.Min(line2.x1, line2.x2);
            var higherX2 = Math.Max(line2.x1, line2.x2);
            var lowerY1 = Math.Min(line1.y1, line1.y2);
            var higherY1 = Math.Max(line1.y1, line1.y2);
            var lowerY2 = Math.Min(line2.y1, line2.y2);
            var higherY2 = Math.Max(line2.y1, line2.y2);
            bool isGood = true;
            if (!line1.IsVertical) {
                isGood &= Compare(lowerX1, _intersectX) == -1;
                isGood &= Compare(higherX1, _intersectX) == 1;
            }
            if (isGood && !line2.IsVertical) {
                isGood &= Compare(lowerX2, _intersectX) == -1;
                isGood &= Compare(higherX2, _intersectX) == 1;
            }
            if (isGood && !line1.IsHorizontal) {
                isGood &= Compare(lowerY1, _intersectY) == -1;
                isGood &= Compare(higherY1, _intersectY) == 1;
            }
            if (isGood && !line2.IsHorizontal) {
                isGood &= Compare(lowerY2, _intersectY) == -1;
                isGood &= Compare(higherY2, _intersectY) == 1;
            }
            if (isGood) {
                AddIntersect();
            }
            return isGood;
        }

        private int Compare(long num, Fraction fract) {
            if (num != 0) {
                var lcm = LCM.GetLCM(Math.Abs(num), Math.Abs(fract.y));
                var numX = num * lcm;
                var fractX = fract.x * lcm / fract.y;
                if (numX < fractX) {
                    return -1;
                } else if (numX > fractX) {
                    return 1;
                } else {
                    return 0;
                }
            } else if (fract.x == 0 || fract.y == 0) {
                return 0;
            } else if (fract.x < 0 && fract.y < 0) {
                return -1;
            } else if (fract.x < 0 && fract.y > 0) {
                return 1;
            } else if (fract.x > 0 && fract.y < 0) {
                return 1;
            } else {
                return -1;
            }
        }

        private void AddIntersect() {
            if (!_final.ContainsKey(_intersectX.x)) {
                _final.Add(_intersectX.x, new Dictionary<long, Dictionary<long, HashSet<long>>>());
            }
            if (!_final[_intersectX.x].ContainsKey(_intersectX.y)) {
                _final[_intersectX.x].Add(_intersectX.y, new Dictionary<long, HashSet<long>>());
            }
            if (!_final[_intersectX.x][_intersectX.y].ContainsKey(_intersectY.x)) {
                _final[_intersectX.x][_intersectX.y].Add(_intersectY.x, new HashSet<long>());
            }
            _final[_intersectX.x][_intersectX.y][_intersectY.x].Add(_intersectY.y);
        }

        private void GetLinesTest() {
            _lines.Add(new Line() { x1 = 27, y1 = 44, x2 = 12, y2 = 32 });
            _lines.Add(new Line() { x1 = 46, y1 = 53, x2 = 17, y2 = 62 });
            _lines.Add(new Line() { x1 = 46, y1 = 70, x2 = 22, y2 = 40 });
        }

        private class Line {
            public long x1 { get; set; }
            public long y1 { get; set; }
            public long x2 { get; set; }
            public long y2 { get; set; }
            public Fraction a { get; set; }
            public Fraction b { get; set; }
            public bool IsHorizontal { get; set; }
            public bool IsVertical { get; set; }
        }

        private class Fraction {
            public long x { get; set; }
            public long y { get; set; }

            public void Add(long mx, long my) {
                var lcm = LCM.GetLCM(Math.Abs(my), Math.Abs(y));
                x = (x * lcm / y) + (mx * lcm / my);
                y = lcm;
                Reduce();
            }

            public void Divide(long mx, long my) {
                x *= my;
                y *= mx;
                Reduce();
            }

            public void Mulitply(long mx, long my) {
                x *= mx;
                y *= my;
                Reduce();
            }

            public void Reduce() {
                var gcd = GCD.GetGCD(Math.Abs(x), Math.Abs(y));
                x /= gcd;
                y /= gcd;
            }

            public void Subtract(long mx, long my) {
                var lcm = LCM.GetLCM(Math.Abs(my), Math.Abs(y));
                x = (x * lcm / y) - (mx * lcm / my);
                y = lcm;
                Reduce();
            }


        }
    }
}
