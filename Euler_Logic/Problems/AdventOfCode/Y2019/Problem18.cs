using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem18 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, Node>> _grid;
        private List<Node> _connectedGrid;
        private Node[] _startNodes;
        private int _maxX;
        private int _maxY;
        private Dictionary<char, ulong> _charToBit;
        private ulong _allKeys;
        private Dictionary<Node, Dictionary<ulong, int>> _hash;
        private BinaryHeap_Min _heap;
        private List<Node> _keyPaths;
        private ulong _keysFound;

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
            _best = int.MaxValue;
            SetCharToBit();
            GetGrid(Input());
            return Answer1().ToString();
        }

        private string Answer1() {
            FindBestRoute(_startNodes[0], 0);
            return _best.ToString();
        }

        private string Answer2() {
            AddCenterDivision();
            FindBestRoute(0);
            return _best.ToString();
        }

        private void AddCenterDivision() {
            var start = _startNodes[0];
            _grid[start.X][start.Y].NodeType = enumNodeType.Wall;
            _grid[start.X - 1][start.Y].NodeType = enumNodeType.Wall;
            _grid[start.X + 1][start.Y].NodeType = enumNodeType.Wall;
            _grid[start.X][start.Y - 1].NodeType = enumNodeType.Wall;
            _grid[start.X][start.Y + 1].NodeType = enumNodeType.Wall;
            _startNodes[0] = _grid[start.X - 1][start.Y - 1];
            _startNodes[1] = _grid[start.X - 1][start.Y + 1];
            _startNodes[2] = _grid[start.X + 1][start.Y - 1];
            _startNodes[3] = _grid[start.X + 1][start.Y + 1];
            _startNodes[0].NodeType = enumNodeType.Start;
            _startNodes[1].NodeType = enumNodeType.Start;
            _startNodes[2].NodeType = enumNodeType.Start;
            _startNodes[3].NodeType = enumNodeType.Start;
        }

        private int _best;
        private void FindBestRoute(Node currentNode, int currentDistance) {
            FindPaths(currentNode);
            var nextPaths = _keyPaths.Select(x => new SubNode() {
                N = x,
                D = x.Num,
                KeysFound = x.KeysFound
            }).ToList();
            FindBestRouteRecursive(nextPaths, currentDistance, false);
        }

        private void FindBestRoute(int currentDistance) {
            var nextPaths = new List<SubNode>();
            for (int index = 0; index < _startNodes.Length; index++) {
                var start = _startNodes[index];
                FindPaths(start);
                nextPaths.AddRange(_keyPaths.Select(x => new SubNode() {
                    N = x,
                    D = x.Num,
                    KeysFound = x.KeysFound,
                    StartNodeIndex = index
                }));
            }
            FindBestRouteRecursive(nextPaths, currentDistance, true);
        }

        private void FindBestRouteRecursive(List<SubNode> nextPaths, int currentDistance, bool multiplePaths) {
            foreach (var path in nextPaths) {
                var keyDiff = _keysFound ^ path.KeysFound;
                keyDiff &= path.KeysFound;
                _keysFound += keyDiff;
                var nextDistance = currentDistance + path.D;
                if (_keysFound == _allKeys && nextDistance < _best) {
                    _best = nextDistance;
                }
                if (_hash[path.N].ContainsKey(_keysFound)) {
                    var bestSoFar = _hash[path.N][_keysFound];
                    if (bestSoFar > nextDistance) {
                        _hash[path.N][_keysFound] = nextDistance;
                        if (multiplePaths) {
                            var startNode = _startNodes[path.StartNodeIndex];
                            _startNodes[path.StartNodeIndex] = path.N;
                            FindBestRoute(nextDistance);
                            _startNodes[path.StartNodeIndex] = startNode;
                        } else {
                            FindBestRoute(path.N, nextDistance);
                        }
                    }
                } else {
                    _hash[path.N].Add(_keysFound, nextDistance);
                    if (multiplePaths) {
                        var startNode = _startNodes[path.StartNodeIndex];
                        _startNodes[path.StartNodeIndex] = path.N;
                        FindBestRoute(nextDistance);
                        _startNodes[path.StartNodeIndex] = startNode;
                    } else {
                        FindBestRoute(path.N, nextDistance);
                    }
                }
                _keysFound -= keyDiff;
            }
        }

        private void FindPaths(Node start) {
            _connectedGrid.ForEach(x => {
                x.Num = int.MaxValue;
                x.Prior = null;
                x.KeysFound = 0;
            });
            _heap.Reset();
            start.Num = 0;
            _heap.Adjust(start.Index);
            int total = _connectedGrid.Count;
            _keyPaths = new List<Node>();
            do {
                var current = (Node)_heap.Top;
                if (current.Num == int.MaxValue) {
                    break;
                }
                var nextDistance = current.Num + 1;
                
                // Left
                if (current.X > 0) {
                    var next = _grid[current.X - 1][current.Y];
                    HandleNode(current, next, nextDistance);
                }

                // Right
                if (current.X < _maxX) {
                    var next = _grid[current.X + 1][current.Y];
                    HandleNode(current, next, nextDistance);
                }

                // Up
                if (current.Y > 0) {
                    var next = _grid[current.X][current.Y - 1];
                    HandleNode(current, next, nextDistance);
                }

                // Down
                if (current.Y < _maxY) {
                    var next = _grid[current.X][current.Y + 1];
                    HandleNode(current, next, nextDistance);
                }
                _heap.Remove(current.Index);
                total--;
                if (total == 0) {
                    break;
                }
            } while (true);
        }

        private void HandleNode(Node current, Node next, int nextDistance) {
            bool canMoveTo = false;
            if (next.NodeType == enumNodeType.Empty || next.NodeType == enumNodeType.Start || next.NodeType == enumNodeType.Key) {
                canMoveTo = true;
            } else if (next.NodeType == enumNodeType.Door && (_keysFound & next.DoorOrKeyBit) == next.DoorOrKeyBit) {
                canMoveTo = true;
            }
            if (canMoveTo) {
                if (next.Num > nextDistance) {
                    next.Num = nextDistance;
                    _heap.Adjust(next.Index);
                    next.Prior = current;
                    if (next.NodeType == enumNodeType.Key && (_keysFound & next.DoorOrKeyBit) == 0) {
                        next.KeysFound = next.DoorOrKeyBit + current.KeysFound;
                        _keyPaths.Add(next);
                    }
                }
            }
        }

        private void GetGrid(List<string> input) {
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            _hash = new Dictionary<Node, Dictionary<ulong, int>>();
            _connectedGrid = new List<Node>();
            _maxX = input[0].Length - 1;
            _maxY = input.Count - 1;
            _startNodes = new Node[4];
            var allKeys = new HashSet<char>();
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
                            _startNodes[0] = node;
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
            _heap = new BinaryHeap_Min();
            _connectedGrid.ForEach(x => _heap.Add(x));
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

        private List<string> Input_Test6() {
            return new List<string>() {
                "#######",
                "#a.#Cd#",
                "##...##",
                "##.@.##",
                "##...##",
                "#cB#Ab#",
                "#######"
            };
        }

        private List<string> Input_Test7() {
            return new List<string>() {
                "###############",
                "#d.ABC.#.....a#",
                "######...######",
                "######.@.######",
                "######...######",
                "#b.....#.....c#",
                "###############"
            };
        }

        private List<string> Input_Test8() {
            return new List<string>() {
                "#############",
                "#DcBa.#.GhKl#",
                "#.###...#I###",
                "#e#d#.@.#j#k#",
                "###C#...###J#",
                "#fEbA.#.FgHi#",
                "#############"
            };
        }

        private List<string> Input_Test9() {
            return new List<string>() {
                "#############",
                "#g#f.D#..h#l#",
                "#F###e#E###.#",
                "#dCba...BcIJ#",
                "#####.@.#####",
                "#nK.L...G...#",
                "#M###N#H###.#",
                "#o#m..#i#jk.#",
                "#############"
            };
        }

        private class Node : BinaryHeap_Min.Node {
            public enumNodeType NodeType { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public char DoorOrKeyChar { get; set; }
            public ulong DoorOrKeyBit { get; set; }
            public Node Prior { get; set; }
            public ulong KeysFound { get; set; }
        }

        private class SubNode {
            public Node N { get; set; }
            public int D { get; set; }
            public ulong KeysFound { get; set; }
            public int StartNodeIndex { get; set; }
        }
    }
}
