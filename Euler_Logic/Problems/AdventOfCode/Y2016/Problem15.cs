using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem15 : AdventOfCodeBase {
        private List<Disc> _discs;

        public override string ProblemName => "Advent of Code 2016: 15";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetDiscs(input);
            return FindDrop();
        }

        private int Answer2(List<string> input) {
            GetDiscs(input);
            AddNewDisc();
            return FindDrop();
        }

        private void AddNewDisc() {
            _discs.Add(new Disc() { Positions = 11 });
        }

        private int FindDrop() {
            int time = 0;
            bool isGood = false;
            do {
                time++;
                isGood = true;
                for (int index = 0; index < _discs.Count; index++) {
                    var disc = _discs[index];
                    var num = (disc.Current + time + index + 1) % disc.Positions;
                    if (num != 0) {
                        isGood = false;
                        break;
                    }
                }
            } while (!isGood);
            return time;
        }

        private void GetDiscs(List<string> input) {
            _discs = input.Select(line => {
                var split = line.Split(' ');
                return new Disc() {
                    Current = Convert.ToInt32(split[11].Replace(".", " ")),
                    Positions = Convert.ToInt32(split[3])
                };
            }).ToList();
        }

        private class Disc {
            public int Positions { get; set; }
            public int Current { get; set; }
        }
    }
}
