using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem17 : AdventOfCodeBase {
        private Target _target;

        public override string ProblemName {
            get { return "Advent of Code 2021: 17"; }
        }

        public override string GetAnswer() {
            GetTarget(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            int maxYV = (Math.Min(_target.MinY, _target.MaxY) * -1) - 1;
            int sum = (maxYV + 1) * (maxYV / 2);
            if (maxYV % 2 == 1) {
                sum += (maxYV / 2) + 1;
            }
            return sum;
        }

        private int Answer2() {
            int total = 0;
            var minVX = FindMinVX();
            var maxVX = _target.MaxX;
            var minVY = Math.Min(_target.MinY, _target.MaxY);
            var maxVY = (Math.Min(_target.MinY, _target.MaxY) * -1) - 1;
            for (int vx = minVX; vx <= maxVX; vx++) {
                for (int vy = maxVY; vy >= minVY; vy--) {
                    if (DoesOverlap(vx, vy)) total++;
                }
            }
            return total;
        }

        private bool DoesOverlap(int vx, int vy) {
            int x = vx;
            int y = vy;
            do {
                if (vx > 0) {
                    vx--;
                } else if (vx < 0) {
                    vx++;
                }
                vy--;
                if (x >= _target.MinX && x <= _target.MaxX && y >= _target.MinY && y <= _target.MaxY) {
                    return true;
                }
                x += vx;
                y += vy;
            } while (x <= _target.MaxX && y >= _target.MinY);
            return false;
        }

        private int FindMinVX() {
            int min = Math.Min(_target.MinX, _target.MaxX);
            int sum = 0;
            int next = 0;
            do {
                next++;
                sum += next;
                if (sum >= min) {
                    return next;
                }
            } while (true);
        }

        private void GetTarget(List<string> input) {
            var split = input[0].Split(' ');
            var x = split[2].Replace("x=", "").Replace(",", "").Replace("..", ",").Split(',');
            var y = split[3].Replace("y=", "").Replace("..", ",").Split(',');
            _target = new Target() {
                MinX = Convert.ToInt32(x[0]),
                MaxX = Convert.ToInt32(x[1]),
                MinY = Convert.ToInt32(y[0]),
                MaxY = Convert.ToInt32(y[1])
            };
        }

        private class Target {
            public int MinX { get; set; }
            public int MaxX { get; set; }
            public int MinY { get; set; }
            public int MaxY { get; set; }
        }
    }
}
