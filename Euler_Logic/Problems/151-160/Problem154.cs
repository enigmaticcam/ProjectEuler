using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem154 : ProblemBase {
        public override string ProblemName {
            get { return "154: Exploring Pascal's pyramid"; }
        }

        public override string GetAnswer() {
            return Solve(200000).ToString();
        }

        private ulong Solve(ulong n) {
            BuildFactors(n);
            return CalcRow(n);
        }

        private Dictionary<ulong, Tuple> _factors = new Dictionary<ulong, Tuple>();
        private void BuildFactors(ulong max) {
            _factors.Add(1, new Tuple());
            for (ulong n = 2; n <= max; n++) {
                var tuple = new Tuple();
                var remainder = n;
                while (remainder % 2 == 0) {
                    remainder /= 2;
                    tuple.Two++;
                }
                while (remainder % 5 == 0) {
                    remainder /= 5;
                    tuple.Five++;
                }
                _factors.Add(n, tuple);
            }
        }

        private ulong CalcRow(ulong n) {
            ulong count = 0;
            var row1 = GetFirstRow(n);
            ulong maxX = n;
            var current = new Tuple();
            foreach (var tuple in row1) {
                ulong subCount = 0;
                if (tuple.Two >= 12 && tuple.Five >= 12) {
                    count++;
                    subCount++;
                }
                current.Two = tuple.Two;
                current.Five = tuple.Five;
                ulong y = 1;
                ulong x = maxX - 1;
                do {
                    if (y == x) {
                        subCount *= 2;
                        break;
                    } else if (y == x - 1) {
                        subCount *= 2;
                    }
                    var factorX = _factors[x];
                    var factorY = _factors[y];
                    current.Two += factorX.Two;
                    current.Five += factorX.Five;
                    current.Two -= factorY.Two;
                    current.Five -= factorY.Five;
                    if (current.Two >= 12 && current.Five >= 12) {
                        subCount++;
                    }
                    if (y == x - 1) {
                        break;
                    }
                    x--;
                    y++;
                } while (true);
                count += subCount;
                maxX--;
            }
            return count;
        }

        private List<Tuple> GetFirstRow(ulong n) {
            var row1 = new List<Tuple>();
            var current = new Tuple();
            ulong y = 1;
            for (ulong x = n; x >= 2; x--) {
                var factorX = _factors[x];
                var factoryY = _factors[y];
                current.Two += factorX.Two;
                current.Five += factorX.Five;
                current.Two -= factoryY.Two;
                current.Five -= factoryY.Five;
                row1.Add(new Tuple() { Two = current.Two, Five = current.Five });
                y++;
            }
            return row1;
        }

        private class Tuple {
            public ulong Two { get; set; }
            public ulong Five { get; set; }
        }
    }
}
