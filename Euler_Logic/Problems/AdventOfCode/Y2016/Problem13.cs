using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem13 : AdventOfCodeBase {
        private Node[,] _nodesGrid;
        private List<Node> _nodesList;

        public override string ProblemName => "Advent of Code 2016: 13";

        public override string GetAnswer() {
            return Answer2(1364).ToString();
        }

        private int Answer1(ulong input) {
            BuildGrid(input, 50);
            return FindShortest(31, 39);
        }

        private int Answer2(ulong input) {
            return CountWithin50(input);
        }

        private int CountWithin50(ulong input) {
            int count = 1;
            var visited = new HashSet<Tuple<ulong, ulong>>();
            var current = new List<SmallNode>();
            var key = new Tuple<ulong, ulong>(0, 0);
            visited.Add(new Tuple<ulong, ulong>(1, 1));
            current.Add(new SmallNode() { X = 1, Y = 1 });
            do {
                var nextCurrent = new List<SmallNode>();
                foreach (var node in current) {
                    
                    // Left
                    if (node.X > 0) {
                        key = new Tuple<ulong, ulong>(node.X - 1, node.Y);
                        if (!visited.Contains(key) && !IsWall(node.X - 1, node.Y, input)) {
                            count++;
                            if (node.Distance + 1 < 50) {
                                nextCurrent.Add(new SmallNode() { Distance = node.Distance + 1, X = node.X - 1, Y = node.Y });
                            }
                        }
                        visited.Add(key);
                    }

                    // Right
                    key = new Tuple<ulong, ulong>(node.X + 1, node.Y);
                    if (!visited.Contains(key) && !IsWall(node.X + 1, node.Y, input)) {
                        count++;
                        if (node.Distance + 1 < 50) {
                            nextCurrent.Add(new SmallNode() { Distance = node.Distance + 1, X = node.X + 1, Y = node.Y });
                        }
                        visited.Add(key);
                    }

                    // Up
                    if (node.Y > 0) {
                        key = new Tuple<ulong, ulong>(node.X, node.Y - 1);
                        if (!visited.Contains(key) && !IsWall(node.X, node.Y - 1, input)) {
                            count++;
                            if (node.Distance + 1 < 50) {
                                nextCurrent.Add(new SmallNode() { Distance = node.Distance + 1, X = node.X, Y = node.Y - 1 });
                            }
                        }
                        visited.Add(key);
                    }

                    // Down
                    key = new Tuple<ulong, ulong>(node.X, node.Y + 1);
                    if (!visited.Contains(key) && !IsWall(node.X, node.Y + 1, input)) {
                        count++;
                        if (node.Distance + 1 < 50) {
                            nextCurrent.Add(new SmallNode() { Distance = node.Distance + 1, X = node.X, Y = node.Y + 1 });
                        }
                    }
                    visited.Add(key);
                }
                current = nextCurrent;
            } while (current.Count > 0);
            return count;
        }

        private int FindShortest(int findX, int findY) {
            var unvisited = new List<Node>(_nodesList.Where(node => !node.IsWall));
            _nodesGrid[1, 1].Distance = 0;
            do {
                var currentNode = unvisited.OrderBy(node => node.Distance).First();
                if (currentNode.X == findX && currentNode.Y == findY) {
                    return currentNode.Distance;
                }
                var x = currentNode.X;
                var y = currentNode.Y;

                // Left
                if (x > 0 && !_nodesGrid[x - 1, y].IsVisited && _nodesGrid[x - 1, y].Distance > currentNode.Distance + 1) {
                    _nodesGrid[x - 1, y].Distance = currentNode.Distance + 1;
                }

                // Right
                if (x < _nodesGrid.GetUpperBound(0) && !_nodesGrid[x + 1, y].IsVisited && _nodesGrid[x + 1, y].Distance > currentNode.Distance + 1) {
                    _nodesGrid[x + 1, y].Distance = currentNode.Distance + 1;
                }

                // Up
                if (y > 0 && !_nodesGrid[x, y - 1].IsVisited && _nodesGrid[x, y - 1].Distance > currentNode.Distance + 1) {
                    _nodesGrid[x, y - 1].Distance = currentNode.Distance + 1;
                }

                // Down
                if (y < _nodesGrid.GetUpperBound(1) && !_nodesGrid[x, y + 1].IsVisited && _nodesGrid[x, y + 1].Distance > currentNode.Distance + 1) {
                    _nodesGrid[x, y + 1].Distance = currentNode.Distance + 1;
                }

                currentNode.IsVisited = true;
                unvisited.Remove(currentNode);
            } while (true);
        }

        private void BuildGrid(ulong input, ulong length) {
            _nodesGrid = new Node[length, length];
            _nodesList = new List<Node>();
            for (ulong x = 0; x < length; x++) {
                for (ulong y = 0; y < length; y++) {
                    var node = new Node() { Distance = int.MaxValue, X = (int)x, Y = (int)y };
                    _nodesGrid[x, y] = node;
                    _nodesList.Add(node);
                    node.IsWall = IsWall(x, y, input);
                }
            }
        }

        private bool IsWall(ulong x, ulong y, ulong input) {
            ulong num = x * x + 3 * x + 2 * x * y + y + y * y + input;
            int count = 0;
            while (num > 0) {
                if (num % 2 == 1) {
                    count++;
                }
                num >>= 1;
            }
            if (count % 2 != 0) {
                return true;
            }
            return false;
        }

        private class Node {
            public int X { get; set; }
            public int Y { get; set; }
            public int Distance { get; set; }
            public bool IsWall { get; set; }
            public bool IsVisited { get; set; }
        }

        private class SmallNode {
            public ulong X { get; set; }
            public ulong Y { get; set; }
            public int Distance { get; set; }
        }
    }
}
