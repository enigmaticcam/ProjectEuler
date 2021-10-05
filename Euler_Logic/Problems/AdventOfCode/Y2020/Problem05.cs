using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem05 : AdventOfCodeBase {
        // This actually don't return the correct answer for Answer 2, and I have no idea why

        private PowerAll _powerOf2;
        public override string ProblemName => "Advent of Code 2020: 5";

        public override string GetAnswer() {
            _powerOf2 = new PowerAll(2);
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            ulong highest = 0;
            var positions = GetPositions(input);
            foreach (var position in positions) {
                var row = GetPoint(position.Row, 'B', 'F');
                var seat = GetPoint(position.Seat, 'L', 'R');
                var next = row * 8 + seat;
                if (next >= highest) {
                    highest = next;
                }
            }
            return highest;
        }

        private ulong Answer2(List<string> input) {
            var positions = GetPositions(input);
            var filled = new HashSet<ulong>();
            foreach (var position in positions) {
                var row = GetPoint(position.Row, 'B', 'F');
                var seat = GetPoint(position.Seat, 'L', 'R');
                var next = row * 8 + seat;
                if (filled.Contains(next)) {
                    bool stop = true;
                }
                filled.Add(next);
            }
            var ordered = filled.OrderBy(x => x);
            for (int index = 1; index < ordered.Count(); index++) {
                if (ordered.ElementAt(index) != ordered.ElementAt(index - 1) + 1) {
                    return ordered.ElementAt(index) - 1;
                }
            }
            return 0;
        }

        private ulong GetPoint(string text, char bottom, char top) {
            ulong min = 0;
            ulong max = _powerOf2.GetPower(text.Length) - 1;
            var size = (max + 1) / 2;
            foreach (var digit in text) {
                if (digit == bottom) {
                    min += size;
                } else {
                    max -= size;
                }
                size /= 2;
            }
            return min;
        }

        private IEnumerable<Position> GetPositions(List<string> input) {
            return input.Select(line => {
                var pos = new Position();
                pos.Row = line.Substring(0, 7);
                pos.Seat = line.Substring(7, 3);
                return pos;
            });
        }

        private class Position {
            public string Row { get; set; }
            public string Seat { get; set; }
        }
    }
}
