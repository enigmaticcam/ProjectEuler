using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem15 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, Node>> _grid;
        private List<Node> _nodes;
        private int _maxX;
        private int _maxY;

        public override string ProblemName {
            get { return "Advent of Code 2021: 15"; }
        }

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        private int Answer1() {
            GetGrid(Input());
            SetEdges();
            return FindShortest();
        }

        private int Answer2() {
            GetGrid2(Input());
            SetEdges();
            return FindShortest();
        }

        private int FindShortest() {
            var heap = new BinaryHeap_Min();
            _nodes.ForEach(x => {
                heap.Add(x);
                x.Num = int.MaxValue;
            });
            _grid[0][0].Num = 0;
            heap.Adjust(_grid[0][0]);
            do {
                var next = (Node)heap.Top;
                if (next.X == _maxX && next.Y == _maxY) {
                    return next.Num;
                }
                foreach (var edge in next.Edges) {
                    if (edge.Risk + next.Num < edge.Num) {
                        edge.Num = edge.Risk + next.Num;
                        heap.Adjust(edge);
                    }
                }
                heap.Remove(next);
            } while (true);
        }

        private void SetEdges() {
            for (int x = 0; x <= _maxX; x++) {
                for (int y = 0; y <= _maxY; y++) {
                    var node = _grid[x][y];
                    if (x > 0) {
                        node.Edges.Add(_grid[x - 1][y]);
                    }
                    if (x < _maxX) {
                        node.Edges.Add(_grid[x + 1][y]);
                    }
                    if (y > 0) {
                        node.Edges.Add(_grid[x][y - 1]);
                    }
                    if (y < _maxY) {
                        node.Edges.Add(_grid[x][y + 1]);
                    }
                }
            }
        }

        private void GetGrid(List<string> input) {
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            _nodes = new List<Node>();
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    var node = new Node() { 
                        X = x, 
                        Y = y, 
                        Risk = Convert.ToInt32(new string(new char[1] { input[y][x] })),
                        Edges = new List<Node>()
                    };
                    AddNode(node);
                }
            }
            _maxX = _grid.Keys.Max();
            _maxY = _grid[0].Keys.Max();
        }

        private void GetGrid2(List<string> input) {
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            _nodes = new List<Node>();
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    var node = new Node() {
                        X = x,
                        Y = y,
                        Risk = Convert.ToInt32(new string(new char[1] { input[y][x] })),
                        Edges = new List<Node>()
                    };
                    AddDupes(x, y, Convert.ToInt32(new string(new char[1] { input[y][x] })), input.Count);
                }
            }
            _maxX = _grid.Keys.Max();
            _maxY = _grid[0].Keys.Max();
        }

        private void AddDupes(int x, int y, int baseRisk, int size) {
            for (int countY = 0; countY < 5; countY++) {
                for (int countX = 0; countX < 5; countX++) {
                    var risk = countY + countX + baseRisk;
                    while (risk > 9) {
                        risk -= 9;
                    }
                    var node = new Node() {
                        X = x + countX * size,
                        Y = y + countY * size,
                        Edges = new List<Node>(),
                        Risk = risk
                    };
                    AddNode(node);
                }
            }
        }

        private void AddNode(Node node) {
            if (!_grid.ContainsKey(node.X)) {
                _grid.Add(node.X, new Dictionary<int, Node>());
            }
            _grid[node.X].Add(node.Y, node);
            _nodes.Add(node);
        }

        private class Node : BinaryHeap_Min.Node {
            public int X { get; set; }
            public int Y { get; set; }
            public int Risk { get; set; }
            public List<Node> Edges { get; set; }
        }
    }
}
