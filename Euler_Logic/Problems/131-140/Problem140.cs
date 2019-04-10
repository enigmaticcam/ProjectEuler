using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Euler_Logic.Problems {
    public class Problem140 : ProblemBase {
        /*
            I was only able to solve this by seeing how others solved problem 137. First,
            we need to reduce the infinite sequence to a generating function. Without details,
            if S(x) is the sum of the sequence and (x) is the ratio, then starting with S(x):

            - multiply by X
            - add S(x)
            - multiply by X

            A correlation can be made here and the infinite series can be cancelled out,
            yielding: S(x) - x - 4x^2 = x(S(x) + S(x)x) - x^2. This can then be rearranged to:
            (-1 - S(x))x - (3 + S(x))x^2 + S(x) = 0. We now have a quadratic equation. We can
            now therefore use the quadratic equation solver and determine that
            5(x^2) + 14x + 1 must be a perfect square in order for (y) to be an integer.

            This is where we can use https://www.alpertron.com.ar/QUAD.HTM. Plug in the numbers:
            a = 5
            b = 0
            c = -1
            d = 14
            e = 0
            f = 1
            This is therefore 5x^2 - y^2 + 14x + 1 = 0. We are given 6 starting points for X and
            Y, and two recursive functions. After playing around in Excel with these functions,
            it turns out we only need the first one. This recursive function will therefore 
            generate a new golden nugget for every other recursion for each starting point
            where X is the golden nugget and Y is the square of the golden nugget. I
            therefore do this for all six starting points until I find 30 values for X.
         */

        public override string ProblemName {
            get { return "140: Modified Fibonacci golden nuggets"; }
        }

        public override string GetAnswer() {
            Initialize();
            return Solve(30).ToString();
        }

        private void Initialize() {
            _chains.Add(new Tuple(2, -7));
            _chains.Add(new Tuple(0, -1));
            _chains.Add(new Tuple(0, 1));
            _chains.Add(new Tuple(-4, 5));
            _chains.Add(new Tuple(-3, 2));
            _chains.Add(new Tuple(-3, -2));
        }

        private BigInteger Solve(int totalCount) {
            do {
                Next();
            } while (_nums.Count < totalCount);
            BigInteger sum = 0;
            _nums.Take(totalCount).ToList().ForEach(x => sum += x);
            return sum;
        }

        private List<BigInteger> _nums = new List<BigInteger>();
        private List<Tuple> _chains = new List<Tuple>();
        private void Next() {
            foreach (var chain in _chains) {
                var x = chain.X;
                var y = chain.Y;
                x = -9 * chain.X - 4 * chain.Y - 14;
                y = -20 * chain.X - 9 * chain.Y - 28;
                chain.X = x;
                chain.Y = y;
                if (chain.X > 0 && chain.Y > 0) {
                    _nums.Add(chain.X);
                }
            }
        }

        private class Tuple {
            public BigInteger X { get; set; }
            public BigInteger Y { get; set; }
            public Tuple() { }
            public Tuple(BigInteger x, BigInteger y) {
                X = x;
                Y = y;
            }
        }
    }
}