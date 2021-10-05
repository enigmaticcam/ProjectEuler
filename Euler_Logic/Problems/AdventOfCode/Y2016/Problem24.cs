using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem24 : AdventOfCodeBase {
        private PowerAll _powerOf2;
        private Node[,] _grid;
        private List<Node> _nodes;
        private List<Node> _valueNodes;
        private Dictionary<int, Dictionary<int, Edge>> _edges;

        public override string ProblemName => "Advent of Code 2016: 24";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            _powerOf2 = new PowerAll(2);
            GetGrid(input);
            SetEdges();
            FindBest(0, 0, _valueNodes.Count, -1, true, false);
            return _best;
        }

        private int Answer2(List<string> input) {
            _powerOf2 = new PowerAll(2);
            GetGrid(input);
            SetEdges();
            FindBest(0, 0, _valueNodes.Count, -1, true, true);
            return _best;
        }

        private int _best = int.MaxValue;
        private void FindBest(ulong bits, int distance, int remaining, int lastIndex, bool isFirst, bool doesRobotReturn) {
            for (int index = 0; index < _valueNodes.Count; index++) {
                var bit = _powerOf2.GetPower(index);
                if ((bits & bit) == 0 && (!isFirst || _valueNodes[index].Value == 0)) {
                    var finalDistance = (lastIndex == -1 ? 0 : distance + _edges[lastIndex][index].Distance);
                    if (remaining == 1) {
                        if (doesRobotReturn) {
                            finalDistance += _edges[index][_valueNodes.IndexOf(_valueNodes.Where(x => x.Value == 0).First())].Distance;
                        }
                        if (finalDistance < _best) {
                            _best = finalDistance;
                        }
                    } else {
                        FindBest(bits + bit, finalDistance, remaining - 1, index, false, doesRobotReturn);
                    }
                }
            }
        }

        private void SetEdges() {
            _edges = new Dictionary<int, Dictionary<int, Edge>>();
            for (int index = 0; index < _valueNodes.Count; index++) {
                SetEdges(index);
            }
        }

        private void AddEdge(Edge edge, int index1, int index2) {
            if (!_edges.ContainsKey(index1)) {
                _edges.Add(index1, new Dictionary<int, Edge>());
            }
            if (!_edges.ContainsKey(index2)) {
                _edges.Add(index2, new Dictionary<int, Edge>());
            }
            if (!_edges[index1].ContainsKey(index2)) {
                _edges[index1].Add(index2, edge);
            }
            if (!_edges[index2].ContainsKey(index1)) {
                _edges[index2].Add(index1, edge);
            }
        }

        private void SetEdges(int index1) {
            int valueCount = 1;
            var node1 = _valueNodes[index1];
            var unvisited = _nodes.Where(x => !x.IsWall).ToList();
            unvisited.ForEach(x => x.Distance = int.MaxValue);
            node1.Distance = 0;
            do {
                var current = unvisited.OrderBy(node => node.Distance).First();
                if (current.HasValue && current.Value != node1.Value) {
                    var edge = new Edge() { Distance = current.Distance, Node1 = node1, Node2 = current };
                    AddEdge(edge, index1, _valueNodes.IndexOf(current));
                    valueCount++;
                    if (valueCount == _valueNodes.Count) {
                        break;
                    }
                }
                var x = current.X;
                var y = current.Y;

                if (x > 0 && !_grid[x - 1, y].IsWall && _grid[x - 1, y].Distance > current.Distance + 1) {
                    _grid[x - 1, y].Distance = current.Distance + 1;
                }
                if (x < _grid.GetUpperBound(0) && !_grid[x + 1, y].IsWall && _grid[x + 1, y].Distance > current.Distance + 1) {
                    _grid[x + 1, y].Distance = current.Distance + 1;
                }
                if (y > 0 && !_grid[x, y - 1].IsWall && _grid[x, y - 1].Distance > current.Distance + 1) {
                    _grid[x, y - 1].Distance = current.Distance + 1;
                }
                if (y < _grid.GetUpperBound(1) && !_grid[x, y + 1].IsWall && _grid[x, y + 1].Distance > current.Distance + 1) {
                    _grid[x, y + 1].Distance = current.Distance + 1;
                }
                unvisited.Remove(current);
            } while (true);
        }
        
        private void GetGrid(List<string> input) {
            _grid = new Node[input[0].Length, input.Count];
            _nodes = new List<Node>();
            _valueNodes = new List<Node>();
            for (int x = 0; x < input[0].Length; x++) {
                for (int y = 0; y < input.Count; y++) {
                    var node = new Node() { X = x, Y = y };
                    _grid[x, y] = node;
                    _nodes.Add(node);
                    switch (input[y][x]) {
                        case '#':
                            node.IsWall = true;
                            break;
                        case '.':
                            break;
                        case '0':
                            node.HasValue = true;
                            _valueNodes.Add(node);
                            break;
                        case '1':
                            node.HasValue = true;
                            node.Value = 1;
                            _valueNodes.Add(node);
                            break;
                        case '2':
                            node.HasValue = true;
                            node.Value = 2;
                            _valueNodes.Add(node);
                            break;
                        case '3':
                            node.HasValue = true;
                            node.Value = 3;
                            _valueNodes.Add(node);
                            break;
                        case '4':
                            node.HasValue = true;
                            node.Value = 4;
                            _valueNodes.Add(node);
                            break;
                        case '5':
                            node.HasValue = true;
                            node.Value = 5;
                            _valueNodes.Add(node);
                            break;
                        case '6':
                            node.HasValue = true;
                            node.Value = 6;
                            _valueNodes.Add(node);
                            break;
                        case '7':
                            node.HasValue = true;
                            node.Value = 7;
                            _valueNodes.Add(node);
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
        }

        private List<string> TestInput() {
            return new List<string>() {
                "###########",
                "#0.1.....2#",
                "#.#######.#",
                "#4.......3#",
                "###########"
            };
        }

        private class Node {
            public int X { get; set; }
            public int Y { get; set; }
            public int Distance { get; set; }
            public bool HasValue { get; set; }
            public int Value { get; set; }
            public bool IsWall { get; set; }
        }

        private class Edge {
            public Node Node1 { get; set; }
            public Node Node2 { get; set; }
            public int Distance { get; set; }
        }
    }
}
