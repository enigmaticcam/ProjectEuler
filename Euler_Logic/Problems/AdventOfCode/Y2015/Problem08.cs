using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem08 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2015: 8";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var result = input.Select(line => line.Length - Interpret1(line)).ToList();
            return result.Sum();
        }

        private int Answer2(List<string> input) {
            var result = input.Select(line => Interpret2(line) - line.Length).ToList();
            return result.Sum();
        }

        private int Interpret1(string text) {
            int length = text.Length - 2;
            for (int index = 1; index < text.Length - 1; index++) {
                if (text[index] == '\\') {
                    if (text[index + 1] == '\\') {
                        length--;
                        index++;
                    } else if (text[index + 1] == '"') {
                        length--;
                        index++;
                    } else if (text[index + 1] == 'x') {
                        length -= 3;
                        index += 3;
                    }
                }
            }
            return length;
        }

        private int Interpret2(string text) {
            int length = text.Length + 2;
            for (int index = 0; index < text.Length; index++) {
                if (text[index] == '"') {
                    length++;
                } else if (text[index] == '\\') {
                    length++;
                }
            }
            return length;
        }
    }
}
