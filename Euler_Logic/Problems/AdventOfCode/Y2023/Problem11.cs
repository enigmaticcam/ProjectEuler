using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem11 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 11";

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
            var galaxies = GetGalaxies(input);
            Expand(galaxies, x => x.X, (x, v) => x.X = v, 1);
            Expand(galaxies, x => x.Y, (x, v) => x.Y = v, 1);
            return GetSumAll(galaxies);
        }

        private long Answer2(List<string> input)
        {
            var galaxies = GetGalaxies(input);
            Expand(galaxies, x => x.X, (x, v) => x.X = v, 999999);
            Expand(galaxies, x => x.Y, (x, v) => x.Y = v, 999999);
            return GetSumAll(galaxies);
        }

        private long GetSumAll(List<Galaxy> galaxies)
        {
            long sum = 0;
            for (int index1 = 0; index1 < galaxies.Count; index1++)
            {
                var g1 = galaxies[index1];
                for (int index2 = index1 + 1; index2 < galaxies.Count; index2++)
                {
                    var g2 = galaxies[index2];
                    long distance = GetShortestPath(g1, g2);
                    sum += distance;
                }
            }
            return sum;
        }

        private long GetShortestPath(Galaxy g1, Galaxy g2)
        {
            return Math.Abs(g1.X - g2.X) + Math.Abs(g1.Y - g2.Y);
        }

        private void Expand(List<Galaxy> galaxies, Func<Galaxy, long> getValue, Action<Galaxy, long> setValue, long expansionCount)
        {
            galaxies = galaxies
                .OrderBy(x => getValue(x))
                .ToList();

            long expandCount = 0;
            long last = getValue(galaxies.First());
            foreach (var galaxy in galaxies.Skip(1))
            {
                setValue(galaxy, getValue(galaxy) + expandCount);
                if (getValue(galaxy) - last - 1 > 0)
                {
                    var diff = getValue(galaxy) - last - 1;
                    expandCount += diff * expansionCount;
                    setValue(galaxy, getValue(galaxy) + (diff * expansionCount));
                }
                last = getValue(galaxy);
            }
        }

        private List<Galaxy> GetGalaxies(List<string> input)
        {
            var galaxies = new List<Galaxy>();
            long y = 0;
            foreach (var line in input)
            {
                long x = 0;
                foreach (var digit in line)
                {
                    if (digit == '#')
                        galaxies.Add(new Galaxy(x, y));
                    x++;
                }
                y++;
            }
            return galaxies;
        }

        private class Galaxy
        {
            public Galaxy(long x, long y)
            {
                X = x;
                Y = y;
            }

            public long X { get; set; }
            public long Y { get; set; }
        }
    }
}
