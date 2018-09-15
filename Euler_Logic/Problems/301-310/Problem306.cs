using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem306 : ProblemBase {

        public override string ProblemName {
            get { return "306: Paper-strip Game"; }
        }

        public override string GetAnswer() {
            return "";
        }

        private void Solve(int upTo) {
            for (int size = 2; size <= upTo; size++) {
                _games.Add(size, Test(size));
            }
        }

        private Dictionary<int, bool> _games = new Dictionary<int, bool>();
        private bool _initialized;
        private bool Test(int size) {
            if (!_initialized) {
                for (int root = 0; root <= 63; root++) {
                    _powers.Add(root, (ulong)Math.Pow(2, root));
                }
                _initialized = true;
            }
            _strip = new bool[size];
            _hash = new Dictionary<ulong, bool>();
            return CanForceWin(true, 0);
        }

        private bool[] _strip;
        private Dictionary<int, ulong> _powers = new Dictionary<int, ulong>();
        private Dictionary<ulong, bool> _hash = new Dictionary<ulong, bool>();
        private bool CanForceWin(bool isPlayerOne, ulong hash) {
            if (!_hash.ContainsKey(hash)) {
                bool value = false;
                for (int index = 0; index < _strip.Count() - 1; index++) {
                    if (!_strip[index] && !_strip[index + 1]) {

                        // Increment for next move
                        _strip[index] = true;
                        _strip[index + 1] = true;
                        hash += _powers[index];
                        hash += _powers[index + 1];

                        // Make next move
                        bool nextMove = CanForceWin(!isPlayerOne, hash);

                        // Revert to current move
                        _strip[index] = false;
                        _strip[index + 1] = false;
                        hash -= _powers[index];
                        hash -= _powers[index + 1];

                        // Stop if can force win
                        if (!nextMove) {
                            value = true;
                            break;
                        }
                    }
                }
                _hash.Add(hash, value);
                return value;
            } else {
                return _hash[hash];
            }
        }
    }
}
