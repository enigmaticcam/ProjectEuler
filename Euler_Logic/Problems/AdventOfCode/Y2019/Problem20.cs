using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem20 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, Node>> _grid;
        private List<Node> _connected;
        private List<Node> _letters;
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
            GetGrid(Input_Test1());
            BuildNeighborsStandard();
            BuildNeighborsPortals();
            return Answer1().ToString();
        }

        private int Answer1() {
            var heap = new BinaryHeap_Min();
            _connected.ForEach(x => {
                x.Num = int.MaxValue;
                heap.Add(x);
            });
            _start.Num = 0;
            heap.Reset();
            heap.Adjust(_start.Index);
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
            var heap = new List<BinaryHeap_Min>();

            do {
                
            } while (true);
        }

        private void BuildNeighborsPortals() {
            var hash = new Dictionary<string, Portal>();
            foreach (var node in _letters) {
                
                // Portal on left of name
                if (_grid.ContainsKey(node.X - 2) && _grid[node.X - 2][node.Y].NodeType == enumNodeType.Empty && _grid[node.X - 1][node.Y].NodeType == enumNodeType.Letter) {
                    var key = new string(new char[2] { _grid[node.X - 1][node.Y].Letter, _grid[node.X][node.Y].Letter });
                    AddToHash(hash, key, _grid[node.X - 2][node.Y]);
                }

                // Portal on right of name
                if (_grid.ContainsKey(node.X + 2) && _grid[node.X + 2][node.Y].NodeType == enumNodeType.Empty && _grid[node.X + 1][node.Y].NodeType == enumNodeType.Letter) {
                    var key = new string(new char[2] { _grid[node.X][node.Y].Letter, _grid[node.X + 1][node.Y].Letter });
                    AddToHash(hash, key, _grid[node.X + 2][node.Y]);
                }

                // Portal on bottom of name
                if (_grid[node.X].ContainsKey(node.Y + 2) && _grid[node.X][node.Y + 2].NodeType == enumNodeType.Empty && _grid[node.X][node.Y + 1].NodeType == enumNodeType.Letter) {
                    var key = new string(new char[2] { _grid[node.X][node.Y].Letter, _grid[node.X][node.Y + 1].Letter });
                    AddToHash(hash, key, _grid[node.X][node.Y + 2]);
                }

                // Portal on top of name
                if (_grid[node.X].ContainsKey(node.Y - 2) && _grid[node.X][node.Y - 2].NodeType == enumNodeType.Empty && _grid[node.X][node.Y - 1].NodeType == enumNodeType.Letter) {
                    var key = new string(new char[2] { _grid[node.X][node.Y - 1].Letter, _grid[node.X][node.Y].Letter });
                    AddToHash(hash, key, _grid[node.X][node.Y - 2]);
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
            edge.IsPortalOuter = edge.NextNode.X != _rimTop && edge.NextNode.X != _rimBottom && edge.NextNode.Y != _rimLeft && edge.NextNode.Y != _rimRight;
            edge.IsPortalInner = edge.NextNode.X == _rimTop || edge.NextNode.X == _rimBottom || edge.NextNode.Y == _rimLeft || edge.NextNode.Y == _rimRight;
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
                var neighbor = _grid[node.X][node.Y - 1];
                if (neighbor.NodeType == enumNodeType.Empty) {
                    node.Neighbors.Add(new Edge() { NextNode = neighbor });
                }
                neighbor = _grid[node.X][node.Y + 1];
                if (neighbor.NodeType == enumNodeType.Empty) {
                    node.Neighbors.Add(new Edge() { NextNode = neighbor });
                }
                neighbor = _grid[node.X - 1][node.Y];
                if (neighbor.NodeType == enumNodeType.Empty) {
                    node.Neighbors.Add(new Edge() { NextNode = neighbor });
                }
                neighbor = _grid[node.X + 1][node.Y];
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
            _grid = new Dictionary<int, Dictionary<int, Node>>();
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
                    if (!_grid.ContainsKey(x)) {
                        _grid.Add(x, new Dictionary<int, Node>());
                    }
                    _grid[x].Add(y, node);
                }
            }
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

        private class Node : BinaryHeap_Min.Node {
            public int X { get; set; }
            public int Y { get; set; }
            public enumNodeType NodeType { get; set; }
            public List<Edge> Neighbors { get; set; }
            public char Letter { get; set; }
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
