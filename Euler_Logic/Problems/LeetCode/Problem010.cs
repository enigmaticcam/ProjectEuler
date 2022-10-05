using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.LeetCode {
    public class Problem010 : LeetCodeBase {
        public override string ProblemName => "Leet Code 10: Regular Expression Matching";

        public override string GetAnswer() {
            Check(IsMatch("aa", "a"), false);
            Check(IsMatch("aa", "a*"), true);
            Check(IsMatch("ab", ".*"), true);
            Check(IsMatch("mississippi", "mis*is*ip*."), true);
            Check(IsMatch("aaa", "aaaa"), false);
            Check(IsMatch("a", "ab*a"), false);
            Check(IsMatch("a", ".*..a*"), false);
            return "";
        }

        public bool IsMatch(string s, string p) {
            return IsMatch(s, p, 0, 0);
        }

        private bool IsMatch(string s, string p, int sIndex, int pIndex) {
            for (; pIndex < p.Length; pIndex++) {
                bool isRepeater = pIndex < p.Length - 1 && p[pIndex + 1] == '*';
                if (p[pIndex] == '.') {
                    if (!isRepeater) {
                        if (sIndex > p.Length - 1) return false;
                    } else {
                        return FindRepeatBlank(s, p, sIndex, pIndex + 2);
                    }
                } else {
                    if (!isRepeater) {
                        if (sIndex == s.Length || s[sIndex] != p[pIndex]) return false;
                    } else {
                        return FindRepeatDigit(s, p, sIndex, pIndex + 2, p[pIndex]);
                    }
                }
                if (isRepeater) {
                    sIndex += 2;
                } else {
                    sIndex++;
                }
            }
            return sIndex == s.Length;
        }

        private bool FindRepeatDigit(string s, string p, int sIndex, int pIndex, char digit) {
            do {
                var result = IsMatch(s, p, sIndex, pIndex);
                if (result) return true;
                sIndex++;
                if (sIndex >= s.Length) break;
                if (s[sIndex - 1] != digit) return false;
            } while (true);
            return pIndex == p.Length;
        }

        private bool FindRepeatBlank(string s, string p, int sIndex, int pIndex) {
            do {
                var result = IsMatch(s, p, sIndex, pIndex);
                if (result) return true;
                sIndex++;
            } while (sIndex < s.Length);
            return pIndex == p.Length;
        }
    }
}
