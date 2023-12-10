using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem10 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 10";

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
            var state = FollowPath(input);
            return GetMost(state);
        }

        private int Answer2(List<string> input)
        {
            var state = FollowPath(input);
            state.ReplaceS = ReplaceS(state, input);
            input = RemoveEverythingElse(state, input);
            input = Expand(state, input);
            LookForGaps(state, input);
            return state.GapCount;
        }

        private char ReplaceS(State state, List<string> input)
        {
            var start = state.Start;
            bool hasTop = true;
            bool hasBottom = true;
            bool hasLeft = true;
            bool hasRight = true;

            var digit = input[start.Y][start.X + 1];
            if (digit != '-' && digit != 'J' && digit != '7')
                hasRight = false;

            digit = input[start.Y][start.X - 1];
            if (digit != '-' && digit != 'L' && digit != 'F')
                hasLeft = false;

            digit = input[start.Y + 1][start.X];
            if (digit != '|' && digit != 'L' && digit != 'J')
                hasBottom = false;

            digit = input[start.Y - 1][start.X];
            if (digit != '|' && digit != '7' && digit != 'F')
                hasTop = false;

            if (hasTop && hasBottom)
            {
                return '|';
            }
            else if (hasTop && hasLeft)
            {
                return 'J';
            }
            else if (hasTop && hasRight)
            {
                return 'L';
            }
            else if (hasBottom && hasLeft)
            {
                return '7';
            }
            else if (hasBottom && hasRight)
            {
                return 'F';
            }
            else
            {
                return '-';
            }
        }

        private List<string> Expand(State state, List<string> input)
        {
            var newGrid = new Dictionary<Tuple<int, int>, char>();
            int y = 0;
            int newY = 0;
            foreach (var line in input)
            {
                int x = 0;
                int newX = 0;
                foreach (var tempDigit in line)
                {
                    var digit = tempDigit;
                    if (x == state.Start.X && y == state.Start.Y)
                        digit = state.ReplaceS;
                    newGrid.Add(new Tuple<int, int>(newX, newY), digit);
                    switch (digit)
                    {
                        case '.':
                        case 'J':
                            newGrid.Add(new Tuple<int, int>(newX + 1, newY), '.');
                            newGrid.Add(new Tuple<int, int>(newX, newY + 1), '.');
                            break;
                        case '|':
                        case '7':
                            newGrid.Add(new Tuple<int, int>(newX + 1, newY), '.');
                            newGrid.Add(new Tuple<int, int>(newX, newY + 1), '|');
                            break;
                        case '-':
                        case 'L':
                            newGrid.Add(new Tuple<int, int>(newX + 1, newY), '-');
                            newGrid.Add(new Tuple<int, int>(newX, newY + 1), '.');
                            break;
                        case 'F':
                            newGrid.Add(new Tuple<int, int>(newX + 1, newY), '-');
                            newGrid.Add(new Tuple<int, int>(newX, newY + 1), '|');
                            break;
                    }
                    x++;
                    newX += 2;
                }
                y++;
                newY += 2;
            }
            var newInput = new List<string>();
            for (int subY = 0; subY < input.Count * 2; subY++)
            {
                var newLine = new List<char>();
                for (int subX = 0; subX < input[0].Length * 2; subX++)
                {
                    var point = new Tuple<int, int>(subX, subY);
                    if (newGrid.ContainsKey(point))
                    {
                        newLine.Add(newGrid[point]);
                    }
                    else
                    {
                        newLine.Add('.');
                    }
                }
                newInput.Add(new string(newLine.ToArray()));
            }
            return newInput;
        }

        private void LookForGaps(State state, List<string> input)
        {
            state.GapHash = new HashSet<Tuple<int, int>>();
            int y = 0;
            foreach (var line in input)
            {
                int x = 0;
                foreach (var digit in line)
                {
                    if (digit == '.')
                    {
                        var point = new Tuple<int, int>(x, y);
                        if (!state.GapHash.Contains(point))
                        {
                            LookForGaps(state, input, point);
                        }
                    }
                    x++;
                }
                y++;
            }
        }

        private void LookForGaps(State state, List<string> input, Tuple<int, int> point)
        {
            bool canCount = true;
            int count = 0;
            var path = new LinkedList<Tuple<int, int>>();
            path.AddFirst(point);
            var list = new List<Tuple<int, int>>();
            do
            {
                var current = path.First.Value;
                path.RemoveFirst();
                if (!state.GapHash.Contains(current))
                {
                    list.Add(current);
                    state.GapHash.Add(current);
                    if ((current.Item1 % 2) == 0 && (current.Item2 % 2) == 0)
                        count++;
                    if (current.Item1 == 0 || current.Item1 == input[0].Length - 1 || current.Item2 == 0 || current.Item2 == input.Count - 1)
                        canCount = false;
                    if (current.Item1 < input[0].Length - 1 && input[current.Item2][current.Item1 + 1] == '.')
                    {
                        var next = new Tuple<int, int>(current.Item1 + 1, current.Item2);
                        if (!state.GapHash.Contains(next))
                            path.AddLast(new Tuple<int, int>(current.Item1 + 1, current.Item2));
                    }
                    if (current.Item1 > 0 && input[current.Item2][current.Item1 - 1] == '.')
                    {
                        var next = new Tuple<int, int>(current.Item1 - 1, current.Item2);
                        if (!state.GapHash.Contains(next))
                            path.AddLast(new Tuple<int, int>(current.Item1 - 1, current.Item2));
                    }
                    if (current.Item2 < input.Count - 1 && input[current.Item2 + 1][current.Item1] == '.')
                    {
                        var next = new Tuple<int, int>(current.Item1, current.Item2 + 1);
                        if (!state.GapHash.Contains(next))
                            path.AddLast(new Tuple<int, int>(current.Item1, current.Item2 + 1));
                    }
                    if (current.Item2 > 0 && input[current.Item2 - 1][current.Item1] == '.')
                    {
                        var next = new Tuple<int, int>(current.Item1, current.Item2 - 1);
                        if (!state.GapHash.Contains(next))
                            path.AddLast(new Tuple<int, int>(current.Item1, current.Item2 - 1));
                    }
                }
            } while (path.Count > 0);
            if (canCount)
                state.GapCount += count;
        }

        private List<string> RemoveEverythingElse(State state, List<string> input)
        {
            var newGrid = new List<string>();
            for (int y = 0; y < input.Count; y++)
            {
                var newLine = new List<char>();
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (!state.Hash.ContainsKey(x) || !state.Hash[x].ContainsKey(y))
                    {
                        newLine.Add('.');
                    }
                    else
                    {
                        newLine.Add(input[y][x]);
                    }
                }
                newGrid.Add(new string(newLine.ToArray()));
            }
            return newGrid;
        }

        private State FollowPath(List<string> input)
        {
            var state = new State()
            {
                Hash = new Dictionary<int, Dictionary<int, Path>>(),
                Paths = new LinkedList<Path>()
            };
            state.Start = FindStart(input);
            state.Hash.Add(state.Start.X, new Dictionary<int, Path>());
            state.Hash[state.Start.X].Add(state.Start.Y, new Path(0, state.Start));
            foreach (var nextStart in GetStartingPoints(input, state.Start))
            {
                state.Paths.AddLast(new Path(1, nextStart));
            }
            do
            {
                var next = state.Paths.First.Value;
                var digit = input[next.Location.Y][next.Location.X];
                switch (digit)
                {
                    case '|':
                        AddToState(state, next.Location.X, next.Location.Y - 1, next.Steps + 1);
                        AddToState(state, next.Location.X, next.Location.Y + 1, next.Steps + 1);
                        break;
                    case '-':
                        AddToState(state, next.Location.X - 1, next.Location.Y, next.Steps + 1);
                        AddToState(state, next.Location.X + 1, next.Location.Y, next.Steps + 1);
                        break;
                    case 'L':
                        AddToState(state, next.Location.X, next.Location.Y - 1, next.Steps + 1);
                        AddToState(state, next.Location.X + 1, next.Location.Y, next.Steps + 1);
                        break;
                    case 'J':
                        AddToState(state, next.Location.X, next.Location.Y - 1, next.Steps + 1);
                        AddToState(state, next.Location.X - 1, next.Location.Y, next.Steps + 1);
                        break;
                    case '7':
                        AddToState(state, next.Location.X, next.Location.Y + 1, next.Steps + 1);
                        AddToState(state, next.Location.X - 1, next.Location.Y, next.Steps + 1);
                        break;
                    case 'F':
                        AddToState(state, next.Location.X, next.Location.Y + 1, next.Steps + 1);
                        AddToState(state, next.Location.X + 1, next.Location.Y, next.Steps + 1);
                        break;
                }
                state.Paths.RemoveFirst();
            } while (state.Paths.Count > 0);
            return state;
        }

        private int GetMost(State state)
        {
            int best = 0;
            foreach (var keyValue1 in state.Hash)
            {
                int next = keyValue1.Value.Values.Select(x => x.Steps).Max();
                if (next > best)
                    best = next;
            }
            return best;
        }

        private IEnumerable<Point> GetStartingPoints(List<string> input, Point start)
        {
            if (input[start.Y - 1][start.X] == '|')
                yield return new Point(start.X, start.Y - 1);
            if (input[start.Y - 1][start.X] == '7')
                yield return new Point(start.X, start.Y - 1);
            if (input[start.Y - 1][start.X] == 'F')
                yield return new Point(start.X, start.Y - 1);
            if (input[start.Y + 1][start.X] == '|')
                yield return new Point(start.X, start.Y + 1);
            if (input[start.Y + 1][start.X] == 'L')
                yield return new Point(start.X, start.Y + 1);
            if (input[start.Y + 1][start.X] == 'J')
                yield return new Point(start.X, start.Y + 1);
            if (input[start.Y][start.X - 1] == '-')
                yield return new Point(start.X - 1, start.Y);
            if (input[start.Y][start.X - 1] == 'L')
                yield return new Point(start.X - 1, start.Y);
            if (input[start.Y][start.X - 1] == 'F')
                yield return new Point(start.X - 1, start.Y);
            if (input[start.Y][start.X + 1] == '-')
                yield return new Point(start.X + 1, start.Y);
            if (input[start.Y][start.X + 1] == 'J')
                yield return new Point(start.X + 1, start.Y);
            if (input[start.Y][start.X + 1] == '7')
                yield return new Point(start.X + 1, start.Y);
        }

        private bool AddToState(State state, int x, int y, int steps)
        {   
            if (!ExistsInHash(state, x, y))
            {
                var point = new Point(x, y);
                var path = new Path(steps, point);
                AddToHash(state, path);
                state.Paths.AddLast(path);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ExistsInHash(State state, int x, int y)
        {
            if (state.Hash.ContainsKey(x))
                return state.Hash[x].ContainsKey(y);
            return false;
        }

        private void AddToHash(State state, Path path)
        {
            if (!state.Hash.ContainsKey(path.Location.X))
                state.Hash.Add(path.Location.X, new Dictionary<int, Path>());
            state.Hash[path.Location.X].Add(path.Location.Y, path);
        }

        private Point FindStart(List<string> input)
        {
            int y = 0;
            foreach (var line in input)
            {
                int index = line.IndexOf('S');
                if (index > -1)
                    return new Point(index, y);
                y++;
            }
            return null;
        }

        private class Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }
        }

        private class Path
        {
            public Path(int steps, Point location)
            {
                Steps = steps;
                Location = location;
            }

            public int Steps { get; set; }
            public Point Location { get; set; }
        }

        private class State
        {
            public Dictionary<int, Dictionary<int, Path>> Hash { get; set; }
            public LinkedList<Path> Paths { get; set; }
            public Point Start { get; set; }
            public HashSet<Tuple<int, int>> GapHash { get; set; }
            public int GapCount { get; set; }

            public char ReplaceS { get; set; }
        }
    }
}
