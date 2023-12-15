using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem15 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 15";

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
            return GetHashAll(input[0]);
        }

        private long Answer2(List<string> input)
        {
            var state = GetState();
            PerformAll(input, state);
            return FocalPower(state);
        }

        private long FocalPower(State state)
        {
            long sum = 0;
            foreach (var box in state.Boxes.Values)
            {
                sum += FocalPower(box);
            }
            return sum;
        }

        private long FocalPower(Box box)
        {
            long sum = 0;
            long number = 1;
            foreach (var lense in box.Lenses)
            {
                var a = (long)box.Number + 1;
                var b = number;
                var c = (long)lense.Strength;
                var result = a * b * c;
                sum += result;
                number++;
            }
            return sum;
        }

        private void PerformAll(List<string> instructions, State state)
        {
            var split = instructions[0].Split(',');
            foreach (var instruction in split)
            {
                Perform(instruction, state);
            }
        }

        private void Perform(string text, State state)
        {
            var op = text[text.Length - 1];
            if (op == '-')
            {
                PerformSubtract(text, state);
            }
            else
            {
                PerformAdd(text, state);
            }
        }

        private void PerformAdd(string text, State state)
        {
            var label = text.Substring(0, text.Length - 2);
            var hash = GetHash(label);
            var box = state.Boxes[hash];
            var strength = Convert.ToInt32(text.Substring(text.Length - 1));
            bool didAdd = false;
            foreach (var next in box.Lenses)
            {
                if (next.Label == label)
                {
                    next.Strength = strength;
                    didAdd = true;
                    break;
                }
            }
            if (!didAdd)
                box.Lenses.AddLast(new Lense()
                {
                    Label = label,
                    Strength = strength
                });
        }

        private void PerformSubtract(string text, State state)
        {
            var label = text.Substring(0, text.Length - 1);
            var hash = GetHash(label);
            var box = state.Boxes[hash];
            var lense = box.Lenses.Where(x => x.Label == label).FirstOrDefault();
            if (lense != null)
            {
                box.Lenses.Remove(lense);
            }
        }

        private State GetState()
        {
            var boxes = new Dictionary<int, Box>();
            for (int number = 0; number < 256; number++)
            {
                var box = new Box()
                {
                    Number = number,
                    Lenses = new LinkedList<Lense>()
                };
                boxes.Add(number, box);
            }
            return new State()
            {
                Boxes = boxes
            };
        }

        private int GetHashAll(string line)
        {
            int sum = 0;
            var split = line.Split(',');
            foreach (var hash in split)
            {
                var next = GetHash(hash);
                sum += next;
            }
            return sum;
        }

        private int GetHash(string text)
        {
            int hash = 0;
            foreach (var digit in text)
            {
                hash += (int)digit;
                hash *= 17;
                hash %= 256;
            }
            return hash;
        }

        private class State
        {
            public Dictionary<int, Box> Boxes { get; set; }
        }

        private class Box
        {
            public int Number { get; set; }
            public LinkedList<Lense> Lenses { get; set; }
        }

        private class Lense
        {
            public int Strength { get; set; }
            public string Label { get; set; }
        }
    }
}
