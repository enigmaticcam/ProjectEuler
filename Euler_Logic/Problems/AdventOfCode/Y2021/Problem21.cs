using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem21 : AdventOfCodeBase {
        private int[] _positions;
        private Dictionary<int, ulong> _combos;

        public override string ProblemName {
            get { return "Advent of Code 2021: 21"; }
        }

        public override string GetAnswer() {
            GetPositions(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            return Simulate();
        }

        private ulong Answer2() {
            _combos = GetDiceCombos();
            int max = 21;
            var wins = new ulong[2];
            var turnWins = new ulong[2, 21];
            Recurisve(_positions, new int[2], wins, 0, max, turnWins, 1, 1);
            return Math.Max(wins[0], wins[1]);
        }

        private void Recurisve(int[] position, int[] score, ulong[] wins, int player, int max, ulong[,] turnWins, int turn, ulong multiply) {
            foreach (var keyValue in _combos) {
                var result = keyValue.Key;
                int oldPosition = position[player];
                int oldScore = score[player];
                position[player] = (position[player] + result) % 10;
                score[player] += position[player] + 1;
                if (score[player] >= max) {
                    wins[player] += keyValue.Value * multiply;
                    turnWins[player, turn] += keyValue.Value * multiply;
                } else {
                    Recurisve(position, score, wins, (player + 1) % 2, max, turnWins, turn + player, multiply * keyValue.Value);
                }
                position[player] = oldPosition;
                score[player] = oldScore;
            }
        }

        private Dictionary<int, ulong> GetDiceCombos() {
            var combos = new Dictionary<int, ulong>();
            for (int dice1 = 1; dice1 <= 3; dice1++) {
                for (int dice2 = 1; dice2 <= 3; dice2++) {
                    for (int dice3 = 1; dice3 <= 3; dice3++) {
                        int sum = dice1 + dice2 + dice3;
                        if (!combos.ContainsKey(sum)) {
                            combos.Add(sum, 1);
                        } else {
                            combos[sum]++;
                        }
                    }
                }
            }
            return combos;
        }

        private int Simulate() {
            var scores = new int[2];
            int currentPlayer = 0;
            int die = 0;
            int rollCount = 0;
            do {
                int next = ((die + 1) % 100) + ((die + 2) % 100) + ((die + 3) % 100);
                rollCount += 3;
                die = (die + 3) % 100;
                _positions[currentPlayer] = (_positions[currentPlayer] + next) % 10;
                scores[currentPlayer] += _positions[currentPlayer] + 1;
                if (scores[currentPlayer] >= 1000) {
                    return rollCount * scores[(currentPlayer + 1) % 2];
                }
                currentPlayer = (currentPlayer + 1) % 2;
            } while (true);
        }

        private void GetPositions(List<string> input) {
            _positions = new int[2];
            _positions[0] = Convert.ToInt32(input[0].Split(' ')[4]) - 1;
            _positions[1] = Convert.ToInt32(input[1].Split(' ')[4]) - 1;
        }
    }
}
