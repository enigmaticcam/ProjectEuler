using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem02 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 2"; }
        }

        public override string GetAnswer() {
            return Answer1(Input());
        }

        public override string GetAnswer2() {
            return Answer2(Input());
        }

        private string Answer1(List<string> input) {
            int sum = 0;
            foreach (string text in input) {
                var split = new List<string>(text.Split('\t'));
                int min = int.MaxValue;
                int max = int.MinValue;
                foreach (string num in split) {
                    int number = Convert.ToInt32(num.Trim());
                    if (number > max) {
                        max = number;
                    }
                    if (number < min) {
                        min = number;
                    }
                }
                sum += max - min;
            }
            return sum.ToString();
        }

        private string Answer2(List<string> input) {
            int sum = 0;
            foreach (string text in input) {
                var split = new List<string>(text.Split('\t'));
                for (int x = 0; x < split.Count - 1; x++) {
                    for (int y = x + 1; y < split.Count; y++) {
                        int a = Convert.ToInt32(split[x]);
                        int b = Convert.ToInt32(split[y]);
                        int min = Math.Min(a, b);
                        int max = Math.Max(a, b);
                        if (max % min == 0) {
                            sum += max / min;
                            break;
                        }
                    }
                }
            }
            return sum.ToString();
        }

        //private List<string> Split(string text) {
        //    int index = 0;
        //    List<string> result = new List<string>();
        //    do {
        //        if (text.Substring(index, 1) != " ") {
        //            int nextSpace = text.IndexOf(' ', index);
        //            if (nextSpace == -1) {
        //                nextSpace = text.Length;
        //            }
        //            result.Add(text.Substring(index, nextSpace - index));
        //            index = nextSpace;
        //        }
        //        index++;
        //    } while (index <= text.Length);
        //    return result;
        //}
    }
}
