namespace Euler_Logic.Problems {
    public class Problem381 : ProblemBase {
        private bool[] _notPrimes;
        private ulong _sum = 0;

        public override string ProblemName {
            get { return "381: (prime-k) factorial"; }
        }

        /*
            Wilson's theorum (https://en.wikipedia.org/wiki/Wilson%27s_theorem) states that for any prime n: (n - 1)! % n will always return n - 1.
            One of the proofs of this is that you can arrange all the digits below the prime into pairs so that all pairs when multiplied and divided
            by n will return a remainder of 1 with one exception. For example, if n = 11, the digits 1 - 10 can be arranged like so:

            1 * 10 = 10 % 11 = 10
            2 * 6 = 12 % 11 = 1
            3 * 4 = 12 % 11 = 1
            5 * 9 = 45 % 11 = 1
            7 * 8 = 56 % 11 = 1

            10 * 1 * 1 * 1 * 1 = 10

            Knowing this then, what I can do is replace numbers in the above equations with a 1 as I'm moving from n - 1 to n - 5. See example:

            (n - 1)! - We start with this
            (1 * 10) * (2 * 6) * (3 * 4) * (5 * 9) * (7 * 8) = 10

            (n - 2)! - Replace the 10 with a 1
            (1 * 1) * (2 * 6) * (3 * 4) * (5 * 9) * (7 * 8) = 1

            (n - 3)! - Replace the 9 with a 1
            (1 * 1) * (2 * 6) * (3 * 4) * (5 * 1) * (7 * 8) = 5

            (n - 4)! - Replace the 8 with a 1
            (1 * 1) * (2 * 6) * (3 * 4) * (5 * 1) * (7 * 1) = 35 % 11 = 2

            (n - 5)! - Replace the 7 with a 1
            (1 * 1) * (2 * 6) * (3 * 4) * (5 * 1) * (1 * 1) = 5

            The tricky part is finding the inverse: if n = 11 and n - 2 = 9, what number when multiplied by 9 and divded by 11 yields a remainder of 1?
            To figure this out quickly without bruteforce, see chart below:

            (9 * 1) % 11 = 9
            (9 * 2) % 11 = 7
            (9 * 3) % 11 = 5
            (9 * 4) % 11 = 3
            (9 * 5) % 11 = 1
            (9 * 6) % 11 = 10
            (9 * 7) % 11 = 8
            (9 * 8) % 11 = 6
            (9 * 9) % 11 = 4
            (9 * 10) % 11 = 2
            (9 * 11) % 11 = 0

            Starting at 9*11, we know the difference between 9 and 11 is 2, so starting at 0 and going up will always increase by 2 and reset back to 0 once
            we go past 10. So all we need to do is calculate what the remainder will be next after starting at 0. That will be 1. Then we just calculate how
            many numbers it took to get there without looping. Suppose it were 2: then we just do it again, except starting at 2.
         */

        public override string GetAnswer() {
            int max = 100000000;
            SievePrimes(max);
            return _sum.ToString();
        }

        private void SievePrimes(int max) {
            _notPrimes = new bool[max + 1];
            for (int num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    if (num >= 5) {
                        _sum += Factorize((ulong)num);
                    }
                    for (int composite = 2; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
        }

        private ulong Factorize(ulong num) {
            if (num == 5) {
                return 4;
            }

            // n - 1 & n - 2
            ulong sum = 0;

            // n - 3
            ulong inverse = FindInverse(num - 2, num);
            sum = (inverse + sum) % num;
            ulong all = inverse;

            // n - 4
            inverse = FindInverse(num - 3, num);
            all = (all * inverse) % num;
            sum += all % num;

            // n - 5
            inverse = FindInverse(num - 4, num);
            all = (all * inverse) % num;
            sum += all % num;

            return (sum % num);
        }

        private ulong FindInverse(ulong inverseOf, ulong prime) {
            ulong diff = prime - inverseOf;
            ulong count = (prime / diff) + 1;
            ulong remainder = diff - (prime % diff);

            while (remainder != 1) {
                count += ((prime - remainder) / diff) + 1;
                remainder = (diff * count) % prime;
            }
            return prime - count;
        }
    }
}