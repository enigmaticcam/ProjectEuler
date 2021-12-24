using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem21 : AdventOfCodeBase {
        private int[] _positions;

        public override string ProblemName {
            get { return "Advent of Code 2021: 21"; }
        }

        public override string GetAnswer() {
            GetPositions(Input_Test(1));
            return Answer2().ToString();
        }

        private int Answer1() {
            return Simulate();
        }

        private ulong Answer2() {
            var player1 = FindAll(_positions[0]);
            var player2 = FindAll(_positions[1]);
            for (int turn = 1; turn <= 21; turn++) {
                
            }

            return 0;
        }

        private ulong[,,] FindAll(int startPosition) {
            var combos = GetDiceCombos();
            var d = new ulong[21, 10, 22]; // turn, position, score
            foreach (var combo in combos) {
                int position = (combo.Key + startPosition) % 10;
                d[0, position, position] = combo.Value;
            }
            for (int turn = 1; turn < 21; turn++) {
                for (int position = 0; position < 10; position++) {
                    for (int score = 1; score < 22; score++) {
                        if (d[turn - 1, position, score] != 0) {
                            foreach (var combo in combos) {
                                int subPosition = (position + combo.Key) % 10;
                                var subScore = Math.Min(21, score + subPosition);
                                d[turn, subPosition, subScore] = d[turn - 1, position, score] * combo.Value;
                            }
                        }
                        
                    }
                }
            }
            return d;
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

        private void FindValues(ulong[,,] d) {
            var list = new List<Tuple<int, int, int, ulong>>();
            for (int x = 0; x <= d.GetUpperBound(0); x++) {
                for (int y = 0; y <= d.GetUpperBound(1); y++) {
                    for (int z = 0; z <= d.GetUpperBound(2); z++) {
                        if (d[x, y, z] != 0) {
                            list.Add(new Tuple<int, int, int, ulong>(x, y, z, d[x, y, z]));
                        }
                    }
                }
            }
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
