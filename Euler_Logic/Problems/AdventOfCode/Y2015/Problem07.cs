using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem07 : AdventOfCodeBase {
        private Dictionary<string, Wire> _wires;
        private uint _16Bit = 65535;

        private enum enumWireType {
            Repeater,
            Signal,
            AND,
            LSHIFT,
            NOT,
            OR,
            RSHIFT
        }

        public override string ProblemName => "Advent of Code 2015: 7";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private uint Answer1(List<string> input) {
            var wires = GetWires(input);
            _wires = wires.ToDictionary(wire => wire.Id, wire => wire);
            LoopAllWires();
            return _wires["a"].SignalValue;
        }

        private uint Answer2(List<string> input) {
            var wires = GetWires(input);
            _wires = wires.ToDictionary(wire => wire.Id, wire => wire);
            _wires["b"].Value = 46065;
            LoopAllWires();
            return _wires["a"].SignalValue;
        }

        private void LoopAllWires() {
            var isFinished = false;
            do {
                isFinished = true;
                foreach (var wire in _wires.Values) {
                    if (!wire.HasSignal) {
                        RunSignal(wire);
                    }
                    if (!wire.HasSignal) {
                        isFinished = false;
                    }
                }
            } while (!isFinished);
        }

        private void RunSignal(Wire wire) {
            switch (wire.WireType) {
                case enumWireType.AND:
                    RunSignalAND(wire);
                    break;
                case enumWireType.LSHIFT:
                    RunSignalLSHIFT(wire);
                    break;
                case enumWireType.NOT:
                    RunSignalNOT(wire);
                    break;
                case enumWireType.OR:
                    RunSignalOR(wire);
                    break;
                case enumWireType.Repeater:
                    RunSignalRepeater(wire);
                    break;
                case enumWireType.RSHIFT:
                    RunSignalRSHIFT(wire);
                    break;
                case enumWireType.Signal:
                    RunSignalStarter(wire);
                    break;
            }
        }

        private void RunSignalStarter(Wire wire) {
            wire.SignalValue = (uint)wire.Value;
            wire.HasSignal = true;
        }

        private void RunSignalAND(Wire wire) {
            bool isGood = true;
            uint input1 = 0;
            uint input2 = 0;
            if (wire.IsInputWire1) {
                var input = _wires[wire.Input1];
                if (!input.HasSignal) {
                    isGood = false;
                }
                input1 = input.SignalValue;
            } else {
                input1 = Convert.ToUInt32(wire.Input1);
            }
            if (wire.IsInputWire2) {
                var input = _wires[wire.Input2];
                if (!input.HasSignal) {
                    isGood = false;
                }
                input2 = input.SignalValue;
            } else {
                input2 = Convert.ToUInt32(wire.Input2);
            }
            if (isGood) {
                wire.SignalValue = (input1 & input2);
                wire.HasSignal = true;
            }
        }

        private void RunSignalLSHIFT(Wire wire) {
            var input = _wires[wire.Input1];
            if (input.HasSignal) {
                wire.SignalValue = (input.SignalValue << wire.Value) & _16Bit;
                wire.HasSignal = true;
            }
        }

        private void RunSignalRSHIFT(Wire wire) {
            var input = _wires[wire.Input1];
            if (input.HasSignal) {
                wire.SignalValue = (input.SignalValue >> wire.Value) & _16Bit;
                wire.HasSignal = true;
            }
        }

        private void RunSignalNOT(Wire wire) {
            var input = _wires[wire.Input1];
            if (input.HasSignal) {
                wire.SignalValue = (~input.SignalValue) & _16Bit;
                wire.HasSignal = true;
            }
        }

        private void RunSignalOR(Wire wire) {
            var input1 = _wires[wire.Input1];
            var input2 = _wires[wire.Input2];
            if (input1.HasSignal && input2.HasSignal) {
                wire.SignalValue = (input1.SignalValue | input2.SignalValue) & _16Bit;
                wire.HasSignal = true;
            }
        }

        private void RunSignalRepeater(Wire wire) {
            var input = _wires[wire.Input1];
            if (input.HasSignal) {
                wire.SignalValue = input.SignalValue;
                wire.HasSignal = true;
            }
        }

        private List<Wire> GetWires(List<string> input) {
            return input.Select(line => {
                var wire = new Wire();
                var split = line.Split(' ');
                if (split.Length == 3) {
                    int num = 0;
                    var isNum = int.TryParse(split[0], out num);
                    if (isNum) {
                        wire.WireType = enumWireType.Signal;
                        wire.Value = num;
                    } else {
                        wire.WireType = enumWireType.Repeater;
                        wire.Input1 = split[0];
                    }
                    wire.Id = split[2];
                } else if (split[0] == "NOT") {
                    wire.WireType = enumWireType.NOT;
                    wire.Input1 = split[1];
                    wire.Id = split[3];
                } else {
                    switch (split[1]) {
                        case "AND":
                            wire.WireType = enumWireType.AND;
                            break;
                        case "OR":
                            wire.WireType = enumWireType.OR;
                            break;
                        case "LSHIFT":
                            wire.WireType = enumWireType.LSHIFT;
                            wire.Value = Convert.ToInt32(split[2]);
                            break;
                        case "RSHIFT":
                            wire.WireType = enumWireType.RSHIFT;
                            wire.Value = Convert.ToInt32(split[2]);
                            break;
                    }
                    wire.Id = split[4];
                    wire.Input1 = split[0];
                    wire.Input2 = split[2];
                    int num = 0;
                    wire.IsInputWire1 = !int.TryParse(split[0], out num);
                    wire.IsInputWire2 = !int.TryParse(split[2], out num);
                }
                return wire;
            }).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "123 -> x",
                "456 -> y",
                "x AND y -> d",
                "x OR y -> e",
                "x LSHIFT 2 -> f",
                "y RSHIFT 2 -> g",
                "NOT x -> h",
                "NOT y -> i"
            };
        }

        private class Wire {
            public string Id { get; set; }
            public enumWireType WireType { get; set; }
            public int Value { get; set; }
            public string Input1 { get; set; }
            public string Input2 { get; set; }
            public bool IsInputWire1 { get; set; }
            public bool IsInputWire2 { get; set; }
            public uint SignalValue { get; set; }
            public bool HasSignal { get; set; }
        }
    }
}
