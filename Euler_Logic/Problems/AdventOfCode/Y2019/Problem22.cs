using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem22 : AdventOfCodeBase {
        private List<Instruction> _instructions;

        public enum enumInstructionType {
            DealNewStack,
            CutCards,
            DealWithIncrement
        }

        public override string ProblemName {
            get { return "Advent of Code 2019: 22"; }
        }

        public override string GetAnswer() {
            //GetInstructions(Input_Test(3));
            //return Answer2(10).ToString();
            GetInstructions(Input());
            return Answer2(119315717514047).ToString();
        }

        private int Answer1(int maxValue) {
            var cards = Enumerable.Range(0, maxValue).ToArray();
            foreach (var instruction in _instructions) {
                switch (instruction.InstructionType) {
                    case enumInstructionType.CutCards:
                        cards = CutCards(cards, (int)instruction.Value);
                        break;
                    case enumInstructionType.DealNewStack:
                        cards = DealNewStack(cards);
                        break;
                    case enumInstructionType.DealWithIncrement:
                        cards = DealWithIncrement(cards, (int)instruction.Value);
                        break;
                }
            }
            return IndexOf(2019, cards);
        }

        private long Answer2(long maxValue) {
            Coalesce(maxValue);
            return 0;
        }

        private long _a;
        private long _b;
        private void Coalesce(long maxValue) {
            _a = 1;
            for (int index = _instructions.Count - 1; index >= 0; index--) {
                var instruction = _instructions[index];
                switch (instruction.InstructionType) {
                    case enumInstructionType.CutCards:
                        _b = (_b + instruction.Value) % maxValue;
                        break;
                    case enumInstructionType.DealNewStack:
                        _a *= -1;
                        _b = maxValue - _b - 1;
                        break;
                    case enumInstructionType.DealWithIncrement:
                        var z = Power.Exp(instruction.Value, maxValue - 2, maxValue);
                        _a = (_a * z) % maxValue;
                        _b = (_b * z) % maxValue;
                        break;
                }
            }
        }

        private long FindResultIndex(long maxValue, long index) {
            foreach (var instruction in _instructions) {
                switch (instruction.InstructionType) {
                    case enumInstructionType.CutCards:
                        index = (index - instruction.Value) % maxValue;
                        if (index < 0) {
                            index += maxValue;
                        }
                        break;
                    case enumInstructionType.DealNewStack:
                        index = maxValue - index - 1;
                        break;
                    case enumInstructionType.DealWithIncrement:
                        index = (index * instruction.Value) % maxValue;
                        break;
                }
            }
            return index;
        }

        private int IndexOf(int num, int[] cards) {
            for (int index = 0; index < cards.Length; index++) {
                if (cards[index] == num) {
                    return index;
                }
            }
            return 0;
        }

        private int[] DealWithIncrement(int[] cards, int value) {
            var newCards = new int[cards.Length];
            int index = 0;
            int newIndex = 0;
            for (int count = 1; count <= cards.Length; count++) {
                newCards[index] = cards[newIndex];
                index = (index + value) % cards.Length;
                newIndex++;
            }
            return newCards;
        }

        private int[] DealNewStack(int[] cards) {
            var newCards = new int[cards.Length];
            for (int index = 0; index < cards.Length; index++) {
                newCards[index] = cards[cards.Length - index - 1];
            }
            return newCards;
        }

        private int[] CutCards(int[] cards, int value) {
            var newCards = new int[cards.Length];
            int count = 0;
            int start = value;
            if (value < 0) {
                start = cards.Length + value;
            }
            for (int index = start; index < cards.Length; index++) {
                newCards[count] = cards[index];
                count++;
            }
            for (int index = 0; index < start; index++) {
                newCards[count] = cards[index];
                count++;
            }
            return newCards;
        }

        private void GetInstructions(List<string> input) {
            _instructions = input.Select(x => {
                var split = x.Split(' ');
                var instruction = new Instruction();
                if (split[0] == "cut") {
                    instruction.InstructionType = enumInstructionType.CutCards;
                    instruction.Value = Convert.ToInt32(split[1]);
                } else if (split[1] == "with") {
                    instruction.InstructionType = enumInstructionType.DealWithIncrement;
                    instruction.Value = Convert.ToInt32(split[3]);
                } else {
                    instruction.InstructionType = enumInstructionType.DealNewStack;
                }
                return instruction;
            }).ToList();
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public long Value { get; set; }
        }
    }
}
