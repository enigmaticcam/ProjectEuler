using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem14 : AdventOfCodeBase {
        private Dictionary<ulong, ulong> _memory;

        private enum enumCommandType {
            Mask,
            Memory
        }

        public override string ProblemName => "Advent of Code 2020: 14";

        public override string GetAnswer() {
            _memory = new Dictionary<ulong, ulong>();
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            var commands = GetCommands(input);
            RunCommands(commands, false);
            return GetSum();
        }

        private ulong Answer2(List<string> input) {
            var commands = GetCommands(input);
            RunCommands(commands, true);
            return GetSum();
        }

        private void RunCommands(List<Command> commands, bool useMaskOnAddress) {
            ulong overwrite = 0;
            ulong mask = 0;
            ulong overwriteAddress = 0;
            ulong maskAddress = 0;
            foreach (var command in commands) {
                if (command.ComType == enumCommandType.Mask) {
                    overwrite = command.Overwrite;
                    mask = command.Mask;
                    overwriteAddress = command.OverwriteAddress;
                    maskAddress = command.MaskAddress;
                } else if (!useMaskOnAddress) {
                    ulong value = command.Value;
                    value &= mask;
                    value |= overwrite;
                    AddToMemory(command.Address, value);
                } else {
                    WriteToAddresses(maskAddress, overwriteAddress, mask, command.Address, command.Value);
                }
            }
        }

        private ulong GetSum() {
            ulong sum = 0;
            foreach (var value in _memory.Values) {
                sum += value;
            }
            return sum;
        }

        private void WriteToAddresses(ulong mask, ulong overwrite, ulong floating, ulong address, ulong value) {
            address &= mask;
            address |= overwrite;
            var digits = FindDigits(floating);
            SetAllAddresses(value, address, digits, 0);
        }

        private List<ulong> FindDigits(ulong floating) {
            var digits = new List<ulong>();
            ulong power = 1;
            while (floating > 0) {
                if ((floating & 1) == 1) {
                    digits.Add(power);
                }
                floating >>= 1;
                power *= 2;
            }
            return digits;
        }

        private void SetAllAddresses(ulong value, ulong address, List<ulong> digits, int digitIndex) {
            address += digits[digitIndex];
            if (digitIndex == digits.Count - 1) {
                AddToMemory(address, value);
                address -= digits[digitIndex];
                AddToMemory(address, value);
            } else {
                SetAllAddresses(value, address, digits, digitIndex + 1);
                address -= digits[digitIndex];
                SetAllAddresses(value, address, digits, digitIndex + 1);
            }
        }

        private void AddToMemory(ulong address, ulong value) {
            if (!_memory.ContainsKey(address)) {
                _memory.Add(address, value);
            } else {
                _memory[address] = value;
            }
        }

        private List<Command> GetCommands(List<string> input) {
            return input.Select(line => {
                var command = new Command();
                if (line.Substring(0, 3) == "mas") {
                    command.ComType = enumCommandType.Mask;
                    var text = line.Substring(7, 36);
                    ulong power = 1;
                    for (int index = text.Length - 1; index >= 0; index--) {
                        var digit = text[index];
                        if (digit == '1') {
                            command.Overwrite += power;
                            command.OverwriteAddress += power;
                        } else if (digit == 'X') {
                            command.Mask += power;
                        } else if (digit == '0') {
                            command.MaskAddress += power;
                        }
                        power *= 2;
                    }
                } else {
                    command.ComType = enumCommandType.Memory;
                    command.Address = Convert.ToUInt64(line.Substring(4, line.IndexOf(']') - 4));
                    var equals = line.IndexOf('=');
                    command.Value = Convert.ToUInt64(line.Substring(equals + 1, line.Length - equals - 1));
                }
                return command;
            }).ToList();
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
                "mem[8] = 11",
                "mem[7] = 101",
                "mem[8] = 0"
            };
        }

        private List<string>Test2Input() {
            return new List<string>() {
                "mask = 000000000000000000000000000000X1001X",
                "mem[42] = 100",
                "mask = 00000000000000000000000000000000X0XX",
                "mem[26] = 1"
            };
        }

        private class Command {
            public ulong Overwrite { get; set; }
            public ulong OverwriteAddress { get; set; }
            public ulong Mask { get; set; }
            public ulong MaskAddress { get; set; }
            public ulong Address { get; set; }
            public ulong Value { get; set; }
            public enumCommandType ComType { get; set; }
        }
    }
}
