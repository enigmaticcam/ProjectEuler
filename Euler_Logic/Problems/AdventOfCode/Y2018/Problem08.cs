using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem08 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 8"; }
        }

        public override string GetAnswer() {
            return Answer2();
        }

        private List<int> _data;
        private string Answer1() {
            _data = GetData();
            BuildTree();
            return _nodes.Select(x => x.Metadata.Sum()).Sum().ToString();
        }

        private string Answer2() {
            _data = GetData();
            var top = BuildTree();
            return CalcValue(top).ToString();
        }

        private int _index;
        private List<Node> _nodes;
        private Node BuildTree() {
            _nodes = new List<Node>();
            _index = 0;
            return GetNode();
        }

        private int CalcValue(Node node) {
            if (!node.IsValueCalculated) {
                if (node.ChildNodes.Count == 0) {
                    node.Value = node.Metadata.Sum();
                } else {
                    foreach (var metadata in node.Metadata) {
                        if (metadata != 0 && metadata - 1 < node.ChildNodes.Count) {
                            node.Value += CalcValue(node.ChildNodes[metadata - 1]);
                        }
                    }
                }
                node.IsValueCalculated = true;
            }
            return node.Value;
        }

        private Node GetNode() {
            var node = new Node();
            int subNodeCount = _data[_index];
            int metadataCount = _data[_index + 1];
            _index += 2;
            for (int sub = 1; sub <= subNodeCount; sub++) {
                node.ChildNodes.Add(GetNode());
            }
            for (int metadata = 1; metadata <= metadataCount; metadata++) {
                node.Metadata.Add(_data[_index]);
                _index++;
            }
            _nodes.Add(node);
            return node;
        }

        private List<int> GetData() {
            return Input().First().Split(' ').Select(x => Convert.ToInt32(x)).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2"
            };
        }

        private class Node {
            public Node() {
                Metadata = new List<int>();
                ChildNodes = new List<Node>();
            }

            public List<int> Metadata { get; set; }
            public List<Node> ChildNodes { get; set; }
            public int Value { get; set; }
            public bool IsValueCalculated { get; set; }
        }
    }
}
