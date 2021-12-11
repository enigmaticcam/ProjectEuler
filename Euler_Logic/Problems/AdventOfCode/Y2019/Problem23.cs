using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem23 : AdventOfCodeBase {
        private Dictionary<int, Computer> _computers;
        private bool _sentTo255;
        private int _valueSentTo255;

        public override string ProblemName {
            get { return "Advent of Code 2019: 23"; }
        }

        public override string GetAnswer() {
            return Answer1().ToString();
        }

        private int Answer1() {
            BootComputers();
            LookForFirstTo255();
            return _valueSentTo255;
        }

        private void BootComputers() {
            _computers = new Dictionary<int, Computer>();
            for (int address = 0; address < 50; address++) {
                _computers.Add(address, new Computer() {
                    Address = address,
                    Comp = new IntComputer(),
                    IsBooting = true,
                    Packets = new LinkedList<Packet>()
                });
                _computers[address].Comp.SingleOutputOnly = true;
            }
        }

        private void LookForFirstTo255() {
            do {
                foreach (var comp in _computers.Values) {
                    comp.Comp.Run(Input(), () => InputCaller(comp), () => OutputCaller(comp));
                }
            } while (!_sentTo255);
        }

        private int InputCaller(Computer computer) {
            if (computer.IsBooting) {
                computer.IsBooting = false;
                return computer.Address;
            } else if (computer.Packets.Count == 0) {
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
                computer.PacketPreparingToSend.X = (int)computer.Comp.LastOutput;
                computer.IsNextOtuputX = false;
            } else {
                computer.PacketPreparingToSend.Y = (int)computer.Comp.LastOutput;
                if (computer.PacketPreparingToSend.Address == 255) {
                    _valueSentTo255 = (int)computer.Comp.LastOutput;
                    _sentTo255 = true;
                } else {
                    _computers[computer.PacketPreparingToSend.Address].Packets.AddLast(computer.PacketPreparingToSend);
                    computer.PacketPreparingToSend = null;
                }
            }
        }

        private class Computer {
            public int Address { get; set; }
            public IntComputer Comp { get; set; }
            public LinkedList<Packet> Packets { get; set; }
            public bool IsBooting { get; set; }
            public Packet PacketPreparingToSend { get; set; }
            public bool IsNextOtuputX { get; set; }
        }

        private class Packet {
            public int Address { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public bool IsXRead { get; set; }
        }
    }
}
