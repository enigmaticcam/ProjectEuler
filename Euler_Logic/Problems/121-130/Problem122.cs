using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem122 : ProblemBase {
        private int[] _sums;
        private List<int> _current;
        private int _nextNumber;
        private int _steps;
        private int _lastMax;
        private int _sum = 0;

        public override string ProblemName {
            get { return "122: Efficient exponentiation"; }
        }

        public override string GetAnswer() {
            int max = 10;
            Initialize(max);
            return Solve(max).ToString();
        }

        private int Solve(int max) {
            do {
                List<int> newCurrent = new List<int>();
                foreach (int num in _current) {
                    for (int next = _nextNumber; _nextNumber - num <= _lastMax && _nextNumber - num < num; next++) {
                        _sums[_nextNumber] = _steps;
                        _sum += _steps;
                        newCurrent.Add(_nextNumber);
                        _nextNumber++;
                        if (_nextNumber > max) {
                            return _sum;
                        }
                    }
                }
                _current = newCurrent;
                _steps++;
                _lastMax = _nextNumber - 1;
            } while (true);
        }

        private void Initialize(int max) {
            _sums = new int[max + 1];
            _sums[1] = 1;
            _sums[2] = 1;
            _sums[3] = 2;
            _sums[4] = 2;
            _current = new List<int>();
            _current.Add(3);
            _current.Add(4);
            _nextNumber = 5;
            _steps = 3;
            _lastMax = 2;
            _sum = 6;
        }
    }
}
