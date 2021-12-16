using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem16 : AdventOfCodeBase {
        private List<char> _binary;
        private int _index;
        private Packet _packet;

        public override string ProblemName {
            get { return "Advent of Code 2021: 16"; }
        }

        public override string GetAnswer() {
            ConvertToBinary(Input()[0]);
            _packet = new Packet() { Subpackets = new List<Packet>() };
            var finalZeros = FindStartOfFinalZeros();
            do {
                ParseBegin();
            } while (_index < finalZeros);
            _packet = _packet.Parent;
            return Answer2().ToString();
        }

        private ulong Answer1() {
            return VersionSum(_packet);
        }

        private ulong Answer2() {
            return CalcSum(_packet.Subpackets[0]);
        }

        private ulong CalcSum(Packet packet) {
            ulong sum = 0;
            switch (packet.TypeId) {
                case 0: // sum
                    packet.Subpackets.ForEach(x => sum += CalcSum(x));
                    break;
                case 1: // product
                    sum = 1;
                    packet.Subpackets.ForEach(x => sum *= CalcSum(x));
                    break;
                case 2: // min
                    sum = int.MaxValue;
                    packet.Subpackets.ForEach(x => {
                        var result = CalcSum(x);
                        if (result < sum) {
                            sum = result;
                        }
                    });
                    break;
                case 3: // max
                    packet.Subpackets.ForEach(x => {
                        var result = CalcSum(x);
                        if (result > sum) {
                            sum = result;
                        }
                    });
                    break;
                case 4: // literal
                    return packet.Value;
                case 5: // greater
                    return (CalcSum(packet.Subpackets[0]) > CalcSum(packet.Subpackets[1]) ? (ulong)1 : 0);
                case 6: // less
                    return (CalcSum(packet.Subpackets[0]) < CalcSum(packet.Subpackets[1]) ? (ulong)1 : 0);
                case 7: // equal
                    return (CalcSum(packet.Subpackets[0]) == CalcSum(packet.Subpackets[1]) ? (ulong)1 : 0);
                default:
                    throw new Exception();
            }
            return sum;
        }

        private ulong VersionSum(Packet packet) {
            ulong sum = 0;
            sum += packet.Version;
            foreach (var sub in packet.Subpackets) {
                sum += VersionSum(sub);
            }
            return sum;
        }

        private int FindStartOfFinalZeros() {
            for (int index = _binary.Count - 1; index >= 0; index--) {
                if (_binary[index] == '1') return index + 1;
            }
            return -1;
        }

        private void ParseBegin() {
            var v = CharToNum(_binary, _index, 3, 0);
            var t = CharToNum(_binary, _index + 3, 3, 0);
            _index += 6;
            var nextPacket = new Packet() {
                Parent = _packet,
                Subpackets = new List<Packet>(),
                TypeId = t,
                Version = v
            };
            _packet.Subpackets.Add(nextPacket);
            _packet = nextPacket;
            if (t == 4) {
                _packet.Value = GetLiteral();
            } else {
                var lengthType = _binary[_index];
                _index++;
                if (lengthType == '0') {
                    SkipTypeId0();
                } else {
                    SkipTypeId1();
                }
            }
        }

        private ulong GetLiteral() {
            var start = _index;
            while (_binary[_index] == '1') {
                _index += 5;
            }
            _index += 5;
            return CharToNum(_binary, start, _index - start, 5);
        }

        private void SkipTypeId0() {
            var length = CharToNum(_binary, _index, 15, 0);
            _index += 15;
            var end = _index + (int)length;
            while (_index < end) {
                ParseBegin();
                _packet = _packet.Parent;
            }
        }

        private void SkipTypeId1() {
            var length = (int)CharToNum(_binary, _index, 11, 0);
            _index += 11;
            for (int count = 1; count <= length; count++) {
                ParseBegin();
                _packet = _packet.Parent;
            }
        }

        private ulong CharToNum(List<char> list, int start, int length, int skipOffset) {
            int index = start + length - 1;
            ulong powerOf2 = 1;
            ulong num = 0;
            for (int count = 1; count <= length; count++) {
                if (skipOffset == 0 || count % skipOffset != 0) {
                    if (list[index] == '1') num += powerOf2;
                    powerOf2 *= 2;
                }
                index--;
            }
            return num;
        }

        private void ConvertToBinary(string line) {
            _binary = new List<char>();
            var hash = GetHexToBinary();
            foreach (var digit in line) {
                _binary.AddRange(hash[digit]);
            }
        }

        private Dictionary<char, List<char>> GetHexToBinary() {
            var hexToBinary = new Dictionary<char, List<char>>();
            hexToBinary.Add('0', new List<char>() { '0', '0', '0', '0' });
            hexToBinary.Add('1', new List<char>() { '0', '0', '0', '1' });
            hexToBinary.Add('2', new List<char>() { '0', '0', '1', '0' });
            hexToBinary.Add('3', new List<char>() { '0', '0', '1', '1' });
            hexToBinary.Add('4', new List<char>() { '0', '1', '0', '0' });
            hexToBinary.Add('5', new List<char>() { '0', '1', '0', '1' });
            hexToBinary.Add('6', new List<char>() { '0', '1', '1', '0' });
            hexToBinary.Add('7', new List<char>() { '0', '1', '1', '1' });
            hexToBinary.Add('8', new List<char>() { '1', '0', '0', '0' });
            hexToBinary.Add('9', new List<char>() { '1', '0', '0', '1' });
            hexToBinary.Add('A', new List<char>() { '1', '0', '1', '0' });
            hexToBinary.Add('B', new List<char>() { '1', '0', '1', '1' });
            hexToBinary.Add('C', new List<char>() { '1', '1', '0', '0' });
            hexToBinary.Add('D', new List<char>() { '1', '1', '0', '1' });
            hexToBinary.Add('E', new List<char>() { '1', '1', '1', '0' });
            hexToBinary.Add('F', new List<char>() { '1', '1', '1', '1' });
            return hexToBinary;
        }

        private class Packet {
            public ulong Version { get; set; }
            public ulong TypeId { get; set; }
            public Packet Parent { get; set; }
            public List<Packet> Subpackets { get; set; }
            public ulong Value { get; set; }
        }
    }
}
