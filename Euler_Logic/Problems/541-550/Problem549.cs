using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    // New Method
    public class Problem549 : ProblemBase {
        private bool[] _notPrimes;
        private HashSet<uint> _primes = new HashSet<uint>();

        public override string ProblemName {
            get { return "549: Divisibility of factorials"; }
        }

        public override string GetAnswer() {
            uint max = 100000000;
            SievePrimes(max);
            PrimeFactor(max);
            return "";
            //return Solve(max).ToString();
        }

        private void SievePrimes(uint max) {
            _notPrimes = new bool[max + 1];
            for (uint num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    _primes.Add(num);
                    for (uint composite = 2; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
            _notPrimes = null;
        }

        private void PrimeFactor(uint max) {
            for (uint num = 4; num <= max; num++) {
                if (!_primes.Contains(num)) {
                    uint leftovers = num;
                    foreach (uint prime in _primes) {
                        while (leftovers % prime == 0) {
                            leftovers /= prime;
                        }
                        if (leftovers == 1 || _primes.Contains(leftovers)) {
                            break;
                        }
                    }
                }
            }
        }
    }

    // Old Method
    //public class Problem549 : ProblemBase {
    //    private bool[] _notPrimes;

    //    public override string ProblemName {
    //        get { return "549: Divisibility of factorials"; }
    //    }

    //    public override string GetAnswer() {
    //        uint max = 100;
    //        SievePrimes(max);
    //        return Solve(max).ToString();
    //    }

    //    private void SievePrimes(uint max) {
    //        _notPrimes = new bool[max + 1];
    //        for (uint num = 2; num <= max; num++) {
    //            if (!_notPrimes[num]) {
    //                for (uint composite = 2; composite * num <= max; composite++) {
    //                    _notPrimes[composite * num] = true;
    //                }
    //            }
    //        }
    //    }

    //    private uint Solve(uint max) {
    //        uint sum = 0;
    //        for (uint num = 2; num <= max; num++) {
    //            sum += FindNextFactorial(num);
    //        }
    //        return sum;
    //    }

    //    private uint FindNextFactorial(uint num) {
    //        if (!_notPrimes[num]) {
    //            return num;
    //        }
    //        uint baseNum = 3;
    //        uint factorial = 2;
    //        do {
    //            factorial = (factorial * baseNum) % num;
    //            if (factorial == 0) {
    //                return baseNum;
    //            }
    //            baseNum++;
    //        } while (true);
    //    }
    //}
}
