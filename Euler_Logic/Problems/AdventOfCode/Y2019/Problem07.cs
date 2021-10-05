using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem07 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 7"; }
        }

        public override string GetAnswer() {
            _powerOf2 = new PowerAll(2);
            _outputs = new List<int>();
            return Answer2().ToString();
        }

        public int Answer1() {
            var phases = GetPhases(0, 4);
            int best = 0;
            foreach (var phase in phases) {
                int next = GetSignal(phase);
                if (next > best) {
                    best = next;
                }
            }
            return best;
        }

        public int Answer2() {
            var phases = GetPhases(5, 9);
            int best = 0;
            foreach (var phase in phases) {
                _outputs.Clear();
                var amps = GetAmps(5);
                int next = GetSignalWithAmp(phase, amps);
                if (next > best) {
                    best = next;
                }
            }
            return best;
        }

        private Amp[] GetAmps(int count) {
            var amps = new Amp[count];
            for (int index = 0; index < count; index++) {
                amps[index] = new Amp() {
                    Codes = GetCodes(),
                    Index = 0
                };
            }
            return amps;
        }

        private List<int[]> GetPhases(int min, int max) {
            var phases = new List<int[]>();
            Recursive(max, min, 0, new int[max - min + 1], phases);
            return phases;
        }

        private PowerAll _powerOf2;
        private void Recursive(int num, int stopNum, ulong bits, int[] phase, List<int[]> phases) {
            for (int index = 0; index < phase.Length; index++) {
                var bit = _powerOf2.GetPower(index);
                if ((bits & bit) == 0) {
                    phase[index] = num;
                    if (num == stopNum) {
                        phases.Add(phase.ToArray());
                    } else {
                        Recursive(num - 1, stopNum, bits + bit, phase, phases);
                    }
                }
            }
        }

        private int GetSignal(int[] phase) {
            var source = GetCodes();
            int signal = 0;
            for (int index = 0; index < phase.Length; index++) {
                var codes = source.ToArray();
                signal = GetSignal(codes, phase[index], signal);
            }
            return signal;
        }

        private int GetSignalWithAmp(int[] phase, Amp[] amps) {
            int ampIndex = 0;
            int signal = 0;
            do {
                var amp = amps[ampIndex];
                bool hold = false;
                bool finish = false;
                do {
                    var next = amp.Codes[amp.Index];
                    var op = next % 100;
                    switch (op) {
                        case 1:
                            amp.Index = Op1(amp.Codes, amp.Index);
                            break;
                        case 2:
                            amp.Index = Op2(amp.Codes, amp.Index);
                            break;
                        case 3:
                            int input = signal;
                            if (!amp.PhaseEntered) {
                                input = phase[ampIndex];
                                amp.PhaseEntered = true;
                            }
                            amp.Index = Op3(amp.Codes, amp.Index, input);
                            break;
                        case 4:
                            amp.Index = Op4(amp.Codes, amp.Index);
                            signal = _outputs.Last();
                            hold = true;
                            break;
                        case 5:
                            amp.Index = Op5(amp.Codes, amp.Index);
                            break;
                        case 6:
                            amp.Index = Op6(amp.Codes, amp.Index);
                            break;
                        case 7:
                            amp.Index = Op7(amp.Codes, amp.Index);
                            break;
                        case 8:
                            amp.Index = Op8(amp.Codes, amp.Index);
                            break;
                        case 99:
                            finish = true;
                            break;
                    }
                } while (!hold && !finish);
                if (finish) {
                    return _outputs.Last();
                }
                ampIndex = (ampIndex + 1) % amps.Length;
            } while (true);
        }

        private int GetSignal(int[] codes, int input1, int input2) {
            var input = input1;
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
                        input = input2;
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

        private List<int> _outputs;
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

        private List<string> TestInput() {
            return new List<string>() {
                "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5"
            };
        }

        private class Amp {
            public int[] Codes { get; set; }
            public int Index { get; set; }
            public bool PhaseEntered { get; set; }
        }
    }
}
