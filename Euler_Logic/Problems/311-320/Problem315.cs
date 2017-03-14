using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem315 : IProblem {
        private List<ulong> _primes = new List<ulong>();
        private bool[] _notPrimes;
        private Dictionary<ulong, ulong> _bits = new Dictionary<ulong, ulong>();

        public string ProblemName {
            get { return "315: Digital root clocks"; }
        }

        public string GetAnswer() {
            SievePrimes(10000000, 20000000);
            //Test();
            BuildBits();
            return Solve().ToString();
        }

        private void Test() {
            _primes.Add(37);
        }

        private int Solve() {
            int count = 0;
            foreach (ulong num in _primes) {
                ulong newNum = num;
                ulong last = num;
                count += CalcDiff(ulong.MaxValue, num);
                while (newNum > 9) {
                    ulong remaining = newNum;
                    ulong power = 1;
                    ulong sum = 0;
                    while (remaining > 0) {
                        ulong mod = (ulong)Math.Pow(10, power);
                        ulong modMinusOne = (ulong)Math.Pow(10, power - 1);
                        ulong diff = (remaining % mod);
                        remaining -= diff;
                        diff /= modMinusOne;
                        sum += diff;
                        power++;
                    }
                    newNum = sum;
                    count += CalcDiff(last, newNum);
                    last = newNum;
                }
            }
            return count;
        }

        private int CalcDiff(ulong numFrom, ulong numTo) {
            ulong remainingFrom = numFrom;
            ulong remainingTo = numTo;
            ulong power = 1;
            int sam = 0;
            int max = 0;
            while (remainingFrom > 0 || remainingTo > 0) {
                ulong mod = (ulong)Math.Pow(10, power);
                ulong modMinusOne = (ulong)Math.Pow(10, power - 1);
                ulong diffFrom = 0;
                ulong diffTo = 0;
                
                if (modMinusOne > remainingFrom || numFrom == ulong.MaxValue) {
                    diffFrom = ulong.MaxValue;
                    remainingFrom = 0;
                } else {
                    diffFrom = (remainingFrom % mod);
                    remainingFrom -= diffFrom;
                    diffFrom /= modMinusOne;
                }
                if (modMinusOne > remainingTo || numTo == ulong.MaxValue) {
                    diffTo = ulong.MaxValue;
                    remainingTo = 0;
                } else {
                    diffTo = (remainingTo % mod);
                    remainingTo -= diffTo;
                    diffTo /= modMinusOne;
                }

                sam += NumberOfSetBits(_bits[diffFrom] ^ _bits[ulong.MaxValue]) + NumberOfSetBits(_bits[ulong.MaxValue] ^ _bits[diffTo]);
                max += NumberOfSetBits(_bits[diffFrom] ^ _bits[diffTo]);

                power++;
            }
            return sam - max;
        }

        private void BuildBits() {
            _bits.Add(ulong.MaxValue, 0);
            _bits.Add(0, 119);
            _bits.Add(1, 36);
            _bits.Add(2, 93);
            _bits.Add(3, 109);
            _bits.Add(4, 46);
            _bits.Add(5, 107);
            _bits.Add(6, 123);
            _bits.Add(7, 39);
            _bits.Add(8, 127);
            _bits.Add(9, 111);
        }

        private int NumberOfSetBits(ulong i) {
            i = i - ((i >> 1) & 0x5555555555555555UL);
            i = (i & 0x3333333333333333UL) + ((i >> 2) & 0x3333333333333333UL);
            return (int)(unchecked(((i + (i >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
        }

        private void SievePrimes(ulong min, ulong max) {
            _notPrimes = new bool[max + 1];
            for (ulong num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    if (num >= min && num <= max) {
                        _primes.Add(num);
                    }
                    for (ulong composite = 2; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
        }
    }
}
