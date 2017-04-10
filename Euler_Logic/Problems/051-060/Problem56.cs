using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem56 : ProblemBase {
        public override string ProblemName {
            get { return "56: Powerful digit sum"; }
        }

        public override string GetAnswer() {
            return BestDigitSum().ToString();
        }

        public int BestDigitSum() {
            int bestDigitSum = 0;
            for (int a = 2; a < 100; a++) {
                List<int> aList = new List<int>();
                string aAsSting = a.ToString();
                for (int i = aAsSting.Length - 1; i >= 0; i--) {
                    aList.Add(Convert.ToInt32(aAsSting.Substring(i, 1)));
                }

                List<int> finalNum = aList;
                for (int b = 1; b < 100; b++) {
                    finalNum = Multiply(finalNum, aList);

                    int digitSum = 0;
                    foreach (int num in finalNum) {
                        digitSum += num;
                    }
                    if (digitSum > bestDigitSum) {
                        bestDigitSum = digitSum;
                    }
                }
            }

            return bestDigitSum;
        }

        private string convertListToString(List<int> number) {
            string returnString = "";
            foreach (int num in number) {
                returnString = num.ToString() + returnString;
            }
            return returnString;
        }

        private List<int> Multiply(List<int> product1, List<int> product2) {
            // Multiple digit by digit
            List<List<int>> adds = new List<List<int>>();
            for (int i = 0; i < product1.Count; i++) {
                List<int> add = new List<int>();
                for (int k = 0; k < i; k++) {
                    add.Add(0);
                }
                int remainder = 0;
                for (int j = 0; j < product2.Count; j++) {
                    int prod = product1[i] * product2[j] + remainder;
                    if (prod > 9) {
                        string prodAsString = prod.ToString();
                        prod = Convert.ToInt32(prodAsString.Substring(1, 1));
                        remainder = Convert.ToInt32(prodAsString.Substring(0, 1));
                    }
                    add.Add(prod);
                }
                if (remainder > 0) {
                    add.Add(remainder);
                }
                adds.Add(add);
            }

            List<int> finalReturn = new List<int>();
            foreach (int num in adds[0]) {
                finalReturn.Add(num);
            }
            for (int i = 1; i < adds.Count; i++) {
                int digitCount = Math.Max(adds[i].Count, finalReturn.Count);
                int remainder = 0;
                for (int j = 0; j < digitCount; j++) {
                    int sum = 0;
                    if (adds[i].Count - 1 < j) {
                        sum = finalReturn[j] + remainder;
                    } else if (finalReturn.Count - 1 < j) {
                        sum = adds[i][j] + remainder;
                        finalReturn.Add(0);
                    } else {
                        sum = adds[i][j] + finalReturn[j] + remainder;
                    }
                    if (sum > 9) {
                        string sumAsString = sum.ToString();
                        finalReturn[j] = Convert.ToInt32(sumAsString.Substring(1, 1));
                        remainder = Convert.ToInt32(sumAsString.Substring(0, 1));
                    } else {
                        finalReturn[j] = sum;
                        remainder = 0;
                    }
                }
                if (remainder > 0) {
                    finalReturn.Add(remainder);
                }
            }

            return finalReturn;
        }

        private List<int> Add(List<int> num1, List<int> num2) {
            List<int> finalReturn = new List<int>();

            int digitCount = Math.Max(num1.Count, num2.Count);
            int remainder = 0;
            for (int i = 0; i < digitCount; i++) {
                int sum = 0;
                if (num1.Count - 1 < i) {
                    sum = num2[i] + remainder;
                } else if (num2.Count - 1 < i) {
                    sum = num1[i] + remainder;
                } else {
                    sum = num1[i] + num2[i] + remainder;
                }
                if (sum > 9) {
                    string sumAsString = sum.ToString();
                    finalReturn.Add(Convert.ToInt32(sumAsString.Substring(1, 1)));
                    remainder = Convert.ToInt32(sumAsString.Substring(0, 1));
                }
            }
            if (remainder > 0) {
                finalReturn.Add(remainder);
            }
            return finalReturn;
        }
    }
}
