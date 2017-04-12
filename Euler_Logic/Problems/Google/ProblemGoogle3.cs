using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class ProblemGoogle3 : GoogleBase {
        public override string ProblemName {
            get { return "Google: Coin Jam"; }
        }

        public override string GetAnswer() {
            Solve();
            DownloadOutputFile();
            return "done";
        }

        //public override void UploadInputFile(string fileName) {
        //    _tests = new string[1];
        //    _tests[0] = "6 3";
        //}

        private void Solve() {
            foreach (string test in _tests) {
                string[] integers = test.Split(' ');
                FindJams(Convert.ToUInt64(integers[0]), Convert.ToUInt64(integers[1]));
            }
        }

        private void FindJams(ulong digitCount, ulong jamCountMax) {
            StringBuilder result = new StringBuilder();
            ulong num = (ulong)Math.Pow(2, digitCount - 1) + 1;
            ulong jamCount = 0;
            do {
                string isJamResult = IsJam(num);
                if (!string.IsNullOrEmpty(isJamResult)) {
                    jamCount++;
                    result.Append(isJamResult);
                }
                if (jamCount == jamCountMax) {
                    break;
                }
                num += 6;
            } while (true);
            _results.Add(result.ToString());
        }

        private string IsJam(ulong num) {
            StringBuilder result = new StringBuilder();
            for (ulong basePower = 2; basePower <= 10; basePower++) {
                ulong divisor = 0;
                divisor = IsPrime(ConvertNumToBase(num, basePower));
                if (divisor == 1) {
                    return "";
                }
                result.Append(" " + divisor);
            }

            StringBuilder finalResult = new StringBuilder();
            finalResult.AppendLine("");
            finalResult.Append(ConvertNumToBinary(num));
            finalResult.Append(" ");
            finalResult.Append(result.ToString());
            return finalResult.ToString();
        }

        private ulong ConvertNumToBase(ulong num, ulong newBase) {
            ulong powerBase = 0;
            ulong powerNum = 1;
            ulong newNum = 0;
            while (powerNum <= num) {
                if ((num & powerNum) != 0) {
                    newNum += (ulong)Math.Pow(newBase, powerBase);
                }
                powerBase++;
                powerNum = (ulong)Math.Pow(2, powerBase);
            }
            return newNum;
        }

        private string ConvertNumToBinary(ulong num) {
            ulong powerBase = 0;
            ulong powerNum = 1;
            StringBuilder binary = new StringBuilder();
            while (powerNum <= num) {
                if ((num & powerNum) != 0) {
                    binary.Insert(0, "1");
                } else {
                    binary.Insert(0, "0");
                }
                powerBase++;
                powerNum = (ulong)Math.Pow(2, powerBase);
            }
            return binary.ToString();
        }

        private ulong IsPrime(ulong num) {
            if (num == 1) {
                return 1;
            } else if (num == 2) {
                return 2;
            } else if (num % 2 == 0) {
                return 2;
            } else {
                ulong max = (ulong)Math.Sqrt(num);
                for (ulong composite = 3; composite <= max; composite += 2) {
                    if (num % composite == 0) {
                        return composite;
                    }
                }
            }
            return 1;
        }
    }
}
