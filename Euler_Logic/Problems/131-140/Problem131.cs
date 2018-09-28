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
            _primes.SievePrimes(999999);
            Solve();
            return "";
        }

        private void Solve() {
            List<Power> powers = new List<Power>();
            ulong root = 1;
            do {
                var newPower = new Power();
                newPower.Root = root;
                newPower.Squared = root * root;
                newPower.Cubed = newPower.Squared * root;
                foreach (var oldPower in powers) {
                    var a = newPower.Cubed - oldPower.Cubed;
                    if (a % oldPower.Squared == 0) {
                        var b = a / oldPower.Squared;
                        if (b <= 999999 && _primes.IsPrime(b)) {
                            bool stop = true;
                        }
                    }
                    

                }
                powers.Add(newPower);
                root++;
            } while (true);
        }

        private class Power {
            public ulong Root;
            public ulong Squared;
            public ulong Cubed;
        }
    }
}
