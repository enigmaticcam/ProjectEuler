using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem18 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 18";

        public override string GetAnswer()
        {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
            return Answer2(Input()).ToString();
        }

        private long Answer1(List<string> input)
        {
            var digs = GetDigs(input);
            var vertices = GetVertices(digs);
            var inner = Shoelace(vertices);
            var outer = GetOuter(digs);
            return Picks(inner, outer);
        }

        private long Answer2(List<string> input)
        {
            var digs = GetDigs(input);
            SwapInstructions(digs);
            var vertices = GetVertices(digs);
            var inner = Shoelace(vertices);
            var outer = GetOuter(digs);
            return Picks(inner, outer);
        }

        private void SwapInstructions(List<Dig> digs)
        {
            foreach (var dig in digs)
            {
                var hex = $"0x{dig.RGB.Substring(0, 5)}";
                dig.Count = Convert.ToInt32(hex, 16);
                dig.Direction = GetDirection(dig.RGB.Last());
            }
        }

        private long Picks(long inner, long outer) // Pick's algorithm
        {
            return (inner + 1) - (outer / 2) + outer;
        }

        private long GetOuter(List<Dig> digs)
        {
            long sum = 0;
            foreach (var dig in digs)
            {
                sum += dig.Count;
            }
            return sum;
        }

        private long Shoelace(List<Tuple<long, long>> vertices) // Shoelace formula
        {
            long xSum = 0;
            long ySum = 0;
            for (int index = 0; index < vertices.Count - 1; index++)
            {
                xSum += vertices[index].Item1 * vertices[index + 1].Item2;
                ySum += vertices[index].Item2 * vertices[index + 1].Item1;
            }
            xSum += vertices.Last().Item1 * vertices.First().Item2;
            ySum += vertices.Last().Item2 * vertices.First().Item1;
            return Math.Abs(xSum - ySum) / 2;
        }

        private List<Tuple<long, long>> GetVertices(List<Dig> digs)
        {
            var vertices = new List<Tuple<long, long>>();
            long x = 0;
            long y = 0;
            foreach (var dig in digs)
            {
                switch (dig.Direction)
                {
                    case enumDirection.Down:
                        y += dig.Count;
                        break;
                    case enumDirection.Left:
                        x -= dig.Count;
                        break;
                    case enumDirection.Right:
                        x += dig.Count;
                        break;
                    case enumDirection.Up:
                        y -= dig.Count;
                        break;
                }
                vertices.Add(new Tuple<long, long>(x, y));
            }
            return vertices;
        }

        private List<Dig> GetDigs(List<string> input)
        {
            var digs = new List<Dig>();
            foreach (var line in input)
            {
                var dig = new Dig();
                digs.Add(dig);
                var split = line.Split(' ');
                dig.Direction = GetDirection(split[0]);
                dig.Count = Convert.ToInt64(split[1]);
                dig.RGB = split[2].Substring(2, 6);
            }
            return digs;
        }

        private enumDirection GetDirection(string text)
        {
            switch (text)
            {
                case "R": return enumDirection.Right;
                case "L": return enumDirection.Left;
                case "U": return enumDirection.Up;
                case "D": return enumDirection.Down;
                default: throw new Exception();
            }
        }

        private enumDirection GetDirection(char digit)
        {
            switch (digit)
            {
                case '0': return enumDirection.Right;
                case '2': return enumDirection.Left;
                case '3': return enumDirection.Up;
                case '1': return enumDirection.Down;
                default: throw new Exception();
            }
        }

        private class Dig
        {
            public enumDirection Direction { get; set; }
            public long Count { get; set; }
            public string RGB { get; set; }
        }

        private enum enumDirection
        {
            Right,
            Down,
            Left,
            Up
        }
    }
}
