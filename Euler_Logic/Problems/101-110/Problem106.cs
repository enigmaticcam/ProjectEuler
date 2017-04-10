using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem106 : ProblemBase {
        private List<SubsetCompare> _subsetsToCompare = new List<SubsetCompare>();

        public override string ProblemName {
            get { return "106: Special subset sums: meta-testing"; }
        }

        public override string GetAnswer() {
            BuildSubsetsToCompare(7);
            return Solve().ToString();
        }

        private void BuildSubsetsToCompare(int n) {
            _subsetsToCompare = new List<SubsetCompare>();
            uint max = (uint)Math.Pow(2, n) - 1;
            for (uint num1 = 1; num1 <= max; num1++) {
                for (uint num2 = num1 + 1; num2 <= max; num2++) {
                    if ((num1 & num2) == 0) {
                        SubsetCompare compare = new SubsetCompare(BuildSubset(num1), BuildSubset(num2));
                        _subsetsToCompare.Add(compare);
                    }
                }
            }
        }

        private int Solve() {
            int sum = 0;
            StringBuilder test = new StringBuilder();
            foreach (SubsetCompare compare in _subsetsToCompare) {
                if (compare.Subset1.SubsetList.Count == 1 && compare.Subset2.SubsetList.Count == 1) {
                    // do nothing
                } else if (compare.Subset1.SubsetList.Count != compare.Subset2.SubsetList.Count) {
                    // do nothing
                } else {
                    int diff = 0;
                    for (int index = 0; index < compare.Subset1.SubsetList.Count; index++) {
                        if (compare.Subset1.SubsetList[index] > compare.Subset2.SubsetList[index]) {
                            diff++;
                        } else {
                            diff--;
                        }
                    }
                    if (diff == 0) {
                        sum++;
                    }
                }
                test.AppendLine(compare.ToString());
            }
            return sum;
        }

        private Subset BuildSubset(uint num) {
            List<int> subset = new List<int>();
            int power = 0;
            uint number = 1;
            int sum = 0;
            do {
                if ((number & num) == number) {
                    subset.Add(power);
                    sum += power;
                }
                power++;
                number = (uint)Math.Pow(2, power);
            } while (number <= num);
            return new Subset(subset, sum);
        }

        private class Subset {
            public List<int> SubsetList { get; set; }
            public int SubsetSum { get; set; }

            public Subset(List<int> subsetList, int subsetSum) {
                this.SubsetList = subsetList;
                this.SubsetSum = subsetSum;
            }
        }

        private class SubsetCompare {
            public Subset Subset1 { get; set; }
            public Subset Subset2 { get; set; }

            public SubsetCompare(Subset subset1, Subset subset2) {
                this.Subset1 = subset1;
                this.Subset2 = subset2;
            }

            public override string ToString() {
                StringBuilder text = new StringBuilder();
                text.Append(SubsetToString(Subset1.SubsetList));
                text.Append(":");
                text.Append(SubsetToString(Subset2.SubsetList));
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
