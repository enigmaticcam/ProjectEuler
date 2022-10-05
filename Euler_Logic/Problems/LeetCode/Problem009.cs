namespace Euler_Logic.Problems.LeetCode {
    public class Problem009 : LeetCodeBase {
        public override string ProblemName => "Leet Code 9: Palindrome Number";

        public override string GetAnswer() {
            Check(IsPalindrome(121), true);
            Check(IsPalindrome(-121), false);
            Check(IsPalindrome(10), false);
            return "";
        }

        public bool IsPalindrome(int x) {
            if (x < 0) return false;
            var value = x.ToString();
            for (int index = 0; index < value.Length / 2; index++) {
                if (value[index] != value[value.Length - index - 1]) return false;
            }
            return true;
        }
    }
}
