using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem349 : ProblemBase {
        private Dictionary<long, Dictionary<long, bool>> _gridIsBlack = new Dictionary<long, Dictionary<long, bool>>();
        private int[] _104Counts;
        private ulong _blackCount = 0;
        private ulong _moveCount = 0;

        /*
            According to the wikipedia article on Langton's ant, the movement of the ant is pseudo-random until around
            10,000 steps. Then it follows an unbreakable pattern every 104 steps. A brute force approach shows that
            once this pattern is reached, not only is the number of black squares increased by exaclty 12 every 104 steps,
            but the exact pattern as to which step within those 104 steps adds or subtracts a black square is exactly
            the same for every 104.

            So what I do is brute force the movement of the ant. During its movement, every step % 104 I save whether
            that step added or subtracted a black square. If one set of 104 steps added/subtracted exactly the same as
            the next 104 steps, then I know I've reached the pattern.

            From this point, it's simply a matter of figuring out how many times I would have to do this until I reach
            10^18. Just subtract the number of moves I've already made from 10^18 and divide it by 104. Then multiply
            that by 12. If there are any remaining steps, just follow the pattern saved earlier to determine exactly
            how many black squares there are at the end.
         */

        public override string ProblemName {
            get { return "349: Langton's ant"; }
        }

        public override string GetAnswer() {
            _104Counts = new int[104];
            Find104();
            return Solve((ulong)Math.Pow(10, 18)).ToString();
        }

        private void Find104() {
            _gridIsBlack = new Dictionary<long, Dictionary<long, bool>>();
            long direction = 0;
            long x = 0;
            long y = 0;
            int count = 0;
            bool is104Good = true;
            do {
                if (!_gridIsBlack.ContainsKey(x)) {
                    _gridIsBlack.Add(x, new Dictionary<long, bool>());
                }
                if (!_gridIsBlack[x].ContainsKey(y)) {
                    _gridIsBlack[x].Add(y, false);
                }
                if (_gridIsBlack[x][y]) {
                    _gridIsBlack[x][y] = false;
                    direction++;
                    _blackCount--;
                    if (is104Good && _104Counts[count] != -1) {
                        is104Good = false;
                    }
                    _104Counts[count] = -1;
                } else {
                    _gridIsBlack[x][y] = true;
                    direction--;
                    _blackCount++;
                    if (is104Good && _104Counts[count] != 1) {
                        is104Good = false;
                    }
                    _104Counts[count] = 1;
                }
                count = (count + 1) % 104;
                _moveCount++;
                if (count == 0) {
                    if (is104Good) {
                        break;
                    } else {
                        is104Good = true;
                    }
                }
                if (Math.Abs(direction % 4) == 0) {
                    y++;
                } else if (Math.Abs(direction % 4) == 1) {
                    x--;
                } else if (Math.Abs(direction % 4) == 2) {
                    y--;
                } else {
                    x++;
                }
            } while (true);
        }

        private ulong Solve(ulong max) {
            ulong totalPer104 = (ulong)_104Counts.Sum();
            ulong total = (max - _moveCount) / 104 * 12;
            int remainder = (int)((max - _moveCount) % 104);
            for (int index = 0; index < remainder; index++) {
                total += (ulong)_104Counts[index];
            }
            return total + _blackCount;
        }
    }
}
