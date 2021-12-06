using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem23 : AdventOfCodeBase {
        private List<Bot> _bots;
        private Point _min;
        private Point _max;
        private List<Tuple<Point, Point, int>> _best;

        public override string ProblemName {
            get { return "Advent of Code 2018: 23"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetBots(input);
            return InRange();
        }

        private long Answer2(List<string> input) {
            GetBots(input);
            SetMinMax();
            FindBestPoint();
            return GetShortest();
        }

        private void FindBestPoint() {
            var start = new Point() { X = _min.X, Y = _min.Y, Z = _min.Z };
            var end = new Point() { X = _max.X, Y = _max.Y, Z = _max.Z };
            _best = new List<Tuple<Point, Point, int>>() { new Tuple<Point, Point, int>(start, end, 0) };
            var tempBest = new List<Tuple<Point, Point, int>>();
            do {
                tempBest = _best;
                _best = new List<Tuple<Point, Point, int>>();
                int bestCount = tempBest.Select(x => x.Item3).Max();
                foreach (var next in tempBest.Where(x => x.Item3 == bestCount)) {
                    FindBestOctant(next.Item1, next.Item2);
                }
            } while (_best.First().Item1.X != _best.First().Item2.X || _best.First().Item1.Y != _best.First().Item2.Y || _best.First().Item1.Z != _best.First().Item2.Z);
        }

        private long GetShortest() {
            long best = int.MaxValue;
            foreach (var next in _best) {
                var distance = next.Item1.X + next.Item1.Y + next.Item1.Z;
                if (distance < best) {
                    best = distance;
                }
            }
            return best;
        }

        private int FindBestOctant(Point start, Point end) {
            var toAdd = new List<Tuple<Point, Point, int>>();
            var len = new Point() {
                X = Math.Abs(start.X - end.X) / 2,
                Y = Math.Abs(start.Y - end.Y) / 2,
                Z = Math.Abs(start.Z - end.Z) / 2
            };
            var adjustedStart = new Point() {
                X = Math.Min(start.X, end.X),
                Y = Math.Min(start.Y, end.Y),
                Z = Math.Min(start.Z, end.Z)
            };
            var adjustedEnd = new Point() {
                X = Math.Max(start.X, end.X),
                Y = Math.Max(start.Y, end.Y),
                Z = Math.Max(start.Z, end.Z)
            };
            int bestCount = 0;

            // Oct 1
            var currentStart = new Point() { X = adjustedStart.X, Y = adjustedStart.Y, Z = adjustedStart.Z };
            var currentEnd = new Point() { X = adjustedStart.X + len.X, Y = adjustedStart.Y + len.Y, Z = adjustedStart.Z + len.Z };
            var count = CountInRange(currentStart, currentEnd);
            if (count > bestCount) {
                bestCount = count;
                toAdd.Clear();
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            } else if (count == bestCount) {
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            }

            // Oct 2
            currentStart.X = adjustedStart.X + len.X + 1;
            currentStart.Y = adjustedStart.Y;
            currentStart.Z = adjustedStart.Z;
            currentEnd.X = adjustedEnd.X;
            currentEnd.Y = adjustedStart.Y + len.Y;
            currentEnd.Z = adjustedStart.Z + len.Z;
            count = CountInRange(currentStart, currentEnd);
            if (count > bestCount) {
                bestCount = count;
                toAdd.Clear();
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            } else if (count == bestCount) {
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            }

            // Oct 3
            currentStart.X = adjustedStart.X + len.X + 1;
            currentStart.Y = adjustedStart.Y + len.Y + 1;
            currentStart.Z = adjustedStart.Z;
            currentEnd.X = adjustedEnd.X;
            currentEnd.Y = adjustedEnd.Y;
            currentEnd.Z = adjustedStart.Z + len.Z;
            count = CountInRange(currentStart, currentEnd);
            if (count > bestCount) {
                bestCount = count;
                toAdd.Clear();
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            } else if (count == bestCount) {
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            }

            // Oct 4
            currentStart.X = adjustedStart.X + len.X + 1;
            currentStart.Y = adjustedStart.Y;
            currentStart.Z = adjustedStart.Z + len.Z + 1;
            currentEnd.X = adjustedEnd.X;
            currentEnd.Y = adjustedStart.Y + len.Y;
            currentEnd.Z = adjustedEnd.Z;
            count = CountInRange(currentStart, currentEnd);
            if (count > bestCount) {
                bestCount = count;
                toAdd.Clear();
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            } else if (count == bestCount) {
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            }

            // Oct 5
            currentStart.X = adjustedStart.X + len.X + 1;
            currentStart.Y = adjustedStart.Y + len.Y + 1;
            currentStart.Z = adjustedStart.Z + len.Z + 1;
            currentEnd.X = adjustedEnd.X;
            currentEnd.Y = adjustedEnd.Y;
            currentEnd.Z = adjustedEnd.Z;
            count = CountInRange(currentStart, currentEnd);
            if (count > bestCount) {
                bestCount = count;
                toAdd.Clear();
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            } else if (count == bestCount) {
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            }

            // Oct 6
            currentStart.X = adjustedStart.X;
            currentStart.Y = adjustedStart.Y + len.Y + 1;
            currentStart.Z = adjustedStart.Z;
            currentEnd.X = adjustedStart.X + len.X;
            currentEnd.Y = adjustedEnd.Y;
            currentEnd.Z = adjustedStart.Z + len.Z;
            count = CountInRange(currentStart, currentEnd);
            if (count > bestCount) {
                bestCount = count;
                toAdd.Clear();
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            } else if (count == bestCount) {
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            }

            // Oct 7
            currentStart.X = adjustedStart.X;
            currentStart.Y = adjustedStart.Y + len.Y + 1;
            currentStart.Z = adjustedStart.Z + len.Z + 1;
            currentEnd.X = adjustedStart.X + len.X;
            currentEnd.Y = adjustedEnd.Y;
            currentEnd.Z = adjustedEnd.Z;
            count = CountInRange(currentStart, currentEnd);
            if (count > bestCount) {
                bestCount = count;
                toAdd.Clear();
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            } else if (count == bestCount) {
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            }

            // Oct 8
            currentStart.X = adjustedStart.X;
            currentStart.Y = adjustedStart.Y;
            currentStart.Z = adjustedStart.Z + len.Z + 1;
            currentEnd.X = adjustedStart.X + len.X;
            currentEnd.Y = adjustedStart.Y + len.Y;
            currentEnd.Z = adjustedEnd.Z;
            count = CountInRange(currentStart, currentEnd);
            if (count > bestCount) {
                bestCount = count;
                toAdd.Clear();
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            } else if (count == bestCount) {
                toAdd.Add(new Tuple<Point, Point, int>(currentStart.Copy(), currentEnd.Copy(), bestCount));
            }
            _best.AddRange(toAdd);
            return bestCount;
        }

        private void SetMinMax() {
            _min = new Point() { X = long.MaxValue, Y = long.MaxValue, Z = long.MaxValue };
            _max = new Point();
            foreach (var bot in _bots) {
                var minX = bot.P.X - bot.Radius;
                var minY = bot.P.Y - bot.Radius;
                var minZ = bot.P.Z - bot.Radius;
                var maxX = bot.P.X + bot.Radius;
                var maxY = bot.P.Y + bot.Radius;
                var maxZ = bot.P.Z + bot.Radius;
                if (minX < _min.X) _min.X = minX;
                if (minY < _min.Y) _min.Y = minY;
                if (minZ < _min.Z) _min.Z = minZ;
                if (maxX > _max.X) _max.X = maxX;
                if (maxY > _max.Y) _max.Y = maxY;
                if (maxZ > _max.Z) _max.Z = maxZ;
            }
        }

        private int CountInRange(Point start, Point end) {
            int count = 0;
            var minX = Math.Min(start.X, end.X);
            var minY = Math.Min(start.Y, end.Y);
            var minZ = Math.Min(start.Z, end.Z);
            var maxX = Math.Max(start.X, end.X);
            var maxY = Math.Max(start.Y, end.Y);
            var maxZ = Math.Max(start.Z, end.Z);
            foreach (var bot in _bots) {
                var x = bot.P.X;
                x = Math.Max(minX, x);
                x = Math.Min(maxX, x);
                var y = bot.P.Y;
                y = Math.Max(minY, y);
                y = Math.Min(maxY, y);
                var z = bot.P.Z;
                z = Math.Max(minZ, z);
                z = Math.Min(maxZ, z);
                if (Distance(bot.P, x, y, z) <= bot.Radius) {
                    count++;
                }
            }
            return count;
        }

        private long Distance(Point point, long x, long y, long z) {
            return Distance(point.X, point.Y, point.Z, x, y, z);
        }

        private long Distance(long x1, long y1, long z1, long x2, long y2, long z2) {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2) + Math.Abs(z1 - z2);
        }

        private int InRange() {
            int count = 0;
            var strongest = _bots.OrderByDescending(x => x.Radius).First();
            foreach (var bot in _bots) {
                var distance = Math.Abs(strongest.P.X - bot.P.X) + Math.Abs(strongest.P.Y - bot.P.Y) + Math.Abs(strongest.P.Z - bot.P.Z);
                count += (distance <= strongest.Radius ? 1 : 0);
            }
            return count;
        }

        private void GetBots(List<string> input) {
            _bots = input.Select(line => {
                var split = line.Replace("pos=<", "").Replace(" r=", "").Replace(">", "").Split(',');
                return new Bot() {
                    P = new Point() {
                        X = Convert.ToInt64(split[0]),
                        Y = Convert.ToInt64(split[1]),
                        Z = Convert.ToInt64(split[2])
                    },
                    Radius = Convert.ToInt64(split[3])
                };
            }).ToList();
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "pos=<0,0,0>, r=4",
                "pos=<1,0,0>, r=1",
                "pos=<4,0,0>, r=3",
                "pos=<0,2,0>, r=1",
                "pos=<0,5,0>, r=3",
                "pos=<0,0,3>, r=1",
                "pos=<1,1,1>, r=1",
                "pos=<1,1,2>, r=1",
                "pos=<1,3,1>, r=1"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "pos=<10,12,12>, r=2",
                "pos=<12,14,12>, r=2",
                "pos=<16,12,12>, r=4",
                "pos=<14,14,14>, r=6",
                "pos=<50,50,50>, r=200",
                "pos=<10,10,10>, r=5"
            };
        }

        private class Bot {
            public Point P { get; set; }
            public long Radius { get; set; }
        }

        private class Point {
            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }

            public Point Copy() {
                return new Point() { X = X, Y = Y, Z = Z };
            }
        }
    }
}
