using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem11 : AdventOfCodeBase {
        private PowerAll _powerOf2;
        private Dictionary<string, Pair> _pairs;
        private Dictionary<string, Single> _singles;
        private List<Single> _singlesOrdered;
        private HashSet<ulong> _visited;
        private int _solvedCount;
        private ulong _mask;

        public override string ProblemName => "Advent of Code 2016: 11";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            _powerOf2 = new PowerAll(2);
            GetPairs(input);
            SetFloors();
            SetMask();
            return FindOptimal();
        }

        private int Answer2(List<string> input) {
            _powerOf2 = new PowerAll(2);
            GetPairs(input);
            AddPart2();
            SetFloors();
            SetMask();
            return FindOptimal();
        }

        private int FindOptimal() {
            _visited = new HashSet<ulong>();
            var positions = new List<ulong>();
            positions.Add(_mask);
            int steps = 0;
            do {
                steps++;
                var newPositions = new List<ulong>();
                foreach (var position in positions) {
                    ulong mask = position;
                    ParsePosition(position);
                    var elevator = (position & 15);
                    for (int index1 = 0; index1 < _singlesOrdered.Count; index1++) {
                        var single1 = _singlesOrdered.ElementAt(index1);
                        if (single1.Floor == elevator) {

                            // Move one up
                            if (elevator < 3) {
                                mask -= single1.Bit;
                                single1.Bit <<= _singles.Count;
                                mask += single1.Bit;
                                single1.Floor++;
                                mask++;
                                if (!_visited.Contains(mask) && IsGood()) {
                                    if (_solvedCount == _singles.Count - 1 && single1.Floor == 3) {
                                        return steps;
                                    }
                                    newPositions.Add(mask);
                                    _visited.Add(mask);
                                }
                                single1.Floor--;
                                mask -= single1.Bit;
                                single1.Bit >>= _singles.Count;
                                mask += single1.Bit;
                                mask--;
                            }

                            // Move one down
                            if (elevator > 0) {
                                mask -= single1.Bit;
                                single1.Bit >>= _singles.Count;
                                mask += single1.Bit;
                                single1.Floor--;
                                mask--;
                                if (!_visited.Contains(mask) && IsGood()) {
                                    if (_solvedCount == _singles.Count - 1 && single1.Floor == 3) {
                                        return steps;
                                    }
                                    newPositions.Add(mask);
                                    _visited.Add(mask);
                                }
                                single1.Floor++;
                                mask -= single1.Bit;
                                single1.Bit <<= _singles.Count;
                                mask += single1.Bit;
                                mask++;
                            }

                            for (int index2 = index1 + 1; index2 < _singlesOrdered.Count; index2++) {
                                var single2 = _singlesOrdered.ElementAt(index2);
                                if (single2.Floor == elevator) {

                                    // Move two up
                                    if (elevator < 3) {
                                        mask -= single1.Bit;
                                        mask -= single2.Bit;
                                        single1.Bit <<= _singles.Count;
                                        single2.Bit <<= _singles.Count;
                                        mask += single1.Bit;
                                        mask += single2.Bit;
                                        single1.Floor++;
                                        single2.Floor++;
                                        mask++;
                                        if (!_visited.Contains(mask) && IsGood()) {
                                            if (_solvedCount == _singles.Count - 2 && single1.Floor == 3) {
                                                return steps;
                                            }
                                            newPositions.Add(mask);
                                            _visited.Add(mask);
                                        }
                                        single1.Floor--;
                                        single2.Floor--;
                                        mask -= single1.Bit;
                                        mask -= single2.Bit;
                                        single1.Bit >>= _singles.Count;
                                        single2.Bit >>= _singles.Count;
                                        mask += single1.Bit;
                                        mask += single2.Bit;
                                        mask--;
                                    }

                                    // Move two down
                                    if (elevator > 0) {
                                        mask -= single1.Bit;
                                        mask -= single2.Bit;
                                        single1.Bit >>= _singles.Count;
                                        single2.Bit >>= _singles.Count;
                                        mask += single1.Bit;
                                        mask += single2.Bit;
                                        single1.Floor--;
                                        single2.Floor--;
                                        mask--;
                                        if (!_visited.Contains(mask) && IsGood()) {
                                            if (_solvedCount == _singles.Count - 2 && single1.Floor == 3) {
                                                return steps;
                                            }
                                            newPositions.Add(mask);
                                            _visited.Add(mask);
                                        }
                                        single1.Floor++;
                                        single2.Floor++;
                                        mask -= single1.Bit;
                                        mask -= single2.Bit;
                                        single1.Bit <<= _singles.Count;
                                        single2.Bit <<= _singles.Count;
                                        mask += single1.Bit;
                                        mask += single2.Bit;
                                        mask++;
                                    }
                                }

                                
                            }
                        }

                        
                    }
                }
                positions = newPositions;
            } while (true);
        }

        private void SetSpecific() {
            _mask = 0;
            _singlesOrdered[0].Floor = 2;
            _singlesOrdered[1].Floor = 3;
            _singlesOrdered[2].Floor = 2;
            _singlesOrdered[3].Floor = 3;
            SetMask();
            _mask += 2;
        }

        private void ParsePosition(ulong position) {
            _solvedCount = 0;
            position >>= 4;
            ulong bit = _powerOf2.GetPower(4);
            for (ulong floor = 0; floor <= 3; floor++) {
                foreach (var single in _singlesOrdered) {
                    if (position % 2 != 0) {
                        single.Bit = bit;
                        single.Floor = floor;
                        _solvedCount += (floor == 3 ? 1 : 0);
                    }
                    bit *= 2;
                    position >>= 1;
                }
            }
        }

        private bool IsGood() {
            for (int index1 = 0; index1 < _pairs.Values.Count; index1++) {
                var pair1 = _pairs.Values.ElementAt(index1);
                for (int index2 = index1 + 1; index2 < _pairs.Values.Count; index2++) {
                    var pair2 = _pairs.Values.ElementAt(index2);
                    if (_singles[pair1.NameChip].Floor == _singles[pair2.NameGenerator].Floor && _singles[pair1.NameGenerator].Floor != _singles[pair1.NameChip].Floor) {
                        return false;
                    }
                    if (_singles[pair1.NameGenerator].Floor == _singles[pair2.NameChip].Floor && _singles[pair2.NameGenerator].Floor != _singles[pair2.NameChip].Floor) {
                        return false;
                    }
                }
            }
            return true;
        }

        private void SetMask() {
            ulong bit = _powerOf2.GetPower(4);
            for (ulong floor = 0; floor <= 3; floor++) {
                foreach (var single in _singlesOrdered) {
                    if (single.Floor == floor) {
                        single.Bit = bit;
                        _mask += single.Bit;
                    }
                    bit *= 2;
                }
            }
        }

        private void SetFloors() {
            _singles = new Dictionary<string, Single>();
            int id = 0;
            foreach (var pair in _pairs.Values) {
                var singleChip = new Single() {
                    //Bit = pair.BitChip << pair.StartFloorChip * _pairs.Count * 2 + 4,
                    Floor = (ulong)pair.StartFloorChip,
                    Name = pair.NameChip,
                    Id = id,
                };
                var singleGen = new Single() {
                    //Bit = pair.BitGenerator << pair.StartFloorGenerator * _pairs.Count * 2 + 4,
                    Floor = (ulong)pair.StartFloorGenerator,
                    PairOther = singleChip,
                    Name = pair.NameGenerator,
                    Id = id + 1
                };
                singleChip.PairOther = singleGen;
                _singles.Add(singleChip.Name, singleChip);
                _singles.Add(singleGen.Name, singleGen);
                id++;
            }
            _singlesOrdered = _singles.Values.OrderBy(x => x.Id).ToList();
        }

        private void GetPairs(List<string> input) {
            _pairs = new Dictionary<string, Pair>();
            int floor = 0;
            input.ForEach(line => {
                var split = line.Split(' ');
                for (int index = 0; index < split.Length; index++) {
                    var word = split[index].Replace(",", "").Replace(".", "");
                    if (word == "generator" || word == "microchip") {
                        var name = split[index - 1].Replace(",", "").Replace("-compatible", "");
                        if (!_pairs.ContainsKey(name)) {
                            _pairs.Add(name, new Pair() {
                                BitGenerator = _powerOf2.GetPower(_pairs.Count * 2),
                                BitChip = _powerOf2.GetPower((_pairs.Count * 2) + 1),
                                Name = name,
                                NameChip = name + " chip",
                                NameGenerator = name + " generator"
                            });
                        }
                        var pair = _pairs[name];
                        if (word == "generator") {
                            pair.StartFloorGenerator = floor;
                        } else {
                            pair.StartFloorChip = floor;
                        }
                    }
                }
                floor++;
            });
        }

        private void AddPart2() {
            _pairs.Add("elerium", new Pair() {
                BitChip = _powerOf2.GetPower((_pairs.Count * 2) + 1),
                BitGenerator = _powerOf2.GetPower((_pairs.Count * 2)),
                Name = "elerium",
                NameChip = "elerium chip",
                NameGenerator = "elerium generator",
            });
            _pairs.Add("dilithium", new Pair() {
                BitChip = _powerOf2.GetPower((_pairs.Count * 2) + 1),
                BitGenerator = _powerOf2.GetPower((_pairs.Count * 2)),
                Name = "dilithium",
                NameChip = "dilithium chip",
                NameGenerator = "dilithium generator"
            });
        }

        private List<string> TestInput() {
            return new List<string>() {
                "The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.",
                "The second floor contains a hydrogen generator.",
                "The third floor contains a lithium generator.",
                "The fourth floor contains nothing relevant."
            };
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
            public Single PairOther { get; set; }
            public ulong Floor { get; set; }
            public int Id { get; set; }
        }
    }
}
