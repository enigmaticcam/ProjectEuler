using System;

namespace Euler_Logic.Problems {
    public class Problem401 : ProblemBase {

        /*
            If (n) is 10, then all the possible divisors for each (n) would be:

            1:  1
            2:  1,2
            3:  1,3
            4:  1,2,4
            5:  1,5
            6:  1,2,3,6
            7:  1,7
            8:  1,2,4,8
            9:  1,3,9
            10: 1,2,5,10

            So the sum of the squares of these divisors would be:
            
            0
            + 10(1^2) 
            + 5(2^2) 
            + 3(3^2) 
            + 2(4^2) 
            + 2(5^2)
            + 1(6^2)
            + 1(7^2)
            + 1(8^2)
            + 1(9^2)
            + 1(10^2)
            
            Interestingly, the sum of the squares of all numbers up to (n) are: n*(n+1)*(2n+1)/6.
            We will call this f(n). so then, if we call f(int(10 / 1)), that gives us the sum of
            all squares up to 10 once. Now we need to do it again, but only up to 5, so that would
            be f(int(10 / 2)). Then up to 3 would be f(int(10 / 3)). We continue to do this until
            f(int(10 / 10)). The sum of all the times we call f(n) will give us the answer.

            We can make this run faster by noticing that both int(10 / 4) and int(10 / 5) are equal
            to 2. So when we first try int(10 / 4), we then try 10 / (10 / 4), which gives us 5.
            We know then that int(10 / 4) will be the same as int(10 / 5), so we just multiply
            f(int(10 / 4)) by 2 and skip f(int(10 / 5)).

            One caveat is that we are trying to find the sum of all divisor squares % 10^9. Since
            f(n) involves dividing by 6, we have a problem in that division doesn't work well when
            handling mods. So instead of dividing by 6, I attempt to divide one of the three parts
            by 2 and then one of the three parts by 3, when each part is divisible by 2 or 3. Then
            I can perform the calculation without division by 6.
            
         */

        public override string ProblemName {
            get { return "401: Sum of squares of divisors"; }
        }

        public override string GetAnswer() {
            return Solve((ulong)Math.Pow(10, 15)).ToString();
        }

        private ulong Solve(ulong max) {
            ulong sum = 1;
            ulong mod = (ulong)Math.Pow(10, 9);
            ulong divideBy = 1;
            do {
                ulong next = max / divideBy;
                ulong x = next;
                ulong y = next + 1;
                ulong z = 2 * next + 1;
                if (x % 3 == 0) {
                    x /= 3;
                } else if (y % 3 == 0) {
                    y /= 3;
                } else if (z % 3 == 0) {
                    z /= 3;
                }
                if (x % 2 == 0) {
                    x /= 2;
                } else if (y % 2 == 0) {
                    y /= 2;
                } else if (z % 2 == 0) {
                    z /= 2;
                }
                x = x % mod;
                y = y % mod;
                z = z % mod;
                var result = (((x * y) % mod) * z) % mod;
                if (next == 1) {
                    sum = (sum + (result * (max - divideBy)) % mod) % mod;
                    return sum;
                }
                var sub = max / next;
                if (divideBy == sub) {
                    sub = next;
                    divideBy++;
                } else {
                    result = (result * (sub - divideBy + 1)) % mod;
                    divideBy = sub + 1;
                }
                sum = (sum + result) % mod;
            } while (true);
        }
    }
}