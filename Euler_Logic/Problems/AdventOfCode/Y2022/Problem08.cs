using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem08 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 8";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var trees = GetTrees(input);
            SetIsVisible(trees);
            return GetCount(trees);
        }

        private long Answer2(List<string> input) {
            var trees = GetTrees(input);
            SetDistance(trees);
            return FindBest(trees);
        }

        private void SetIsVisible(Tree[,] trees) {
            SetIsVisible_Horizontal(trees);
            SetIsVisible_Vertial(trees);
        }

        private void SetIsVisible_Horizontal(Tree[,] trees) {
            for (int y = 0; y <= trees.GetUpperBound(1); y++) {
                int highest = -1;
                for (int x = 0; x <= trees.GetUpperBound(0); x++) {
                    var tree = trees[x, y];
                    if (tree.Height > highest) {
                        highest = tree.Height;
                        tree.IsVisible = true;
                    }
                }
                highest = -1;
                for (int x = trees.GetUpperBound(0); x >= 0; x--) {
                    var tree = trees[x, y];
                    if (tree.Height > highest) {
                        highest = tree.Height;
                        tree.IsVisible = true;
                    }
                }
            }
        }

        private void SetIsVisible_Vertial(Tree[,] trees) {
            for (int x = 0; x <= trees.GetUpperBound(0); x++) {
                int highest = -1;
                for (int y = 0; y <= trees.GetUpperBound(1); y++) {
                    var tree = trees[x, y];
                    if (tree.Height > highest) {
                        highest = tree.Height;
                        tree.IsVisible = true;
                    }
                }
                highest = -1;
                for (int y = trees.GetUpperBound(1); y >= 0; y--) {
                    var tree = trees[x, y];
                    if (tree.Height > highest) {
                        highest = tree.Height;
                        tree.IsVisible = true;
                    }
                }
            }
        }

        private void SetDistance(Tree[,] trees) {
            SetDistance_Horizontal(trees);
        }

        private void SetDistance_Horizontal(Tree[,] trees) {
            for (int y = 0; y <= trees.GetUpperBound(1); y++) {
                trees[0, y].BlockedDistance = 1;
                for (int x = 1; x <= trees.GetUpperBound(0); x++) {
                    var tree = trees[x, y];
                    var prior = trees[x - 1, y];
                    if (tree.Height <= prior.Height) {
                        tree.Distance *= 1;
                        tree.BlockedDistance = 1;
                    } else {
                        var distance = GetDistance_Recursive(trees, x - 1, y, enumDirection.Left, tree.Height) + 1;
                        tree.Distance *= distance;
                        tree.BlockedDistance = distance;
                    }
                }
                trees[trees.GetUpperBound(0), y].BlockedDistance = 1;
                for (int x = trees.GetUpperBound(0) - 1; x >= 0; x--) {
                    var tree = trees[x, y];
                    var prior = trees[x + 1, y];
                    if (tree.Height <= prior.Height) {
                        tree.Distance *= 1;
                        tree.BlockedDistance = 1;
                    } else {
                        var distance = GetDistance_Recursive(trees, x + 1, y, enumDirection.Right, tree.Height) + 1;
                        tree.Distance *= distance;
                        tree.BlockedDistance = distance;
                    }
                }
            }
            for (int x = 0; x <= trees.GetUpperBound(0); x++) {
                trees[x, 0].BlockedDistance = 1;
                for (int y = 1; y <= trees.GetUpperBound(1); y++) {
                    var tree = trees[x, y];
                    var prior = trees[x, y - 1];
                    if (tree.Height <= prior.Height) {
                        tree.Distance *= 1;
                        tree.BlockedDistance = 1;
                    } else {
                        var distance = GetDistance_Recursive(trees, x, y - 1, enumDirection.Up, tree.Height) + 1;
                        tree.Distance *= distance;
                        tree.BlockedDistance = distance;
                    }
                }
                trees[x, trees.GetUpperBound(1)].BlockedDistance = 1;
                for (int y = trees.GetUpperBound(1) - 1; y >= 0; y--) {
                    var tree = trees[x, y];
                    var prior = trees[x, y + 1];
                    if (tree.Height <= prior.Height) {
                        tree.Distance *= 1;
                        tree.BlockedDistance = 1;
                    } else {
                        var distance = GetDistance_Recursive(trees, x, y + 1, enumDirection.Down, tree.Height) + 1;
                        tree.Distance *= distance;
                        tree.BlockedDistance = distance;
                    }
                }
            }
        }

        private long FindBest(Tree[,] trees) {
            long best = 0;
            for (int x = 1; x < trees.GetUpperBound(0); x++) {
                for (int y = 1; y < trees.GetUpperBound(1); y++) {
                    var tree = trees[x, y];
                    if (tree.Distance > best) best = tree.Distance;
                }
            }
            return best;
        }

        private int GetDistance_Recursive(Tree[,] trees, int x, int y, enumDirection direction, int height) {
            var tree = trees[x, y];
            Tree prior = null;
            switch (direction) {
                case enumDirection.Up:
                    if (y - tree.BlockedDistance == -1) return 0;
                    prior = trees[x, y - tree.BlockedDistance];
                    break;
                case enumDirection.Down:
                    if (y + tree.BlockedDistance > trees.GetUpperBound(1)) return 0;
                    prior = trees[x, y + tree.BlockedDistance];
                    break;
                case enumDirection.Right:
                    if (x + tree.BlockedDistance > trees.GetUpperBound(1)) return 0;
                    prior = trees[x + tree.BlockedDistance, y];
                    break;
                case enumDirection.Left:
                    if (x - tree.BlockedDistance == -1) return 0;
                    prior = trees[x - tree.BlockedDistance, y];
                    break;
            }
            if (prior.Height >= height) {
                return tree.BlockedDistance;
            } else {
                int nextDistance = 0;
                switch (direction) {
                    case enumDirection.Up:
                        nextDistance = GetDistance_Recursive(trees, x, y - prior.BlockedDistance, direction, height);
                        break;
                    case enumDirection.Down:
                        nextDistance = GetDistance_Recursive(trees, x, y + prior.BlockedDistance, direction, height);
                        break;
                    case enumDirection.Right:
                        nextDistance = GetDistance_Recursive(trees, x + prior.BlockedDistance, y, direction, height);
                        break;
                    case enumDirection.Left:
                        nextDistance = GetDistance_Recursive(trees, x - prior.BlockedDistance, y, direction, height);
                        break;
                }
                return nextDistance + prior.BlockedDistance;
            }
        }

        private int GetCount(Tree[,] trees) {
            int count = 0;
            foreach (var tree in trees) {
                if (tree.IsVisible) count++;
            }
            return count;
        }

        private Tree[,] GetTrees(List<string> input) {
            var trees = new Tree[input[0].Length, input.Count];
            int y = 0;
            foreach (var line in input) {
                int x = 0;
                foreach (var digit in line) {
                    trees[x, y] = new Tree() {
                        Height = Convert.ToInt32(new string(new char[] { digit })),
                        IsVisible = false,
                        Distance = 1
                    };
                    x++;
                }
                y++;
            }
            return trees;
        }

        private class Tree {
            public int Height { get; set; }
            public bool IsVisible { get; set; }
            public int BlockedDistance { get; set; }
            public long Distance { get; set; }
        }

        private enum enumDirection {
            Up,
            Down,
            Left,
            Right
        }
    }
}
