using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem22 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, Dictionary<enumTool, DijNode>>> _dijGrid;
        private Dictionary<int, Dictionary<int, Node>> _grid;
        private List<Node> _nodes;
        private ulong _depth;
        private int _targetX;
        private int _targetY;
        private LinkedList<DijNode> _activeUnvisited;
        private Dictionary<enumRegionType, List<enumTool>> _regionToTool;

        private enum enumRegionType {
            Rocky,
            Wet,
            Narrow
        }

        private enum enumTool {
            Torch,
            Climbing,
            Neither
        }

        public override string ProblemName {
            get { return "Advent of Code 2018: 22"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            _dijGrid = new Dictionary<int, Dictionary<int, Dictionary<enumTool, DijNode>>>();
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            GetTargetAndDepth(input);
            return GetRisk();
        }

        private int Answer2(List<string> input) {
            _dijGrid = new Dictionary<int, Dictionary<int, Dictionary<enumTool, DijNode>>>();
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            GetTargetAndDepth(input);
            SetRegionToTool();
            return FindFastest();
        }

        private void SetRegionToTool() {
            _regionToTool = new Dictionary<enumRegionType, List<enumTool>>();
            _regionToTool.Add(enumRegionType.Narrow, new List<enumTool>() { enumTool.Torch, enumTool.Neither });
            _regionToTool.Add(enumRegionType.Rocky, new List<enumTool>() { enumTool.Climbing, enumTool.Torch });
            _regionToTool.Add(enumRegionType.Wet, new List<enumTool>() { enumTool.Climbing, enumTool.Neither });
        }

        private int FindFastest() {
            AddNode(0, 0, enumTool.Torch);
            var start = _dijGrid[0][0][enumTool.Torch];
            start.Distance = 0;
            _activeUnvisited = new LinkedList<DijNode>();
            _activeUnvisited.AddLast(start);
            do {
                var currentDij = GetShortest();
                var x = currentDij.X;
                var y = currentDij.Y;
                var tool = currentDij.ToolType;
                if (x == _targetX && y == _targetY && tool == enumTool.Torch) {
                    return currentDij.Distance;
                }
                AdjustDistance(currentDij, tool, x + 1, y);
                AdjustDistance(currentDij, tool, x, y + 1);
                if (x > 0) {
                    AdjustDistance(currentDij, tool, x - 1, y);
                }
                if (y > 0) {
                    AdjustDistance(currentDij, tool, x, y - 1);
                }
                _activeUnvisited.Remove(currentDij);
                currentDij.IsVisited = true;
            } while (true);
        }

        private void AdjustDistance(DijNode currentDij, enumTool currentTool, int x, int y) {
            var currentNode = GetNode(currentDij.X, currentDij.Y);
            var nextNode = GetNode(x, y);
            var nextTool = _regionToTool[currentNode.RegionType][0];
            if (_regionToTool[nextNode.RegionType][0] == nextTool || _regionToTool[nextNode.RegionType][1] == nextTool) {
                int distance = currentDij.Distance + (nextTool == currentTool ? 1 : 8);
                var nextDij = GetNode(x, y, nextTool);
                if (nextDij.Distance > distance) {
                    nextDij.Distance = distance;
                    nextDij.Prior = currentDij;
                    if (!nextDij.IsActive) {
                        nextDij.IsActive = true;
                        _activeUnvisited.AddLast(nextDij);
                    }
                }
            }

            nextTool = _regionToTool[currentNode.RegionType][1];
            if (_regionToTool[nextNode.RegionType][0] == nextTool || _regionToTool[nextNode.RegionType][1] == nextTool) {
                int distance = currentDij.Distance + (nextTool == currentTool ? 1 : 8);
                var nextDij = GetNode(x, y, nextTool);
                if (nextDij.Distance > distance) {
                    nextDij.Distance = distance;
                    nextDij.Prior = currentDij;
                    if (!nextDij.IsActive) {
                        nextDij.IsActive = true;
                        _activeUnvisited.AddLast(nextDij);
                    }
                }
            }
        }

        private Node GetNode(int x, int y) {
            if (!_grid.ContainsKey(x) || !_grid[x].ContainsKey(y)) {
                AddNode(x, y);
            }
            return _grid[x][y];
        }

        private DijNode GetNode(int x, int y, enumTool tool) {
            if (!_dijGrid.ContainsKey(x) || !_dijGrid[x].ContainsKey(y) || !_dijGrid[x][y].ContainsKey(tool)) {
                AddNode(x, y, tool);
            }
            return _dijGrid[x][y][tool];
        }

        private DijNode GetShortest() {
            int shortestNum = int.MaxValue;
            DijNode shortestNode = null;
            foreach (var node in _activeUnvisited) {
                if (node.Distance < shortestNum) {
                    shortestNum = node.Distance;
                    shortestNode = node;
                }
            }
            return shortestNode;
        }

        private int GetRisk() {
            int sum = 0;
            for (int x = 0; x <= _targetX; x++) {
                for (int y = 0; y <= _targetY; y++) {
                    var node = GetNode(x, y);
                    sum += (int)node.RegionType;
                }
            }
            return sum;
        }

        private void AddNode(int x, int y, enumTool tool) {
            if (!_dijGrid.ContainsKey(x)) {
                _dijGrid.Add(x, new Dictionary<int, Dictionary<enumTool, DijNode>>());
            }
            if (!_dijGrid[x].ContainsKey(y)) {
                _dijGrid[x].Add(y, new Dictionary<enumTool, DijNode>());
            }
            var node = new DijNode() { X = x, Y = y, ToolType = tool, Distance = int.MaxValue };
            _dijGrid[x][y].Add(tool, node);
        }

        private void AddNode(int x, int y) {
            if (!_grid.ContainsKey(x)) {
                _grid.Add(x, new Dictionary<int, Node>());
            }
            var node = new Node() { X = x, Y = y };
            _grid[x].Add(y, node);
            SetNodeStatus(node);
        }

        private void SetNodeStatus(Node node) {
            if (node.X == 0 && node.Y == 0) {
                node.Gelologic = 0;
            } else if (node.X == 0) {
                node.Gelologic = (ulong)node.Y * 48271;
            } else if (node.Y == 0) {
                node.Gelologic = (ulong)node.X * 16807;
            } else if (node.X == _targetX && node.Y == _targetY) {
                node.Gelologic = 0;
            } else {
                var minusX = GetNode(node.X - 1, node.Y);
                var minusY = GetNode(node.X, node.Y - 1);
                node.Gelologic = minusX.Erosion * minusY.Erosion;
            }
            node.Erosion = (node.Gelologic + _depth) % 20183;
            node.RegionType = (enumRegionType)(node.Erosion % 3);
        }

        private void GetTargetAndDepth(List<string> input) {
            _depth = Convert.ToUInt64(input[0].Split(' ')[1]);
            var split = input[1].Split(' ')[1].Split(',');
            _targetX = Convert.ToInt32(split[0]);
            _targetY = Convert.ToInt32(split[1]);
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "depth: 510",
                "target: 10,10"
            };
        }

        private List<string> Test2Input() { // Answer: 1070
            return new List<string>() {
                "depth: 5616",
                "target: 10,785"
            };
        }

        private class Node {
            public enumRegionType RegionType { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public ulong Gelologic { get; set; }
            public ulong Erosion { get; set; }
            public enumTool ToolType { get; set; }
        }

        private class DijNode {
            public int X { get; set; }
            public int Y { get; set; }
            public enumTool ToolType { get; set; }
            public int Distance { get; set; }
            public DijNode Prior { get; set; }
            public bool IsVisited { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
