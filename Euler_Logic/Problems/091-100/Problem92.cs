using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem92 : IProblem {
        private Dictionary<UInt64, int> _chainToLoop = new Dictionary<UInt64, int>();
        private Dictionary<int, int> _counts = new Dictionary<int, int>();

        public string ProblemName {
            get { return "92: Square Digit Chains"; }
        }

        public string GetAnswer() {
            _counts.Add(1, 0);
            _counts.Add(89, 0);
            _chainToLoop.Add(1, 1);
            _chainToLoop.Add(89, 89);
            for (UInt64 num = 1; num < 10000000; num++) {
                AddTo89Or1(num);
            }
            return _counts[89].ToString();
        }

        private void AddTo89Or1(UInt64 num) {
            HashSet<UInt64> _chain = new HashSet<UInt64>();
            if (_chainToLoop.ContainsKey(num)) {
                _counts[_chainToLoop[num]]++;
            } else {
                UInt64 newNum = num;
                do {
                    newNum = GetNext(newNum);
                    if (_chainToLoop.ContainsKey(newNum)) {
                        newNum = (UInt64)_chainToLoop[newNum];
                        break;
                    }
                    _chain.Add(newNum);
                } while (newNum != 1 && newNum != 89);
                foreach (UInt64 chainNum in _chain) {
                    if (chainNum != 1 && chainNum != 89) {
                        _chainToLoop.Add(chainNum, (int)newNum);
                    }
                }
                _counts[(int)newNum]++;
            }
        }

        private UInt64 GetNext(UInt64 num) {
            string numAsString = num.ToString();
            UInt64 total = 0;
            for (int i = 0; i < numAsString.Length; i++) {
                UInt64 x = Convert.ToUInt64(numAsString.Substring(i, 1));
                total += x * x;
            }
            return total;
        }
    }
}
