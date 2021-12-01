using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem04 : ProblemBase {
        public override string ProblemName {
            get { return "Advent of Code 2015: 4"; }
        }

        public override string GetAnswer() {
            return Answer2();
        }

        public string Answer1() {
            int num = 0;
            do {
                string md5 = CreateMD5(this.Input + num.ToString());
                if (md5.Substring(0, 5) == "00000") {
                    return num.ToString();
                }
                num++;
            } while (true);
        }

        public string Answer2() {
            int num = 0;
            do {
                string md5 = CreateMD5(this.Input + num.ToString());
                if (md5.Substring(0, 6) == "000000") {
                    return num.ToString();
                }
                num++;
            } while (true);
        }

        public string Input {
            get { return "ckczppom"; }
        }

        public string CreateMD5(string input) {

            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++) {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
