using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem18 : AdventOfCodeBase {
        private enum enumInstructionType {
            snd,
            set,
            add,
            mul,
            mod,
            rcv,
            jgz
        }

        private Dictionary<string, long>[] _registers;
        private List<Instruction> _instructions;
        private int[] _index;
        private long _lastSoundPlayed;
        private long _lastRecovery;
        private int _currentProgram;
        private List<long>[] _queues;
        private bool _isFinished;
        private bool[] _isWaiting;
        private int _sendCount;

        public override string ProblemName {
            get { return "Advent of Code 2017: 18"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private long Answer1(List<string> input) {
            GetInstructions(input);
            Initialize(false);
            Perform(false);
            return _lastRecovery;
        }

        private int Answer2(List<string> input) {
            GetInstructions(input);
            Initialize(true);
            Perform(true);
            return _sendCount;
        }

        private void Initialize(bool useNew) {
            _currentProgram = 0;
            _isWaiting = new bool[2];
            _index = new int[2];
            _registers = new Dictionary<string, long>[2];
            _registers[0] = new Dictionary<string, long>();
            _registers[1] = new Dictionary<string, long>();
            foreach (var instruction in _instructions) {
                if (instruction.IsValueRegister1 && !_registers[0].ContainsKey(instruction.Register1)) {
                    _registers[0].Add(instruction.Register1, 0);
                    _registers[1].Add(instruction.Register1, 0);
                }
                if (instruction.IsValueRegister2 && !_registers[0].ContainsKey(instruction.Register2)) {
                    _registers[0].Add(instruction.Register2, 0);
                    _registers[1].Add(instruction.Register2, 0);
                }
            }
            _queues = new List<long>[2];
            _queues[0] = new List<long>();
            _queues[1] = new List<long>();
            if (useNew) {
                _registers[1]["p"] = 1;
            }
        }

        private void Perform(bool useNew) {
            do {
                var instruction = _instructions[_index[_currentProgram]];
                switch (instruction.InstructionType) {
                    case enumInstructionType.snd:
                        if (!useNew) {
                            PerformSND(instruction);
                        } else {
                            PerformSNDNew(instruction);
                        }
                        break;
                    case enumInstructionType.add:
                        PerformADD(instruction);
                        break;
                    case enumInstructionType.jgz:
                        PerformJGZ(instruction);
                        break;
                    case enumInstructionType.mod:
                        PerformMOD(instruction);
                        break;
                    case enumInstructionType.mul:
                        PerformMUL(instruction);
                        break;
                    case enumInstructionType.rcv:
                        if (!useNew) {
                            PerformRCV(instruction);
                        } else {
                            PerformRCVNew(instruction);
                        }
                        break;
                    case enumInstructionType.set:
                        PerformSET(instruction);
                        break;
                }
            } while (!_isFinished);
        }

        private void PerformSND(Instruction instruction) {
            _lastSoundPlayed = (instruction.IsValueRegister1 ? _registers[_currentProgram][instruction.Register1] : instruction.Value1);
            _index[_currentProgram]++;
        }

        private void PerformSNDNew(Instruction instruction) {
            var value = (instruction.IsValueRegister1 ? _registers[_currentProgram][instruction.Register1] : instruction.Value1);
            int nextProgram = (_currentProgram + 1) % 2;
            _queues[nextProgram].Add(value);
            _index[_currentProgram]++;
            if (_currentProgram == 1) {
                _sendCount++;
            }
            _isWaiting[nextProgram] = false;
        }

        private void PerformSET(Instruction instruction) {
            _registers[_currentProgram][instruction.Register1] = (instruction.IsValueRegister2 ? _registers[_currentProgram][instruction.Register2] : instruction.Value2);
            _index[_currentProgram]++;
        }

        private void PerformADD(Instruction instruction) {
            var value = (instruction.IsValueRegister2 ? _registers[_currentProgram][instruction.Register2] : instruction.Value2);
            _registers[_currentProgram][instruction.Register1] += value;
            _index[_currentProgram]++;
        }

        private void PerformMUL(Instruction instruction) {
            var value = (instruction.IsValueRegister2 ? _registers[_currentProgram][instruction.Register2] : instruction.Value2);
            _registers[_currentProgram][instruction.Register1] *= value;
            _index[_currentProgram]++;
        }

        private void PerformMOD(Instruction instruction) {
            var value = (instruction.IsValueRegister2 ? _registers[_currentProgram][instruction.Register2] : instruction.Value2);
            _registers[_currentProgram][instruction.Register1] %= value;
            _index[_currentProgram]++;
        }

        private void PerformRCV(Instruction instruction) {
            var value = (instruction.IsValueRegister1 ? _registers[_currentProgram][instruction.Register1] : instruction.Value1);
            if (value != 0) {
                _lastRecovery = _lastSoundPlayed;
                if (_lastRecovery != 0) {
                    _isFinished = true;
                }
            }
            _index[_currentProgram]++;
        }

        private void PerformRCVNew(Instruction instruction) {
            if (_queues[_currentProgram].Count == 0) {
                _isWaiting[_currentProgram] = true;
                if (_isWaiting[0] && _isWaiting[1]) {
                    _isFinished = true;
                } else {
                    _currentProgram = (_currentProgram + 1) % 2;
                }
            } else {
                _registers[_currentProgram][instruction.Register1] = _queues[_currentProgram][0];
                _queues[_currentProgram].RemoveAt(0);
                _index[_currentProgram]++;
            }
        }

        private void PerformJGZ(Instruction instruction) {
            var x = (instruction.IsValueRegister1 ? _registers[_currentProgram][instruction.Register1] : instruction.Value1);
            var y = (instruction.IsValueRegister2 ? _registers[_currentProgram][instruction.Register2] : instruction.Value2);
            if (x > 0) {
                _index[_currentProgram] += (int)y;
            } else {
                _index[_currentProgram]++;
            }
        }

        private void GetInstructions(List<string> input) {
            _instructions = input.Select(line => {
                var instruction = new Instruction();
                var split = line.Split(' ');
                switch (split[0]) {
                    case "snd":
                        instruction.InstructionType = enumInstructionType.snd;
                        break;
                    case "set":
                        instruction.InstructionType = enumInstructionType.set;
                        break;
                    case "add":
                        instruction.InstructionType = enumInstructionType.add;
                        break;
                    case "mul":
                        instruction.InstructionType = enumInstructionType.mul;
                        break;
                    case "mod":
                        instruction.InstructionType = enumInstructionType.mod;
                        break;
                    case "rcv":
                        instruction.InstructionType = enumInstructionType.rcv;
                        break;
                    case "jgz":
                        instruction.InstructionType = enumInstructionType.jgz;
                        break;
                }
                int value = 0;
                bool isNum = int.TryParse(split[1], out value);
                if (isNum) {
                    instruction.Value1 = value;
                } else {
                    instruction.Register1 = split[1];
                    instruction.IsValueRegister1 = true;
                }
                if (split.Length == 3) {
                    isNum = int.TryParse(split[2], out value);
                    if (isNum) {
                        instruction.Value2 = value;
                    } else {
                        instruction.Register2 = split[2];
                        instruction.IsValueRegister2 = true;
                    }
                }
                return instruction;
            }).ToList();
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public long Value1 { get; set; }
            public long Value2 { get; set; }
            public string Register1 { get; set; }
            public string Register2 { get; set; }
            public bool IsValueRegister1 { get; set; }
            public bool IsValueRegister2 { get; set; }
        }
    }
}
