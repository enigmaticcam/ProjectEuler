using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem158 : ProblemBase {
        public override string ProblemName {
            get { return "158: Exploring strings for which only one character comes lexicographically after its neighbour to the left"; }
        }

        public override string GetAnswer() {
            FindAllCombosOf2(4);
            return BruteForce(4).ToString();
            return "";
        }

        private void FindAllCombosOf2(int maxLength) {
            var combo = new int[2];
            for (int num1 = 0; num1 <= 25; num1++) {
                combo[0] = num1;
                for (int num2 = num1 + 1; num2 <= 25; num2++) {
                    combo[1] = num2;
                    CountComboOf2(combo, maxLength);
                }
            }
        }

        private void CountComboOf2(int[] combo, int maxLength) {
            bool stop = true;
        }

        private ulong _count = 0;
        private ulong BruteForce(int maxLength) {
            var nums = new int[maxLength];
            Recursive(nums, 0, 0);
            return _count;
        }

        private ulong _total = 0;
        private PowerAll _powerOf2 = new PowerAll(2);
        private void Recursive(int[] nums, int index, ulong bitMask) {
            for (int num = 1; num <= 26; num++) {
                var bit = _powerOf2.GetPower(num);
                if ((bitMask & bit) == 0) {
                    nums[index] = num;
                    if (index < nums.Length - 1) {
                        Recursive(nums, index + 1, bitMask + bit);
                    } else {
                        if (IsGood(nums)) {
                            _count++;
                        }
                        _total++;
                    }
                }
                
            }
        }

        private bool IsGood(int[] nums) {
            bool isGood = false;
            for (int index = 0; index < nums.Length - 1; index++) {
                if (nums[index] < nums[index + 1]) {
                    if (!isGood) {
                        isGood = true;
                    } else {
                        return false;
                    }
                }
            }
            return isGood;
        }
    }
}
