using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem221 : ProblemBase {
        /*
             Set a = p, b = q, c = r. Assume a <= b <= c. I found that (b) cannot be greater than or equal to a*2. I also found that
             b and c are always negative. So I loop through all values of (a). For each value, I loop through all values of (b) from
             a*2-1 to a+1. For each value of (b), I find 1/a - 1/b. I also found that once you have (a) and (b), (c) is always the
             interger value of 1/a - 1/b. So now we have (a,b,c). Because of possible overflow, instead of calculating a*b*c, I
             first check if ulong.maxvalue / (a * b) >= c. If it's not, then the product will overflow and we can ignore. If it is,
             then I add the product to an ordered list if the resulting fraction numerator of (1/a - 1/b - 1/c) is 1. To save a bit
             of time, if the ordered list has more items than 150,000, I only add the product to the list if it's less than the
             150,000th item. I do this because even though (a,b) gradually increases, the product can go low or high. I continue
             to do this until the lowest possible product of (a) is more than the 150,00th item I've found.

            FYI, this method doesn't find (1,2,3), so I return the 149,999th instead.

            Not very goood method, takes about an hour.
         */

        public override string ProblemName {
            get { return "221: Alexandrian Integers"; }
        }

        public override string GetAnswer() {
            return Solve(150000).ToString();
        }

        private ulong Solve(int count) {
            var results = new SortedList<ulong, ulong>();
            ulong a = 1;
            do {
                var lowest = ulong.MaxValue;
                for (ulong b = a * 2 - 1; b > a; b--) {
                    _fract.X = 1;
                    _fract.Y = a;
                    _fract.Subtract(1, b);
                    var c = _fract.Y / _fract.X + 1;
                    _fract.Subtract(1, c);
                    if (ulong.MaxValue / (a * b) >= c) {
                        var prod = a * b * c;
                        if (prod < lowest) {
                            lowest = prod;
                        }
                        if (_fract.X == 1 && prod == _fract.Y) {
                            if (results.Count <= count - 2) {
                                results.Add(prod, prod);
                            } else if (results.Last().Key > prod) {
                                results.Add(prod, prod);
                            }
                        }
                    }
                }
                if (results.Count > count - 2 && lowest >= results.ElementAt(count - 2).Key) {
                    return results.ElementAt(count - 2).Key;
                }
                a++;
            } while (true);
        }

        private bool FindPQR(ulong num) {
            var divisors = new List<ulong>();
            ulong max = (ulong)Math.Sqrt(num);
            for (ulong d = 1; d <= max; d++) {
                if (num % d == 0) {
                    divisors.Add(d);
                    divisors.Add(num / d);
                }
            }
            divisors = divisors.Distinct().OrderBy(x => x).ToList();
            for (int index1 = 0; index1 < divisors.Count; index1++) {
                var d1 = divisors[index1];
                for (int index2 = index1; index2 < divisors.Count; index2++) {
                    var d2 = divisors[index2];
                    if (d1 * d2 > num) {
                        break;
                    }
                    if (d2 != 1) {
                        for (int index3 = index2; index3 < divisors.Count; index3++) {
                            var d3 = divisors[index3];
                            if (d1 * d2 * d3 > num) {
                                break;
                            } else if (d1 * d2 * d3 == num) {
                                if (IsGood(d1, d2, d3)) {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private Fraction _fract = new Fraction();
        private bool IsGood(ulong d1, ulong d2, ulong d3) {
            _fract.X = 1;
            _fract.Y = d1;
            _fract.Subtract(1, d2);
            _fract.Subtract(1, d3);
            return _fract.X == 1 && _fract.Y == d1 * d2 * d3;
        }
    }
}