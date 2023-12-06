using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem06 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 6";

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
            var races = GetRaces(input);
            return GetSimpleBest(races);
        }
        private ulong Answer2(List<string> input)
        {
            var races = GetRaces(input);
            var race = Converge(races);
            return GetSimpleBest(race);
        }

        private Race Converge(List<Race> races)
        {
            string time = "";
            string distance = "";
            foreach (var next in races)
            {
                time += next.Time;
                distance += next.Distance;
            }
            return new Race()
            {
                Distance = Convert.ToUInt64(distance),
                Time = Convert.ToUInt64(time)
            };
        }

        private ulong GetSimpleBest(List<Race> races)
        {
            ulong final = 1;
            foreach (var race in races)
            {
                var best = GetSimpleBest(race);
                final *= best;
            }
            return final;
        }

        private ulong GetSimpleBest(Race race)
        {
            ulong beatCount = 0;
            for (ulong count = 1; count <= race.Time; count++)
            {
                ulong remaining = race.Time - count;
                ulong distance = remaining * count;
                if (distance > race.Distance)
                    beatCount++;
            }
            return beatCount;
        }

        private List<Race> GetRaces(List<string> input)
        {
            var times = RemoveExtraSpaces(input[0])
                .Split(' ')
                .Skip(1)
                .Select(x => Convert.ToUInt64(x))
                .ToList();
            var distances = RemoveExtraSpaces(input[1])
                .Split(' ')
                .Skip(1)
                .Select(x => Convert.ToUInt64(x))
                .ToList();
            return times
                .Select((x, i) => new Race()
                {
                    Distance = distances[i],
                    Time = x
                })
                .ToList();
        }

        private string RemoveExtraSpaces(string line)
        {
            int length;
            do
            {
                length = line.Length;
                line = line.Replace("  ", " ");
            } while (line.Length != length);
            return line;
        }

        private class Race
        {
            public ulong Time { get; set; }
            public ulong Distance { get; set; }
        }
    }
}
