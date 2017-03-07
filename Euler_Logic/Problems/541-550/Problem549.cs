using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem549 : IProblem {
        private bool[] _notPrimes;

        public string ProblemName {
            get { return "549: Divisibility of factorials"; }
        }

        public string GetAnswer() {
            uint max = 10000000;
            SievePrimes(max);
            return Solve(max).ToString();
        }

        private void SievePrimes(uint max) {
            _notPrimes = new bool[max + 1];
            for (uint num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    for (uint composite = 2; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
        }

        private uint Solve(uint max) {
            uint sum = 0;
            for (uint num = 2; num <= max; num++) {
                sum += FindNextFactorial(num);
            }
            return sum;
        }

        private uint FindPowers(uint num, uint max) {
            uint sum = num;
            uint next = sum * 2;
            uint basePower = 2;
            uint power = (uint)Math.Pow(num, basePower);
            while (power <= max) {
                sum += FindNextFactorial(power);
                basePower++;
                power = (uint)Math.Pow(num, basePower);
            }
            return sum;
        }

        private uint FindNextFactorial(uint num) {
            if (!_notPrimes[num]) {
                return num;
            }
            uint baseNum = 3;
            uint factorial = 2;
            do {
                factorial = (factorial * baseNum) % num;
                if (factorial == 0) {
                    return baseNum;
                }
                baseNum++;
            } while (true);
        }
    }
}
