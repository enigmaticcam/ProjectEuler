using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem20 : AdventOfCodeBase {
        private Dictionary<ulong, Tile> _tiles;
        private Dictionary<Tuple<int, int>, Tile> _grid;
        private HashSet<ulong> _completed;

        public override string ProblemName => "Advent of Code 2020: 20";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            _grid = new Dictionary<Tuple<int, int>, Tile>();
            _completed = new HashSet<ulong>();
            GetTiles(input);
            SetAllOrientations();
            var start = _tiles.First().Value;
            _grid.Add(new Tuple<int, int>(0, 0), start);
            _completed.Add(start.Id);
            BuildGrid(start, 0, 0);
            return GetCorners();
        }

        private int Answer2(List<string> input) {
            _grid = new Dictionary<Tuple<int, int>, Tile>();
            _completed = new HashSet<ulong>();
            GetTiles(input);
            SetAllOrientations();
            var start = _tiles.First().Value;
            _grid.Add(new Tuple<int, int>(0, 0), start);
            _completed.Add(start.Id);
            BuildGrid(start, 0, 0);
            return FindMonsters();
        }

        private int FindMonsters() {
            int xMin = _grid.Keys.Select(x => x.Item1).Min();
            int xMax = _grid.Keys.Select(x => x.Item1).Max();
            int yMin = _grid.Keys.Select(x => x.Item2).Min();
            int yMax = _grid.Keys.Select(x => x.Item2).Max();
            var newSize = (_tiles.First().Value.Grid.GetUpperBound(0) - 1) * (xMax - xMin + 1);
            var final = new bool[newSize, newSize];
            for (int x = xMin; x <= xMax; x++) {
                int finalX = (x - xMin) * 8;
                for (int y = yMin; y <= yMax; y++) {
                    int finalY = (y - yMin) * 8;
                    AddToFinal(_grid[new Tuple<int, int>(x, y)], finalX, finalY, final);
                }
            }
            var count = CountMonster(final);
            if (count == 0) {
                final = Rotate(final);
                count = CountMonster(final);
            }
            if (count == 0) {
                final = Rotate(final);
                count = CountMonster(final);
            }
            if (count == 0) {
                final = Rotate(final);
                count = CountMonster(final);
            }
            if (count == 0) {
                final = Flip(final);
                count = CountMonster(final);
            }
            if (count == 0) {
                final = Rotate(final);
                count = CountMonster(final);
            }
            if (count == 0) {
                final = Rotate(final);
                count = CountMonster(final);
            }
            if (count == 0) {
                final = Rotate(final);
                count = CountMonster(final);
            }
            return CountAll(final) - (count * 15);
        }

        private int CountAll(bool[,] final) {
            int count = 0;
            for (int x = 0; x <= final.GetUpperBound(0); x++) {
                for (int y = 0; y <= final.GetUpperBound(0); y++) {
                    if (final[x, y]) count++;
                }
            }
            return count;
        }

        private int CountMonster(bool[,] final) {
            int count = 0;
            for (int x = 0; x <= final.GetUpperBound(1) - 20; x++) {
                for (int y = 0; y <= final.GetUpperBound(1) - 2; y++) {
                    if (IsMonster(x, y, final)) count++;
                }
            }
            return count;
        }

        private bool IsMonster(int x, int y, bool[,] final) {
            return final[y, x + 18]
                && final[y + 1, x + 0]
                && final[y + 1, x + 5]
                && final[y + 1, x + 6]
                && final[y + 1, x + 11]
                && final[y + 1, x + 12]
                && final[y + 1, x + 17]
                && final[y + 1, x + 18]
                && final[y + 1, x + 19]
                && final[y + 2, x + 1]
                && final[y + 2, x + 4]
                && final[y + 2, x + 7]
                && final[y + 2, x + 10]
                && final[y + 2, x + 13]
                && final[y + 2, x + 16];

        }

        private void AddToFinal(Tile toAdd, int x, int y, bool[,] final) {
            for (int countX = 0; countX < 8; countX++) {
                for (int countY = 0; countY < 8; countY++) {
                    final[x + countX, y + countY] = toAdd.Grid[countX + 1, countY + 1];
                }
            }
        }

        private void BuildGrid(Tile tile, int x, int y) {
            var tilesAdded = new List<Tile>();
            var emptySide = new Tuple<int, int>(x + 1, y);
            if (!_grid.ContainsKey(emptySide)) {
                foreach (var next in _tiles.Values) {
                    if (!_completed.Contains(next.Id)) {
                        foreach (var orientation in next.AllOrientations) {
                            if (DoesSideMatchOnY(orientation, tile.Grid)) {
                                next.Grid = orientation;
                                next.X = emptySide.Item1;
                                next.Y = emptySide.Item2;
                                _grid.Add(emptySide, next);
                                tilesAdded.Add(next);
                                _completed.Add(next.Id);
                                break;
                            }
                        }
                    }
                }
            }

            emptySide = new Tuple<int, int>(x - 1, y);
            if (!_grid.ContainsKey(emptySide)) {
                foreach (var next in _tiles.Values) {
                    if (!_completed.Contains(next.Id)) {
                        foreach (var orientation in next.AllOrientations) {
                            if (DoesSideMatchOnY(tile.Grid, orientation)) {
                                next.Grid = orientation;
                                next.X = emptySide.Item1;
                                next.Y = emptySide.Item2;
                                _grid.Add(emptySide, next);
                                tilesAdded.Add(next);
                                _completed.Add(next.Id);
                                break;
                            }
                        }
                    }
                }
            }

            emptySide = new Tuple<int, int>(x, y + 1);
            if (!_grid.ContainsKey(emptySide)) {
                foreach (var next in _tiles.Values) {
                    if (!_completed.Contains(next.Id)) {
                        foreach (var orientation in next.AllOrientations) {
                            if (DoesSideMatchOnX(orientation, tile.Grid)) {
                                next.Grid = orientation;
                                next.X = emptySide.Item1;
                                next.Y = emptySide.Item2;
                                _grid.Add(emptySide, next);
                                tilesAdded.Add(next);
                                _completed.Add(next.Id);
                                break;
                            }
                        }
                    }
                }
            }

            emptySide = new Tuple<int, int>(x, y - 1);
            if (!_grid.ContainsKey(emptySide)) {
                foreach (var next in _tiles.Values) {
                    if (!_completed.Contains(next.Id)) {
                        foreach (var orientation in next.AllOrientations) {
                            if (DoesSideMatchOnX(tile.Grid, orientation)) {
                                next.Grid = orientation;
                                next.X = emptySide.Item1;
                                next.Y = emptySide.Item2;
                                _grid.Add(emptySide, next);
                                tilesAdded.Add(next);
                                _completed.Add(next.Id);
                                break;
                            }
                        }
                    }
                }
            }

            foreach (var next in tilesAdded) {
                BuildGrid(next, next.X, next.Y);
            }
        }

        private bool DoesSideMatchOnX(bool[,] tile1, bool[,] tile2) {
            for (int x = 0; x < 10; x++) {
                if (tile1[x, 0] != tile2[x, 9]) return false;
            }
            return true;
        }

        private bool DoesSideMatchOnY(bool[,] tile1, bool[,] tile2) {
            for (int y = 0; y < 10; y++) {
                if (tile1[0, y] != tile2[9, y]) return false;
            }
            return true;
        }

        private void SetAllOrientations() {
            foreach (var tile in _tiles.Values) {
                var grid = tile.Grid;
                tile.AllOrientations = new List<bool[,]>();
                tile.AllOrientations.Add(grid);
                grid = Rotate(grid);
                tile.AllOrientations.Add(grid);
                grid = Rotate(grid);
                tile.AllOrientations.Add(grid);
                grid = Rotate(grid);
                tile.AllOrientations.Add(grid);
                grid = Flip(grid);
                tile.AllOrientations.Add(grid);
                grid = Rotate(grid);
                tile.AllOrientations.Add(grid);
                grid = Rotate(grid);
                tile.AllOrientations.Add(grid);
                grid = Rotate(grid);
                tile.AllOrientations.Add(grid);
            }
        }

        private bool[,] Flip(bool[,] old) {
            int size = old.GetUpperBound(0) + 1;
            var grid = new bool[size, size];
            for (int x = 0; x < size / 2; x++) {
                for (int y = 0; y < size; y++) {
                    grid[x, y] = old[size - 1 - x, y];
                    grid[size - 1 - x, y] = old[x, y];
                }
            }
            return grid;
        }

        private bool[,] Rotate(bool[,] old) {
            int size = old.GetUpperBound(0) + 1;
            var grid = new bool[size, size];
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    grid[x, y] = old[y, size - 1 - x];
                }
            }
            return grid;
        }

        private void GetTiles(List<string> input) {
            var tiles = new List<Tile>();
            int index = 0;
            do {
                var tile = new Tile() { Grid = new bool[10, 10] };
                tile.Id = Convert.ToUInt64(input[index].Substring(5).Replace(":", ""));
                index++;
                for (int x = 0; x <= 9; x++) {
                    for (int y = 0; y <= 9; y++) {
                        tile.Grid[x, y] = input[index + y][x] == '#';
                    }
                }
                tiles.Add(tile);
                index += 11;
            } while (index < input.Count - 1);
            _tiles = tiles.ToDictionary(tile => tile.Id, tile => tile);
        }

        private ulong GetCorners() {
            int xMin = _grid.Keys.Select(x => x.Item1).Min();
            int xMax = _grid.Keys.Select(x => x.Item1).Max();
            int yMin = _grid.Keys.Select(x => x.Item2).Min();
            int yMax = _grid.Keys.Select(x => x.Item2).Max();
            ulong total = _grid[new Tuple<int, int>(xMin, yMin)].Id;
            total *= _grid[new Tuple<int, int>(xMin, yMax)].Id;
            total *= _grid[new Tuple<int, int>(xMax, yMin)].Id;
            total *= _grid[new Tuple<int, int>(xMax, yMax)].Id;
            return total;
        }

        private class Tile {
            public ulong Id { get; set; }
            public bool[,] Grid { get; set; }
            public List<bool[,]> AllOrientations { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
