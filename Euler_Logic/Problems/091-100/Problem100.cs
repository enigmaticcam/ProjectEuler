using System;

namespace Euler_Logic.Problems {
    public class Problem100 : ProblemBase {

        /*
            Brute Force results
            4           3
            21	        15
            120	        85
            697	        493
            4060	    2871
            23661	    16731
            137904	    97513
            803761	    568345
            4684660	    3312555
            27304197    19306983
            159140520   112529341
            927538921   655869061
         */

        /*
            If the total number of discs is T, the number of blue discs is A, and the probability is 50%, then the following equation is true:
                
            0.5=(a/T)×((A-1)/(T-1))

            You can rearrange the equation like so:

            0.5(T*T-T)=A*A-A

            It can be seen that 0.5(T*T-T) is slightly less than a perfect square. If A is an integer, then the following must be true:

            x = 0.5(T*T-T)
            (int(Sqrt(x)) + 1)^2 = x      (int function truncates the decimals and returns only the integer portion)

            We can use the above test to determine if any variable T will yield an integer A. Using this, after writing a brute force algorithm
            to see a list of the first few cases where this is true, it can be seen that where A is an integer, T is 5~ times the last time T
            had an integer for A. For example, 21 / 4 = 5.25, 120 / 21 = 5.71, 697 / 120 = 5.81. So rather than brute forcing every number,
            when we find a solution for T containing an integer A, we save the ratio of this T against the previons T, and then multiple
            this T by that ratio. This will get us close to the next one, and we begin incrementing by 1 from there until we find it.

         */

        public override string ProblemName {
            get { return "100: Arranged probability"; }
        }

        public override string GetAnswer() {
            return Solve(2).ToString();
        }

        private decimal Solve(decimal t) {
            decimal last = 1;
            do {
                decimal test = (t * t - t) / 2;
                decimal estimate = (ulong)Math.Sqrt((double)test) + 1;
                decimal final = estimate * estimate - estimate;
                if (test == final) {
                    if (t > 1000000000000) {
                        return estimate;
                    }
                    decimal next = ((ulong)(t * (t / last)));
                    last = t;
                    t = next;
                } else {
                    t++;
                }
            } while (true);
        }
    }
}