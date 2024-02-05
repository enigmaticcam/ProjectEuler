using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2023;

public class Problem21 : AdventOfCodeBase {

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
        var state = new State1() {
            Input = input,
            MaxSteps = steps
        };
        return BruteForce(state);
    }

    private ulong Answer2(int steps, List<string> input) {
        var state = new State1() {
            Input = input,
            MaxSteps = steps
        };
        return BruteForce(state);
    }

    private ulong BruteForce(State1 state) {
        ulong count = 0;
        var start = FindS(state.Input);
        state.Steps.AddFirst(new Step() {
            StepNum = 0,
            X = start.X,
            Y = start.Y
        });
        var mod = (start.X + start.Y) % 2;
        do {
            var current = state.Steps.First.Value;
            if (current.StepNum == state.MaxSteps)
                count++;
            if (current.StepNum < state.MaxSteps) {
                AddToStep(current, 1, 0, state);
                AddToStep(current, -1, 0, state);
                AddToStep(current, 0, 1, state);
                AddToStep(current, 0, -1, state);
            }
            state.Steps.RemoveFirst();
        } while (state.Steps.Count > 0);
        return count;
    }

    private void AddToStep(Step current, int xOffset, int yOffset, State1 state) {
        int x = current.X + xOffset;
        int y = current.Y + yOffset;
        int modX = Mod.NegativeMod(x, state.Input[0].Length);
        int modY = Mod.NegativeMod(y, state.Input.Count);
        bool isOpen = state.Input[modY][modX] != '#';
        if (isOpen && !DoesExistInHash(x, y, current.StepNum, state)) {
            state.Steps.AddLast(new Step() {
                StepNum = current.StepNum + 1,
                X = x,
                Y = y
            });
            AddToHash(x, y, current.StepNum, state);
        }
    }

    private bool DoesExistInHash(int x, int y, int steps, State1 state) {
        return state.Hash.ContainsKey(x) && state.Hash[x].ContainsKey(y) && state.Hash[x][y].Contains(steps);
    }

    private void AddToHash(int x, int y, int steps, State1 state) {
        if (!state.Hash.ContainsKey(x))
            state.Hash.Add(x, new Dictionary<int, HashSet<int>>());
        if (!state.Hash[x].ContainsKey(y))
            state.Hash[x].Add(y, new HashSet<int>());
        state.Hash[x][y].Add(steps);
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

    private class State1 {
        public List<string> Input { get; set; }
        public Dictionary<int, Dictionary<int, HashSet<int>>> Hash { get; set; } = new();
        public int MaxSteps { get; set; }
        public LinkedList<Step> Steps { get; set; } = new();
    }
}
