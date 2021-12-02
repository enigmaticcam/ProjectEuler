using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class IntComputer {
        public IntComputer() {
            Output = new List<long>();
        }

        public bool PerformFinish { get; set; }
        public List<long> Output { get; set; }
        public long LastOutput { get; set; }
        public bool SingleOutputOnly { get; set; }
        private long _relativeBase = 0;

        public void Run(List<string> instructions, Func<int> input, Action outputCaller, IEnumerable<LineOverride> lineOverrides) {
            var codes = GetCodes(instructions);
            foreach (var over in lineOverrides) {
                codes[over.Index] = over.Value;
            }
            Run(codes, input, outputCaller);
        }

        public void Run(List<string> instructions, Func<int> input, Action outputCaller) {
            var codes = GetCodes(instructions);
            Run(codes, input, outputCaller);
        }

        private void Run(Dictionary<long, long> codes, Func<int> input, Action outputCaller) {
            long index = 0;
            bool finish = false;
            int count = 0;
            do {
                var next = GetRawValue(codes, index);
                var op = next % 100;
                switch ((int)op) {
                    case 1:
                        index = Op1(codes, index);
                        break;
                    case 2:
                        index = Op2(codes, index);
                        break;
                    case 3:
                        index = Op3(codes, index, input());
                        if (PerformFinish) {
                            finish = true;
                        }
                        break;
                    case 4:
                        index = Op4(codes, index);
                        outputCaller();
                        if (PerformFinish) {
                            finish = true;
                        }
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
                    case 9:
                        index = Op9(codes, index);
                        break;
                    case 99:
                        finish = true;
                        break;
                    default:
                        throw new Exception();
                }
                count++;
            } while (!finish);
        }

        private long Op1(Dictionary<long, long> codes, long index) {
            long op = codes[index];
            op /= 100;
            long num = GetValue(codes, op % 10, GetRawValue(codes, index + 1));
            op /= 10;
            num += GetValue(codes, op % 10, GetRawValue(codes, index + 2));
            op /= 10;
            SetValue(codes, op, codes[index + 3], num);
            return index + 4;
        }

        private long Op2(Dictionary<long, long> codes, long index) {
            long op = codes[index];
            op /= 100;
            long num = GetValue(codes, op % 10, GetRawValue(codes, index + 1));
            op /= 10;
            num *= GetValue(codes, op % 10, GetRawValue(codes, index + 2));
            op /= 10;
            SetValue(codes, op, codes[index + 3], num);
            return index + 4;
        }

        private long Op3(Dictionary<long, long> codes, long index, long input) {
            long op = codes[index];
            op /= 100;
            SetValue(codes, op % 10, codes[index + 1], input);
            return index + 2;
        }

        private long Op4(Dictionary<long, long> codes, long index) {
            long op = codes[index];
            op /= 100;
            LastOutput = GetValue(codes, op % 10, GetRawValue(codes, index + 1));
            if (!SingleOutputOnly) {
                Output.Add(LastOutput);
            }
            return index + 2;
        }

        private long Op5(Dictionary<long, long> codes, long index) {
            long op = codes[index];
            op /= 100;
            long num = GetValue(codes, op % 10, GetRawValue(codes, index + 1));
            if (num != 0) {
                op /= 10;
                return GetValue(codes, op % 10, GetRawValue(codes, index + 2));
            } else {
                return index + 3;
            }
        }

        private long Op6(Dictionary<long, long> codes, long index) {
            long op = codes[index];
            op /= 100;
            long num = GetValue(codes, op % 10, GetRawValue(codes, index + 1));
            if (num == 0) {
                op /= 10;
                return GetValue(codes, op % 10, GetRawValue(codes, index + 2));
            } else {
                return index + 3;
            }
        }

        private long Op7(Dictionary<long, long> codes, long index) {
            long op = codes[index];
            op /= 100;
            long num1 = GetValue(codes, op % 10, GetRawValue(codes, index + 1));
            op /= 10;
            long num2 = GetValue(codes, op % 10, GetRawValue(codes, index + 2));
            op /= 10;
            SetValue(codes, op % 10, codes[index + 3], (num1 < num2 ? (long)1 : 0));
            return index + 4;
        }

        private long Op8(Dictionary<long, long> codes, long index) {
            long op = codes[index];
            op /= 100;
            long num1 = GetValue(codes, op % 10, GetRawValue(codes, index + 1));
            op /= 10;
            long num2 = GetValue(codes, op % 10, GetRawValue(codes, index + 2));
            op /= 10;
            SetValue(codes, op % 10, codes[index + 3], (num1 == num2 ? (long)1 : 0));
            return index + 4;
        }

        private long Op9(Dictionary<long, long> codes, long index) {
            long op = codes[index];
            op /= 100;
            long num = GetValue(codes, op % 10, GetRawValue(codes, index + 1));
            _relativeBase += num;
            return index + 2;
        }

        private long GetValue(Dictionary<long, long> codes, long mode, long register) {
            if (mode == 0) {
                return GetRawValue(codes, register);
            } else if (mode == 1) {
                return register;
            } else {
                return GetRawValue(codes, register + _relativeBase);
            }
        }

        private void SetValue(Dictionary<long, long> codes, long mode, long register, long value) {
            if (mode == 0) {
                codes[register] = value;
            } else {
                codes[register + _relativeBase] = value;
            }
        }

        private long GetRawValue(Dictionary<long, long> codes, long index) {
            if (!codes.ContainsKey(index)) {
                codes.Add(index, 0);
            }
            return codes[index];
        }

        private Dictionary<long, long> GetCodes(List<string> instructions) {
            var input = instructions.First().Split(',');
            var codes = new Dictionary<long, long>();
            for (int index = 0; index < input.Length; index++) {
                codes.Add((long)index, (long)Convert.ToInt64(input[index]));
            }
            return codes;
        }

        public class LineOverride {
            public int Index { get; set; }
            public long Value { get; set; }
        }
    }
}
