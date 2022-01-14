using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem14 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2015: 14";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var deer = GetDeer(input);
            return GetFarthest(deer, 2503);
        }

        private int Answer2(List<string> input) {
            var deer = GetDeer(input);
            return GetScore(deer, 2503);
        }

        private int GetFarthest(List<Deer> deer, int time) {
            int farthest = 0;
            foreach (var d in deer) {
                var cycles = time / (d.Fly + d.Rest);
                var distance = cycles * d.Fly * d.Rate;
                var remaining = time - (cycles * (d.Fly + d.Rest));
                var result = Math.Min(remaining, d.Fly) * d.Rate + distance;
                if (result > farthest) {
                    farthest = result;
                }
            }
            return farthest;
        }

        private int GetScore(List<Deer> deer, int time) {
            var score = new Dictionary<string, int>();
            deer.ForEach(d => score.Add(d.Id, 0));
            var winningList = new string[deer.Count];
            var winningCount = 0;
            for (int sec = 1; sec <= time; sec++) {
                int farthest = 0;
                foreach (var d in deer) {
                    var cycles = sec / (d.Fly + d.Rest);
                    var distance = cycles * d.Fly * d.Rate;
                    var remaining = sec - (cycles * (d.Fly + d.Rest));
                    var result = Math.Min(remaining, d.Fly) * d.Rate + distance;
                    if (result == farthest) {
                        winningCount++;
                        winningList[winningCount] = d.Id;
                    } else if (result > farthest) {
                        farthest = result;
                        winningCount = 0;
                        winningList[winningCount] = d.Id;
                    }
                }
                for (int index = 0; index <= winningCount; index++) {
                    score[winningList[index]]++;
                }
            }
            return score.Values.Max();
        }

        private List<Deer> GetDeer(List<string> input) {
            return input.Select(line => {
                var deer = new Deer();
                var split = line.Split(' ');
                deer.Id = split[0];
                deer.Fly = Convert.ToInt32(split[6]);
                deer.Rate = Convert.ToInt32(split[3]);
                deer.Rest = Convert.ToInt32(split[13]);
                return deer;
            }).ToList();
        }

        private class Deer {
            public string Id { get; set; }
            public int Fly { get; set; }
            public int Rate { get; set; }
            public int Rest { get; set; }
        }
    }
}
