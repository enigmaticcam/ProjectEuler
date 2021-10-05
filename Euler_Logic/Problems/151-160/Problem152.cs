using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Euler_Logic.Problems {
    public class Problem152 : ProblemBase {
        public override string ProblemName {
            get { return "152: Writing 1/2 as a sum of inverse squares"; }
        }

        /*
            Whenever you add fractions, if you introduce a new prime factor, the lcm will undoubtedly be higher. You have to find
            another number with that same prime factor to reduce it. So I knew that any combination of distinct integers can't
            use more than a few prime factors, and those prime factors have to be small. So after much testing and trial/error,
            I narrowed it down to just 5 prime factors: 2, 3, 5, 7, and 13.

            My next method is to try all combinations of numbers that have at least one of those prime factors. You can reduce
            the variations to check by (1) stopping when the resulting fraction is greater than or equal to 1/2, and (2) not
            adding a new fraction if the difference between it and 1/2 is less than the smallest possible size fraction that we
            can make.

            Even despite these, the number of combinations is still too high. So instead of trying all combinations recursively,
            I precalculate all possible subsets of the highest 20 numbers. Then, when trying combinations, I stop just before the
            highest 20 and check if the the remainder to 1/2 exists in those subsets. That significantly reduces the number of
            combinations I have to check.

            Also, instead of using fractions, I simply calculate the lcm of all valid numbers. Because we are not using all numbers
            between 2 and 80, the lcm is within ulong range.
         */

        private PrimeSieve _primes;
        private int _count;
        public override string GetAnswer() {
            int max = 80;
            int remove = 20;
            _primes = new PrimeSieve((ulong)max);
            FindNumsWithPrimeLimit(new List<ulong>() { 2, 3, 5, 7, 13 }, (ulong)max);
            int subsetMax = _validNums.Count - remove;
            InitializeBigInt();
            ComputeBottomSubsets((int)subsetMax - 1, 0);
            _validNums.RemoveRange(subsetMax - 1, remove + 1);
            Recursive(1);
            return _count.ToString();
        }

        private List<ulong> _validNums = new List<ulong>();
        private void FindNumsWithPrimeLimit(List<ulong> primes, ulong max) {
            for (ulong num = 2; num <= max; num++) {
                var temp = num;
                foreach (var prime in primes) {
                    while (temp % prime == 0) {
                        temp /= prime;
                    }
                }
                if (temp == 1) {
                    _validNums.Add(num);
                }
            }
        }

        private Dictionary<ulong, ulong> _lcm = new Dictionary<ulong, ulong>();
        private ulong _half;
        private ulong _max;
        private void InitializeBigInt() {
            ulong lcm = 1;
            foreach (var num in _validNums) {
                lcm = LCM.GetLCM(lcm, num * num);
            }
            foreach (var num in _validNums) {
                _lcm.Add(num, lcm / (num * num));
            }
            _half = lcm / 2;
            _max = _lcm[_validNums.Last()];
            _current = _lcm[2];
        }

        private ulong _current;
        private void Recursive(int index) {
            if (index == _validNums.Count) {
                var remainder = _half - _current;
                if (_subsets.ContainsKey(remainder)) {
                    _count += _subsets[remainder];
                }
            } else {
                var tempCurrent = _current;
                var num = _validNums[index];
                _current += _lcm[num];
                if (_current < _half && _half - _current >= _max) {
                    Recursive(index + 1);
                }
                _current = tempCurrent;
                Recursive(index + 1);
            }
        }

        private Dictionary<ulong, int> _subsets = new Dictionary<ulong, int>();
        private void ComputeBottomSubsets(int index, ulong sum) {
            var num = _lcm[_validNums[index]];
            var nextSum = sum + num;
            if (nextSum <= _half) {
                if (index < _validNums.Count - 1) {
                    ComputeBottomSubsets(index + 1, nextSum);
                } else {
                    if (!_subsets.ContainsKey(nextSum)) {
                        _subsets.Add(nextSum, 1);
                    } else {
                        _subsets[nextSum]++;
                    }
                }
            }
            if (index < _validNums.Count - 1) {
                ComputeBottomSubsets(index + 1, sum);
            } else {
                if (!_subsets.ContainsKey(sum)) {
                    _subsets.Add(sum, 1);
                } else {
                    _subsets[sum]++;
                }
            }
        }
    }
}
