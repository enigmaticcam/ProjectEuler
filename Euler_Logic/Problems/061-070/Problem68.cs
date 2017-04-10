using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem68 : ProblemBase {
        private List<List<int>> _nodePaths = new List<List<int>>();
        private int[] _nodes;
        private int _maxNum;
        private List<string> _permutations = new List<string>();
        private int _lowestNodePath;
        private int _maxKeyLength;

        public override string ProblemName {
            get { return "68: Magic 5-gon ring"; }
        }

        public override string GetAnswer() {
            GenerateNodePaths();
            BuildPermutations();
            return GetMaxSet();
        }

        private string GetMaxSet() {
            string best = "";
            foreach (string permutation in _permutations) {
                for (int index = 0; index < permutation.Length; index++) {
                    _nodes[index] = Convert.ToInt32(permutation.Substring(index, 1));
                }
                if (IsGood()) {
                    string key = GetPathString();
                    if (key.Length <= _maxKeyLength && string.Compare(key, best) > 0) {
                        best = key;
                    }
                }
            }
            return best;
        }

        private string GetPathString() {
            int path = _lowestNodePath;
            StringBuilder key = new StringBuilder();
            do {
                foreach (int node in _nodePaths[path]) {
                    key.Append((_nodes[node] + 1).ToString());
                }
                path++;
                if (path == _nodePaths.Count) {
                    path = 0;
                }
            } while (path != _lowestNodePath);
            return key.ToString();
        }

        private bool IsGood() {
            int lastPath = -1;
            int lowestNodeValue = 0;
            int pathIndex = 0;
            foreach (List<int> path in _nodePaths) {
                int sum = 0;
                foreach (int node in path) {
                    sum += _nodes[node];
                }
                if (lastPath == -1) {
                    lastPath = sum;
                    lowestNodeValue = _nodes[path[0]];
                    _lowestNodePath = 0;
                } else {
                    if (lastPath != sum) {
                        return false;
                    }
                    if (_nodes[path[0]] < lowestNodeValue) {
                        _lowestNodePath = pathIndex;
                        lowestNodeValue = _nodes[path[0]];
                    }
                }
                pathIndex++;
            }
            return true;
        }

        private void BuildPermutations() {
            _permutations.Add("0");
            for (int a = 1; a < _maxNum; a++) {
                List<string> tempPerms = new List<string>();
                for (int b = 0; b < _permutations.Count; b++) {
                    for (int c = 0; c <= _permutations[b].Length; c++) {
                        tempPerms.Add(_permutations[b].Insert(c, a.ToString()));
                    }
                }
                _permutations = tempPerms;
            }
        }

        private void GenerateNodePathsTest() {
            _maxNum = 6;
            _nodes = new int[6];
            _maxKeyLength = 9;

            _nodePaths = new List<List<int>>();
            _nodePaths.Add(new List<int>());
            _nodePaths[_nodePaths.Count - 1].Add(0);
            _nodePaths[_nodePaths.Count - 1].Add(1);
            _nodePaths[_nodePaths.Count - 1].Add(2);

            _nodePaths.Add(new List<int>());
            _nodePaths[_nodePaths.Count - 1].Add(3);
            _nodePaths[_nodePaths.Count - 1].Add(2);
            _nodePaths[_nodePaths.Count - 1].Add(4);

            _nodePaths.Add(new List<int>());
            _nodePaths[_nodePaths.Count - 1].Add(5);
            _nodePaths[_nodePaths.Count - 1].Add(4);
            _nodePaths[_nodePaths.Count - 1].Add(1);
        }

        private void GenerateNodePaths() {
            _maxNum = 10;
            _nodes = new int[10];
            _maxKeyLength = 16;

            _nodePaths = new List<List<int>>();
            _nodePaths.Add(new List<int>());
            _nodePaths[_nodePaths.Count - 1].Add(0);
            _nodePaths[_nodePaths.Count - 1].Add(1);
            _nodePaths[_nodePaths.Count - 1].Add(2);

            _nodePaths.Add(new List<int>());
            _nodePaths[_nodePaths.Count - 1].Add(3);
            _nodePaths[_nodePaths.Count - 1].Add(2);
            _nodePaths[_nodePaths.Count - 1].Add(4);

            _nodePaths.Add(new List<int>());
            _nodePaths[_nodePaths.Count - 1].Add(5);
            _nodePaths[_nodePaths.Count - 1].Add(4);
            _nodePaths[_nodePaths.Count - 1].Add(6);

            _nodePaths.Add(new List<int>());
            _nodePaths[_nodePaths.Count - 1].Add(7);
            _nodePaths[_nodePaths.Count - 1].Add(6);
            _nodePaths[_nodePaths.Count - 1].Add(8);

            _nodePaths.Add(new List<int>());
            _nodePaths[_nodePaths.Count - 1].Add(9);
            _nodePaths[_nodePaths.Count - 1].Add(8);
            _nodePaths[_nodePaths.Count - 1].Add(1);
        }
    }
}
