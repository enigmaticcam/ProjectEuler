using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem170 : ProblemBase {
        /*
            I was not expecting this to be so easy! The first thing I do is loop through all possible pandigital numbers,
            starting with the highest. This is easily done with a recursive function that uses bitwise notation to ensure
            each digit is used only once. Then for each number found, I loop through all possible ways to break into two
            parts. For each way to break into two parts, I check if the GCD for each part is more than one. If so, I
            then divide the part by the GCD. I then test if part 1, part 2, and the GCD all together use each digit
            exactly once, again using bitwise notation. If it is, then I found my number!

            I suspect this was easy because I did not realize the part could be broken into more than two, but the solution
            uses only two. Though, if I had tried breaking into 2 or more parts, I don't think that would have been too hard
            and taken all that much more time. I also only use the GCD of each part. Perhaps some other number less than the
            GCD would've worked, but yet again it didn't.
         */

        public override string ProblemName {
            get { return "170: Find the largest 0 to 9 pandigital that can be formed by concatenating products"; }
        }

        public override string GetAnswer() {
            return FindNums(0, 0).ToString();
        }

        private ulong _fullBits = (ulong)Math.Pow(2, 10) - 1;
        private ulong FindNums(ulong num, ulong bits) {
            for (ulong current = 10; current > 0; current--) {
                ulong bit = (ulong)Math.Pow(2, current - 1);
                if ((bits & bit) == 0) {
                    bits += bit;
                    num = num * 10 + current - 1;
                    if (bits != _fullBits) {
                        var sub = FindNums(num, bits);
                        if (sub != 0) {
                            return sub;
                        }
                    } else {
                        if (TryNum(num)) {
                            return num;
                        }
                    }
                    bits -= bit;
                    num /= 10;
                }
            }
            return 0;
        }

        private bool TryNum(ulong num) {
            for (int rootOf10 = 1; rootOf10 <= 9; rootOf10++) {
                ulong powerOf10 = (ulong)Math.Pow(10, rootOf10);
                var a = num % powerOf10;
                var b = num / powerOf10;
                if ((ulong)Math.Log10(a) + (ulong)Math.Log10(b) == 8) {
                    var gcd = GCD.GetGCD(a, b);
                    if (gcd != 1) {
                        var x = a / gcd;
                        var y = b / gcd;
                        if (IsGood(x, y, gcd)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private ulong[] _nums = new ulong[3];
        private bool IsGood(ulong num1, ulong num2, ulong num3) {
            ulong bits = 0;
            _nums[0] = num1;
            _nums[1] = num2;
            _nums[2] = num3;
            foreach (var num in _nums) {
                var sub = num;
                while (sub > 0) {
                    ulong remainder = sub % 10;
                    ulong bit = (ulong)Math.Pow(2, remainder);
                    if ((bits & bit) != 0) {
                        return false;
                    }
                    bits += bit;
                    sub /= 10;
                }
            }
            return bits == _fullBits;
        }
    }
}