using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem23 : AdventOfCodeBase {
        private Dictionary<int, Computer> _computers;
        private bool _sentTo255;
        private Packet _valueSentTo255;
        private HashSet<long> _prior255s;

        public override string ProblemName {
            get { return "Advent of Code 2019: 23"; }
        }

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        private long Answer1() {
            BootComputers(50);
            LookForFirstTo255(Input());
            return _valueSentTo255.Y;
        }

        private long Answer2() {
            _prior255s = new HashSet<long>();
            BootComputers(50);
            LookForSecondTo255(Input());
            return _valueSentTo255.Y;
        }

        private void BootComputers(int count) {
            _computers = new Dictionary<int, Computer>();
            for (int address = 0; address < count; address++) {
                _computers.Add(address, new Computer() {
                    Address = address,
                    Comp = new IntComputer(),
                    IsBooting = true,
                    Packets = new LinkedList<Packet>()
                });
                _computers[address].Comp.SingleOutputOnly = true;
            }
        }

        private void LookForFirstTo255(List<string> input) {
            foreach (var comp in _computers.Values) {
                comp.IsFinishedInput = false;
                comp.Comp.Run(input, () => InputCaller(comp), () => OutputCaller(comp));
            }
            do {
                foreach (var comp in _computers.Values) {
                    comp.IsFinishedInput = false;
                    comp.Comp.Continue(() => InputCaller(comp), () => OutputCaller(comp));
                }
            } while (!_sentTo255);
        }

        private void LookForSecondTo255(List<string> input) {
            foreach (var comp in _computers.Values) {
                comp.IsFinishedInput = false;
                comp.Comp.Run(input, () => InputCaller(comp), () => OutputCaller(comp));
            }
            do {
                bool hasPackets = false;
                foreach (var comp in _computers.Values) {
                    hasPackets |= comp.Packets.Count > 0;
                    comp.IsFinishedInput = false;
                    comp.Comp.Continue(() => InputCaller(comp), () => OutputCaller(comp));
                }
                if (!hasPackets) {
                    if (_prior255s.Contains(_valueSentTo255.Y)) {
                        break;
                    }
                    _prior255s.Add(_valueSentTo255.Y);
                    _computers[0].Packets.AddLast(_valueSentTo255);
                }
            } while (true);
        }

        private long InputCaller(Computer computer) {
            if (computer.IsBooting) {
                computer.IsBooting = false;
                return computer.Address;
            } else if (computer.Packets.Count == 0) {
                if (!computer.IsFinishedInput) {
                    computer.IsFinishedInput = true;
                } else {
                    computer.Comp.PerformFinish = true;
                }
                return -1;
            } else {
                var packet = computer.Packets.First.Value;
                if (!packet.IsXRead) {
                    packet.IsXRead = true;
                    return packet.X;
                } else {
                    computer.Packets.RemoveFirst();
                    return packet.Y;
                }
            }
        }

        private void OutputCaller(Computer computer) {
            if (computer.PacketPreparingToSend == null) {
                computer.PacketPreparingToSend = new Packet() { Address = (int)computer.Comp.LastOutput };
                computer.IsNextOtuputX = true;
            } else if (computer.IsNextOtuputX) {
                computer.PacketPreparingToSend.X = computer.Comp.LastOutput;
                computer.IsNextOtuputX = false;
            } else {
                computer.PacketPreparingToSend.Y = computer.Comp.LastOutput;
                if (computer.PacketPreparingToSend.Address == 255) {
                    _valueSentTo255 = computer.PacketPreparingToSend;
                    _sentTo255 = true;
                } else {
                    _computers[computer.PacketPreparingToSend.Address].Packets.AddLast(computer.PacketPreparingToSend);
                }
                computer.PacketPreparingToSend = null;
            }
        }

        private class Computer {
            public int Address { get; set; }
            public IntComputer Comp { get; set; }
            public LinkedList<Packet> Packets { get; set; }
            public bool IsBooting { get; set; }
            public Packet PacketPreparingToSend { get; set; }
            public bool IsNextOtuputX { get; set; }
            public bool IsFinishedInput { get; set; }
        }

        private class Packet {
            public int Address { get; set; }
            public long X { get; set; }
            public long Y { get; set; }
            public bool IsXRead { get; set; }
        }
    }
}
