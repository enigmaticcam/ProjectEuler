using System.Text;

namespace Euler_Logic.Problems.LeetCode {
    public class Problem002 : ProblemBase {
        public override string ProblemName => "Leet Code 2: Add Two Numbers";

        public override string GetAnswer() {
            var l1 = ListNode.FromArray(new int[] { 2, 4, 3 });
            var l2 = ListNode.FromArray(new int[] { 5, 6, 4 });
            //var l1 = ListNode.FromArray(new int[] { 0 });
            //var l2 = ListNode.FromArray(new int[] { 0 });
            //var l1 = ListNode.FromArray(new int[] { 9, 9, 9, 9, 9, 9, 9 });
            //var l2 = ListNode.FromArray(new int[] { 9, 9, 9, 9 });
            return Solve(l1, l2);
        }

        private string Solve(ListNode l1, ListNode l2) {
            var result = AddTwoNumbers(l1, l2);
            var text = new StringBuilder();
            text.Append("[");
            bool firstTime = true;
            while (result != null) {
                if (firstTime) {
                    firstTime = false;
                } else {
                    text.Append(",");
                }
                text.Append(result.val);
                result = result.next;
            }
            text.Append("]");
            return text.ToString();
        }

        public ListNode AddTwoNumbers(ListNode l1, ListNode l2) {
            var result = new ListNode();
            var final = result;
            bool carryOver = false;
            while (l1 != null || l2 != null) {
                if (carryOver) {
                    result.val = 1;
                    carryOver = false;
                }
                if (l1 != null && l2 != null) {
                    result.val += l1.val + l2.val;
                    l1 = l1.next;
                    l2 = l2.next;
                } else if (l1 != null) {
                    result.val += l1.val;
                    l1 = l1.next;
                } else {
                    result.val += l2.val;
                    l2 = l2.next;
                }
                if (result.val > 9) {
                    carryOver = true;
                    result.val %= 10;
                }
                if (l1 != null || l2 != null) {
                    var next = new ListNode();
                    result.next = next;
                    result = next;
                } else if (carryOver) {
                    var next = new ListNode();
                    result.next = next;
                    next.val = 1;
                }
            }
            return final;
        }

        public class ListNode {
            public int val;
            public ListNode next;
            public ListNode(int val=0, ListNode next=null) {
                this.val = val;
                this.next = next;
            }

            public static ListNode FromArray(int[] nums) {
                ListNode node = null;
                for (int index = nums.Length - 1; index >= 0; index--) {
                    node = new ListNode(nums[index], node);
                }
                return node;
            }
        }
    }
}
