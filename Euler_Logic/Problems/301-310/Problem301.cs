using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem301 : IProblem {
        private Dictionary<ulong, Dictionary<ulong, Dictionary<ulong, bool>>> _games = new Dictionary<ulong, Dictionary<ulong, Dictionary<ulong, bool>>>();

        public string ProblemName {
            get { return "301: Nim"; }
        }

        public string GetAnswer() {
            ulong max = 100;
            RunGames(max);
            return CalcLosses(max).ToString();
        }

        private void RunGames(ulong n) {
            for (ulong a = 0; a <= n; a++) {
                for (ulong b = 0; b <= n * 2; b++) {
                    for (ulong c = 0; c <= n * 3; c++) {
                        if (!_games.ContainsKey(a) || !_games[a].ContainsKey(b) || !_games[a][b].ContainsKey(c)) {
                            TryGame(a, b, c);
                            ComputeSimilarGames(a, b, c, n);
                        }
                    }
                }
            }
        }

        private bool TryGame(ulong a, ulong b, ulong c) {
            if (a == 0 && b == 0 && c == 0) {
                AddGameResult(a, b, c, false);
                return true;
            } else if (a != 0 && b == 0 && c == 0) {
                AddGameResult(a, b, c, true);
                return true;
            } else if (a == 0 && b != 0 && c == 0) {
                AddGameResult(a, b, c, true);
                return true;
            } else if (a == 0 && b == 0 && c != 0) {
                AddGameResult(a, b, c, true);
                return true;
            } else if (a == 0 && b != 0 && c != 0 && b == c) {
                AddGameResult(a, b, c, false);
                return true;
            } else if (a != 0 && b == 0 && c != 0 && a == c) {
                AddGameResult(a, b, c, false);
                return true;
            } else if (a != 0 && b != 0 && c == 0 && a == b) {
                AddGameResult(a, b, c, false);
                return true;
            } else if (a == 0 && b != 0 && c != 0 && b != c) {
                AddGameResult(a, b, c, true);
                return true;
            } else if (a != 0 && b == 0 && c != 0 && a != c) {
                AddGameResult(a, b, c, true);
                return true;
            } else if (a != 0 && b != 0 && c == 0 && a != b) {
                AddGameResult(a, b, c, true);
                return true;
            }
            return false;
        }

        private void ComputeSimilarGames(ulong a, ulong b, ulong c, ulong n) {
            bool result = _games[a][b][c];
            for (ulong add = 1; add + c <= n * 3; add++) {
                if (add + a <= n) {
                    if (!TryGame(add + a, b, c)) {
                        AddGameResult(add + a, b, c, !result);
                    }
                }
                if (add + b <= n * 2) {
                    if (!TryGame(a, add + b, c)) {
                        AddGameResult(a, add + b, c, !result);
                    }
                }
                if (add + c <= n * 3) {
                    if (!TryGame(a, b, add + c)) {
                        AddGameResult(a, b, add + c, !result);
                    }
                }
            }
        }

        private void AddGameResult(ulong a, ulong b, ulong c, bool result) {
            if (!_games.ContainsKey(a)) {
                _games.Add(a, new Dictionary<ulong, Dictionary<ulong, bool>>());
            }
            if (!_games[a].ContainsKey(b)) {
                _games[a].Add(b, new Dictionary<ulong, bool>());
            }
            if (!_games[a][b].ContainsKey(c)) {
                _games[a][b].Add(c, result);
            }
        }

        private ulong CalcLosses(ulong n) {
            ulong sum = 0;
            for (ulong num = 1; num <= n; num++) {
                if (!_games[num][num * 2][num * 3]) {
                    sum++;
                }
            }
            return sum;
        }
    }
}
