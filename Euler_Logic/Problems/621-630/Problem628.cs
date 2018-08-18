using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem628 : ProblemBase {
        private List<int> _pawns = new List<int>();
        private int _usedPositions;
        private int _sum;

        public override string ProblemName {
            get { return "628: Open chess positions"; }
        }

        public override string GetAnswer() {
            int size = 3;
            Initialize(size);
            Solve(size, 0);
            return "";
        }

        private void Initialize(int size) {
            for (int index = 0; index < size; index++) {
                _pawns.Add(0);
            }
        }

        private void Solve(int size, int column) {
            for (int row = 0; row < size; row++) {
                int bit = (int)Math.Pow(2, row);
                if ((_usedPositions & bit) == 0) {
                    _pawns[column] = row;
                    if (column == size - 1) {
                        Calculate();
                    } else {
                        _usedPositions += bit;
                        Solve(size, column + 1);
                        _usedPositions -= bit;
                    }
                }
            }
        }

        private void Calculate() {
            //if (_pawns[0] != 0)
        }
    }
}
