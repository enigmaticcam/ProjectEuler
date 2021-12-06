using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem02 : AdventOfCodeBase {
        private List<Movement> _movements;

        public override string ProblemName {
            get { return "Advent of Code 2021: 02"; }
        }

        public override string GetAnswer() {
            GetMovements(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            int horiz = 0;
            int depth = 0;
            foreach (var movement in _movements) {
                if (movement.Text == "forward") {
                    horiz += movement.Amount;
                } else if (movement.Text == "down") {
                    depth += movement.Amount;
                } else if (movement.Text == "up") {
                    depth -= movement.Amount;
                }
            }
            return horiz * depth;
        }

        private int Answer2() {
            int horiz = 0;
            int depth = 0;
            int aim = 0;
            foreach (var movement in _movements) {
                if (movement.Text == "forward") {
                    horiz += movement.Amount;
                    depth += aim * movement.Amount;
                } else if (movement.Text == "down") {
                    aim += movement.Amount;
                } else if (movement.Text == "up") {
                    aim -= movement.Amount;
                }
            }
            return horiz * depth;
        }

        private void GetMovements(List<string> input) {
            _movements = input.Select(x => {
                var split = x.Split(' ');
                return new Movement() { Amount = Convert.ToInt32(split[1]), Text = split[0] };
            }).ToList();
        }

        private class Movement {
            public string Text { get; set; }
            public int Amount { get; set; }
        }
    }
}
