using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.LeetCode {
    public class Problem008 : LeetCodeBase {
        public override string ProblemName => "Leet Code 8: String to Integer (atoi)";

        public override string GetAnswer() {
            Check(MyAtoi("42"), 42);
            Check(MyAtoi("0032"), 32);
            Check(MyAtoi("   -42"), -42);
            Check(MyAtoi("4193 with words"), 4193);
            Check(MyAtoi("1"), 1);
            Check(MyAtoi(" "), 0);
            Check(MyAtoi("9223372036854775808"), 2147483647);
            Check(MyAtoi("  0000000000012345678"), 12345678);
            return "";
        }

        public int MyAtoi(string s) {
            s = s.Trim();
            if (s.Length == 0) return 0;
            int maxDigit = (int)Math.Log10(int.MaxValue) + 2;
            long value = 0;
            bool isNegative = false;
            int index = 0;
            if (s[0] == '-') isNegative = true;
            if (s[0] == '-') {
                isNegative = true;
                index = 1;
            } else if (s[0] == '+') {
                index = 1;
            }
            bool finished = false;
            int digitCount = 0;
            while (index < s.Length && ! finished && digitCount <= maxDigit) {
                var digit = s[index];
                switch (digit) {
                    case '0':
                        value *= 10;
                        if (value != 0) digitCount++;
                        break;
                    case '1':
                        value = value * 10 + 1;
                        digitCount++;
                        break;
                    case '2':
                        value = value * 10 + 2;
                        digitCount++;
                        break;
                    case '3':
                        value = value * 10 + 3;
                        digitCount++;
                        break;
                    case '4':
                        value = value * 10 + 4;
                        digitCount++;
                        break;
                    case '5':
                        value = value * 10 + 5;
                        digitCount++;
                        break;
                    case '6':
                        value = value * 10 + 6;
                        digitCount++;
                        break;
                    case '7':
                        value = value * 10 + 7;
                        digitCount++;
                        break;
                    case '8':
                        value = value * 10 + 8;
                        digitCount++;
                        break;
                    case '9':
                        value = value * 10 + 9;
                        digitCount++;
                        break;
                    default:
                        finished = true;
                        break;
                }
                index++;
            }
            if (isNegative) value *= -1;
            if (value > (long)int.MaxValue) value = (long)int.MaxValue;
            if (value < (long)int.MinValue) value = (long)int.MinValue;
            return (int)value;
        }
    }
}
