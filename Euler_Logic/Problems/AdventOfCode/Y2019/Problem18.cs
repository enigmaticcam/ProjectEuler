using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem18 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, Node>> _grid;
        private List<Node> _connectedGrid;
        private Node _startNode;
        private int _maxX;
        private int _maxY;
        private Dictionary<char, ulong> _charToBit;
        private List<Node> _keys;
        private Dictionary<Node, List<Route>> _routes;
        private ulong _allKeys;
        private Dictionary<Node, Dictionary<ulong, int>> _hash;
        private BinaryHeap_Min _heap;

        private enum enumNodeType {
            Empty,
            Wall,
            Door,
            Key,
            Start
        }

        public override string ProblemName {
            get { return "Advent of Code 2019: 18"; }
        }

        public override string GetAnswer() {
            _heap = new BinaryHeap_Min();
            SetCharToBit();
            GetGrid(Input());
            return Answer1().ToString();
        }

        private string Answer1() {
            SetAllRoutes();
            _best = int.MaxValue;
            FindBestRoute(_startNode, 0, 0);
            return _best.ToString();
        }

        private int _best;
        private void FindBestRoute(Node currentNode, int currentDistance, ulong keysFound) {
            foreach (var route in _routes[currentNode]) {
                var newDistance = currentDistance + route.Distance;
                if (newDistance > _best) {
                    break;
                }
                var nextNode = route.Node1;
                if (currentNode == nextNode) {
                    nextNode = route.Node2;
                }
                if ((nextNode.DoorOrKeyBit & keysFound) == 0) {
                    var keyXor = route.Keys ^ keysFound;
                    var keyDiff = keyXor - (keyXor & keysFound);
                    if ((route.Doors & keysFound) == route.Doors && keyDiff != 0) {
                        keysFound += keyDiff;
                        if (keysFound == _allKeys) {
                            _best = newDistance;
                        } else {
                            if (_hash[currentNode].ContainsKey(keysFound)) {
                                var bestForThisNode = _hash[currentNode][keysFound];
                                if (newDistance < bestForThisNode) {
                                    _hash[currentNode][keysFound] = newDistance;
                                    FindBestRoute(nextNode, newDistance, keysFound);
                                }
                            } else {
                                _hash[currentNode].Add(keysFound, newDistance);
                                FindBestRoute(nextNode, newDistance, keysFound);
                            }
                        }
                        keysFound -= keyDiff;
                    }
                }
            }
        }

        private void SetAllRoutes() {
            _heap = new BinaryHeap_Min();
            _connectedGrid.ForEach(x => _heap.Add(x));
            _routes = new Dictionary<Node, List<Route>>();
            _routes.Add(_startNode, new List<Route>());
            foreach (var key in _keys) {
                _routes[_startNode].Add(FindRoute(_startNode, key));
                _routes.Add(key, new List<Route>());
            }
            _routes[_startNode] = _routes[_startNode].OrderBy(x => x.Distance).ToList();
            for (int index1 = 0; index1 < _keys.Count; index1++) {
                var key1 = _keys[index1];
                for (int index2 = index1 + 1; index2 < _keys.Count; index2++) {
                    var key2 = _keys[index2];
                    var route = FindRoute(key1, key2);
                    _routes[key1].Add(route);
                    _routes[key2].Add(route);
                }
                _routes[key1] = _routes[key1].OrderBy(x => x.Distance).ToList();
            }
        }

        private Route FindRoute(Node start, Node find) {
            _connectedGrid.ForEach(x => {
                x.Num = int.MaxValue;
                x.Prior = null;
            });
            _heap.Reset();
            start.Num = 0;
            _heap.Adjust(start.Index);
            Node found = null;
            do {
                var current = FindLowestDistance();
                var nextDistance = current.Num + 1;
                
                // Left
                if (current.X > 0) {
                    var next = _grid[current.X - 1][current.Y];
                    if (next.Num > nextDistance) {
                        next.Num = nextDistance;
                        _heap.Adjust(next.Index);
                        next.Prior = current;
                    }
                    if (next == find) {
                        found = next;
                        break;
                    }
                }

                // Right
                if (current.X < _maxX) {
                    var next = _grid[current.X + 1][current.Y];
                    if (next.Num > nextDistance) {
                        next.Num = nextDistance;
                        _heap.Adjust(next.Index);
                        next.Prior = current;
                    }
                    if (next == find) {
                        found = next;
                        break;
                    }
                }

                // Up
                if (current.Y > 0) {
                    var next = _grid[current.X][current.Y - 1];
                    if (next.Num > nextDistance) {
                        next.Num = nextDistance;
                        _heap.Adjust(next.Index);
                        next.Prior = current;
                    }
                    if (next == find) {
                        found = next;
                        break;
                    }
                }

                // Down
                if (current.Y < _maxY) {
                    var next = _grid[current.X][current.Y + 1];
                    if (next.Num > nextDistance) {
                        next.Num = nextDistance;
                        _heap.Adjust(next.Index);
                        next.Prior = current;
                    }
                    if (next == find) {
                        found = next;
                        break;
                    }
                }
                //_heap.Adjust(current.Index);
                _heap.Remove(current.Index);
            } while (true);
            return MakeRoute(start, found);
        }

        private Route MakeRoute(Node start, Node find) {
            var route = new Route() { Node1 = start, Node2 = find, Distance = find.Num };
            while (find != null) {
                if (find.NodeType == enumNodeType.Door) {
                    route.Doors += find.DoorOrKeyBit;
                } else if (find.NodeType == enumNodeType.Key) {
                    route.Keys += find.DoorOrKeyBit;
                }
                find = find.Prior;
            }
            return route;
        }

        private Node FindLowestDistance() {
            return (Node)_heap.Top;
        }

        private void GetGrid(List<string> input) {
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            _hash = new Dictionary<Node, Dictionary<ulong, int>>();
            _connectedGrid = new List<Node>();
            _maxX = input[0].Length - 1;
            _maxY = input.Count - 1;
            var allKeys = new HashSet<char>();
            _keys = new List<Node>();
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    var node = new Node() {
                        X = x,
                        Y = y
                    };
                    var digit = input[y][x];
                    if (digit == '#') {
                        node.NodeType = enumNodeType.Wall;
                    } else {
                        _connectedGrid.Add(node);
                        if (digit == '.') {
                            node.NodeType = enumNodeType.Empty;
                        } else if (digit == '@') {
                            node.NodeType = enumNodeType.Start;
                            _startNode = node;
                            _hash.Add(node, new Dictionary<ulong, int>());
                        } else {
                            int ascii = (int)digit;
                            if (ascii <= 90) {
                                node.NodeType = enumNodeType.Door;
                                node.DoorOrKeyChar = digit;
                                node.DoorOrKeyBit = _charToBit[node.DoorOrKeyChar];
                            } else {
                                node.NodeType = enumNodeType.Key;
                                node.DoorOrKeyChar = (char)(ascii - 32);
                                node.DoorOrKeyBit = _charToBit[node.DoorOrKeyChar];
                                allKeys.Add(digit);
                                _keys.Add(node);
                                _allKeys += node.DoorOrKeyBit;
                                _hash.Add(node, new Dictionary<ulong, int>());
                            }
                        }
                    }
                    if (!_grid.ContainsKey(x)) {
                        _grid.Add(x, new Dictionary<int, Node>());
                    }
                    _grid[x].Add(y, node);
                }
            }
        }

        private void SetCharToBit() {
            _charToBit = new Dictionary<char, ulong>();
            ulong power = 1;
            for (int ascii = 65; ascii <= 90; ascii++) {
                _charToBit.Add((char)ascii, power);
                power *= 2;
            }
        }

        private List<string> Input_Test1() {
            return new List<string>() {
                "#########",
                "#b.A.@.a#",
                "#########"
            };
        }

        private List<string> Input_Test2() {
            return new List<string>() {
                "########################",
                "#f.D.E.e.C.b.A.@.a.B.c.#",
                "######################.#",
                "#d.....................#",
                "########################"
            };
        }

        private List<string> Input_Test3() {
            return new List<string>() {
                "########################",
                "#...............b.C.D.f#",
                "#.######################",
                "#.....@.a.B.c.d.A.e.F.g#",
                "########################"
            };
        }

        private List<string> Input_Test4() {
            return new List<string>() {
                "#################",
                "#i.G..c...e..H.p#",
                "########.########",
                "#j.A..b...f..D.o#",
                "########@########",
                "#k.E..a...g..B.n#",
                "########.########",
                "#l.F..d...h..C.m#",
                "#################"
            };
        }

        private List<string> Input_Test5() {
            return new List<string>() {
                "########################",
                "#@..............ac.GI.b#",
                "###d#e#f################",
                "###A#B#C################",
                "###g#h#i################",
                "########################"
            };
        }

        private class Node : BinaryHeap_Min.Node {
            public enumNodeType NodeType { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public char DoorOrKeyChar { get; set; }
            public ulong DoorOrKeyBit { get; set; }
            public Node Prior { get; set; }
        }

        private class Route {
            public Node Node1 { get; set; }
            public Node Node2 { get; set; }
            public ulong Keys { get; set; }
            public ulong Doors { get; set; }
            public int Distance { get; set; }
        }
    }
}
