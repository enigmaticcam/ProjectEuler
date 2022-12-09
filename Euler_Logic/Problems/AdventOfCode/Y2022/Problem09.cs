using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem09 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 9";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var moves = GetMoves(input);
            var state = Process(moves);
            return Count(state);
        }

        private int Answer2(List<string> input) {
            var moves = GetMoves(input);
            var state = ProcessAll(moves);
            return Count(state);
        }

        private State Process(List<Move> moves) {
            var state = new State() {
                Hash = new Dictionary<int, HashSet<int>>(),
                Head = new Point(),
                Tail = new Point()
            };
            foreach (var move in moves) {
                for (int count = 1; count <= move.Count; count++) {
                    MoveHead(state, move);
                    Follow(state.Head, state.Tail);
                    AddToHash(state.Tail, state.Hash);
                }
            }
            return state;
        }

        private State ProcessAll(List<Move> moves) {
            var state = new State() {
                Hash = new Dictionary<int, HashSet<int>>(),
                Head = new Point(),
                All = Enumerable.Range(0, 9).Select(x => new Point()).ToArray()
            };
            foreach (var move in moves) {
                for (int count = 1; count <= move.Count; count++) {
                    MoveHead(state, move);
                    Follow(state.Head, state.All[0]);
                    for (int index = 1; index < state.All.Length; index++) {
                        Follow(state.All[index - 1], state.All[index]);
                    }
                    AddToHash(state.All.Last(), state.Hash);
                }
            }
            return state;
        }

        private void MoveHead(State state, Move move) {
            switch (move.Direction) {
                case enumDirection.Up:
                    state.Head.Y--;
                    break;
                case enumDirection.Down:
                    state.Head.Y++;
                    break;
                case enumDirection.Left:
                    state.Head.X--;
                    break;
                case enumDirection.Right:
                    state.Head.X++;
                    break;
            }
        }

        private void AddToHash(Point point, Dictionary<int, HashSet<int>> hash) {
            if (!hash.ContainsKey(point.X)) {
                hash.Add(point.X, new HashSet<int>());
            }
            hash[point.X].Add(point.Y);
        }

        private void Follow(Point head, Point tail) {
            if (head.X != tail.X && head.Y != tail.Y && (Math.Abs(head.X - tail.X) == 2 || Math.Abs(head.Y - tail.Y) == 2)) {
                if (head.X > tail.X) {
                    tail.X++;
                } else {
                    tail.X--;
                }
                if (head.Y > tail.Y) {
                    tail.Y++;
                } else {
                    tail.Y--;
                }
            } else if (head.X - tail.X == 2) {
                tail.X++;
            } else if (tail.X - head.X == 2) {
                tail.X--;
            } else if (head.Y - tail.Y == 2) {
                tail.Y++;
            } else if (tail.Y - head.Y == 2) {
                tail.Y--;
            }
        }

        private int Count(State state) {
            return state.Hash.SelectMany(x => x.Value).Count();
        }

        private List<Move> GetMoves(List<string> input) {
            return input.Select(x => new Move() {
                Count = Convert.ToInt32(x.Substring(2)),
                Direction = Move.GetDirection(x[0])
            }).ToList();
        }

        private class Move {
            public enumDirection Direction { get; set; }
            public int Count { get; set; }

            public static enumDirection GetDirection(char digit) {
                switch (digit) {
                    case 'D': return enumDirection.Down;
                    case 'U': return enumDirection.Up;
                    case 'L': return enumDirection.Left;
                    case 'R': return enumDirection.Right;
                    default: throw new Exception();
                }
            }
        }

        private enum enumDirection {
            Down,
            Up,
            Left,
            Right
        }

        private class Point {
            public int X { get; set; }
            public int Y { get; set; }
        }

        private class State {
            public Point Head { get; set; }
            public Point Tail { get; set; }
            public Point[] All { get; set; }
            public Dictionary<int, HashSet<int>> Hash { get; set; }
        }
    }
}
