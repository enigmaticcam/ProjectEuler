using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem05 : AdventOfCodeBase {
        private Dictionary<int, LinkedList<char>> _crates;
        private List<Instruction> _instructions;

        public override string ProblemName => "Advent of Code 2022: 5";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private string Answer1(List<string> input) {
            GetInput(input);
            Perform(false);
            return GetTop();
        }

        private string Answer2(List<string> input) {
            GetInput(input);
            Perform(true);
            return GetTop();
        }

        private string GetTop() {
            var top = new List<char>();
            foreach (var crate in _crates.OrderBy(x => x.Key)) {
                top.Add(crate.Value.First.Value);
            }
            return new string(top.ToArray());
        }

        private void Perform(bool multiple) {
            foreach (var instruction in _instructions) {
                var from = _crates[instruction.From];
                var to = _crates[instruction.To];
                if (!multiple) {
                    Move_Single(from, to, instruction);
                } else {
                    Move_Mulitple(from, to, instruction);
                }
            }
        }

        private void Move_Mulitple(LinkedList<char> from, LinkedList<char> to, Instruction instruction) {
            LinkedListNode<char> last = null;
            for (int count = 1; count <= instruction.Count; count++) {
                var digit = from.First.Value;
                from.RemoveFirst();
                if (last == null) {
                    to.AddFirst(digit);
                    last = to.First;
                } else {
                    to.AddAfter(last, digit);
                    last = last.Next;
                }
            }
        }

        private void Move_Single(LinkedList<char> from, LinkedList<char> to, Instruction instruction) {
            for (int count = 1; count <= instruction.Count; count++) {
                var digit = from.First.Value;
                from.RemoveFirst();
                to.AddFirst(digit);
            }
        }

        private void GetInput(List<string> input) {
            int index = GetInput_Crates(input);
            GetInput_Instructions(input, index);
        }

        private int GetInput_Crates(List<string> input) {
            int lineIndex = 0;
            _crates = new Dictionary<int, LinkedList<char>>();
            do {
                var line = input[lineIndex];
                if (line.Substring(0, 2) == " 1") break;
                int charIndex = line.IndexOf('[');
                do {
                    int column = charIndex / 4 + 1;
                    if (!_crates.ContainsKey(column)) _crates.Add(column, new LinkedList<char>());
                    _crates[column].AddLast(line.Substring(charIndex + 1, 1)[0]);
                    charIndex = line.IndexOf('[', charIndex + 1);
                } while (charIndex != -1);
                lineIndex++;
            } while (true);
            return lineIndex + 2;
        }

        private void GetInput_Instructions(List<string> input, int indexStart) {
            _instructions = new List<Instruction>();
            for (int index = indexStart; index < input.Count; index++) {
                var line = input[index];
                var split = line.Split(' ');
                _instructions.Add(new Instruction() {
                    Count = Convert.ToInt32(split[1]),
                    From = Convert.ToInt32(split[3]),
                    To = Convert.ToInt32(split[5])
                });
            }
        }

        private class Instruction {
            public int Count { get; set; }
            public int From { get; set; }
            public int To { get; set; }
        }
    }
}
