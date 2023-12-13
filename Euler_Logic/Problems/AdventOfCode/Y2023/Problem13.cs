using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem13 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 13";

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
            var grids = GetGrids(input);
            return FindSum(grids);
        }

        private int Answer2(List<string> input)
        {
            var grids = GetGrids(input);
            return FindSmudge(grids);
        }

        private int FindSmudge(List<List<char[]>> grids)
        {
            int sum = 0;
            int count = 1;
            foreach (var grid in grids)
            {
                var next = FindSmudge(grid);
                sum += next;
                count++;
            }
            return sum;
        }

        private int FindSmudge(List<char[]> grid)
        {
            var ph = Horizontal(grid, false, 0);
            var pv = Vertical(grid, false, 0);
            for (int y = 0; y < grid.Count; y++)
            {
                var line = grid[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var old = line[x];
                    if (line[x] == '.')
                    {
                        line[x] = '#';
                    }
                    else
                    {
                        line[x] = '.';
                    }

                    var isGood = false;
                    var next = Horizontal(grid, true, ph);
                    if (next != -1)
                    {
                        next = (next + 1) * 100;
                        isGood = true;
                    }
                    else
                    {
                        next = Vertical(grid, true, pv);
                        if (next != -1)
                        {
                            next++;
                            isGood = true;
                        }
                    }
                    line[x] = old;
                    if (isGood)
                        return next;
                }
            }
            throw new Exception();
        }

        private int FindSum(List<List<char[]>> grids)
        {
            int sum = 0;
            foreach (var grid in grids)
            {
                var next = FindSum(grid, false, -1);
                sum += next;
            }
            return sum;
        }

        private int FindSum(List<char[]> grid, bool ignoreLine, int lineToIgnore)
        {
            var next = Horizontal(grid, ignoreLine, lineToIgnore);
            if (next != -1)
            {
                next = (next + 1) * 100;
            } 
            else
            {
                next = Vertical(grid, ignoreLine, lineToIgnore);
                if (next != -1)
                    next++;
            }
            return next;
        }

        private int Horizontal(List<char[]> grid, bool ignoreLine, int lineToIgnore)
        {
            for (int y = 0; y < grid.Count - 1; y++)
            {
                if ((!ignoreLine || y != lineToIgnore) && IsHorizontal(grid, y, y + 1))
                    return y;
            }
            return -1;
        }

        private bool IsHorizontal(List<char[]> grid, int top, int bottom)
        {
            do
            {
                for (int index = 0; index < grid[0].Length; index++)
                {
                    if (grid[top][index] != grid[bottom][index])
                        return false;
                }
                top--;
                bottom++;
            } while (top >= 0 && bottom < grid.Count);
            
            return true;
        }

        private int Vertical(List<char[]> grid, bool ignoreLine, int lineToIgnore)
        {
            for (int x = 0; x < grid[0].Length - 1; x++)
            {
                if ((!ignoreLine || x != lineToIgnore) && IsVertical(grid, x, x + 1))
                    return x;
            }
            return -1;
        }

        private bool IsVertical(List<char[]> grid, int left, int right)
        {
            do
            {
                for (int index = 0; index < grid.Count; index++)
                {
                    if (grid[index][left] != grid[index][right])
                        return false;
                }
                left--;
                right++;
            } while (left >= 0 && right < grid[0].Length);
            return true;
        }

        private List<List<char[]>> GetGrids(List<string> input)
        {
            var grids = new List<List<char[]>>();
            var current = new List<char[]>();
            grids.Add(current);
            foreach (var line in input)
            {
                if (line == "")
                {
                    current = new List<char[]>();
                    grids.Add(current);
                }
                else
                {
                    current.Add(line.ToArray());
                }
            }
            return grids;
        }
    }
}
