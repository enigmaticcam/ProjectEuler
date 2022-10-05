namespace Euler_Logic.Problems.LeetCode {
    public class Problem001 : ProblemBase {
        public override string ProblemName => "Leet Code 1: Two Sum";

        public override string GetAnswer() {
            //return Solve(new int[] { 2, 7, 11, 15 }, 9);
            return Solve(new int[] { 3,2,4 }, 6);
            //return Solve(new int[] { 3, 3 }, 6);
        }

        private string Solve(int[] nums, int target) {
            var result = TwoSum(nums, target);
            return $"[{result[0]},{result[1]}]";
        }

        private int[] TwoSum(int[] nums, int target) {
            for (int index1 = 0; index1 < nums.Length; index1++) {
                var num1 = nums[index1];
                for (int index2 = index1 + 1; index2 < nums.Length; index2++) {
                    var num2 = nums[index2];
                    if ((long)num1 + (long)num2 == (long)target) return new int[2] { index1, index2 };
                }
            }
            return null;
        }
    }
}
