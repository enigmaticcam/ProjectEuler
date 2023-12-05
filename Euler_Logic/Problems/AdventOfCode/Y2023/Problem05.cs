using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem05 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 5";

        public override string GetAnswer()
        {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
            return Answer2(Input()).ToString();
        }

        public long Answer1(List<string> input)
        {
            var tiers = GetTiers(input);
            OrderBy(tiers);
            var starters = GetStarters(input[0]);
            return FindLowest(tiers, starters);
        }
        public long Answer2(List<string> input)
        {
            var tiers = GetTiers(input);
            OrderBy(tiers);
            var ranges = GetRanges(input[0]);
            return FindLowestWithRanges(ranges, tiers);
        }

        private long FindLowestWithRanges(List<Range> ranges, List<Tier> tiers)
        {
            foreach (var tier in tiers)
            {
                var nextRanges = new List<Range>();
                foreach (var range in ranges)
                {
                    var splits = new List<Range>() { range };
                    foreach (var mapping in tier.Maps)
                    {
                        var nextSplits = new List<Range>();
                        foreach (var split in splits)
                        {
                            if (split.Start <= mapping.End && split.End >= mapping.Start)
                            {
                                Split(split, new Range() { Start = mapping.Start, End = mapping.End }, mapping.Offset, nextSplits, nextRanges);
                            }
                            else
                            {
                                nextSplits.Add(split);
                            }
                        }
                        splits = nextSplits;
                    }
                    nextRanges.AddRange(splits);
                }
                ranges = nextRanges;
            }
            return ranges
                .OrderBy(x => x.Start)
                .First()
                .Start;
        }

        private void Split(Range range, Range mapper, long overlapOffset, List<Range> unconverted, List<Range> converted)
        {
            if (range.Start < mapper.Start)
            {
                unconverted.Add(new Range()
                {
                    Start = range.Start,
                    End = mapper.End - 1
                });
            }
            converted.Add(new Range()
            {
                Start = Math.Max(range.Start, mapper.Start) + overlapOffset,
                End = Math.Min(range.End, mapper.End) + overlapOffset
            });
            if (range.End > mapper.End)
            {
                unconverted.Add(new Range()
                {
                    Start = mapper.End + 1,
                    End = range.End
                });
            }
        }

        private long FindLowest(List<Tier> tiers, List<long> starters)
        {
            long lowest = long.MaxValue;
            foreach (var starter in starters)
            {
                var next = GetLocation(starter, tiers);
                if (next < lowest)
                    lowest = next;
            }
            return lowest;
        }

        private long GetLocation(long seed, List<Tier> tiers)
        {
            foreach (var tier in tiers)
            {
                foreach (var mapping in tier.Maps)
                {
                    if (mapping.Start > seed)
                        break;
                    if (seed >= mapping.Start && seed <= mapping.End)
                    {
                        seed = seed + mapping.Offset;
                        break;
                    }
                }
            }
            return seed;
        }

        private List<long> GetStarters(string line)
        {
            return line.Split(' ')
                .Skip(1)
                .Select(x => Convert.ToInt64(x))
                .ToList();
        }

        private List<Range> GetRanges(string line)
        {
            var pairs = new List<Range>();
            var split = line.Split(' ');
            for (int index = 1; index < split.Length; index += 2)
            {
                var start = Convert.ToInt64(split[index]);
                var length = Convert.ToInt64(split[index + 1]);
                var end = start + length - 1;
                pairs.Add(new Range()
                {
                    Start = start,
                    End = end
                });
            }
            return pairs;
        }

        private void OrderBy(List<Tier> tiers)
        {
            foreach (var tier in tiers)
            {
                tier.Maps = tier.Maps
                    .OrderBy(x => x.Start)
                    .ToList();
            }
        }

        private List<Tier> GetTiers(List<string> input)
        {
            var tiers = new List<Tier>();
            int number = 1;
            var current = new Tier() { 
                Maps = new List<Mapping>(),
                Text = input[2],
                Number = 1
            };
            tiers.Add(current);
            for (int index = 3; index < input.Count; index++)
            {
                var line = input[index];
                if (line == "")
                {
                    number++;
                    current = new Tier()
                    {
                        Maps = new List<Mapping>(),
                        Number = number,
                        Text = input[index + 1]
                    };
                    tiers.Add(current);
                    index++;
                }
                else
                {
                    var split = line.Split(' ');
                    var source = Convert.ToInt64(split[1]);
                    var destination = Convert.ToInt64(split[0]);
                    var length = Convert.ToInt64(split[2]);
                    current.Maps.Add(new Mapping()
                    {
                        Start = source,
                        End = source + length - 1,
                        Offset = destination - source
                    });
                }
            }
            return tiers;
        }

        private class Tier
        {
            public int Number { get; set; }
            public string Text { get; set; }
            public List<Mapping> Maps { get; set; }
        }

        private class Mapping
        {
            public long Start { get; set; }
            public long End { get; set; }
            public long Offset { get; set; }
        }

        private class Range
        {
            public long Start { get; set; }
            public long End { get; set; }
        }
    }
}
