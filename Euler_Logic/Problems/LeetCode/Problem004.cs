using System.Linq;

namespace Euler_Logic.Problems.LeetCode {
    public class Problem004 : ProblemBase {
        public override string ProblemName => "Leet Code 4: Median of Two Sorted Arrays";

        public override string GetAnswer() {
            return FindMedianSortedArrays(new int[] { 1, 3 }, new int[] { 2 }).ToString();
            //return FindMedianSortedArrays(new int[] { 1, 2 }, new int[] { 3, 4 }).ToString();
            //return FindMedianSortedArrays(new int[] { 1, 1 }, new int[] { 1, 2 }).ToString();
        }

        public double FindMedianSortedArrays(int[] nums1, int[] nums2) {
            var full = new int[nums1.Length + nums2.Length];
            int index1 = 0;
            int index2 = 0;
            for (int index = 0; index < full.Length; index++) {
                if (index1 < nums1.Length && index2 < nums2.Length) {
                    if (nums1[index1] < nums2[index2]) {
                        full[index] = nums1[index1];
                        index1++;
                    } else {
                        full[index] = nums2[index2];
                        index2++;
                    }
                } else if (index1 < nums1.Length) {
                    full[index] = nums1[index1];
                    index1++;
                } else {
                    full[index] = nums2[index2];
                    index2++;
                }
                if (index == full.Length / 2) {
                    if (full.Length % 2 == 0) {
                        return ((double)full[full.Length / 2 - 1] + (double)full[full.Length / 2]) / (double)2;
                    } else {
                        return (double)full[full.Length / 2];
                    }
                }
            }
            return 0;
        }
    }
}
