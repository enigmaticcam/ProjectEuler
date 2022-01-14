using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem07 : AdventOfCodeBase {
        private Dictionary<string, Wire> _wires;

        public enum enumGateType {
            Value,
            And,
            Lshift,
            Not,
            Or,
            Rshift
        }

        public override string ProblemName => "Advent of Code 2015: 7";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetWires(input);
            return _wires["a"].Value;
        }

        private int Answer2(List<string> input) {
            GetWires(input);
            _wires["b"].GetValue = () => 46065;
            return _wires["a"].Value;
        }

        private void GetWires(List<string> input) {
            _wires = new Dictionary<string, Wire>();
            foreach (var line in input) {
                var split = line.Split(' ');
                Wire wire = null;
                if (split.Length == 3) {
                    wire = GetWireValue(split);
                } else if (split.Length == 4) {
                    wire = GetWireNot(split);
                } else if (split[1] == "AND") {
                    wire = GetWireAnd(split);
                } else if (split[1] == "OR") {
                    wire = GetWireOr(split);
                } else if (split[1] == "LSHIFT") {
                    wire = GetWireLShift(split);
                } else if (split[1] == "RSHIFT") {
                    wire = GetWireRShift(split);
                } else {
                    throw new Exception();
                }
                _wires.Add(wire.Name, wire);
            }
        }

        private Wire GetWireOr(string[] split) {
            return new Wire() {
                GateType = enumGateType.Or,
                GetValue = () => {
                    int value = (split[0] == "1" ? 1 : _wires[split[0]].Value);
                    return value | _wires[split[2]].Value;
                },
                Name = split[4]
            };
        }

        private Wire GetWireRShift(string[] split) {
            return new Wire() {
                GateType = enumGateType.Rshift,
                GetValue = () => _wires[split[0]].Value >> Convert.ToInt32(split[2]),
                Name = split[4]
            };
        }

        private Wire GetWireLShift(string[] split) {
            return new Wire() {
                GateType = enumGateType.Lshift,
                GetValue = () => _wires[split[0]].Value << Convert.ToInt32(split[2]),
                Name = split[4]
            };
        }

        private Wire GetWireAnd(string[] split) {
            return new Wire() {
                GateType = enumGateType.And,
                GetValue = () => {
                    int value = (split[0] == "1" ? 1 : _wires[split[0]].Value);
                    return value & _wires[split[2]].Value;
                },
                Name = split[4]
            };
        }

        private Wire GetWireNot(string[] split) {
            return new Wire() {
                GateType = enumGateType.Not,
                GetValue = () => ~_wires[split[1]].Value,
                Name = split[3]
            };
        }

        private Wire GetWireValue(string[] split) {
            return new Wire() {
                GateType = enumGateType.Value,
                GetValue = () => {
                    if (_wires.ContainsKey(split[0])) {
                        return _wires[split[0]].Value;
                    } else {
                        return Convert.ToInt32(split[0]);
                    }
                },
                Name = split[2]
            };
        }

        private class Wire {
            private bool _hasValue;

            private int _value;
            public int Value {
                get {
                    if(!_hasValue) {
                        _value = GetValue();
                        _hasValue = true;
                    }
                    return _value;
                }
            }

            public string Name { get; set; }
            public enumGateType GateType { get; set; }
            public Func<int> GetValue { get; set; }
        }
    }
}
