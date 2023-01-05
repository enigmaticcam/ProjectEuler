using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem23 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 23";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var state = new State();
            SetMovers(state, input);
            SetHash(state);
            PerformMoves(state, 10);
            SetMinMax(state);
            return CalcEmpty(state);
        }

        private int Answer2(List<string> input) {
            var state = new State();
            SetMovers(state, input);
            SetHash(state);
            return PerformMovesUntilEnd(state);
        }

        private int PerformMovesUntilEnd(State state) {
            int count = 1;
            do {
                Step1(state);
                if (!Step2(state)) return count;
                state.CurrentDirection = (state.CurrentDirection + 1) % 4;
                count++;
            } while (true);
        }

        private int CalcEmpty(State state) {
            int all = (state.MaxX - state.MinX + 1) * (state.MaxY - state.MinY + 1);
            return all - state.Movers.Count;
        }

        private void SetMinMax(State state) {
            state.MinX = state.Movers.Select(x => x.X).Min();
            state.MinY = state.Movers.Select(x => x.Y).Min();
            state.MaxX = state.Movers.Select(x => x.X).Max();
            state.MaxY = state.Movers.Select(x => x.Y).Max();
        }

        private void PerformMoves(State state, int total) {
            for (int count = 1; count <= total; count++) {
                Step1(state);
                Step2(state);
                state.CurrentDirection = (state.CurrentDirection + 1) % 4;
            }
        }

        private bool Step2(State state) {
            var hash = new Dictionary<Tuple<int, int>, int>();
            foreach (var mover in state.Movers) {
                if (mover.CanMove) {
                    mover.Key = new Tuple<int, int>(mover.NextX, mover.NextY);
                    if (hash.ContainsKey(mover.Key)) {
                        hash[mover.Key]++;
                    } else {
                        hash.Add(mover.Key, 1);
                    }
                }
            }
            bool didMove = false;
            foreach (var mover in state.Movers) {
                if (mover.CanMove && hash[mover.Key] == 1) {
                    state.Hash[mover.X].Remove(mover.Y);
                    mover.X = mover.NextX;
                    mover.Y = mover.NextY;
                    AddHash(state, mover.X, mover.Y);
                    didMove = true;
                }
            }
            return didMove;
        }

        private void Step1(State state) {
            foreach (var mover in state.Movers) {
                mover.CanMove = SetNext(state, mover);
            }
        }

        private bool SetNext(State state, Mover mover) {
            int direction = state.CurrentDirection;
            var bits = GetBits(state, mover.X, mover.Y);
            if (bits != 0) {
                int count = 1;
                do {
                    switch (direction) {
                        case 0:
                            if ((bits & 7) == 0) {
                                mover.NextX = mover.X;
                                mover.NextY = mover.Y - 1;
                                return true;
                            }
                            break;
                        case 1:
                            if ((bits & 448) == 0) {
                                mover.NextX = mover.X;
                                mover.NextY = mover.Y + 1;
                                return true;
                            }
                            break;
                        case 2:
                            if ((bits & 73) == 0) {
                                mover.NextX = mover.X - 1;
                                mover.NextY = mover.Y;
                                return true;
                            }
                            break;
                        case 3:
                            if ((bits & 292) == 0) {
                                mover.NextX = mover.X + 1;
                                mover.NextY = mover.Y;
                                return true;
                            }
                            break;
                    }
                    direction = (direction + 1) % 4;
                    count++;
                } while (count <= 4);
            }
            return false;
        }

        private void AddHash(State state, int x, int y) {
            if (!state.Hash.ContainsKey(x)) {
                state.Hash.Add(x, new HashSet<int>());
            }
            state.Hash[x].Add(y);
        }

        private int GetBits(State state, int x, int y) {
            int bits = 0;
            if (MoverExists(state, x - 1, y - 1)) bits += 1;
            if (MoverExists(state, x, y - 1)) bits += 2;
            if (MoverExists(state, x + 1, y - 1)) bits += 4;
            if (MoverExists(state, x - 1, y)) bits += 8;
            if (MoverExists(state, x + 1, y)) bits += 32;
            if (MoverExists(state, x - 1, y + 1)) bits += 64;
            if (MoverExists(state, x, y + 1)) bits += 128;
            if (MoverExists(state, x + 1, y + 1)) bits += 256;
            return bits;
        }

        private bool MoverExists(State state, int x, int y) {
            return state.Hash.ContainsKey(x) && state.Hash[x].Contains(y);
        }

        private void SetHash(State state) {
            state.Hash = new Dictionary<int, HashSet<int>>();
            foreach (var mover in state.Movers) {
                AddHash(state, mover.X, mover.Y);
            }
        }

        private void SetMovers(State state, List<string> input) {
            state.Movers = new List<Mover>();
            int y = 0;
            foreach (var line in input) {
                int x = 0;
                foreach (var digit in line) {
                    if (digit == '#') {
                        state.Movers.Add(new Mover() {
                            X = x,
                            Y = y
                        });
                    }
                    x++;
                }
                y++;
            }
        }

        private class State {
            public List<Mover> Movers { get; set; }
            public Dictionary<int, HashSet<int>> Hash { get; set; }
            public int CurrentDirection { get; set; }
            public int MinX { get; set; }
            public int MinY { get; set; }
            public int MaxX { get; set; }
            public int MaxY { get; set; }
        }

        private class Mover {
            public int X { get; set; }
            public int Y { get; set; }
            public int NextX { get; set; }
            public int NextY { get; set; }
            public bool CanMove { get; set; }
            public Tuple<int, int> Key { get; set; }
        }
    }
}

