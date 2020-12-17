using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem17 : AdventOfCodeBase {
        private List<Cube> _cubes;
        private Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Cube>>>> _hash;

        public override string ProblemName => "Advent of Code 2020: 17";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer2(List<string> input) {
            GetInitialCubes(input);
            Perform(6);
            return GetSum();
        }

        private int GetSum() {
            return _cubes.Where(cube => cube.IsActive).Count();
        }

        private void Perform(int totalCount) {
            for (int count = 1; count <= totalCount; count++) {
                var hash = GetActiveCounts();
                SetActiveCubes(hash);
            }
        }

        private Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, int>>>> GetActiveCounts() {
            var counts = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, int>>>>();
            var test = new HashSet<string>();
            foreach (var cube in _cubes) {
                if (cube.IsActive) {
                    IncrementSpace(cube.W, cube.X + 1, cube.Y, cube.Z, counts);
                    IncrementSpace(cube.W, cube.X - 1, cube.Y, cube.Z, counts);
                    IncrementSpace(cube.W, cube.X + 1, cube.Y + 1, cube.Z, counts);
                    IncrementSpace(cube.W, cube.X - 1, cube.Y + 1, cube.Z, counts);
                    IncrementSpace(cube.W, cube.X, cube.Y + 1, cube.Z, counts);
                    IncrementSpace(cube.W, cube.X + 1, cube.Y - 1, cube.Z, counts);
                    IncrementSpace(cube.W, cube.X - 1, cube.Y - 1, cube.Z, counts);
                    IncrementSpace(cube.W, cube.X, cube.Y - 1, cube.Z, counts);
                    IncrementSpace(cube.W, cube.X + 1, cube.Y, cube.Z + 1, counts);
                    IncrementSpace(cube.W, cube.X - 1, cube.Y, cube.Z + 1, counts);
                    IncrementSpace(cube.W, cube.X, cube.Y, cube.Z + 1, counts);
                    IncrementSpace(cube.W, cube.X + 1, cube.Y + 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W, cube.X - 1, cube.Y + 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W, cube.X, cube.Y + 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W, cube.X + 1, cube.Y - 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W, cube.X - 1, cube.Y - 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W, cube.X, cube.Y - 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W, cube.X + 1, cube.Y, cube.Z - 1, counts);
                    IncrementSpace(cube.W, cube.X - 1, cube.Y, cube.Z - 1, counts);
                    IncrementSpace(cube.W, cube.X, cube.Y, cube.Z - 1, counts);
                    IncrementSpace(cube.W, cube.X + 1, cube.Y + 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W, cube.X - 1, cube.Y + 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W, cube.X, cube.Y + 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W, cube.X + 1, cube.Y - 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W, cube.X - 1, cube.Y - 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W, cube.X, cube.Y - 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W + 1, cube.X + 1, cube.Y, cube.Z, counts);
                    IncrementSpace(cube.W + 1, cube.X - 1, cube.Y, cube.Z, counts);
                    IncrementSpace(cube.W + 1, cube.X, cube.Y, cube.Z, counts);
                    IncrementSpace(cube.W + 1, cube.X + 1, cube.Y + 1, cube.Z, counts);
                    IncrementSpace(cube.W + 1, cube.X - 1, cube.Y + 1, cube.Z, counts);
                    IncrementSpace(cube.W + 1, cube.X, cube.Y + 1, cube.Z, counts);
                    IncrementSpace(cube.W + 1, cube.X + 1, cube.Y - 1, cube.Z, counts);
                    IncrementSpace(cube.W + 1, cube.X - 1, cube.Y - 1, cube.Z, counts);
                    IncrementSpace(cube.W + 1, cube.X, cube.Y - 1, cube.Z, counts);
                    IncrementSpace(cube.W + 1, cube.X + 1, cube.Y, cube.Z + 1, counts);
                    IncrementSpace(cube.W + 1, cube.X - 1, cube.Y, cube.Z + 1, counts);
                    IncrementSpace(cube.W + 1, cube.X, cube.Y, cube.Z + 1, counts);
                    IncrementSpace(cube.W + 1, cube.X + 1, cube.Y + 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W + 1, cube.X - 1, cube.Y + 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W + 1, cube.X, cube.Y + 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W + 1, cube.X + 1, cube.Y - 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W + 1, cube.X - 1, cube.Y - 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W + 1, cube.X, cube.Y - 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W + 1, cube.X + 1, cube.Y, cube.Z - 1, counts);
                    IncrementSpace(cube.W + 1, cube.X - 1, cube.Y, cube.Z - 1, counts);
                    IncrementSpace(cube.W + 1, cube.X, cube.Y, cube.Z - 1, counts);
                    IncrementSpace(cube.W + 1, cube.X + 1, cube.Y + 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W + 1, cube.X - 1, cube.Y + 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W + 1, cube.X, cube.Y + 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W + 1, cube.X + 1, cube.Y - 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W + 1, cube.X - 1, cube.Y - 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W + 1, cube.X, cube.Y - 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W - 1, cube.X + 1, cube.Y, cube.Z, counts);
                    IncrementSpace(cube.W - 1, cube.X - 1, cube.Y, cube.Z, counts);
                    IncrementSpace(cube.W - 1, cube.X, cube.Y, cube.Z, counts);
                    IncrementSpace(cube.W - 1, cube.X + 1, cube.Y + 1, cube.Z, counts);
                    IncrementSpace(cube.W - 1, cube.X - 1, cube.Y + 1, cube.Z, counts);
                    IncrementSpace(cube.W - 1, cube.X, cube.Y + 1, cube.Z, counts);
                    IncrementSpace(cube.W - 1, cube.X + 1, cube.Y - 1, cube.Z, counts);
                    IncrementSpace(cube.W - 1, cube.X - 1, cube.Y - 1, cube.Z, counts);
                    IncrementSpace(cube.W - 1, cube.X, cube.Y - 1, cube.Z, counts);
                    IncrementSpace(cube.W - 1, cube.X + 1, cube.Y, cube.Z + 1, counts);
                    IncrementSpace(cube.W - 1, cube.X - 1, cube.Y, cube.Z + 1, counts);
                    IncrementSpace(cube.W - 1, cube.X, cube.Y, cube.Z + 1, counts);
                    IncrementSpace(cube.W - 1, cube.X + 1, cube.Y + 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W - 1, cube.X - 1, cube.Y + 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W - 1, cube.X, cube.Y + 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W - 1, cube.X + 1, cube.Y - 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W - 1, cube.X - 1, cube.Y - 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W - 1, cube.X, cube.Y - 1, cube.Z + 1, counts);
                    IncrementSpace(cube.W - 1, cube.X + 1, cube.Y, cube.Z - 1, counts);
                    IncrementSpace(cube.W - 1, cube.X - 1, cube.Y, cube.Z - 1, counts);
                    IncrementSpace(cube.W - 1, cube.X, cube.Y, cube.Z - 1, counts);
                    IncrementSpace(cube.W - 1, cube.X + 1, cube.Y + 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W - 1, cube.X - 1, cube.Y + 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W - 1, cube.X, cube.Y + 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W - 1, cube.X + 1, cube.Y - 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W - 1, cube.X - 1, cube.Y - 1, cube.Z - 1, counts);
                    IncrementSpace(cube.W - 1, cube.X, cube.Y - 1, cube.Z - 1, counts);
                }
            }
            return counts;
        }

        private void SetActiveCubes(Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, int>>>> counts) {
            var setCube = new List<Cube>();
            foreach (var w in counts.Keys) {
                foreach (var x in counts[w].Keys) {
                    foreach (var y in counts[w][x].Keys) {
                        foreach (var z in counts[w][x][y].Keys) {
                            bool isActive = IsActive(w, x, y, z);
                            if (isActive) {
                                if (counts[w][x][y][z] < 2 || counts[w][x][y][z] > 3) {
                                    setCube.Add(new Cube() {
                                        W = w,
                                        X = x,
                                        Y = y,
                                        Z = z,
                                        IsActive = false
                                    });
                                }
                            } else if (counts[w][x][y][z] == 3) {
                                setCube.Add(new Cube() {
                                    W = w,
                                    X = x,
                                    Y = y,
                                    Z = z,
                                    IsActive = true
                                });
                            }
                        }
                    }
                }
            }
            foreach (var cube in _cubes) {
                if (!counts.ContainsKey(cube.W) || !counts[cube.W].ContainsKey(cube.X) || !counts[cube.W][cube.X].ContainsKey(cube.Y) || !counts[cube.W][cube.X][cube.Y].ContainsKey(cube.Z)) {
                    bool isActive = IsActive(cube.W, cube.X, cube.Y, cube.Z);
                    if (isActive) {
                        setCube.Add(new Cube() {
                            W = cube.W,
                            X = cube.X,
                            Y = cube.Y,
                            Z = cube.Z,
                            IsActive = false
                        });
                    }
                }
            }
            setCube.ForEach(cube => SetIsActive(cube.W, cube.X, cube.Y, cube.Z, cube.IsActive));
        }

        private void GetInitialCubes(List<string> input) {
            _cubes = new List<Cube>();
            _hash = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Cube>>>>();
            for (int x = 0; x < input[0].Length; x++) {
                for (int y = 0; y < input.Count; y++) {
                    if (input[x][y] == '#') {
                        SetIsActive(x, y, 0, 0, true);
                    }
                }
            }
        }

        private void IncrementSpace(int w, int x, int y, int z, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, int>>>> hash) {
            if (!hash.ContainsKey(w)) {
                hash.Add(w, new Dictionary<int, Dictionary<int, Dictionary<int, int>>>());
            }
            if (!hash[w].ContainsKey(x)) {
                hash[w].Add(x, new Dictionary<int, Dictionary<int, int>>());
            }
            if (!hash[w][x].ContainsKey(y)) {
                hash[w][x].Add(y, new Dictionary<int, int>());
            }
            if (!hash[w][x][y].ContainsKey(z)) {
                hash[w][x][y].Add(z, 1);
            } else {
                hash[w][x][y][z]++;
            }
        }

        private void SetIsActive(int w, int x, int y, int z, bool isActive) {
            if (!_hash.ContainsKey(w)) {
                _hash.Add(w, new Dictionary<int, Dictionary<int, Dictionary<int, Cube>>>());
            }
            if (!_hash[w].ContainsKey(x)) {
                _hash[w].Add(x, new Dictionary<int, Dictionary<int, Cube>>());
            }
            if (!_hash[w][x].ContainsKey(y)) {
                _hash[w][x].Add(y, new Dictionary<int, Cube>());
            }
            if (!_hash[w][x][y].ContainsKey(z)) {
                var cube = new Cube() { W = w, X = x, Y = y, Z = z, IsActive = isActive };
                _hash[w][x][y].Add(z, cube);
                _cubes.Add(cube);
            } else {
                _hash[w][x][y][z].IsActive = isActive;
            }
        }

        private bool DoesExist(int w, int x, int y, int z) {
            if (!_hash.ContainsKey(w)) {
                return false;
            }
            if (!_hash[w].ContainsKey(x)) {
                return false;
            }
            if (!_hash[w][x].ContainsKey(y)) {
                return false;
            }
            return _hash[w][x][y].ContainsKey(z);
        }

        private bool IsActive(int w, int x, int y, int z) {
            if (!_hash.ContainsKey(w)) {
                return false;
            }
            if (!_hash[w].ContainsKey(x)) {
                return false;
            }
            if (!_hash[w][x].ContainsKey(y)) {
                return false;
            }
            if (!_hash[w][x][y].ContainsKey(z)) {
                return false;
            }
            return _hash[w][x][y][z].IsActive;
        }

        private List<string> TestInput() {
            return new List<string>() {
                ".#.",
                "..#",
                "###"
            };
        }

        private class Cube {
            public int W { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
