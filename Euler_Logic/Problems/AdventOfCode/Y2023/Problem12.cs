using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem12 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 12";

        public override string GetAnswer()
        {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input)
        {
            var groups = GetGroups(input);
            return SumAll(groups);
        }
        private ulong Answer2(List<string> input)
        {
            input = CreateCopies(input);
            var groups = GetGroups(input);
            return SumAll(groups);
        }

        private List<string> CreateCopies(List<string> input)
        {
            var newInput = new List<string>();
            foreach (var line in input)
            {
                var split = line.Split(' ');
                var copy0 = string.Join("?", Enumerable.Repeat(split[0], 5));
                var copy1 = string.Join(",", Enumerable.Repeat(split[1], 5));
                newInput.Add($"{copy0} {copy1}");
            }
            return newInput;
        }

        private ulong SumAll(List<Group> groups)
        {
            ulong sum = 0;
            int count = 0;
            DP(groups.Last());
            foreach (var group in groups)
            {
                count++;
                var next = DP(group);
                sum += next;
            }
            return sum;
        }

        private ulong DP(Group group)
        {
            var data = new ulong[group.Spring.Length];
            for (int setIndex = group.Sets.Count - 1; setIndex >= 0; setIndex--)
            {
                var set = group.Sets[setIndex];
                var subData = new ulong[group.Spring.Length];
                for (int length = 1; length <= group.Spring.Length; length++)
                {
                    for (int index = group.Spring.Length - 1; index >= group.Spring.Length - length; index--)
                    {
                        
                        if (group.Spring.Length - index >= set)
                        {
                            var rangeStart = group.Spring.Length - length;
                            var rangeEnd = index + set;
                            if (setIndex == 0)
                            {
                                rangeStart = 0;
                            }
                            else if (setIndex == group.Sets.Count - 1)
                            {
                                rangeEnd = group.Spring.Length - 1;
                            }
                            if (rangeEnd < group.Spring.Length && IsGood(group.Spring, set, rangeStart, rangeEnd, index))
                            {
                                if (setIndex == group.Sets.Count - 1)
                                {
                                    subData[group.Spring.Length - length] += 1;
                                }
                                else if (index + set + 1 < group.Spring.Length)
                                {
                                    subData[group.Spring.Length - length] += data[index + set + 1];
                                }
                            }
                        }
                    }
                }
                data = subData;
            }
            return data[0];
        }

        private bool IsGood(string spring, int setLength, int rangeStart, int rangeEnd, int setStart)
        {
            for (int index = rangeStart; index <= rangeEnd; index++)
            {
                if (index < setStart)
                {
                    if (spring[index] == '#')
                        return false;
                }
                else if (index > setStart + setLength - 1)
                {
                    if (spring[index] == '#')
                        return false;
                }
                else if (spring[index] == '.')
                {
                    return false;
                }
            }
            return true;
        }

        private List<Group> GetGroups(List<string> input)
        {
            var groups = new List<Group>();
            foreach (var line in input)
            {
                var group = new Group();
                groups.Add(group);
                var split = line.Split(' ');
                group.Spring = split[0];
                group.Sets = split[1]
                    .Split(',')
                    .Select(x => Convert.ToInt32(x))
                    .ToList();
            }
            return groups;
        }

        private class Group
        {
            public string Spring { get; set; }
            public List<int> Sets { get; set; }
            public ulong PossibilitiesCount { get; set; }
        }
    }
}
