using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem138 : ProblemBase {
        public override string ProblemName {
            get { return "138: Special isosceles triangles"; }
        }

        public override string GetAnswer() {
            BruteForce2();
            return "";
        }

        private void BruteForce2() {
            BigInteger index = 3;
            do {
                var square = index * index;
                var next1 = (index - 1) / 2;
                var next2 = (index + 1) / 2;
                var result1 = square + (next1 * next1);
                var result2 = square + (next2 * next2);
                if (IsSquare(result1)) {
                    bool stop = true;
                    index = index * 179411764705882 / 10000000000000;
                    index += index % 2 == 0 ? 1 : 0;
                }
                if (IsSquare(result2)) {
                    bool stop = true;
                    index = index * 179411764705882 / 10000000000000;
                    index += index % 2 == 0 ? 1 : 0;
                }
                index += 2;
            } while (true);
        }

        private bool IsSquare(BigInteger num) {
            var root = SqRtN(num);
            return root * root == num;
        }

        private bool IsSquare(ulong num) {
            ulong root = (ulong)Math.Sqrt(num);
            return root * root == num;
        }

        public static BigInteger SqRtN(BigInteger N) {
            /*++
             *  Using Newton Raphson method we calculate the
             *  square root (N/g + g)/2
             */
            BigInteger rootN = N;
            int count = 0;
            int bitLength = 1; // There is a bug in finding bit length hence we start with 1 not 0
            while (rootN / 2 != 0) {
                rootN /= 2;
                bitLength++;
            }
            bitLength = (bitLength + 1) / 2;
            rootN = N >> bitLength;

            BigInteger lastRoot = BigInteger.Zero;
            do {
                if (lastRoot > rootN) {
                    if (count++ > 1000)                   // Work around for the bug where it gets into an infinite loop
                    {
                        return rootN;
                    }
                }
                lastRoot = rootN;
                rootN = (BigInteger.Divide(N, rootN) + rootN) >> 1;
            }
            while (!((rootN ^ lastRoot).ToString() == "0"));
            return rootN;
        } // SqRtN
    }
}
