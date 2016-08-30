using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem94 : IProblem {
        private ulong _sum = 0;

        public string ProblemName {
            get { return "94: Almost equilateral triangles"; }
        }

        public string GetAnswer() {
            Solve(10000000);
            return _sum.ToString();
        }

        private void Solve(ulong max) {
            ulong p = 12;
            max = (max * 5 / 6) + 1;
            do {
                TryTriangle(p, max);
                p += 2;
            } while (p <= max);
        }

        private void TryTriangle(ulong s, ulong max) {
            for (ulong m = 2; m <= Math.Sqrt(s); m++) {
                if (s % (2 * m) == 0) {
                    ulong odd = s / (2 * m);
                    ulong k = m + 1;
                    if (k % 2 == 0) {
                        k++;
                    }
                    while (k <= odd && k < 2 * m) {
                        if (odd % k == 0 && IsGCDOne(m, k)) {
                            ulong n = k - m;
                            ulong d = s / (2 * m * k);
                            ulong a = ((m * m) - (n * n)) * d;
                            ulong b = 2 * m * n * d;
                            ulong c = ((m * m) + (n * n)) * d;
                                                        
                            if (a * 2 == c + 1) {
                                ulong sum = (c * 2) + (a * 2);
                                if (sum <= max) {
                                    _sum += sum;
                                }
                            }
                            if (a * 2 == c - 1) {
                                ulong sum = (c * 2) + (a * 2);
                                if (sum <= max) {
                                    _sum += sum;
                                }
                            }
                            if (b * 2 == c + 1) {
                                ulong sum = (c * 2) + (b * 2);
                                if (sum <= max) {
                                    _sum += sum;
                                }
                            }
                            if (b * 2 == c - 1) {
                                ulong sum = (c * 2) + (b * 2);
                                if (sum <= max) {
                                    _sum += sum;
                                }
                            }
                        }
                        k += 2;
                    }
                }
            }
        }

        private bool IsGCDOne(ulong a, ulong b) {
            ulong max = 0;
            if (a > b) {
                if (a % b == 0) {
                    return false;
                }
                max = (ulong)Math.Sqrt(b);
            } else {
                if (b % a == 0) {
                    return false;
                }
                max = (ulong)Math.Sqrt(a);
                a = a + b;
                b = a - b;
                a = a - b;
            }
            for (ulong divisor = 2; divisor <= max; divisor++) {
                if (b % divisor == 0) {
                    if (a % divisor == 0) {
                        return false;
                    } else if (a % (b / divisor) == 0) {
                        return false;
                    }
                }
            }
            return true;
        }

        //public string GetAnswer() {
        //    Solve(1000000000);
        //    return _sum.ToString();
        //}

        //private void Solve(double max) {
        //    double num = 3;
        //    do {
        //        TryTriangle(num, num, num + 1, max);
        //        TryTriangle(num, num, num - 1, max);
        //        num += 2;
        //    } while (num * 2 + num - 1 <= max);
        //}

        //private void TryTriangle(double a, double b, double c, double max) {
        //    double p = (a + b + c) / 2;
        //    double area = Math.Sqrt(p * (p - a) * (p - b) * (p - c));

        //    if ((ulong)area == area) {
        //        ulong sum = (ulong)(a + b + c);
        //        if (sum <= max) {
        //            _sum += sum;
        //        }
        //    }
        //}
    }
}
