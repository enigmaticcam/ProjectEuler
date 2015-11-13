using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem104 : IProblem {
        private BigInteger _rootOfFivePower = 0;
        private BigInteger _powerOfTwo = 0;
        private BigInteger _index = 0;
        private BigInteger _rootOfFiveNumber = 0;

        public string ProblemName {
            get { return "104: Pandigital Fibonacci ends"; }
        }

        public string GetAnswer() {
            return FindPandigitalFib().ToString();
        }

        private int FindPandigitalFib() {
            int index = 2;
            int lastTenDigits = 1;
            int lastTenDigitsPrev = 1;
            do {
                index++;
                lastTenDigits = lastTenDigitsPrev + lastTenDigits;
                lastTenDigitsPrev = lastTenDigits - lastTenDigitsPrev;
                lastTenDigits = lastTenDigits % 1000000000;
                NextPower(index);
                if (IsPandigital(lastTenDigits.ToString()) && IsPandigital(GetFirstNineDigits())) {
                    return index;
                }                
            } while (true);
        }

        private bool IsPandigital(string number) {
            for (int digit = 1; digit <= 9; digit++) {
                if (number.Replace(digit.ToString(), "").Length != number.Length - 1) {
                    return false;
                }
            }
            return true;
        }

        private string GetFirstNineDigits() {
            BigInteger number = _rootOfFivePower / (_powerOfTwo * (BigInteger)223606797749978);
            if (number < (BigInteger)1000000000) {
                return number.ToString();
            } else {
                return number.ToString().Substring(0, 9);
            }
        }

        private void NextPower(int maxIndex) {
            if (_rootOfFivePower == 0) {
                _rootOfFivePower = 100000000000000 + 223606797749978;
                _powerOfTwo = 2;
                _index = 1;
                _rootOfFiveNumber = _rootOfFivePower;
            }
            while (_index < maxIndex) {
                _index++;
                _rootOfFivePower *= _rootOfFiveNumber;
                _powerOfTwo *= 2;
            }
        }
    }
}
