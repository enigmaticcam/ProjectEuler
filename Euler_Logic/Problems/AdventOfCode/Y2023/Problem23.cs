using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2023;

public class Problem23 : AdventOfCodeBase
{
    public override string ProblemName => "Advent of Code 2023: 23";

    public override string GetAnswer()
    {
        //return Answer1(Input_Test(1)).ToString();
        return Answer1(Input()).ToString();
    }

    public override string GetAnswer2()
    {
        return Answer2(Input()).ToString();
    }

    private int Answer1(List<string> input)
    {
        var state = new State() { Hash = new Dictionary<int, Dictionary<int, int>>() };
        return FindLongest(input, state, new Point() { X = 1 }) - 1;
    }
    private int Answer2(List<string> input)
    {
        return 0;
    }

    private int FindLongest(List<string> input, State state, Point point)
    {
        if (!IsInHash(state, point.X, point.Y))
        {
            int steps = 0;
            int best = 0;
            var last = new Point() { X = point.X, Y = point.Y };
            var current = new Point() { X = point.X, Y = point.Y };
            var next = new Point();
            bool canGo;

            do
            {
                steps++;
                canGo = false;
                var lastDigit = input[current.Y][current.X];

                // Left
                if (current.X > 0 && (lastDigit == '.' || lastDigit == '<'))
                {
                    next.X = current.X - 1;
                    next.Y = current.Y;
                    var digit = input[next.Y][next.X];
                    if ((next.X != last.X || next.Y != last.Y) && digit != '#')
                    {
                        if (digit == '.')
                        {
                            canGo = true;
                        }
                        else if (digit != '>')
                        {
                            var longest = FindLongest(input, state, next);
                            if (longest > best)
                                best = longest;
                        }
                    }
                }

                // Right
                if (!canGo && current.X < input[0].Length - 1 && (lastDigit == '.' || lastDigit == '>'))
                {
                    next.X = current.X + 1;
                    next.Y = current.Y;
                    var digit = input[next.Y][next.X];
                    if ((next.X != last.X || next.Y != last.Y) && digit != '#')
                    {
                        if (digit == '.')
                        {
                            canGo = true;
                        } 
                        else if (digit != '<')
                        {
                            var longest = FindLongest(input, state, next);
                            if (longest > best)
                                best = longest;
                        }
                    }
                }

                // Up
                if (!canGo && current.Y > 0 && (lastDigit == '.' || lastDigit == '^'))
                {
                    next.X = current.X;
                    next.Y = current.Y - 1;
                    var digit = input[next.Y][next.X];
                    if ((next.X != last.X || next.Y != last.Y) && digit != '#')
                    {
                        if (digit == '.')
                        {
                            canGo = true;
                        } 
                        else if (digit != 'v')
                        {
                            var longest = FindLongest(input, state, next);
                            if (longest > best)
                                best = longest;
                        }
                    }
                }

                // Down
                if (!canGo && current.Y < input.Count - 1 && (lastDigit == '.' || lastDigit == 'v'))
                {
                    next.X = current.X;
                    next.Y = current.Y + 1;
                    var digit = input[next.Y][next.X];
                    if ((next.X != last.X || next.Y != last.Y) && digit != '#')
                    {
                        if (digit == '.')
                        {
                            canGo = true;
                        } 
                        else if (digit != '^')
                        {
                            var longest = FindLongest(input, state, next);
                            if (longest > best)
                                best = longest;
                        }
                    }
                }
                if (!canGo)
                {
                    if (!state.Hash.ContainsKey(point.X))
                        state.Hash.Add(point.X, new Dictionary<int, int>());
                    state.Hash[point.X].Add(point.Y, best + steps);
                }
                else
                {
                    last.X = current.X;
                    last.Y = current.Y;
                    current.X = next.X;
                    current.Y = next.Y;
                }
            } while (canGo);
        }
        return state.Hash[point.X][point.Y];
    }

    private bool IsInHash(State state, int x, int y)
    {
        return state.Hash.ContainsKey(x) && state.Hash[x].ContainsKey(y);
    }

    private class State
    {
        public Dictionary<int, Dictionary<int, int>> Hash { get; set; }
    }

    private class Step
    {
        public Point P { get; set; }
        public List<Point> Path { get; set; }
    }

    private class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
