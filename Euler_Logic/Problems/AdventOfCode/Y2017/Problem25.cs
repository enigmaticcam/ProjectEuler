using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem25 : AdventOfCodeBase {
        private Dictionary<int, bool> _hash;
        private Dictionary<string, Action> _states;
        private string _currentState;
        private int _maxSteps;
        private int _index;

        public override string ProblemName {
            get { return "Advent of Code 2017: 25"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            _hash = new Dictionary<int, bool>();
            _hash.Add(0, false);
            GetStates(input);
            Perform();
            return GetChecksum();
        }

        private void Perform() {
            for (int count = 1; count <= _maxSteps; count++) {
                _states[_currentState]();
            }
        }

        private int GetChecksum() {
            return _hash.Values.Where(x => x).Count();
        }

        private void GetStates(List<string> input) {
            _states = new Dictionary<string, Action>();
            _currentState = input[0].Substring(15, 1);
            _maxSteps = Convert.ToInt32(input[1].Split(' ')[5]);
            for (int index = 3; index < input.Count; index += 10) {
                var line = input[index];
                var name = line.Substring(9, 1);
                var writeValueIf0 = input[index + 2][22] == '1';
                var moveDirectionIf0 = (input[index + 3][27] == 'r' ? 1 : -1);
                var nextStateIf0 = input[index + 4].Substring(26, 1);
                var writeValueIf1 = input[index + 6][22] == '1';
                var moveDirectionIf1 = (input[index + 7][27] == 'r' ? 1 : -1);
                var nextStateIf1 = input[index + 8].Substring(26, 1);
                _states.Add(name, () => {
                    var value = _hash[_index];
                    _hash[_index] = (!value ? writeValueIf0 : writeValueIf1);
                    _index += (!value ? moveDirectionIf0 : moveDirectionIf1);
                    if (!_hash.ContainsKey(_index)) {
                        _hash.Add(_index, false);
                    }
                    _currentState = (!value ? nextStateIf0 : nextStateIf1);
                });
            }
        }
    }
}
