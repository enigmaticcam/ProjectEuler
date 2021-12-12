using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem12 : AdventOfCodeBase {
        private Dictionary<string, Node> _nodes;
        

        public override string ProblemName {
            get { return "Advent of Code 2021: 12"; }
        }

        public override string GetAnswer() {
            GetNodes(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            var start = _nodes["start"];
            FindRecursive(start, start.Bit, false, false);
            return _count;
        }

        private int Answer2() {
            var start = _nodes["start"];
            FindRecursive(start, start.Bit, true, false);
            return _count;
        }

        private int _count;
        private void FindRecursive(Node node, ulong bits, bool canRevisit, bool didRevisit) {
            foreach (var edge in node.Edges) {
                bool canTry = true;
                var exists = (bits & edge.Bit);
                bool setRevisit = didRevisit;
                if (!edge.IsBig && exists != 0) {
                    if (canRevisit) {
                        if (!didRevisit) {
                            setRevisit = true;
                        } else {
                            canTry = false;
                        }
                    } else {
                        canTry = false;
                    }
                }
                if (canTry && edge.Name != "start") {
                    if (edge.Name == "end") {
                        _count++;
                    } else {
                        if (exists == 0) {
                            FindRecursive(edge, bits + edge.Bit, canRevisit, setRevisit);
                        } else {
                            FindRecursive(edge, bits, canRevisit, setRevisit);
                        }
                    }
                }
            }
        }

        private void GetNodes(List<string> input) {
            ulong powerOf2 = 1;
            _nodes = new Dictionary<string, Node>();
            foreach (var line in input) {
                var split = line.Split('-');
                if (!_nodes.ContainsKey(split[0])) {
                    _nodes.Add(split[0], new Node() {
                        Edges = new List<Node>(),
                        Name = split[0],
                        IsBig = IsBig(split[0]),
                        Bit = powerOf2
                    });
                    powerOf2 *= 2;
                }
                if (!_nodes.ContainsKey(split[1])) {
                    _nodes.Add(split[1], new Node() {
                        Edges = new List<Node>(),
                        Name = split[1],
                        IsBig = IsBig(split[1]),
                        Bit = powerOf2
                    });
                    powerOf2 *= 2;
                }
                _nodes[split[0]].Edges.Add(_nodes[split[1]]);
                _nodes[split[1]].Edges.Add(_nodes[split[0]]);
            }
        }

        private bool IsBig(string name) {
            if (name == "start" || name == "end") {
                return false;
            } else { 
                return (int)name[0] == (int)name.ToUpper()[0];
            }
        }

        private class Node {
            public string Name { get; set; }
            public bool IsBig { get; set; }
            public List<Node> Edges { get; set; }
            public ulong Bit { get; set; }
        }
    }
}
