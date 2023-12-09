using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem08 : AdventOfCodeBase
    {
        private Dictionary<string, Node> _nodes;
        private List<int> _directions;
        public override string ProblemName => "Advent of Code 2023: 8";

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
            SetNodes(input);
            SetDirections(input);
            return FollowSteps();
        }

        private ulong Answer2(List<string> input)
        {
            SetNodes(input);
            SetDirections(input);
            return FollowStepsMultiple();
        }
        private ulong FollowStepsMultiple()
        {
            var current = _nodes
                .Select(x => x.Value)
                .Where(x => x.Name.Last() == 'A')
                .ToList();
            ulong final = 1;
            foreach (var node in current)
            {
                ulong steps = (ulong)FollowStepsIndefinitely(node);
                final = LCM.GetLCM(final, steps);
            }
            return final;
        }

        private int FollowStepsIndefinitely(Node current)
        {
            int directionIndex = 0;
            int steps = 0;
            do
            {
                steps++;
                current = _nodes[current.Next[_directions[directionIndex]]];
                if (current.Name.Last() == 'Z')
                {
                    return steps;
                }
                directionIndex = (directionIndex + 1) % _directions.Count;
            } while (true);
        }

        private int FollowSteps()
        {
            var current = _nodes["AAA"];
            int directionIndex = 0;
            int steps = 0;
            do
            {
                steps++;
                current = _nodes[current.Next[_directions[directionIndex]]];
                if (current.Name == "ZZZ")
                    return steps;
                directionIndex = (directionIndex + 1) % _directions.Count;
            } while (true);
        }

        private void SetDirections(List<string> input)
        {
            _directions = new List<int>();
            foreach (var digit in input[0])
            {
                if (digit == 'L')
                {
                    _directions.Add(0);
                }
                else
                {
                    _directions.Add(1);
                }
            }
        }

        private void SetNodes(List<string> input)
        {
            _nodes = new Dictionary<string, Node>();
            for (int index = 2; index < input.Count; index++)
            {
                var line = input[index];
                var node = new Node() { Next = new string[2] };
                node.Name = line.Substring(0, 3);
                node.Next[0] = line.Substring(7, 3);
                node.Next[1] = line.Substring(12, 3);
                _nodes.Add(node.Name, node);
            }
        }

        private class Node
        {
            public string Name { get; set; }
            public string[] Next { get; set; }
        }
    }
}
