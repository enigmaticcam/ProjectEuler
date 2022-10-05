using System;

namespace Euler_Logic.Problems.LeetCode {
    public abstract class LeetCodeBase : ProblemBase {
        public void Check(string result, string expected) {
            if (expected != result) throw new Exception($"{result} unexpected. Expected {expected}");
        }

        public void Check(int result, int expected) {
            if (expected != result) throw new Exception($"{result} unexpected. Expected {expected}");
        }

        public void Check(bool result, bool expected) {
            if (expected != result) throw new Exception($"{result} unexpected. Expected {expected}");
        }
    }
}
