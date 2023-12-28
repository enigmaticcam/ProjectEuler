using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023;
public class Problem22 : AdventOfCodeBase
{
    public override string ProblemName => "Advent of Code 2023: 22";

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
        var bricks = GetBricks(input);
        DropBricks(bricks);
        SetSupportedBy(bricks);
        return CountRemovable(bricks);
    }
    private int Answer2(List<string> input)
    {
        var bricks = GetBricks(input);
        DropBricks(bricks);
        SetSupportedBy(bricks);
        return FindBest(bricks);
    }

    private int FindBest(List<Brick> bricks)
    {
        int index = 0;
        int sum = 0;
        foreach (var brick in bricks)
        {
            var result = CountRemoves(brick, index, bricks);
            sum += result;
            index++;
        }
        return sum;
    }

    private int CountRemoves(Brick brick, int brickIndex, List<Brick> bricks)
    {
        int count = -1;
        var queue = new LinkedList<Brick>();
        var changed = new LinkedList<Brick>();
        queue.AddFirst(brick);
        do
        {
            count++;
            var current = queue.First.Value;
            foreach (var supportIndex in current.Supports)
            {
                var support = bricks[supportIndex];
                changed.AddLast(support);
                support.SupportedByCount--;
                if (support.SupportedByCount == 0)
                    queue.AddLast(support);
            }
            queue.RemoveFirst();
        } while (queue.Count > 0);
        foreach (var sub in changed)
        {
            sub.SupportedByCount = sub.SupportedBy.Count;
        }
        return count;
    }

    private int CountRemovable(List<Brick> bricks)
    {
        var singles = bricks
            .Where(x => x.SupportedBy.Count == 1)
            .Select(x => x.SupportedBy.First())
            .ToHashSet();
        return bricks.Count - singles.Count;
    }

    private void SetSupportedBy(List<Brick> bricks)
    {
        int index = 0;
        foreach (var brick in bricks)
        {
            SetSupportedBy(brick, index, bricks);
            index++;
        }
        bricks.ForEach(x => x.SupportedByCount = x.SupportedBy.Count);
    }

    private void SetSupportedBy(Brick brick, int brickIndex, List<Brick> bricks)
    {
        int index = 0;
        foreach (var current in bricks)
        {
            if (current != brick  && current.Start.Z == brick.End.Z + 1)
            {
                if (current.Start.X <= brick.End.X
                    && current.End.X >= brick.Start.X
                    && current.Start.Y <= brick.End.Y
                    && current.End.Y >= brick.Start.Y)
                {
                    current.SupportedBy.Add(brickIndex);
                    brick.Supports.Add(index);
                }
            }
            index++;
        }
    }

    private void DropBricks(List<Brick> bricks)
    {
        int index = 0;
        foreach (var brick in bricks)
        {
            DropBrick(brick, index, bricks);
            index++;
        }
    }

    private bool DropBrick(Brick brick, int brickIndex, List<Brick> bricks)
    {
        while (brick.Start.Z > 1)
        {
            for (int index = brickIndex - 1; index >= 0; index--)
            {
                var current = bricks[index];
                if (current.Start.X <= brick.End.X
                    && current.End.X >= brick.Start.X
                    && current.Start.Y <= brick.End.Y
                    && current.End.Y >= brick.Start.Y
                    && current.Start.Z <= brick.End.Z - 1
                    && current.End.Z >= brick.Start.Z - 1)
                    return false;
            }
            brick.Start.Z--;
            brick.End.Z--;
        }
        return true;
    }

    private List<Brick> GetBricks(List<string> input)
    {
        var bricks = new List<Brick>();
        foreach (var line in input)
        {
            var split = line.Split('~');
            var startSplit = split[0].Split(',');
            var endSplit = split[1].Split(',');
            bricks.Add(new Brick()
            {
                Start = new Point()
                {
                    X = Convert.ToInt32(startSplit[0]),
                    Y = Convert.ToInt32(startSplit[1]),
                    Z = Convert.ToInt32(startSplit[2])
                },
                End = new Point()
                {
                    X = Convert.ToInt32(endSplit[0]),
                    Y = Convert.ToInt32(endSplit[1]),
                    Z = Convert.ToInt32(endSplit[2])
                },
                SupportedBy = new HashSet<int>(),
                Supports = new List<int>()
            });
        }
        return bricks
            .OrderBy(x => x.Start.Z)
            .ToList();
    }

    private class Brick
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public HashSet<int> SupportedBy { get; set; }
        public List<int> Supports { get; set; }
        public int SupportedByCount { get; set; }
    }

    private class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}
