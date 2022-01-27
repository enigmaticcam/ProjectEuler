using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem05 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 5"; }
        }

        public override string GetAnswer() {
            _outputs = new List<int>();
            return Answer1(1).ToString();
        }

        public override string GetAnswer2() {
            _outputs = new List<int>();
            return Answer2(5).ToString();
        }

        private List<int> _outputs;
        public int Answer1(int input) {
            var codes = GetCodes();
            int index = 0;
            bool finish = false;
            do {
                var next = codes[index];
                var op = next % 100;
                switch (op) {
                    case 1:
                        index = Op1(codes, index);
                        break;
                    case 2:
                        index = Op2(codes, index);
                        break;
                    case 3:
                        index = Op3(codes, index, input);
                        break;
                    case 4:
                        index = Op4(codes, index);
                        break;
                    case 99:
                        finish = true;
                        break;
                }
            } while (!finish);
            return _outputs.Last();
        }

        public int Answer2(int input) {
            var codes = GetCodes();
            int index = 0;
            bool finish = false;
            do {
                var next = codes[index];
                var op = next % 100;
                switch (op) {
                    case 1:
                        index = Op1(codes, index);
                        break;
                    case 2:
                        index = Op2(codes, index);
                        break;
                    case 3:
                        index = Op3(codes, index, input);
                        break;
                    case 4:
                        index = Op4(codes, index);
                        break;
                    case 5:
                        index = Op5(codes, index);
                        break;
                    case 6:
                        index = Op6(codes, index);
                        break;
                    case 7:
                        index = Op7(codes, index);
                        break;
                    case 8:
                        index = Op8(codes, index);
                        break;
                    case 99:
                        finish = true;
                        break;
                }
            } while (!finish);
            return _outputs.Last();
        }

        private int Op1(int[] codes, int index) {
            int op = codes[index];
            op /= 100;
            int num = GetValue(codes, op % 10, codes[index + 1]);
            op /= 10;
            num += GetValue(codes, op % 10, codes[index + 2]);
            op /= 10;
            int offset = 2;
            while (op != 0) {
                offset++;
                num += GetValue(codes, op % 10, codes[index + offset]);
            }
            offset++;
            codes[codes[index + offset]] = num;
            return index + offset + 1;
        }

        private int Op2(int[] codes, int index) {
            int op = codes[index];
            op /= 100;
            int num = GetValue(codes, op % 10, codes[index + 1]);
            op /= 10;
            num *= GetValue(codes, op % 10, codes[index + 2]);
            op /= 10;
            int offset = 2;
            while (op != 0) {
                offset++;
                num *= GetValue(codes, op % 10, codes[index + offset]);
            }
            offset++;
            codes[codes[index + offset]] = num;
            return index + offset + 1;
        }

        private int Op3(int[] codes, int index, int input) {
            codes[codes[index + 1]] = input;
            return index + 2;
        }

        private int Op4(int[] codes, int index) {
            int op = codes[index];
            op /= 100;
            _outputs.Add(GetValue(codes, op % 10, codes[index + 1]));
            return index + 2;
        }

        private int Op5(int[] codes, int index) {
            int op = codes[index];
            op /= 100;
            int num = GetValue(codes, op % 10, codes[index + 1]);
            if (num != 0) {
                op /= 10;
                return GetValue(codes, op % 10, codes[index + 2]);
            } else {
                return index + 3;
            }
        }

        private int Op6(int[] codes, int index) {
            int op = codes[index];
            op /= 100;
            int num = GetValue(codes, op % 10, codes[index + 1]);
            if (num == 0) {
                op /= 10;
                return GetValue(codes, op % 10, codes[index + 2]);
            } else {
                return index + 3;
            }
        }

        private int Op7(int[] codes, int index) {
            int op = codes[index];
            op /= 100;
            int num1 = GetValue(codes, op % 10, codes[index + 1]);
            op /= 10;
            int num2 = GetValue(codes, op % 10, codes[index + 2]);
            codes[codes[index + 3]] = (num1 < num2 ? 1 : 0);
            return index + 4;
        }

        private int Op8(int[] codes, int index) {
            int op = codes[index];
            op /= 100;
            int num1 = GetValue(codes, op % 10, codes[index + 1]);
            op /= 10;
            int num2 = GetValue(codes, op % 10, codes[index + 2]);
            codes[codes[index + 3]] = (num1 == num2 ? 1 : 0);
            return index + 4;
        }

        private int GetValue(int[] codes, int mode, int register) {
            if (mode == 0) {
                return codes[register];
            } else {
                return register;
            }
        }

        private int[] GetCodes() {
            var input = Input().First().Split(',');
            var codes = new int[input.Length];
            for (int index = 0; index < input.Length; index++) {
                codes[index] = Convert.ToInt32(input[index]);
            }
            return codes;
        }
    }
}
