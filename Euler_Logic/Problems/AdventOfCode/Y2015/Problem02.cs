using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem02 : AdventOfCodeBase {
        private List<Tuple<int, int, int>> _boxes;

        public override string ProblemName {
            get { return "Advent of Code 2015: 2"; }
        }

        public override string GetAnswer() {
            GetBoxes(Input());
            return Answer1().ToString();
        }

        public override string GetAnswer2() {
            GetBoxes(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            var sum = 0;
            foreach (var box in _boxes) {
                sum += GetArea(box);
                    
            }
            return sum;
        }

        private int Answer2() {
            var sum = 0;
            _boxes.ForEach(x => sum += GetRibbon(x));
            return sum;
        }

        private int GetRibbon(Tuple<int, int, int> box) {
            int par = Math.Min(Math.Min(box.Item1 + box.Item2, box.Item2 + box.Item3), box.Item3 + box.Item1) * 2;
            return par + box.Item1 * box.Item2 * box.Item3;
        }

        private int GetArea(Tuple<int, int, int> box) {
            var side1 = box.Item1 * box.Item2;
            var side2 = box.Item2 * box.Item3;
            var side3 = box.Item3 * box.Item1;
            return side1 * 2 + side2 * 2 + side3 * 2 + Math.Min(Math.Min(side1, side2), side3);
        }

        private void GetBoxes(List<string> input) {
            _boxes = input.Select(x => {
                var split = x.Split('x');
                return new Tuple<int, int, int>(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2]));
            }).ToList();
        }
    }
}
