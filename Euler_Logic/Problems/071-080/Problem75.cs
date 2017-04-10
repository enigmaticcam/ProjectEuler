using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem75 : ProblemBase {

        public override string ProblemName {
            get { return "75: Singular Integer Right Triangles"; }
        }

        public override string GetAnswer() {
            return CalcAllTriangles(1500000).ToString();
        }

        public int CalcAllTriangles(int maxPerimeter) {
            int count = 0;
            for (int s = 12; s <= maxPerimeter; s += 2) {
                if (IsGood(s)) {
                    count++;
                }
            }
            return count;
        }

        private bool IsGood(int s) {
            // https://projecteuler.net/overview=009
            int count = 0;
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
                            int total = a + b + c;
                            count++;
                            if (count > 1) {
                                return false;
                            }
                        }
                        k += 2;
                    }
                }
            }
            if (count == 1) {
                return true;
            } else {
                return false;
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
