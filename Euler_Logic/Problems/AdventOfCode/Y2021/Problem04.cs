using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem04 : AdventOfCodeBase {
        private List<int> _randoms;
        private List<int[,]> _boards;
        private List<bool[,]> _finals;
        private Dictionary<int, List<Func<int>>> _hash;

        public override string ProblemName {
            get { return "Advent of Code 2021: 04"; }
        }

        public override string GetAnswer() {
            GetRandomsAndBoards(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            InitializeFinals();
            BuildHash();
            FindWinningBoard();
            return Score();
        }

        private int Answer2() {
            InitializeFinals();
            BuildHash();
            FindLastBoard();
            return Score();
        }

        private void FindWinningBoard() {
            bool stop = false;
            foreach (var random in _randoms) {
                _lastRandom = random;
                var changed = new List<int>();
                _hash[random].ForEach(x => {
                    changed.Add(x());
                });
                foreach (var x in changed) {
                    stop = CheckBoard(x);
                    if (stop) {
                        break;
                    }
                }
                if (stop) {
                    break;
                }
            }
        }

        private void FindLastBoard() {
            var winners = new HashSet<int>();
            bool stop = false;
            foreach (var random in _randoms) {
                _lastRandom = random;
                var changed = new List<int>();
                _hash[random].ForEach(x => {
                    changed.Add(x());
                });
                foreach (var x in changed) {
                    if (!winners.Contains(x)) {
                        var result = CheckBoard(x);
                        if (result) {
                            winners.Add(x);
                            if (winners.Count == _boards.Count) {
                                stop = true;
                                break;
                            }
                        }
                    }
                }
                if (stop) {
                    break;
                }
            }
        }

        private int _winner;
        private int _lastRandom;
        private bool CheckBoard(int index) {
            var final = _finals[index];
            var counts = new int[10];
            for (int count = 0; count <= 4; count++) {
                if (final[0, count]) counts[0]++;
                if (final[1, count]) counts[1]++;
                if (final[2, count]) counts[2]++;
                if (final[3, count]) counts[3]++;
                if (final[4, count]) counts[4]++;
                if (final[count, 0]) counts[5]++;
                if (final[count, 1]) counts[6]++;
                if (final[count, 2]) counts[7]++;
                if (final[count, 3]) counts[8]++;
                if (final[count, 4]) counts[9]++;
            }
            foreach (var count in counts) {
                if (count == 5) {
                    _winner = index;
                    return true;
                }
            }
            return false;
        }

        private int Score() {
            var board = _boards[_winner];
            var final = _finals[_winner];
            int sum = 0;
            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 5; y++) {
                    if (!final[x, y]) {
                        sum += board[x, y]; 
                    }
                }
            }
            return sum * _lastRandom;
        }

        private void InitializeFinals() {
            _finals = new List<bool[,]>();
            _boards.ForEach(x => _finals.Add(new bool[5, 5]));
        }

        private void BuildHash() {
            _hash = new Dictionary<int, List<Func<int>>>();
            foreach (var num in _randoms) {
                if (!_hash.ContainsKey(num)) {
                    var hash = new List<Func<int>>();
                    for (int boardIndex = 0; boardIndex < _boards.Count; boardIndex++) {
                        var board = _boards[boardIndex];
                        for (int x = 0; x < 5; x++) {
                            for (int y = 0; y < 5; y++) {
                                if (board[x, y] == num) {
                                    int result = boardIndex;
                                    int subX = x;
                                    int subY = y;
                                    hash.Add(() => {
                                        _finals[result][subX, subY] = true;
                                        return result;
                                    });
                                }
                            }
                        }
                    }
                    _hash.Add(num, hash);
                }
            }
        }

        private void GetRandomsAndBoards(List<string> input) {
            _randoms = input[0].Split(',').Select(x => Convert.ToInt32(x)).ToList();
            _boards = new List<int[,]>();
            int index = 2;
            do {
                var board = new int[5, 5];
                for (int y = 0; y < 5; y++) {
                    for (int x = 0; x < 5; x++) {
                        var test = new char[2] { input[index][x * 3], input[index][x * 3 + 1] };
                        var split = Convert.ToInt32(new string(new char[2] { input[index][x * 3], input[index][x * 3 + 1] }));
                        board[x, y] = split;
                    }
                    index++;
                }
                _boards.Add(board);
                index++;
            } while (index < input.Count);
        }

        private List<string> Input_Test1() {
            return new List<string>() {
                "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1",
                "",
                "22 13 17 11  0",
                " 8  2 23  4 24",
                "21  9 14 16  7",
                " 6 10  3 18  5",
                " 1 12 20 15 19",
                "",
                " 3 15  0  2 22",
                " 9 18 13 17  5",
                "19  8  7 25 23",
                "20 11 10 24  4",
                "14 21 16 12  6",
                "",
                "14 21 17 24  4",
                "10 16 15  9 19",
                "18  8 23 26 20",
                "22 11 13  6  5",
                " 2  0 12  3  7"
            };
        }
    }
}
