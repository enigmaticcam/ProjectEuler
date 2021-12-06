using System;

namespace Euler_Logic.Problems {
    public class Problem144 : ProblemBase {
        /*
            This was a difficult problem for me because I know nothing about linear algebra. This page
            http://paulbourke.net/geometry/reflected/ described how to calculate a reflected ray. Dot
            product between two point vectors is simply point1.X * point2.X + point1.Y * point2.Y.
            But even knowing that, there were still a couple things, such as vector normalization
            and determing the direction of of your current point. I had to borrow some code from someone
            else who solved it. But that was just for the line reflection; I figured everything else.

            Once I knew how to calculate the slope of the reflected line, it was just a matter of
            basic algrebra to determine where the reflected line intersects the ellipse. The final
            coordinates derive from a quadratic equation. Since a QE can have two possible values,
            it's the value that does not equal the point we already know. Loop until we find it.
        */

        public override string ProblemName {
            get { return "144: Investigating multiple reflections of a laser beam"; }
        }

        public override string GetAnswer() {
            return Solve1().ToString();
        }

        private ulong Solve1() {
            ulong count = 0;
            var start = new Point(0, (decimal)10.1);
            var end = new Point((decimal)1.4, (decimal)-9.6);
            var normal = new Point();
            var direction = new Point();
            var reflect = new Point();
            do {
                if (end.X >= (decimal)-0.01 && end.X <= (decimal)0.01 && end.Y > 0 && count > 0) {
                    return count;
                }

                // Reflect
                normal.X = -4 * end.X;
                normal.Y = -end.Y;
                normal.Normalize();
                direction.X = end.X - start.X;
                direction.Y = end.Y - start.Y;
                var dot = direction.X * normal.X + direction.Y * normal.Y;
                reflect.X = direction.X - 2 * dot * normal.X;
                reflect.Y = direction.Y - 2 * dot * normal.Y;

                // Ellipse intersect
                var s = (start.Y - end.Y) / (start.X - end.X);
                var m = -4 * end.X / end.Y;
                var p = reflect.Y / reflect.X;
                var k = end.Y - p * end.X;
                var a = 4 + p * p;
                var b = 2 * k * p;
                var c = k * k - 100;

                // Determine which values from quadratic are correct
                start.X = end.X;
                start.Y = end.Y;
                var root = b * b - 4 * a * c;
                var newX1 = (-b - (decimal)Math.Sqrt((double)root)) / (2 * a);
                var newY1 = p * newX1 + k;
                var newX2 = (-b + (decimal)Math.Sqrt((double)root)) / (2 * a);
                var newY2 = p * newX2 + k;
                if (Math.Abs(newX1 - end.X) > Math.Abs(newX2 - end.X)) {
                    end.X = newX1;
                    end.Y = newY1;
                } else {
                    end.X = newX2;
                    end.Y = newY2;
                }
                count++;
            } while (true);
        }

        private class Point {
            public Point() { }
            public Point(decimal x, decimal y) {
                X = x;
                Y = y;
            }

            public decimal X { get; set; }
            public decimal Y { get; set; }

            public void Normalize() {
                var squaredX = X * X;
                var squaredY = Y * Y;
                decimal length = (decimal)Math.Sqrt((double)squaredX + (double)squaredY);
                X /= length;
                Y /= length;
            }
        }
    }
}
