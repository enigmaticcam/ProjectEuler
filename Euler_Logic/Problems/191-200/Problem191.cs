using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem191 : ProblemBase {
        private ulong _max = 0;
        private ulong[] _absences;
        private ulong _count = 0;
        private Dictionary<ulong, Dictionary<ulong, ulong>> _combinations = new Dictionary<ulong, Dictionary<ulong, ulong>>();
        private Dictionary<ulong, ulong> _factorials = new Dictionary<ulong, ulong>();

        public override string ProblemName {
            get { return "191: Prize Strings"; }
        }

        public override string GetAnswer() {
            int length = 30;
            _max = CalcMax(length);
            FindAbsencesNoLates(length);
            FindTwoLatesOrMore((ulong)length);
            FindOneLates(length);
            return (_max - _count).ToString();
        }

        private ulong CalcMax(int length) {
            ulong num = 3;
            for (int power = 2; power <= length; power++) {
                num *= 3;
            }
            return num;
        }

        private void FindAbsencesNoLates(int maxLength) {
            _absences = new ulong[maxLength + 1];
            _absences[3] = 1;
            for (int length = 4; length <= maxLength; length++) {
                for (int position = 0; position + 3 <= length; position++) {
                    ulong count = (ulong)Math.Pow(2, length - position - 3);
                    if (position >= 2) {
                        _absences[length] += count * (ulong)Math.Pow(2, position - 1) - _absences[position - 1];
                    } else {
                        _absences[length] += count;
                    }
                }
            }
            _count += _absences[maxLength];
        }

        private void FindTwoLatesOrMore(ulong maxLength) {
            for (ulong lateCount = 2; lateCount <= maxLength; lateCount++) {
                _count += (ulong)Math.Pow(2, maxLength - lateCount) * DistinctCombinationsOfSetInSet(maxLength, lateCount);
            }
        }

        private void FindOneLates(int maxLength) {
            for (int position = 0; position < maxLength; position++) {
                ulong left = (ulong)Math.Pow(2, maxLength - position - 1) * _absences[position];
                ulong right = (ulong)Math.Pow(2, position) * _absences[maxLength - position - 1];
                ulong count = left + right - (left * right);
                _count += count;
            }
        }

        private ulong Factorial(ulong max) {
            if (!_factorials.ContainsKey(max)) {
                ulong sum = max;
                for (ulong num = max - 1; num >= 1; num--) {
                    sum *= num;
                }
                _factorials.Add(max, sum);
            }
            return _factorials[max];
        }

        private ulong DistinctCombinationsOfSetInSet(ulong n, ulong r) {
            if (!_combinations.ContainsKey(n)) {
                _combinations.Add(n, new Dictionary<ulong, ulong>());
            }
            if (!_combinations[n].ContainsKey(r)) {
                if (n == r) {
                    _combinations[n].Add(r, 1);
                } else {
                    _combinations[n].Add(r, Factorial(n) / (Factorial(r) * Factorial(n - r)));
                }
            }
            return _combinations[n][r];
        }
    }
}
