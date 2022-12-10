using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem10 : AdventOfCodeBase {
        private State _state;

        public override string ProblemName => "Advent of Code 2022: 10";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private long Answer1(List<string> input) {
            var instructions = GetInstructions(input);
            var strength = Perform(instructions);
            return strength;
        }

        private string Answer2(List<string> input) {
            var instructions = GetInstructions(input);
            Perform(instructions);
            //Draw();
            return "zuprfecl";
        }

        private string Draw() {
            var text = new StringBuilder();
            text.AppendLine(new string(_state.Display.Take(40).ToArray()));
            text.AppendLine(new string(_state.Display.Skip(40).Take(40).ToArray()));
            text.AppendLine(new string(_state.Display.Skip(80).Take(40).ToArray()));
            text.AppendLine(new string(_state.Display.Skip(120).Take(40).ToArray()));
            text.AppendLine(new string(_state.Display.Skip(160).Take(40).ToArray()));
            text.AppendLine(new string(_state.Display.Skip(200).Take(40).ToArray()));
            return text.ToString();
        }

        private long Perform(List<Instruction> instructions) {
            long strength = 0;
            _state = new State() { Value = 1, Cycle = 1, Display = new List<char>() };
            var action = new InstructAction();
            foreach (var instruction in instructions) {
                if (instruction.InstructionType == enumInstructionType.addx) {
                    Perform_addx(instruction, action);
                } else {
                    Perform_noop(action);
                }
                while (action.Wait > 0) {
                    if (_state.Cycle == 20 || ((_state.Cycle - 20) % 40) == 0) {
                        strength += _state.Cycle * _state.Value;
                    }
                    DrawPixel(_state);
                    _state.Cycle++;
                    action.Wait--;
                }
                action.Perform(_state);
            }
            return strength;
        }

        private void DrawPixel(State state) {
            var cycle = state.Cycle;
            while (cycle > 40) {
                cycle -= 40;
            }
            cycle--;
            if (Math.Abs(cycle - state.Value) <= 1) {
                state.Display.Add('#');
            } else {
                state.Display.Add('.');
            }
        }

        private void Perform_noop(InstructAction action) {
            action.Perform = state => { };
            action.Wait = 1;
        }

        private void Perform_addx(Instruction instruction, InstructAction action) {
            action.Perform = state => state.Value += instruction.Value;
            action.Wait = 2;
        }

        private List<Instruction> GetInstructions(List<string> input) {
            return input.Select(line => {
                var split = line.Split(' ');
                if (split[0] == "noop") {
                    return new Instruction() { InstructionType = enumInstructionType.noop };
                } else {
                    return new Instruction() {
                        InstructionType = enumInstructionType.addx,
                        Value = Convert.ToInt32(split[1])
                    };
                }
            }).ToList();
        }

        private enum enumInstructionType {
            addx,
            noop
        }

        private class InstructAction {
            public Action<State> Perform { get; set; }
            public int Wait { get; set; }
        }

        private class State {
            public long Value { get; set; }
            public long Cycle { get; set; }
            public List<char> Display { get; set; }
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public int Value { get; set; }
        }
    }
}
