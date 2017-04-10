using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem91 : ProblemBase {
        private HashSet<string> _hash = new HashSet<string>();
        private Dictionary<int, Dictionary<int, int>> _keys = new Dictionary<int, Dictionary<int, int>>();

        public override string ProblemName {
            get { return "91: Right triangles with integer coordinates"; }
        }

        public override string GetAnswer() {
            return Solve(50, 50).ToString();
        }

        private int Solve(int rowMax, int colMax) {
            BuildKeys(rowMax, colMax);
            int sum = 0;
            for (int x1 = 0; x1 <= colMax; x1++) {
                for (int y1 = 0; y1 <= rowMax; y1++) {
                    if (x1 != 0 || y1 != 0) {
                        for (int x2 = 0; x2 <= colMax; x2++) {
                            for (int y2 = 0; y2 <= rowMax; y2++) {
                                string key = Math.Max(_keys[x1][y1], _keys[x2][y2]).ToString() + ":" + Math.Min(_keys[x1][y1], _keys[x2][y2]).ToString();
                                if (!_hash.Contains(key)) {
                                    _hash.Add(key);
                                    if (x2 <= x1 && y2 <= y1) {
                                        // do nothing
                                    } else if (x1 == x2 && x1 == 0) {
                                        // do nothing
                                    } else if (y1 == y2 && y1 == 0) {
                                        // do nothing
                                    } else {
                                        decimal d1 = ((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1));
                                        decimal d2 = ((x2 - 0) * (x2 - 0)) + ((y2 - 0) * (y2 - 0));
                                        decimal d3 = ((x1 - 0) * (x1 - 0)) + ((y1 - 0) * (y1 - 0));
                                        if (d1 > d2 && d1 > d3 && DoesFit(d2, d3, d1)) {
                                            sum += 1;
                                        } else if (d2 > d1 && d2 > d3 && DoesFit(d1, d3, d2)) {
                                            sum += 1;
                                        } else if (DoesFit(d1, d2, d3)) {
                                            sum += 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return sum;
        }

        private void BuildKeys(int rowMax, int colMax) {
            int key = 1;
            for (int x = 0; x <= rowMax; x++) {
                _keys.Add(x, new Dictionary<int, int>());
                for (int y = 0; y <= colMax; y++) {
                    _keys[x].Add(y, key);
                    key++;
                }
            }
        }

        private bool DoesFit(decimal a, decimal b, decimal c) {
            if (a + b == c) {
                return true;
            } else {
                return false;
            }
        }
    }
}
