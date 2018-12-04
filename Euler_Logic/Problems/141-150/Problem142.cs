using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem142 : ProblemBase {
        private HashSet<uint> _squares = new HashSet<uint>();

        /*
            Generate all squares up to some limit and save them in a hash. If x + y = some square, then loop through the squares and for
            each one loop through from 1 to square/2 to derive y, therefore knowing that x + y = some square. Test if x - y = some square. 
            If so, then look for z. To find z, again loop through all squares and derive z by subtracting x from the square, knowing that
            x + z = some square. If z > x we can break. Otherwise, do the other checks: x - z, y + z, and y - z.
         */

        public override string ProblemName {
            get { return "142: Perfect Square Collection"; }
        }

        public override string GetAnswer() {
            BuildSquares(1500);
            return LookForXY().ToString();
        }

        private void BuildSquares(uint max) {
            for (uint num = 1; num <= max; num++) {
                _squares.Add(num * num);
            }
        }

        private uint LookForXY() {
            foreach (var square in _squares) {
                for (uint y = 2; y <= square / 2; y++) {
                    uint x = square - y;
                    if (_squares.Contains(x - y)) {
                        uint z = LookForZ(x, y);
                        if (z != 0) {
                            return x + y + z;
                        }
                    }
                }
            }
            return 0;
        }

        private uint LookForZ(uint x, uint y) {
            foreach (var square in _squares) {
                if (square > x) {
                    uint z = square - x;
                    if (z > x) {
                        break;
                    }
                    if (_squares.Contains(x - z) && _squares.Contains(y + z) && _squares.Contains(y - z)) {
                        return z;
                    }
                }
            }
            return 0;
        }
    }
}
