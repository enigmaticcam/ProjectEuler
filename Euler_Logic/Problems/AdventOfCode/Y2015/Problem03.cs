using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem03 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2015: 3"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            int x = 0;
            int y = 0;
            var hash = new HashSet<Tuple<int, int>>();
            foreach (var digit in input[0]) {
                switch (digit) {
                    case '>':
                        x++;
                        break;
                    case '<':
                        x--;
                        break;
                    case 'v':
                        y++;
                        break;
                    case '^':
                        y--;
                        break;
                }
                hash.Add(new Tuple<int, int>(x, y));
            }
            return hash.Count;
        }

        private int Answer2(List<string> input) {
            int[,] points = new int[2, 2];
            var hash = new HashSet<Tuple<int, int>>();
            int index = 0;
            foreach (var digit in input[0]) {
                switch (digit) {
                    case '>':
                        points[index, 0]++;
                        break;
                    case '<':
                        points[index, 0]--;
                        break;
                    case 'v':
                        points[index, 1]++;
                        break;
                    case '^':
                        points[index, 1]--;
                        break;
                }
                hash.Add(new Tuple<int, int>(points[index, 0], points[index, 1]));
                index = (index + 1) % 2;
            }
            return hash.Count;
        }
    }
}
