using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem86 : IProblem {

        public string ProblemName {
            get { return "86: Cuboid route"; }
        }

        public string GetAnswer() {
            return Solve(1000000).ToString();
        }

        private int Solve(int max) {
            int x = 2;
            int sum = 0;
            do {
                x++;
                for (int y = 1; y <= x * 2; y++) {
                    double z = Math.Sqrt((x * x) + (y * y));
                    if (z == (int)z) {
                        if (y > x) {
                            sum += (y / 2) - (y - x) + 1;
                        } else {
                            sum += y / 2;
                        }

                    }
                }
            } while (sum < max);
            return x;
        }

        private bool IsIntegerShortest(int x, int y, int z) {
            int a = (x * x) + ((y + z) * (y + z));
            int b = (y * y) + ((x + z) * (x + z));
            int c = (z * z) + ((x + y) * (x + y));
            int shortest = Math.Min(Math.Min(a, b), c);
            int root = (int)Math.Sqrt(shortest);
            if (root * root == shortest) {
                return true;
            } else {
                return false;
            }
        }
    }
}
