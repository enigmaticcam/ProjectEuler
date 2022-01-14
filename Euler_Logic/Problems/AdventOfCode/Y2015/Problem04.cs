using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem04 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2015: 4"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            return FindMD5(input[0], 5);
        }

        private int Answer2(List<string> input) {
            return FindMD5(input[0], 6);
        }

        private int FindMD5(string input, int zeroLength) {
            int count = 1;
            do {
                var text = input + count;
                if (CreateMD5(text, zeroLength)) return count;
                count++;
            } while (true);
        }

        public bool CreateMD5(string input, int zeroLength) {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                int inputIndex = 0;
                int remainingZero = zeroLength;
                do {
                    var result = hashBytes[inputIndex].ToString("X2");
                    if (result[0] != '0') return false;
                    if (remainingZero > 1 && result[1] != '0') return false;
                    inputIndex++;
                    remainingZero -= 2;
                } while (remainingZero > 0);
                return true;
            }
        }
    }
}
