using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem13 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2020: 13";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        public ulong Answer1(List<string> input) {
            var notes = GetNotes(input);
            ulong lowest = ulong.MaxValue;
            ulong result = 0;
            foreach (var num in notes.Buses) {
                if (num != 0) {
                    var mod = notes.Num % num;
                    var remaining = num - mod;
                    if (remaining < lowest) {
                        lowest = remaining;
                        result = remaining * num;
                    }
                }
            }
            return result;
        }

        public ulong Answer2(List<string> input) {
            var notes = GetNotes(input);
            ulong offset = notes.Buses[0];
            ulong start = notes.Buses[0];
            ulong last = notes.Buses[0];
            for (int index = 1; index < notes.Buses.Count; index++) {
                if (notes.Buses[index] != 0) {
                    var bus = notes.Buses[index];
                    ulong indexOffset = 0;
                    if (bus > (ulong)index) {
                        indexOffset = bus - (ulong)index;
                    } else {
                        indexOffset = bus - ((ulong)index % bus);
                    }
                    ulong nextOffset = start;
                    ulong mod = 0;
                    do {
                        mod = nextOffset % bus;
                        if (mod == indexOffset) {
                            break;
                        }
                        nextOffset += offset;
                    } while (true);
                    offset *= bus;
                    start = nextOffset;
                    last = notes.Buses[index];
                }
            }
            return start;
        }

        private Notes GetNotes(List<string> input) {
            var notes = new Notes();
            notes.Num = Convert.ToUInt64(input[0]);
            var split = input[1].Split(',');
            notes.Buses = split.Select(id => {
                if (id == "x") {
                    return Convert.ToUInt64(0);
                } else {
                    return Convert.ToUInt64(id);
                }
            }).ToList();
            return notes;
        }

        private List<string> TestInput() {
            return new List<string>() {
                "1008713",
                "7,13,x,x,59,x,31,19"
            };
        }

        private class Notes {
            public ulong Num { get; set; }
            public List<ulong> Buses { get; set; }
        }
    }
}
