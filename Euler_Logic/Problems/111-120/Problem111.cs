using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem111 : ProblemBase {
        private Power _power = new Power();
        private List<ulong> _found = new List<ulong>();

        /*
            I simply brute force each digit 0-9, first looking for primes replacing only
            one digit. If no primes were found, then try looking for primes replacing two
            digits. Employ some basic time savers, such as not looking for numbers
            where the last digit is even or is a five. I do a special case for digit 0.
         */

        public override string ProblemName {
            get { return "111: Primes with runs"; }
        }

        public override string GetAnswer() {
            int length = 10;
            SolveDigitsZero(length);
            SolveDigitsAboveZero(length);
            return Sum().ToString();
        }

        private ulong Sum() {
            ulong sum = 0;
            _found.ForEach(x => sum += x);
            return sum;
        }

        private void SolveDigitsZero(int length) {
            ulong powerOfTen = (ulong)_power.GetPower(10, length - 1);
            for (ulong firstDigit = 1; firstDigit <= 9; firstDigit++) {
                ulong num = powerOfTen * firstDigit;
                if (PrimalityULong.IsPrime(num + 1)) {
                    _found.Add(num + 1);
                }
                if (PrimalityULong.IsPrime(num + 3)) {
                    _found.Add(num + 3);
                }
                if (PrimalityULong.IsPrime(num + 7)) {
                    _found.Add(num + 7);
                }
                if (PrimalityULong.IsPrime(num + 9)) {
                    _found.Add(num + 9);
                }
            }
        }

        private void SolveDigitsAboveZero(int length) {
            for (ulong digit = 1; digit <= 9; digit++) {
                ulong num = GenerateAllDigits(digit, length);
                int remainingDigits = 1;
                do {
                    int count = _found.Count;
                    LookForPrime(length, num, remainingDigits, digit, 0);
                    if (_found.Count != count) {
                        break;
                    }
                    remainingDigits += 1;
                } while (true);
            }
        }

        private ulong GenerateAllDigits(ulong digit, int length) {
            ulong num = digit;
            for (int index = 2; index <= length; index++) {
                num *= 10;
                num += digit;
            }
            return num;
        }

        private void LookForPrime(int length, ulong num, int remainingDigits, ulong currentDigit, int startIndex) {
            for (ulong digit = 0; digit <= 9; digit++) {
                if (digit != currentDigit) {
                    int stop = length - 1;
                    if (digit == 0) {
                        stop -= 1;
                    }
                    if (remainingDigits == 1 && (num % 10) % 2 == 0) {
                        stop = 0;
                    }
                    for (int index = startIndex; index <= stop; index++) {
                        if (index != 0 || (digit % 2 != 0 && digit != 5)) {
                            ulong next = ReplaceDigit(num, index, digit);
                            if (remainingDigits == 1) {
                                if (PrimalityULong.IsPrime(next)) {
                                    _found.Add(next);
                                }
                            } else {
                                LookForPrime(length, next, remainingDigits - 1, currentDigit, index + 1);
                            }
                            
                        }
                    }
                }
            }
        }

        private ulong ReplaceDigit(ulong num, int index, ulong replaceWith) {
            ulong length = (ulong)_power.GetPower(10, index);
            ulong nextLength = (ulong)_power.GetPower(10, index + 1);
            ulong last = num % length;
            ulong first = num - (num % nextLength);
            ulong replaced = replaceWith * length;
            return last + first + replaced;
        }

    }
}
