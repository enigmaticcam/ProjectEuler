using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem03 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 3";

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
            var state = GetState(input);
            SetHasAdjacent(state);
            return SumOfAllParts(state);
        }
        private int Answer2(List<string> input)
        {
            var state = GetState(input);
            SetHasAdjacent(state);
            return SumOfAllGears(state);
        }

        private int SumOfAllGears(State state)
        {
            var gears = state.AllSymbols.Where(x => x.AdjacentParts.Count == 2 && x.Digit == '*');
            int sum = 0;
            foreach (var gear in gears)
            {
                sum += gear.AdjacentParts.First().Number * gear.AdjacentParts.Last().Number;
            }
            return sum;
        }

        private int SumOfAllParts(State state)
        {
            var parts = state.AllParts.Where(x => x.HasAdjacentSymbol);
            return parts
                .Select(x => x.Number)
                .Sum();
        }

        private void SetHasAdjacent(State state)
        {
            foreach (var symbol in state.AllSymbols)
            {
                symbol.AdjacentParts = new HashSet<PartNumber>();
                var part = GetPart(state, symbol.X - 1, symbol.Y);
                if (part != null)
                {
                    part.HasAdjacentSymbol = true;
                    symbol.AdjacentParts.Add(part);
                }
                part = GetPart(state, symbol.X + 1, symbol.Y);
                if (part != null)
                {
                    part.HasAdjacentSymbol = true;
                    symbol.AdjacentParts.Add(part);
                }
                part = GetPart(state, symbol.X, symbol.Y - 1);
                if (part != null)
                {
                    part.HasAdjacentSymbol = true;
                    symbol.AdjacentParts.Add(part);
                }
                part = GetPart(state, symbol.X, symbol.Y + 1);
                if (part != null)
                {
                    part.HasAdjacentSymbol = true;
                    symbol.AdjacentParts.Add(part);
                }
                part = GetPart(state, symbol.X - 1, symbol.Y - 1);
                if (part != null)
                {
                    part.HasAdjacentSymbol = true;
                    symbol.AdjacentParts.Add(part);
                }
                part = GetPart(state, symbol.X + 1, symbol.Y - 1);
                if (part != null)
                {
                    part.HasAdjacentSymbol = true;
                    symbol.AdjacentParts.Add(part);
                }
                part = GetPart(state, symbol.X - 1, symbol.Y + 1);
                if (part != null)
                {
                    part.HasAdjacentSymbol = true;
                    symbol.AdjacentParts.Add(part);
                }
                part = GetPart(state, symbol.X + 1, symbol.Y + 1);
                if (part != null)
                {
                    part.HasAdjacentSymbol = true;
                    symbol.AdjacentParts.Add(part);
                }
            }
        }

        private PartNumber GetPart(State state, int x, int y)
        {
            if (state.Parts.ContainsKey(x) && state.Parts[x].ContainsKey(y))
                return state.Parts[x][y];
            return null;
        }

        private State GetState(List<string> input)
        {
            var state = new State()
            {
                AllParts = new List<PartNumber>(),
                AllSymbols = new List<Symbol>(),
                Parts = new Dictionary<int, Dictionary<int, PartNumber>>(),
                Symbols = new Dictionary<int, Dictionary<int, Symbol>>()
            };
            int y = 0;
            foreach (var line in input)
            {
                PartNumber currentNumber = null;
                int x = 0;
                foreach (var digit in line)
                {
                    if (digit == '.')
                    {
                        currentNumber = null;
                    } else 
                    {
                        int number = 0;
                        bool isNumber = TryNumber(digit, out number);
                        if (isNumber)
                        {
                            currentNumber = AddNumber(state, x, y, digit, currentNumber);
                        }
                        else
                        {
                            currentNumber = null;
                            AddSymbol(state, x, y, digit);
                        }
                    }
                    x++;
                }
                y++;
            }
            return state;
        }

        private PartNumber AddNumber(State state, int x, int y, char digit, PartNumber current)
        {
            int number = Convert.ToInt32(new string(new char[1] { digit }));
            if (current == null)
            {
                current = new PartNumber()
                {
                    Number = number,
                    X = x,
                    Y = y
                };
                state.AllParts.Add(current);
            }
            else
            {
                current.Number = current.Number * 10 + number;
            }
            if (!state.Parts.ContainsKey(x))
                state.Parts.Add(x, new Dictionary<int, PartNumber>());
            state.Parts[x][y] = current;
            return current;
        }

        private void AddSymbol(State state, int x, int y, char digit)
        {
            if (!state.Symbols.ContainsKey(x))
                state.Symbols.Add(x, new Dictionary<int, Symbol>());
            var newSymbol = new Symbol()
            {
                Digit = digit,
                X = x,
                Y = y
            };
            state.Symbols[x][y] = newSymbol;
            state.AllSymbols.Add(newSymbol);
        }

        private bool TryNumber(char digit, out int number)
        {
            switch (digit)
            {
                case '0':
                    number = 0;
                    return true;
                case '1':
                    number = 1;
                    return true;
                case '2':
                    number = 2;
                    return true;
                case '3':
                    number = 3;
                    return true;
                case '4':
                    number = 4;
                    return true;
                case '5':
                    number = 5;
                    return true;
                case '6':
                    number = 6;
                    return true;
                case '7':
                    number = 7;
                    return true;
                case '8':
                    number = 8;
                    return true;
                case '9':
                    number = 9;
                    return true;
            }
            number = -1;
            return false;
        }

        private class PartNumber
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Number { get; set; }
            public bool HasAdjacentSymbol { get; set; }
        }

        private class Symbol
        {
            public int X { get; set; }
            public int Y { get; set; }
            public char Digit { get; set; }
            public HashSet<PartNumber> AdjacentParts { get; set; }
        }

        private class State
        {
            public Dictionary<int, Dictionary<int, PartNumber>> Parts { get; set; }
            public Dictionary<int, Dictionary<int, Symbol>> Symbols { get; set; }
            public List<PartNumber> AllParts { get; set; }
            public List<Symbol> AllSymbols { get; set; }
        }
    }
}
