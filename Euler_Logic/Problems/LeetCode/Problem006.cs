using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.LeetCode {
    public class Problem006 : LeetCodeBase {
        public override string ProblemName => "Leet Code 6: Zigzag Conversion";

        public override string GetAnswer() {
            Check(Convert("PAYPALISHIRING", 3), "PAHNAPLSIIGYIR");
            Check(Convert("PAYPALISHIRING", 4), "PINALSIGYAHRPI");
            Check("AB", Convert("AB", 1));
            return "";
        }

        public string Convert(string s, int numRows) {
            var text = new StringBuilder[numRows];
            for (int index = 0; index < numRows; index++) {
                text[index] = new StringBuilder();
            }
            int digitIndex = 0;
            int x = 0;
            int direction = -1;
            while (digitIndex < s.Length) {
                text[x].Append(s[digitIndex]);
                if (x == 0 || x == numRows - 1) direction *= -1;
                if (numRows > 1) x += direction;
                digitIndex++;
            }
            var final = new StringBuilder();
            foreach (var sub in text) {
                final.Append(sub.ToString());
            }
            return final.ToString();
        }
    }
}
