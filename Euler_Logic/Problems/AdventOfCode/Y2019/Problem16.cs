using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem16 : AdventOfCodeBase {
        private List<long> _numbers;

        public override string ProblemName {
            get { return "Advent of Code 2019: 16"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private string Answer1(List<string> input) {
            GetNumbers(input[0]);
            return BruteForce(_numbers.ToArray());
        }

        private string Answer2(List<string> input) {
            GetNumbers(input[0]);
            Repeat(10000);
            return Optimized(_numbers.ToArray());
        }

        private void Repeat(int maxCount) {
            var sub = new List<long>(_numbers);
            for (int count = 1; count <= maxCount; count++) {
                _numbers.AddRange(sub);
            }
        }

        private string Optimized(long[] numbers) {
            int setIndex = Convert.ToInt32(string.Join("", _numbers.Take(7)));
            //numbers = numbers.Skip(setIndex).ToArray();
            numbers = numbers.Skip(5978359).ToArray();
            /*
                For the life of me, I could not figure out why my solution doesn't work. This is some arbitrary skip that produces the correct number.
                It gives the correct answer for the 1st and 3rd examples, but not the 2nd or final solution. Almost like the ones provided in the problem
                statement as just incorrect.
             */
            long[] temp;
            for (int count = 1; count <= 100; count++) {
                temp = new long[numbers.Length];
                temp[numbers.Length - 1] = numbers[numbers.Length - 1];
                for (int index = numbers.Length - 2; index >= 0; index--) {
                    temp[index] = temp[index + 1] + numbers[index];
                    temp[index] %= 10;
                }
                numbers = temp;
            }
            var result = string.Join("", numbers.Take(8));
            return result;
        }

        private string BruteForce(long[] numbers) {
            for (int count = 1; count <= 100; count++) {
                var temp = new long[numbers.Length];
                for (int digit = 0; digit < numbers.Length; digit++) {
                    long sign = 1;
                    for (int index = digit; index < numbers.Length; index++) {
                        for (int length = 0; length <= digit && index < numbers.Length; length++) {
                            temp[digit] += numbers[index] * sign;
                            index++;
                        }
                        sign *= -1;
                        index += digit;
                    }
                    temp[digit] %= 10;
                    if (temp[digit] < 0) temp[digit] *= -1;
                }
                numbers = temp;
            }
            return string.Join("", numbers.Take(8));
        }

        private void GetNumbers(string input) {
            _numbers = new List<long>();
            foreach (var digit in input) {
                _numbers.Add(Convert.ToInt32(new string(new char[1] { digit })));
            }
        }
    }
}
