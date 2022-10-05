using System.Collections.Generic;

namespace Euler_Logic.Problems.LeetCode {
    public class Problem003 : ProblemBase {
        public override string ProblemName => "Leet Code 3: Longest Substring Without Repeating Characters";

        public override string GetAnswer() {
            return LengthOfLongestSubstring("abcabcbb").ToString();
            //return LengthOfLongestSubstring("bbbbb").ToString();
            //return LengthOfLongestSubstring("pwwkew").ToString();
            //return LengthOfLongestSubstring("aab").ToString();
        }

        public int LengthOfLongestSubstring(string s) {
            var hash = new HashSet<char>();
            int maxLength = 0;
            int currentLength = 0;
            for (int startIndex = 0; startIndex < s.Length; startIndex++) {
                hash.Clear();
                currentLength = 0;
                for (int index = startIndex; index < s.Length; index++) {
                    var digit = s[index];
                    if (hash.Contains(digit)) break;
                    currentLength++;
                    if (currentLength > maxLength) maxLength++;
                    hash.Add(digit);
                }
            }
            return maxLength;
        }
    }
}
