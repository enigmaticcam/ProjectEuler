
namespace Euler_Logic.Problems {
    public class Problem129 : ProblemBase {

        /*
            For any number n, a simple solution is to start with 1, then multiply by 10,
            add 1, and return the remainder when divded by n. If the remainder is 0, you
            are done. If it is not, then do it again this time starting with the remainder
            (instead of 1). Continue to do this until the remainder is 0.

            You can search for numbers by starting at 3 and going up 2 skipping any number
            that's divisble by 5.

            By itself, this method is too slow to find the first value that exceeds 1
            million. However, looking at the first values that exceed 10, 100, 1000, etc,
            it's found that the first value that exceeds x is more than x. So if we
            want to find the first value that exceeds 1000000, instead of starting at 3,
            we start at the first number above 1000000 that's not divisible by 5 or 10.
        */

        public override string ProblemName {
            get { return "129: Repunit divisibility"; }
        }

        public override string GetAnswer() {
            return Solve(1000000).ToString();
        }

        private ulong Solve(ulong max) {
            ulong num = max;
            while (num % 2 == 0 || num % 5 == 0) {
                num += 1;
            }
            do {
                if (num % 5 != 0) {
                    ulong repunit = GetRepunit(num);
                    if (repunit > max) {
                        return num;
                    }
                }
                num += 2;
            } while (true);
        }

        private ulong GetRepunit(ulong num) {
            ulong remainder = 1;
            ulong count = 1;
            do {
                remainder = ((remainder * 10) + 1) % num;
                count++;
            } while (remainder != 0);
            return count;
        }
    }
}
