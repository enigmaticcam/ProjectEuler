using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem336 : ProblemBase {
        private int[] _tempNums;
        private Dictionary<ulong, string> _numToAlpha = new Dictionary<ulong, string>();
        private int _count = 0;
        private string _found = "";

        /*
            I bruteforce this. Starting with the lowest possible position (A, B, C, etc), 
            I find all subsequent permutations. I use bitwise notation to find these 
            permutations. For each permutation, I calculate the number of steps it takes 
            to get it in the correct order. When I find the 2011th instance of the highest, 
            I stop and return it. The highest is the number of cars * 2 - 3, which is 19 
            with 11 cars.
         */

        public override string ProblemName {
            get { return "336: Maximix Arrangements"; }
        }

        public override string GetAnswer() {
            int max = 11;
            _tempNums = new int[max];
            BuildNumToAlpha();
            Solve(0, 0, new ulong[max], (ulong)max, 2011);
            return _found;
        }

        private void BuildNumToAlpha() {
            _numToAlpha.Add(0, "A");
            _numToAlpha.Add(1, "B");
            _numToAlpha.Add(2, "C");
            _numToAlpha.Add(3, "D");
            _numToAlpha.Add(4, "E");
            _numToAlpha.Add(5, "F");
            _numToAlpha.Add(6, "G");
            _numToAlpha.Add(7, "H");
            _numToAlpha.Add(8, "I");
            _numToAlpha.Add(9, "J");
            _numToAlpha.Add(10, "K");
        }

        private bool Solve(ulong bits, int index, ulong[] nums, ulong max, int lookingFor) {
            for (ulong powerOf2 = 0; powerOf2 < max; powerOf2++) {
                ulong bit = (ulong)Math.Pow(2, powerOf2);
                if ((bits & bit) == 0) {
                    bits += bit;
                    nums[index] = powerOf2;
                    if (index < nums.Length - 1) {
                        if (Solve(bits, index + 1, nums, max, lookingFor)) {
                            return true;
                        }
                    } else {
                        for (int subIndex = 0; subIndex < nums.Length; subIndex++) {
                            _tempNums[subIndex] = (int)nums[subIndex];
                        }
                        int count = CalcPermutation(_tempNums);
                        if (count == nums.Length * 2 - 3) {
                            _count++;
                            if (_count == lookingFor) {
                                _found = string.Join("", nums.Select(x => _numToAlpha[x]));
                                return true;
                            }
                        }
                    }
                    bits -= bit;
                }
            }
            return false;
        }

        private int CalcPermutation(int[] nums) {
            int total = 0;
            for (int index = 0; index < nums.Length; index++) {
                if (nums[index] != index) {
                    if (nums[nums.Length - 1] != index) {
                        for (int next = index + 1; next < nums.Length; next++) {
                            if (nums[next] == index) {
                                Reverse(nums, next);
                                total++;
                                break;
                            }
                        }
                    }
                    if (nums[index] != index) {
                        Reverse(nums, index);
                        total++;
                    }
                }
            }
            return total;
        }

        private void Reverse(int[] nums, int start) {
            int stop = (nums.Length - start - 1) / 2 + start;
            for (int swapA = start; swapA <= stop; swapA++) {
                int swapB = nums.Length - 1 - (swapA - start);
                int temp = nums[swapA];
                nums[swapA] = nums[swapB];
                nums[swapB] = temp;
            }
        }
    }
}