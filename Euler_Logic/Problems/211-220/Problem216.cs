using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem216 : ProblemBase {
        private PrimeSieve _primes;

        public override string ProblemName {
            get { return "216: Investigating the primality of numbers of the form 2n2-1"; }
        }

        public override string GetAnswer() {
            //_primes = new PrimeSieve(50000000);
            return Solve(50000000).ToString();
            return "";
        }

        private int Solve(ulong maxN) {
            var primes = new PrimeSieve(2 * maxN);
            foreach (var prime in primes.Enumerate) {
                if (prime != 2) {
                    var startN = (ulong)Math.Sqrt((prime + 1) / 2);
                    var offset = new List<ulong>();
                    for (ulong count = 1; count <= prime; count++) {
                        var n = (startN + count) * (startN + count) * 2 - 1;
                        if (n % prime == 0) {
                            offset.Add(startN + count);
                        }
                    }
                }
            }
            return 0;
        }

        private int BruteForce(ulong maxN) {
            int count = 0;
            var list = new List<ulong>();
            for (ulong n = 2; n <= maxN; n++) {
                if (PrimalityULong.IsPrime(2 * n * n - 1)) {
                    count++;
                    list.Add(n);
                }
            }
            return count; 
        }
    }
}
