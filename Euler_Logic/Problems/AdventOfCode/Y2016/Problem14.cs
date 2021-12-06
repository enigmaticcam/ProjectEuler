using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem14 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2016: 14";

        public override string GetAnswer() {
            return Answer2("yjdafjpo").ToString();
        }

        private int Answer1(string input) {
            return Find64(input, false);
        }

        private int Answer2(string input) {
            return Find64(input, true);
        }

        private int Find64(string input, bool additionalLoop) {
            int keyNum = 0;
            var looking = new List<LookingFor>();
            var completed = new List<LookingFor>();
            do {
                string key = GetMD5(input + keyNum.ToString());
                if (additionalLoop) {
                    for (int count = 1; count <= 2016; count++) {
                        key = GetMD5(key);
                    }
                }
                bool doneWith3 = false;
                for (int index = 0; index < key.Length - 2; index++) {
                    if (key[index] == key[index + 1] && key[index] == key[index + 2]) {
                        if (!doneWith3) {
                            doneWith3 = true;
                            looking.Add(new LookingFor() { DigitToFind = key[index], StartNum = keyNum });
                        }
                        if (index < key.Length - 5 && key[index] == key[index + 3] && key[index] == key[index + 4]) {
                            for (int lookIndex = 0; lookIndex < looking.Count; lookIndex++) {
                                var nextLooking = looking[lookIndex];
                                if (keyNum - nextLooking.StartNum > 1000) {
                                    looking.RemoveAt(lookIndex);
                                    lookIndex--;
                                } else if (nextLooking.DigitToFind == key[index] && nextLooking.StartNum != keyNum) {
                                    completed.Add(nextLooking);
                                    completed = completed.OrderBy(x => x.StartNum).ToList();
                                    nextLooking.EndNum = keyNum;
                                    looking.RemoveAt(lookIndex);
                                    lookIndex--;
                                }
                            }
                        }
                    }
                }
                keyNum++;
            } while (completed.Count < 64 || keyNum > 1000 + completed[63].StartNum);
            return completed.OrderBy(x => x.StartNum).ElementAt(63).StartNum;
        }

        private string GetMD5(string key) {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(key);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++) {
                    sb.Append(hashBytes[i].ToString("X2").ToLower());
                }
                return sb.ToString();
            }
        }

        private class LookingFor {
            public int StartNum { get; set; }
            public int EndNum { get; set; }
            public char DigitToFind { get; set; }
        }
    }
}
