using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem03 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2020: 3";

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        private ulong Answer1() {
            return GetTreeCount(Input(), new Tuple<int, int>(3, 1));
        }

        private ulong Answer2() {
            var map = Input();
            ulong total = 1;
            total *= GetTreeCount(map, new Tuple<int, int>(1, 1));
            total *= GetTreeCount(map, new Tuple<int, int>(3, 1));
            total *= GetTreeCount(map, new Tuple<int, int>(5, 1));
            total *= GetTreeCount(map, new Tuple<int, int>(7, 1));
            total *= GetTreeCount(map, new Tuple<int, int>(1, 2));
            return total;
        }

        private ulong GetTreeCount(List<string> map, Tuple<int, int> slope) {
            int x = slope.Item1;
            int y = slope.Item2;
            int length = map[0].Length;
            ulong treeCount = 0;
            do {
                if (map[y][x] == '#') {
                    treeCount++;
                }
                x = (x + slope.Item1) % length;
                y += slope.Item2;
            } while (y < map.Count);
            return treeCount;
        }

        private List<string> TestInput() {
            return new List<string>() {
                "..##.......",
                "#...#...#..",
                ".#....#..#.",
                "..#.#...#.#",
                ".#...##..#.",
                "..#.##.....",
                ".#.#.#....#",
                ".#........#",
                "#.##...#...",
                "#...##....#",
                ".#..#...#.#"
            };
        }
    }
}
