using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem22 : AdventOfCodeBase {
        private DijNode[,,] _dijGrid;
        private ulong _depth;
        private int _targetX;
        private int _targetY;
        private Dictionary<enumRegionType, List<enumTool>> _regionToTool;
        private BinaryHeap_Min _heap;

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
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetTargetAndDepth(input);
            SetGrid(0, 0);
            return GetRisk();
        }

        private int Answer2(List<string> input) {
            GetTargetAndDepth(input);
            SetRegionToTool();
            SetGrid(800, 100); // lol, literally just guessed these until boundaries were not exceeded
            return FindFastest();
        }

        private void SetRegionToTool() {
            _regionToTool = new Dictionary<enumRegionType, List<enumTool>>();
            _regionToTool.Add(enumRegionType.Narrow, new List<enumTool>() { enumTool.Torch, enumTool.Neither });
            _regionToTool.Add(enumRegionType.Rocky, new List<enumTool>() { enumTool.Climbing, enumTool.Torch });
            _regionToTool.Add(enumRegionType.Wet, new List<enumTool>() { enumTool.Climbing, enumTool.Neither });
        }

        private int FindFastest() {
            var start = _dijGrid[0, 0, (int)enumTool.Torch];
            start.Num = 0;
            _heap.Adjust(start.Num);
            do {
                var currentDij = (DijNode)_heap.Top;
                var x = currentDij.X;
                var y = currentDij.Y;
                var tool = currentDij.ToolType;
                if (x == _targetX && y == _targetY && tool == enumTool.Torch) {
                    return currentDij.Num;
                }
                AdjustDistance(currentDij, tool, x + 1, y);
                AdjustDistance(currentDij, tool, x, y + 1);
                if (x > 0) {
                    AdjustDistance(currentDij, tool, x - 1, y);
                }
                if (y > 0) {
                    AdjustDistance(currentDij, tool, x, y - 1);
                }
                _heap.Remove(currentDij);
            } while (true);
        }

        private void AdjustDistance(DijNode currentDij, enumTool currentTool, int x, int y) {
            var currentNode = _dijGrid[currentDij.X, currentDij.Y, (int)currentTool];
            var nextTool = _regionToTool[currentNode.RegionType][0];
            var nextNode = _dijGrid[x, y, (int)nextTool];
            if (_regionToTool[nextNode.RegionType][0] == nextTool || _regionToTool[nextNode.RegionType][1] == nextTool) {
                int distance = currentDij.Num + (nextTool == currentTool ? 1 : 8);
                if (nextNode.Num > distance) {
                    nextNode.Num = distance;
                    nextNode.Prior = currentDij;
                    _heap.Adjust(nextNode);
                }
            }

            nextTool = _regionToTool[currentNode.RegionType][1];
            nextNode = _dijGrid[x, y, (int)nextTool];
            if (_regionToTool[nextNode.RegionType][0] == nextTool || _regionToTool[nextNode.RegionType][1] == nextTool) {
                int distance = currentDij.Num + (nextTool == currentTool ? 1 : 8);
                if (nextNode.Num > distance) {
                    nextNode.Num = distance;
                    nextNode.Prior = currentDij;
                    _heap.Adjust(nextNode);
                }
            }
        }

        private int GetRisk() {
            int sum = 0;
            for (int x = 0; x <= _targetX; x++) {
                for (int y = 0; y <= _targetY; y++) {
                    var node = _dijGrid[x, y, 0];
                    sum += (int)node.RegionType;
                }
            }
            return sum;
        }

        private void SetGrid(int extraX, int extraY) {
            _dijGrid = new DijNode[_targetX + extraX + 1, _targetY + extraY + 1, 3];
            _heap = new BinaryHeap_Min();
            for (int x = 0; x <= _targetX + extraX; x++) {
                for (int y = 0; y <= _targetY + extraY; y++) {
                    foreach (enumTool tool in Enum.GetValues(typeof(enumTool))) {
                        var node = new DijNode() {
                            ToolType = tool,
                            X = x,
                            Y = y,
                            Num = int.MaxValue
                        };
                        _dijGrid[x, y, (int)tool] = node;
                        _heap.Add(node);
                        SetNodeStatus(node);
                    }
                }
            }
        }

        private void SetNodeStatus(DijNode node) {
            if (node.X == 0 && node.Y == 0) {
                node.Gelologic = 0;
            } else if (node.X == 0) {
                node.Gelologic = (ulong)node.Y * 48271;
            } else if (node.Y == 0) {
                node.Gelologic = (ulong)node.X * 16807;
            } else if (node.X == _targetX && node.Y == _targetY) {
                node.Gelologic = 0;
            } else {
                var minusX = _dijGrid[node.X - 1, node.Y, 0];
                var minusY = _dijGrid[node.X, node.Y - 1, 0];
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

        private class DijNode : BinaryHeap_Min.Node {
            public int X { get; set; }
            public int Y { get; set; }
            public enumTool ToolType { get; set; }
            public enumRegionType RegionType { get; set; }
            public ulong Gelologic { get; set; }
            public ulong Erosion { get; set; }
            public DijNode Prior { get; set; }
        }
    }
}
