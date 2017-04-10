using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem306 : ProblemBase {
        private Dictionary<int, bool> _games = new Dictionary<int, bool>();

        public override string ProblemName {
            get { return "306: Paper-strip Game"; }
        }

        public override string GetAnswer() {
            int max = 1000000;
            int winCount = 0;

            Initialize();
            for (int i = 1; i <= max; i++) {
                bool didWin = TryWithLength(i);
                if (didWin) {
                    winCount++;
                }
                if (!_games.ContainsKey(i)) {
                    _games.Add(i, didWin);
                }
            }

            return winCount.ToString();
        }

        private void Initialize() {
            _games.Add(0, false);
            _games.Add(1, false);
            _games.Add(2, true);
        }

        private bool TryWithLength(int length) {
            if (_games.ContainsKey(length)) {
                return _games[length];            
            } else {
                for (int i = 1; i <= length - 1; i++) {
                    bool prefix = TryWithLength(i - 1);
                    bool suffix = TryWithLength(length - i - 1);
                    if (!prefix) {
                        suffix = !suffix;
                    }
                    if (suffix) {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
