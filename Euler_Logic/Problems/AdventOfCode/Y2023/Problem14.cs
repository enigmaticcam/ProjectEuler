using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem14 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 14";

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
            var map = GetMap(input);
            TiltNorth(map);
            return CalcLoad(map);
        }

        private int Answer2(List<string> input)
        {
            var map = GetMap(input);
            return FindRepeat(map);
        }

        private int FindRepeat(List<char[]> map)
        {
            int count = 1;
            for (; count <= 1000; count++)
            {
                Cycle(map);
            }
            var hash = new List<string>() { GetHash(map) };
            int subCount = 0;
            do
            {
                count++;
                subCount++;
                Cycle(map);
                hash.Add(GetHash(map));
                if (hash.First() == hash.Last())
                {
                    var remaining = 1000000000 - count;
                    var mod = remaining % subCount;
                    var finish = subCount - mod - 1;
                    for (int finalCount = 1; finalCount <= finish; finalCount++)
                    {
                        Cycle(map);
                    }
                    return CalcLoad(map);
                }
            } while (true);
        }

        private string GetHash(List<char[]> map)
        {
            var hash = new StringBuilder();
            foreach (var line in map)
            {
                hash.Append(new string(line));
            }
            return hash.ToString();
        }

        private int CalcLoad(List<char[]> map)
        {
            int load = 0;
            int y = map.Count;
            foreach (var line in map)
            {
                foreach (var digit in line)
                {
                    if (digit == 'O')
                        load += y;
                }
                y--;
            }
            return load;
        }

        private void Cycle(List<char[]> map)
        {
            TiltNorth(map);
            TiltWest(map);
            TiltSouth(map);
            TiltEast(map);
        }

        private void TiltNorth(List<char[]> map)
        {
            for (int y = 1; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    var digit = map[y][x];
                    if (digit == 'O')
                    {
                        int subY = y;
                        while (subY > 0)
                        {
                            var next = map[subY - 1][x];
                            if (next == '.')
                            {
                                map[subY][x] = '.';
                                map[subY - 1][x] = 'O';

                            }
                            else
                            {
                                break;
                            }
                            subY--;
                        }
                    }
                }
            }
        }

        private void TiltSouth(List<char[]> map)
        {
            for (int y = map.Count - 1; y >= 0; y--)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    var digit = map[y][x];
                    if (digit == 'O')
                    {
                        int subY = y;
                        while (subY < map.Count - 1)
                        {
                            var next = map[subY + 1][x];
                            if (next == '.')
                            {
                                map[subY][x] = '.';
                                map[subY + 1][x] = 'O';
                            } 
                            else
                            {
                                break;
                            }
                            subY++;
                        }
                    }
                }
            }
        }

        private void TiltWest(List<char[]> map)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                for (int y = 0; y < map.Count; y++)
                {
                    var digit = map[y][x];
                    if (digit == 'O')
                    {
                        int subX = x;
                        while (subX > 0)
                        {
                            var next = map[y][subX - 1];
                            if (next == '.')
                            {
                                map[y][subX] = '.';
                                map[y][subX - 1] = 'O';
                            }
                            else
                            {
                                break;
                            }
                            subX--;
                        }
                    }
                }
            }
        }

        private void TiltEast(List<char[]> map)
        {
            for (int x = map[0].Length - 1; x >= 0; x--)
            {
                for (int y = 0; y < map.Count; y++)
                {
                    var digit = map[y][x];
                    if (digit == 'O')
                    {
                        int subX = x;
                        while (subX < map[0].Length - 1)
                        {
                            var next = map[y][subX + 1];
                            if (next == '.')
                            {
                                map[y][subX] = '.';
                                map[y][subX + 1] = 'O';
                            } 
                            else
                            {
                                break;
                            }
                            subX++;
                        }
                    }
                }
            }
        }

        private List<char[]> GetMap(List<string> input)
        {
            return input
                .Select(x => x.ToArray())
                .ToList();
        }
    }
}
