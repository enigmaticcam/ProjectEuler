using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem86 : IProblem {
        private int _count = 0;
        private HashSet<string> _found = new HashSet<string>();

        public string ProblemName {
            get { return "86: Cuboid route"; }
        }

        public string GetAnswer() {
            return Solve(2059).ToString();
        }

        private int Solve(int max) {
            int s = 12;
            int last = 0;
            do {
                for (int m = 2; m <= Math.Sqrt(s); m++) {
                    if (s % (2 * m) == 0) {
                        int odd = s / (2 * m);
                        int k = m + 1;
                        if (k % 2 == 0) {
                            k++;
                        }
                        while (k <= odd && k < 2 * m) {
                            if (odd % k == 0 && IsGCDOne(m, k)) {
                                int n = k - m;
                                int d = s / (2 * m * k);
                                int a = ((m * m) - (n * n)) * d;
                                int b = 2 * m * n * d;
                                int c = ((m * m) + (n * n)) * d;
                                CountCuboidsForTriangle(a, b, c, max);
                                //int sum = a + b + c;
                                //int min = Math.Min(a, Math.Min(b, c));
                                //int max2 = Math.Max(a, Math.Max(b, c));
                                //int middle = sum - min - max2;
                                //if (middle > last) {
                                //    middle = last;
                                //    if (_found.Count > max) {
                                //        return s;
                                //    }
                                //}
                            }
                            k += 2;
                        }
                    }
                }
                s += 2;
            } while (true);
            
        }

        private void CountCuboidsForTriangle(int a, int b, int hyp, int max) {
            int count = 0;
            do {
                for (int newB = 1; newB < b; newB++) {
                    if (newB > b / 2) {
                        break;
                    }
                    int newC = a;
                    int newA = b - newB;
                    IsIntegerShortest(newA, newB, newC, max);
                }

                a = a + b;
                b = a - b;
                a = a - b;
                count++;
            } while (count <= 1);
        }

        private void IsIntegerShortest(int x, int y, int z, int max) {
            int a = (x * x) + ((y + z) * (y + z));
            int b = (y * y) + ((x + z) * (x + z));
            int c = (z * z) + ((x + y) * (x + y));
            int shortest = Math.Min(Math.Min(a, b), c);
            int root = (int)Math.Sqrt(shortest);
            if (root * root == shortest) {
                int sum = x + y + z;
                int min = Math.Min(x, Math.Min(y, z));
                int max2 = Math.Max(x, Math.Max(y, z));
                int middle = sum - min - max2;
                _found.Add(min + ":" + middle + ":" + max2);
            }
        }

        private bool IsGCDOne(int a, int b) {
            int max = 0;
            if (a > b) {
                if (a % b == 0) {
                    return false;
                }
                max = (int)Math.Sqrt(b);
            } else {
                if (b % a == 0) {
                    return false;
                }
                max = (int)Math.Sqrt(a);
                a = a + b;
                b = a - b;
                a = a - b;
            }
            for (int divisor = 2; divisor <= max; divisor++) {
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
    }
}
