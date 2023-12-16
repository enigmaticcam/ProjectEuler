using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem16 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 16";

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
            var grid = GetPoints(input);
            EnergizeGrid(grid, new Beam() { X = 0, Y = 0, Direction = 0 });
            return CountEnergized(grid);
        }

        private int Answer2(List<string> input)
        {
            var grid = GetPoints(input);
            return FindBest(grid);
        }

        private int FindBest(Point[,] grid)
        {
            int best = 0;
            int next = 0;
            for (int x = 0; x <= grid.GetUpperBound(0); x++)
            {
                next = FindBest(grid, new Beam() { Direction = enumDirection.Down, X = x });
                if (next > best)
                    best = next;
                next = FindBest(grid, new Beam() { Direction = enumDirection.Up, Y = grid.GetUpperBound(1), X = x });
                if (next > best)
                    best = next;
            }
            for (int y = 0; y <= grid.GetUpperBound(1); y++)
            {
                next = FindBest(grid, new Beam() { Direction = enumDirection.Right, Y = y });
                if (next > best)
                    best = next;
                next = FindBest(grid, new Beam() { Direction = enumDirection.Left, Y = y, X = grid.GetUpperBound(0) });
                if (next > best)
                    best = next;
            }
            return best;
        }

        private int FindBest(Point[,] grid, Beam beam)
        {
            ClearEnergized(grid);
            EnergizeGrid(grid, beam);
            return CountEnergized(grid);
        }

        private void ClearEnergized(Point[,] grid)
        {
            foreach (var point in grid)
            {
                point.Energized[0] = false;
                point.Energized[1] = false;
                point.Energized[2] = false;
                point.Energized[3] = false;
            }
        }

        private int CountEnergized(Point[,] grid)
        {
            int count = 0;
            foreach (var point in grid)
            {
                if (point.Energized != null && point.Energized.Any(x => x))
                    count++;
            }
            return count;
        }

        private void EnergizeGrid(Point[,] grid, Beam startBeam)
        {
            var beams = new LinkedList<Beam>();
            beams.AddLast(startBeam);
            do
            {
                var beam = beams.First;
                while (beam != null)
                {
                    var point = grid[beam.Value.X, beam.Value.Y];
                    Beam newBeam = null;
                    switch (point.Digit)
                    {
                        case '\\':
                        case '/':
                            ChangeDirection_Mirror(grid, point, beam.Value);
                            break;
                        case '-':
                        case '|':
                            newBeam = ChangeDirection_Splitter(grid, point, beam.Value);
                            break;
                    }
                    var canKeep1 = MoveBeam(grid, point, beam.Value);
                    var canKeep2 = false;
                    if (newBeam != null)
                        canKeep2 = MoveBeam(grid, point, newBeam);
                    if (!canKeep1)
                        beams.Remove(beam);
                    if (canKeep2)
                        beams.AddLast(newBeam);
                    beam = beam.Next;
                }
            } while (beams.Count > 0);
        }

        private Beam ChangeDirection_Splitter(Point[,] grid, Point point, Beam beam)
        {
            bool didSplit = false;
            enumDirection direction = enumDirection.Right;
            if (point.Digit == '|')
            {
                if (beam.Direction == enumDirection.Right || beam.Direction == enumDirection.Left)
                {
                    beam.Direction = enumDirection.Up;
                    direction = enumDirection.Down;
                    didSplit = true;
                }
            }
            else
            {
                if (beam.Direction == enumDirection.Up || beam.Direction == enumDirection.Down)
                {
                    beam.Direction = enumDirection.Left;
                    direction = enumDirection.Right;
                    didSplit = true;
                }
            }
            if (didSplit)
            {
                return new Beam()
                {
                    Direction = direction,
                    X = beam.X,
                    Y = beam.Y
                };
            }
            return null;
        }

        private void ChangeDirection_Mirror(Point[,] grid, Point point, Beam beam)
        {
            if (point.Digit == '\\')
            {
                if (beam.Direction == enumDirection.Right)
                {
                    beam.Direction = enumDirection.Down;
                } 
                else if (beam.Direction == enumDirection.Down)
                {
                    beam.Direction = enumDirection.Right;
                }
                else if (beam.Direction == enumDirection.Left)
                {
                    beam.Direction = enumDirection.Up;
                }
                else
                {
                    beam.Direction = enumDirection.Left;
                }
            }
            else
            {
                if (beam.Direction == enumDirection.Right)
                {
                    beam.Direction = enumDirection.Up;
                } else if (beam.Direction == enumDirection.Up)
                {
                    beam.Direction = enumDirection.Right;
                } else if (beam.Direction == enumDirection.Left)
                {
                    beam.Direction = enumDirection.Down;
                } else
                {
                    beam.Direction = enumDirection.Left;
                }
            }
        }

        private bool MoveBeam(Point[,] grid, Point point, Beam beam)
        {
            if (point.Energized[(int)beam.Direction])
            {
                return false;
            }
            point.Energized[(int)beam.Direction] = true;
            switch (beam.Direction)
            {
                case enumDirection.Right:
                    beam.X++;
                    break;
                case enumDirection.Down:
                    beam.Y++;
                    break;
                case enumDirection.Left:
                    beam.X--;
                    break;
                case enumDirection.Up:
                    beam.Y--;
                    break;
            }
            if (IsOutsideGrid(grid, beam))
            {
                return false;
            }
            return true;
        }

        private bool IsOutsideGrid(Point[,] grid, Beam beam)
        {
            if (beam.X > grid.GetUpperBound(0))
                return true;
            if (beam.X < 0)
                return true;
            if (beam.Y > grid.GetUpperBound(1))
                return true;
            if (beam.Y < 0)
                return true;
            return false;
        }

        private Point[,] GetPoints(List<string> input)
        {
            var grid = new Point[input[0].Length, input.Count];
            int y = 0;
            foreach (var line in input)
            {
                int x = 0;
                foreach (var digit in line)
                {
                    var point = new Point()
                    {
                        X = x,
                        Y = y,
                        Energized = new bool[4],
                        Digit = digit
                    };
                    grid[x, y] = point;
                    x++;
                }
                y++;
            }
            return grid;
        }

        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public bool[] Energized { get; set; }
            public char Digit { get; set; }
        }

        private class Beam
        {
            public int X { get; set; }
            public int Y { get; set; }
            public enumDirection Direction { get; set; }
        }

        public enum enumDirection
        {
            Right,
            Down,
            Left,
            Up
        }
    }
}
