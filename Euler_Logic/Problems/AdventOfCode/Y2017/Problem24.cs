using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem24 : AdventOfCodeBase {
        private List<Magnet> _magnets;
        private int _best;
        private int _longest;

        public override string ProblemName {
            get { return "Advent of Code 2017: 24"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetMagnets(input);
            SetMatches();
            StartFindBest();
            return _best;
        }

        private int Answer2(List<string> input) {
            GetMagnets(input);
            SetMatches();
            StartFindLongest();
            return _best;
        }

        private void StartFindLongest() {
            foreach (var magnet in _magnets) {
                if (magnet.SideA == 0) {
                    FindLongest(magnet, true, magnet.Bit, magnet.Value, 1);
                } else if (magnet.SideB == 0) {
                    FindLongest(magnet, false, magnet.Bit, magnet.Value, 1);
                }
            }
        }

        private void StartFindBest() {
            foreach (var magnet in _magnets) {
                if (magnet.SideA == 0) {
                    FindStrongest(magnet, true, magnet.Bit, magnet.Value);
                } else if (magnet.SideB == 0) {
                    FindStrongest(magnet, false, magnet.Bit, magnet.Value);
                }
            }
        }

        private void FindLongest(Magnet last, bool lastIsSideA, ulong bits, int sum, int count) {
            var nextMagnets = (lastIsSideA ? last.MatchingOnB : last.MatchingOnA);
            var matchValue = (lastIsSideA ? last.SideB : last.SideA);
            foreach (var magnet in nextMagnets) {
                if ((magnet.Bit & bits) == 0) {
                    if (matchValue == magnet.SideA) {
                        var nextSum = sum + magnet.Value;
                        var nextCount = count + 1;
                        if (nextCount > _longest) {
                            _longest = nextCount;
                            _best = nextSum;
                        } else if (nextCount == _longest && nextSum > _best) {
                            _best = nextSum;
                        }
                        FindLongest(magnet, true, bits + magnet.Bit, nextSum, nextCount);
                    } else if (matchValue == magnet.SideB) {
                        var nextSum = sum + magnet.Value;
                        var nextCount = count + 1;
                        if (nextCount > _longest) {
                            _longest = nextCount;
                            _best = nextSum;
                        } else if (nextCount == _longest && nextSum > _best) {
                            _best = nextSum;
                        }
                        FindLongest(magnet, false, bits + magnet.Bit, nextSum, nextCount);
                    }
                }
            }
        }

        private void FindStrongest(Magnet last, bool lastIsSideA, ulong bits, int sum) {
            var nextMagnets = (lastIsSideA ? last.MatchingOnB : last.MatchingOnA);
            var matchValue = (lastIsSideA ? last.SideB : last.SideA);
            foreach (var magnet in nextMagnets) {
                if ((magnet.Bit & bits) == 0) {
                    if (matchValue == magnet.SideA) {
                        var nextSum = sum + magnet.Value;
                        if (nextSum > _best) {
                            _best = nextSum; 
                        }
                        FindStrongest(magnet, true, bits + magnet.Bit, nextSum);
                    } else if (matchValue == magnet.SideB) {
                        var nextSum = sum + magnet.Value;
                        if (nextSum > _best) {
                            _best = nextSum;
                        }
                        FindStrongest(magnet, false, bits + magnet.Bit, nextSum);
                    }
                }
            }
        }

        private void SetMatches() {
            for (int index1 = 0; index1 < _magnets.Count; index1++) {
                var magnetA = _magnets[index1];
                for (int index2 = index1 + 1; index2 < _magnets.Count; index2++) {
                    var magnetB = _magnets[index2];
                    if (magnetA.SideA == magnetB.SideA) {
                        magnetA.MatchingOnA.Add(magnetB);
                        magnetB.MatchingOnA.Add(magnetA);
                    }
                    if (magnetA.SideA == magnetB.SideB) {
                        magnetA.MatchingOnA.Add(magnetB);
                        magnetB.MatchingOnB.Add(magnetA);
                    }
                    if (magnetA.SideB == magnetB.SideA) {
                        magnetA.MatchingOnB.Add(magnetB);
                        magnetB.MatchingOnA.Add(magnetA);
                    }
                    if (magnetA.SideB == magnetB.SideB) {
                        magnetA.MatchingOnB.Add(magnetB);
                        magnetB.MatchingOnB.Add(magnetA);
                    }
                }
            }
        }

        private void GetMagnets(List<string> input) {
            _magnets = new List<Magnet>();
            ulong power = 1;
            foreach (var line in input) {
                var split = line.Split('/');
                var magnet = new Magnet() {
                    Bit = power,
                    MatchingOnA = new List<Magnet>(),
                    MatchingOnB = new List<Magnet>(),
                    SideA = Convert.ToInt32(split[0]),
                    SideB = Convert.ToInt32(split[1])
                };
                magnet.Value = magnet.SideA + magnet.SideB;
                _magnets.Add(magnet);
                power *= 2;
            }
        }

        private List<string> TestInput() {
            return new List<string>() {
                "0/2",
                "2/2",
                "2/3",
                "3/4",
                "3/5",
                "0/1",
                "10/1",
                "9/10"
            };
        }

        private class Magnet {
            public int SideA { get; set; }
            public int SideB { get; set; }
            public ulong Bit { get; set; }
            public List<Magnet> MatchingOnA { get; set; }
            public List<Magnet> MatchingOnB { get; set; }
            public int Value { get; set; }
        }
    }
}
