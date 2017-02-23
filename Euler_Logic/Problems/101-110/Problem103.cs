using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem103 : IProblem {
        private List<SubsetCompare> _subsetsToCompare;
        private List<int> _currentList;
        private List<List<int>> _start = new List<List<int>>();

        public string ProblemName {
            get { return "103: Special subset sums: optimum"; }
        }

        public string GetAnswer() {
            int digits = 7;
            BuildStarts();
            BuildSubsetsToCompare(digits);
            Solve(digits);
            return GetSolveKey();
        }

        private void Solve(int digits) {
            _currentList = Initialize(digits);
            IncreaseDigits(digits - 1, int.MaxValue);
        }

        private bool IncreaseDigits(int digit, int max) {
            for (int increase = 1; _currentList[digit] + 1 <= max; increase++) {
                _currentList[digit]++;
                for (int resetDigit = 0; resetDigit < digit; resetDigit++) {
                    _currentList[resetDigit] = _start[_currentList.Count - 2][resetDigit];
                }
                if (IsValid(_currentList)) {
                    return true;
                } else if (digit >= 1) {
                    if (IncreaseDigits(digit - 1, _currentList[digit] - 1) == true) {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<int> Initialize(int digits) {
            List<int> list = new List<int>();
            for (int num = 1; num < digits; num++) {
                list.Add(_start[digits - 2][num - 1]);
            }
            list.Add(list[digits - 2] + 1);
            return list;
        }

        private string GetSolveKey() {
            StringBuilder key = new StringBuilder();
            foreach (int num in _currentList) {
                key.Append(num);
            }
            return key.ToString();
        }

        private void BuildSubsetsToCompare(int n) {
            _subsetsToCompare = new List<SubsetCompare>();
            uint max = (uint)Math.Pow(2, n) - 1;
            for (uint num1 = 1; num1 <= max; num1++) {
                for (uint num2 = num1 + 1; num2 <= max; num2++) {
                    if ((num1 & num2) == 0) {
                        SubsetCompare compare = new SubsetCompare(BuildSubset(num1), BuildSubset(num2));
                        if (compare.Subset1.Count > 1 || compare.Subset2.Count > 1) {
                            _subsetsToCompare.Add(compare);
                        }
                    }
                }
            }
        }

        private List<int> BuildSubset(uint num) {
            List<int> subset = new List<int>();
            int power = 0;
            uint number = 1;
            do {
                if ((number & num) == number) {
                    subset.Add(power);
                }
                power++;
                number = (uint)Math.Pow(2, power);
            } while (number <= num);
            return subset;
        }

        private bool IsValid(List<int> set) {
            foreach (SubsetCompare compare in _subsetsToCompare) {
                int sum1 = 0;
                int sum2 = 0;
                foreach (int index in compare.Subset1) {
                    sum1 += set[index];
                }
                foreach (int index in compare.Subset2) {
                    sum2 += set[index];
                }
                if (sum1 == sum2) {
                    return false;
                } else if (compare.Subset1.Count > compare.Subset2.Count && sum1 <= sum2) {
                    return false;
                } else if (compare.Subset2.Count > compare.Subset1.Count && sum2 <= sum1) {
                    return false;
                }
            }
            return true;
        }

        private void BuildStarts() {
            _start.Add(new List<int> { 1 });
            _start.Add(new List<int> { 1, 2 });
            _start.Add(new List<int> { 2, 3, 4 });
            _start.Add(new List<int> { 3, 5, 6, 7 });
            _start.Add(new List<int> { 6, 9, 11, 12, 13 });
            _start.Add(new List<int> { 11, 18, 19, 20, 22, 26 });
        }

        private bool PerformTest() {
            BuildSubsetsToCompare(1);
            if (!IsValid(new List<int> { 1 })) {
                throw new Exception("Test failed");
            }
            BuildSubsetsToCompare(2);
            if (!IsValid(new List<int> { 1, 2 })) {
                throw new Exception("Test failed");
            }
            BuildSubsetsToCompare(3);
            if (!IsValid(new List<int> { 2, 3, 4 })) {
                throw new Exception("Test failed");
            }
            BuildSubsetsToCompare(4);
            if (!IsValid(new List<int> { 3, 5, 6, 7 })) {
                throw new Exception("Test failed");
            }
            BuildSubsetsToCompare(5);
            if (!IsValid(new List<int> { 6, 9, 11, 12, 13 })) {
                throw new Exception("Test failed");
            }
            BuildSubsetsToCompare(6);
            if (!IsValid(new List<int> { 11, 18, 19, 20, 22, 25 })) {
                throw new Exception("Test failed");
            }
            BuildSubsetsToCompare(6);
            if (IsValid(new List<int> { 11, 16, 19, 21, 22, 23 })) {
                throw new Exception("Test failed");
            }
            BuildSubsetsToCompare(7);
            if (IsValid(new List<int> { 19, 31, 37, 38, 39, 41, 44 })) {
                throw new Exception("Test failed");
            }
            BuildSubsetsToCompare(4);
            if (IsValid(new List<int> { 2, 3, 4, 8 })) {
                throw new Exception("Test failed");
            }
            BuildSubsetsToCompare(4);
            if (IsValid(new List<int> { 1, 2, 3, 5 })) {
                throw new Exception("Test failed");
            }
            BuildSubsetsToCompare(5);
            if (IsValid(new List<int> { 7, 8, 9, 11, 14 })) {
                throw new Exception("Test failed");
            }
            return true;
        }

        private class SubsetCompare {
            public List<int> Subset1 { get; set; }
            public List<int> Subset2 { get; set; }

            public SubsetCompare(List<int> subset1, List<int> subset2) {
                this.Subset1 = subset1;
                this.Subset2 = subset2;
            }

            public override string ToString() {
                StringBuilder text = new StringBuilder();
                text.Append(SubsetToString(Subset1));
                text.Append(":");
                text.Append(SubsetToString(Subset2));
                return text.ToString();
            }

            private StringBuilder SubsetToString(List<int> subset) {
                StringBuilder text = new StringBuilder();
                bool firstTime = true;
                foreach (int num in subset) {
                    if (firstTime) {
                        firstTime = false;
                    } else {
                        text.Append(",");
                    }
                    text.Append(num);
                }
                return text;
            }
        }
    }
}
