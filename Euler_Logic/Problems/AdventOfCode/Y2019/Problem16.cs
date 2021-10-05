using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem16 : AdventOfCodeBase {
        private List<int> _numbers;
        private int _offset;

        public override string ProblemName {
            get { return "Advent of Code 2019: 16"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private string Answer1(List<string> input) {
            GetNumbers(input);
            PerformPhases(100);
            return FirstDigits(8);
        }

        private string Answer2(List<string> input) {
            GetNumbers(input);
            SetOffset();
            SetRepeats(10000);
            PerformPhasesWithRepeatThreaded(100);
            return OffsetDigits();
        }

        private string OffsetDigits() {
            var text = new StringBuilder();
            for (int count = 0; count < 8; count++) {
                text.Append(_numbers[_offset + count]);
            }
            return text.ToString();
        }

        private void SetOffset() {
            int power = 1;
            for (int index = 6; index >= 0; index--) {
                _offset += _numbers[index] * power;
                power *= 10;
            }
        }

        private string FirstDigits(int digitCount) {
            var text = new StringBuilder();
            for (int count = 1; count <= digitCount; count++) {
                text.Append(_numbers[count - 1]);
            }
            return text.ToString();
        }

        private int _finishCount = 0;
        private void PerformPhasesWithRepeatThreaded(int maxPhase) {
            var numbers = _numbers.ToArray();
            for (int phase = 1; phase <= maxPhase; phase++) {
                var tempNumbers = new int[numbers.Length];

                _finishCount = 0;
                var threads = new Thread[4];
                threads[0] = new Thread(() => PerformThread(1, numbers, tempNumbers));
                threads[1] = new Thread(() => PerformThread(3, numbers, tempNumbers));
                threads[2] = new Thread(() => PerformThread(5, numbers, tempNumbers));
                threads[3] = new Thread(() => PerformThread(7, numbers, tempNumbers));
                threads[0].Start();
                threads[1].Start();
                threads[2].Start();
                threads[3].Start();
                do { } while (_finishCount < 4);
                for (int index = 0; index < tempNumbers.Length; index++) {
                    tempNumbers[index] = Math.Abs(tempNumbers[index]) % 10;
                }
                numbers = tempNumbers;
            }
            _numbers = numbers.ToList();
        }

        private void PerformThread(int begin, int[] numbers, int [] tempNumbers) {
            int inversion = 1;
            for (int set = begin; set <= numbers.Length; set += 8) {
                int start = set - 1;
                int end = start;
                int lastStart = int.MinValue;
                int lastEnd = int.MinValue;
                int lastSum = 0;
                int length = 1;
                do {
                    int sum = 0;
                    if (lastEnd > start && start - lastStart + end - lastEnd < length) {
                        sum = lastSum - GetSum(lastStart, start - lastStart, numbers) + GetSum(lastEnd + 1, end - lastEnd, numbers);
                    } else {
                        sum = GetSum(start, length, numbers);
                    }
                    tempNumbers[length - 1] += sum * inversion;
                    length++;
                    lastSum = sum;
                    lastStart = start;
                    lastEnd = end;
                    start += set;
                    end = Math.Min(start + length - 1, _numbers.Count - 1);
                } while (start < _numbers.Count);
                inversion *= -1;
            }
            _finishCount++;
        }

        private void PerformPhases(int maxPhase) {
            for (int phase = 1; phase <= maxPhase; phase++) {
                var tempNumbers = new List<int>();
                for (int newNumCount = 0; newNumCount < _numbers.Count; newNumCount++) {
                    int inversion = 1;
                    int numToAdd = 0;
                    for (int numIndex = newNumCount; numIndex < _numbers.Count; numIndex += (newNumCount + 1) * 2) {
                        numToAdd += GetSum(numIndex, newNumCount + 1) * inversion;
                        inversion *= -1;
                    }
                    tempNumbers.Add(Math.Abs(numToAdd % 10));
                }
                _numbers = tempNumbers;
            }
        }

        private int GetSum(int index, int maxCount, int[] numbers) {
            int sum = 0;
            for (int count = 0; count < maxCount; count++) {
                if (index + count >= numbers.Length) {
                    return sum;
                }
                sum += numbers[index + count];
            }
            return sum;
        }

        private int GetSum(int index, int maxCount) {
            int sum = 0;
            for (int count = 0; count < maxCount; count++) {
                if (index + count >= _numbers.Count) {
                    return sum;
                }
                sum += _numbers[index + count];
            }
            return sum;
        }

        private void GetNumbers(List<string> input) {
            _numbers = new List<int>();
            for (int index = 0; index < input[0].Length; index++) {
                var digit = input[0].Substring(index, 1);
                _numbers.Add(Convert.ToInt32(digit));
            }
        }

        private void SetRepeats(int maxCount) {
            var numbers = new List<int>();
            for (int count = 1; count <= maxCount; count++) {
                numbers.AddRange(_numbers);
            }
            _numbers = numbers;
        }

        private List<string> Test1Input() {
            return new List<string>() { "12345678" };
        }

        private List<string> Test2Input() {
            return new List<string>() { "80871224585914546619083218645595" };
        }

        private List<string> Test3Input() {
            return new List<string>() { "19617804207202209144916044189917" };
        }

        private List<string> Test4Input() {
            return new List<string>() { "69317163492948606335995924319873" };
        }

        private List<string> Test5Input() {
            return new List<string>() { "03036732577212944063491565474664" };
        }
    }
}
