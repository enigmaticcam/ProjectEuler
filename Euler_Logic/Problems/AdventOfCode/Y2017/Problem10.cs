using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem10 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 10"; }
        }

        public override string GetAnswer() {
            return Answer1(Input());
        }

        public override string GetAnswer2() {
            return Answer2(Input());
        }
        
        private string Answer1(List<string> input2) {
            int totalLength = 256;
            int[] numbers = Initialize(totalLength);
            int position = 0;
            int skipSize = 0;
            var input = new List<string>(input2[0].Split(','));
            foreach (int length in input.Select(x => Convert.ToInt32(x))) {
                for (int index = 0; index < length / 2; index++) {
                    int swapAPosition = (position + index) % totalLength;
                    int swapBPosition = (position + length - index - 1) % totalLength;
                    int temp = numbers[swapAPosition];
                    numbers[swapAPosition] = numbers[swapBPosition];
                    numbers[swapBPosition] = temp;
                }
                position = (position + length + skipSize) % totalLength;
                skipSize++;
            }
            return (numbers[0] * numbers[1]).ToString();
        }

        private string Answer2(List<string> input2) {
            int totalLength = 256;
            int position = 0;
            int skipSize = 0;
            string input = string.Join(",", input2);
            int[] numbers = Initialize(totalLength);

            // Get lengths
            List<int> lengths = new List<int>();
            for (int character = 0; character < input.Length; character++) {
                lengths.Add(Encoding.ASCII.GetBytes(input.Substring(character, 1)).First());
            }
            lengths.AddRange(new List<int>() { 17, 31, 73, 47, 23 });

            // Perform rounds
            for (int round = 1; round <= 64; round++) {
                foreach (int length in lengths) {
                    for (int index = 0; index < length / 2; index++) {
                        int swapAPosition = (position + index) % totalLength;
                        int swapBPosition = (position + length - index - 1) % totalLength;
                        int temp = numbers[swapAPosition];
                        numbers[swapAPosition] = numbers[swapBPosition];
                        numbers[swapBPosition] = temp;
                    }
                    position = (position + length + skipSize) % totalLength;
                    skipSize++;
                }
            }

            // Make dense hash
            List<int> dense = new List<int>();
            for (int denseIndex = 0; denseIndex < 16; denseIndex++) {
                int number = 0;
                for (int count = 0; count < 16; count++) {
                    number ^= numbers[(denseIndex * 16) + count];
                }
                dense.Add(number);
            }

            // Make hexadecimal
            StringBuilder final = new StringBuilder();
            foreach (int hash in dense) {
                string next = hash.ToString("X");
                if (next.Length == 1) {
                    final.Append("0" + next);
                } else {
                    final.Append(next);
                }
            }

            return final.ToString();
        }

        private int[] Initialize(int length) {
            int[] numbers = new int[length];
            for (int number = 0; number < length; number++) {
                numbers[number] = number;
            }
            return numbers;
        }
    }
}
