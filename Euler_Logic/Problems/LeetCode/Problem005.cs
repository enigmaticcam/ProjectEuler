namespace Euler_Logic.Problems.LeetCode {
    public class Problem005 : LeetCodeBase {
        public override string ProblemName => "Leet Code 5: Longest Palindromic Substring";

        public override string GetAnswer() {
            Check(LongestPalindrome("babad"), "bab");
            Check(LongestPalindrome("bb"), "bb");
            Check(LongestPalindrome("a"), "a");
            Check(LongestPalindrome("aacabdkacaa"), "aca");
            return "";
        }

        private string LongestPalindrome(string s) {
            int bestLength = 1;
            int bestStart = 0;
            for (int index = 0; index < s.Length; index++) {
                for (int next = index + 1; next < s.Length; next++) {
                    if (s[index] == s[next] && (next - index + 1) > bestLength && IsPalindrome(s, index + 1, next - 1)) {
                        bestLength = next - index + 1;
                        bestStart = index;
                    }
                }
            }
            return s.Substring(bestStart, bestLength);
        }

        private bool IsPalindrome(string s, int start, int end) {
            while (start <= end) {
                if (s[start] != s[end]) return false;
                start++;
                end--;
            }
            return true;
        }
    }
}
