using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem16 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2016: 16";

        public override string GetAnswer() {
            return Answer1("10001110011110000", 35651584).ToString();
        }

        private string Answer1(string input, int length) {
            var data = FillData(input, length);
            return GetChecksum(data.Substring(0, length));
        }

        private string GetChecksum(string data) {
            var checksum = data.ToCharArray();
            do {
                var nextChecksum = new char[checksum.Length / 2];
                for (int index = 0; index < checksum.Length - 1; index += 2) {
                    if (checksum[index] == checksum[index + 1]) {
                        nextChecksum[index / 2] = '1';
                    } else {
                        nextChecksum[index / 2] = '0';
                    }
                }
                checksum = nextChecksum;
            } while (checksum.Length % 2 == 0);
            return new string(checksum);
        }

        private string FillData(string start, int length) {
            var swap = new Dictionary<char, char>();
            swap.Add('0', '1');
            swap.Add('1', '0');
            var result = start.ToCharArray();
            while (result.Length < length) {
                var nextResult = new char[result.Length * 2 + 1];
                int index = 0;
                foreach (var digit in result) {
                    nextResult[index] = digit;
                    nextResult[nextResult.Length - index - 1] = swap[digit];
                    index++;
                }
                nextResult[index] = '0';
                result = nextResult;
            }
            return new string(result);
        }
    }
}
