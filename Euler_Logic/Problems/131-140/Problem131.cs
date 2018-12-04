using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem131 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();

        public override string ProblemName {
            get { return "131: Prime cube partnership"; }
        }

        public override string GetAnswer() {
            ulong max = 1000000;
            _primes.SievePrimes(max - 1);
            return Solve(max).ToString();
        }

        private int Solve(ulong max) {
            ulong nBase = 2;
            ulong nPower = 2 * 2 * 2;
            double third = (double)1 / (double)3;
            int count = 2;
            foreach (var prime in _primes.Enumerate) {
                for (ulong increment = 1; increment <= 10; increment++) {
                    ulong newBase = nBase + increment;
                    ulong n = newBase * newBase * newBase;
                    ulong cube = (n * n * n) + (prime * n * n);
                    ulong root = (ulong)Math.Pow((double)cube, third) + 1;
                    if (root * root * root == cube) {
                        nBase = newBase;
                        nPower = nBase * nBase * nBase;
                        count++;
                        break;
                    }
                }
            }
            return count;
        }

        //private List<Number> _cubes = new List<Number>();
        //private int Solve(ulong max) {
        //    ulong root = 2;
        //    _cubes.Add(new Number() {
        //        Cubed = 1,
        //        Root = 1,
        //        Squared = 1
        //    });
        //    do {
        //        ulong squared = root * root;
        //        ulong cubed = squared * root;
        //        foreach (var oldCube in _cubes) {
        //            ulong diff = cubed - oldCube.Cubed;
        //            if (diff % oldCube.Squared == 0) {
        //                ulong result = diff / oldCube.Squared;
        //                if (result < max && _primes.IsPrime(result)) {
        //                    bool found = true;
        //                    _cubes = new List<Number>();
        //                }
        //            }
        //        }
        //        _cubes.Add(new Number() {
        //            Cubed = cubed,
        //            Root = root,
        //            Squared = squared
        //        });
        //        root++;
        //    } while (true);
        //}

        private class Number {
            public ulong Root;
            public ulong Squared;
            public ulong Cubed;
        }
    }
}
