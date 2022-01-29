using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem15 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, Node>> _grid;
        private List<int> _currentRoute;
        private List<Node> _nodes;
        private int _x;
        private int _y;
        private Node _targetNode;

        private enum enumNodeType {
            Wall,
            Empty,
            Target,
            Unknown
        }

        public override string ProblemName {
            get { return "Advent of Code 2019: 15"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            Initialize();
            var computer = new IntComputer();
            computer.Run(
                instructions: input,
                input: () => HandleInput(computer),
                outputCaller: () => HandleOutput(computer));
            return GetDistance();
        }

        private int Answer2(List<string> input) {
            Initialize();
            var computer = new IntComputer();
            computer.Run(
                instructions: input,
                input: () => HandleInput(computer),
                outputCaller: () => HandleOutput(computer));
            return FillOxygen();
        }

        private int FillOxygen() {
            int minutes = 0;
            var nodes = new List<Node>() { _targetNode };
            _targetNode.HasOxygen = true;
            do {
                var nextNodes = new List<Node>();
                foreach (var node in nodes) {
                    int x = node.X;
                    int y = node.Y;
                    if (_grid.ContainsKey(x - 1) && _grid[x - 1].ContainsKey(y) && _grid[x - 1][y].NodeType == enumNodeType.Empty && !_grid[x - 1][y].HasOxygen) {
                        var adj = _grid[x - 1][y];
                        adj.HasOxygen = true;
                        nextNodes.Add(adj);
                    }
                    if (_grid.ContainsKey(x + 1) && _grid[x + 1].ContainsKey(y) && _grid[x + 1][y].NodeType == enumNodeType.Empty && !_grid[x + 1][y].HasOxygen) {
                        var adj = _grid[x + 1][y];
                        adj.HasOxygen = true;
                        nextNodes.Add(adj);
                    }
                    if (_grid.ContainsKey(x) && _grid[x].ContainsKey(y - 1) && _grid[x][y - 1].NodeType == enumNodeType.Empty && !_grid[x][y - 1].HasOxygen) {
                        var adj = _grid[x][y - 1];
                        adj.HasOxygen = true;
                        nextNodes.Add(adj);
                    }
                    if (_grid.ContainsKey(x) && _grid[x].ContainsKey(y + 1) && _grid[x][y + 1].NodeType == enumNodeType.Empty && !_grid[x][y + 1].HasOxygen) {
                        var adj = _grid[x][y + 1];
                        adj.HasOxygen = true;
                        nextNodes.Add(adj);
                    }
                }
                minutes++;
                nodes = nextNodes;
            } while (nodes.Count != 0);
            return minutes - 1;
        }

        private int GetDistance() {
            _x = 0;
            _y = 0;
            var result = FindNearestUnknown(enumNodeType.Target);
            return result.Distance;
        }

        private void Initialize() {
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            _currentRoute = new List<int>();
            _nodes = new List<Node>();
            AddNode(new Node() { NodeType = enumNodeType.Empty });
            AddNode(new Node() { X = -1, NodeType = enumNodeType.Unknown });
            AddNode(new Node() { X = 1, NodeType = enumNodeType.Unknown });
            AddNode(new Node() { Y = -1, NodeType = enumNodeType.Unknown });
            AddNode(new Node() { Y = 1, NodeType = enumNodeType.Unknown });
        }

        private int HandleInput(IntComputer computer) {
            if (_currentRoute.Count > 0) {
                _currentRoute.RemoveAt(0);
            }
            if (_currentRoute.Count == 0) {
                var keepGoing = SetRoute();
                if (!keepGoing) {
                    computer.PerformFinish = true;
                    return 0;
                }
            }
            return _currentRoute[0];
        }

        private void HandleOutput(IntComputer computer) {
            Node node = null;
            var output = computer.Output.Last();
            switch (_currentRoute[0]) {
                case 1:
                    node = _grid[_x][_y + 1];
                    if (output != 0) _y++;
                    break;
                case 2:
                    node = _grid[_x][_y - 1];
                    if (output != 0) _y--;
                    break;
                case 3:
                    node = _grid[_x - 1][_y];
                    if (output != 0) _x--;
                    break;
                case 4:
                    node = _grid[_x + 1][_y];
                    if (output != 0) _x++;
                    break;
            }
            if (node.NodeType == enumNodeType.Unknown) {
                if (output == 0) {
                    node.NodeType = enumNodeType.Wall;
                } else if (output == 1) {
                    node.NodeType = enumNodeType.Empty;
                } else {
                    node.NodeType = enumNodeType.Target;
                    _targetNode = node;
                }
                AddNewNodes();
            }
        }

        private void AddNewNodes() {
            if (!_grid.ContainsKey(_x - 1)) {
                _grid.Add(_x - 1, new Dictionary<int, Node>());
            }
            if (!_grid[_x - 1].ContainsKey(_y)) {
                AddNode(new Node() {
                    NodeType = enumNodeType.Unknown,
                    X = _x - 1,
                    Y = _y
                });
            }
            if (!_grid.ContainsKey(_x + 1)) {
                _grid.Add(_x + 1, new Dictionary<int, Node>());
            }
            if (!_grid[_x + 1].ContainsKey(_y)) {
                AddNode(new Node() {
                    NodeType = enumNodeType.Unknown,
                    X = _x + 1,
                    Y = _y
                });
            }
            if (!_grid[_x].ContainsKey(_y - 1)) {
                AddNode(new Node() {
                    NodeType = enumNodeType.Unknown,
                    X = _x,
                    Y = _y - 1
                });
            }
            if (!_grid[_x].ContainsKey(_y + 1)) {
                AddNode(new Node() {
                    NodeType = enumNodeType.Unknown,
                    X = _x,
                    Y = _y + 1
                });
            }
        }

        private bool SetRoute() {
            var nearest = FindNearestUnknown(enumNodeType.Unknown);
            if (nearest == null) {
                return false;
            }
            ExtractRouteFromNodes(nearest);
            return true;
        }

        private void ExtractRouteFromNodes(Node node) {
            var route = new List<int>();
            do {
                if (node.Prior == null) {
                    break;
                }
                if (node.Prior.X == node.X - 1) {
                    route.Insert(0, 4);
                } else if (node.Prior.X == node.X + 1) {
                    route.Insert(0, 3);
                } else if (node.Prior.Y == node.Y - 1) {
                    route.Insert(0, 1);
                } else {
                    route.Insert(0, 2);
                }
                node = node.Prior;
            } while (true);
            _currentRoute = route;
        }

        private Node FindNearestUnknown(enumNodeType nodeType) {
            _nodes.ForEach(node => {
                node.Distance = int.MaxValue;
                node.Prior = null;
            });
            _grid[_x][_y].Distance = 0;
            var unvisited = new List<Node>(_nodes);
            do {
                var currentNode = GetLowestDistanceNode(unvisited);
                if (currentNode == null || currentNode.NodeType == nodeType) {
                    return currentNode;
                }
                int x = currentNode.X;
                int y = currentNode.Y;
                SetNodeDistance(currentNode, x - 1, y);
                SetNodeDistance(currentNode, x + 1, y);
                SetNodeDistance(currentNode, x, y - 1);
                SetNodeDistance(currentNode, x, y + 1);
                unvisited.Remove(currentNode);
            } while (unvisited.Count > 0);
            return null;
        }

        private void SetNodeDistance(Node currentNode, int x, int y) {
            if (_grid.ContainsKey(x) && _grid[x].ContainsKey(y)) {
                var adjacent = _grid[x][y];
                if (adjacent.Distance > currentNode.Distance + 1) {
                    adjacent.Distance = currentNode.Distance + 1;
                    adjacent.Prior = currentNode;
                }
            }
        }

        private Node GetLowestDistanceNode(IEnumerable<Node> nodes) {
            Node lowest = null;
            foreach (var node in nodes) {
                if (node.NodeType != enumNodeType.Wall) {
                    if (lowest == null) {
                        lowest = node;
                    } else if (lowest.Distance > node.Distance) {
                        lowest = node;
                    }
                }
            }
            return lowest;
        }

        private void AddNode(Node node) {
            if (!_grid.ContainsKey(node.X)) {
                _grid.Add(node.X, new Dictionary<int, Node>());
            }
            _grid[node.X].Add(node.Y, node);
            _nodes.Add(node);
        }

        private class Node {
            public enumNodeType NodeType { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Distance { get; set; }
            public Node Prior { get; set; }
            public bool HasOxygen { get; set; }
        }
    }
}
