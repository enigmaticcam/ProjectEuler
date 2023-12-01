using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem01 : AdventOfCodeBase
    {
        private List<Num> _nums;
        public override string ProblemName => "Advent of Code 2023: 1";

        public override string GetAnswer()
        {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input)
        {
            BuildNums();
            int sum = 0;
            foreach (var line in input)
            {
                var numbers = GetNumbersSpelt(line, false);
                var num = numbers.First() * 10 + numbers.Last();
                sum += num;
            }
            return sum;
        }

        private int Answer2(List<string> input)
        {
            BuildNums();
            int sum = 0;
            foreach (var line in input)
            {
                var numbers = GetNumbersSpelt(line, true);
                var num = numbers.First() * 10 + numbers.Last();
                sum += num;
            }
            return sum;
        }

        private void BuildNums()
        {
            _nums = new List<Num>()
            {
                new Num() { Number = 0, Text = "zero" },
                new Num() { Number = 1, Text = "one" },
                new Num() { Number = 2, Text = "two" },
                new Num() { Number = 3, Text = "three" },
                new Num() { Number = 4, Text = "four" },
                new Num() { Number = 5, Text = "five" },
                new Num() { Number = 6, Text = "six" },
                new Num() { Number = 7, Text = "seven" },
                new Num() { Number = 8, Text = "eight" },
                new Num() { Number = 9, Text = "nine" },
            };
        }

        private List<int> GetNumbersSpelt(string line, bool includeFullText)
        {
            var nums = new List<Num>();
            foreach (var num in _nums)
            {
                if (includeFullText)
                    nums.AddRange(FindByIndex(line, num, num.Text));
                nums.AddRange(FindByIndex(line, num, num.Number.ToString()));
            }
            return nums
                .Where(x => x.Index > -1)
                .OrderBy(x => x.Index)
                .Select(x => x.Number)
                .ToList();
        }

        private IEnumerable<Num> FindByIndex(string line, Num num, string pattern)
        {
            int index = line.IndexOf(pattern);
            while (index > -1)
            {
                yield return new Num() { Index = index, Number = num.Number };
                index = line.IndexOf(pattern, index + 1);
            }
        }

        private class Num
        {
            public int Number { get; set; }
            public string Text { get; set; }
            public int Index { get; set; }
        }
    }
}
