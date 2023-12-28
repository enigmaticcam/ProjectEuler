using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2023;
public class Problem21 : AdventOfCodeBase
{
    public override string ProblemName => "Advent of Code 2023: 21";

    public override string GetAnswer()
    {
        //return Answer1(6, Input_Test(1)).ToString();
        return Answer1(64, Input()).ToString();
    }

    public override string GetAnswer2()
    {
        //return Answer2(6, Input_Test(1)).ToString();
        //return Answer2(10, Input_Test(1)).ToString();
        //return Answer2(50, Input_Test(1)).ToString();
        //return Answer2(100, Input_Test(1)).ToString();
        //return Answer2(500, Input_Test(1)).ToString();
        //return Answer2(1000, Input_Test(1)).ToString();
        return Answer2(5000, Input_Test(1)).ToString();
        return Answer2(26501365, Input()).ToString();
    }

    private int Answer1(int steps, List<string> input)
    {
        return CountSteps(steps, input);
        
    }

    private ulong Answer2(int steps, List<string> input)
    {
        return CountSteps2(steps, input);
    }

    private ulong CountSteps2(int steps, List<string> input)
    {
        ulong total = 0;
        var s = FindS(input);
        var hash = new Dictionary<int, HashSet<int>>();
        var queue = new LinkedList<Step>();
        queue.AddLast(new Step() { X = s.Item1, Y = s.Item2 });
        var next = new int[2];
        var key = new int[2];
        do
        {
            var current = queue.First.Value;
            if (current.StepNum == steps)
            {
                total++;
            }
            else
            {
                var diff = steps - current.StepNum;
                if ((diff % 2) == (steps % 2))
                    total++;

                // Right
                next[0] = current.X + 1;
                next[1] = current.Y;
                key[0] = next[0] % input[0].Length;
                key[1] = next[1] % input.Count;
                if (key[0] < 0)
                    key[0] = input[0].Length - (key[0] * -1);
                if (key[1] < 0)
                    key[1] = input.Count - (key[1] * -1);
                if (input[key[1]][key[0]] == '.' && (!hash.ContainsKey(next[0]) || !hash[next[0]].Contains(next[1])))
                {
                    queue.AddLast(new Step()
                    {
                        StepNum = current.StepNum + 1,
                        X = next[0],
                        Y = next[1]
                    });
                    if (!hash.ContainsKey(next[0]))
                        hash.Add(next[0], new HashSet<int>());
                    hash[next[0]].Add(next[1]);
                }

                // Left
                next[0] = current.X - 1;
                next[1] = current.Y;
                key[0] = next[0] % input[0].Length;
                key[1] = next[1] % input.Count;
                if (key[0] < 0)
                    key[0] = input[0].Length - (key[0] * -1);
                if (key[1] < 0)
                    key[1] = input.Count - (key[1] * -1);
                if (input[key[1]][key[0]] == '.' && (!hash.ContainsKey(next[0]) || !hash[next[0]].Contains(next[1])))
                {
                    queue.AddLast(new Step()
                    {
                        StepNum = current.StepNum + 1,
                        X = next[0],
                        Y = next[1]
                    });
                    if (!hash.ContainsKey(next[0]))
                        hash.Add(next[0], new HashSet<int>());
                    hash[next[0]].Add(next[1]);
                }

                // Up
                next[0] = current.X;
                next[1] = current.Y - 1;
                key[0] = next[0] % input[0].Length;
                key[1] = next[1] % input.Count;
                if (key[0] < 0)
                    key[0] = input[0].Length - (key[0] * -1);
                if (key[1] < 0)
                    key[1] = input.Count - (key[1] * -1);
                if (input[key[1]][key[0]] == '.' && (!hash.ContainsKey(next[0]) || !hash[next[0]].Contains(next[1])))
                {
                    queue.AddLast(new Step()
                    {
                        StepNum = current.StepNum + 1,
                        X = next[0],
                        Y = next[1]
                    });
                    if (!hash.ContainsKey(next[0]))
                        hash.Add(next[0], new HashSet<int>());
                    hash[next[0]].Add(next[1]);
                }

                // Down
                next[0] = current.X;
                next[1] = current.Y + 1;
                key[0] = next[0] % input[0].Length;
                key[1] = next[1] % input.Count;
                if (key[0] < 0)
                    key[0] = input[0].Length - (key[0] * -1);
                if (key[1] < 0)
                    key[1] = input.Count - (key[1] * -1);
                if (input[key[1]][key[0]] == '.' && (!hash.ContainsKey(next[0]) || !hash[next[0]].Contains(next[1])))
                {
                    queue.AddLast(new Step()
                    {
                        StepNum = current.StepNum + 1,
                        X = next[0],
                        Y = next[1]
                    });
                    if (!hash.ContainsKey(next[0]))
                        hash.Add(next[0], new HashSet<int>());
                    hash[next[0]].Add(next[1]);
                }
            }
            queue.RemoveFirst();
        } while (queue.Count > 0);
        return total;
    }

    private int CountSteps(int steps, List<string> input)
    {
        int count = 1;
        var s = FindS(input);
        var hash = new HashSet<Tuple<int, int>>();
        var queue = new LinkedList<Step>();
        queue.AddLast(new Step() { X = s.Item1, Y = s.Item2 });
        int lastCount = 0;
        var next = new int[2];
        do
        {
            var current = queue.First.Value;
            if (current.StepNum == steps)
            {
                count++;
            }
            else
            {
                if (current.StepNum != lastCount)
                {
                    hash.Clear();
                    lastCount = current.StepNum;
                }

                // Right
                next[0] = current.X + 1;
                next[1] = current.Y;
                if (next[0] == input[0].Length)
                    next[0] = 0;
                if (input[next[1]][next[0]] == '.' && !hash.Contains(new Tuple<int, int>(next[0], next[1])))
                {
                    queue.AddLast(new Step()
                    {
                        StepNum = current.StepNum + 1,
                        X = next[0],
                        Y = next[1]
                    });
                    hash.Add(new Tuple<int, int>(next[0], next[1]));
                }

                // Left
                next[0] = current.X - 1;
                next[1] = current.Y;
                if (next[0] == -1)
                    next[0] = input[0].Length - 1;
                if (input[next[1]][next[0]] == '.' && !hash.Contains(new Tuple<int, int>(next[0], next[1])))
                {
                    queue.AddLast(new Step()
                    {
                        StepNum = current.StepNum + 1,
                        X = next[0],
                        Y = next[1]
                    });
                    hash.Add(new Tuple<int, int>(next[0], next[1]));
                }

                // Up
                next[0] = current.X;
                next[1] = current.Y - 1;
                if (next[1] == -1)
                    next[1] = input.Count - 1;
                if (input[next[1]][next[0]] == '.' && !hash.Contains(new Tuple<int, int>(next[0], next[1])))
                {
                    queue.AddLast(new Step()
                    {
                        StepNum = current.StepNum + 1,
                        X = next[0],
                        Y = next[1]
                    });
                    hash.Add(new Tuple<int, int>(next[0], next[1]));
                }

                // Down
                next[0] = current.X;
                next[1] = current.Y + 1;
                if (next[1] == input.Count)
                    next[1] = 0;
                if (input[next[1]][next[0]] == '.' && !hash.Contains(new Tuple<int, int>(next[0], next[1])))
                {
                    queue.AddLast(new Step()
                    {
                        StepNum = current.StepNum + 1,
                        X = next[0],
                        Y = next[1]
                    });
                    hash.Add(new Tuple<int, int>(next[0], next[1]));
                }
            }
            queue.RemoveFirst();
        } while (queue.Count > 0);
        return count;
    }

    private Tuple<int, int> FindS(List<string> map)
    {
        int y = 0;
        foreach (var line in map)
        {
            int x = 0;
            foreach (var digit in line)
            {
                if (digit == 'S')
                    return new Tuple<int, int>(x, y);
                x++;
            }
            y++;
        }
        throw new Exception();
    }

    private class Step
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int StepNum { get; set; }
    }
}
