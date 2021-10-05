using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem209 : ProblemBase {

        public override string ProblemName {
            get { return "209: Circular Logic"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
            //var y = GetTruthTable((a, b) => new Tuple<bool, bool>(b, (a ^ b)));
            //BruteForce(2);
            return "";
        }

        private ulong Solve() {
            var nums = new HashSet<ulong>(GetTruthTable((a, b, c, d, e, f) => new Tuple<bool, bool, bool, bool, bool, bool>(b, c, d, e, f, a ^ (b & c))));
            _power = new PowerAll(2);
            IncludeExclude(true, 0, 0, nums.ToList());
            return _sum;
        }

        //private void IncludeExlucde(bool add, int startPrime, ulong prod, ulong max) {
        //    for (int index = startPrime; index < _primes.Count; index++) {
        //        var prime = _primes[index];
        //        prime *= prime;
        //        if (max / prime >= prod) {
        //            if (add) {
        //                var temp = _sum;
        //                _sum += max / (prime * prod);
        //            } else {
        //                _sum -= max / (prime * prod);
        //            }
        //            IncludeExlucde(!add, index + 1, prime * prod, max);
        //        } else {
        //            break;
        //        }
        //    }
        //}

        private ulong _sum = 0;
        private PowerAll _power;
        private void IncludeExclude(bool add, int start, ulong mask, List<ulong> nums) {
            for (int index = start; index < nums.Count; index++) {
                var num = nums[index];
                var nextMask = num | mask;
                var bitCount = CountBits(nextMask);
                var sumToAdd = _power.GetPower(64 - CountBits(nextMask));
                if (add) {
                    _sum += sumToAdd;
                } else {
                    _sum -= sumToAdd;
                }
                IncludeExclude(!add, index + 1, nextMask, nums);
            }
        }

        private int CountBits(ulong num) {
            int sum = 0;
            ulong power = _power.GetPower(63);
            for (int count = 1; count <= 64; count++) {
                if (num >= power) {
                    num -= power;
                    sum++;
                }
                power /= 2;
            }
            return sum;
        }

        private void BruteForce(int size) {
            var nums = BruteForceGetNums(size);
            nums = BruteForceFind(8, nums);
            nums = BruteForceFind(5, nums);
            nums = BruteForceFind(6, nums);
            nums = BruteForceFind(3, nums);
            bool stop = true;
        }

        private List<ulong> GetTruthTable(Func<bool, bool, Tuple<bool, bool>> op) {
            var result = new List<ulong>();
            var hash = GetFullTableSize2();
            bool bit1 = true;
            bool bit2 = true;
            for (int count1 = 1; count1 <= 2; count1++) {
                for (int count2 = 1; count2 <= 2; count2++) {
                    var bit = op(bit1, bit2);
                    result.Add(hash[bit] | hash[new Tuple<bool, bool>(bit1, bit2)]);
                    bit2 = !bit2;
                }
                bit1 = !bit1;
            }
            return result;
        }

        private List<ulong> GetTruthTable(Func<bool, bool, bool, bool, bool, bool, Tuple<bool, bool, bool, bool, bool, bool>> op) {
            var result = new List<ulong>();
            var hash = GetFullTableSize64();
            bool[] bits = new bool[6] { true, true, true, true, true, true };
            for (int count1 = 1; count1 <= 2; count1++) {
                for (int count2 = 1; count2 <= 2; count2++) {
                    for (int count3 = 1; count3 <= 2; count3++) {
                        for (int count4 = 1; count4 <= 2; count4++) {
                            for (int count5 = 1; count5 <= 2; count5++) {
                                for (int count6 = 1; count6 <= 2; count6++) {
                                    var bit = op(bits[0], bits[1], bits[2], bits[3], bits[4], bits[5]);
                                    result.Add(hash[bit] | hash[new Tuple<bool, bool, bool, bool, bool, bool>(bits[0], bits[1], bits[2], bits[3], bits[4], bits[5])]);
                                    bits[5] = !bits[5];
                                }
                                bits[4] = !bits[4];
                            }
                            bits[3] = !bits[3];
                        }
                        bits[2] = !bits[2];
                    }
                    bits[1] = !bits[1];
                }
                bits[0] = !bits[0];
            }
            return result;
        }

        private Dictionary<Tuple<bool, bool>, ulong> GetFullTableSize2() {
            var hash = new Dictionary<Tuple<bool, bool>, ulong>();
            bool bit1 = true;
            bool bit2 = true;
            ulong powerOf2 = 1;
            for (int count1 = 1; count1 <= 2; count1++) {
                for (int count2 = 1; count2 <= 2; count2++) {
                    hash.Add(new Tuple<bool, bool>(bit1, bit2), powerOf2);
                    powerOf2 *= 2;
                    bit2 = !bit2;
                }
                bit1 = !bit1;
            }
            return hash;
        }

        private Dictionary<Tuple<bool, bool, bool, bool, bool, bool>, ulong> GetFullTableSize64() {
            _powerOf2 = 1;
            var hash = new Dictionary<Tuple<bool, bool, bool, bool, bool, bool>, ulong>();
            FullTableSizeRecursive(0, hash, new bool[6]);
            return hash;
        }

        private ulong _powerOf2;
        private void FullTableSizeRecursive(int index, Dictionary<Tuple<bool, bool, bool, bool, bool, bool>, ulong> hash, bool[] bits) {
            if (index < bits.Length - 1) {
                bits[index] = true;
                FullTableSizeRecursive(index + 1, hash, bits);
                bits[index] = false;
                FullTableSizeRecursive(index + 1, hash, bits);
            } else {
                bits[index] = true;
                hash.Add(new Tuple<bool, bool, bool, bool, bool, bool>(bits[0], bits[1], bits[2], bits[3], bits[4], bits[5]), _powerOf2);
                _powerOf2 *= 2;
                bits[index] = false;
                hash.Add(new Tuple<bool, bool, bool, bool, bool, bool>(bits[0], bits[1], bits[2], bits[3], bits[4], bits[5]), _powerOf2);
                _powerOf2 *= 2;
            }
        }

        private List<ulong> BruteForceGetNums(int size) {
            ulong max = (ulong)Math.Pow(2, size + 1);
            var result = new List<ulong>();
            for (ulong num = 0; num < max; num++) {
                result.Add(num);
            }
            return result;
        }

        private List<ulong> BruteForceFind(ulong and, List<ulong> nums) {
            var result = new List<ulong>();
            foreach (var num in nums) {
                if ((num & and) != and) {
                    result.Add(num);
                }
            }
            return result;
        }
    }
}
