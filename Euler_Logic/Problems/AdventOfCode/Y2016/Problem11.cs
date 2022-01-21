using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem11 : AdventOfCodeBase {
        private List<Pair> _pairs;
        private List<Single> _singles;
        private Dictionary<int, Dictionary<ulong, int>> _hash;

        public override string ProblemName => "Advent of Code 2016: 11";

        public override string GetAnswer() {
            return Answer1(Input_Test(1)).ToString();
        }

        private int Answer1(List<string> input) {
            _hash = new Dictionary<int, Dictionary<ulong, int>>();
            GetPairs(input);
            //AddPart2();
            SetPairBits();
            SetSingles();
            var key = GetKey();
            return Recursive(key, 0);
        }

        private int Recursive(ulong key, int elevator) {
            if (!_hash.ContainsKey(elevator)) {
                _hash.Add(elevator, new Dictionary<ulong, int>());
            }
            if (!_hash[elevator].ContainsKey(key)) {
                _hash[elevator].Add(key, 0);
                int best = int.MaxValue;
                for (int single1Index = 0; single1Index < _singles.Count; single1Index++) {
                    var single1 = _singles[single1Index];
                    if (single1.Floor == elevator) {
                        for (int nextFloor = 3; nextFloor >= 0; nextFloor--) {
                            if ((nextFloor == elevator - 1 || nextFloor == elevator + 1) && CanMoveToFloor(single1, nextFloor)) {
                                single1.Floor = nextFloor;
                                if (IsSolved()) {
                                    best = 1;
                                } else {
                                    var next = Recursive(GetKey(), nextFloor);
                                    if (next != int.MaxValue && next + 1 < best && next > 0) best = next + 1;
                                }
                                for (int single2Index = single1Index + 1; single2Index < _singles.Count; single2Index++) {
                                    var single2 = _singles[single2Index];
                                    if (single2.Floor == elevator && (single2.IsChip == single1.IsChip || single2.Pair == single1Index) && CanMoveToFloor(single2, nextFloor)) {
                                        single2.Floor = nextFloor;
                                        if (IsSolved()) {
                                            best = 1;
                                        } else {
                                            var next = Recursive(GetKey(), nextFloor);
                                            if (next != int.MaxValue && next + 1 < best && next > 0) best = next + 1;
                                        }
                                        single2.Floor = elevator;
                                    }
                                }
                                single1.Floor = elevator;
                            }
                        }
                    }
                }
                _hash[elevator][key] = best;
            }
            return _hash[elevator][key];
        }

        private bool IsSolved() {
            foreach (var single in _singles) {
                if (single.Floor != 3) return false;
            }
            return true;
        }

        private bool CanMoveToFloor(Single single, int floor) {
            if (_singles[single.Pair].Floor == floor) return true;
            foreach (var next in _singles) {
                //if (next.IsChip)
            }
            return true;
        }

        private ulong GetKey() {
            ulong key = 0;
            foreach (var single in _singles) {
                key += single.Bit << (_singles.Count * single.Floor);
            }
            return key;
        }

        private void SetSingles() {
            _singles = new List<Single>();
            foreach (var pair in _pairs) {
                _singles.Add(new Single() {
                    Bit = pair.BitChip,
                    IsChip = true,
                    Name = pair.NameChip,
                    Pair = _singles.Count + 2,
                    Floor = pair.StartFloorChip
                });
                _singles.Add(new Single() {
                    Bit = pair.BitGenerator,
                    IsChip = false,
                    Name = pair.NameGenerator,
                    Pair = _singles.Count + 1,
                    Floor = pair.StartFloorGenerator
                });
            }
        }

        private void SetPairBits() {
            ulong bit = 1;
            foreach (var pair in _pairs) {
                pair.BitChip = bit;
                pair.BitGenerator = bit * 2;
                bit *= 4;
            }
        }

        private void GetPairs(List<string> input) {
            var pairs = new Dictionary<string, Pair>();
            int floor = 0;
            input.ForEach(line => {
                var split = line.Split(' ');
                for (int index = 0; index < split.Length; index++) {
                    var word = split[index].Replace(",", "").Replace(".", "");
                    if (word == "generator" || word == "microchip") {
                        var name = split[index - 1].Replace(",", "").Replace("-compatible", "");
                        if (!pairs.ContainsKey(name)) {
                            pairs.Add(name, new Pair() {
                                Name = name,
                                NameChip = name + " chip",
                                NameGenerator = name + " generator"
                            });
                        }
                        var pair = pairs[name];
                        if (word == "generator") {
                            pair.StartFloorGenerator = floor;
                        } else {
                            pair.StartFloorChip = floor;
                        }
                    }
                }
                floor++;
            });
            _pairs = pairs.Values.ToList();
        }

        private void AddPart2() {
            _pairs.Add(new Pair() {
                Name = "elerium",
                NameChip = "elerium chip",
                NameGenerator = "elerium generator",
            });
            _pairs.Add(new Pair() {
                Name = "dilithium",
                NameChip = "dilithium chip",
                NameGenerator = "dilithium generator"
            });
        }

        private class Pair {
            public string Name { get; set; }
            public ulong BitGenerator { get; set; }
            public ulong BitChip { get; set; }
            public int StartFloorGenerator { get; set; }
            public int StartFloorChip { get; set; }
            public string NameGenerator { get; set; }
            public string NameChip { get; set; }
        }

        private class Single {
            public string Name { get; set; }
            public ulong Bit { get; set; }
            public bool IsChip { get; set; }
            public int Pair { get; set; }
            public int Floor { get; set; }
        }
    }
}
