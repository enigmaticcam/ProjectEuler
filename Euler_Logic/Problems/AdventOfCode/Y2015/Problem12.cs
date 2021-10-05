using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem12 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2015: 12";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            return FindNumbers(input[0]);
        }

        private int Answer2(List<string> input) {
            return FindNumbersIgnoreRed(input[0]);
        }

        private int FindNumbers(string text) {
            var nums = new List<int>();
            var currentNum = new List<char>();
            bool isNegative = false;
            foreach (var digit in text) {
                if (digit == '-') {
                    if (currentNum.Count == 0) {
                        isNegative = true;
                    } else if (currentNum.Count > 0) {
                        currentNum = new List<char>();
                        isNegative = false;
                    }
                } else {
                    if (Char.IsNumber(digit)) {
                        currentNum.Add(digit);
                    } else {
                        if (currentNum.Count > 0) {
                            var result = new string(currentNum.ToArray());
                            var num = Convert.ToInt32(result);
                            if (isNegative) {
                                num *= -1;
                                isNegative = false;
                            }
                            nums.Add(num);
                            currentNum = new List<char>();
                        }
                    }
                }
            }
            return nums.Sum();
        }

        private int FindNumbersIgnoreRed(string text) {
            var currentNum = new List<char>();
            bool isNegative = false;
            var bb = new BracketBrace() { Children = new List<BracketBrace>(), Numbers = new List<int>() };
            int index = 0;
            foreach (var digit in text) {
                if (digit == '-') {
                    if (currentNum.Count == 0) {
                        isNegative = true;
                    } else if (currentNum.Count > 0) {
                        currentNum = new List<char>();
                        isNegative = false;
                    }
                } else {
                    if (Char.IsNumber(digit)) {
                        currentNum.Add(digit);
                    } else {
                        if (currentNum.Count > 0) {
                            var result = new string(currentNum.ToArray());
                            var num = Convert.ToInt32(result);
                            if (isNegative) {
                                num *= -1;
                                isNegative = false;
                            }
                            bb.Numbers.Add(num);
                            currentNum = new List<char>();
                        }
                    }
                }
                if (digit == '[') {
                    var newBB = new BracketBrace() {
                        Digit = digit,
                        Numbers = new List<int>(),
                        Children = new List<BracketBrace>(),
                        Parent = bb
                    };
                    bb.Children.Add(newBB);
                    bb = newBB;
                } else if (digit == ']') {
                    bb = bb.Parent;
                } else if (digit == '{') {
                    var newBB = new BracketBrace() {
                        Digit = digit,
                        Numbers = new List<int>(),
                        Children = new List<BracketBrace>(),
                        Parent = bb
                    };
                    bb.Children.Add(newBB);
                    bb = newBB;
                } else if (digit == '}') {
                    bb = bb.Parent;
                } else if (index > 1 && text[index] == 'd' && text[index - 1] == 'e' && text[index - 2] == 'r') {
                    bb.IsRed = true;
                }
                index++;
            }
            return GetSum(bb);
        }

        private int GetSum(BracketBrace bb) {
            int sum = 0;
            foreach (var child in bb.Children) {
                if (!child.IsRed || child.Digit == '[') {
                    sum += child.Numbers.Sum();
                    sum += GetSum(child);
                }
            }
            return sum;
        }

        private class BracketBrace {
            public char Digit { get; set; }
            public bool IsRed { get; set; }
            public List<int> Numbers { get; set; }
            public List<BracketBrace> Children { get; set; }
            public BracketBrace Parent { get; set; }
        }
    }
}
