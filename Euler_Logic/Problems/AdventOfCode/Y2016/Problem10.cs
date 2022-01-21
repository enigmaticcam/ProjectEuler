using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem10 : AdventOfCodeBase {
        private List<Instruction> _instructions;
        private Dictionary<int, Receiver> _outputs;
        private Dictionary<int, Receiver> _bots;

        private enum enumInstructionType {
            InitialState,
            GiveToReceiver
        }

        private enum enumReceiverType {
            Bot,
            Output
        }

        public override string ProblemName => "Advent of Code 2016: 10";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            _bots = new Dictionary<int, Receiver>();
            _outputs = new Dictionary<int, Receiver>();
            GetInstructions(input);
            SetInitialInputs();
            return LookForExchange(61, 17);
        }

        private int Answer2(List<string> input) {
            _bots = new Dictionary<int, Receiver>();
            _outputs = new Dictionary<int, Receiver>();
            GetInstructions(input);
            SetInitialInputs();
            PerformAllExchanges();
            return _outputs[0].Holding[0] * _outputs[1].Holding[0] * _outputs[2].Holding[0];
        }

        private void PerformAllExchanges() {
            do {
                var next = _bots.Values.Where(bot => bot.HoldIndex == 2);
                if (next.Count() == 0) {
                    break;
                }
                PerformBotInstruction(next.First());
            } while (true);
        }

        private int LookForExchange(int value1, int value2) {
            do {
                var next = _bots.Values.Where(bot => bot.HoldIndex == 2).First();
                if ((next.Holding[0] == value1 && next.Holding[1] == value2) || (next.Holding[0] == value2 && next.Holding[1] == value1)) {
                    return next.Id;
                }
                PerformBotInstruction(next);
            } while (true);
        }

        private void PerformBotInstruction(Receiver bot) {
            int low = bot.Holding[0];
            int high = bot.Holding[1];
            if (low > high) {
                low = bot.Holding[1];
                high = bot.Holding[0];
            }
            bot.HoldIndex = 0;
            if (bot.Instruct.IsLowOutput) {
                var output = GetOuput(bot.Instruct.Value2);
                output.Holding[0] = low;
            } else {
                var toBot = GetBot(bot.Instruct.Value2);
                toBot.Holding[toBot.HoldIndex] = low;
                toBot.HoldIndex++;
            }
            if (bot.Instruct.IsHighOutput) {
                var output = GetOuput(bot.Instruct.Value3);
                output.Holding[0] = high;
            } else {
                var toBot = GetBot(bot.Instruct.Value3);
                toBot.Holding[toBot.HoldIndex] = high;
                toBot.HoldIndex++;
            }
        }

        private void SetInitialInputs() {
            for (int index = 0; index < _instructions.Count; index++) {
                var instruction = _instructions[index];
                if (instruction.InstructionType == enumInstructionType.InitialState) {
                    var bot = GetBot(instruction.Value2);
                    bot.Holding[bot.HoldIndex] = instruction.Value1;
                    bot.HoldIndex++;
                    _instructions.RemoveAt(index);
                    index--;
                } else {
                    var bot = GetBot(instruction.Value1);
                    bot.Instruct = instruction;
                }
            }
        }

        private Receiver GetBot(int id) {
            if (!_bots.ContainsKey(id)) {
                _bots.Add(id, new Receiver() {
                    Id = id,
                    ReceiverType = enumReceiverType.Bot,
                    Holding = new int[2]
                });
            }
            return _bots[id];
        }

        private Receiver GetOuput(int id) {
            if (!_outputs.ContainsKey(id)) {
                _outputs.Add(id, new Receiver() {
                    Id = id,
                    ReceiverType = enumReceiverType.Output,
                    Holding = new int[1]
                }); ;
            }
            return _outputs[id];
        }

        private void GetInstructions(List<string> input) {
            _instructions = input.Select(line => {
                var instruction = new Instruction();
                var split = line.Split(' ');
                if (split.Length == 6) {
                    instruction.InstructionType = enumInstructionType.InitialState;
                    instruction.Value1 = Convert.ToInt32(split[1]);
                    instruction.Value2 = Convert.ToInt32(split[5]);
                } else {
                    instruction.InstructionType = enumInstructionType.GiveToReceiver;
                    instruction.Value1 = Convert.ToInt32(split[1]);
                    instruction.Value2 = Convert.ToInt32(split[6]);
                    instruction.Value3 = Convert.ToInt32(split[11]);
                    if (split[5] == "output") {
                        instruction.IsLowOutput = true;
                    }
                    if (split[10] == "output") {
                        instruction.IsHighOutput = true;
                    }
                }
                return instruction;
            }).ToList();
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public int Value1 { get; set; }
            public int Value2 { get; set; }
            public int Value3 { get; set; }
            public bool IsLowOutput { get; set; }
            public bool IsHighOutput { get; set; }
        }

        private class Receiver {
            public int Id { get; set; }
            public enumReceiverType ReceiverType { get; set; }
            public int[] Holding { get; set; }
            public Instruction Instruct { get; set; }
            public int HoldIndex { get; set; }
        }
    }
}
