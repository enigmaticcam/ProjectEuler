using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem15 : ProblemBase {
        ulong[][] _paths;

        public override string ProblemName {
            get { return "15: Lattice paths"; }
        }

        public override string GetAnswer() {
            return GetLatticePaths(20).ToString();
        }

        private ulong GetLatticePaths(ulong gridSize) {
            _paths = new ulong[gridSize + 1][];
            for (ulong a = 0; a <= gridSize; a++) {
                _paths[a] = new ulong[gridSize + 1];
                for (ulong b = 0; b <= gridSize; b++) {
                    if (a == 0) {
                        _paths[a][b] = 1;
                    } else {
                        ulong sum = 0;
                        for (ulong c = 0; c <= b; c++) {
                            sum += _paths[a - 1][c];
                        }
                        _paths[a][b] = sum;
                    }
                }
            }
            return _paths[gridSize][gridSize];
        }
    }
}
