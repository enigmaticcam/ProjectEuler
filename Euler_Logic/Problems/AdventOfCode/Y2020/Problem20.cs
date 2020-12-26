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
        private Dictionary<int, Dictionary<int, Image>> _images;
        private Dictionary<Tuple<int, int>, Tile> _grid;
        private List<ulong> _corners;

        public override string ProblemName => "Advent of Code 2020: 20";

        public override string GetAnswer() {
            _powerOf2 = new PowerAll(2);
            _images = new Dictionary<int, Dictionary<int, Image>>();
            return Answer2(TestInput()).ToString();
        }

        private ulong Answer1(List<string> input) {
            GetTiles(input);
            PopulateTileBorders();
            BuildEdges();
            SetFirstImage();
            FindCorners();
            return GetCornerProduct();
        }

        private ulong Answer2(List<string> input) {
            GetTiles(input);
            PopulateTileBorders();
            BuildEdges();
            SetFirstImage();
            FindCorners();
            BuildGrid();
            return 0;
        }

        private void BuildGrid() {
            _grid = new Dictionary<Tuple<int, int>, Tile>();
            _grid.Add(new Tuple<int, int>(0, 0), _tiles[_corners[0]]);
            BuildGrid(new Tuple<int, int>(0, 0));
        }

        private void BuildGrid(Tuple<int, int> point) {
            var image = _grid[point];
            foreach (var edge in _edges[image.Borders[0]]) {
                if (edge.EdgeTile.Id != image.Id) {
                    foreach (var border in edge.EdgeTile.Borders) {
                        if (border == image.Borders[0]) {
                            bool stop = true;
                        }
                    }
                }
            }
        }

        private void FindMatching(Tile sourceTile, ulong border) {
            
        }

        private ulong GetCornerProduct() {
            ulong result = 1;
            foreach (var corner in _corners) {
                result *= corner;
            }
            return result;
        }

        private void SetFirstImage() {
            var image = new Image() {
                ImageTile = _tiles.Values.ElementAt(0),
                IsFlipped = false,
                RotateCount = 0,
                X = 0,
                Y = 0
            };
            AddImage(image);
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

        private bool DoesImageExist(int x, int y) {
            if (!_images.ContainsKey(x)) {
                return false;
            }
            return _images[x].ContainsKey(y);
        }

        private void AddImage(Image image) {
            if (!_images.ContainsKey(image.X)) {
                _images.Add(image.X, new Dictionary<int, Image>());
            }
            _images[image.X].Add(image.Y, image);
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
                    tile.Borders[1] += _powerOf2.GetPower(9 - index) * (tile.Grid[9, index] ? (ulong)1 : 0);
                    tile.Borders[2] += _powerOf2.GetPower(9 - index) * (tile.Grid[index, 9] ? (ulong)1 : 0);
                    tile.Borders[3] += _powerOf2.GetPower(9 - index) * (tile.Grid[0, index] ? (ulong)1 : 0);
                    tile.BordersFlipped[0] += _powerOf2.GetPower(index) * (tile.Grid[index, 0] ? (ulong)1 : 0);
                    tile.BordersFlipped[1] += _powerOf2.GetPower(index) * (tile.Grid[9, index] ? (ulong)1 : 0);
                    tile.BordersFlipped[2] += _powerOf2.GetPower(index) * (tile.Grid[index, 9] ? (ulong)1 : 0);
                    tile.BordersFlipped[3] += _powerOf2.GetPower(index) * (tile.Grid[0, index] ? (ulong)1 : 0);
                }
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
            public bool IsFlipped { get; set; }
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
