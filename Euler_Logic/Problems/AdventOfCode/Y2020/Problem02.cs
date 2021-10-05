using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem02 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2020: 2";

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        private int Answer1() {
            var passwords = GetPasswords();
            int goodCount = 0;
            foreach (var password in passwords) {
                var count = 0;
                bool isGood = true;
                foreach (var digit in password.Text) {
                    if (digit == password.Digit) {
                        count++;
                        if (count > password.Max) {
                            isGood = false;
                            break;
                        }
                    }
                }
                if (isGood && count >= password.Min) {
                    goodCount++;
                }
            }
            return goodCount;
        }

        private int Answer2() {
            var passwords = GetPasswords();
            int goodCount = 0;
            foreach (var password in passwords) {
                bool isGood = false;
                if (password.Text[password.Min - 1] != password.Text[password.Max - 1]) {
                    isGood = password.Text[password.Min - 1] == password.Digit || password.Text[password.Max - 1] == password.Digit;
                }
                if (isGood) {
                    goodCount++;
                }
            }
            return goodCount;
        }

        private IEnumerable<Password> GetPasswords() {
            return Input().Select(x => {
                var password = new Password();
                int hyphen = x.IndexOf('-');
                int space = x.IndexOf(' ', hyphen);
                int colon = x.IndexOf(':', hyphen + 1);
                password.Min = Convert.ToInt32(x.Substring(0, hyphen));
                password.Max = Convert.ToInt32(x.Substring(hyphen + 1, space - hyphen));
                password.Digit = x.Substring(space + 1, colon - space)[0];
                password.Text = x.Substring(colon + 1, x.Length - colon - 1).Trim();
                return password;
            });
        }

        private List<string> TestInput() {
            return new List<string>() {
                "1-3 a: abcde",
                "1-3 b: cdefg",
                "2-9 c: ccccccccc"
            };
        }

        private class Password {
            public string Text { get; set; }
            public char Digit { get; set; }
            public int Min { get; set; }
            public int Max { get; set; }
        }
    }
}
