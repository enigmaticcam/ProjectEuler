using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems  {
    public class Problem381 : ProblemBase {
        bool[] _notPrimes;
        int _sum = 0;

        public override string ProblemName {
            get { return "381: (prime-k) factorial"; }
        }

        public override string GetAnswer() {
            int max = 1000000;
            SievePrimes(max);
            return _sum.ToString();
        }

        private void SievePrimes(int max) {
            _notPrimes = new bool[max + 1];
            for (int num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    if (num >= 5) {
                        _sum += Factorize(num);
                    }
                    for (int composite = 2; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
        }

        private int Factorize(int num) {
            if (num == 5) {
                return 4;
            }
            int factorial = 1;
            int sum = 0;
            for (int baseNum = 2; baseNum < num; baseNum++) {
                factorial = (factorial * baseNum) % num;
                if (factorial == 0) {
                    break;
                }
                if (baseNum >= num - 5) {
                    sum = (sum + factorial) % num;
                }
            }
            return sum;
        }
    }
}
