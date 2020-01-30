using Euler_Logic.Helpers;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem10 : ProblemBase {

        public override string ProblemName {
            get { return "10: Summation of primes"; }
        }

        public override string GetAnswer() {
            return Solve(2000000).ToString();
        }

        private ulong Solve(ulong max) {
            var primes = new PrimeSieve(max - 1);
            ulong sum = 0;
            foreach (var prime in primes.Enumerate) {
                sum += prime;
            }
            return sum;
        }
    }
}
