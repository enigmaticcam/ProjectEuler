using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem22 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 22";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var state = new State();
            SetNodes(state, input);
            SetMax(state);
            SetRemaining(state);
            SetEdges(state);
            SetMoves(state, input.Last());
            SetStart(state);
            PerformMoves(state);
            return CalcPassword(state);
        }

        private int Answer2(List<string> input) {
            var state = new State();
            SetNodes(state, input);
            SetMax(state);
            SetRemaining(state);
            SetEdges(state);
            SetEdges_Cube(state);
            SetMoves(state, input.Last());
            SetStart(state);
            PerformMoves(state);
            return CalcPassword(state);
        }

        private void SetEdges_Cube(State state) {
            SetCubeLength(state);
            SetFirstFace(state);
            var hash = new HashSet<Tuple<int, int>>();
            Recursive(state, state.CubeFace, hash);
            AddMissingFace(state);
            ConnectEdges(state);
        }

        private void ConnectEdges(State state) {
            foreach (var face in state.CubeFaceAll) {
                for (int direction = 0; direction < 4; direction++) {
                    int index = FindIndex(face, face.Edges[direction].Edges);
                    ConnectEdge(state, face, face.Edges[direction], direction, index);
                }
            }
        }

        private void ConnectEdge(State state, CubeEdge face1, CubeEdge face2, int faceDirection1, int faceDirection2) {
            int direction1 = Mod(faceDirection1 + 1);
            var node1 = state.Nodes[face1.X + state.CubeLength - 1][face1.Y];
            if (faceDirection1 == 1) {
                node1 = state.Nodes[face1.X + state.CubeLength - 1][face1.Y + state.CubeLength - 1];
            } else if (faceDirection1 == 2) {
                node1 = state.Nodes[face1.X][face1.Y + state.CubeLength - 1];
            } else if (faceDirection1 == 3) {
                node1 = state.Nodes[face1.X][face1.Y];
            }

            int direction2 = Mod(faceDirection2 - 1);
            var node2 = state.Nodes[face2.X + state.CubeLength - 1][face2.Y + state.CubeLength - 1];
            if (faceDirection2 == 1) {
                node2 = state.Nodes[face2.X][face2.Y + state.CubeLength - 1];
            } else if (faceDirection2 == 2) {
                node2 = state.Nodes[face2.X][face2.Y];
            } else if (faceDirection2 == 3) {
                node2 = state.Nodes[face2.X + state.CubeLength - 1][face2.Y];
            }

            for (int count = 1; count <= state.CubeLength; count++) {
                node1.Edges[faceDirection1] = node2;
                if (node1.DirectionAdjust == null) node1.DirectionAdjust = new int[4] { -1, -1, -1, -1 };
                node1.DirectionAdjust[faceDirection1] = Mod(faceDirection2 + 2);
                node1 = node1.Edges[direction1];
                node2 = node2.Edges[direction2];
            }
        }

        private void AddMissingFace(State state) {
            do {
                state.CubeFaceAll.ForEach(x => AddMissingFace(x));
            } while (state.CubeFaceAll.Any(x => x.Edges.Any(y => y == null)));
        }

        private void AddMissingFace(CubeEdge face) {
            AddMissingFace(face, 0);
            AddMissingFace(face, 1);
            AddMissingFace(face, 2);
            AddMissingFace(face, 3);
        }

        private void AddMissingFace(CubeEdge face, int direction) {
            if (face.Edges[direction] == null) {
                int plus = Mod(direction + 1);
                int minus = Mod(direction - 1);
                if (face.Edges[plus] != null) {
                    int index = FindIndex(face, face.Edges[plus].Edges);
                    if (index != -1) {
                        int offset = Mod(index + 1);
                        if (face.Edges[plus].Edges[offset] != null) {
                            face.Edges[direction] = face.Edges[plus].Edges[offset];
                        }
                    }
                    
                } 
                if (face.Edges[direction] == null && face.Edges[minus] != null) {
                    int index = FindIndex(face, face.Edges[minus].Edges);
                    if (index != -1) {
                        int offset = Mod(index - 1);
                        if (face.Edges[minus].Edges[offset] != null) {
                            face.Edges[direction] = face.Edges[minus].Edges[offset];
                        }
                    }
                    
                }
            }
        }

        private int Mod(int num) {
            num %= 4;
            if (num < 0) num += 4;
            return num;
        }

        private int FindIndex(CubeEdge face, CubeEdge[] edges) {
            if (edges[0] == face) return 0;
            if (edges[1] == face) return 1;
            if (edges[2] == face) return 2;
            if (edges[3] == face) return 3;
            return -1;
        }

        private void Recursive(State state, CubeEdge face, HashSet<Tuple<int, int>> hash) {
            hash.Add(new Tuple<int, int>(face.X, face.Y));
            if (face.Edges[0] == null && face.X + state.CubeLength <= state.MaxX) {
                NextRecursive(state, face, face.X + state.CubeLength, face.Y, 0, hash);
            }
            if (face.Edges[2] == null && face.X - state.CubeLength >= 0) {
                NextRecursive(state, face, face.X - state.CubeLength, face.Y, 2, hash);
            }
            if (face.Edges[1] == null && face.Y + state.CubeLength <= state.MaxY) {
                NextRecursive(state, face, face.X, face.Y + state.CubeLength, 1, hash);
            }
            if (face.Edges[3] == null && face.Y - state.CubeLength >= 0) {
                NextRecursive(state, face, face.X, face.Y - state.CubeLength, 3, hash);
            }
        }

        private void NextRecursive(State state, CubeEdge face, int x, int y, int direction, HashSet<Tuple<int, int>> hash) {
            if (!hash.Contains(new Tuple<int, int>(x, y))) {
                var nextNode = state.Nodes[x][y];
                if (nextNode.NodeType != enumNodeType.Empty) {
                    var nextFace = new CubeEdge() {
                        Edges = new CubeEdge[4],
                        X = x,
                        Y = y
                    };
                    face.Edges[direction] = nextFace;
                    nextFace.Edges[(direction + 2) % 4] = face;
                    state.CubeFaceAll.Add(nextFace);
                    Recursive(state, nextFace, hash);
                }
            }
        }

        private bool SetFirstFace(State state) {
            for (int x = 0; x <= state.MaxX; x += state.CubeLength) {
                for (int y = 0; y <= state.MaxY; y += state.CubeLength) {
                    var node = state.Nodes[x][y];
                    if (node.NodeType != enumNodeType.Empty) {
                        state.CubeFace = new CubeEdge() {
                            Edges = new CubeEdge[4],
                            X = x,
                            Y = y
                        };
                        state.CubeFaceAll = new List<CubeEdge>();
                        state.CubeFaceAll.Add(state.CubeFace);
                        return true;
                    }
                }
            }
            return false;
        }

        private void SetCubeLength(State state) {
            if ((state.MaxX + 1) % 3 == 0) {
                state.CubeLength = (state.MaxX + 1) / 3;
            } else {
                state.CubeLength = (state.MaxX + 1) / 4;
            }
        }

        private int CalcPassword(State state) {
            var node = state.CurrentNode;
            return ((node.Y + 1) * 1000) + ((node.X + 1) * 4) + state.CurrentDirection;
        }

        private void PerformMoves(State state) {
            var node = state.CurrentNode;
            foreach (var move in state.Moves) {
                for (int count = 1; count <= move.Spaces; count++) {
                    var next = node.Edges[state.CurrentDirection];
                    if (next.NodeType == enumNodeType.Wall) break;
                    if (node.DirectionAdjust != null && node.DirectionAdjust[state.CurrentDirection] != -1) {
                        state.CurrentDirection = node.DirectionAdjust[state.CurrentDirection];
                    }
                    node = next;
                }
                state.CurrentDirection = (state.CurrentDirection + move.Direction) % 4;
                if (state.CurrentDirection < 0) state.CurrentDirection += 4;
            }
            state.CurrentNode = node;
        }

        private void SetStart(State state) {
            int x = 0;
            do {
                var node = state.Nodes[x][0];
                if (node.NodeType == enumNodeType.Open) {
                    state.CurrentNode = node;
                    break;
                }
                x++;
            } while (true);
        }

        private void SetMoves(State state, string line) {
            state.Moves = new List<Move>();
            var move = new Move();
            foreach (var digit in line) {
                if (digit == 'R') {
                    move.Direction = 1;
                    state.Moves.Add(move);
                    move = new Move();
                } else if (digit == 'L') {
                    move.Direction = -1;
                    state.Moves.Add(move);
                    move = new Move();
                } else {
                    var number = Convert.ToInt32(new string(new char[1] { digit }));
                    move.Spaces = move.Spaces * 10 + number;
                }
            }
            state.Moves.Add(move);
        }

        private void SetEdges(State state) {
            foreach (var node in state.NodesAll) {
                if (node.NodeType != enumNodeType.Empty) {
                    int x = node.X;
                    int y = node.Y;
                    do {
                        x = (x + 1) % (state.MaxX + 1);
                        var edge = state.Nodes[x][y];
                        if (edge.NodeType != enumNodeType.Empty) {
                            node.Edges[0] = edge;
                            edge.Edges[2] = node;
                            break;
                        }
                    } while (true);

                    x = node.X;
                    y = node.Y;
                    do {
                        y = (y + 1) % (state.MaxY + 1);
                        var edge = state.Nodes[x][y];
                        if (edge.NodeType != enumNodeType.Empty) {
                            node.Edges[1] = edge;
                            edge.Edges[3] = node;
                            break;
                        }
                    } while (true);
                }
            }
        }

        private void SetRemaining(State state) {
            for (int x = 0; x <= state.MaxX; x++) {
                for (int y = 0; y <= state.MaxY; y++) {
                    if (!state.Nodes.ContainsKey(x)) state.Nodes.Add(x, new Dictionary<int, Node>());
                    if (!state.Nodes[x].ContainsKey(y)) {
                        var node = new Node() {
                            Edges = new Node[4],
                            NodeType = enumNodeType.Empty,
                            X = x,
                            Y = y
                        };
                        state.Nodes[x].Add(y, node);
                        state.NodesAll.Add(node);
                    }
                }
            }
        }

        private void SetMax(State state) {
            state.MaxX = state.Nodes.Keys.Max();
            state.MaxY = state.Nodes.SelectMany(x => x.Value).Select(x => x.Key).Max();
        }

        private void SetNodes(State state, List<string> input) {
            state.Nodes = new Dictionary<int, Dictionary<int, Node>>();
            state.NodesAll = new List<Node>();
            int y = 0;
            foreach (var line in input) {
                if (string.IsNullOrEmpty(line)) break;
                for (int x = 0; x < line.Length; x++) {
                    var node = new Node() { 
                        X = x, 
                        Y = y,
                        NodeType = GetNodeType(line[x]),
                        Edges = new Node[4]
                    };
                    if (!state.Nodes.ContainsKey(x)) {
                        state.Nodes.Add(x, new Dictionary<int, Node>());
                    }
                    state.Nodes[x].Add(y, node);
                    state.NodesAll.Add(node);
                }
                y++;
            }
        }

        private enumNodeType GetNodeType(char digit) {
            switch (digit) {
                case ' ': return enumNodeType.Empty;
                case '.': return enumNodeType.Open;
                case '#': return enumNodeType.Wall;
                default: throw new NotImplementedException();
            }
        }

        private class State {
            public Dictionary<int, Dictionary<int, Node>> Nodes { get; set; }
            public List<Node> NodesAll { get; set; }
            public CubeEdge CubeFace { get; set; }
            public List<CubeEdge> CubeFaceAll { get; set; }
            public int CubeLength { get; set; }
            public int CurrentDirection { get; set; }
            public Node CurrentNode { get; set; }
            public List<Move> Moves { get; set; }
            public int MaxX { get; set; }
            public int MaxY { get; set; }
        }

        public enum enumNodeType {
            Empty,
            Open,
            Wall
        }

        private class Node {
            public enumNodeType NodeType { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public Node[] Edges { get; set; }
            public int[] DirectionAdjust { get; set; }
        }

        private class Move {
            public int Spaces { get; set; }
            public int Direction { get; set; }
        }

        private class CubeEdge {
            public int X { get; set; }
            public int Y { get; set; }
            public CubeEdge[] Edges { get; set; }
        }
    }
}
