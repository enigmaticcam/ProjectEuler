using System;
using System.Collections.Generic;
using System.Linq;
namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem05 : AdventOfCodeBase {
        private List<Line> _lines;

        public override string ProblemName {
            get { return "Advent of Code 2021: 05"; }
        }

        public override string GetAnswer() {
            GetLines(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            var counts = GetCounts(_lines.Where(x => x.X1 == x.X2 || x.Y1 == x.Y2));
            return FindMultiplePoints(counts);
        }

        private int Answer2() {
            var counts = GetCounts(_lines);
            return FindMultiplePoints(counts);
        }

        private int FindMultiplePoints(Dictionary<int, Dictionary<int, int>> counts) {
            int count = 0;
            foreach (var x in counts.Keys) {
                foreach (var y in counts[x].Keys) {
                    if (counts[x][y] >= 2) {
                        count++;
                    }
                }
            }
            return count;
        }
        
        private Dictionary<int, Dictionary<int, int>> GetCounts(IEnumerable<Line> lines) {
            var counts = new Dictionary<int, Dictionary<int, int>>();
            foreach (var line in lines) {
                var direction = GetDirection(line);
                var x = line.X1;
                var y = line.Y1;
                do {
                    AddToCounts(counts, x, y);
                    x += direction.Item1;
                    y += direction.Item2;
                } while (x != line.X2 || y != line.Y2);
                AddToCounts(counts, x, y);
            }
            return counts;
        }

        private Tuple<int, int> GetDirection(Line line) {
            int directionX = 1;
            int directionY = 1;
            if (line.X1 == line.X2) {
                directionX = 0;
            } else if (line.X1 > line.X2) {
                directionX = -1;
            }
            if (line.Y1 == line.Y2) {
                directionY = 0;
            } else if (line.Y1 > line.Y2) {
                directionY = -1;
            }
            return new Tuple<int, int>(directionX, directionY);
        }

        private void AddToCounts(Dictionary<int, Dictionary<int, int>> counts, int x, int y) {
            if (!counts.ContainsKey(x)) {
                counts.Add(x, new Dictionary<int, int>());
            }
            if (!counts[x].ContainsKey(y)) {
                counts[x].Add(y, 1);
            } else {
                counts[x][y]++;
            }
        }

        private void GetLines(List<string> input) {
            _lines = input.Select(x => {
                var split = x.Replace(" -> ", ",").Split(',');
                return new Line() {
                    X1 = Convert.ToInt32(split[0]),
                    X2 = Convert.ToInt32(split[2]),
                    Y1 = Convert.ToInt32(split[1]),
                    Y2 = Convert.ToInt32(split[3])
                };
            }).ToList();
        }

        private List<string> Input_Test1() {
            return new List<string>() {
                "0,9 -> 5,9",
                "8,0 -> 0,8",
                "9,4 -> 3,4",
                "2,2 -> 2,1",
                "7,0 -> 7,4",
                "6,4 -> 2,0",
                "0,9 -> 2,9",
                "3,4 -> 1,4",
                "0,0 -> 8,8",
                "5,5 -> 8,2"
            };
        }

        private class Line {
            public int X1 { get; set; }
            public int X2 { get; set; }
            public int Y1 { get; set; }
            public int Y2 { get; set; }
        }
    }
}
