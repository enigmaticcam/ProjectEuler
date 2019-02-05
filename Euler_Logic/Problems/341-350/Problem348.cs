using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem348 : ProblemBase {
        private List<ulong> _powersOf3 = new List<ulong>();
        private List<ulong> _found = new List<ulong>();

        /*
            First generate a list of all valid powers of 3 up to near 64 bit limit. Then loop through all possible palindromes, 
            (starting at 2, perform a full and a partial mirror to get a palindrome, e.g. 369 = 369963 & 36963). For each
            palindrome, loop through all powers of 3, subtract the power of 3, and test if remainder is a
            perfect square. Stop if powers of 3 exceed palindrome or more than 4 are found.
         */

        public override string ProblemName {
            get { return "348: Sum of a square and a cube"; }
        }

        public override string GetAnswer() {
            FindPowersOf3();
            LoopThroughPalindromes();
            return Solve().ToString();
        }

        private void FindPowersOf3() {
            for (ulong root = 2; root <= 2000000; root++) {
                _powersOf3.Add(root * root * root);
            }
        }

        private void LoopThroughPalindromes() {
            ulong num = 1;
            do {

                ulong sub = num;
                ulong full = num;
                ulong part = num / 10;
                while (sub > 0) {
                    var next = sub % 10;
                    full = full * 10 + next;
                    part = part * 10 + next;
                    sub /= 10;
                }
                if (TestPalindrome(full)) {
                    _found.Add(full);
                }
                if (TestPalindrome(part)) {
                    _found.Add(part);
                }
                if (_found.Count == 5) {
                    break;
                }
                num++;
            } while (true);
        }

        private ulong Solve() {
            ulong sum = 0;
            _found.ForEach(x => sum += x);
            return sum;
        }

        private bool TestPalindrome(ulong num) {
            int count = 0;
            foreach (var powerOf3 in _powersOf3) {
                if (powerOf3 > num) {
                    break;
                }
                ulong result = num - powerOf3;
                ulong root = (ulong)Math.Sqrt(result);
                if (root * root == result) {
                    count++;
                    if (count > 4) {
                        break;
                    }
                }
            }
            return count == 4;
        }
    }
}