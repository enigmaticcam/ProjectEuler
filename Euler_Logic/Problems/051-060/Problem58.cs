using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem58 : ProblemBase {
        private List<UInt64> _diagonals = new List<UInt64>();
        private UInt64 _squareSize;
        private UInt64 _number;
        private UInt64 _primeCount;

        public override string ProblemName {
            get { return "58: Spiral Primes"; }
        }

        public override string GetAnswer() {
            _squareSize = 1;
            _number = 1;
            _diagonals.Add(1);
            do {
                AddNewLayer();
            } while (Convert.ToDecimal(_primeCount) / Convert.ToDecimal(_diagonals.Count) >= Convert.ToDecimal(.1));
            return _squareSize.ToString();
        }

        public void AddNewLayer() {
            _squareSize += 2;
            _diagonals.Add((_squareSize - 1) + _number);
            _diagonals.Add(((_squareSize - 1) * 2) + _number);
            _diagonals.Add(((_squareSize - 1) * 3) + _number);
            _diagonals.Add(((_squareSize - 1) * 4) + _number);
            _number += (_squareSize - 1) * 4;
            if (IsPrime(_diagonals[_diagonals.Count - 1])) {
                _primeCount++;
            }
            if (IsPrime(_diagonals[_diagonals.Count - 2])) {
                _primeCount++;
            }
            if (IsPrime(_diagonals[_diagonals.Count - 3])) {
                _primeCount++;
            }
            if (IsPrime(_diagonals[_diagonals.Count - 4])) {
                _primeCount++;
            }
        }

        private bool IsPrime(UInt64 number) {
            if (number <= 1) {
                return false;
            }
            if (number == 2) {
                return true;
            }
            if (number % 2 == 0) {
                return false;
            }
            for (UInt64 i = 2; i <= Math.Sqrt(number); i++) {
                if (number % i == 0) {
                    return false;
                }
            }
            return true;
        }
    }
}
