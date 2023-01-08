using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem25 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 25";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        private string Answer1(List<string> input) {
            var state = new State() { Snafus = input };
            SetNumbers(state);
            var sum = state.Numbers.Sum();
            return ToSnafu(sum);
        }

        private void SetNumbers(State state) {
            state.Numbers = new List<long>();
            foreach (var snafu in state.Snafus) {
                state.Numbers.Add(FromSnafu(snafu));
            }
        }

        private string ToSnafu(long number) {
            if (number == 0) return "0";
            if (number == 1) return "1";
            if (number == 2) return "2";
            long subNumber = 0;
            long powerOf5 = 1;
            var digits = new List<long>() { 0 };
            int digitIndex = 0;
            bool found = false;

            // forwards
            do {
                int count = 1;
                do {
                    subNumber += powerOf5;
                    digits[digitIndex]++;
                    if (subNumber == number) return ToSnafu(digits);
                    if (subNumber > number) {
                        digits[digitIndex]--;
                        digitIndex--;
                        subNumber -= powerOf5;
                        powerOf5 /= 5;
                        found = true;
                        break;
                    }
                    count++;
                } while (count <= 2);
                if (!found) {
                    digits[digitIndex] = -2;
                    subNumber -= powerOf5 * 4;
                    digitIndex++;
                    digits.Add(0);
                    powerOf5 *= 5;
                }
            } while (!found);

            // backwards
            while (digitIndex >= 0) {
                do {
                    digits[digitIndex]++;
                    subNumber += powerOf5;
                    if (subNumber == number) return ToSnafu(digits);
                    if (subNumber > number) {
                        digits[digitIndex]--;
                        subNumber -= powerOf5;
                        break;
                    }
                } while (true);
                powerOf5 /= 5;
                digitIndex--;
            }
            return "";
        }

        private string ToSnafu(List<long> digits) {
            if (digits.Last() == 0) digits.RemoveAt(digits.Count - 1);
            var snafu = new char[digits.Count];
            for (int index = 0; index < digits.Count; index++) {
                snafu[digits.Count - index - 1] = ToSnafuDigit(digits[index]);
            }
            return new string(snafu.ToArray());
        }

        private char ToSnafuDigit(long digit) {
            switch (digit) {
                case -2: return '=';
                case -1: return '-';
                case 0: return '0';
                case 1: return '1';
                case 2: return '2';
            }
            throw new Exception();
        }

        private long FromSnafu(string text) {
            long powerOf5 = 1;
            long number = 0;
            for (int index = text.Length - 1; index >= 0; index--) {
                var digit = text[index];
                switch (digit) {
                    case '2':
                        number += powerOf5 * 2;
                        break;
                    case '1':
                        number += powerOf5;
                        break;
                    case '-':
                        number += powerOf5 * -1;
                        break;
                    case '=':
                        number += powerOf5 * -2;
                        break;
                }
                powerOf5 *= 5;
            }
            return number;
        }

        private class State {
            public List<string> Snafus { get; set; }
            public List<long> Numbers { get; set; }
        }
    }
}
