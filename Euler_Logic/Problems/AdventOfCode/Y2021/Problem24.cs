using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem24 : AdventOfCodeBase {
        private Variables _var = new Variables();
        private long[] _input;
        private long[] _y;
        private List<Tuple<int, int>> _set;

        public override string ProblemName {
            get { return "Advent of Code 2021: 24"; }
        }

        public override string GetAnswer() {
            return Answer1().ToString();
        }

        public override string GetAnswer2() {
            return Answer2().ToString();
        }

        private string Answer1() {
            _input = new long[14];
            _y = new long[14];
            SetSets();
            Recurisve(0, 4, 0, true);
            return ConvertToString();
        }

        private string Answer2() {
            _input = new long[14];
            _y = new long[14];
            SetSets();
            Recurisve(0, 4, 0, false);
            return ConvertToString();
        }

        private bool Recurisve(int index, int last, int setIndex, bool findHighest) {
            if (findHighest) {
                for (long num = 9; num >= 1; num--) {
                    _input[index] = num;
                    var result = RecurisveTest(index, last, setIndex, findHighest);
                    if (result) return true;
                }
            } else {
                for (long num = 1; num <= 9; num++) {
                    _input[index] = num;
                    var result = RecurisveTest(index, last, setIndex, findHighest);
                    if (result) return true;
                }
            }
            return false;
        }

        private bool RecurisveTest(int index, int last, int setIndex, bool findHighest) {
            if (index == last) {
                _var.Reset();
                RunManual();
                if (_y[last] == 0) {
                    if (setIndex == _set.Count - 1) {
                        return true;
                    } else {
                        var nextSet = _set[setIndex + 1];
                        var result = Recurisve(nextSet.Item1, nextSet.Item2, setIndex + 1, findHighest);
                        if (result) return true;
                    }
                }
            } else {
                var result = Recurisve(index + 1, last, setIndex, findHighest);
                if (result) return true;
            }
            return false;
        }

        private void SetSets() {
            _set = new List<Tuple<int, int>>();
            _set.Add(new Tuple<int, int>(0, 4));
            _set.Add(new Tuple<int, int>(5, 7));
            _set.Add(new Tuple<int, int>(8, 9));
            _set.Add(new Tuple<int, int>(10, 10));
            _set.Add(new Tuple<int, int>(11, 11));
            _set.Add(new Tuple<int, int>(12, 12));
            _set.Add(new Tuple<int, int>(13, 13));
        }

        private string ConvertToString() {
            return string.Join("", _input);
        }

        private void RunManual() {
            Manual(_input[0], 1, 12, 6, 0);
            Manual(_input[1], 1, 11, 12, 1);
            Manual(_input[2], 1, 10, 5, 2);
            Manual(_input[3], 1, 10, 10, 3);
            Manual(_input[4], 26, -16, 7, 4);
            Manual(_input[5], 1, 14, 0, 5);
            Manual(_input[6], 1, 12, 4, 6);
            Manual(_input[7], 26, -4, 12, 7);
            Manual(_input[8], 1, 15, 14, 8);
            Manual(_input[9], 26, -7, 13, 9);
            Manual(_input[10], 26, -8, 10, 10);
            Manual(_input[11], 26, -4, 11, 11);
            Manual(_input[12], 26, -15, 9, 12);
            Manual(_input[13], 26, -8, 9, 13);
        }

        private void Manual(long w, long a, long b, long c, int yIndex) {
            if ((_var.Z % 26) + b != w) {
                _var.X = 1;
                _var.Y = 26;
            } else {
                _var.X = 0;
                _var.Y = 1;
            }
            _var.Z /= a;
            _var.Z *= _var.Y;
            _var.Y = (w + c) * _var.X;
            _var.Z += _var.Y;
            _y[yIndex] = _var.Y;
        }

        private class Variables {
            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }
            public void Reset() {
                X = 0;
                Y = 0;
                Z = 0;
            }
        }
    }
}
