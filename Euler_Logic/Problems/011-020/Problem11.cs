﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem11 : IProblem {
        private string[][] _grid;
        private int[][] _gridAsNum;

        public string ProblemName {
            get { return "11: Largest product in a grid"; }
        }

        public string GetAnswer() {
            BuildGrid();
            ConvertGridToInt();
            return FindBestSums().ToString();
        }

        private void BuildGrid() {
            _grid = new string[20][];
            _grid[0] = "08 02 22 97 38 15 00 40 00 75 04 05 07 78 52 12 50 77 91 08".Split(' ');
            _grid[1] = "49 49 99 40 17 81 18 57 60 87 17 40 98 43 69 48 04 56 62 00".Split(' ');
            _grid[2] = "81 49 31 73 55 79 14 29 93 71 40 67 53 88 30 03 49 13 36 65".Split(' ');
            _grid[3] = "52 70 95 23 04 60 11 42 69 24 68 56 01 32 56 71 37 02 36 91".Split(' ');
            _grid[4] = "22 31 16 71 51 67 63 89 41 92 36 54 22 40 40 28 66 33 13 80".Split(' ');
            _grid[5] = "24 47 32 60 99 03 45 02 44 75 33 53 78 36 84 20 35 17 12 50".Split(' ');
            _grid[6] = "32 98 81 28 64 23 67 10 26 38 40 67 59 54 70 66 18 38 64 70".Split(' ');
            _grid[7] = "67 26 20 68 02 62 12 20 95 63 94 39 63 08 40 91 66 49 94 21".Split(' ');
            _grid[8] = "24 55 58 05 66 73 99 26 97 17 78 78 96 83 14 88 34 89 63 72".Split(' ');
            _grid[9] = "21 36 23 09 75 00 76 44 20 45 35 14 00 61 33 97 34 31 33 95".Split(' ');
            _grid[10] = "78 17 53 28 22 75 31 67 15 94 03 80 04 62 16 14 09 53 56 92".Split(' ');
            _grid[11] = "16 39 05 42 96 35 31 47 55 58 88 24 00 17 54 24 36 29 85 57".Split(' ');
            _grid[12] = "86 56 00 48 35 71 89 07 05 44 44 37 44 60 21 58 51 54 17 58".Split(' ');
            _grid[13] = "19 80 81 68 05 94 47 69 28 73 92 13 86 52 17 77 04 89 55 40".Split(' ');
            _grid[14] = "04 52 08 83 97 35 99 16 07 97 57 32 16 26 26 79 33 27 98 66".Split(' ');
            _grid[15] = "88 36 68 87 57 62 20 72 03 46 33 67 46 55 12 32 63 93 53 69".Split(' ');
            _grid[16] = "04 42 16 73 38 25 39 11 24 94 72 18 08 46 29 32 40 62 76 36".Split(' ');
            _grid[17] = "20 69 36 41 72 30 23 88 34 62 99 69 82 67 59 85 74 04 36 16".Split(' ');
            _grid[18] = "20 73 35 29 78 31 90 01 74 31 49 71 48 86 81 16 23 57 05 54".Split(' ');
            _grid[19] = "01 70 54 71 83 51 54 69 16 92 33 48 61 43 52 01 89 19 67 48".Split(' ');
        }

        private void ConvertGridToInt() {
            _gridAsNum = new int[20][];
            for (int x = 0; x < 20; x++) {
                _gridAsNum[x] = new int[20];
                for (int y = 0; y < 20; y++) {
                    _gridAsNum[x][y] = Convert.ToInt32(_grid[x][y]);
                }
            }
        }

        private int FindBestSums() {
            int best = 0;
            for (int x = 0; x < 20; x++) {
                for (int y = 0; y < 20; y++) {
                    int horizontal = GetHorizontal(x, y);
                    if (horizontal > best) {
                        best = horizontal;
                    }

                    int vertical = GetVertical(x, y);
                    if (vertical > best) {
                        best = vertical;
                    }
                    int rightDiagonal = GetRightDiagonal(x, y);
                    if (rightDiagonal > best) {
                        best = rightDiagonal;
                    }

                    int leftDiagonal = GetLeftDiagonal(x, y);
                    if (leftDiagonal > best) {
                        best = leftDiagonal;
                    }
                }
            }
            return best;
        }

        private int GetHorizontal(int x, int y) {
            if (y < 17) {
                return _gridAsNum[x][y] * _gridAsNum[x][y + 1] * _gridAsNum[x][y + 2] * _gridAsNum[x][y + 3];
            } else {
                return 0;
            }
        }

        private int GetVertical(int x, int y) {
            if (x < 17) {
                return _gridAsNum[x][y] * _gridAsNum[x + 1][y] * _gridAsNum[x + 2][y] * _gridAsNum[x + 3][y];
            } else {
                return 0;
            }   
        }

        private int GetRightDiagonal(int x, int y) {
            if (x < 17 && y < 17) {
                return _gridAsNum[x][y] * _gridAsNum[x + 1][y + 1] * _gridAsNum[x + 2][y + 2] * _gridAsNum[x + 3][y + 3];
            } else {
                return 0;
            }
        }

        private int GetLeftDiagonal(int x, int y) {
            if (x > 3 && y < 17) {
                return _gridAsNum[x][y] * _gridAsNum[x - 1][y + 1] * _gridAsNum[x - 2][y + 2] * _gridAsNum[x - 3][y + 3];
            } else {
                return 0;
            }
        }

        
    }
}