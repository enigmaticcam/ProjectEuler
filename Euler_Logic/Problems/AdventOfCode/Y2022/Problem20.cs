using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem20 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 20";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private long Answer1(List<string> input) {
            var movements = GetMovements(input);
            var state = SetLists(movements);
            PerformOrder(state, 1);
            return GetSum(state);
        }

        private long Answer2(List<string> input) {
            var movements = GetMovements(input);
            ApplyDecryptionKey(movements);
            var state = SetLists(movements);
            PerformOrder(state, 10);
            return GetSum(state);
        }

        private void ApplyDecryptionKey(List<long> movements) {
            for (int index = 0; index < movements.Count; index++) {
                movements[index] *= 811589153;
            }
        }

        private long GetSum(State state) {
            long sum = 0;
            var nextNode = GetNext(1000, state.Zero);
            sum += nextNode.Number;

            nextNode = GetNext(1000, nextNode);
            sum += nextNode.Number;

            nextNode = GetNext(1000, nextNode);
            sum += nextNode.Number;

            return sum;
        }

        private Node GetNext(int totalCount, Node node) {
            for (int count = 1; count <= totalCount; count++) {
                node = node.Next;
            }
            return node;
        }

        private void PerformOrder(State state, int rounds) {
            for (int round = 1; round <= rounds; round++) {
                int count = 1;
                foreach (var node in state.MoveOrder) {
                    Move(state, node);
                    count++;
                }
            }
        }

        private void Move(State state, Node node) {
            int totalCount = (int)(node.Number % (long)state.Size);
            if (node.Number != 0 && totalCount != 0) {
                int opposite = (totalCount < 0 ? totalCount + state.Size : totalCount - state.Size);
                if (Math.Abs(opposite) < Math.Abs(totalCount)) totalCount = opposite;
                var temp = node;
                var nextTop = node.Next;
                int count = 0;
                int direction = (totalCount < 0 ? -1 : 1);
                while (count != totalCount) {
                    count += direction;
                    if (totalCount < 0) {
                        temp = temp.Previous;
                    } else {
                        temp = temp.Next;
                    }
                }
                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
                if (totalCount < 0) {
                    node.Next = temp;
                    node.Previous = temp.Previous;
                } else {
                    node.Previous = temp;
                    node.Next = temp.Next;
                }
                node.Next.Previous = node;
                node.Previous.Next = node;
                if (state.Top == node) state.Top = nextTop;
            }
        }

        private State SetLists(List<long> movements) {
            var state = new State() { MoveOrder = new List<Node>() };
            Node prior = null;
            foreach (var num in movements) {
                var node = new Node() { Number = num };
                state.MoveOrder.Add(node);
                if (state.Top == null) {
                    state.Top = node;
                } else {
                    prior.Next = node;
                    node.Previous = prior;
                }
                prior = node;
                if (num == 0) state.Zero = node;
            }
            state.Top.Previous = prior;
            prior.Next = state.Top;
            state.Size = state.MoveOrder.Count - 1;
            return state;
        }

        private List<long> GetMovements(List<string> input) {
            return input.Select(x => Convert.ToInt64(x)).ToList();
        }

        private class State {
            public List<Node> MoveOrder { get; set; }
            public Node Top { get; set; }
            public Node Zero { get; set; }
            public int Size { get; set; }
        }

        private class Node {
            public long Number { get; set; }
            public Node Next { get; set; }
            public Node Previous { get; set; }
        }
    }
}
