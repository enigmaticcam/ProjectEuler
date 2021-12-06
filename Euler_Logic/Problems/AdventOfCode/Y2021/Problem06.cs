using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem06 : AdventOfCodeBase {
        private List<int> _fish;

        public override string ProblemName {
            get { return "Advent of Code 2021: 06"; }
        }

        public override string GetAnswer() {
            GetFish(Input_Test(1));
            return Answer2(256).ToString();
        }

        private int Answer1(int days) {
            Simulate1(days);
            return _fish.Count;
        }

        private ulong Answer2(int days) {
            return Simulate2(days);
        }

        private int Simulate1(int days) {
            for (int count = 1; count <= days; count++) {
                var newFish = new List<int>();
                for (int index = 0; index < _fish.Count; index++) {
                    _fish[index]--;
                    if (_fish[index] == -1) {
                        _fish[index] = 6;
                        newFish.Add(8);
                    }
                }
                _fish.AddRange(newFish);
            }
            return _fish.Count;
        }

        private ulong Simulate2(int days) {
            var daysForward = new ulong[9];
            foreach (var fish in _fish) {
                daysForward[fish + 1]++;
            }
            int dayIndex = 0;
            ulong fishCount = (ulong)_fish.Count;
            for (int count = 1; count <= days; count++) {
                dayIndex = (dayIndex + 1) % 9;
                var fishAdded = daysForward[dayIndex];
                fishCount += fishAdded;
                daysForward[(dayIndex + 7) % 9] += fishAdded;
            }
            return fishCount;
        }

        private void GetFish(List<string> input) {
            _fish = input[0].Split(',').Select(x => Convert.ToInt32(x)).ToList();
        }
    }
}
