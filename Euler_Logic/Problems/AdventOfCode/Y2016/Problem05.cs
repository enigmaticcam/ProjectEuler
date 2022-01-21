using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem05 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2016: 5";

        public override string GetAnswer() {
            return Answer1("abbhdwsy").ToString();
        }

        public override string GetAnswer2() {
            return Answer2("abbhdwsy").ToString();
        }

        private string Answer1(string input) {
            return FindNext1(input);
        }

        private string Answer2(string input) {
            return FindNext2(input);
        }

        private string FindNext1(string input) {
            int index = 0;
            var password = new char[8];
            var charIndex = 0;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                do {
                    var next = input + index.ToString();
                    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(next);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);
                    if (hashBytes[0].ToString("X2") == "00" && hashBytes[1].ToString("X2") == "00") {
                        var result = hashBytes[2].ToString("X2");
                        if (result[0] == '0') {
                            password[charIndex] = result[1];
                            charIndex++;
                        }
                    }
                    index++;
                } while (charIndex < 8);
            }
            return new string(password);
        }

        private string FindNext2(string input) {
            int index = 0;
            var password = new char[8];
            var filled = new bool[8];
            var charCount = 0;
            var valids = GetValids();
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                do {
                    var next = input + index.ToString();
                    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(next);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);
                    if (hashBytes[0].ToString("X2") == "00" && hashBytes[1].ToString("X2") == "00") {
                        var result = hashBytes[2].ToString("X2");
                        if (result[0] == '0' && valids.ContainsKey(result[1])) {
                            int position = valids[result[1]];
                            if (!filled[position]) {
                                password[valids[result[1]]] = hashBytes[3].ToString("X2")[0];
                                charCount++;
                                filled[position] = true;
                            }
                        }
                    }
                    index++;
                } while (charCount <= 7);
            }
            return new string(password);
        }

        private Dictionary<char, int> GetValids() {
            var valids = new Dictionary<char, int>();
            valids.Add('0', 0);
            valids.Add('1', 1);
            valids.Add('2', 2);
            valids.Add('3', 3);
            valids.Add('4', 4);
            valids.Add('5', 5);
            valids.Add('6', 6);
            valids.Add('7', 7);
            return valids;
        }
    }
}
