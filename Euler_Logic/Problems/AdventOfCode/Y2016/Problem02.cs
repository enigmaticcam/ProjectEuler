using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem02 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2016: 2"; }
        }

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        public string Answer1() {
            return PerformMoves(Input(), GetKeypad1(), 1, 1);
        }

        public string Answer2() {
            return PerformMoves(Input(), GetKeypad2(), 0, 2);
        }

        private string PerformMoves(List<string> instructions, char[,] keypad, int x, int y) {
            var code = new char[instructions.Count];
            int index = 0;
            foreach (var set in instructions) {
                foreach (var d in set) {
                    if (d == 'U' && y > 0 && keypad[x, y - 1] != '-') {
                        y--;
                    } else if (d == 'D' && y < keypad.GetUpperBound(1) && keypad[x, y + 1] != '-') {
                        y++;
                    } else if (d == 'L' && x > 0 && keypad[x - 1, y] != '-') {
                        x--;
                    } else if (d == 'R' && x < keypad.GetUpperBound(0) && keypad[x + 1, y] != '-') {
                        x++;
                    }
                }
                code[index] = keypad[x, y];
                index++;
            }
            return new string(code);
        }

        private char[,] GetKeypad1() {
            var keypad = new char[3, 3];
            keypad[0, 0] = '1';
            keypad[1, 0] = '2';
            keypad[2, 0] = '3';
            keypad[0, 1] = '4';
            keypad[1, 1] = '5';
            keypad[2, 1] = '6';
            keypad[0, 2] = '7';
            keypad[1, 2] = '8';
            keypad[2, 2] = '9';
            return keypad;
        }

        private char[,] GetKeypad2() {
            var keypad = new char[5, 5];
            keypad[0, 0] = '-';
            keypad[1, 0] = '-';
            keypad[2, 0] = '1';
            keypad[3, 0] = '-';
            keypad[4, 0] = '-';
            keypad[0, 1] = '-';
            keypad[1, 1] = '2';
            keypad[2, 1] = '3';
            keypad[3, 1] = '4';
            keypad[4, 1] = '-';
            keypad[0, 2] = '5';
            keypad[1, 2] = '6';
            keypad[2, 2] = '7';
            keypad[3, 2] = '8';
            keypad[4, 2] = '9';
            keypad[0, 3] = '-';
            keypad[1, 3] = 'A';
            keypad[2, 3] = 'B';
            keypad[3, 3] = 'C';
            keypad[4, 3] = '-';
            keypad[0, 4] = '-';
            keypad[1, 4] = '-';
            keypad[2, 4] = 'D';
            keypad[3, 4] = '-';
            keypad[4, 4] = '-';
            return keypad;
        }

        private List<string> TestInput() {
            return new List<string>() {
                "ULL",
                "RRDDD",
                "LURDL",
                "UUUUD"
            };
        }
    }
}
