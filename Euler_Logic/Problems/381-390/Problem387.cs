using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem387 : IProblem {
        private HashSet<string> _hashadNumbers = new HashSet<string>();
        private HashSet<ulong> _strongTruncatable = new HashSet<ulong>();

        public string ProblemName {
            get { return "387: Harshad Numbers"; }
        }

        public string GetAnswer() {
            GenerateHashadNumbers("", 14);
            GenerateStrongTruncatablePrimes();
            return Sum().ToString();
        }

        public void GenerateHashadNumbers(string num, int maxLength) {
            int start = 0;
            if (num == "") {
                start = 1;
            }
            for (int nextDigit = start; nextDigit <= 9; nextDigit++) {
                string newNum = num + nextDigit.ToString();
                if (IsHashad(newNum)) {
                    if (newNum.Length < maxLength - 1) {
                        GenerateHashadNumbers(newNum, maxLength);
                    }
                }
            }
        }

        private void GenerateStrongTruncatablePrimes() {
            foreach (string num in _hashadNumbers) {
                for (int digit = 1; digit <= 9; digit++) {
                    ulong newNum = Convert.ToUInt64(num + digit.ToString());
                    if (IsPrime(newNum)) {
                        _strongTruncatable.Add(newNum);
                    }
                }
            }
        }

        private ulong Sum() {
            ulong sum = 0;
            foreach (ulong num in _strongTruncatable) {
                sum += num;
            }
            return sum;
        }

        private bool IsHashad(string num) {
            ulong sum = 0;
            for (int index = 0; index < num.Length; index++) {
                sum += Convert.ToUInt64(num.Substring(index, 1));
            }
            if (Convert.ToUInt64(num) % sum == 0) {
                if (IsPrime(Convert.ToUInt64(num) / sum) && num.Length > 1) {
                    _hashadNumbers.Add(num);
                }
                return true;
            } else {
                return false;
            }
        }

        private bool IsPrime(ulong num) {
            if (num == 1) {
                return false;
            } else if (num == 2) {
                return true;
            } else if (num % 2 == 0) {
                return false;
            } else {
                for (ulong composite = 3; composite <= (ulong)Math.Sqrt(num); composite += 2) {
                    if (num % composite == 0) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
