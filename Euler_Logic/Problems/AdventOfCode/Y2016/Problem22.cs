using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem22 : AdventOfCodeBase {
        private List<Node> _nodes;
        private Dictionary<int, Dictionary<int, Node>> _grid;
        private Node _football;

        public override string ProblemName => "Advent of Code 2016: 22";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetNodes(input);
            return CountViables();
        }

        private int Answer2(List<string> input) {
            GetNodes(input);
            SetGrid();
            return GetBest();
        }

        private int GetBest() {
            int count = 0;
            _football = _grid[_grid.Keys.Max()][0];
            _football.IsFootball = true;
            var zero = FindZeroNode();
            FindShortest(zero.X, zero.Y, _football.X, 0, false);
            count += PerformMovement(_grid[_football.X][0]);
            do {
                zero = FindZeroNode();
                FindShortest(_football.X, _football.Y, 0, 0, false);
                var moves = GetMovement(_grid[0][0]);
                var nextNode = moves[1];
                FindShortest(zero.X, zero.Y, nextNode.X, nextNode.Y, true);
                count += PerformMovement(nextNode);
                nextNode.Used = _football.Used;
                _football.Used = 0;
                nextNode.IsFootball = true;
                _football.IsFootball = false;
                _football = nextNode;
                count++;
            } while (_football.X != 0 || _football.Y != 0);
            return count;
        }

        private int PerformMovement(Node node) {
            int count = 0;
            var nodes = GetMovement(node);
            foreach (var current in nodes) {
                if (current.Path != null) {
                    current.Path.Used = current.Used;
                    current.Used = 0;
                    if (current.IsFootball) {
                        current.Path.IsFootball = true;
                        current.IsFootball = false;
                        _football = current.Path;
                    }
                    count++;
                }
            }
            return count;
        }

        private List<Node> GetMovement(Node node) {
            var nodes = new List<Node>();
            do {
                nodes.Insert(0, node);
                node = node.Path;
            } while (node != null);
            return nodes;
        }

        private Node FindZeroNode() {
            foreach (var node in _nodes) {
                if (node.Used == 0) {
                    return node;
                }
            }
            return null;
        }

        private int FindShortest(int startX, int startY, int endX, int endY, bool doNotMoveFootball) {
            foreach (var node in _nodes) {
                node.Distance = int.MaxValue;
            }
            var unvisited = new List<Node>(_nodes);
            _grid[startX][startY].Distance = 0;
            _grid[startX][startY].Path = null;
            do {
                var currentNode = unvisited.OrderBy(node => node.Distance).First();
                if (currentNode.X == endX && currentNode.Y == endY) {
                    return currentNode.Distance;
                }
                int x = currentNode.X;
                int y = currentNode.Y;
                if (_grid.ContainsKey(x - 1) && _grid[x - 1].ContainsKey(y)) {
                    var toNode = _grid[x - 1][y];
                    if (toNode.Size >= currentNode.Used && toNode.Distance > currentNode.Distance + 1) {
                        if (!doNotMoveFootball || !toNode.IsFootball) {
                            toNode.Distance = currentNode.Distance + 1;
                            toNode.Path = currentNode;
                        }
                    }
                }
                if (_grid.ContainsKey(x + 1) && _grid[x + 1].ContainsKey(y)) {
                    var toNode = _grid[x + 1][y];
                    if (toNode.Size >= currentNode.Used && toNode.Distance > currentNode.Distance + 1) {
                        if (!doNotMoveFootball || !toNode.IsFootball) {
                            toNode.Distance = currentNode.Distance + 1;
                            toNode.Path = currentNode;
                        }
                    }
                }
                if (_grid.ContainsKey(x) && _grid[x].ContainsKey(y - 1)) {
                    var toNode = _grid[x][y - 1];
                    if (toNode.Size >= currentNode.Used && toNode.Distance > currentNode.Distance + 1) {
                        if (!doNotMoveFootball || !toNode.IsFootball) {
                            toNode.Distance = currentNode.Distance + 1;
                            toNode.Path = currentNode;
                        }
                    }
                }
                if (_grid.ContainsKey(x) && _grid[x].ContainsKey(y + 1)) {
                    var toNode = _grid[x][y + 1];
                    if (toNode.Size >= currentNode.Used && toNode.Distance > currentNode.Distance + 1) {
                        if (!doNotMoveFootball || !toNode.IsFootball) {
                            toNode.Distance = currentNode.Distance + 1;
                            toNode.Path = currentNode;
                        }
                    }
                }
                unvisited.Remove(currentNode);
            } while (true);
        }

        private void SetGrid() {
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            foreach (var node in _nodes) {
                if (!_grid.ContainsKey(node.X)) {
                    _grid.Add(node.X, new Dictionary<int, Node>());
                }
                _grid[node.X].Add(node.Y, node);
            }
        }

        private int CountViables() {
            int count = 0;
            foreach (var nodeA in _nodes) {
                foreach (var nodeB in _nodes) {
                    if (nodeA != nodeB && nodeA.Used != 0 && nodeA.Used <= nodeB.Available) {
                        count++;
                    }
                }
            }
            return count;
        }

        private void GetNodes(List<string> input) {
            _nodes = new List<Node>();
            for (int index = 2; index < input.Count; index++) {
                var node = new Node();
                var split = input[index].Split(' ').ToList();
                split = split.Where(x => x.Length > 0).ToList();
                node.Available = Convert.ToInt32(split[3].Replace("T", ""));
                node.Size = Convert.ToInt32(split[1].Replace("T", ""));
                node.Used = Convert.ToInt32(split[2].Replace("T", ""));
                node.UsePerc = Convert.ToInt32(split[4].Replace("%", ""));
                var pointSplit = split[0].Split('-');
                node.X = Convert.ToInt32(pointSplit[1].Replace("x", ""));
                node.Y = Convert.ToInt32(pointSplit[2].Replace("y", ""));
                _nodes.Add(node);
            }
        }

        private class Node {
            public int X { get; set; }
            public int Y { get; set; }
            public int Size { get; set; }
            public int Used { get; set; }
            public int Available { get; set; }
            public int UsePerc { get; set; }
            public int Distance { get; set; }
            public Node Path { get; set; }
            public bool IsFootball { get; set; }
        }

        private class Point {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
