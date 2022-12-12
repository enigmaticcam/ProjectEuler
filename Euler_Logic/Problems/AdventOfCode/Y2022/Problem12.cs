using Euler_Logic.Helpers;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem12 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 12";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var state = GetState(input);
            SetHeap(state);
            return FindShortest(state, false);
        }

        private int Answer2(List<string> input) {
            var state = GetState(input);
            return FindShortestFromAny(state);
        }

        private int FindShortestFromAny(State state) {
            state.Start = state.End;
            SetHeap(state);
            return FindShortest(state, true);
        }

        private int FindShortest(State state, bool lookForAny) {
            state.Heap.Reset();
            do {
                var current = (Node)state.Heap.Top;
                if (current.Num == int.MaxValue) return -1;
                if (lookForAny && current.Elevation == 1) {
                    return current.Num;
                } else if (!lookForAny && current == state.End) {
                    return current.Num;
                }
                if (current.X > 0) {
                    var next = state.Grid[current.X - 1, current.Y];
                    IsBetter(current, next, state, lookForAny);
                }
                if (current.X < state.Grid.GetUpperBound(0)) {
                    var next = state.Grid[current.X + 1, current.Y];
                    IsBetter(current, next, state, lookForAny);
                }
                if (current.Y > 0) {
                    var next = state.Grid[current.X, current.Y - 1];
                    IsBetter(current, next, state, lookForAny);
                }
                if (current.Y < state.Grid.GetUpperBound(1)) {
                    var next = state.Grid[current.X, current.Y + 1];
                    IsBetter(current, next, state, lookForAny);
                }
                state.Heap.Remove(current);
            } while (true);
        }

        private void IsBetter(Node current, Node next, State state, bool isReverse) {
            if (isReverse) {
                if (current.Elevation - next.Elevation <= 1 && next.Num > current.Num + 1) {
                    next.Num = current.Num + 1;
                    state.Heap.Adjust(next);
                }
            } else if (next.Elevation - current.Elevation <= 1 && next.Num > current.Num + 1) {
                next.Num = current.Num + 1;
                state.Heap.Adjust(next);
            }
        }

        private void SetHeap(State state) {
            state.Heap = new BinaryHeap_Min(state.Grid.Length);
            foreach (var node in state.Grid) {
                if (node != state.Start) node.Num = int.MaxValue;
                state.Heap.Add(node);
            }
        }

        private State GetState(List<string> input) {
            var state = new State() { AnyStart = new List<Node>() };
            state.Grid = new Node[input[0].Length, input.Count];
            for (int y = 0; y < input.Count; y++) {
                var line = input[y];
                for (int x = 0; x < line.Length; x++) {
                    var node = new Node() { Digit = line[x], X = x, Y = y };
                    state.Grid[x, y] = node;
                    if (node.Digit == 'S') {
                        state.Start = node;
                        node.Digit = 'a';
                        node.Elevation = 1;
                    } else if (node.Digit == 'E') {
                        state.End = node;
                        node.Digit = 'z';
                        node.Elevation = 26;
                    } else {
                        node.Elevation = (int)node.Digit - 96;
                    }
                    if (node.Elevation == 1) state.AnyStart.Add(node);
                }
            }
            return state;
        }

        private class Node : BinaryHeap_Min.Node {
            public int Elevation { get; set; }
            public char Digit { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public bool Explored { get; set; }
        }

        private class State {
            public Node[,] Grid { get; set; }
            public Node Start { get; set; }
            public Node End { get; set; }
            public BinaryHeap_Min Heap { get; set; }
            public List<Node> AnyStart { get; set; }
        }
    }
}
