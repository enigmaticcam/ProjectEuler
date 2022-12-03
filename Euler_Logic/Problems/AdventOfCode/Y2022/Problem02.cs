using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem02 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 2";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var games = GetGames(input);
            int total = 0;
            foreach (var game in games) {
                var score = GetScore(game);
                total += score;
            }
            return total;
        }

        private int Answer2(List<string> input) {
            var games = GetGames(input);
            int total = 0;
            foreach (var game in games) {
                int second = 0;
                if (game.Item2 == 1) {
                    if (game.Item1 == 1) {
                        second = 3;
                    } else if (game.Item1 == 2) {
                        second = 1;
                    } else {
                        second = 2;
                    }
                } else if (game.Item2 == 2) {
                    second = game.Item1;
                } else {
                    if (game.Item1 == 1) {
                        second = 2;
                    } else if (game.Item1 == 2) {
                        second = 3;
                    } else {
                        second = 1;
                    }
                }
                var score = GetScore(new Tuple<int, int>(game.Item1, second));
                total += score;
            }
            return total;
        }

        private int GetScore(Tuple<int, int> game) {
            // 1 = rock
            // 2 = paper
            // 3 = scissors
            if (game.Item1 == game.Item2)
                return game.Item2 + 3;
            if (game.Item2 == 1) {
                if (game.Item1 == 2) {
                    return game.Item2;
                } else {
                    return game.Item2 + 6;
                }
            } else if (game.Item2 == 2) {
                if (game.Item1 == 1) {
                    return game.Item2 + 6;
                } else {
                    return game.Item2;
                }
            } else {
                if (game.Item1 == 1) {
                    return game.Item2;
                } else {
                    return game.Item2 + 6;
                }
            }
        }

        private List<Tuple<int, int>> GetGames(List<string> input) {
            return input.Select(x => {
                var split = x.Split(' ');
                int first = 0;
                int second = 0;
                switch (split[0]) {
                    case "A":
                        first = 1;
                        break;
                    case "B":
                        first = 2;
                        break;
                    case "C":
                        first = 3;
                        break;
                }
                switch (split[1]) {
                    case "X":
                        second = 1;
                        break;
                    case "Y":
                        second = 2;
                        break;
                    case "Z":
                        second = 3;
                        break;

                }
                return new Tuple<int, int>(first, second);
            }).ToList();
        }
    }
}
