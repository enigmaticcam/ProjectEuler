using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem01 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2016: 1"; }
        }

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        public int Answer1() {
            var moves = GetMoves();
            return PerformMoves(moves);
        }

        public int Answer2() {
            var moves = GetMoves();
            return FindTwiceLocation(moves);
        }

        private int PerformMoves(List<Move> moves) {
            int x = 0;
            int y = 0;
            int index = 0;
            var velocities = GetVelocities();
            foreach (var move in moves) {
                if (move.Direction == enumDirection.Left) {
                    index--;
                    if (index == -1) {
                        index = 3;
                    }
                } else {
                    index++;
                    if (index > 3) {
                        index = 0;
                    }
                }
                x += velocities[index].X * move.Count;
                y += velocities[index].Y * move.Count;
            }
            return Math.Abs(x) + Math.Abs(y);
        }

        private int FindTwiceLocation(List<Move> moves) {
            int x = 0;
            int y = 0;
            int index = 0;
            var velocities = GetVelocities();
            var hash = new Dictionary<int, HashSet<int>>();
            hash.Add(0, new HashSet<int>());
            hash[0].Add(0);
            foreach (var move in moves) {
                if (move.Direction == enumDirection.Left) {
                    index--;
                    if (index == -1) {
                        index = 3;
                    }
                } else {
                    index++;
                    if (index > 3) {
                        index = 0;
                    }
                }
                for (int count = 1; count <= move.Count; count++) {
                    x += velocities[index].X;
                    y += velocities[index].Y;
                    if (!hash.ContainsKey(x)) {
                        hash.Add(x, new HashSet<int>());
                    }
                    if (hash[x].Contains(y)) {
                        return Math.Abs(x) + Math.Abs(y);
                    } else {
                        hash[x].Add(y);
                    }
                }
            }
            return -1;
        }

        private Velocity[] GetVelocities() {
            var velocities = new Velocity[4];
            velocities[0] = new Velocity(0, 1);
            velocities[1] = new Velocity(1, 0);
            velocities[2] = new Velocity(0, -1);
            velocities[3] = new Velocity(-1, 0);
            return velocities;
        }

        private List<Move> GetMoves() {
            var moves = Input().First().Split(',');
            return moves.Select(x => {
                x = x.Trim();
                var move = new Move();
                if (x[0] == 'L') {
                    move.Direction = enumDirection.Left;
                } else {
                    move.Direction = enumDirection.Right;
                }
                move.Count = Convert.ToInt32(x.Substring(1, x.Length - 1));
                return move;
            }).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "R8, R4, R4, R8"
            };
        }

        private class Velocity {
            public Velocity() { }
            public Velocity(int x, int y) {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }
        }

        private class Move {
            public enumDirection Direction { get; set; }
            public int Count { get; set; }
        }

        private enum enumDirection {
            Left,
            Right
        }
    }
}
