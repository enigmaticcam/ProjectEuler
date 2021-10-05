using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem03 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 3"; }
        }

        public override string GetAnswer() {
            return Answer2();
        }

        private string Answer1() {
            var input = Claims();
            var overlaps = new List<Claim>();
            for (int index1 = 0; index1 < input.Count; index1++) {
                var claim1 = input[index1];
                for (int index2 = index1 + 1; index2 < input.Count; index2++) {
                    var claim2 = input[index2];
                    if (IsOverlapped(claim1, claim2)) {
                        var overlap = GetOverlap(claim1, claim2);
                        overlaps.Add(overlap);
                    }
                }
            }
            return CountOverlaps(overlaps).ToString();
        }

        private string Answer2() {
            var input = Claims();
            foreach (var claim1 in input) {
                bool isGood = true;
                foreach (var claim2 in input) {
                    if (claim1.Id != claim2.Id && IsOverlapped(claim1, claim2)) {
                        isGood = false;
                        break;
                    }
                }
                if (isGood) {
                    return claim1.Id;
                }
            }
            return "";
        }

        private bool IsOverlapped(Claim claim1, Claim claim2) {
            return claim1.Start.X < claim2.End.X && claim1.End.X > claim2.Start.X
                && claim1.Start.Y < claim2.End.Y && claim1.End.Y > claim2.Start.Y;
        }

        private Claim GetOverlap(Claim claim1, Claim claim2) {
            var overlap = new Claim();
            overlap.Start = new Point(Math.Max(claim1.Start.X, claim2.Start.X), Math.Max(claim1.Start.Y, claim2.Start.Y));
            overlap.End = new Point(Math.Min(claim1.End.X, claim2.End.X), Math.Min(claim1.End.Y, claim2.End.Y));
            return overlap;
        }

        private int CountOverlaps(List<Claim> overlaps) {
            var hash = new HashSet<Tuple<int, int>>();
            foreach (var overlap in overlaps) {
                for (int x = overlap.Start.X; x < overlap.End.X; x++) {
                    for (int y = overlap.Start.Y; y < overlap.End.Y; y++) {
                        hash.Add(new Tuple<int, int>(x, y));
                    }
                }
            }
            return hash.Count;
        }

        private List<Claim> Claims() {
            var claims = new List<Claim>();
            Input().ForEach(x => {
                var parts = x.Split(' ');
                var claim = new Claim();
                claim.Id = parts[0];
                var xy = parts[2].Split(',');
                claim.Start = new Point(Convert.ToInt32(xy[0]), Convert.ToInt32(xy[1].Replace(":", "")));
                var wh = parts[3].Split('x');
                claim.End = new Point(Convert.ToInt32(wh[0]) + claim.Start.X, Convert.ToInt32(wh[1]) + claim.Start.Y);
                claims.Add(claim);
            });
            return claims;
        }

        private List<string> TestInput() {
            return new List<string>() {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2"
            };
        }

        private class Claim {
            public string Id { get; set; }
            public Point Start { get; set; }
            public Point End { get; set; }
        }

        private class Point {
            public Point() { }
            public Point(int x, int y) {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
