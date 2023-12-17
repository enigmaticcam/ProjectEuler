using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem17 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 17";

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
            var state = GetState(input, 4);
            return FindPath2(state, false);
        }

        private int Answer2(List<string> input)
        {
            var state = GetState(input, 10);
            return FindPath2(state, true);
        }

        private int FindPath2(State state, bool isUltra)
        {
            do
            {
                var current = (Block)state.Heap.Top;
                if (current.X == state.Blocks.GetUpperBound(0) && current.Y == state.Blocks.GetUpperBound(1) && (!isUltra || current.Depth > 2))
                {
                    return current.Num;
                }
                var direction = GetDirection(current, current.BestFrom);

                // Right
                if (current.X < state.Blocks.GetUpperBound(0) && direction != enumDirection.Left)
                {
                    bool canGo = false;
                    if (isUltra)
                    {
                        if (direction == enumDirection.Right)
                        {
                            canGo = current.Depth < 9;
                        } else
                        {
                            canGo = current.Depth > 2;
                        }
                    }
                    else
                    {
                        canGo = current.Depth != 2 || direction != enumDirection.Right;
                    }
                    
                    if (canGo || direction == enumDirection.None)
                    {
                        int depth = direction == enumDirection.Right ? current.Depth + 1 : 0;
                        var next = state.Blocks[current.X + 1, current.Y, depth, (int)enumDirection.Right];
                        int calc = current.Num + next.HeatLoss;
                        if (calc < next.Num)
                        {
                            next.Num = calc;
                            next.BestFrom = current;
                            state.Heap.Adjust(next);
                        }
                    }
                }

                // Left
                if (current.X > 0 && direction != enumDirection.Right)
                {
                    bool canGo = false;
                    if (isUltra)
                    {
                        if (direction == enumDirection.Left)
                        {
                            canGo = current.Depth < 9;
                        } else
                        {
                            canGo = current.Depth > 2;
                        }
                    }
                    else
                    {
                        canGo = current.Depth != 2 || direction != enumDirection.Left;
                    }
                    
                    if (canGo || direction == enumDirection.None)
                    {
                        int depth = direction == enumDirection.Left ? current.Depth + 1 : 0;
                        var next = state.Blocks[current.X - 1, current.Y, depth, (int)enumDirection.Left];
                        int calc = current.Num + next.HeatLoss;
                        if (calc < next.Num)
                        {
                            next.Num = calc;
                            next.BestFrom = current;
                            state.Heap.Adjust(next);
                        }
                    }
                }

                // Up
                if (current.Y > 0 && direction != enumDirection.Down)
                {
                    bool canGo = false;
                    if (isUltra)
                    {
                        if (direction == enumDirection.Up)
                        {
                            canGo = current.Depth < 9;
                        } else
                        {
                            canGo = current.Depth > 2;
                        }
                    }
                    else
                    {
                        canGo = current.Depth != 2 || direction != enumDirection.Up;
                    }
                    
                    if (canGo || direction == enumDirection.None)
                    {
                        int depth = direction == enumDirection.Up ? current.Depth + 1 : 0;
                        var next = state.Blocks[current.X, current.Y - 1, depth, (int)enumDirection.Up];
                        int calc = current.Num + next.HeatLoss;
                        if (calc < next.Num)
                        {
                            next.Num = calc;
                            next.BestFrom = current;
                            state.Heap.Adjust(next);
                        }
                    }
                }

                // Down
                if (current.Y < state.Blocks.GetUpperBound(1) && direction != enumDirection.Up)
                {
                    bool canGo = false;
                    if (isUltra)
                    {
                        if (direction == enumDirection.Down)
                        {
                            canGo = current.Depth < 9;
                        } else
                        {
                            canGo = current.Depth > 2;
                        }
                    }
                    else
                    {
                        canGo = current.Depth != 2 || direction != enumDirection.Down;
                    }
                    
                    if (canGo || direction == enumDirection.None)
                    {
                        int depth = direction == enumDirection.Down ? current.Depth + 1 : 0;
                        var next = state.Blocks[current.X, current.Y + 1, depth, (int)enumDirection.Down];
                        int calc = current.Num + next.HeatLoss;
                        if (calc < next.Num)
                        {
                            next.Num = calc;
                            next.BestFrom = current;
                            state.Heap.Adjust(next);
                        }
                    }
                }

                state.Heap.Remove(current);
            } while (true);
        }

        private enumDirection GetDirection(Block current, Block prior)
        {
            if (prior == null)
                return enumDirection.None;
            if (current.X > prior.X)
                return enumDirection.Right;
            if (current.X < prior.X)
                return enumDirection.Left;
            if (current.Y > prior.Y)
                return enumDirection.Down;
            if (current.Y < prior.Y)
                return enumDirection.Up;
            throw new Exception();
        }

        private State GetState(List<string> input, int maxDepth)
        {
            var heap = new BinaryHeap_Min();
            var blocks = new Block[input[0].Length, input.Count, maxDepth, 4];
            int y = 0;
            foreach (var line in input)
            {
                int x = 0;
                foreach (var digit in line)
                {
                    for (int depth = 0; depth < maxDepth; depth++)
                    {
                        for (int direction = 0; direction < 4; direction++)
                        {
                            var block = new Block()
                            {
                                HeatLoss = Convert.ToInt32(digit.ToString()),
                                Num = x == 0 && y == 0 && depth == 0 && direction == 0 ? 0 : int.MaxValue,
                                Depth = depth,
                                Direction = (enumDirection)direction,
                                X = x,
                                Y = y
                            };
                            blocks[x, y, depth, direction] = block;
                            heap.Add(block);
                        }
                    }
                        
                    x++;
                }
                y++;
            }
            return new State()
            {
                Blocks = blocks,
                Heap = heap
            };
        }

        private enum enumDirection
        {
            Up,
            Down,
            Left,
            Right,
            None
        }

        private class State
        {
            public Block[,,,] Blocks { get; set; }
            public BinaryHeap_Min Heap { get; set; }
        }

        private class Block : BinaryHeap_Min.Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Depth { get; set; }
            public int HeatLoss { get; set; }
            public enumDirection Direction { get; set; }
            public Block BestFrom { get; set; }
        }
    }
}
