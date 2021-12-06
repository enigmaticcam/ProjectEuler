using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem147 : ProblemBase {
        public override string ProblemName {
            get { return "147: Rectangles in cross-hatched grids"; }
        }

        public override string GetAnswer() {
            ulong sum = 0;
            BuildCounts(47);
            sum += GetStraightSet(47, 43);
            return "";
        }

        private Dictionary<ulong, Dictionary<ulong, ulong>> _counts = new Dictionary<ulong, Dictionary<ulong, ulong>>();
        private void BuildCounts(ulong maxX) {
            for (ulong gridX = 1; gridX <= maxX; gridX++) {
                _counts.Add(gridX, new Dictionary<ulong, ulong>());
                for (ulong gridY = 1; gridY <= gridX; gridY++) {
                    ulong count = 0;
                    for (ulong blockX = 1; blockX <= gridX; blockX++) {
                        for (ulong blockY = 1; blockY <= gridY; blockY++) {
                            ulong x = gridX - blockX + 1;
                            ulong y = gridY - blockY + 1;
                            count += x * y;
                        }
                    }
                    _counts[gridX].Add(gridY, count);
                }
            }
        }

        private ulong GetStraightSet(ulong maxX, ulong maxY) {
            return _counts[maxX][maxY];
        }

        //private ulong GetDiagonalSet(ulong maxX, ulong maxY) {

        //}
    }
}
