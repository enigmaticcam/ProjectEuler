using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem171 : ProblemBase {
        public override string ProblemName {
            get { return "171: Finding numbers for which the sum of the squares of the digits is a square"; }
        }

        public override string GetAnswer() {
            return Solve1(3).ToString();
            return "";
        }

        private ulong _sum = 0;
        private ulong Solve2(int maxDigit) {
            BuildSquares(maxDigit);
            BuildFactorials();
            FindCombos(maxDigit, 1, new int[9], 0, 1000000000, maxDigit);
            return _sum;
        }

        private void FindCombos(int remaining, int currentDigit, int[] digits, ulong currentSum, ulong mod, int maxDigit) {
            if (currentDigit < 9) {
                FindCombos(remaining, currentDigit + 1, digits, currentSum, mod, maxDigit);
            }
            for (int next = 1; next <= remaining; next++) {
                digits[currentDigit - 1] = next;
                currentSum += (ulong)(currentDigit * currentDigit);
                if (_squares.Contains(currentSum)) {
                    FoundOne(digits, currentSum, mod, maxDigit);
                }
                if (remaining - next > 0 && currentDigit < 9) {
                    FindCombos(remaining - next, currentDigit + 1, digits, currentSum, mod, maxDigit);
                }
            }
            digits[currentDigit - 1] = 0;
        }

        private ulong _count = 0;
        private void FoundOne(int[] digits, ulong currentSum, ulong mod, int maxDigit) {
            _count++;
            ulong last = 1;
            ulong count = 0;
            int totalDigits = digits.Sum();
            int remaining = totalDigits;
            for (int digit = 1; digit <= 9; digit++) {
                if (digits[digit - 1] > 0) {
                    if (count == 0) {
                        count = 1;
                    } else {
                        count = (count * last) % mod;
                    }
                    last = Choose(remaining, digits[digit - 1]);
                    remaining -= digits[digit - 1];
                }
            }
            _sum = (((count * currentSum) % mod) + _sum) % mod;
            CalcZeros(count, currentSum, mod, totalDigits, maxDigit);
        }

        private void CalcZeros(ulong count, ulong currentSum, ulong mod, int totalDigits, int maxDigit) {
            ulong sum = 0;
            for (int zeroCount = 1; zeroCount + totalDigits <= maxDigit; zeroCount++) {
                sum += Choose(totalDigits + zeroCount - 1, totalDigits - 1);
            }
            _sum = (((((count * currentSum) % mod) * sum) % mod) + _sum) % mod;
        }

        private ulong Choose(int x, int y) {
            return _factorial[x] / _factorial[y] / _factorial[x - y];
        }

        private Dictionary<int, ulong> _factorial = new Dictionary<int, ulong>();
        private void BuildFactorials() {
            _factorial.Add(0, 1);
            _factorial.Add(1, 1);
            ulong num = 1;
            for (ulong next = 2; next <= 20; next++) {
                num = (num * next);
                _factorial.Add((int)next, num);
            }
        }

        private ulong Solve1(int maxDigit) {
            BuildSquares(maxDigit);
            ulong sum = 0;
            ulong max = (ulong)Math.Pow(10, maxDigit);
            for (ulong num = 1; num < max; num++) {
                var digitSum = GetDigitSum(num);
                if (_squares.Contains(digitSum)) {
                    sum = (sum + num) % 1000000000;
                }
            }
            return sum;
        }

        private ulong GetDigitSum(ulong num) {
            ulong sum = 0;
            do {
                var remainder = num % 10;
                sum += remainder * remainder;
                num /= 10;
            } while (num > 0);
            return sum;
        }

        private HashSet<ulong> _squares = new HashSet<ulong>();
        private void BuildSquares(int maxDigit) {
            ulong max = (ulong)Math.Sqrt(maxDigit * 81);
            for (ulong n = 1; n <= max; n++) {
                _squares.Add(n * n);
            }
        }
    }
}
