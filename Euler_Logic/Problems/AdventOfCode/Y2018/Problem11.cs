using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem11 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 11"; }
        }

        public override string GetAnswer() {
            return Answer1();
        }

        public override string GetAnswer2() {
            return Answer2();
        }

        private string Answer1() {
            var grid = GetGrid(9110);
            var highest = FindHighest(grid);
            return highest.Item1 + "," + highest.Item2;
        }

        private string Answer2() {
            var grid = GetGrid(9110);
            var highest = FindHighestVaried(grid);
            return highest.Item1 + "," + highest.Item2 + "," + highest.Item3;
        }

        private Tuple<int, int> FindHighest(int[,] grid) {
            int highest = 0;
            int bestX = 0;
            int bestY = 0;
            for (int x = 0; x <= 297; x++) {
                for (int y = 0; y <= 297; y++) {
                    int sum = 0;
                    for (int xOffset = 0; xOffset <= 2; xOffset++) {
                        for (int yOffset = 0; yOffset <= 2; yOffset++) {
                            sum += grid[x + xOffset, y + yOffset];
                        }
                    }
                    if (sum > highest) {
                        highest = sum;
                        bestX = x + 1;
                        bestY = y + 1;
                    }
                }
            }
            return new Tuple<int, int>(bestX, bestY);
        }

        private Tuple<int, int, int> FindHighestVaried(int[,] grid) {
            int size = grid.GetUpperBound(0) + 1;
            var hash = new List<int[,]>();
            hash.Add(grid);
            hash.Add(new int[size - 1, size - 1]);
            int bestValue = 0;
            int bestX = 0;
            int bestY = 0;
            int bestL = 0;

            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    if (hash[0][x, y] > bestValue) {
                        bestValue = hash[0][x, y];
                        bestX = x + 1;
                        bestY = y + 1;
                        bestL = 1;
                    }
                }
            }

            for (int x = 0; x < size - 1; x++) {
                for (int y = 0; y < size - 1; y++) {
                    hash[1][x, y] = hash[0][x, y] + hash[0][x + 1, y] + hash[0][x, y + 1] + hash[0][x + 1, y + 1];
                    if (hash[1][x, y] > bestValue) {
                        bestValue = hash[1][x, y];
                        bestX = x + 1;
                        bestY = y + 1;
                        bestL = 1;
                    }
                }
            }

            for (int length = 3; length <= size; length++) {
                var next = new int[size - length + 1, size - length + 1];
                for (int x = 0; x <= size - length; x++) {
                    for (int y = 0; y <= size - length; y++) {
                        next[x, y] = hash[1][x, y] + hash[1][x + 1, y + 1] - hash[0][x + 1, y + 1] + grid[x + length - 1, y] + grid[x, y + length - 1];
                        if (next[x, y] > bestValue) {
                            bestValue = next[x, y];
                            bestX = x + 1;
                            bestY = y + 1;
                            bestL = length;
                        }
                    }
                }
                hash[0] = hash[1];
                hash[1] = next;
            }
            return new Tuple<int, int, int>(bestX, bestY, bestL);
        }

        private int[,] GetGrid(int serialNumber) {
            var grid = new int[300, 300];
            for (int x = 1; x <= 300; x++) {
                for (int y = 1; y <= 300; y++) {
                    grid[x - 1, y - 1] = GetPower(x, y, serialNumber);
                }
            }
            return grid;
        }

        private int GetPower(int x, int y, int serialNumber) {
            var power = x + 10;
            power *= y;
            power += serialNumber;
            power *= x + 10;
            power = (power / 100) % 10;
            power -= 5;
            return power;
        }
    }
}
