using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem19 : AdventOfCodeBase {
        private List<Instruction> _instructions;
        private ulong[] _registers;
        private int _indexRegister;

        private enum enumInstructionType {
            addr,
            addi,
            mulr,
            muli,
            banr,
            bani,
            borr,
            bori,
            setr,
            seti,
            gtir,
            gtri,
            gtrr,
            eqir,
            eqri,
            eqrr
        }

        public override string ProblemName {
            get { return "Advent of Code 2018: 19"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            _registers = new ulong[6];
            GetInstructions(input);
            Perform();
            return _registers[0];
        }

        private ulong Answer2(List<string> input) {
            _registers = new ulong[6];
            _registers[0] = 1;
            GetInstructions(input);
            Perform();
            return _registers[0];
        }

        private void Perform() {
            do {
                if (_registers[_indexRegister] == 3) {
                    if (_registers[1] % _registers[5] == 0) {
                        _registers[0] += _registers[5];
                    }
                    _registers[3] = _registers[1] + 1;
                    _registers[_indexRegister] = 12;
                    _registers[4] = 1;
                }
                var instruction = _instructions[(int)_registers[_indexRegister]];
                switch (instruction.InstructionType) {
                    case enumInstructionType.addi:
                        PerformAddi(instruction);
                        break;
                    case enumInstructionType.addr:
                        PerformAddr(instruction);
                        break;
                    case enumInstructionType.bani:
                        PerformBani(instruction);
                        break;
                    case enumInstructionType.banr:
                        PerformBanr(instruction);
                        break;
                    case enumInstructionType.bori:
                        PerformBori(instruction);
                        break;
                    case enumInstructionType.borr:
                        PerformBorr(instruction);
                        break;
                    case enumInstructionType.eqir:
                        PerformEqir(instruction);
                        break;
                    case enumInstructionType.eqri:
                        PerformEqri(instruction);
                        break;
                    case enumInstructionType.eqrr:
                        PerformEqrr(instruction);
                        break;
                    case enumInstructionType.gtir:
                        PerformGtir(instruction);
                        break;
                    case enumInstructionType.gtri:
                        PerformGtri(instruction);
                        break;
                    case enumInstructionType.gtrr:
                        PerformGtrr(instruction);
                        break;
                    case enumInstructionType.muli:
                        PerformMuli(instruction);
                        break;
                    case enumInstructionType.mulr:
                        PerformMulr(instruction);
                        break;
                    case enumInstructionType.seti:
                        PerformSeti(instruction);
                        break;
                    case enumInstructionType.setr:
                        PerformSetr(instruction);
                        break;
                }
                _registers[_indexRegister]++;
            } while (_registers[_indexRegister] < (ulong)_instructions.Count);
        }

        private void PerformAddr(Instruction instruction) {
            _registers[instruction.ValueC] = _registers[instruction.ValueA] + _registers[instruction.ValueB];
        }

        private void PerformAddi(Instruction instruction) {
            _registers[instruction.ValueC] = _registers[instruction.ValueA] + instruction.ValueB;
        }

        private void PerformMulr(Instruction instruction) {
            _registers[instruction.ValueC] = _registers[instruction.ValueA] * _registers[instruction.ValueB];
        }

        private void PerformMuli(Instruction instruction) {
            _registers[instruction.ValueC] = _registers[instruction.ValueA] * instruction.ValueB;
        }

        private void PerformBanr(Instruction instruction) {
            _registers[instruction.ValueC] = _registers[instruction.ValueA] & _registers[instruction.ValueB];
        }

        private void PerformBani(Instruction instruction) {
            _registers[instruction.ValueC] = _registers[instruction.ValueA] & instruction.ValueB;
        }

        private void PerformBorr(Instruction instruction) {
            _registers[instruction.ValueC] = _registers[instruction.ValueA] | _registers[instruction.ValueB];
        }

        private void PerformBori(Instruction instruction) {
            _registers[instruction.ValueC] = _registers[instruction.ValueA] | instruction.ValueB;
        }

        private void PerformSetr(Instruction instruction) {
            _registers[instruction.ValueC] = _registers[instruction.ValueA];
        }

        private void PerformSeti(Instruction instruction) {
            _registers[instruction.ValueC] = instruction.ValueA;
        }

        private void PerformGtir(Instruction instruction) {
            _registers[instruction.ValueC] = (instruction.ValueA > _registers[instruction.ValueB] ? (ulong)1 : 0);
        }

        private void PerformGtri(Instruction instruction) {
            _registers[instruction.ValueC] = (_registers[instruction.ValueA] > instruction.ValueB ? (ulong)1 : 0);
        }

        private void PerformGtrr(Instruction instruction) {
            _registers[instruction.ValueC] = (_registers[instruction.ValueA] > _registers[instruction.ValueB] ? (ulong)1 : 0);
        }

        private void PerformEqir(Instruction instruction) {
            _registers[instruction.ValueC] = (instruction.ValueA == _registers[instruction.ValueB] ? (ulong)1 : 0);
        }

        private void PerformEqri(Instruction instruction) {
            _registers[instruction.ValueC] = (_registers[instruction.ValueA] == instruction.ValueB ? (ulong)1 : 0);
        }

        private void PerformEqrr(Instruction instruction) {
            _registers[instruction.ValueC] = (_registers[instruction.ValueA] == _registers[instruction.ValueB] ? (ulong)1 : 0);
        }

        private void GetInstructions(List<string> input) {
            _indexRegister = Convert.ToInt32(input[0].Substring(4, 1));
            _instructions = new List<Instruction>();
            for (int index = 1; index < input.Count; index++) {
                var instruction = new Instruction();
                var line = input[index];
                var split = line.Split(' ');
                instruction.InstructionType = (enumInstructionType)Enum.Parse(typeof(enumInstructionType), split[0]);
                instruction.ValueA = Convert.ToUInt64(split[1]);
                instruction.ValueB = Convert.ToUInt64(split[2]);
                instruction.ValueC = Convert.ToUInt64(split[3]);
                _instructions.Add(instruction);
            }
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public ulong ValueA { get; set; }
            public ulong ValueB { get; set; }
            public ulong ValueC { get; set; }
        }
    }
}
