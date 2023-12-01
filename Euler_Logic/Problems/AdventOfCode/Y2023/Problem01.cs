using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem01 : AdventOfCodeBase
    {
        private Dictionary<int, Num> _nums;
        public override string ProblemName => "Advent of Code 2023: 1";

        public override string GetAnswer()
        {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
            return Answer2(Input()).ToString();
        }

        private int Answer2(List<string> input)
        {
            BuildNums();
            int sum = 0;
            foreach (var line in input)
            {
                var numbers = GetNumbersSpelt(line);
                var num = numbers.First() * 10 + numbers.Last();
                sum += num;
            }
            return sum;
        }

        private int Answer1(List<string> input)
        {
            int sum = 0;
            foreach (var line in input)
            {
                var numbers = GetNumbers(line);
                var num = numbers.First() * 10 + numbers.Last();
                sum += num;
            }
            return sum;
        }

        private void BuildNums()
        {
            _nums = new Dictionary<int, Num>();
            _nums.Add(0, new Num()
            {
                Number = 0,
                Text = "zero"
            });
            _nums.Add(1, new Num()
            {
                Number = 1,
                Text = "one"
            });
            _nums.Add(2, new Num()
            {
                Number = 2,
                Text = "two"
            });
            _nums.Add(3, new Num()
            {
                Number = 3,
                Text = "three"
            });
            _nums.Add(4, new Num()
            {
                Number = 4,
                Text = "four"
            });
            _nums.Add(5, new Num()
            {
                Number = 5,
                Text = "five"
            });
            _nums.Add(6, new Num()
            {
                Number = 6,
                Text = "six"
            });
            _nums.Add(7, new Num()
            {
                Number = 7,
                Text = "seven"
            });
            _nums.Add(8, new Num()
            {
                Number = 8,
                Text = "eight"
            });
            _nums.Add(9, new Num()
            {
                Number = 9,
                Text = "nine"
            });
        }

        private List<int> GetNumbersSpelt(string line)
        {
            var nums = new List<Num>();
            foreach (var num in _nums)
            {
                var index = line.IndexOf(num.Value.Text);
                while (index > -1)
                {
                    nums.Add(new Num()
                    {
                        Index = index,
                        Number = num.Value.Number
                    });
                    index = line.IndexOf(num.Value.Text, index + 1);
                }
                index = line.IndexOf(num.Value.Number.ToString());
                while (index > -1)
                {
                    nums.Add(new Num()
                    {
                        Index = index,
                        Number = num.Value.Number
                    });
                    index = line.IndexOf(num.Value.Number.ToString(), index + 1);
                }
            }
            return nums
                .Where(x => x.Index > -1)
                .OrderBy(x => x.Index)
                .Select(x => x.Number)
                .ToList();
        }

        private List<int> GetNumbers(string line)
        {
            var list = new List<int>();
            foreach (var digit in line)
            {
                switch (digit)
                {
                    case '0': 
                        list.Add(0);
                        break;
                    case '1':
                        list.Add(1);
                        break;
                    case '2':
                        list.Add(2);
                        break;
                    case '3':
                        list.Add(3);
                        break;
                    case '4':
                        list.Add(4);
                        break;
                    case '5':
                        list.Add(5);
                        break;
                    case '6':
                        list.Add(6);
                        break;
                    case '7':
                        list.Add(7);
                        break;
                    case '8':
                        list.Add(8);
                        break;
                    case '9':
                        list.Add(9);
                        break;
                }
            }
            return list;
        }

        private class Num
        {
            public int Number { get; set; }
            public string Text { get; set; }
            public int Index { get; set; }
        }
    }
}
