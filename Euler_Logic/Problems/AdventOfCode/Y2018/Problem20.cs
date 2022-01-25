using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem20 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, Node>> _grid;
        private BinaryHeap_Min _heap;

        private enum enumNodeType {
            Empty,
            Door,
            Wall,
            Start
        }

        public override string ProblemName {
            get { return "Advent of Code 2018: 20"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            _heap = new BinaryHeap_Min();
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            _grid.Add(0, new Dictionary<int, Node>());
            _grid[0].Add(0, new Node() { NodeType = enumNodeType.Start });
            BuildGrid(0, 0, 0, input[0]);
            return FindMostDoors();
        }

        private int Answer2(List<string> input) {
            _heap = new BinaryHeap_Min();
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            _grid.Add(0, new Dictionary<int, Node>());
            _grid[0].Add(0, new Node() { NodeType = enumNodeType.Start });
            BuildGrid(0, 0, 0, input[0]);
            FindMostDoors();
            return Find1000Doors();
        }

        private int Find1000Doors() {
            int count = 0;
            foreach (var x in _grid) {
                foreach (var y in x.Value) {
                    count += (y.Value.DoorCount >= 1000 && y.Value.NodeType == enumNodeType.Empty ? 1 : 0);
                }
            }
            return count;
        }

        private int FindMostDoors() {
            int highest = 0;
            var visited = GetNodes();
            int count = 0;
            do {
                //var currentNode = FindShortestDistance(visited);
                var currentNode = (Node)_heap.Top;
                var nodeType = GetNodeType(currentNode.X - 1, currentNode.Y);
                if (nodeType == enumNodeType.Empty || nodeType == enumNodeType.Door) {
                    var node = _grid[currentNode.X - 1][currentNode.Y];
                    if (node.Num > currentNode.Num + 1) {
                        node.Num = currentNode.Num + 1;
                        node.DoorCount = currentNode.DoorCount + (currentNode.NodeType == enumNodeType.Door ? 1 : 0);
                        if (node.DoorCount > highest) {
                            highest = node.DoorCount;
                        }
                        _heap.Adjust(node);
                    }
                }
                nodeType = GetNodeType(currentNode.X + 1, currentNode.Y);
                if (nodeType == enumNodeType.Empty || nodeType == enumNodeType.Door) {
                    var node = _grid[currentNode.X + 1][currentNode.Y];
                    if (node.Num > currentNode.Num + 1) {
                        node.Num = currentNode.Num + 1;
                        node.DoorCount = currentNode.DoorCount + (currentNode.NodeType == enumNodeType.Door ? 1 : 0);
                        if (node.DoorCount > highest) {
                            highest = node.DoorCount;
                        }
                        _heap.Adjust(node);
                    }
                }
                nodeType = GetNodeType(currentNode.X, currentNode.Y - 1);
                if (nodeType == enumNodeType.Empty || nodeType == enumNodeType.Door) {
                    var node = _grid[currentNode.X][currentNode.Y - 1];
                    if (node.Num > currentNode.Num + 1) {
                        node.Num = currentNode.Num + 1;
                        node.DoorCount = currentNode.DoorCount + (currentNode.NodeType == enumNodeType.Door ? 1 : 0);
                        if (node.DoorCount > highest) {
                            highest = node.DoorCount;
                        }
                        _heap.Adjust(node);
                    }
                }
                nodeType = GetNodeType(currentNode.X, currentNode.Y + 1);
                if (nodeType == enumNodeType.Empty || nodeType == enumNodeType.Door) {
                    var node = _grid[currentNode.X][currentNode.Y + 1];
                    if (node.Num > currentNode.Num + 1) {
                        node.Num = currentNode.Num + 1;
                        node.DoorCount = currentNode.DoorCount + (currentNode.NodeType == enumNodeType.Door ? 1 : 0);
                        if (node.DoorCount > highest) {
                            highest = node.DoorCount;
                        }
                        _heap.Adjust(node);
                    }
                }
                _heap.Remove(currentNode);
                count++;
            } while (count < visited.Count);
            return highest;
        }

        private List<Node> GetNodes() {
            var nodes = new List<Node>();
            foreach (var x in _grid) {
                foreach (var y in x.Value) {
                    var nodeType = GetNodeType(x.Key, y.Key);
                    if (nodeType == enumNodeType.Door || nodeType == enumNodeType.Empty || nodeType == enumNodeType.Start) {
                        var node = _grid[x.Key][y.Key];
                        nodes.Add(node);
                        node.Num = (nodeType == enumNodeType.Start ? 0 : int.MaxValue);
                        _heap.Add(node);
                        if (node.Num == 0) _heap.Adjust(node);
                    }
                }
            }
            return nodes;
        }

        private int BuildGrid(int x, int y, int index, string digits) {
            int startX = x;
            int startY = y;
            do {
                switch (digits[index]) {
                    case 'E':
                        AddNode(x + 1, y, enumNodeType.Door);
                        AddNode(x + 2, y, enumNodeType.Empty);
                        x += 2;
                        break;
                    case 'W':
                        AddNode(x - 1, y, enumNodeType.Door);
                        AddNode(x - 2, y, enumNodeType.Empty);
                        x -= 2;
                        break;
                    case 'S':
                        AddNode(x, y + 1, enumNodeType.Door);
                        AddNode(x, y + 2, enumNodeType.Empty);
                        y += 2;
                        break;
                    case 'N':
                        AddNode(x, y - 1, enumNodeType.Door);
                        AddNode(x, y - 2, enumNodeType.Empty);
                        y -= 2;
                        break;
                    case '(':
                        index = BuildGrid(x, y, index + 1, digits);
                        break;
                    case '|':
                        x = startX;
                        y = startY;
                        break;
                    case ')':
                        return index;
                }
                index++;
            } while (index < digits.Length);
            return index;
        }

        private void AddNode(int x, int y, enumNodeType nodeType) {
            if (!_grid.ContainsKey(x)) {
                _grid.Add(x, new Dictionary<int, Node>());
            }
            if (!_grid[x].ContainsKey(y)) {
                _grid[x].Add(y, new Node() { NodeType = nodeType, X = x, Y = y });
            } else if (_grid[x][y].NodeType != nodeType) {
                throw new Exception();
            }
        }

        private enumNodeType GetNodeType(int x, int y) {
            if (_grid.ContainsKey(x) && _grid[x].ContainsKey(y)) {
                return _grid[x][y].NodeType;
            } else {
                return enumNodeType.Wall;
            }
        }

        private class Node : BinaryHeap_Min.Node {
            public enumNodeType NodeType { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int DoorCount { get; set; }
        }
    }
}
