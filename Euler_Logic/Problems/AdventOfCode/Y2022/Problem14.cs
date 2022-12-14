using System;
using System.Collections.Generic;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem14 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 14";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var state = new State();
            SetLines(input, state);
            SetMinMax(state);
            BuildGrid(state);
            return Process(state);
        }

        private int Answer2(List<string> input) {
            var state = new State();
            SetLines(input, state);
            SetMinMax(state);
            BuildGrid(state);
            return Process2(state);
        }

        private int Process2(State state) {
            int count = 0;
            var current = new Point() { X = 500, Y = 0 };
            do {
                if (TryNext2(state, current.X, current.Y + 1)) {
                    current.Y++;
                } else if (TryNext2(state, current.X - 1, current.Y + 1)) {
                    current.X--;
                    current.Y++;
                } else if (TryNext2(state, current.X + 1, current.Y + 1)) {
                    current.X++;
                    current.Y++;
                } else if (current.X == 500 && current.Y == 0) {
                    return count + 1;
                } else {
                    count++;
                    AddToDictionary(state, current.X, current.Y, 'O');
                    current = new Point() { X = 500, Y = 0 };
                }
            } while (true);
        }

        private int Process(State state) {
            int count = 0;
            var current = new Point() { X = 500, Y = 0 };
            do {
                if (current.Y == state.Max.Y) return count;
                if (TryNext(state, current.X, current.Y + 1)) {
                    current.Y++;
                } else if (TryNext(state, current.X - 1, current.Y + 1)) {
                    current.X--;
                    current.Y++;
                } else if (TryNext(state, current.X + 1, current.Y + 1)) {
                    current.X++;
                    current.Y++;
                } else {
                    count++;
                    AddToDictionary(state, current.X, current.Y, 'O');
                    current = new Point() { X = 500, Y = 0 };
                }
            } while (true);
        }

        private bool TryNext2(State state, int nextX, int nextY) {
            if (state.Grid.ContainsKey(nextX) && state.Grid[nextX].ContainsKey(nextY)) {
                var next = state.Grid[nextX][nextY];
                return next == '.';
            } else if (nextY == state.Max.Y + 1) {
                return true;
            } else if (nextY == state.Max.Y + 2) {
                return false;
            } else {
                return true;
            }
        }

        private bool TryNext(State state, int nextX, int nextY) {
            if (state.Grid.ContainsKey(nextX) && state.Grid[nextX].ContainsKey(nextY)) {
                var next = state.Grid[nextX][nextY];
                return next == '.';
            } else {
                return true;
            }
        }

        private void BuildGrid(State state) {
            state.Grid = new Dictionary<int, Dictionary<int, char>>();
            for (int x = state.Min.X; x <= state.Max.X; x++) {
                for (int y = state.Min.Y; y <= state.Max.Y; y++) {
                    AddToDictionary(state, x, y, '.');
                }
            }
            AddToDictionary(state, 500, 0, '+');
            foreach (var line in state.Lines) {
                var next = line;
                Line last = null;
                do {
                    if (last != null) {
                        int x = last.LinePoint.X;
                        int y = last.LinePoint.Y;
                        if (next.LinePoint.X != last.LinePoint.X) {
                            int direction = last.LinePoint.X > next.LinePoint.X ? -1 : 1;
                            do {
                                AddToDictionary(state, x, y, '#');
                                if (x == next.LinePoint.X) break;
                                x += direction;
                            } while (true);
                        } else {
                            int direction = last.LinePoint.Y > next.LinePoint.Y ? -1 : 1;
                            do {
                                AddToDictionary(state, x, y, '#');
                                if (y == next.LinePoint.Y) break;
                                y += direction;
                            } while (true);
                        }
                    }
                    last = next;
                    next = next.Next;
                } while (next != null);
            }
        }

        private void AddToDictionary(State state, int x, int y, char digit) {
            if (!state.Grid.ContainsKey(x)) state.Grid.Add(x, new Dictionary<int, char>());
            if (state.Grid[x].ContainsKey(y)) {
                state.Grid[x][y] = digit;
            } else {
                state.Grid[x].Add(y, digit);
            }
        }

        private void SetMinMax(State state) {
            state.Min = new Point() { X = 500, Y = 0 };
            state.Max = new Point() { X = int.MinValue, Y = 0 };
            foreach (var line in state.Lines) {
                var temp = line;
                do {
                    if (temp.LinePoint.X < state.Min.X) state.Min.X = temp.LinePoint.X;
                    if (temp.LinePoint.X > state.Max.X) state.Max.X = temp.LinePoint.X;
                    if (temp.LinePoint.Y > state.Max.Y) state.Max.Y = temp.LinePoint.Y;
                    temp = temp.Next;
                } while (temp != null);
            }
            if (500 > state.Max.X) state.Max.X = 500;
            if (0 > state.Max.Y) state.Max.Y = 0;
        }

        private void SetLines(List<string> input, State state) {
            state.Lines = new List<Line>();
            foreach (var text in input) {
                var replace = text.Replace("->", "|");
                var split = replace.Split('|');
                Line current = null;
                foreach (var point in split) {
                    var subSplit = point.Split(',');
                    if (current == null) {
                        current = new Line();
                        state.Lines.Add(current);
                    } else {
                        current.Next = new Line();
                        current = current.Next;
                    }
                    current.LinePoint = new Point() {
                        X = Convert.ToInt32(subSplit[0]),
                        Y = Convert.ToInt32(subSplit[1])
                    };
                }
            }
        }

        private string Output(State state) {
            var text = new StringBuilder();
            for (int y = state.Min.Y; y <= state.Max.Y; y++) {
                for (int x = state.Min.X; x <= state.Max.X; x++) {
                    text.Append(state.Grid[x][y]);
                }
                text.AppendLine();
            }
            return text.ToString();
        }

        private class State {
            public List<Line> Lines { get; set; }
            public Point Max { get; set; }
            public Point Min { get; set; }
            public Dictionary<int, Dictionary<int, char>> Grid { get; set; }
        }

        private class Line {
            public Point LinePoint { get; set; }
            public Line Next { get; set; }
        }

        private class Point {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
