using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem387 : IProblem {
        private HashSet<string> _hashadNumbers = new HashSet<string>();
        private bool[] _notPrimes;

        public string ProblemName {
            get { return "387: Harshad Numbers"; }
        }

        public string GetAnswer() {
            ulong max = 10000;
            SievePrimes(max - 1);
            GenerateHashadNumbers("", max.ToString().Length);
            return Solve(max - 1).ToString();
        }

        public void GenerateHashadNumbers(string num, int maxLength) {
            int start = 0;
            if (num == "") {
                start = 1;
            }
            for (int nextDigit = start; nextDigit <= 9; nextDigit++) {
                string newNum = num + nextDigit.ToString();
                if (IsHashad(newNum)) {
                    if (newNum.Length > 1) {
                        _hashadNumbers.Add(newNum);
                    }
                    if (newNum.Length < maxLength) {
                        GenerateHashadNumbers(newNum, maxLength);
                    }
                }
            }
        }

        private bool IsHashad(string num) {
            ulong sum = 0;
            for (int index = 0; index < num.Length; index++) {
                sum += Convert.ToUInt64(num.Substring(index, 1));
            }
            if (Convert.ToUInt64(num) % sum == 0 && !_notPrimes[sum]) {
                return true;
            } else {
                return false;
            }
        }

        private void SievePrimes(ulong max) {
            _notPrimes = new bool[max + 2];
            for (ulong num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    for (ulong composite = num * 2; composite <= max + 1; composite += num) {
                        _notPrimes[composite] = true;
                    }
                }
            }
        }

        private ulong Solve(ulong max) {
            ulong sum = 0;
            for (ulong num = 11; num <= max; num += 2) {
                if (!_notPrimes[num] && _hashadNumbers.Contains(num.ToString().Substring(0, num.ToString().Length - 1))) {
                    sum += num;
                }
            }
            return sum;
        }
    }
}
