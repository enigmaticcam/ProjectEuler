using Euler_Logic.Helpers;

namespace Euler_Logic.Problems {
    public class Problem7 : ProblemBase {
        /*
            A simple primesieve will yield the 10001st prime.
        */

        public override string ProblemName {
            get { return "7: 10001st prime"; }
        }

        public override string GetAnswer() {
            return Solve(10001).ToString();
        }

        private ulong Solve(int primeIndex) {
            var primes = new PrimeSieve(1000000);
            return primes[primeIndex - 1];
        }
    }
}
