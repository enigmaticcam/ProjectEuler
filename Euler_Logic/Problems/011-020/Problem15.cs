using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem15 : ProblemBase {

        public override string ProblemName {
            get { return "15: Lattice paths"; }
        }

        public override string GetAnswer() {
            return GetLatticePaths(20).ToString();
        }

        private ulong GetLatticePaths(int size) {
            var grid = new ulong[size + 1, size + 1];
            for (int x = size; x >= 0; x--) {
                for (int y = size; y >= 0; y--) {
                    if (x != size || y != size) {
                        if (x == size) {
                            grid[x, y] = 1;
                        } else if (y == size) {
                            grid[x, y] = 1;
                        } else {
                            grid[x, y] = grid[x + 1, y] + grid[x, y + 1];
                        }
                    }
                }
            }
            return grid[0, 0];
        }
    }
}
