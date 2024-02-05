using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2023;

public class Problem21 : AdventOfCodeBase {
    private Dictionary<int, Dictionary<int, HashSet<int>>> _hash = new();

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
        return Answer2(100, Input_Test(1)).ToString();
        return Answer2(500, Input_Test(1)).ToString();
        return Answer2(1000, Input_Test(1)).ToString();
        return Answer2(5000, Input_Test(1)).ToString();
        return Answer2(26501365, Input()).ToString();
    }

    private ulong Answer1(int steps, List<string> input)
    {
        return BruteForce(steps, input);
        
    }

    private ulong Answer2(int steps, List<string> input)
    {
        return BruteForce(steps, input);
    }

    private ulong BruteForce(int steps, List<string> input) {
        ulong count = 0;
        var start = FindS(input);
        var list = new LinkedList<Step>();
        list.AddFirst(new Step() {
            StepNum = 0,
            X = start.X,
            Y = start.Y
        });
        var mod = (start.X + start.Y) % 2;
        do {
            var current = list.First.Value;
            if (current.StepNum == steps)
                count++;
            if (current.StepNum < steps) {
                AddToStep(current, 1, 0, list, input);
                AddToStep(current, -1, 0, list, input);
                AddToStep(current, 0, 1, list, input);
                AddToStep(current, 0, -1, list, input);
            }
            list.RemoveFirst();
        } while (list.Count > 0);
        return count;
    }

    private void AddToStep(Step current, int xOffset, int yOffset, LinkedList<Step> list, List<string> input) {
        int x = current.X + xOffset;
        int y = current.Y + yOffset;
        int modX = Mod.NegativeMod(x, input[0].Length);
        int modY = Mod.NegativeMod(y, input.Count);
        bool isOpen = input[modY][modX] != '#';
        if (isOpen && !DoesExistInHash(x, y, current.StepNum, input)) {
            list.AddLast(new Step() {
                StepNum = current.StepNum + 1,
                X = x,
                Y = y
            });
            AddToHash(x, y, current.StepNum);
        }
    }

    private bool DoesExistInHash(int x, int y, int steps, List<string> input) {
        return _hash.ContainsKey(x) && _hash[x].ContainsKey(y) && _hash[x][y].Contains(steps);
    }

    private void AddToHash(int x, int y, int steps) {
        if (!_hash.ContainsKey(x))
            _hash.Add(x, new Dictionary<int, HashSet<int>>());
        if (!_hash[x].ContainsKey(y))
            _hash[x].Add(y, new HashSet<int>());
        _hash[x][y].Add(steps);
    }

    private (int X, int Y) FindS(List<string> input) {
        int y = 0;
        foreach (var line in input) {
            int x = 0;
            foreach (var digit in line) {
                if (digit == 'S')
                    return (x, y);
                x++;
            }
            y++;
        }
        throw new Exception("S not found");
    }

    private class Step
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int StepNum { get; set; }
    }
}
