using System;

namespace Euler_Logic.Problems {
    public class Problem287 : ProblemBase {
        private ulong _n = 0;
        private long _num = 0;
        private long _numMinus1 = 0;
        private long _max = 0;
        private bool _first = true;

        /*
            Apparently the shape of D(n) is a circle of size (n). So a recursive function will only
            need to check the corners. If the corners are the same, return 2. If not, then break the
            current square into four quadrants and return the sum of each plus 1.
         */

        public override string ProblemName {
            get { return "287: Quadtree encoding (a simple compression algorithm)"; }
        }

        public override string GetAnswer() {
            _n = 24;
            _max = (long)Math.Pow(2, _n);
            _num = (long)Math.Pow(2, 2 * _n - 2);
            _numMinus1 = (long)Math.Pow(2, _n - 1);
            return Solve(0, 0, _max).ToString();
        }

        private long Solve(long startX, long startY, long length) {
            if (!_first) {
                long endX = length + startX - 1;
                long endY = length + startY - 1;

                var spotX = (long)Math.Pow(startX - _numMinus1, 2);
                var spotY = (long)Math.Pow(startY - _numMinus1, 2);
                var isWhite = spotX + spotY <= _num;

                spotX = (long)Math.Pow(endX - _numMinus1, 2);
                spotY = (long)Math.Pow(startY - _numMinus1, 2);
                var temp = spotX + spotY <= _num;

                if (isWhite == temp) {
                    spotX = (long)Math.Pow(startX - _numMinus1, 2);
                    spotY = (long)Math.Pow(endY - _numMinus1, 2);
                    temp = spotX + spotY <= _num;

                    if (isWhite == temp) {
                        spotX = (long)Math.Pow(endX - _numMinus1, 2);
                        spotY = (long)Math.Pow(endY - _numMinus1, 2);
                        temp = spotX + spotY <= _num;

                        if (isWhite == temp) {
                            return 2;
                        }
                    }
                }
            } else {
                _first = false;
            }
            if (length == 2) {
                return 9;
            } else {
                var newLength = length / 2;
                var midX = startX + length + 1;
                var midY = startY + length + 1;
                long sum = 1;
                sum += Solve(startX, startY + newLength, newLength);
                sum += Solve(startX + newLength, startY + newLength, newLength);
                sum += Solve(startX, startY, newLength);
                sum += Solve(startX + newLength, startY, newLength);
                return sum;
            }
        }
    }
}