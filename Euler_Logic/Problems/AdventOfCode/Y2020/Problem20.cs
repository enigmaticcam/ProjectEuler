using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem20 : AdventOfCodeBase {
        private PowerAll _powerOf2;
        private Dictionary<ulong, Tile> _tiles;
        private Dictionary<ulong, List<Edge>> _edges;
        private Dictionary<Tuple<int, int>, Image> _images;
        private List<ulong> _corners;

        public override string ProblemName => "Advent of Code 2020: 20";

        public override string GetAnswer() {
            _powerOf2 = new PowerAll(2);
            return Answer2(TestInput()).ToString();
        }

        private ulong Answer1(List<string> input) {
            GetTiles(input);
            PopulateTileBorders();
            BuildEdges();
            FindCorners();
            return GetCornerProduct();
        }

        private ulong Answer2(List<string> input) {
            GetTiles(input);
            PopulateTileBorders();
            BuildEdges();
            FindCorners();
            BuildGrid();
            return 0;
        }

        private void BuildGrid() {
            _images = new Dictionary<Tuple<int, int>, Image>();
            _images.Add(new Tuple<int, int>(0, 0), new Image() {
                HasImage = true,
                ImageTile = _tiles[_corners[0]],
                IsFlippedX = false,
                IsFlippedY = false,
                RotateCount = 0,
                X = 0,
                Y = 0
            });
            BuildGrid(new Tuple<int, int>(0, 0));
        }

        private void BuildGrid(Tuple<int, int> point) {
            var image = _images[point];
            FindEdges(image, image.ImageTile.Borders[0], 0, false);
            FindEdges(image, image.ImageTile.Borders[1], 1, false);
            FindEdges(image, image.ImageTile.Borders[2], 2, false);
            FindEdges(image, image.ImageTile.Borders[3], 3, false);
            FindEdges(image, image.ImageTile.BordersFlipped[0], 0, true);
            FindEdges(image, image.ImageTile.BordersFlipped[1], 1, true);
            FindEdges(image, image.ImageTile.BordersFlipped[2], 2, true);
            FindEdges(image, image.ImageTile.BordersFlipped[3], 3, true);
        }

        private void FindEdges(Image image, ulong border, int borderSide, bool isBorderFlipped) {
            foreach (var edge in _edges[border]) {
                if (image.ImageTile.Id == 1427 && edge.EdgeTile.Id == 1489) {
                    bool stop = true;
                }
                if (edge.EdgeTile.Id != image.ImageTile.Id) {
                    var rotateBy = ((borderSide + 2) % 4) - edge.Side;
                    if (rotateBy < 0) {
                        rotateBy += 4;
                    }
                    rotateBy = (rotateBy + image.RotateCount) % 4;
                    var position = (borderSide + image.RotateCount) % 4;
                    if (image.IsFlippedX && position % 2 == 1) {
                        position = (position + 2) % 4;
                    } else if (image.IsFlippedY && position % 2 == 0) {
                        position = (position + 2) % 4;
                    }
                    var flippedX = false;
                    var flippedY = false;
                    int x = image.X;
                    int y = image.Y;
                    switch (position) {
                        case 0:
                            y++;
                            flippedX = isBorderFlipped == image.IsFlippedX;
                            break;
                        case 1:
                            x++;
                            flippedY = isBorderFlipped == image.IsFlippedY;
                            break;
                        case 2:
                            y--;
                            flippedX = isBorderFlipped == image.IsFlippedX;
                            break;
                        case 3:
                            x--;
                            flippedY = isBorderFlipped == image.IsFlippedY;
                            break;
                    }
                    var key = new Tuple<int, int>(x, y);
                    if (!_images.ContainsKey(key)) {
                        _images.Add(key, new Image() {
                            HasImage = true,
                            ImageTile = edge.EdgeTile,
                            IsFlippedX = flippedX,
                            IsFlippedY = flippedY,
                            RotateCount = rotateBy,
                            X = x,
                            Y = y
                        });
                        BuildGrid(key);
                    }
                }
            }
        }

        private ulong GetCornerProduct() {
            ulong result = 1;
            foreach (var corner in _corners) {
                result *= corner;
            }
            return result;
        }

        private void FindCorners() {
            var hash = new Dictionary<ulong, int>();
            foreach (var edgeSet in _edges.Values) {
                if (edgeSet.Count > 1) {
                    foreach (var edge in edgeSet) {
                        if (!hash.ContainsKey(edge.EdgeTile.Id)) {
                            hash.Add(edge.EdgeTile.Id, 1);
                        } else {
                            hash[edge.EdgeTile.Id]++;
                        }
                    }
                }
            }
            var min = hash.Values.Min();
            _corners = hash.Where(x => x.Value == min).Select(x => x.Key).ToList();
        }

        private void BuildEdges() {
            _edges = new Dictionary<ulong, List<Edge>>();
            foreach (var tile in _tiles.Values) {
                AddEdge(tile.Borders[0], new Edge() { EdgeTile = tile, IsFlipped = false, Side = 0 });
                AddEdge(tile.Borders[1], new Edge() { EdgeTile = tile, IsFlipped = false, Side = 1 });
                AddEdge(tile.Borders[2], new Edge() { EdgeTile = tile, IsFlipped = false, Side = 2 });
                AddEdge(tile.Borders[3], new Edge() { EdgeTile = tile, IsFlipped = false, Side = 3 });
                AddEdge(tile.BordersFlipped[0], new Edge() { EdgeTile = tile, IsFlipped = true, Side = 0 });
                AddEdge(tile.BordersFlipped[1], new Edge() { EdgeTile = tile, IsFlipped = true, Side = 1 });
                AddEdge(tile.BordersFlipped[2], new Edge() { EdgeTile = tile, IsFlipped = true, Side = 2 });
                AddEdge(tile.BordersFlipped[3], new Edge() { EdgeTile = tile, IsFlipped = true, Side = 3 });
            }
        }

        private void AddEdge(ulong key, Edge edge) {
            if (!_edges.ContainsKey(key)) {
                _edges.Add(key, new List<Edge>());
            }
            _edges[key].Add(edge);
        }

        private void PopulateTileBorders() {
            foreach (var tile in _tiles.Values) {
                tile.Borders = new ulong[4];
                tile.BordersFlipped = new ulong[4];
                for (int index = 0; index < 10; index++) {
                    tile.Borders[0] += _powerOf2.GetPower(9 - index) * (tile.Grid[index, 0] ? (ulong)1 : 0);
                    tile.Borders[1] += _powerOf2.GetPower(index) * (tile.Grid[9, index] ? (ulong)1 : 0);
                    tile.Borders[2] += _powerOf2.GetPower(index) * (tile.Grid[index, 9] ? (ulong)1 : 0);
                    tile.Borders[3] += _powerOf2.GetPower(9 - index) * (tile.Grid[0, index] ? (ulong)1 : 0);
                    tile.BordersFlipped[0] += _powerOf2.GetPower(index) * (tile.Grid[index, 0] ? (ulong)1 : 0);
                    tile.BordersFlipped[1] += _powerOf2.GetPower(9 - index) * (tile.Grid[9, index] ? (ulong)1 : 0);
                    tile.BordersFlipped[2] += _powerOf2.GetPower(9 - index) * (tile.Grid[index, 9] ? (ulong)1 : 0);
                    tile.BordersFlipped[3] += _powerOf2.GetPower(index) * (tile.Grid[0, index] ? (ulong)1 : 0);
                }
                bool stop = true;
            }
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

        private class Tile {
            public ulong Id { get; set; }
            public bool[,] Grid { get; set; }
            public ulong[] Borders { get; set; }
            public ulong[] BordersFlipped { get; set; }
        }

        private class Edge {
            public Tile EdgeTile { get; set; }
            public int Side { get; set; }
            public bool IsFlipped { get; set; }
        }

        private class Image {
            public bool HasImage { get; set; }
            public Tile ImageTile { get; set; }
            public bool IsFlippedX { get; set; }
            public bool IsFlippedY { get; set; }
            public int RotateCount { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }

        private List<string> TestInput() {
            return new List<string>() {
                "Tile 2311:",
                "..##.#..#.",
                "##..#.....",
                "#...##..#.",
                "####.#...#",
                "##.##.###.",
                "##...#.###",
                ".#.#.#..##",
                "..#....#..",
                "###...#.#.",
                "..###..###",
                "",
                "Tile 1951:",
                "#.##...##.",
                "#.####...#",
                ".....#..##",
                "#...######",
                ".##.#....#",
                ".###.#####",
                "###.##.##.",
                ".###....#.",
                "..#.#..#.#",
                "#...##.#..",
                "",
                "Tile 1171:",
                "####...##.",
                "#..##.#..#",
                "##.#..#.#.",
                ".###.####.",
                "..###.####",
                ".##....##.",
                ".#...####.",
                "#.##.####.",
                "####..#...",
                ".....##...",
                "",
                "Tile 1427:",
                "###.##.#..",
                ".#..#.##..",
                ".#.##.#..#",
                "#.#.#.##.#",
                "....#...##",
                "...##..##.",
                "...#.#####",
                ".#.####.#.",
                "..#..###.#",
                "..##.#..#.",
                "",
                "Tile 1489:",
                "##.#.#....",
                "..##...#..",
                ".##..##...",
                "..#...#...",
                "#####...#.",
                "#..#.#.#.#",
                "...#.#.#..",
                "##.#...##.",
                "..##.##.##",
                "###.##.#..",
                "",
                "Tile 2473:",
                "#....####.",
                "#..#.##...",
                "#.##..#...",
                "######.#.#",
                ".#...#.#.#",
                ".#########",
                ".###.#..#.",
                "########.#",
                "##...##.#.",
                "..###.#.#.",
                "",
                "Tile 2971:",
                "..#.#....#",
                "#...###...",
                "#.#.###...",
                "##.##..#..",
                ".#####..##",
                ".#..####.#",
                "#..#.#..#.",
                "..####.###",
                "..#.#.###.",
                "...#.#.#.#",
                "",
                "Tile 2729:",
                "...#.#.#.#",
                "####.#....",
                "..#.#.....",
                "....#..#.#",
                ".##..##.#.",
                ".#.####...",
                "####.#.#..",
                "##.####...",
                "##..#.##..",
                "#.##...##.",
                "",
                "Tile 3079:",
                "#.#.#####.",
                ".#..######",
                "..#.......",
                "######....",
                "####.#..#.",
                ".#...#.##.",
                "#.#####.##",
                "..#.###...",
                "..#.......",
                "..#.###..."
            };
        }
    }
}
