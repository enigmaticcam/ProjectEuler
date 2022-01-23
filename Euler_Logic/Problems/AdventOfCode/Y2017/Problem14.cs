using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem14 : AdventOfCodeBase {
        private Dictionary<char, int> _bitCounts;

        public override string ProblemName {
            get { return "Advent of Code 2017: 14"; }
        }

        public override string GetAnswer() {
            return Answer1("xlqgujun").ToString();
        }

        public override string GetAnswer2() {
            return Answer2("xlqgujun").ToString();
        }

        private int Answer1(string input) {
            SetBits();
            return CountAll(input);
        }

        private int Answer2(string input) {
            SetBits();
            return CountRegions(input);
        }

        private int CountAll(string input) {
            int sum = 0;
            for (int count = 0; count < 128; count++) {
                var key = input + "-" + count;
                sum += CountUsedSquares(key);
            }
            return sum;
        }

        private int CountRegions(string input) {
            var grid = GetGrid(input);
            var region = new int[128, 128];
            int regionId = 0;
            for (int y = 0; y < 128; y++) {
                for (int x = 0; x < 128; x++) {
                    if (grid[x, y] && region[x, y] == 0) {
                        regionId++;
                        SetRegion(grid, region, x, y, regionId);
                    }
                }
            }
            return regionId;
        }

        private void SetRegion(bool[,] grid, int[,] region, int x, int y, int regionId) {
            region[x, y] = regionId;
            if (x > 0 && grid[x - 1, y] && region[x - 1, y] == 0) {
                SetRegion(grid, region, x - 1, y, regionId);
            }
            if (x < 127 && grid[x + 1, y] && region[x + 1, y] == 0) {
                SetRegion(grid, region, x + 1, y, regionId);
            }
            if (y > 0 && grid[x, y - 1] && region[x, y - 1] == 0) {
                SetRegion(grid, region, x, y - 1, regionId);
            }
            if (y < 127 && grid[x, y + 1] && region[x, y + 1] == 0) {
                SetRegion(grid, region, x, y + 1, regionId);
            }
        }

        private bool[,] GetGrid(string input) {
            var grid = new bool[128, 128];
            for (int count = 0; count < 128; count++) {
                var key = input + "-" + count;
                var bits = GetSquareBits(key);
                int x = 0;
                foreach (var num in bits) {
                    for (int power = 128; power >= 1; power /= 2) {
                        grid[x, count] = (power & num) == power;
                        x++;
                    }
                }
            }
            return grid;
        }

        private int CountUsedSquares(string key) {
            var nums = GetSquareBits(key);
            int sum = 0;
            foreach (var hash in nums) {
                string next = hash.ToString("X");
                if (next.Length == 2) {
                    sum += _bitCounts[next[0]];
                }
                sum += _bitCounts[next.Last()];
            }
            return sum;
        }

        private List<int> GetSquareBits(string input) {
            int totalLength = 256;
            int position = 0;
            int skipSize = 0;
            int[] numbers = Initialize(totalLength);

            // Get lengths
            var lengths = new List<int>();
            for (int character = 0; character < input.Length; character++) {
                lengths.Add(Encoding.ASCII.GetBytes(input.Substring(character, 1)).First());
            }
            lengths.AddRange(new List<int>() { 17, 31, 73, 47, 23 });

            // Perform rounds
            for (int round = 1; round <= 64; round++) {
                foreach (int length in lengths) {
                    for (int index = 0; index < length / 2; index++) {
                        int swapAPosition = (position + index) % totalLength;
                        int swapBPosition = (position + length - index - 1) % totalLength;
                        int temp = numbers[swapAPosition];
                        numbers[swapAPosition] = numbers[swapBPosition];
                        numbers[swapBPosition] = temp;
                    }
                    position = (position + length + skipSize) % totalLength;
                    skipSize++;
                }
            }

            // Make dense hash
            var dense = new List<int>();
            for (int denseIndex = 0; denseIndex < 16; denseIndex++) {
                int number = 0;
                for (int step = 0; step < 16; step++) {
                    number ^= numbers[(denseIndex * 16) + step];
                }
                dense.Add(number);
            }
            return dense;
        }

        private int[] Initialize(int length) {
            int[] numbers = new int[length];
            for (int number = 0; number < length; number++) {
                numbers[number] = number;
            }
            return numbers;
        }

        private void SetBits() {
            _bitCounts = new Dictionary<char, int>();
            _bitCounts.Add('0', 0);
            _bitCounts.Add('1', 1);
            _bitCounts.Add('2', 1);
            _bitCounts.Add('3', 2);
            _bitCounts.Add('4', 1);
            _bitCounts.Add('5', 2);
            _bitCounts.Add('6', 2);
            _bitCounts.Add('7', 3);
            _bitCounts.Add('8', 1);
            _bitCounts.Add('9', 2);
            _bitCounts.Add('A', 2);
            _bitCounts.Add('B', 3);
            _bitCounts.Add('C', 2);
            _bitCounts.Add('D', 3);
            _bitCounts.Add('E', 3);
            _bitCounts.Add('F', 4);
        }
    }
}
