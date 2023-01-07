using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem24 : AdventOfCodeBase {
        /*
         * 375 - too high
         * 220 - too low
         */
        public override string ProblemName => "Advent of Code 2022: 24";

        public override string GetAnswer() {
            //return Answer1(Input_Test(1)).ToString();
            return Answer1(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var state = new State();
            var recursive = new RecursiveData() {
                Best = int.MaxValue,
                X = 1,
                Y = 0
            };
            SetGrid(state, input);
            SetEmptyMinutes(state);
            Recursive(state, recursive, true);
            return recursive.Best;
            //return BFS(state);
        }

        private int BFS(State state) {
            var list = new LinkedList<Node>();
            list.AddLast(new Node() { Minutes = 0, X = 1, Y = 0, Start = true });
            int count = 1;
            do {
                var next = list.First.Value;
                if (next.X == state.MaxX - 1 && next.Y == state.MaxY) return next.Minutes;
                if (CanMove(state, next.X + 1, next.Y, next.Minutes + 1, next.Start)) {
                    list.AddLast(new Node() { Minutes = next.Minutes + 1, X = next.X + 1, Y = next.Y });
                }
                if (CanMove(state, next.X, next.Y + 1, next.Minutes + 1, next.Start)) {
                    list.AddLast(new Node() { Minutes = next.Minutes + 1, X = next.X, Y = next.Y + 1 });
                }
                if (CanMove(state, next.X - 1, next.Y, next.Minutes + 1, next.Start)) {
                    list.AddLast(new Node() { Minutes = next.Minutes + 1, X = next.X - 1, Y = next.Y });
                }
                if (CanMove(state, next.X, next.Y - 1, next.Minutes + 1, next.Start)) {
                    list.AddLast(new Node() { Minutes = next.Minutes + 1, X = next.X, Y = next.Y - 1 });
                }
                if (CanMove(state, next.X, next.Y, next.Minutes + 1, next.Start)) {
                    list.AddLast(new Node() { Minutes = next.Minutes + 1, X = next.X, Y = next.Y, Start = next.Start });
                }
                list.RemoveFirst();
                count++;
            } while (true);
        }

        private void Recursive(State state, RecursiveData data, bool start) {
            if (data.X == state.MaxX - 1 && data.Y == state.MaxY) {
                if (data.Minutes < data.Best) {
                    data.Best = data.Minutes;
                }
            } else if (data.Minutes < data.Best - 1 && Continue(state, data)) {
                if (CanMove(state, data.X + 1, data.Y, data.Minutes + 1, start)) {
                    RecursiveNext(state, data, 1, 0, false);
                }
                if (CanMove(state, data.X, data.Y + 1, data.Minutes + 1, start)) {
                    RecursiveNext(state, data, 0, 1, false);
                }
                if (CanMove(state, data.X - 1, data.Y, data.Minutes + 1, start)) {
                    RecursiveNext(state, data, -1, 0, false);
                }
                if (CanMove(state, data.X, data.Y - 1, data.Minutes + 1, start)) {
                    RecursiveNext(state, data, 0, -1, false);
                }
                if (CanMove(state, data.X, data.Y, data.Minutes + 1, start)) {
                    RecursiveNext(state, data, 0, 0, start);
                }
            }
        }

        private bool Continue(State state, RecursiveData data) {
            int distance = Math.Abs(state.MaxX - 1 - data.X) + Math.Abs(state.MaxY - data.Y);
            return distance + data.Minutes < data.Best;
        }

        private void RecursiveNext(State state, RecursiveData data, int xDiff, int yDiff, bool start) {
            data.X += xDiff;
            data.Y += yDiff;
            data.Minutes++;
            Recursive(state, data, start);
            data.Minutes--;
            data.Y -= yDiff;
            data.X -= xDiff;
        }

        private bool CanMove(State state, int x, int y, int minutes, bool start) {
            if (x == state.MaxX - 1 && y == state.MaxY) return true;
            if (x == 1 && y == 0 && start) return true;
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
        }

        private class Node {
            public int X { get; set; }
            public int Y { get; set; }
            public int Minutes { get; set; }
            public bool Start { get; set; }
        }
    }
}
