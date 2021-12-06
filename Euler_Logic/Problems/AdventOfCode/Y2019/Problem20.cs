using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem20 : AdventOfCodeBase {
        private List<Dictionary<int, Dictionary<int, Node>>> _grid;
        private List<Node> _connected;
        private List<Node> _letters;
        private List<Node> _portals;
        private Node _start;
        private Node _end;
        private int _rimTop;
        private int _rimBottom;
        private int _rimLeft;
        private int _rimRight;

        private enum enumNodeType {
            Empty,
            Null,
            Wall,
            Letter
        }

        public override string ProblemName {
            get { return "Advent of Code 2019: 20"; }
        }

        public override string GetAnswer() {
            GetGrid(Input());
            BuildNeighborsStandard();
            BuildNeighborsPortals();
            return Answer2().ToString();
        }

        private int Answer1() {
            var heap = InitializeHeap();
            for (int count = 1; count <= _connected.Count; count++) {
                var current = (Node)heap.Top;
                foreach (var neighbor in current.Neighbors) {
                    if (neighbor.NextNode.Num > current.Num + 1) {
                        neighbor.NextNode.Num = current.Num + 1;
                        heap.Adjust(neighbor.NextNode.Index);
                    }
                }
                heap.Remove(current.Index);
            }
            return _end.Num;
        }

        private int Answer2() {
            FindPortals();
            var heap = InitializeHeap();
            do {
                var current = (Node)heap.Top;
                if (current == _end) {
                    return current.Num;
                }
                foreach (var neighbor in current.Neighbors) {
                    if (neighbor.IsPortalInner && neighbor.NextNode.Stack == current.Stack) {
                        AddNewStack(current, heap);
                    }
                    bool canConnect = true;
                    if (current.Stack != 0) {
                        if (neighbor.NextNode.X == _start.X && neighbor.NextNode.Y == _start.Y) {
                            canConnect = false;
                        } else if (neighbor.NextNode.X == _end.X && neighbor.NextNode.Y == _end.Y) {
                            canConnect = false;
                        }
                    } else if (neighbor.IsPortalOuter) {
                        canConnect = false;
                    }
                    if (canConnect && neighbor.NextNode.Num > current.Num + 1) {
                        neighbor.NextNode.Num = current.Num + 1;
                        heap.Adjust(neighbor.NextNode.Index);
                        neighbor.NextNode.Prior = current;
                    }
                }
                heap.Remove(current.Index);
            } while (true);
        }

        private void FindPortals() {
            _portals = new List<Node>();
            foreach (var node in _connected) {
                foreach (var neighbor in node.Neighbors) {
                    if (neighbor.IsPortalInner || neighbor.IsPortalOuter) {
                        _portals.Add(node);
                        break;
                    }
                }
            }
        }

        private void AddNewStack(Node start, BinaryHeap_Min heap) {
            AddNewStackNodes(heap, start.Stack + 1);
            AddNewStackNeighbors();
            ConnectNewStack(start.Stack + 1);
        }

        private void AddNewStackNodes(BinaryHeap_Min heap, int nextStack) {
            var newGrid = new Dictionary<int, Dictionary<int, Node>>();
            foreach (var node in _connected) {
                var newNode = new Node() {
                    Neighbors = new List<Edge>(),
                    Num = int.MaxValue,
                    Stack = nextStack,
                    X = node.X,
                    Y = node.Y
                };
                heap.Add(newNode);
                if (!newGrid.ContainsKey(node.X)) {
                    newGrid.Add(node.X, new Dictionary<int, Node>());
                }
                newGrid[node.X].Add(node.Y, newNode);
            }
            _grid.Add(newGrid);
        }

        private void AddNewStackNeighbors() {
            var newGrid = _grid.Last();
            foreach (var node in _connected) {
                var newNode = newGrid[node.X][node.Y];
                foreach (var oldNeighbor in node.Neighbors) {
                    newNode.Neighbors.Add(new Edge() {
                        IsPortalInner = oldNeighbor.IsPortalInner,
                        IsPortalOuter = oldNeighbor.IsPortalOuter,
                        NextNode = newGrid[oldNeighbor.NextNode.X][oldNeighbor.NextNode.Y]
                    });
                }
            }
        }

        private void ConnectNewStack(int nextStack) {
            var newGrid = _grid[nextStack];
            var priorGrid = _grid[nextStack - 1];
            foreach (var node in _portals) {
                var newNode = newGrid[node.X][node.Y];
                var priorNode = priorGrid[node.X][node.Y];
                foreach (var neighbor in node.Neighbors) {
                    if (neighbor.IsPortalInner) {
                        var adjust = priorNode.Neighbors.Where(x => x.IsPortalInner).First();
                        adjust.NextNode = newGrid[adjust.NextNode.X][adjust.NextNode.Y];
                    } else if (neighbor.IsPortalOuter) {
                        var adjust = newNode.Neighbors.Where(x => x.IsPortalOuter).First();
                        adjust.NextNode = priorGrid[adjust.NextNode.X][adjust.NextNode.Y];
                    }
                }
            }
        }

        private BinaryHeap_Min InitializeHeap() {
            var heap = new BinaryHeap_Min();
            _connected.ForEach(x => {
                x.Num = int.MaxValue;
                heap.Add(x);
            });
            _start.Num = 0;
            heap.Reset();
            heap.Adjust(_start.Index);
            return heap;
        }

        private void BuildNeighborsPortals() {
            var hash = new Dictionary<string, Portal>();
            foreach (var node in _letters) {
                
                // Portal on left of name
                if (_grid[0].ContainsKey(node.X - 2) && _grid[0][node.X - 2][node.Y].NodeType == enumNodeType.Empty && _grid[0][node.X - 1][node.Y].NodeType == enumNodeType.Letter) {
                    var key = new string(new char[2] { _grid[0][node.X - 1][node.Y].Letter, _grid[0][node.X][node.Y].Letter });
                    AddToHash(hash, key, _grid[0][node.X - 2][node.Y]);
                }

                // Portal on right of name
                if (_grid[0].ContainsKey(node.X + 2) && _grid[0][node.X + 2][node.Y].NodeType == enumNodeType.Empty && _grid[0][node.X + 1][node.Y].NodeType == enumNodeType.Letter) {
                    var key = new string(new char[2] { _grid[0][node.X][node.Y].Letter, _grid[0][node.X + 1][node.Y].Letter });
                    AddToHash(hash, key, _grid[0][node.X + 2][node.Y]);
                }

                // Portal on bottom of name
                if (_grid[0][node.X].ContainsKey(node.Y + 2) && _grid[0][node.X][node.Y + 2].NodeType == enumNodeType.Empty && _grid[0][node.X][node.Y + 1].NodeType == enumNodeType.Letter) {
                    var key = new string(new char[2] { _grid[0][node.X][node.Y].Letter, _grid[0][node.X][node.Y + 1].Letter });
                    AddToHash(hash, key, _grid[0][node.X][node.Y + 2]);
                }

                // Portal on top of name
                if (_grid[0][node.X].ContainsKey(node.Y - 2) && _grid[0][node.X][node.Y - 2].NodeType == enumNodeType.Empty && _grid[0][node.X][node.Y - 1].NodeType == enumNodeType.Letter) {
                    var key = new string(new char[2] { _grid[0][node.X][node.Y - 1].Letter, _grid[0][node.X][node.Y].Letter });
                    AddToHash(hash, key, _grid[0][node.X][node.Y - 2]);
                }
            }

            foreach (var portal in hash.Values) {
                if (portal.Node2 != null) {
                    portal.Node1.Neighbors.Add(new Edge() { NextNode = portal.Node2 });
                    portal.Node2.Neighbors.Add(new Edge() { NextNode = portal.Node1 });
                    SetEdgePortal(portal.Node1, portal.Node1.Neighbors.Last());
                    SetEdgePortal(portal.Node2, portal.Node2.Neighbors.Last());
                } else if (portal.Key == "AA") {
                    _start = portal.Node1;
                } else if (portal.Key == "ZZ") {
                    _end = portal.Node1;
                }
            }
        }

        private void SetEdgePortal(Node node, Edge edge) {
            edge.IsPortalInner = node.Y != _rimTop && node.Y != _rimBottom && node.X != _rimLeft && node.X != _rimRight;
            edge.IsPortalOuter = node.Y == _rimTop || node.Y == _rimBottom || node.X == _rimLeft || node.X == _rimRight;
        }

        private void AddToHash(Dictionary<string, Portal> hash, string key, Node node) {
            if (!hash.ContainsKey(key)) {
                hash.Add(key, new Portal() { Key = key });
            }
            var portal = hash[key];
            if (portal.Node1 == null) {
                portal.Node1 = node;
            } else {
                portal.Node2 = node;
            }
        }

        private void BuildNeighborsStandard() {
            foreach (var node in _connected) {
                var neighbor = _grid[0][node.X][node.Y - 1];
                if (neighbor.NodeType == enumNodeType.Empty) {
                    node.Neighbors.Add(new Edge() { NextNode = neighbor });
                }
                neighbor = _grid[0][node.X][node.Y + 1];
                if (neighbor.NodeType == enumNodeType.Empty) {
                    node.Neighbors.Add(new Edge() { NextNode = neighbor });
                }
                neighbor = _grid[0][node.X - 1][node.Y];
                if (neighbor.NodeType == enumNodeType.Empty) {
                    node.Neighbors.Add(new Edge() { NextNode = neighbor });
                }
                neighbor = _grid[0][node.X + 1][node.Y];
                if (neighbor.NodeType == enumNodeType.Empty) {
                    node.Neighbors.Add(new Edge() { NextNode = neighbor });
                }
            }
        }

        private void GetGrid(List<string> input) {
            _rimBottom = input.Count - 3;
            _rimTop = 2;
            _rimLeft = 2;
            _rimRight = input[0].Length - 3;
            _grid = new List<Dictionary<int, Dictionary<int, Node>>>();
            _grid.Add(new Dictionary<int, Dictionary<int, Node>>());
            _connected = new List<Node>();
            _letters = new List<Node>();
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    var node = new Node() { X = x, Y = y, Neighbors = new List<Edge>() };
                    switch (input[y][x]) {
                        case ' ':
                            node.NodeType = enumNodeType.Null;
                            break;
                        case '#':
                            node.NodeType = enumNodeType.Wall;
                            break;
                        case '.':
                            node.NodeType = enumNodeType.Empty;
                            _connected.Add(node);
                            break;
                        default:
                            node.NodeType = enumNodeType.Letter;
                            node.Letter = input[y][x];
                            _letters.Add(node);
                            break;
                    }
                    if (!_grid[0].ContainsKey(x)) {
                        _grid[0].Add(x, new Dictionary<int, Node>());
                    }
                    _grid[0][x].Add(y, node);
                }
            }
        }

        private string PrintOutput(int index) {
            var grid = _grid[index];
            var maxX = grid.Keys.Max();
            var maxY = grid.First().Value.Keys.Max();
            var text = new StringBuilder();
            
            for (int y = 0; y <= maxY; y++) {
                for (int x = 0; x <= maxX; x++) {
                    if (grid.ContainsKey(x) && grid[x].ContainsKey(y)) {
                        var node = grid[x][y];
                        if (node.NodeType == enumNodeType.Empty) {
                            if (node.IsTraversed) {
                                text.Append("*");
                            } else {
                                text.Append(".");
                            }
                        } else if (node.NodeType == enumNodeType.Wall) {
                            text.Append("#");
                        } else {
                            text.Append(" ");
                        }
                    } else {
                        text.Append(" ");
                    }
                }
                text.AppendLine();
            }
            return text.ToString();
        }

        private List<string> Input_Test1() {
            return new List<string>() {
                "         A           ",
                "         A           ",
                "  #######.#########  ",
                "  #######.........#  ",
                "  #######.#######.#  ",
                "  #######.#######.#  ",
                "  #######.#######.#  ",
                "  #####  B    ###.#  ",
                "BC...##  C    ###.#  ",
                "  ##.##       ###.#  ",
                "  ##...DE  F  ###.#  ",
                "  #####    G  ###.#  ",
                "  #########.#####.#  ",
                "DE..#######...###.#  ",
                "  #.#########.###.#  ",
                "FG..#########.....#  ",
                "  ###########.#####  ",
                "             Z       ",
                "             Z       "
            };
        }

        private List<string> Input_Test2() {
            return new List<string>() {
                "                   A               ",
                "                   A               ",
                "  #################.#############  ",
                "  #.#...#...................#.#.#  ",
                "  #.#.#.###.###.###.#########.#.#  ",
                "  #.#.#.......#...#.....#.#.#...#  ",
                "  #.#########.###.#####.#.#.###.#  ",
                "  #.............#.#.....#.......#  ",
                "  ###.###########.###.#####.#.#.#  ",
                "  #.....#        A   C    #.#.#.#  ",
                "  #######        S   P    #####.#  ",
                "  #.#...#                 #......VT",
                "  #.#.#.#                 #.#####  ",
                "  #...#.#               YN....#.#  ",
                "  #.###.#                 #####.#  ",
                "DI....#.#                 #.....#  ",
                "  #####.#                 #.###.#  ",
                "ZZ......#               QG....#..AS",
                "  ###.###                 #######  ",
                "JO..#.#.#                 #.....#  ",
                "  #.#.#.#                 ###.#.#  ",
                "  #...#..DI             BU....#..LF",
                "  #####.#                 #.#####  ",
                "YN......#               VT..#....QG",
                "  #.###.#                 #.###.#  ",
                "  #.#...#                 #.....#  ",
                "  ###.###    J L     J    #.#.###  ",
                "  #.....#    O F     P    #.#...#  ",
                "  #.###.#####.#.#####.#####.###.#  ",
                "  #...#.#.#...#.....#.....#.#...#  ",
                "  #.#####.###.###.#.#.#########.#  ",
                "  #...#.#.....#...#.#.#.#.....#.#  ",
                "  #.###.#####.###.###.#.#.#######  ",
                "  #.#.........#...#.............#  ",
                "  #########.###.###.#############  ",
                "           B   J   C               ",
                "           U   P   P               "
            };
        }

        private List<string> Input_Test3() {
            return new List<string>() {
                "             Z L X W       C                 ",
                "             Z P Q B       K                 ",
                "  ###########.#.#.#.#######.###############  ",
                "  #...#.......#.#.......#.#.......#.#.#...#  ",
                "  ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###  ",
                "  #.#...#.#.#...#.#.#...#...#...#.#.......#  ",
                "  #.###.#######.###.###.#.###.###.#.#######  ",
                "  #...#.......#.#...#...#.............#...#  ",
                "  #.#########.#######.#.#######.#######.###  ",
                "  #...#.#    F       R I       Z    #.#.#.#  ",
                "  #.###.#    D       E C       H    #.#.#.#  ",
                "  #.#...#                           #...#.#  ",
                "  #.###.#                           #.###.#  ",
                "  #.#....OA                       WB..#.#..ZH",
                "  #.###.#                           #.#.#.#  ",
                "CJ......#                           #.....#  ",
                "  #######                           #######  ",
                "  #.#....CK                         #......IC",
                "  #.###.#                           #.###.#  ",
                "  #.....#                           #...#.#  ",
                "  ###.###                           #.#.#.#  ",
                "XF....#.#                         RF..#.#.#  ",
                "  #####.#                           #######  ",
                "  #......CJ                       NM..#...#  ",
                "  ###.#.#                           #.###.#  ",
                "RE....#.#                           #......RF",
                "  ###.###        X   X       L      #.#.#.#  ",
                "  #.....#        F   Q       P      #.#.#.#  ",
                "  ###.###########.###.#######.#########.###  ",
                "  #.....#...#.....#.......#...#.....#.#...#  ",
                "  #####.#.###.#######.#######.###.###.#.#.#  ",
                "  #.......#.......#.#.#.#.#...#...#...#.#.#  ",
                "  #####.###.#####.#.#.#.#.###.###.#.###.###  ",
                "  #.......#.....#.#...#...............#...#  ",
                "  #############.#.#.###.###################  ",
                "               A O F   N                     ",
                "               A A D   M                     "
            };
        }

        private class Node : BinaryHeap_Min.Node {
            public int X { get; set; }
            public int Y { get; set; }
            public enumNodeType NodeType { get; set; }
            public List<Edge> Neighbors { get; set; }
            public char Letter { get; set; }
            public int Stack { get; set; }
            public Node Prior { get; set; }
            public bool IsTraversed { get; set; }
        }

        private class Portal {
            public Node Node1 { get; set; }
            public Node Node2 { get; set; }
            public string Key { get; set; }
        }

        private class Edge {
            public Node NextNode { get; set; }
            public bool IsPortalOuter { get; set; }
            public bool IsPortalInner { get; set; }
        }
    }
}
