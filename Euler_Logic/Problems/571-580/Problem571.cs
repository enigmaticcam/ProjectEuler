using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem571 : ProblemBase {
        private Power _power = new Power();

        public override string ProblemName {
            get { return "571: Super Pandigital Numbers"; }
        }

        public override string GetAnswer() {
            ulong max = 12;
            BuildGoodBits(max - 1);
            FindPermutations(0, max - 1, max - 1, max, 0);
            return Solve().ToString();
        }

        private ulong Solve() {
            ulong sum = 0;
            _nums.ForEach(x => sum += x);
            return sum;
        }

        private bool FindPermutations(ulong bits, ulong remainder, ulong maxPower, ulong baseNum, ulong num) {
            ulong start = (remainder == maxPower ? (ulong)1 : 0);
            for (ulong power = start; power <= maxPower; power++) {
                ulong bit = _power.GetPower(2, power);
                if ((bits & bit) == 0) {
                    bits += bit;
                    var add = _power.GetPower(baseNum, remainder) * power;
                    num += add;
                    if (remainder >= 1) {
                        if (FindPermutations(bits, remainder - 1, maxPower, baseNum, num)) {
                            return true;
                        }
                    } else {
                        if (CalcNum(baseNum, num)) {
                            return true;
                        }
                    }
                    num -= add;
                    bits -= bit;
                }
            }
            return false;
        }

        private List<ulong> _nums = new List<ulong>();
        private bool CalcNum(ulong baseNum, ulong num) {
            bool isGood = true;
            for (ulong newBase = baseNum - 1; newBase >= 5; newBase--) {
                if (!IsGood(newBase, num)) {
                    isGood = false;
                    break;
                }
            }
            if (isGood) {
                _nums.Add(num);
                if (_nums.Count == 10) {
                    return true;
                }
            }
            return false;
        }

        private bool IsGood(ulong baseNum, ulong num) {
            ulong digits = (ulong)Math.Log(num, baseNum);
            if (digits < baseNum) {
                return false;
            }
            ulong powerBase = _power.GetPower(baseNum, digits);
            ulong bits = 0;
            for (int power = (int)digits; power >= 0; power--) {
                ulong digit = num / powerBase;
                ulong bit = _power.GetPower(2, digit);
                if ((bits & bit) == 0) {
                    bits += bit;
                }
                num -= digit * powerBase;
                powerBase /= baseNum;
            }
            return bits == _goodBits[baseNum];
        }

        private Dictionary<ulong, ulong> _goodBits = new Dictionary<ulong, ulong>();
        private void BuildGoodBits(ulong max) {
            for (ulong baseNum = 3; baseNum <= max; baseNum++) {
                _goodBits.Add(baseNum, _power.GetPower(2, baseNum) - 1);
            }
        }
    }
}