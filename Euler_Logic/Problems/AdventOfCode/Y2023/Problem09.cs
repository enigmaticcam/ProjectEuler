using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem09 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 9";

        public override string GetAnswer()
        {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
            return Answer2(Input()).ToString();
        }

        private long Answer1(List<string> input)
        {
            return CalcSumAll(input, false);
        }

        private long Answer2(List<string> input)
        {
            return CalcSumAll(input, true);
        }

        private long CalcSumAll(List<string> input, bool reverse)
        {
            long total = 0;
            foreach (var line in input)
            {
                var split = line.Split(' ')
                    .Select(x => Convert.ToInt64(x))
                    .ToList();
                if (reverse)
                    split.Reverse();
                var next = CalcNext(split);
                total += next;
            }
            return total;
        }

        private long CalcNext(List<long> nums)
        {
            if (nums.Count == 0)
                return 0;
            var next = new List<long>();
            for (int index = 1; index < nums.Count; index++)
            {
                next.Add(nums[index] - nums[index - 1]);
            }
            return nums.Last() + CalcNext(next);
        }
    }
}
