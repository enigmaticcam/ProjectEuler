using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class IntComputer {
        public IntComputer() {
            Output = new List<long>();
        }

        private Dictionary<long, long> _codes;
        private long _relativeBase = 0;
        private long _index;
        public bool PerformFinish { get; set; }
        public List<long> Output { get; set; }
        public long LastOutput { get; set; }
        public bool SingleOutputOnly { get; set; }
        

        public void Run(List<string> instructions, Func<long> input, Action outputCaller, IEnumerable<LineOverride> lineOverrides) {
            SetCodes(instructions);
            foreach (var over in lineOverrides) {
                _codes[over.Index] = over.Value;
            }
            Run(input, outputCaller);
        }

        public void Run(List<string> instructions, Func<long> input, Action outputCaller) {
            SetCodes(instructions);
            Run(input, outputCaller);
        }

        public void Continue(Func<long> input, Action outputCaller) {
            PerformFinish = false;
            Run(input, outputCaller);
        }

        private void Run(Func<long> input, Action outputCaller) {
            bool finish = false;
            int count = 0;
            do {
                var next = GetRawValue(_codes, _index);
                var op = next % 100;
                switch ((int)op) {
                    case 1:
                        _index = Op1(_codes, _index);
                        break;
                    case 2:
                        _index = Op2(_codes, _index);
                        break;
                    case 3:
                        _index = Op3(_codes, _index, input());
                        if (PerformFinish) {
                            finish = true;
                        }
                        break;
                    case 4:
                        _index = Op4(_codes, _index);
                        outputCaller();
                        if (PerformFinish) {
                            finish = true;
                        }
                        break;
                    case 5:
                        _index = Op5(_codes, _index);
                        break;
                    case 6:
                        _index = Op6(_codes, _index);
                        break;
                    case 7:
                        _index = Op7(_codes, _index);
                        break;
                    case 8:
                        _index = Op8(_codes, _index);
                        break;
                    case 9:
                        _index = Op9(_codes, _index);
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

        private void SetCodes(List<string> instructions) {
            var input = instructions.First().Split(',');
            var codes = new Dictionary<long, long>();
            for (int index = 0; index < input.Length; index++) {
                codes.Add((long)index, (long)Convert.ToInt64(input[index]));
            }
            _codes = codes;
        }

        public class LineOverride {
            public int Index { get; set; }
            public long Value { get; set; }
        }
    }
}
