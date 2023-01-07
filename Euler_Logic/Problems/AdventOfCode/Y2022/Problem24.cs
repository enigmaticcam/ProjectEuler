using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem24 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 24";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var state = new State();
            var recursive = new RecursiveData() {
                Best = int.MaxValue,
                Hash = new Dictionary<int, Dictionary<int, HashSet<int>>>()
            };
            SetGrid(state, input);
            SetStart(state, recursive, false);
            SetEmptyMinutes(state);
            Recursive(state, recursive, true, false);
            return recursive.Best;
        }

        private int Answer2(List<string> input) {
            var state = new State();
            SetGrid(state, input);
            SetEmptyMinutes(state);
            return ReverseRecursive(state);
        }

        private int ReverseRecursive(State state) {
            var data = new RecursiveData() { Best = int.MaxValue, Hash = new Dictionary<int, Dictionary<int, HashSet<int>>>() };
            SetStart(state, data, false);
            Recursive(state, data, true, false);

            data.Hash = new Dictionary<int, Dictionary<int, HashSet<int>>>();
            data.Minutes = data.Best;
            data.Best = int.MaxValue;
            SetStart(state, data, true);
            Recursive(state, data, true, true);

            data.Hash = new Dictionary<int, Dictionary<int, HashSet<int>>>();
            data.Minutes = data.Best;
            data.Best = int.MaxValue;
            SetStart(state, data, false);
            Recursive(state, data, true, false);
            return data.Best;
        }

        private void SetStart(State state, RecursiveData data, bool isReverse) {
            if (!isReverse) {
                data.X = 1;
                data.Y = 0;
                data.EndX = state.MaxX - 1;
                data.EndY = state.MaxY;
            } else {
                data.X = state.MaxX - 1;
                data.Y = state.MaxY;
                data.EndX = 1;
                data.EndY = 0;
            }
            data.StartX = data.X;
            data.StartY = data.Y;
        }

        private void Recursive(State state, RecursiveData data, bool start, bool isReverse) {
            if (data.X == data.EndX && data.Y == data.EndY) {
                if (data.Minutes < data.Best) {
                    data.Best = data.Minutes;
                }
            } else if (data.Minutes < data.Best - 1 && Continue(data) && CheckHash(state, data)) {
                if (!isReverse) {
                    if (CanMove(state, data, data.X + 1, data.Y, data.Minutes + 1, start)) {
                        RecursiveNext(state, data, 1, 0, false, isReverse);
                    }
                    if (CanMove(state, data, data.X, data.Y + 1, data.Minutes + 1, start)) {
                        RecursiveNext(state, data, 0, 1, false, isReverse);
                    }
                    if (CanMove(state, data, data.X - 1, data.Y, data.Minutes + 1, start)) {
                        RecursiveNext(state, data, -1, 0, false, isReverse);
                    }
                    if (CanMove(state, data, data.X, data.Y - 1, data.Minutes + 1, start)) {
                        RecursiveNext(state, data, 0, -1, false, isReverse);
                    }
                    if (CanMove(state, data, data.X, data.Y, data.Minutes + 1, start)) {
                        RecursiveNext(state, data, 0, 0, start, isReverse);
                    }
                } else {
                    if (CanMove(state, data, data.X - 1, data.Y, data.Minutes + 1, start)) {
                        RecursiveNext(state, data, -1, 0, false, isReverse);
                    }
                    if (CanMove(state, data, data.X, data.Y - 1, data.Minutes + 1, start)) {
                        RecursiveNext(state, data, 0, -1, false, isReverse);
                    }
                    if (CanMove(state, data, data.X + 1, data.Y, data.Minutes + 1, start)) {
                        RecursiveNext(state, data, 1, 0, false, isReverse);
                    }
                    if (CanMove(state, data, data.X, data.Y + 1, data.Minutes + 1, start)) {
                        RecursiveNext(state, data, 0, 1, false, isReverse);
                    }
                    if (CanMove(state, data, data.X, data.Y, data.Minutes + 1, start)) {
                        RecursiveNext(state, data, 0, 0, start, isReverse);
                    }
                }
                
            }
        }

        private bool CheckHash(State state, RecursiveData data) {
            if (!data.Hash.ContainsKey(data.X)) data.Hash.Add(data.X, new Dictionary<int, HashSet<int>>());
            if (!data.Hash[data.X].ContainsKey(data.Y)) data.Hash[data.X].Add(data.Y, new HashSet<int>());
            if (data.Hash[data.X][data.Y].Contains(data.Minutes)) {
                return false;
            } else {
                data.Hash[data.X][data.Y].Add(data.Minutes);
                return true;
            }
        }

        private bool Continue(RecursiveData data) {
            int distance = Math.Abs(data.EndX - data.X) + Math.Abs(data.EndY - data.Y);
            return distance + data.Minutes < data.Best;
        }

        private void RecursiveNext(State state, RecursiveData data, int xDiff, int yDiff, bool start, bool isReverse) {
            data.X += xDiff;
            data.Y += yDiff;
            data.Minutes++;
            Recursive(state, data, start, isReverse);
            data.Minutes--;
            data.Y -= yDiff;
            data.X -= xDiff;
        }

        private bool CanMove(State state, RecursiveData data, int x, int y, int minutes, bool start) {
            if (x == data.EndX && y == data.EndY) return true;
            if (x == data.StartX && y == data.StartY && start) return true;
            if (x >= state.MaxX || x <= 0 || y >= state.MaxY || y <= 0) return false;
            if (state.BlizzardMinutes[x, y, 0] != null) {
                var mod = minutes % (state.MaxX - 1);
                if (state.BlizzardMinutes[x, y, 0].Contains(mod)) return false;
            }
            if (state.BlizzardMinutes[x, y, 1] != null) {
                var mod = minutes % (state.MaxY - 1);
                if (state.BlizzardMinutes[x, y, 1].Contains(mod)) return false;
            }
            return true;
        }

        private void SetEmptyMinutes(State state) {
            state.BlizzardMinutes = new HashSet<int>[state.MaxX + 1, state.MaxY + 1, 2];
            int modX = state.MaxX - 1;
            int modY = state.MaxY - 1;
            for (int x = 0; x <= state.MaxX; x++) {
                for (int y = 0; y <= state.MaxY; y++) {
                    var digit = state.Grid[x, y];
                    if (digit != '.' && digit != '#') {
                        int nextX = x - 1;
                        int nextY = y - 1;
                        int directionX = 1;
                        int directionY = 0;
                        int directionIndex = 0;
                        if (digit == '<') {
                            directionX = -1;
                        } else if (digit == '^') {
                            directionX = 0;
                            directionY = -1;
                            directionIndex = 1;
                        } else if (digit == 'v') {
                            directionX = 0;
                            directionY = 1;
                            directionIndex = 1;
                        }
                        int minutes = 0;
                        do {
                            if (state.Grid[nextX + 1, nextY + 1] != '#') {
                                if (state.BlizzardMinutes[nextX + 1, nextY + 1, directionIndex] == null) state.BlizzardMinutes[nextX + 1, nextY + 1, directionIndex] = new HashSet<int>();
                                state.BlizzardMinutes[nextX + 1, nextY + 1, directionIndex].Add(minutes);
                                minutes++;
                            }
                            nextX = Mod(nextX + directionX, modX);
                            nextY = Mod(nextY + directionY, modY);
                        } while (nextX != x - 1 || nextY != y - 1);
                    }
                }
            }
        }

        private int Mod(int num, int divisor) {
            num %= divisor;
            if (num < 0) num += divisor;
            return num;
        }

        private void SetGrid(State state, List<string> input) {
            state.Grid = new char[input[0].Length, input.Count];
            state.MaxX = state.Grid.GetUpperBound(0);
            state.MaxY = state.Grid.GetUpperBound(1);
            for (int y = 0; y < input.Count; y++) {
                var line = input[y];
                for (int x = 0; x < line.Length; x++) {
                    state.Grid[x, y] = line[x];
                }
            }
        }

        private class State {
            public char[,] Grid { get; set; }
            public HashSet<int>[,,] BlizzardMinutes { get; set; }
            public int MaxX { get; set; }
            public int MaxY { get; set; }
        }

        private class RecursiveData {
            public int Best { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Minutes { get; set; }
            public Dictionary<int, Dictionary<int, HashSet<int>>> Hash { get; set; }
            public int EndX { get; set; }
            public int EndY { get; set; }
            public int StartX { get; set; }
            public int StartY { get; set; }
        }

        private class Node {
            public int X { get; set; }
            public int Y { get; set; }
            public int Minutes { get; set; }
            public bool Start { get; set; }
        }
    }
}
