using System;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem11 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 11"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()[0]);
        }

        public override string GetAnswer2() {
            return Answer2(Input()[0]);
        }

        private string Answer1(string input) {
            int x = 0;
            int y = 0;
            string[] stepsToFollow = input.Split(',');
            foreach (string next in stepsToFollow) {
                switch (next) {
                    case "n":
                        y += 2;
                        break;
                    case "ne":
                        y++;
                        x++;
                        break;
                    case "se":
                        y--;
                        x++;
                        break;
                    case "s":
                        y -= 2;
                        break;
                    case "sw":
                        y--;
                        x--;
                        break;
                    case "nw":
                        y++;
                        x--;
                        break;
                }
            }
            return Distance(x, y).ToString();
        }

        private string Answer2(string input) {
            int x = 0;
            int y = 0;
            int maxDistance = int.MinValue;
            string[] stepsToFollow = input.Split(',');
            foreach (string next in stepsToFollow) {
                switch (next) {
                    case "n":
                        y += 2;
                        break;
                    case "ne":
                        y++;
                        x++;
                        break;
                    case "se":
                        y--;
                        x++;
                        break;
                    case "s":
                        y -= 2;
                        break;
                    case "sw":
                        y--;
                        x--;
                        break;
                    case "nw":
                        y++;
                        x--;
                        break;
                }
                int distance = Distance(x, y);
                if (distance > maxDistance) {
                    maxDistance = distance;
                }
            }
            return maxDistance.ToString();
        }

        private int Distance(int x, int y) {
            int count = 0;
            x = Math.Abs(x);
            y = Math.Abs(y);
            while (x > 0 && y > 0) {
                x--;
                y--;
                count++;
            }
            count += (Math.Max(x, y) / 2);
            return count;
        }
    }
}
