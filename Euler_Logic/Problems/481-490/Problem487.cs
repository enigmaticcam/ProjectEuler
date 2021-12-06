using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem487 : ProblemBase {
        private IEnumerable<ulong> _primes;

        public override string ProblemName {
            get { return "668: Square root Smooth Numbers"; }
        }

        public override string GetAnswer() {
            Solve();
            return "";
        }

        private void GetPrimes(ulong start, ulong length) {
            var primes = new PrimeSieveSegmented(start, length);
            _primes = primes.Primes;
        }

        private void Solve() {
            GetPrimes((ulong)Math.Pow(10, 9) * 2, 2000);
            var primeMods = new List<List<ulong>>();
            foreach (var prime in _primes) {
                var mod = new List<ulong>();
                primeMods.Add(mod);
                ulong k = 1;
                ulong v = 0;
                do {
                    v = Power.Exp(k, 10000, 19);
                    if (v != 0) {
                        mod.Add(v);
                    }
                    k++;
                } while (v != 0);
            }
            bool stop = true;
        }
    }
}
