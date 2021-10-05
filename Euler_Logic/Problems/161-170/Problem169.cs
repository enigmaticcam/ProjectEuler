using System.Collections.Generic;
using System.Numerics;

namespace Euler_Logic.Problems {
    public class Problem169 : ProblemBase {
        private Dictionary<BigInteger, Dictionary<int, ulong>> _counts = new Dictionary<BigInteger, Dictionary<int, ulong>>();
        private Dictionary<int, BigInteger> _maxs = new Dictionary<int, BigInteger>();

        /*
            I cheated and used BigInteger. This is easily solved with a recurisive function. If the number of ways of
            expressing (n) using powers of 2 up to (x) is f(n, x), then f(n, x) = f(n, x - 1) + f(n - 2^x, x - 1) + 
            f(n - 2(2^x), x - 1). I use a hash of each function call to save previous work. I also precalculate the
            maximum possible value that can be reached for each power of 2.
         */

        public override string ProblemName {
            get { return "169: Exploring the number of different ways a number can be expressed as a sum of powers of 2"; }
        }

        public override string GetAnswer() {
            BigInteger max = BigInteger.Pow(10, 25);
            BuildMaxs(max);
            return Solve(max, (int)BigInteger.Log(max, 2)).ToString();
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