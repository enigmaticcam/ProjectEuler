using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem175 : ProblemBase {
        private Dictionary<BigInteger, Dictionary<int, ulong>> _counts = new Dictionary<BigInteger, Dictionary<int, ulong>>();
        private Dictionary<int, BigInteger> _maxs = new Dictionary<int, BigInteger>();

        public override string ProblemName {
            get { return "175: Fractions involving the number of different ways a number can be expressed as a sum of powers of 2"; }
        }

        public override string GetAnswer() {
            BigInteger max = BigInteger.Pow(10, 25);
            BuildMaxs(max);
            Solve();
            return "";
        }

        private void Solve() {
            var fraction = new Fraction();
            BigInteger n = 2;
            do {
                fraction.X = Fn(n);
                fraction.Y = Fn(n - 1);
                fraction.Reduce();
                if (fraction.X == 13717421 && fraction.Y == 109739369) {
                    bool stop = true;
                }
                n++;
            } while (true);
        }

        private ulong Fn(BigInteger n) {
            //return Solve(max, (int)BigInteger.Log(max, 2)).ToString();
            return Solve(n, (int)BigInteger.Log(n, 2));
        }

        private void BuildMaxs(BigInteger max) {
            int power = 1;
            BigInteger num = 2;
            _maxs.Add(0, 2);
            do {
                _maxs.Add(power, _maxs[power - 1] + num * 2);
                num *= 2;
                power++;
            } while (num <= max);
        }

        private ulong Solve(BigInteger remainder, int powerOf2) {
            if (_maxs[powerOf2] < remainder) {
                return 0;
            }
            if (!_counts.ContainsKey(remainder)) {
                _counts.Add(remainder, new Dictionary<int, ulong>());
            }
            if (!_counts[remainder].ContainsKey(powerOf2)) {
                ulong sum = 0;
                var nextTwo = BigInteger.Pow(2, powerOf2);
                for (int count = 0; count <= 2; count++) {
                    if (nextTwo * count <= remainder) {
                        var nextRemainder = remainder - (nextTwo * count);
                        if (nextRemainder == 0) {
                            sum++;
                        } else if (powerOf2 > 0) {
                            sum += Solve(remainder - (nextTwo * count), powerOf2 - 1);
                        }
                    } else {
                        break;
                    }
                }
                _counts[remainder].Add(powerOf2, sum);
            }
            return _counts[remainder][powerOf2];
        }
    }
}
