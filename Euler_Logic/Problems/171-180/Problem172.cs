using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem172 : ProblemBase {
        public override string ProblemName {
            get { return "172: Investigating numbers with few repeated digits"; }
        }

        public override string GetAnswer() {
            return Solve2(18).ToString();
        }

        private ulong _total = 0;
        private ulong Solve2(int maxDigit) {
            BuildFactorials(maxDigit);
            LoopBase4(maxDigit);
            return _total;
        }

        private int[] _digitCounts = new int[10];
        private int _digitCountSum = 0;
        private int _digitDistinctCount = 0;
        private void LoopBase4(int maxDigit) {
            ulong max = (ulong)Math.Pow(4, 10) - 1;
            _digitCounts[0] = 3;
            _digitCountSum = 3;
            _digitDistinctCount = 1;
            for (ulong count = 4; count <= max; count++) {
                AddOne();
                if (_digitCountSum == maxDigit) {
                    CalcCurrentSet(maxDigit);
                }
            }
        }

        private void CalcCurrentSet(int maxDigit) {
            ulong count = 1;
            int found = 0;
            int remaining = _digitCountSum;
            for (int digit = 0; digit <= 9; digit++) {
                if (_digitCounts[digit] > 0) {
                    count *= Choose(digit == 0 ? remaining - 1 : remaining, _digitCounts[digit]);
                    found++;
                    if (found == _digitDistinctCount - 1) {
                        break;
                    }
                    remaining -= _digitCounts[digit];
                }
            }
            _total += count;
        }

        private void AddOne() {
            int currentIndex = 0;
            while (_digitCounts[currentIndex] == 3) {
                _digitCountSum -= _digitCounts[currentIndex];
                _digitCounts[currentIndex] = 0;
                currentIndex++;
                _digitDistinctCount--;
            }
            if (_digitCounts[currentIndex] == 0) {
                _digitDistinctCount++;
            }
            _digitCounts[currentIndex]++;
            _digitCountSum++;
        }

        private Dictionary<int, ulong> _factorials = new Dictionary<int, ulong>();
        private void BuildFactorials(int maxDigit) {
            ulong num = 1;
            _factorials.Add(0, 1);
            _factorials.Add(1, 1);
            for (int next = 2; next <= maxDigit; next++) {
                num *= (ulong)next;
                _factorials.Add(next, num);
            }
        }

        private ulong Choose(int x, int y) {
            return _factorials[x] / (_factorials[y] * _factorials[x - y]);
        }
    }
}
