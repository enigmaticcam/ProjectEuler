using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem24 : AdventOfCodeBase {
        private List<List<Move>> _moves;
        private Dictionary<Tuple<int, int>, bool> _hash;

        public override string ProblemName => "Advent of Code 2020: 24";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetMoves(input);
            return PerformFlips();
        }

        private int Answer2(List<string> input) {
            GetMoves(input);
            PerformFlips();
            return PerformFlipsAcrossDays(100);
        }

        private int PerformFlips() {
            _hash = new Dictionary<Tuple<int, int>, bool>();
            foreach (var moves in _moves) {
                int x = 0;
                int y = 0;
                foreach (var move in moves) {
                    x += move.X;
                    y += move.Y;
                }
                var key = new Tuple<int, int>(x, y);
                if (_hash.ContainsKey(key)) {
                    _hash[key] = !_hash[key];
                } else {
                    _hash.Add(key, true);
                }
            }
            return GetFlippedCount();
        }

        private int PerformFlipsAcrossDays(int days) {
            var neighbors = new List<Move>() { new MoveE(), new MoveNE(), new MoveNW(), new MoveSE(), new MoveSW(), new MoveW() };
            for (int day = 1; day <= days; day++) {
                var counts = new Dictionary<Tuple<int, int>, int>();
                foreach (var black in _hash.Where(x => x.Value)) {
                    foreach (var neighbor in neighbors) {
                        var key = new Tuple<int, int>(black.Key.Item1 + neighbor.X, black.Key.Item2 + neighbor.Y);
                        if (counts.ContainsKey(key)) {
                            counts[key]++;
                        } else {
                            counts.Add(key, 1);
                        }
                    }
                }
                var flips = new List<Tuple<int, int>>();
                foreach (var tile in counts) {
                    if (!_hash.ContainsKey(tile.Key)) {
                        _hash.Add(tile.Key, false);
                    }
                    var value = _hash[tile.Key];
                    if (!value) {
                        if (tile.Value == 2) {
                            flips.Add(tile.Key);
                        }
                    } else {
                        if (tile.Value == 0 || tile.Value > 2) {
                            flips.Add(tile.Key);
                        }
                    }
                }
                foreach (var black in _hash.Where(x => x.Value)) {
                    if (!counts.ContainsKey(black.Key)) {
                        flips.Add(black.Key);
                    }
                }
                foreach (var flip in flips) {
                    _hash[flip] = !_hash[flip];
                }
            }
            return GetFlippedCount();
        }

        private int GetFlippedCount() {
            return _hash.Values.Where(value => value).Count();
        }

        private void GetMoves(List<string> input) {
            _moves = new List<List<Move>>();
            foreach (var line in input) {
                var moves = new List<Move>();
                int index = 0;
                while (index < line.Length) {
                    if (line[index] == 'e') {
                        moves.Add(new MoveE());
                        index++;
                    } else if (line[index] == 'w') {
                        moves.Add(new MoveW());
                        index++;
                    } else if (line[index] == 's' && line[index + 1] == 'e') {
                        moves.Add(new MoveSE());
                        index += 2;
                    } else if (line[index] == 's' && line[index + 1] == 'w') {
                        moves.Add(new MoveSW());
                        index += 2;
                    } else if (line[index] == 'n' && line[index + 1] == 'w') {
                        moves.Add(new MoveNW());
                        index += 2;
                    } else if (line[index] == 'n' && line[index + 1] == 'e') {
                        moves.Add(new MoveNE());
                        index += 2;
                    }
                }
                _moves.Add(moves);
            }
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "esew"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "nwwswee"
            };
        }

        private List<string> Test3Input() {
            return new List<string>() {
                "sesenwnenenewseeswwswswwnenewsewsw",
                "neeenesenwnwwswnenewnwwsewnenwseswesw",
                "seswneswswsenwwnwse",
                "nwnwneseeswswnenewneswwnewseswneseene",
                "swweswneswnenwsewnwneneseenw",
                "eesenwseswswnenwswnwnwsewwnwsene",
                "sewnenenenesenwsewnenwwwse",
                "wenwwweseeeweswwwnwwe",
                "wsweesenenewnwwnwsenewsenwwsesesenwne",
                "neeswseenwwswnwswswnw",
                "nenwswwsewswnenenewsenwsenwnesesenew",
                "enewnwewneswsewnwswenweswnenwsenwsw",
                "sweneswneswneneenwnewenewwneswswnese",
                "swwesenesewenwneswnwwneseswwne",
                "enesenwswwswneneswsenwnewswseenwsese",
                "wnwnesenesenenwwnenwsewesewsesesew",
                "nenewswnwewswnenesenwnesewesw",
                "eneswnwswnwsenenwnwnwwseeswneewsenese",
                "neswnwewnwnwseenwseesewsenwsweewe",
                "wseweeenwnesenwwwswnew"
            };
        }

        private abstract class Move {
            public abstract string Text { get; }
            public abstract int X { get; }
            public abstract int Y { get; }
        }

        private class MoveE : Move {
            public override string Text { get => "E"; }
            public override int X => 1;
            public override int Y => 0;
        }

        private class MoveSE : Move {
            public override string Text { get => "SE"; }
            public override int X => 0;
            public override int Y => -1;
        }

        private class MoveSW : Move {
            public override string Text { get => "SW"; }
            public override int X => -1;
            public override int Y => -1;
        }

        private class MoveW : Move {
            public override string Text { get => "W"; }
            public override int X => -1;
            public override int Y => 0;
        }

        private class MoveNW : Move {
            public override string Text { get => "NW"; }
            public override int X => 0;
            public override int Y => 1;
        }

        private class MoveNE : Move {
            public override string Text { get => "NE"; }
            public override int X => 1;
            public override int Y => 1;
        }
    }
}
