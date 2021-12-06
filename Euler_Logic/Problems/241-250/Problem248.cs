using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem248 : ProblemBase {
        public override string ProblemName {
            get { return "248: Numbers for which Euler’s totient function equals 13!"; }
        }

        public override string GetAnswer() {
            var fact = new FactorialWithHashULong();
            var x = fact.Get(13);
            return Test().ToString();
        }

        private ulong Test() {
            int count = 0;
            ulong num = 6227180928;
            ulong fact = new FactorialWithHashULong().Get(13);
            do {
                num++;
                if (Totient(num, fact) == fact) {
                    count++;
                }
            } while (count < 150000);
            return num;
        }

        private ulong Totient(ulong num, ulong fact) {
            var primes = new PrimeSieve((ulong)Math.Sqrt(num));
            var fraction = new Fraction(num, 1);
            foreach (var prime in primes.Enumerate) {
                if (num % prime == 0) {
                    fraction.Multiply(prime - 1, prime);
                    do {
                        num /= prime;
                    } while (num % prime == 0);
                    if (fraction.X < fact) {
                        return 0;
                    }
                }
            }
            if (num != 1) {
                fraction.Multiply(num - 1, num);
            }
            return fraction.X;
        }
    }
}
