using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem10 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 10"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        public int Answer1(List<string> input) {
            var grid = GetGrid(input);
            int best = 0;
            foreach (var asteroid in grid.AsteroidsList) {
                int next = CountVisible(grid, asteroid);
                if (next > best) {
                    best = next;
                }
            }
            return best;
        }

        private int Answer2(List<string> input) {
            var grid = GetGrid(input);
            int bestScore = 0;
            Asteroid bestSpot = null;
            foreach (var asteroid in grid.AsteroidsList) {
                int next = CountVisible(grid, asteroid);
                if (next > bestScore) {
                    bestScore = next;
                    bestSpot = asteroid;
                }
            }
            return UseLaser(grid, bestSpot);
        }

        private int UseLaser(Grid grid, Asteroid spot) {
            CalcDistances(grid.AsteroidsList, spot);
            spot.Vaporized = true;
            var ordered = grid.AsteroidsList.OrderBy(x => x.Quadrant).ThenByDescending(x => x.Ratio).ThenBy(x => x.Distance).ToList();
            int index = 0;
            var lastVaporized = ordered[index];
            ordered[index].Vaporized = true;
            int count = 1;
            do {
                index = (index + 1) % ordered.Count;
                var next = ordered[index];
                if (!next.Vaporized && (next.Quadrant != lastVaporized.Quadrant || next.Ratio != lastVaporized.Ratio)) {
                    next.Vaporized = true;
                    lastVaporized = next;
                    count++;
                    if (count == 200) {
                        return next.X * 100 + next.Y;
                    }
                }
                
            } while (true);
        }

        private int _ratioX;
        private int _ratioY;
        private int CountVisible(Grid grid, Asteroid spot) {
            CalcDistances(grid.AsteroidsList, spot);
            spot.Removed = true;
            var ordered = grid.AsteroidsList.OrderBy(x => x.Distance);
            foreach (var next in ordered) {
                if (!next.Removed) {
                    GetRatio(spot, next);
                    int x = next.X;
                    int y = next.Y;
                    do {
                        x += _ratioX;
                        y += _ratioY;
                        if (x > grid.MaxX || y > grid.MaxY || x < 0 || y < 0) {
                            break;
                        } else if (grid.AsteroidsLocation.ContainsKey(x) && grid.AsteroidsLocation[x].ContainsKey(y)) {
                            grid.AsteroidsLocation[x][y].Removed = true;
                        }
                    } while (true);
                }
            }
            return grid.AsteroidsList.Where(x => !x.Removed).Count();
        }

        private void GetRatio(Asteroid spot, Asteroid next) {
            _ratioX = next.X - spot.X;
            _ratioY = next.Y - spot.Y;
            int signX = (_ratioX < 0 ? -1 : 1);
            int signY = (_ratioY < 0 ? -1 : 1);
            if (_ratioX == 0) {
                _ratioY = 1;
            } else if (_ratioY == 0) {
                _ratioX = 1;
            } else {
                _ratioX = Math.Abs(_ratioX);
                _ratioY = Math.Abs(_ratioY);
                var gcd = GCD.GetGCD(_ratioX, _ratioY);
                _ratioX /= gcd;
                _ratioY /= gcd;
            }
            _ratioX *= signX;
            _ratioY *= signY;
        }

        private void CalcDistances(List<Asteroid> asteroids, Asteroid spot) {
            foreach (var asteroid in asteroids) {
                asteroid.Distance = Math.Abs(asteroid.X - spot.X) + Math.Abs(asteroid.Y - spot.Y);
                asteroid.Removed = false;
                asteroid.Ratio = (double)(asteroid.X - spot.X) / (double)(asteroid.Y - spot.Y);
                if (asteroid.X == spot.X && asteroid.Y < spot.Y) {
                    asteroid.Quadrant = 1;
                } else if (asteroid.X > spot.X && asteroid.Y < spot.Y) {
                    asteroid.Quadrant = 2;
                } else if (asteroid.X > spot.X && asteroid.Y == spot.Y) {
                    asteroid.Quadrant = 3;
                } else if (asteroid.X > spot.X && asteroid.Y > spot.Y) {
                    asteroid.Quadrant = 4;
                } else if (asteroid.X == spot.X && asteroid.Y > spot.Y) {
                    asteroid.Quadrant = 5;
                } else if (asteroid.X < spot.X && asteroid.Y > spot.Y) {
                    asteroid.Quadrant = 6;
                } else if (asteroid.X < spot.X && asteroid.Y == spot.Y) {
                    asteroid.Quadrant = 7;
                } else {
                    asteroid.Quadrant = 8;
                }
            }
        }

        private Grid GetGrid(List<string> input) {
            var grid = new Grid();
            int x = 0;
            int y = 0;
            foreach (var line in input) {
                x = 0;
                foreach (var spot in line) {
                    if (spot == '#') {
                        var next = new Asteroid(x, y);
                        grid.AsteroidsList.Add(next);
                        if (!grid.AsteroidsLocation.ContainsKey(x)) {
                            grid.AsteroidsLocation.Add(x, new Dictionary<int, Asteroid>());
                        }
                        grid.AsteroidsLocation[x].Add(y, next);
                    }
                    x++;
                }
                y++;
            }
            grid.MaxX = x;
            grid.MaxY = y;
            return grid;
        }

        private string Output(Grid grid) {
            var text = new StringBuilder();
            for (int y = 0; y <= grid.MaxY; y++) {
                var line = new char[grid.MaxX + 1];
                for (int x = 0; x <= grid.MaxX; x++) {
                    if (grid.AsteroidsLocation.ContainsKey(x) && grid.AsteroidsLocation[x].ContainsKey(y) && !grid.AsteroidsLocation[x][y].Removed) {
                        line[x] = '#';
                    } else {
                        line[x] = '.';
                    }
                }
                text.AppendLine(new string(line));
            }
            return text.ToString();
        }

        private List<string> Test1Input() {
            return new List<string>() {
                ".#..#",
                ".....",
                "#####",
                "....#",
                "...##"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "......#.#.",
                "#..#.#....",
                "..#######.",
                ".#.#.###..",
                ".#..#.....",
                "..#....#.#",
                "#..#....#.",
                ".##.#..###",
                "##...#..#.",
                ".#....####"
            };
        }

        private List<string> Test3Input() {
            return new List<string>() {
                "#.#...#.#.",
                ".###....#.",
                ".#....#...",
                "##.#.#.#.#",
                "....#.#.#.",
                ".##..###.#",
                "..#...##..",
                "..##....##",
                "......#...",
                ".####.###."
            };
        }

        private List<string> Test4Input() {
            return new List<string>() {
                ".#..#..###",
                "####.###.#",
                "....###.#.",
                "..###.##.#",
                "##.##.#.#.",
                "....###..#",
                "..#.#..#.#",
                "#..#.#.###",
                ".##...##.#",
                ".....#.#.."
            };
        }

        private List<string> Test5Input() {
            return new List<string>() {
                ".#..##.###...#######",
                "##.############..##.",
                ".#.######.########.#",
                ".###.#######.####.#.",
                "#####.##.#.##.###.##",
                "..#####..#.#########",
                "####################",
                "#.####....###.#.#.##",
                "##.#################",
                "#####.##.###..####..",
                "..######..##.#######",
                "####.##.####...##..#",
                ".#####..#.######.###",
                "##...#.##########...",
                "#.##########.#######",
                ".####.#.###.###.#.##",
                "....##.##.###..#####",
                ".#.#.###########.###",
                "#.#.#.#####.####.###",
                "###.##.####.##.#..##"
            };
        }

        private List<string> Test6Input() {
            return new List<string>() {
                ".#....#####...#..",
                "##...##.#####..##",
                "##...#...#.#####.",
                "..#.....#...###..",
                "..#.#.....#....##"
            };
        }

        private class Asteroid {
            public Asteroid() { }
            public Asteroid(int x, int y) {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }
            public int Distance { get; set; }
            public bool Removed { get; set; }
            public double Ratio { get; set; }
            public int Quadrant { get; set; }
            public bool Vaporized { get; set; }
        }

        private class Grid {
            public Grid() {
                AsteroidsList = new List<Asteroid>();
                AsteroidsLocation = new Dictionary<int, Dictionary<int, Asteroid>>();
            }
            public List<Asteroid> AsteroidsList { get; set; }
            public Dictionary<int, Dictionary<int, Asteroid>> AsteroidsLocation { get; set; }
            public int MaxX { get; set; }
            public int MaxY { get; set; }
        }
    }
}
